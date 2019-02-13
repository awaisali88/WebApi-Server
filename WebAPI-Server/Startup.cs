using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Common;
using Common.Exception;
using Common.Messages;
using ElmahCore.Mvc;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WebAPI_Server.AppStart;
using WebAPI_Server.Middleware;

namespace WebAPI_Server
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        internal IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment { get;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
            });

            #region Configure AutoMapper
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.ConfigureDependencyServices(Configuration, CurrentEnvironment);
            services.ConfigureDatabase(Configuration, CurrentEnvironment);
            services.ConfigureJwt(Configuration, CurrentEnvironment);
            services.ConfigureElmah(Configuration, CurrentEnvironment);

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            #region Configure MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());

                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                //options.Filters.AddService(typeof(AngularAntiforgeryCookieResultFilter));

                //options.Filters.Add(new CustomExceptionFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.RegisterValidatorsFromAssembly(Assembly.Load(nameof(WebAPI_ViewModel).ToString().Replace("_","-")));
                });

            //services.AddAntiforgery(delegate (AntiforgeryOptions x)
            //{
            //    x.HeaderName = HttpRequestHeaders.AntiForgeryTokenHeader;
            //    x.SuppressXFrameOptionsHeader = false;
            //});
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Name = "auth_cookie";

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(12);

                options.LoginPath = "/";
                options.AccessDeniedPath = "/";
                options.SlidingExpiration = true;
            });
            //services.ConfigureApplicationCookie(cookieOptions =>
            //{
            //    cookieOptions.Cookie.SameSite = SameSiteMode.None;
            //    cookieOptions.Cookie.Name = "auth_cookie";

            //    cookieOptions.Events = new CookieAuthenticationEvents
            //    {
            //        OnRedirectToLogin = redirectContext =>
            //        {
            //            redirectContext.HttpContext.Response.StatusCode = 401;
            //            return Task.CompletedTask;
            //        }
            //    };
            //});
            #endregion

            services.AddSession(config =>
                config.IdleTimeout = TimeSpan.FromMinutes(1)
            );
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });


            #region Api Behavior Optons ([ApiController] Attribute)
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    if (!OpenUrls.WebUrls.Any(x => actionContext.HttpContext.Request.Path.StartsWithSegments(x)))
                    {
                        var errors = actionContext.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .Select(e => new ErrorsModelException
                            {
                                Code = e.Key,
                                Description = e.Value.Errors.First().ErrorMessage
                            }).ToList();

                        throw new ModelValidationException(ErrorMessages.ModelValidationFailed, errors);
                    }

                    return new RedirectToActionResult("Login", "Home", null);
                };
            });
            #endregion

            services.ConfigureCors();

            services.ConfigureSwagger();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseHsts();
            }

            #region Anti Forgery Config
            //app.Use(next => context =>
            //{
            //    string path = context.Request.Path.Value;

            //    List<bool> checkOpenUrls = new List<bool>();
            //    foreach (var url in OpenUrls.Urls)
            //    {
            //        checkOpenUrls.Add(string.Equals(path, url, StringComparison.OrdinalIgnoreCase));
            //    }
            //    //if (
            //    //    string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
            //    //    string.Equals(path, "/errorlog", StringComparison.OrdinalIgnoreCase) ||
            //    //    string.Equals(path, "/index.html", StringComparison.OrdinalIgnoreCase))
            //    if (checkOpenUrls.Any())
            //    {
            //        // The request token can be sent as a JavaScript-readable cookie, 
            //        // and Angular uses it by default.
            //        var tokens = antiforgery.GetAndStoreTokens(context);
            //        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //            new CookieOptions() { HttpOnly = false });
            //    }

            //    return next(context);
            //});
            #endregion

            app.UseCookiePolicy();
            app.UseSession();

            app.UseAuthentication();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<TokenManagerMiddleware>();

            //app.UseStatusCodePages();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    c.RoutePrefix = "api-doc";
                    c.DocumentTitle = "Web API Doc";
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseElmah();

            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
