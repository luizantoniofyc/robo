using Becomex.Robo.Application.Models.Input;
using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Tests.Fakers
{
    public static class RobotInputFaker
    {
        public static Faker<RobotInput> Create()
        {
            return new Faker<RobotInput>("pt_BR")
                .RuleFor(r => r.Description, (f, r) => f.Lorem.Sentences(30))
                .RuleFor(r => r.Name, (f, r) => f.Lorem.Sentences(10));
        }
    }
}
