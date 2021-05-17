using Becomex.Robo.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Becomex.Robo.Api.Controllers._Shared
{
    /// <summary>
    /// Base controller.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]s")]
    public abstract class BaseController : ControllerBase 
    {
        protected ExceptionModel LogException(Exception ex)
        {
            return new ExceptionModel()
            {
                Code = ex.HResult.ToString(),
                Message = ex.Message,
                Type = ex.GetType().ToString()
            };
        }
    }
}
