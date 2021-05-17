using Becomex.Robo.Application.Models;
using Becomex.Robo.Application.Models.Input;
using Bexomex.Robo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Becomex.Robo.Application.Interfaces
{
    public interface IRobotUseCase
    {
        Task<List<RobotModel>> GetRobots();
        Task<RobotModel> GetRobot(Guid robotId);
        Task<Robot> AddNewRobotAsync(RobotInput robotInput);
        Task UpdateRobotAsync(Guid robotId, RobotInput robotInput);
        Task FixRobotAsync(Guid robotId);
        Task MoveRobotHeadAsync(Guid robotId, HeadMovementInput movementInput);
        Task MoveRobotElbowAsync(Guid robotId, ElbowMovementInput movementInput);
        Task MoveRobotFistAsync(Guid robotId, FistMovementInput movementInput);
        void Dispose();
    }
}
