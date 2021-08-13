using Domain.Abstracts;
using Domain.Interfaces;
using Domain.Models.Requests;
using FluentValidation;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class UserRegisterRequestValidator : AbstractValidatorWithTranslator<UserRegisterRequest>, IValidatorWithTranslator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator(ITranslator translator) : base(translator)
        { }
        public override IValidatorWithTranslator<UserRegisterRequest> Init(string language)
        {
            RuleFor(item => item.Email).NotNull().WithMessage(GetMessage(language, "notNull", "email"));
            RuleFor(item => item.Email).Matches(Help.Constents.RegexStrings.EmailFormExpression).WithMessage(GetMessage(language, "email_form"));
            return this;
        }
        private string GetMessage(string language, string messageKey, string paramMessagekey = null)
        {
            if (string.IsNullOrEmpty(paramMessagekey))
                return _translator[language, messageKey];
            return _translator[language, messageKey, _translator[language, paramMessagekey]];
        }
    }
}
