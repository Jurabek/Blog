﻿using System.Web;
using System.Web.Mvc;

namespace Blog.Web
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
