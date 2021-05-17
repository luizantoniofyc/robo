using Bexomex.Robo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Models.Input
{
    public class FistMovementInput : ArmMovementInput
    {
        public FistState NewFistState { get; set; }
    }
}
