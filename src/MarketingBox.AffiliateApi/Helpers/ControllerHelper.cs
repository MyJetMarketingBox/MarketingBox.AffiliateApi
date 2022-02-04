using System;
using MarketingBox.Postback.Service.Grpc.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Helpers
{
    public static class ControllerHelper
    {
        public static ActionResult<TOut> ProcessResult<TOut,TIn>(
            this ControllerBase controllerBase,
            Response<TIn> result,
            TOut body)
        {
            switch (result.StatusCode)
            {
                case StatusCode.Ok:
                    return controllerBase.Ok(body);
                case StatusCode.NotFound:
                    controllerBase.ModelState.AddModelError("Error", result.ErrorMessage);
                    return controllerBase.NotFound(controllerBase.ModelState);
                case StatusCode.BadRequest:
                    controllerBase.ModelState.AddModelError("Error", result.ErrorMessage);
                    return controllerBase.BadRequest(controllerBase.ModelState);
                case StatusCode.InternalError:
                    return controllerBase.StatusCode(500);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}