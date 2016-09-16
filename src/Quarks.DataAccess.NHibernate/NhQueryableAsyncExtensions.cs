using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global

namespace Quarks.DataAccess.NHibernate
{
    public static class NhQueryableAsyncExtensions
    {
        public static Task<TEntity[]> ToArrayAsync<TEntity>(
            this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.ToArray());
        }

        public static Task<List<TEntity>> ToListAsync<TEntity>(
            this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.ToList());
        }

        public static Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TEntity, TKey>(
            this IQueryable<TEntity> source, Func<TEntity, TKey> keySelector, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.ToDictionary(keySelector));
        }

        public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TEntity, TKey, TValue>(
           this IQueryable<TEntity> source, Func<TEntity, TKey> keySelector, Func<TEntity, TValue> elementSelector, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.ToDictionary(keySelector, elementSelector));
        }

        public static Task<HashSet<TEntity>> ToHashSetAsync<TEntity>(
            this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HashSet<TEntity>(source));
        }

        public static Task<TEntity> FirstAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.First());
        }

        public static Task<TEntity> FirstAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.First(predicate));
        }

        public static Task<TEntity> FirstAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.First(expression));
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.FirstOrDefault());
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.FirstOrDefault(predicate));
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.FirstOrDefault(expression));
        }

        public static Task<TEntity> SingleAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Single());
        }

        public static Task<TEntity> SingleAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Single(predicate));
        }

        public static Task<TEntity> SingleAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Single(expression));
        }

        public static Task<TEntity> SingleOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.SingleOrDefault());
        }

        public static Task<TEntity> SingleOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.SingleOrDefault(predicate));
        }

        public static Task<TEntity> SingleOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.SingleOrDefault(expression));
        }

        public static Task<TEntity> LastAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Last());
        }

        public static Task<TEntity> LastAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Last(predicate));
        }

        public static Task<TEntity> LastAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Last(expression));
        }

        public static Task<TEntity> LastOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.LastOrDefault());
        }

        public static Task<TEntity> LastOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.LastOrDefault(predicate));
        }

        public static Task<TEntity> LastOrDefaultAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.LastOrDefault(expression));
        }

        public static Task<int> CountAsync<TEntity>(this IQueryable<TEntity> source,  CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Count());
        }

        public static Task<int> CountAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Count(predicate));
        }

        public static Task<int> CountAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Count(expression));
        }

        public static Task<bool> AnyAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Any());
        }

        public static Task<bool> AsyAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Any(predicate));
        }

        public static Task<bool> AsyAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.Any(expression));
        }

        public static Task<bool> AllAsync<TEntity>(this IQueryable<TEntity> source, Func<TEntity, bool> predicate, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.All(predicate));
        }

        public static Task<bool> AllAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(source.All(expression));
        }
    }
}