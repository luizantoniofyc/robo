using Bexomex.Robo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bexomex.Robo.Domain.Entities
{
    public class Head
    {
        public HeadRotationState RotationState { get; set; }
        public HeadInclinationState InclinationState { get; set; }
    }
}
