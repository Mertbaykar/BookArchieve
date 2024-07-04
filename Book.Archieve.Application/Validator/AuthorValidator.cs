using Book.Archieve.Domain.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Validator
{
    public class AuthorValidator : AbstractValidator<Domain.Entity.Author>
    {
       
        public AuthorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad bilgisi giriniz");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad bilgisi giriniz");
        }
    }
}
