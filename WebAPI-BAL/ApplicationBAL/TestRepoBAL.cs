using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.ApplicationContext;
using WebAPI_Model.Test;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.ApplicationBAL
{
    public class TestRepoBal : CommonBusinessLogic<IApplicationDbContext, TestRepo, TestRepoViewModel>, ITestRepoBal
    {
        private readonly ILogger<TestRepoBal> _logger;

        public TestRepoBal(IApplicationDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<TestRepoBal> logger,
            ILogger<CommonBusinessLogic<IApplicationDbContext, TestRepo, TestRepoViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
