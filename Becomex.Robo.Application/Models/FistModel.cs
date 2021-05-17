using Bexomex.Robo.Domain.Enums;
using Becomex.Robo.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Models
{
    public class FistModel
    {
        public FistState State { get; set; }
        public string StateDescription { get { return State.GetDescription(); } }
    }
}
