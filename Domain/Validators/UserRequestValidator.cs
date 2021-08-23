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
    
    public class UserRequestValidator : AbstractValidatorWithTranslator<UserRequest>, IValidatorWithTranslator<UserRequest>
    {
        public UserRequestValidator(ITranslator translator) : base(translator)
        { }
        public override IValidatorWithTranslator<UserRequest> Init(string language)
        {
            RuleFor(item => item.Email).NotNull().WithMessage(GetMessage(language, "notNull", "email"));
            RuleFor(item => item.Email).Matches(Help.Constents.RegexStrings.EmailFormExpression).WithMessage(GetMessage(language, "email_form"));
            RuleFor(item => item.UserName).NotNull().WithMessage(GetMessage(language, "notNull", "user_name"));
            RuleFor(item => item.RoleId).NotNull().WithMessage(GetMessage(language, "notNull", "role_id"));
            return this;
        }
        
    }
}
