using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPI_ViewModel.ViewModelSwaggerExamples;

namespace WebAPI_Server.AppStart
{
    internal static partial class ServiceInjection
    {
        internal static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Web GCS API",
                    Description = "Access token: J15jZUAyrKnFadeTQrR4rQWLTRDAeGBFSWGhyfRPzDuK3qtZAa",
                    //TermsOfService = "None",
                    //Contact = new Contact
                    //{
                    //    Name = "Awais Ali",
                    //    Email = "awais.ali@liveadmins.com",
                    //},
                    //License = new License
                    //{
                    //    Name = "Use under LICX",
                    //    Url = "https://example.com/license"
                    //}
                });

                c.ExampleFilters();

                c.OperationFilter<RequestCallbackUrlFilter>();

                c.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]

                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                // or use the generic method, e.g. c.OperationFilter<AppendAuthorizeToSummaryOperationFilter<MyCustomAttribute>>();


                // add Security information to each operation for OAuth2
                c.OperationFilter<CustomSecurityRequirementsOperationFilter>(true, true);
                // or use the generic method, e.g. c.OperationFilter<SecurityRequirementsOperationFilter<MyCustomAttribute>>();

                // if you're using the SecurityRequirementsOperationFilter, you also need to tell Swashbuckle you're using OAuth2
                c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = HttpRequestHeaders.Authorization,
                    Type = "apiKey",
                });
                //c.OperationFilter<ApiKeySecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition(HttpRequestHeaders.ApiKey, new ApiKeyScheme
                {
                    Description = "Api Access Token",
                    In = "header",
                    Name = HttpRequestHeaders.ApiKey,
                    Type = "apiKey"
                });

                c.AddFluentValidationRules();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //services.AddSwaggerExamples();
            services.AddSwaggerExamplesFromAssemblyOf<AddNewUserViewModelExample>();
        }
    }

    #region Custom Swagger Filters
    /// <inheritdoc />
    internal class CustomSecurityRequirementsOperationFilter<TAuthorize, TAccessToken> : IOperationFilter where TAuthorize : Attribute where TAccessToken : Attribute
    {
        // inspired by https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/test/WebSites/OAuth2Integration/ResourceServer/Swagger/SecurityRequirementsOperationFilter.cs

        private readonly bool _includeUnauthorizedResponses;
        private readonly bool _includeForbiddenResponses;
        private readonly Func<IEnumerable<TAuthorize>, IEnumerable<string>> _authorizePolicySelector;
        private readonly Func<IEnumerable<TAccessToken>, IEnumerable<string>> _accessTokenPolicySelector;

        /// <summary>
        /// Constructor for SecurityRequirementsOperationFilter
        /// </summary>
        /// <param name="authorizePolicySelector">Used to select the authorization policy from the attribute e.g. (a => a.Policy)</param>
        /// <param name="accessTokenPolicySelector">Used to select the access token policy from the attribute e.g. (a => a.Policy)</param>
        /// <param name="includeUnauthorizedResponses">If true (default), then 401 responses will be added to every operation</param>
        /// <param name="includeForbiddenResponses">If true (default), then 403 responses will be added to every operation</param>
        public CustomSecurityRequirementsOperationFilter(Func<IEnumerable<TAuthorize>, IEnumerable<string>> authorizePolicySelector, Func<IEnumerable<TAccessToken>, IEnumerable<string>> accessTokenPolicySelector,
            bool includeUnauthorizedResponses = true, bool includeForbiddenResponses = true)
        {
            _authorizePolicySelector = authorizePolicySelector;
            _accessTokenPolicySelector = accessTokenPolicySelector;
            _includeUnauthorizedResponses = includeUnauthorizedResponses;
            _includeForbiddenResponses = includeForbiddenResponses;
        }

        /// <inheritdoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            Dictionary<string, IEnumerable<string>> policy = new Dictionary<string, IEnumerable<string>>();

            if (!context.GetControllerAndActionAttributes<AllowNoAccessToken>().Any())
            {
                var allAccessTokenActions = context.GetControllerAndActionAttributes<TAccessToken>();
                var atAttributes = allAccessTokenActions as TAccessToken[] ?? allAccessTokenActions.ToArray();
                if (!atAttributes.Any())
                {
                    return;
                }

                var accessTokenPolicies = _accessTokenPolicySelector(atAttributes) ?? Enumerable.Empty<string>();
                var tokenPolicies = accessTokenPolicies as string[] ?? accessTokenPolicies.ToArray();
                if (tokenPolicies.Any())
                {
                    if (_includeForbiddenResponses && !operation.Responses.ContainsKey("412"))
                        operation.Responses.Add("412",
                            new Response
                            {
                                Description = "Precondition Failed",
                                //Examples = new ApiResponse(false, ErrorMessages.InvalidApiKey, null)
                            });
                    //operation.Responses.Add("403",
                    //    new Response
                    //    {
                    //        Description =
                    //            JsonConvert.SerializeObject(new ApiResponse(false, ErrorMessages.InvalidApiKey,
                    //                null))
                    //    });

                    policy.Add(HttpRequestHeaders.ApiKey, tokenPolicies);
                }
            }


            if (!context.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
            {
                var actionAttributes = context.GetControllerAndActionAttributes<TAuthorize>();
                var attributes = actionAttributes as TAuthorize[] ?? actionAttributes.ToArray();
                if (!attributes.Any())
                {
                    return;
                }

                if (_includeUnauthorizedResponses && !operation.Responses.ContainsKey("401"))
                    operation.Responses.Add("401",
                        new Response
                        {
                            Description = "Unauthorized",
                            //Examples = new ApiResponse401Example()
                        });
                //operation.Responses.Add("401", new Response { Description = JsonConvert.SerializeObject(new ApiResponse(false, ErrorMessages.UnAuthorized, null)) });
                
                var authorizePolicies = _authorizePolicySelector(attributes) ?? Enumerable.Empty<string>();

                policy.Add("oauth2", authorizePolicies);

            }

            if (policy.Any())
            {
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    policy
                };
            }
        }
    }

    /// <inheritdoc />
    internal class CustomSecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly CustomSecurityRequirementsOperationFilter<AuthorizeAttribute, AccessTokenFilter> _filter;

        /// <summary>
        /// Constructor for SecurityRequirementsOperationFilter
        /// </summary>
        /// <param name="includeUnauthorizedResponses">If true (default), then 401 responses will be added to every operation</param>
        /// <param name="includeForbiddenResponses">If true (default), then 403 responses will be added to every operation</param>
        public CustomSecurityRequirementsOperationFilter(bool includeUnauthorizedResponses = true, bool includeForbiddenResponses = true)
        {
            IEnumerable<string> AuthorizePolicySelector(IEnumerable<AuthorizeAttribute> authAttributes) =>
                authAttributes.Where(a => !string.IsNullOrEmpty(a.Policy))
                    .Select(a => a.Policy);
            IEnumerable<string> AccessTokenPolicySelector(IEnumerable<AccessTokenFilter> authAttributes) =>
                authAttributes.Where(a => a.UseAccessToken)
                    .Select(a => a.Policy);

            _filter = new CustomSecurityRequirementsOperationFilter<AuthorizeAttribute, AccessTokenFilter>(AuthorizePolicySelector, AccessTokenPolicySelector, includeUnauthorizedResponses, includeForbiddenResponses);
        }

        /// <inheritdoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            _filter.Apply(operation, context);
        }
    }

    internal static class OperationFilterContextExtensions
    {
        public static IEnumerable<T> GetControllerAndActionAttributes<T>(this OperationFilterContext context) where T : Attribute
        {
            var controllerAttributes = context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes<T>();
            var actionAttributes = context.MethodInfo.GetCustomAttributes<T>();

            var result = new List<T>(controllerAttributes);
            result.AddRange(actionAttributes);
            return result;
        }
    }

    internal class RequestCallbackUrlFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            if (context.GetControllerAndActionAttributes<RequireCallbackUrl>().Any())
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = HttpRequestHeaders.RequestUrl,
                    In = "header",
                    Type = "string",
                    Required = true, // set to false if this is optional,
                    Description = "Request Callback Url value (Use for email links etc.)"
                });
            }
        }
    }
    #endregion
}
