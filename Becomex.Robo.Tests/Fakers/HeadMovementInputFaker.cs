using Becomex.Robo.Application.Models.Input;
using Bexomex.Robo.Domain.Enums;
using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Tests.Fakers
{
    public static class HeadMovementInputFaker
    {
        public static Faker<HeadMovementInput> Create()
        {
            return new Faker<HeadMovementInput>("pt_BR")
                .RuleFor(r => r.NewHeadInclinationState, HeadInclinationState.HeadUp)
                .RuleFor(r => r.NewHeadRotationState, (f, r) => HeadRotationState.Negative45Degrees);
        }
    }
}
