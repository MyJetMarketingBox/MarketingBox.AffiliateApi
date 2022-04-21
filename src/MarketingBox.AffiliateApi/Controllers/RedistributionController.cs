using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.Redistribution.Service.Domain.Models;
using MarketingBox.Redistribution.Service.Grpc;
using MarketingBox.Redistribution.Service.Grpc.Models;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/redistribution")]
    public class RedistributionController : Controller
    {
        private readonly IRedistributionService _redistributionService;
        private readonly ILogger<RedistributionController> _logger;
        private readonly IMapper _mapper;

        public RedistributionController(IRedistributionService redistributionService, 
            ILogger<RedistributionController> logger, 
            IMapper mapper)
        {
            _redistributionService = redistributionService;
            _logger = logger;
            _mapper = mapper;
        }
        
    }
}