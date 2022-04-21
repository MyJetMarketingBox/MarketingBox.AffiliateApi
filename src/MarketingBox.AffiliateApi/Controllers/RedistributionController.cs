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
        private readonly IRegistrationImporter _registrationImporter;
        private readonly ILogger<RedistributionController> _logger;
        private readonly IMapper _mapper;

        public RedistributionController(IRedistributionService redistributionService, 
            IRegistrationImporter registrationImporter, 
            ILogger<RedistributionController> logger, 
            IMapper mapper)
        {
            _redistributionService = redistributionService;
            _registrationImporter = registrationImporter;
            _logger = logger;
            _mapper = mapper;
        }
        
        [HttpPost("upload-file")]
        public async Task<ActionResult<ImportResponse>> UploadFileAsync(IFormFile file)
        {
            try
            {
                if (!file.FileName.Contains(".csv", StringComparison.InvariantCultureIgnoreCase))
                    throw new BadRequestException("Unsupported file type");

                await using var s = file.OpenReadStream();
                using var br = new BinaryReader(s);
                var bytes = br.ReadBytes((int)s.Length);

                var response = await _registrationImporter.ImportAsync(new ImportRequest()
                {
                    RegistrationsFile = bytes,
                    UserId = this.GetUserId()
                });

                return this.ProcessResult(response, response?.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex.Failed<ImportResponse>();
            }
        }
        
        [HttpGet("files")]
        public async Task<ActionResult<List<RegistrationsFileHttp>>> GetFilesAsync()
        {
            try
            {
                var result = await _registrationImporter.GetRegistrationFilesAsync();

                return this.ProcessResult(result, result?.Data?.Files.Select(_mapper.Map<RegistrationsFileHttp>).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }
        }
        
        [HttpGet("parse-file")]
        public async Task<ActionResult<List<RegistrationFromFile>>> GetRegistrationsFromFileAsync([FromQuery] long fileId)
        {
            var result = await _registrationImporter.GetRegistrationsFromFileAsync(new GetRegistrationsFromFileRequest()
            {
                FileId = fileId
            });

            return this.ProcessResult(result, result?.Data);
        }
    }
}