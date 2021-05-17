using Becomex.Robo.Application.Helpers;
using Bexomex.Robo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Models
{
    public class RobotModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RobotStatus Status { get; set; }
        public HeadModel Head { get; set; }
        public ArmModel RightArm { get; set; }
        public ArmModel LeftArm { get; set; }

        public string StatusDescription { get { return Status.GetDescription(); } }
    }
}
