using Becomex.Robo.Application.Models.Input;
using Bexomex.Robo.Domain.Enums;
using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Tests.Fakers
{
    public static class FistMovementInputFaker
    {
        public static Faker<FistMovementInput> Create()
        {
            return new Faker<FistMovementInput>("pt_BR")
                .RuleFor(r => r.ArmSide, (f, r) => ArmSide.Left)
                .RuleFor(r => r.NewFistState, (f, r) => FistState.Negative45Degrees);
        }
    }
}
