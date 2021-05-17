using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bexomex.Robo.Domain.Enums
{
    public enum HeadInclinationState
    {
        [Description("Para Cima")]
        HeadUp = 1,
        [Description("Em Repouso")]
        InRest = 2,
        [Description("Para Baixo")]
        HeadDown = 3
    }
}
