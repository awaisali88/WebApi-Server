using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;

namespace WebAPI_BAL.BLL
{
    public class NorthwindCustomQueryBal : CommonStoreProcBusinessLogic<INorthwindDbContext>, INorthwindCustomQueryBal
    {
        private readonly ILogger<NorthwindCustomQueryBal> _logger;

        public NorthwindCustomQueryBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<NorthwindCustomQueryBal> logger,
            ILogger<CommonStoreProcBusinessLogic<INorthwindDbContext>> baseLogger
        )
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
