using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.RegistrationFile;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.Redistribution.Service.Domain.Models;
using MarketingBox.Redistribution.Service.Grpc;
using MarketingBox.Redistribution.Service.Grpc.Models;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/registration-file")]
    public class RegistrationFileController : Controller
    {
        private readonly IRegistrationImporter _registrationImporter;
        private readonly ILogger<RegistrationFileController> _logger;
        private readonly IMapper _mapper;

        public RegistrationFileController(IRegistrationImporter registrationImporter,
            ILogger<RegistrationFileController> logger,
            IMapper mapper)
        {
            _registrationImporter = registrationImporter;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("upload-file")]
        public async Task<ActionResult<ImportResponse>> UploadFileAsync(IFormFile file)
        {
            try
            {
                var fileName = file.FileName;
                if (!fileName.Contains(".csv", StringComparison.InvariantCultureIgnoreCase))
                    throw new BadRequestException("Unsupported file type");

                await using var s = file.OpenReadStream();
                using var br = new BinaryReader(s);
                var bytes = br.ReadBytes((int) s.Length);

                var response = await _registrationImporter.ImportAsync(new ImportRequest()
                {
                    FileName = fileName.Replace(".csv",""),
                    RegistrationsFile = bytes,
                    UserId = this.GetUserId(),
                    TenantId = this.GetTenantId()
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
        public async Task<ActionResult<Paginated<RegistrationsFileHttp, long?>>> GetFilesAsync(
            [FromQuery] GetFilesRequestHttp request)
        {
            var response = await _registrationImporter.GetRegistrationFilesAsync(new GetFilesRequest
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                FileName = request.FileName
            });

            return this.ProcessResult(
                response,
                (response.Data?.Select(_mapper.Map<RegistrationsFileHttp>)
                    .ToArray() ?? Array.Empty<RegistrationsFileHttp>())
                .Paginate(request, Url, response.Total ?? default, x => x.Id));
        }

        [HttpGet("parse-file")]
        public async Task<ActionResult<Paginated<RegistrationFromFile, long?>>> GetRegistrationsFromFileAsync(
            [FromQuery] GetRegistrationsFromFileRequestHttp request)
        {
            var response = await _registrationImporter.GetRegistrationsFromFileAsync(
                new GetRegistrationsFromFileRequest()
            {                
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                Take = request.Limit,
                FileId = request.FileId,
            });

            return this.ProcessResult(
                response,
                (response.Data ?? Array.Empty<RegistrationFromFile>())
                .Paginate(request, Url, response.Total ?? default, x => x.Index));
        }
    }
}