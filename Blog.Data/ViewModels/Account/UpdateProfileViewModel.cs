﻿using Blog.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class UpdateProfileViewModel : IUpdateProfileViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
