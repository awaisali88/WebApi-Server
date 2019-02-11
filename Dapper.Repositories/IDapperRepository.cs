using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper.Repositories.SqlGenerator;

namespace Dapper.Repositories
{
    /// <summary>
    ///     interface for repository
    /// </summary>
    public interface IDapperRepository<TEntity> where TEntity : class
    {
        /// <summary>
        ///     DB Connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        ///     SQL Genetator
        /// </summary>
        ISqlGenerator<TEntity> SqlGenerator { get; }

        /// <summary>
        ///     Get number of rows
        /// </summary>
        int Count(bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows
        /// </summary>
        int Count(bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get number of rows with WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows with WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get number of rows with DISTINCT clause
        /// </summary>
        int Count(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows with DISTINCT clause
        /// </summary>
        int Count(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get number of rows
        /// </summary>
        Task<int> CountAsync(bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows
        /// </summary>
        Task<int> CountAsync(bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get number of rows with WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows with WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get number of rows with DISTINCT clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows with DISTINCT clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted);
        
        /// <summary>
        ///     Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get first object
        /// </summary>
        TEntity Find(bool includeLogicalDeleted);

        /// <summary>
        ///     Get first object
        /// </summary>
        TEntity Find(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted);

        /// <summary>
        ///     Get first object
        /// </summary>
        TEntity Find(bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get first object
        /// </summary>
        TEntity Find(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntity Find<TChild1>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id
        /// </summary>
        TEntity FindById(object id, bool includeLogicalDeleted);

        /// <summary>
        ///     Get object by Id
        /// </summary>
        TEntity FindById(object id, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1>(object id,
            Expression<Func<TEntity, object>> tChild1,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3, TChild4>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id
        /// </summary>
        Task<TEntity> FindByIdAsync(object id, bool includeLogicalDeleted);

        /// <summary>
        ///     Get object by Id
        /// </summary>
        Task<TEntity> FindByIdAsync(object id, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1>(object id,
            Expression<Func<TEntity, object>> tChild1,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            bool includeLogicalDeleted, IDbTransaction transaction = null);


        /// <summary>
        ///     Get first object
        /// </summary>
        Task<TEntity> FindAsync(bool includeLogicalDeleted);

        /// <summary>
        ///     Get first object
        /// </summary>
        Task<TEntity> FindAsync(bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get first object
        /// </summary>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted);

        /// <summary>
        ///     Get first object
        /// </summary>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects
        /// </summary>
        IEnumerable<TEntity> FindAll(bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects
        /// </summary>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction = null);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, bool includeLogicalDeleted, IDbTransaction transaction = null);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync(bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, IDbTransaction transaction = null);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            bool includeLogicalDeleted, IDbTransaction transaction = null);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Insert object to DB
        /// </summary>
        (bool, TEntity) Insert(TEntity instance);

        /// <summary>
        ///     Insert object to DB
        /// </summary>
        (bool, TEntity) Insert(TEntity instance, IDbTransaction transaction);

        /// <summary>
        ///     Insert object to DB
        /// </summary>
        Task<(bool, TEntity)> InsertAsync(TEntity instance);

        /// <summary>
        ///     Insert object to DB
        /// </summary>
        Task<(bool, TEntity)> InsertAsync(TEntity instance, IDbTransaction transaction);

        /// <summary>
        ///     Bulk Insert objects to DB
        /// </summary>
        int BulkInsert(IEnumerable<TEntity> instances, IDbTransaction transaction = null);

        /// <summary>
        ///     Bulk Insert objects to DB
        /// </summary>
        Task<int> BulkInsertAsync(IEnumerable<TEntity> instances, IDbTransaction transaction = null);

        /// <summary>
        ///     Delete object from DB
        /// </summary>
        bool Delete(TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        ///     Delete object from DB
        /// </summary>
        Task<bool> DeleteAsync(TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        ///     Delete objects from DB
        /// </summary>
        bool Delete(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        ///     Delete objects from DB
        /// </summary>
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction = null);


        /// <summary>
        ///     Update object in DB
        /// </summary>
        (bool, TEntity) Update(TEntity instance);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        (bool, TEntity) Update(TEntity instance, IDbTransaction transaction);


        /// <summary>
        ///     Update object in DB
        /// </summary>
        Task<(bool, TEntity)> UpdateAsync(TEntity instance);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        Task<(bool, TEntity)> UpdateAsync(TEntity instance, IDbTransaction transaction);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        (bool, TEntity) Update(Expression<Func<TEntity, bool>> predicate, TEntity instance);


        /// <summary>
        ///     Update object in DB
        /// </summary>
        (bool, TEntity) Update(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        Task<(bool, TEntity)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        Task<(bool, TEntity)> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction);


        /// <summary>
        ///     Bulk Update objects in DB
        /// </summary>
        Task<bool> BulkUpdateAsync(IEnumerable<TEntity> instances);

        /// <summary>
        ///     Bulk Update objects in DB
        /// </summary>
        Task<bool> BulkUpdateAsync(IEnumerable<TEntity> instances, IDbTransaction transaction);

        /// <summary>
        ///     Bulk Update objects in DB
        /// </summary>
        bool BulkUpdate(IEnumerable<TEntity> instances);

        /// <summary>
        ///     Bulk Update objects in DB
        /// </summary>
        bool BulkUpdate(IEnumerable<TEntity> instances, IDbTransaction transaction);


        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate = null,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, bool includeLogicalDeleted, IDbTransaction transaction = null);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            bool includeLogicalDeleted, IDbTransaction transaction = null);
    }
}