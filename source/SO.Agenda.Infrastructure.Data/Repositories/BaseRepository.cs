﻿using Microsoft.EntityFrameworkCore;
using SO.Agenda.Domain.Model.Entities;
using SO.Agenda.Domain.Model.Interfaces.Repositories;
using SO.Agenda.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SO.Agenda.Infrastructure.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : BaseEntity
    {
        protected AgendaContext Ctx { get; }
        protected DbSet<TEntity> Set { get; }

        public BaseRepository(AgendaContext context)
        {
            Ctx = context;
            Set = Ctx.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity tEntity)
        {
            var entity = await Set.AddAsync(tEntity);
            return entity.Entity;
        }

        public virtual async Task<TEntity> FindAsync(Int32 id)
        {
            return await Set.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsNoTrackingAsync()
        {
            return await Set.AsNoTracking().ToListAsync();
        }

        public virtual TEntity Update(TEntity tEntity)
        {
            var entry = Ctx.Entry(tEntity);
            Set.Attach(tEntity);
            entry.State = EntityState.Modified;

            return tEntity;
        }

        public virtual void Remove(TEntity tEntity)
        {
            Set.Remove(tEntity);
        }

        public virtual async Task RemoveAsync(Int32 id)
        {
            Set.Remove(await Set.FindAsync(id));
        }
        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set.Where(predicate).AsNoTracking().ToListAsync();
        }
        public virtual async Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set.Where(predicate).AsNoTracking().FirstOrDefaultAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Ctx?.Dispose();
            }
        }
    }
}
