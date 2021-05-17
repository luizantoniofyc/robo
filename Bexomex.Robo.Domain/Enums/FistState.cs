using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bexomex.Robo.Domain.Enums
{
    public enum FistState
    {
        [Description("Rotação para -90º")]
        Negative90Degrees = 1,
        [Description("Rotação para -45º")]
        Negative45Degrees = 2,
        [Description("Em Repouso")]
        InRest = 3,
        [Description("Rotação para 45º")]
        Positive45Degrees = 4,
        [Description("Rotação para 90º")]
        Positive90Degrees = 5,
        [Description("Rotação para 135º")]
        Positive135Degrees = 6,
        [Description("Rotação para 180º")]
        Positive180Degrees = 7
    }
}
