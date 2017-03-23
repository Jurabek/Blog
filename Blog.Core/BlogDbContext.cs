﻿using Blog.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Core
{
    public class BlogDbContext : IdentityDbContext<User>
    {
        public BlogDbContext() : base(Settings.ConnectionString, throwIfV1Schema: false)
        {            
        }        
    }
}