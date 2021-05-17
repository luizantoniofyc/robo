using Bexomex.Robo.Domain.Entities;
using Bexomex.Robo.Domain.Enums;
using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Tests.Fakers
{
    public static class RobotFaker
    {
        public static Faker<Robot> Create()
        {
            return new Faker<Robot>("pt_BR")
                .RuleFor(r => r.Description, (f, r) => f.Lorem.Sentences(10))
                .RuleFor(r => r.Name, (f, r) => f.Lorem.Sentences(10))
                .RuleFor(r => r.Status, (f, r) => RobotStatus.Ready);
        }
    }
}
