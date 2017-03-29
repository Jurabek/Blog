﻿using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Managers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class IdentityRoleManager : RoleManager<IdentityRole, string>
    {
        public IdentityRoleManager(IdentityRoleStore store) : base(store)
        {
        }
    }

    /// <summary>
    /// PermissionManager Object
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class IdentityPermissionManager : IdentityPermissionExtension.PermissionManager<string, IdentityPermission, IdentityRolePermission>
    {
        public IdentityPermissionManager(IdentityPermissionStore store) : base(store)
        {
        }
    }
}
