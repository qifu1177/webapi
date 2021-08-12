using BLL.Helps;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Validators
{
    public abstract class AbstractValidatorWithTranslator<T> : AbstractValidator<T>
    {
        protected StringLocalizer _stringLocalizer;
        public AbstractValidatorWithTranslator(StringLocalizer stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

    }
}
