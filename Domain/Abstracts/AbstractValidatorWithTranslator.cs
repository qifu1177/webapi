using Domain.Interfaces;
using FluentValidation;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstracts
{
    public abstract class AbstractValidatorWithTranslator<T> : AbstractValidator<T>, IValidatorWithTranslator<T>
    {
        public AbstractValidatorWithTranslator(ITranslator translator)
        {
            _translator = translator;
        }
        protected ITranslator _translator;

        public abstract IValidatorWithTranslator<T> Init(string language);

    }
}
