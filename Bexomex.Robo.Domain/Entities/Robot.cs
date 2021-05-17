using Bexomex.Robo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bexomex.Robo.Domain.Entities
{
    public class Robot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RobotStatus Status { get; set; }
        public Head Head { get; set; }
        public Arm RightArm { get; set; }
        public Arm LeftArm { get; set; }

        public Robot()
        {
            Id = Guid.NewGuid();
            Status = RobotStatus.Ready;

            Head = new Head() 
            { 
                InclinationState = Enums.HeadInclinationState.InRest, 
                RotationState = Enums.HeadRotationState.InRest 
            };

            RightArm = new Arm()
            {
                Elbow = new Elbow()
                {
                    State = Enums.ElbowState.InRest
                },
                Fist = new Fist()
                {
                    State = Enums.FistState.InRest
                }
            };

            LeftArm = new Arm()
            {
                Elbow = new Elbow()
                {
                    State = Enums.ElbowState.InRest
                },
                Fist = new Fist()
                {
                    State = Enums.FistState.InRest
                }
            };
        }

        public void Fix()
        {
            Status = RobotStatus.Ready;
        }

        public void Corrupt()
        {
            Status = RobotStatus.Corrupted;
        }
    }
}
