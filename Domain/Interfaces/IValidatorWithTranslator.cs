using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IValidatorWithTranslator<T> : IValidator<T>
    {
        IValidatorWithTranslator<T> Init(string language);
    }
}
