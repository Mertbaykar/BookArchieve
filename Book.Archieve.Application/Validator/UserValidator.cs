using Book.Archieve.Domain.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        private const string mailExpression = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private const string passwordExpression = @"^(?=(.*[a-z]){3,})(?=(.*[A-Z]){2,})(?=(.*[0-9]){2,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{8,}$";

        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad bilgisi giriniz");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad bilgisi giriniz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Açıklama bilgisi giriniz")
                .Matches(mailExpression).WithMessage("Mail formatı uygun değil");
            RuleFor(x => x.Password).NotEmpty().Matches(passwordExpression).WithMessage("Şifrede en az iki büyük harf, bir özel karakter, iki sayı, üç küçük harf gerekiyor");
        }
    }
}
