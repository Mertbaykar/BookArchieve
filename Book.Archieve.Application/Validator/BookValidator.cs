using Book.Archieve.Domain.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Validator
{
    public class BookValidator : AbstractValidator<Domain.Entity.Book>
    {
       
        public BookValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad bilgisi giriniz");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("Özet bilgisi giriniz");
            RuleFor(x => x.ShelfLocation).NotEmpty().WithMessage("Raf bilgisi giriniz");
            RuleFor(x => x.CoverImagePath).NotEmpty().WithMessage("Kapak resmi giriniz");
            RuleFor(x => x.AuthorId).NotEmpty().WithMessage("Yazar bilgisi giriniz");
            RuleFor(x => x.PublishYear).NotEmpty().WithMessage("Yayın yılı giriniz");
        }
    }
}
