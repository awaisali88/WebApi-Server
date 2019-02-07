using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Common;
using Common.Exception;
using Common.Messages;
using Dapper.Identity.Stores;
using Dapper.Repositories;
using Microsoft.AspNetCore.Http;

namespace WebAPI_BAL
{
    public static class ExtBusinessLogic
    {
        public static string UserValue(ClaimsPrincipal claim, string value = nameof(ApplicationUser.Id))
        {
            return claim.Claims.Any() ? claim.Claims.Single(c => c.Type == value.ToLower()).Value : "";
        }

        internal static void CheckRecord<TDefaultColumns>(TDefaultColumns data) where TDefaultColumns : IDefaultColumns
        {
            if (data == null)
                throw new GcsApplicationException(StatusCodes.Status404NotFound, ErrorMessages.RecordNotFound);
            if (!data.Status)
                throw new GcsApplicationException(StatusCodes.Status403Forbidden, ErrorMessages.InActiveRecord);
            if (data.Trashed)
                throw new GcsApplicationException(StatusCodes.Status405MethodNotAllowed, ErrorMessages.TrashedRecord);
        }

        internal static TEntity GetInsertEntity<TEntity>(TEntity data, string userId) where TEntity : class, IDefaultColumns
        {
            data.CreatedBy = userId;
            data.ModifiedBy = userId;
            data.Status = true;
            data.Trashed = false;
            data.RecordStatus = RecordStatus.NewMode;
            return data;
        }

        internal static IEnumerable<TEntity> GetBulkInsertEntity<TEntity>(IEnumerable<TEntity> data, string userId) where TEntity : class, IDefaultColumns
        {
            foreach (var entity in data)
            {
                entity.CreatedBy = userId;
                entity.ModifiedBy = userId;
                entity.Status = true;
                entity.Trashed = false;
                entity.RecordStatus = RecordStatus.NewMode;
            }
            return data;
        }

        internal static TEntity GetUpdateEntity<TEntity>(TEntity data, string userId) where TEntity : class, IDefaultColumns
        {
            data.ModifiedBy = userId;
            data.RecordStatus = RecordStatus.EditMode;
            return data;
        }

        internal static IEnumerable<TEntity> GetBulkUpdateEntity<TEntity>(IEnumerable<TEntity> data, string userId) where TEntity : class, IDefaultColumns
        {
            foreach (var entity in data)
            {
                entity.ModifiedBy = userId;
                entity.RecordStatus = RecordStatus.EditMode;
            }
            return data;
        }

        internal static TEntity GetDeleteEntity<TEntity>(TEntity data, string userId) where TEntity : class, IDefaultColumns
        {
            data.ModifiedBy = userId;
            data.Status = false;
            data.Trashed = true;
            data.RecordStatus = RecordStatus.DeleteMode;
            return data;
        }
    }
}
