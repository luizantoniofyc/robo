using Becomex.Robo.Application.Helpers;
using Becomex.Robo.Application.Models;
using Bexomex.Robo.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Becomex.Robo.UserInterface.Models
{
    public class RobotModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RobotStatus Status { get; set; }
        public HeadModel Head { get; set; }
        public ArmModel RightArm { get; set; }
        public ArmModel LeftArm { get; set; }

        public int HeadRotationStateId { get; set; }
        public int HeadInclinationStateId { get; set; }
        public int ElbowStateId { get; set; }
        public int FistStateId { get; set; }

        public IEnumerable<SelectListItem> HeadRotationStateList { get; set; }
        public IEnumerable<SelectListItem> HeadInclinationStateList { get; set; }
        public IEnumerable<SelectListItem> ElbowStateList { get; set; }
        public IEnumerable<SelectListItem> FistStateList { get; set; }

        public string StatusDescription { get { return Status.GetDescription(); } }
    }
}
