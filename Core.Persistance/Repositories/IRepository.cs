﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Persistance.Dynamic;
using Core.Persistance.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Persistance.Repositories
{
    public interface IRepository<TEntity, TId> : IQuery<TEntity> where TEntity : Entity<TId> 
    {
        TEntity? Get(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, // provides to join between tables
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Paginate<TEntity> GetList(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, // provides ordering
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, // provides to join between tables
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Paginate<TEntity> GetListByDynamic(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, // provides to join between tables
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        bool Any(
            Expression<Func<TEntity, bool>> predicate = null,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        TEntity Add(TEntity entity);

        ICollection<TEntity> AddRange(ICollection<TEntity> entity);

        TEntity Update(TEntity entity);

        ICollection<TEntity> UpdateRange(ICollection<TEntity> entity);

        TEntity Delete(TEntity entity, bool permanent = false);

        ICollection<TEntity> DeleteRange(ICollection<TEntity> entity, bool permanent = false);
    }
}
