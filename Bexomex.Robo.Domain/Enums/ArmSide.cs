using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bexomex.Robo.Domain.Enums
{
    public enum ArmSide
    {
        [Description("Braço Direito")]
        Right = 1,
        [Description("Braço Esquerdo")]
        Left = 2,
    }
}
