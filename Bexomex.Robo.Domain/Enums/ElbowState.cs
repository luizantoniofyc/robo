using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bexomex.Robo.Domain.Enums
{
    public enum ElbowState
    {
        [Description("Em Repouso")]
        InRest = 1,
        [Description("Levemente Contraído")]
        SlightlyContracted = 2,
        [Description("Contraído")]
        Contracted = 3,
        [Description("Fortemente Contraído")]
        StronglyContracted = 4
    }
}
