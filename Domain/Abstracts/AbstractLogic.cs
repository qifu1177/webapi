using Domain.Interfaces;
using Domain.Models.Responses;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstracts
{
    public abstract class AbstractLogic
    {
        protected ITranslator _translator;
        public AbstractLogic(ITranslator translator)
        {
            _translator = translator;
        }

        protected ValidationResponse Validat<T>(IValidatorWithTranslator<T> validator, T request, string language) where T : IRequest
        {
            var result = validator.Init(language).Validate(request);
            if (result.Errors.Count > 0)
            {
                ValidationResponse response = new ValidationResponse();
                foreach (var failure in result.Errors)
                {
                    response.Messages.Add(failure.ErrorMessage);
                }
                return response;
            }
            return null;
        }
    }
}
