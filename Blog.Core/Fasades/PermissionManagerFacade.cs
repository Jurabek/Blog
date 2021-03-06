﻿using Blog.Core.Managers;
using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Abstractions.Facades;

namespace Blog.Core.Fasades
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PermissionManagerFacade : IPermissionManagerFacade<IdentityPermission>
    {
        private readonly IdentityPermissionManager _permissionManager;

        public PermissionManagerFacade(IdentityPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public Task AddToRole(IdentityPermission permission, string roleId)
        {
            return _permissionManager.AddToRole(permission, roleId);
        }

        public Task RemoveFromRole(IdentityPermission permission, string roleId)
        {
            return _permissionManager.RemoveFromRole(permission, roleId);
        }

        public Task DeletePermissionAsync(string id)
        {
            return _permissionManager.DeletePermissionAsync(id);
        }

        public Task CreatePermissionAsync(string name, string description, bool isGlobal)
        {
            return _permissionManager.CreatePermissionAsync(name, description, isGlobal);
        }

        public Task<IEnumerable<IdentityPermission>> GetAll()
        {
            return _permissionManager.GetAll();
        }

        public Task<bool> CheckPermission(string name, IList<string> roles, bool iGlobal)
        {
            return _permissionManager.CheckPermissionAsync(name, roles, isGlobal: iGlobal);
        }
    }
}
