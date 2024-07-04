using Book.Archieve.Domain.Entity;
using Book.Archieve.Infrastructure.Context;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Repo
{
    public abstract class RepositoryBase
    {
        protected BookContext MasterDataDb;

        protected RepositoryBase(BookContext BookContext)
        {
            this.MasterDataDb = BookContext;
        }

        /// <summary>
        /// If invalid, throws exception along with error messages in it. Otherwise execution continues
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual void Validate<TModel>(TModel entity, IValidator<TModel> validator) where TModel : class
        {
            FluentValidation.Results.ValidationResult validationResult = validator.Validate(entity);

            if (!validationResult.IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                validationResult.Errors.ForEach(e => stringBuilder.AppendLine(e.ErrorMessage));
                throw new Exception(stringBuilder.ToString());
            }
        }

        #region EF Core

        protected virtual TEntity Add<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            MasterDataDb.Set<TEntity>().Add(entity);
            Save();
            return entity;
        }

        protected virtual TEntity? Get<TEntity>(int id) where TEntity : EntityBase
        {
            return MasterDataDb.Set<TEntity>().Where(x => x.Id == id).FirstOrDefault();
        }

        protected virtual void Delete<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            entity.DeActivate();
            Save();
        }

        protected virtual void Delete<TEntity>(int id) where TEntity : EntityBase
        {
            var entity = MasterDataDb.Set<TEntity>().Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new Exception($"{typeof(TEntity).Name} bulunamadı");
            Delete(entity);
        }

        protected void Save()
        {
            MasterDataDb.SaveChanges();
        }

        #endregion
    }
}
