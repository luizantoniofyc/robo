using AutoMapper;
using Becomex.Robo.Application.Exceptions;
using Becomex.Robo.Application.Interfaces;
using Becomex.Robo.Application.UseCases;
using Becomex.Robo.Tests.Fakers;
using Bexomex.Robo.Domain.Entities;
using Bexomex.Robo.Domain.Enums;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Becomex.Robo.Tests.Application
{
    public class RobotUseCaseTest
    {
        private readonly IRobotUseCase _robotUseCase;
        private readonly IMemoryCache _cache;
        private readonly Mock<IMapper> _mapper;

        public RobotUseCaseTest()
        {
            _cache = Create.MockedMemoryCache();

            var entryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
            _cache.Set("Robots", new List<Robot>(), entryOptions);

            _mapper = new Mock<IMapper>();
            _robotUseCase = new RobotUseCase(_cache, _mapper.Object);
        }

        [Fact]
        public async Task RobotUseCaseTest_AddNewRobotAsync_Success()
        {
            // Arrange
            var robotInput = RobotInputFaker.Create().Generate();

            // Act
            var result = await _robotUseCase.AddNewRobotAsync(robotInput);

            //Assert
            Assert.True(result != null);
        }

        [Fact]
        public async Task RobotUseCaseTest_AddNewRobotAsync_BN002()
        {
            // Arrange
            var robotInput = RobotInputFaker.Create().Generate();
            robotInput.Name = string.Empty;

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.AddNewRobotAsync(robotInput));

            //Assert
            Assert.Contains(ExceptionMessages.BN002, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_UpdateRobotAsync_Success()
        {
            // Arrange
            var robotInputFaker = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.UpdateRobotAsync(robot.Id, robotInputFaker);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robotObject.Name == robotInputFaker.Name);
        }

        [Fact]
        public async Task RobotUseCaseTest_UpdateRobotAsync_BN001()
        {
            // Arrange
            var robotInput = RobotInputFaker.Create().Generate();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.UpdateRobotAsync(Guid.NewGuid(), robotInput));

            //Assert
            Assert.Contains(ExceptionMessages.BN001, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_FixRobotAsync_Success()
        {
            // Arrange
            var robotInputFaker = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.FixRobotAsync(robot.Id);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robotObject.Status == RobotStatus.Ready);
        }

        [Fact]
        public async Task RobotUseCaseTest_FixRobotAsync_BN001()
        {
            // Arrange
            var robotInput = RobotInputFaker.Create().Generate();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.FixRobotAsync(Guid.NewGuid()));

            //Assert
            Assert.Contains(ExceptionMessages.BN001, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_Inclination_Success()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadRotationState = null;

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robot.Head.InclinationState == headMovementInputFaker.NewHeadInclinationState);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_Rotation_Success()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadInclinationState = null;

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robot.Head.RotationState == headMovementInputFaker.NewHeadRotationState);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_BN001()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            var robotInput = RobotInputFaker.Create().Generate();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(Guid.NewGuid(), headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN001, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_BN008()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            var robotInput = RobotInputFaker.Create().Generate();

            var robotFaker = RobotFaker.Create().Generate();
            robotFaker.Status = RobotStatus.Corrupted;

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN008, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_BN003()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadInclinationState = null;
            headMovementInputFaker.NewHeadRotationState = null;

            var robotInput = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN003, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_BN004()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();

            var robotInput = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN004, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_BN005()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadInclinationState = null;

            var robotInput = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();
            robotFaker.Head.InclinationState = HeadInclinationState.HeadDown;

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN005, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_Rotation_BN009()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadInclinationState = null;
            headMovementInputFaker.NewHeadRotationState = (HeadRotationState)10;

            var robotInput = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN009, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_Rotation_BN006()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadInclinationState = null;
            headMovementInputFaker.NewHeadRotationState = HeadRotationState.Negative90Degrees;

            var robotInput = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN006, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_Inclination_BN009()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadInclinationState = (HeadInclinationState)10;
            headMovementInputFaker.NewHeadRotationState = null;

            var robotInput = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN009, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveRobotHeadAsync_Inclination_BN006()
        {
            // Arrange
            var headMovementInputFaker = HeadMovementInputFaker.Create().Generate();
            headMovementInputFaker.NewHeadInclinationState = HeadInclinationState.HeadDown;
            headMovementInputFaker.NewHeadRotationState = null;

            var robotInput = RobotInputFaker.Create().Generate();
            var robotFaker = RobotFaker.Create().Generate();
            robotFaker.Head.InclinationState = HeadInclinationState.HeadUp;

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotHeadAsync(robot.Id, headMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN006, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveElbowAsync_Right_Success()
        {
            // Arrange
            var elbowMovementInputFaker = ElbowMovementInputFaker.Create().Generate();
            elbowMovementInputFaker.ArmSide = ArmSide.Right;

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.MoveRobotElbowAsync(robot.Id, elbowMovementInputFaker);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robot.RightArm.Elbow.State == elbowMovementInputFaker.NewElbowState);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveElbowAsync_Left_Success()
        {
            // Arrange
            var elbowMovementInputFaker = ElbowMovementInputFaker.Create().Generate();

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.MoveRobotElbowAsync(robot.Id, elbowMovementInputFaker);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robot.LeftArm.Elbow.State == elbowMovementInputFaker.NewElbowState);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveElbowAsync_BN001()
        {
            // Arrange
            var elbowMovementInputFaker = ElbowMovementInputFaker.Create().Generate();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotElbowAsync(Guid.NewGuid(), elbowMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN001, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveElbowAsync_BN009()
        {
            // Arrange
            var elbowMovementInputFaker = ElbowMovementInputFaker.Create().Generate();
            elbowMovementInputFaker.NewElbowState = (ElbowState)10;

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotElbowAsync(robot.Id, elbowMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN009, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveElbowAsync_BN006()
        {
            // Arrange
            var elbowMovementInputFaker = ElbowMovementInputFaker.Create().Generate();
            elbowMovementInputFaker.NewElbowState = ElbowState.StronglyContracted;

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotElbowAsync(robot.Id, elbowMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN006, exception.Message);
        }



        [Fact]
        public async Task RobotUseCaseTest_MoveFistAsync_Right_Success()
        {
            // Arrange
            var fistMovementInputFaker = FistMovementInputFaker.Create().Generate();
            fistMovementInputFaker.ArmSide = ArmSide.Right;

            var robotFaker = RobotFaker.Create().Generate();
            robotFaker.RightArm.Elbow.State = ElbowState.StronglyContracted;

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.MoveRobotFistAsync(robot.Id, fistMovementInputFaker);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robot.RightArm.Fist.State == fistMovementInputFaker.NewFistState);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveFistAsync_Left_Success()
        {
            // Arrange
            var fistMovementInputFaker = FistMovementInputFaker.Create().Generate();

            var robotFaker = RobotFaker.Create().Generate();
            robotFaker.LeftArm.Elbow.State = ElbowState.StronglyContracted;

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            await _robotUseCase.MoveRobotFistAsync(robot.Id, fistMovementInputFaker);

            var robotObject = robotList.FirstOrDefault(f => f.Id == robot.Id);

            //Assert
            Assert.True(robot.LeftArm.Fist.State == fistMovementInputFaker.NewFistState);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveFistAsync_BN001()
        {
            // Arrange
            var fistMovementInputFaker = FistMovementInputFaker.Create().Generate();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotFistAsync(Guid.NewGuid(), fistMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN001, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveFistAsync_BN009()
        {
            // Arrange
            var fistMovementInputFaker = FistMovementInputFaker.Create().Generate();
            fistMovementInputFaker.NewFistState = (FistState)10;

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotFistAsync(robot.Id, fistMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN009, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveFistAsync_BN006()
        {
            // Arrange
            var fistMovementInputFaker = FistMovementInputFaker.Create().Generate();
            fistMovementInputFaker.NewFistState = FistState.Positive180Degrees;

            var robotFaker = RobotFaker.Create().Generate();
            robotFaker.LeftArm.Elbow.State = ElbowState.StronglyContracted;

            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotFistAsync(robot.Id, fistMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN006, exception.Message);
        }

        [Fact]
        public async Task RobotUseCaseTest_MoveFistAsync_BN007()
        {
            // Arrange
            var fistMovementInputFaker = FistMovementInputFaker.Create().Generate();
            fistMovementInputFaker.NewFistState = FistState.Positive180Degrees;

            var robotFaker = RobotFaker.Create().Generate();
            var robotList = _cache.Get<List<Robot>>("Robots");
            robotList.Add(robotFaker);

            var robot = robotList.FirstOrDefault();

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _robotUseCase.MoveRobotFistAsync(robot.Id, fistMovementInputFaker));

            //Assert
            Assert.Contains(ExceptionMessages.BN007, exception.Message);
        }
    }
}
