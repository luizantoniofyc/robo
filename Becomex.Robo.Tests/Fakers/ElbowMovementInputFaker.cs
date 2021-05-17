using Becomex.Robo.Application.Models.Input;
using Bexomex.Robo.Domain.Enums;
using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Tests.Fakers
{
    public static class ElbowMovementInputFaker
    {
        public static Faker<ElbowMovementInput> Create()
        {
            return new Faker<ElbowMovementInput>("pt_BR")
                .RuleFor(r => r.ArmSide, (f, r) => ArmSide.Left)
                .RuleFor(r => r.NewElbowState, (f, r) => ElbowState.SlightlyContracted);
        }
    }
}
