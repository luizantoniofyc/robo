using AutoMapper;
using Becomex.Robo.Application.Exceptions;
using Becomex.Robo.Application.Interfaces;
using Becomex.Robo.Application.Models;
using Becomex.Robo.Application.Models.Input;
using Becomex.Robo.Application.Validators;
using Bexomex.Robo.Domain.Entities;
using Bexomex.Robo.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Becomex.Robo.Application.UseCases
{
    public class RobotUseCase : IRobotUseCase
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public RobotUseCase(
            IMemoryCache cache,
            IMapper mapper)
        {
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<Robot> AddNewRobotAsync(RobotInput robotInput)
        {
            return await Task.Run(() => {
                var robot = new Robot()
                {
                    Description = robotInput.Description,
                    Name = robotInput.Name
                };

                var validator = new RobotValidator();
                if (!validator.Validate(robot).IsValid) throw new BusinessException(ExceptionMessages.BN002, "BN002");

                // Add new robot to cache list
                var robotList = _cache.Get<List<Robot>>("Robots");
                robotList.Add(robot);

                return robot;
            });
        }

        public async Task UpdateRobotAsync(Guid robotId, RobotInput robotInput)
        {
            await Task.Run(() => {
                var robotList = _cache.Get<List<Robot>>("Robots");
                var robot = robotList.FirstOrDefault(f => f.Id == robotId);

                if (robot == null) throw new BusinessException(ExceptionMessages.BN001, "BN001");

                robot.Description = robotInput.Description;
                robot.Name = robotInput.Name;
            });
        }

        public async Task FixRobotAsync(Guid robotId)
        {
            await Task.Run(() => {
                var robotList = _cache.Get<List<Robot>>("Robots");
                var robot = robotList.FirstOrDefault(f => f.Id == robotId);

                if (robot == null) throw new BusinessException(ExceptionMessages.BN001, "BN001");

                robot.Fix();
            });
        }

        public async Task MoveRobotHeadAsync(Guid robotId, HeadMovementInput movementInput)
        {
            await Task.Run(() => {
                var robotList = _cache.Get<List<Robot>>("Robots");
                var robot = robotList.FirstOrDefault(f => f.Id == robotId);

                if (robot == null) throw new BusinessException(ExceptionMessages.BN001, "BN001");

                if (robot.Status == RobotStatus.Corrupted)
                    throw new BusinessException(ExceptionMessages.BN008, "BN008");

                if (!movementInput.NewHeadInclinationState.HasValue && !movementInput.NewHeadRotationState.HasValue)
                    throw new BusinessException(ExceptionMessages.BN003, "BN003");

                if (movementInput.NewHeadInclinationState.HasValue && movementInput.NewHeadRotationState.HasValue) 
                    throw new BusinessException(ExceptionMessages.BN004, "BN004");

                if (movementInput.NewHeadRotationState.HasValue && robot.Head.InclinationState == HeadInclinationState.HeadDown)
                        throw new BusinessException(ExceptionMessages.BN005, "BN005");

                if (movementInput.NewHeadRotationState.HasValue)
                {
                    if (!Enum.IsDefined(typeof(HeadRotationState), movementInput.NewHeadRotationState))
                    {
                        robot.Corrupt();
                        throw new BusinessException(ExceptionMessages.BN009, "BN009");
                    }
                    
                    if (!ValidateNewState((int)robot.Head.RotationState, (int)movementInput.NewHeadRotationState))
                        throw new BusinessException(ExceptionMessages.BN006, "BN006");

                    robot.Head.RotationState = movementInput.NewHeadRotationState.Value;
                }

                if (movementInput.NewHeadInclinationState.HasValue)
                {
                    if (!Enum.IsDefined(typeof(HeadInclinationState), movementInput.NewHeadInclinationState))
                    {
                        robot.Corrupt();
                        throw new BusinessException(ExceptionMessages.BN009, "BN009");
                    }
                    
                    if (!ValidateNewState((int)robot.Head.InclinationState, (int)movementInput.NewHeadInclinationState))
                        throw new BusinessException(ExceptionMessages.BN006, "BN006");

                    robot.Head.InclinationState = movementInput.NewHeadInclinationState.Value;
                }
            });
        }

        public async Task MoveRobotElbowAsync(Guid robotId, ElbowMovementInput movementInput)
        {
            await Task.Run(() => {
                var robotList = _cache.Get<List<Robot>>("Robots");
                var robot = robotList.FirstOrDefault(f => f.Id == robotId);

                if (robot == null) throw new BusinessException(ExceptionMessages.BN001, "BN001");

                if (!Enum.IsDefined(typeof(ElbowState), movementInput.NewElbowState))
                {
                    robot.Corrupt();
                    throw new BusinessException(ExceptionMessages.BN009, "BN009");
                }

                switch (movementInput.ArmSide)  
                {
                    case ArmSide.Right:
                        MoveElbow(robot.RightArm, movementInput.NewElbowState);
                        break;
                    case ArmSide.Left:
                        MoveElbow(robot.LeftArm, movementInput.NewElbowState);
                        break;
                    default:
                        break;
                }
            });
        }

        public async Task MoveRobotFistAsync(Guid robotId, FistMovementInput movementInput)
        {
            await Task.Run(() => {
                var robotList = _cache.Get<List<Robot>>("Robots");
                var robot = robotList.FirstOrDefault(f => f.Id == robotId);

                if (robot == null) throw new BusinessException(ExceptionMessages.BN001, "BN001");

                if (!Enum.IsDefined(typeof(FistState), movementInput.NewFistState))
                {
                    robot.Corrupt();
                    throw new BusinessException(ExceptionMessages.BN009, "BN009");
                }

                switch (movementInput.ArmSide)
                {
                    case ArmSide.Right:
                        MoveFist(robot.RightArm, movementInput.NewFistState);
                        break;
                    case ArmSide.Left:
                        MoveFist(robot.LeftArm, movementInput.NewFistState);
                        break;
                    default:
                        break;
                }
            });
        }

        public async Task<List<RobotModel>> GetRobots()
        {
            return await Task.Run(() => {
                return _mapper.Map<List<Robot>, List<RobotModel>>(_cache.Get<List<Robot>>("Robots"));
            });
        }

        public async Task<RobotModel> GetRobot(Guid robotId)
        {
            return await Task.Run(() => {
                var robotList = _mapper.Map<List<Robot>, List<RobotModel>>(_cache.Get<List<Robot>>("Robots"));
                return robotList.FirstOrDefault(f => f.Id == robotId);
            });
        }

        private bool ValidateNewState(int actualState, int newState)
        {
            return (newState == (actualState + 1)) || (newState == (actualState - 1));
        }

        private void MoveElbow(Arm arm, ElbowState newElbowState)
        {
            if (!ValidateNewState((int)arm.Elbow.State, (int)newElbowState))
                throw new BusinessException(ExceptionMessages.BN006, "BN006");

            arm.Elbow.State = newElbowState;
        }

        private void MoveFist(Arm arm, FistState newFistState)
        {
            if (arm.Elbow.State != ElbowState.StronglyContracted)
                throw new BusinessException(ExceptionMessages.BN007, "BN007");

            if (!ValidateNewState((int)arm.Fist.State, (int)newFistState))
                throw new BusinessException(ExceptionMessages.BN006, "BN006");

            arm.Fist.State = newFistState;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
