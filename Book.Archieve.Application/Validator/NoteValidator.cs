using Book.Archieve.Domain.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Validator
{
    public class NoteValidator : AbstractValidator<Domain.Entity.Note>
    {
       
        public NoteValidator()
        {
            //RuleFor(x => x.BookId).NotEmpty().WithMessage("Kitap seçiniz");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Not giriniz");
         
        }
    }
}
