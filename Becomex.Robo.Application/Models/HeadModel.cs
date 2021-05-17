using Bexomex.Robo.Domain.Enums;
using Becomex.Robo.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Models
{
    public class HeadModel
    {
        public HeadRotationState RotationState { get; set; }
        public HeadInclinationState InclinationState { get; set; }

        public string RotationStateDescription { get { return RotationState.GetDescription(); } }
        public string InclinationStateDescription { get { return InclinationState.GetDescription(); } }
    }
}
