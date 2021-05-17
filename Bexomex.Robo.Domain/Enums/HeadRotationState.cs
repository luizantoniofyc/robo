using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bexomex.Robo.Domain.Enums
{
    public enum HeadRotationState
    {
        [Description("Rotação -90º")]
        Negative90Degrees = 1,
        [Description("Rotação -45º")]
        Negative45Degrees = 2,
        [Description("Em Repouso")]
        InRest = 3,
        [Description("Rotação 45º")]
        Positive45Degrees = 4,
        [Description("Rotação 90º")]
        Positive90Degrees = 5
    }
}
