using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bexomex.Robo.Domain.Enums
{
    public enum RobotStatus
    {
        [Description("Pronto")]
        Ready = 1,
        [Description("Corrompido")]
        Corrupted = 2,
    }
}
