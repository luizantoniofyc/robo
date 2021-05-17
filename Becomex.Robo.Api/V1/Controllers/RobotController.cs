using Becomex.Robo.Api.Controllers._Shared;
using Becomex.Robo.Application.Interfaces;
using Becomex.Robo.Application.Models;
using Becomex.Robo.Application.Models.Input;
using Bexomex.Robo.Domain.Entities;
using Bexomex.Robo.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Becomex.Robo.Api.V1.Controllers
{
    public class RobotController : BaseController
    {
        private readonly IRobotUseCase _robotUseCase;

        public RobotController(IRobotUseCase robotUseCase)
        {
            _robotUseCase = robotUseCase;
        }

        /// <summary>
        /// Create a new robot
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <param name="model">Robot to be created</param>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Robot), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create(ApiVersion apiVersion, [FromBody] RobotInput model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();
            var robot = await _robotUseCase.AddNewRobotAsync(model);
            var urlString = $"{HttpContext.Request.Path}/{robot.Id}";
            return Created(urlString, robot);
        }

        /// <summary>
        ///  Gets a list of robots
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <returns>A collection of robots</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> GetRobots(ApiVersion apiVersion)
        {
            return Ok(await _robotUseCase.GetRobots());
        }

        /// <summary>
        /// Get a specific robot
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <param name="robotId">Robot Id</param>
        /// <returns>A specific robot.</returns>
        [HttpGet("{robotId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> GetRobot(ApiVersion apiVersion, [FromRoute] Guid robotId)
        {
            return Ok(await _robotUseCase.GetRobot(robotId));
        }

        /// <summary>
        /// Update a specific robot
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <param name="robotId">Robot Id</param>
        /// <param name="model">Robot to be updated</param>
        [HttpPut("{robotId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> Update(ApiVersion apiVersion, [FromRoute] Guid robotId, [FromBody] RobotInput model)
        {
            await _robotUseCase.UpdateRobotAsync(robotId, model);
            return Ok();
        }

        /// <summary>
        /// Fix a specific robot
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <param name="robotId">Robot Id</param>
        [HttpPut("{robotId}/fix")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> Update(ApiVersion apiVersion, [FromRoute] Guid robotId)
        {
            await _robotUseCase.FixRobotAsync(robotId);
            return Ok();
        }

        /// <summary>
        /// Move robot head
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <param name="robotId">Robot Id</param>
        /// <param name="model">How to move the robot head.</param>
        [HttpPut("{robotId}/moveHead")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> MoveHead(ApiVersion apiVersion, [FromRoute] Guid robotId, [FromBody] HeadMovementInput model)
        {
            await _robotUseCase.MoveRobotHeadAsync(robotId, model);
            return Ok();
        }

        /// <summary>
        /// Move robot elbow
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <param name="robotId">Robot Id</param>
        /// <param name="model">How to move the robot elbow.</param>
        [HttpPut("{robotId}/moveElbow")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> MoveElbow(ApiVersion apiVersion, [FromRoute] Guid robotId, [FromBody] ElbowMovementInput model)
        {
            await _robotUseCase.MoveRobotElbowAsync(robotId, model);
            return Ok();
        }

        /// <summary>
        /// Move robot fist
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if using URL versioning.</param>
        /// <param name="robotId">Robot Id</param>
        /// <param name="model">How to move the robot fist.</param>
        [HttpPut("{robotId}/moveFist")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ExceptionModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> MoveFist(ApiVersion apiVersion, [FromRoute] Guid robotId, [FromBody] FistMovementInput model)
        {
            await _robotUseCase.MoveRobotFistAsync(robotId, model);
            return Ok();
        }
    }
}
