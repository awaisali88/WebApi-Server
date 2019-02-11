using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dapper.Repositories;
using Dapper.Repositories.DbContext;
using Dapper.Repositories.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebAPI_BAL
{
    public class CommonBusinessLogic<TDbContext, TEntity, TEntityViewModel> :  ICommonBusinessLogic<TDbContext, TEntity, TEntityViewModel> 
        where TEntity : class, IDefaultColumns
        where TEntityViewModel : class 
        where TDbContext : IDapperDbContext
    {
        private readonly TDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CommonBusinessLogic<TDbContext, TEntity, TEntityViewModel>> _logger;

        private readonly IDapperRepository<TEntity> _repo;
        private readonly IDapperSProcRepository _spRepo;

        public CommonBusinessLogic(TDbContext db, IMapper mapper, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, ILogger<CommonBusinessLogic<TDbContext, TEntity, TEntityViewModel>> logger)
        {
            _db = db;
            _mapper = mapper;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            var repoProperty = _db.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == typeof(IDapperRepository<TEntity>));
            var spRepoProperty = _db.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == typeof(IDapperSProcRepository));

            if (repoProperty != null)
                _repo = (IDapperRepository<TEntity>) repoProperty.GetValue(_db);
            else
                throw new ArgumentNullException($"Repository for {typeof(TEntity).Name} is not defined in Database Context.");

            if (spRepoProperty != null)
                _spRepo = (IDapperSProcRepository)spRepoProperty.GetValue(_db);
        }

        #region Manage Or Handle Transaction

        private TReturn ManageOrHandleTransaction<TReturn>(Func<IDbTransaction, TReturn> func, IDbTransaction transaction, bool manageTransaction)
        {
            if (!manageTransaction)
                return func(null);

            return transaction == null ? HandleTransaction(func) : func(transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1>(Func<TData1, IDbTransaction, TReturn> func,
            TData1 data1, IDbTransaction transaction, bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, null);

            return transaction == null ? HandleTransaction(func, data1) : func(data1, transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1, TData2>(
            Func<TData1, TData2, IDbTransaction, TReturn> func, TData1 data1, TData2 data2, IDbTransaction transaction,
            bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, data2, null);

            return transaction == null ? HandleTransaction(func, data1, data2) : func(data1, data2, transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1, TData2, TData3>(
            Func<TData1, TData2, TData3, IDbTransaction, TReturn> func, TData1 data1, TData2 data2, TData3 data3,
            IDbTransaction transaction, bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, data2, data3, null);

            return transaction == null
                ? HandleTransaction(func, data1, data2, data3)
                : func(data1, data2, data3, transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1, TData2, TData3, TData4>(
            Func<TData1, TData2, TData3, TData4, IDbTransaction, TReturn> func, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, IDbTransaction transaction, bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, data2, data3, data4, null);

            return transaction == null
                ? HandleTransaction(func, data1, data2, data3, data4)
                : func(data1, data2, data3, data4, transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5>(
            Func<TData1, TData2, TData3, TData4, TData5, IDbTransaction, TReturn> func, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, IDbTransaction transaction, bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, data2, data3, data4, data5, null);

            return transaction == null
                ? HandleTransaction(func, data1, data2, data3, data4, data5)
                : func(data1, data2, data3, data4, data5, transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, IDbTransaction, TReturn> func, TData1 data1,
            TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6, IDbTransaction transaction, bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, data2, data3, data4, data5, data6, null);

            return transaction == null
                ? HandleTransaction(func, data1, data2, data3, data4, data5, data6)
                : func(data1, data2, data3, data4, data5, data6, transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6, TData7>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, TData7, IDbTransaction, TReturn> func, TData1 data1,
            TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6, TData7 data7, IDbTransaction transaction,
            bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, data2, data3, data4, data5, data6, data7, null);

            return transaction == null
                ? HandleTransaction(func, data1, data2, data3, data4, data5, data6, data7)
                : func(data1, data2, data3, data4, data5, data6, data7, transaction);
        }

        private TReturn ManageOrHandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6, TData7, TData8>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, TData7, TData8, IDbTransaction, TReturn> func, TData1 data1,
            TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6, TData7 data7, TData8 data8, IDbTransaction transaction,
            bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, data2, data3, data4, data5, data6, data7, data8, null);

            return transaction == null
                ? HandleTransaction(func, data1, data2, data3, data4, data5, data6, data7, data8)
                : func(data1, data2, data3, data4, data5, data6, data7, data8, transaction);
        }
        #endregion

        #region Handle Transaction
        public TReturn HandleTransaction<TReturn>(Func<IDbTransaction, TReturn> repoFunc)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public TReturn HandleTransaction<TReturn, TData>(Func<TData, IDbTransaction, TReturn> repoFunc, TData data)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public TReturn HandleTransaction<TReturn, TData1, TData2>(
            Func<TData1, TData2, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data1, data2, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }
        
        public TReturn HandleTransaction<TReturn, TData1, TData2, TData3>(
            Func<TData1, TData2, TData3, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2, TData3 data3)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data1, data2, data3, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4>(
            Func<TData1, TData2, TData3, TData4, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data1, data2, data3, data4, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5>(
            Func<TData1, TData2, TData3, TData4, TData5, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, TData5 data5)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data1, data2, data3, data4, data5, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data1, data2, data3, data4, data5, data6, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6, TData7>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, TData7, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6, TData7 data7)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data1, data2, data3, data4, data5, data6, data7, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6, TData7, TData8>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, TData7, TData8, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6, TData7 data7, TData8 data8)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data1, data2, data3, data4, data5, data6, data7, data8, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }
        #endregion

        #region Store Procedure

        public virtual IEnumerable<TDataViewModel> ExecuteStoreProcedure<TData, TParams, TDataViewModel, TParamsViewModel>(
            TParamsViewModel spParam, IDbTransaction transaction = null, bool manageTransaction = true) where TParams : class, ISProcParam
        {
            if (_spRepo != null)
            {
                TParams paramsData = _mapper.Map<TParams>(spParam);
                IEnumerable<TData> spData = ManageOrHandleTransaction(_spRepo.Execute<TData, TParams>, paramsData,
                    transaction, manageTransaction);
                return _mapper.Map<IEnumerable<TDataViewModel>>(spData);
            }

            throw new ArgumentNullException($"Store Procedure Repository is not defined in Database Context");
        }

        public virtual async Task<IEnumerable<TDataViewModel>> ExecuteStoreProcedureAsync<TData, TParams, TDataViewModel, TParamsViewModel>(
            TParamsViewModel spParam, IDbTransaction transaction = null, bool manageTransaction = true) where TParams : class, ISProcParam
        {
            if (_spRepo != null)
            {
                TParams paramsData = _mapper.Map<TParams>(spParam);
                IEnumerable<TData> spData = await ManageOrHandleTransaction(_spRepo.ExecuteAsync<TData, TParams>,
                    paramsData, transaction, manageTransaction);
                return _mapper.Map<IEnumerable<TDataViewModel>>(spData);
            }

            throw new ArgumentNullException($"Store Procedure Repository is not defined in Database Context");
        }

        #endregion

        #region Insert
        public virtual int BulkInsert(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            IEnumerable<TEntity> entityData = _mapper.Map<IEnumerable<TEntity>>(viewModelData);
            entityData = ExtBusinessLogic.GetBulkInsertEntity(entityData, ExtBusinessLogic.UserValue(claim));

            return ManageOrHandleTransaction(_repo.BulkInsert, entityData, transaction, manageTransaction);
        }

        public virtual Task<int> BulkInsertAsync(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            IEnumerable<TEntity> entityData = _mapper.Map<IEnumerable<TEntity>>(viewModelData);
            entityData = ExtBusinessLogic.GetBulkInsertEntity(entityData, ExtBusinessLogic.UserValue(claim));

            return ManageOrHandleTransaction(_repo.BulkInsertAsync, entityData, transaction, manageTransaction);
        }

        public virtual (bool, TEntityViewModel) Insert(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            entityData = ExtBusinessLogic.GetInsertEntity(entityData, ExtBusinessLogic.UserValue(claim));

            (bool, TEntity) data = ManageOrHandleTransaction(_repo.Insert, entityData, transaction, manageTransaction);

            return (data.Item1, _mapper.Map<TEntityViewModel>(data.Item2));
        }

        public virtual async Task<(bool, TEntityViewModel)> InsertAsync(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            entityData = ExtBusinessLogic.GetInsertEntity(entityData, ExtBusinessLogic.UserValue(claim));

            Task<(bool, TEntity)> dataAsync = ManageOrHandleTransaction(_repo.InsertAsync, entityData, transaction, manageTransaction);
            (bool, TEntity) data = await dataAsync;

            return (data.Item1, _mapper.Map<TEntityViewModel>(data.Item2));
        }
        #endregion

        #region Update
        public virtual (bool, TEntityViewModel) Update(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            entityData = ExtBusinessLogic.GetUpdateEntity(entityData, ExtBusinessLogic.UserValue(claim));

            (bool, TEntity) data = ManageOrHandleTransaction(_repo.Update, entityData, transaction, manageTransaction);

            return (data.Item1, _mapper.Map<TEntityViewModel>(data.Item2));
        }

        public virtual async Task<(bool, TEntityViewModel)> UpdateAsync(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            entityData = ExtBusinessLogic.GetUpdateEntity(entityData, ExtBusinessLogic.UserValue(claim));

            Task<(bool, TEntity)> dataAsync = ManageOrHandleTransaction(_repo.UpdateAsync, entityData, transaction, manageTransaction);

            (bool, TEntity) data = await dataAsync;
            return (data.Item1, _mapper.Map<TEntityViewModel>(data.Item2));
        }

        public virtual (bool, TEntityViewModel) Update(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where, TEntityViewModel viewModelData, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);

            entityData = ExtBusinessLogic.GetUpdateEntity(entityData, ExtBusinessLogic.UserValue(claim));

            (bool, TEntity) data = ManageOrHandleTransaction(_repo.Update, entityPredicate, entityData, transaction, manageTransaction);

            return (data.Item1, _mapper.Map<TEntityViewModel>(data.Item2));
        }

        public virtual async Task<(bool, TEntityViewModel)> UpdateAsync(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where, TEntityViewModel viewModelData, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);

            entityData = ExtBusinessLogic.GetUpdateEntity(entityData, ExtBusinessLogic.UserValue(claim));

            Task<(bool, TEntity)> dataAsync = ManageOrHandleTransaction(_repo.UpdateAsync, entityPredicate, entityData, transaction, manageTransaction);
            (bool, TEntity) data = await dataAsync;

            return (data.Item1, _mapper.Map<TEntityViewModel>(data.Item2));
        }

        public virtual Task<bool> BulkUpdateAsync(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            IEnumerable<TEntity> entityData = _mapper.Map<IEnumerable<TEntity>>(viewModelData);
            entityData = ExtBusinessLogic.GetBulkUpdateEntity(entityData, ExtBusinessLogic.UserValue(claim));

            Task<bool> data = ManageOrHandleTransaction(_repo.BulkUpdateAsync, entityData, transaction, manageTransaction);

            return data;
        }

        public virtual bool BulkUpdate(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            IEnumerable<TEntity> entityData = _mapper.Map<IEnumerable<TEntity>>(viewModelData);
            entityData = ExtBusinessLogic.GetBulkUpdateEntity(entityData, ExtBusinessLogic.UserValue(claim));

            bool data = ManageOrHandleTransaction(_repo.BulkUpdate, entityData, transaction, manageTransaction);

            return data;
        }
        #endregion 

        #region Delete
        public virtual bool Delete(ClaimsPrincipal claim, TEntityViewModel data, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(data);
            entityData = ExtBusinessLogic.GetDeleteEntity(entityData, ExtBusinessLogic.UserValue(claim));

            return ManageOrHandleTransaction(_repo.Delete, entityData, transaction, manageTransaction);
        }

        public virtual Task<bool> DeleteAsync(ClaimsPrincipal claim, TEntityViewModel data, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(data);
            entityData = ExtBusinessLogic.GetDeleteEntity(entityData, ExtBusinessLogic.UserValue(claim));

            return ManageOrHandleTransaction(_repo.DeleteAsync, entityData, transaction, manageTransaction);
        }

        public virtual bool Delete(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where,
            TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            entityData = ExtBusinessLogic.GetDeleteEntity(entityData, ExtBusinessLogic.UserValue(claim));
            return ManageOrHandleTransaction(_repo.Delete, entityPredicate, entityData, transaction, manageTransaction);
            //return ManageOrHandleTransaction(_repo.Delete, entityPredicate, transaction, manageTransaction);
        }

        public virtual Task<bool> DeleteAsync(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where,
            TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity entityData = _mapper.Map<TEntity>(viewModelData);
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            entityData = ExtBusinessLogic.GetDeleteEntity(entityData, ExtBusinessLogic.UserValue(claim));
            return ManageOrHandleTransaction(_repo.DeleteAsync, entityPredicate, entityData, transaction, manageTransaction);
            //return ManageOrHandleTransaction(_repo.DeleteAsync, entityPredicate, transaction, manageTransaction);
        }

        #endregion 

        #region Count
        public virtual int Count(bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            return ManageOrHandleTransaction(_repo.Count, includeLogicalDeleted, transaction, manageTransaction);
        }

        public virtual int Count(Expression<Func<TEntityViewModel, bool>> where, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            return ManageOrHandleTransaction(_repo.Count, entityPredicate, includeLogicalDeleted, transaction, manageTransaction);
        }

        public virtual int Count(Expression<Func<TEntityViewModel, object>> distinct, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> entityDistinctPredicate = _mapper.Map<Expression<Func<TEntity, object>>>(distinct);
            return ManageOrHandleTransaction(_repo.Count, entityDistinctPredicate, includeLogicalDeleted, transaction, manageTransaction);
        }

        public virtual int Count(Expression<Func<TEntityViewModel, bool>> where, Expression<Func<TEntityViewModel, object>> distinct, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> entityDistinctPredicate = _mapper.Map<Expression<Func<TEntity, object>>>(distinct);
            return ManageOrHandleTransaction(_repo.Count, entityPredicate, entityDistinctPredicate, includeLogicalDeleted, transaction, manageTransaction);
        }

        public virtual Task<int> CountAsync(bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            return ManageOrHandleTransaction(_repo.CountAsync, includeLogicalDeleted, transaction, manageTransaction);
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntityViewModel, bool>> where, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            return ManageOrHandleTransaction(_repo.CountAsync, entityPredicate, includeLogicalDeleted, transaction, manageTransaction);
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntityViewModel, object>> distinct, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> entityDistinctPredicate = _mapper.Map<Expression<Func<TEntity, object>>>(distinct);
            return ManageOrHandleTransaction(_repo.CountAsync, entityDistinctPredicate, includeLogicalDeleted, transaction, manageTransaction);
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntityViewModel, bool>> where, Expression<Func<TEntityViewModel, object>> distinct, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> entityPredicate = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> entityDistinctPredicate = _mapper.Map<Expression<Func<TEntity, object>>>(distinct);
            return ManageOrHandleTransaction(_repo.CountAsync, entityPredicate, entityDistinctPredicate, includeLogicalDeleted, transaction, manageTransaction);
        }
        #endregion

        #region Select
        #region Find
        public virtual TEntityViewModel Find(bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity data = ManageOrHandleTransaction(_repo.Find, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel Find(Expression<Func<TEntityViewModel, bool>> @where, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            TEntity data = ManageOrHandleTransaction(_repo.Find, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel Find<TChild1>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            TEntity data = ManageOrHandleTransaction(_repo.Find<TChild1>, whereEntity, whereChild1, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel Find<TChild1, TChild2>(Expression<Func<TEntityViewModel, bool>> @where,
            Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            TEntity data = ManageOrHandleTransaction(_repo.Find<TChild1, TChild2>, whereEntity, whereChild1,
                whereChild2, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel Find<TChild1, TChild2, TChild3>(Expression<Func<TEntityViewModel, bool>> @where,
            Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);

            TEntity data = ManageOrHandleTransaction(_repo.Find<TChild1, TChild2, TChild3>, whereEntity, whereChild1,
                whereChild2, whereChild3, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel Find<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);

            TEntity data = ManageOrHandleTransaction(_repo.Find<TChild1, TChild2, TChild3, TChild4>, whereEntity, whereChild1,
                whereChild2, whereChild3, whereChild4, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel Find<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);

            TEntity data = ManageOrHandleTransaction(_repo.Find<TChild1, TChild2, TChild3, TChild4, TChild5>, whereEntity, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel Find<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, Expression<Func<TEntityViewModel, object>> tChild6,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            Expression<Func<TEntity, object>> whereChild6 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild6);

            TEntity data = ManageOrHandleTransaction(_repo.Find<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>, whereEntity, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, whereChild6, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync(bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync(Expression<Func<TEntityViewModel, bool>> @where, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync<TChild1>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync<TChild1>, whereEntity, whereChild1, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync<TChild1, TChild2>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync<TChild1, TChild2>, whereEntity, whereChild1,
                whereChild2, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);

            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync<TChild1, TChild2, TChild3>, whereEntity, whereChild1,
                whereChild2, whereChild3, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);

            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync<TChild1, TChild2, TChild3, TChild4>, whereEntity, whereChild1,
                whereChild2, whereChild3, whereChild4, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);

            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5>, whereEntity, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, Expression<Func<TEntityViewModel, object>> tChild6,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            Expression<Func<TEntity, object>> whereChild6 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild6);

            TEntity data = await ManageOrHandleTransaction(_repo.FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>, whereEntity, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, whereChild6, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }
        #endregion

        #region Find By Id
        public virtual TEntityViewModel FindById(object id, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity data = ManageOrHandleTransaction(_repo.FindById, id, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel FindById<TChild1>(object id, Expression<Func<TEntityViewModel, object>> tChild1, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            TEntity data = ManageOrHandleTransaction(_repo.FindById<TChild1>, id, whereChild1, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel FindById<TChild1, TChild2>(object id, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            TEntity data = ManageOrHandleTransaction(_repo.FindById<TChild1, TChild2>, id, whereChild1,
                whereChild2, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel FindById<TChild1, TChild2, TChild3>(object id, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            TEntity data = ManageOrHandleTransaction(_repo.FindById<TChild1, TChild2, TChild3>, id, whereChild1,
                whereChild2, whereChild3, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel FindById<TChild1, TChild2, TChild3, TChild4>(object id, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            TEntity data = ManageOrHandleTransaction(_repo.FindById<TChild1, TChild2, TChild3, TChild4>, id, whereChild1,
                whereChild2, whereChild3, whereChild4, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(object id, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            TEntity data = ManageOrHandleTransaction(_repo.FindById<TChild1, TChild2, TChild3, TChild4, TChild5>, id, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual TEntityViewModel FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, Expression<Func<TEntityViewModel, object>> tChild6,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            Expression<Func<TEntity, object>> whereChild6 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild6);
            TEntity data = ManageOrHandleTransaction(_repo.FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>, id, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, whereChild6, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindByIdAsync(object id, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            TEntity data = await ManageOrHandleTransaction(_repo.FindByIdAsync, id, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindByIdAsync<TChild1>(object id, Expression<Func<TEntityViewModel, object>> tChild1, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            TEntity data = await ManageOrHandleTransaction(_repo.FindByIdAsync<TChild1>, id, whereChild1, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2>(object id, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            TEntity data = await ManageOrHandleTransaction(_repo.FindByIdAsync<TChild1, TChild2>, id, whereChild1,
                whereChild2, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3>(object id, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            TEntity data = await ManageOrHandleTransaction(_repo.FindByIdAsync<TChild1, TChild2, TChild3>, id, whereChild1,
                whereChild2, whereChild3, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(object id, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            TEntity data = await ManageOrHandleTransaction(_repo.FindByIdAsync<TChild1, TChild2, TChild3, TChild4>, id, whereChild1,
                whereChild2, whereChild3, whereChild4, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(object id, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            TEntity data = await ManageOrHandleTransaction(_repo.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>, id, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }

        public virtual async Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, Expression<Func<TEntityViewModel, object>> tChild6,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            Expression<Func<TEntity, object>> whereChild6 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild6);
            TEntity data = await ManageOrHandleTransaction(_repo.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>, id, whereChild1,
                whereChild2, whereChild3, whereChild4, whereChild5, whereChild6, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<TEntityViewModel>(data);
        }
        #endregion

        #region Find All
        public virtual IEnumerable<TEntityViewModel> FindAll(bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            IEnumerable<TEntity> data = ManageOrHandleTransaction(_repo.FindAll, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map< IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAll(Expression<Func<TEntityViewModel, bool>> @where, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            IEnumerable<TEntity> data =
                ManageOrHandleTransaction(_repo.FindAll, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAll<TChild1>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            IEnumerable<TEntity> data = ManageOrHandleTransaction(_repo.FindAll<TChild1>, whereEntity, whereChild1, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            IEnumerable<TEntity> data = ManageOrHandleTransaction(_repo.FindAll<TChild1, TChild2>, whereEntity,
                whereChild1, whereChild2, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            IEnumerable<TEntity> data = ManageOrHandleTransaction(_repo.FindAll<TChild1, TChild2, TChild3>, whereEntity,
                whereChild1, whereChild2, whereChild3, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            IEnumerable<TEntity> data = ManageOrHandleTransaction(_repo.FindAll<TChild1, TChild2, TChild3, TChild4>,
                whereEntity, whereChild1, whereChild2, whereChild3, whereChild4, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            IEnumerable<TEntity> data = ManageOrHandleTransaction(
                _repo.FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>, whereEntity, whereChild1, whereChild2,
                whereChild3, whereChild4, whereChild5, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, Expression<Func<TEntityViewModel, object>> tChild6,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            Expression<Func<TEntity, object>> whereChild6 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild6);
            IEnumerable<TEntity> data = ManageOrHandleTransaction(
                _repo.FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>, whereEntity, whereChild1, whereChild2,
                whereChild3, whereChild4, whereChild5, whereChild6, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync(bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            IEnumerable<TEntity> data = await ManageOrHandleTransaction(_repo.FindAllAsync, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync(Expression<Func<TEntityViewModel, bool>> @where, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            IEnumerable<TEntity> data =
                await ManageOrHandleTransaction(_repo.FindAllAsync, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            IEnumerable<TEntity> data = await ManageOrHandleTransaction(_repo.FindAllAsync<TChild1>, whereEntity, whereChild1, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }
        
        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            IEnumerable<TEntity> data = await ManageOrHandleTransaction(_repo.FindAllAsync<TChild1, TChild2>, whereEntity,
                whereChild1, whereChild2, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            IEnumerable<TEntity> data = await ManageOrHandleTransaction(_repo.FindAllAsync<TChild1, TChild2, TChild3>, whereEntity,
                whereChild1, whereChild2, whereChild3, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1, Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            IEnumerable<TEntity> data = await ManageOrHandleTransaction(_repo.FindAllAsync<TChild1, TChild2, TChild3, TChild4>,
                whereEntity, whereChild1, whereChild2, whereChild3, whereChild4, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            IEnumerable<TEntity> data = await ManageOrHandleTransaction(
                _repo.FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>, whereEntity, whereChild1, whereChild2,
                whereChild3, whereChild4, whereChild5, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntityViewModel, bool>> @where, Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, Expression<Func<TEntityViewModel, object>> tChild3, Expression<Func<TEntityViewModel, object>> tChild4, Expression<Func<TEntityViewModel, object>> tChild5, Expression<Func<TEntityViewModel, object>> tChild6,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            Expression<Func<TEntity, object>> whereChild1 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild1);
            Expression<Func<TEntity, object>> whereChild2 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild2);
            Expression<Func<TEntity, object>> whereChild3 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild3);
            Expression<Func<TEntity, object>> whereChild4 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild4);
            Expression<Func<TEntity, object>> whereChild5 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild5);
            Expression<Func<TEntity, object>> whereChild6 = _mapper.Map<Expression<Func<TEntity, object>>>(tChild6);
            IEnumerable<TEntity> data = await ManageOrHandleTransaction(
                _repo.FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>, whereEntity, whereChild1, whereChild2,
                whereChild3, whereChild4, whereChild5, whereChild6, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }
        #endregion

        #region Find All Between
        public virtual IEnumerable<TEntityViewModel> FindAllBetween(object @from, object to, Expression<Func<TEntityViewModel, object>> btwField, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            IEnumerable<TEntity> data =
                ManageOrHandleTransaction(_repo.FindAllBetween, from, to, btwFieldEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAllBetween(object @from, object to, Expression<Func<TEntityViewModel, object>> btwField, Expression<Func<TEntityViewModel, bool>> @where = null,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            IEnumerable<TEntity> data =
                ManageOrHandleTransaction(_repo.FindAllBetween, from, to, btwFieldEntity, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAllBetween(DateTime @from, DateTime to, Expression<Func<TEntityViewModel, object>> btwField, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            IEnumerable<TEntity> data =
                ManageOrHandleTransaction(_repo.FindAllBetween, from, to, btwFieldEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual IEnumerable<TEntityViewModel> FindAllBetween(DateTime @from, DateTime to, Expression<Func<TEntityViewModel, object>> btwField, Expression<Func<TEntityViewModel, bool>> @where,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            IEnumerable<TEntity> data =
                ManageOrHandleTransaction(_repo.FindAllBetween, from, to, btwFieldEntity, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(object @from, object to, Expression<Func<TEntityViewModel, object>> btwField, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            IEnumerable<TEntity> data =
                await ManageOrHandleTransaction(_repo.FindAllBetweenAsync, from, to, btwFieldEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(object @from, object to, Expression<Func<TEntityViewModel, object>> btwField, Expression<Func<TEntity, bool>> @where,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            IEnumerable<TEntity> data =
                await ManageOrHandleTransaction(_repo.FindAllBetweenAsync, from, to, btwFieldEntity, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(DateTime @from, DateTime to, Expression<Func<TEntityViewModel, object>> btwField, bool includeLogicalDeleted = false, IDbTransaction transaction = null,
            bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            IEnumerable<TEntity> data =
                await ManageOrHandleTransaction(_repo.FindAllBetweenAsync, from, to, btwFieldEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }

        public virtual async Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(DateTime @from, DateTime to, Expression<Func<TEntityViewModel, object>> btwField, Expression<Func<TEntity, bool>> @where,
            bool includeLogicalDeleted = false, IDbTransaction transaction = null, bool manageTransaction = true)
        {
            Expression<Func<TEntity, object>> btwFieldEntity = _mapper.Map<Expression<Func<TEntity, object>>>(btwField);
            Expression<Func<TEntity, bool>> whereEntity = _mapper.Map<Expression<Func<TEntity, bool>>>(where);
            IEnumerable<TEntity> data =
                await ManageOrHandleTransaction(_repo.FindAllBetweenAsync, from, to, btwFieldEntity, whereEntity, includeLogicalDeleted, transaction, manageTransaction);
            return _mapper.Map<IEnumerable<TEntityViewModel>>(data);
        }
        #endregion
        #endregion
    }

    public class CommonStoreProcBusinessLogic<TDbContext> : ICommonStoreProcBusinessLogic<TDbContext>
        where TDbContext : IDapperDbContext
    {
        private readonly TDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<CommonStoreProcBusinessLogic<TDbContext>> _logger;
        private readonly IDapperSProcRepository _spRepo;

        public CommonStoreProcBusinessLogic(TDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CommonStoreProcBusinessLogic<TDbContext>> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;

            var spRepoProperty = _db.GetType().GetProperties()
                .FirstOrDefault(x => x.PropertyType == typeof(IDapperSProcRepository));

            if (spRepoProperty != null)
                _spRepo = (IDapperSProcRepository)spRepoProperty.GetValue(_db);
        }


        private TReturn ManageOrHandleTransaction<TReturn, TData1>(Func<TData1, IDbTransaction, TReturn> func,
            TData1 data1, IDbTransaction transaction, bool manageTransaction)
        {
            if (!manageTransaction)
                return func(data1, null);

            return transaction == null ? HandleTransaction(func, data1) : func(data1, transaction);
        }

        private TReturn HandleTransaction<TReturn, TData>(Func<TData, IDbTransaction, TReturn> repoFunc, TData data)
        {
            TReturn result;
            using (var transaction = _db.BeginTransaction())
            {
                try
                {
                    result = repoFunc(data, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        #region Store Procedure

        public virtual IEnumerable<TDataViewModel> ExecuteStoreProcedure<TData, TParams, TDataViewModel, TParamsViewModel>(
            TParamsViewModel spParam, IDbTransaction transaction = null, bool manageTransaction = true) where TParams : class, ISProcParam
        {
            if (_spRepo != null)
            {
                TParams paramsData = _mapper.Map<TParams>(spParam);
                IEnumerable<TData> spData = ManageOrHandleTransaction(_spRepo.Execute<TData, TParams>, paramsData,
                    transaction, manageTransaction);
                return _mapper.Map<IEnumerable<TDataViewModel>>(spData);
            }

            throw new ArgumentNullException($"Store Procedure Repository is not defined in Database Context");
        }

        public virtual async Task<IEnumerable<TDataViewModel>> ExecuteStoreProcedureAsync<TData, TParams, TDataViewModel, TParamsViewModel>(
            TParamsViewModel spParam, IDbTransaction transaction = null, bool manageTransaction = true) where TParams : class, ISProcParam
        {
            if (_spRepo != null)
            {
                TParams paramsData = _mapper.Map<TParams>(spParam);
                IEnumerable<TData> spData = await ManageOrHandleTransaction(_spRepo.ExecuteAsync<TData, TParams>,
                    paramsData, transaction, manageTransaction);
                return _mapper.Map<IEnumerable<TDataViewModel>>(spData);
            }

            throw new ArgumentNullException($"Store Procedure Repository is not defined in Database Context");
        }

        #endregion
    }
}