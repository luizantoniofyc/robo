using Bexomex.Robo.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Validators
{
    public class RobotValidator : AbstractValidator<Robot>
    {
        public RobotValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.Description).NotEmpty().WithMessage("Required field");
        }
    }
}
