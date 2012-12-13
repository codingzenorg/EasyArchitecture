﻿using Application4Test.Domain;
using EasyArchitecture.Validation.Plugin.BultIn;

namespace Application4Test.Infrastructure.Validators
{
    public class DogValidator:Validator<Dog>
    {
        public DogValidator() {
            AddRule(d => d.Age > 10, "There's no dog so old");
        }
    }
}
