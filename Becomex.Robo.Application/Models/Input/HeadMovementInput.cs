using Bexomex.Robo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Becomex.Robo.Application.Models.Input
{
    public class HeadMovementInput
    {
        public HeadInclinationState? NewHeadInclinationState { get; set; }
        public HeadRotationState? NewHeadRotationState { get; set; }
    }
}
