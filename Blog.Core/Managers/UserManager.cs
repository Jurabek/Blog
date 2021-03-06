﻿using Blog.Abstractions.Facades;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Blog.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserManagerFacade<User> _userManagerFacade;
        private readonly IUserRepository<User, string, IdentityResult> _userRepository;
        private readonly IEmailManager _emailManager;
        private readonly IMappingManager _mappingManager;
        private readonly IAuthenticationManager<User> _authenticationManager;

        public UserManager(
            IUserManagerFacade<User> userManagerFacade,
            IUserRepository<User, string, IdentityResult> userRepository,
            IEmailManager emailManager,
            IMappingManager mappingManager,
            IAuthenticationManager<User> authenticationManager)
        {
            _userManagerFacade = userManagerFacade;
            _userRepository = userRepository;
            _emailManager = emailManager;
            _mappingManager = mappingManager;
            _authenticationManager = authenticationManager;
        }

        public async Task<IdentityResult> SignUpAndSignIn(IRegisterUserViewModel model)
        {
            var user = _mappingManager.Map<IRegisterUserViewModel, User>(model);
            var result = await _userRepository.AddAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _authenticationManager.SignInAsync(user);
                await _emailManager.SendConfirmationEmail(user.Id);
            }
            return result;
        }

        public async Task<IdentityResult> ResetPassword(IResetPasswordViewModel model)
        {
            var user = await _userManagerFacade.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new IdentityResult("We did not find user, Perhaps it was deleted or blocked, Please inform customer support!");
            }
                
            return await _userManagerFacade.ResetPasswordAsync(user.Id, model.Code, model.Password);
        }

        public async Task<IdentityResult> SendResetPasswordEmail(IForgotPasswordViewModel model)
        {
            var user = await _userRepository.GetByNameAsync(model.Email);
            if (user == null)
                return new IdentityResult($"The {model.Email} not registired to our service!");

            await _emailManager.SendResetPasswordEmail(user.Id);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return Task.FromResult(new IdentityResult("Code or UserId should not be empty"));

            return _userManagerFacade.ConfirmEmailAsync(userId, code);
        }

        public Task<IdentityResult> UpdatePassword(string userId, IUpdatePasswordViewModel model)
        {
            return _userManagerFacade.ChangePassword(userId, model.OldPassword, model.NewPassword);
        }

        public async Task<IdentityResult> UpdateProfile(string userId, IUpdateProfileViewModel model)
        {
            var user = await _userManagerFacade.FindByIdAsync(userId);
            user.Name = model.Name;
            user.LastName = model.LastName;

            return await _userManagerFacade.UpdateAsync(user);
        }

        public Task<IdentityResult> AddToRoleAsync(string userId, string roleName)
        {
            return _userManagerFacade.AddToRoleAsync(userId, roleName);
        }

        public Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles)
        {
            return _userManagerFacade.AddToRolesAsync(userId, roles);
        }

        public Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            return _userManagerFacade.RemoveFromRoleAsync(userId, role);
        }

        public Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles)
        {
            return _userManagerFacade.RemoveFromRolesAsync(userId, roles);
        }

        public Task<bool> IsInRoleAsync(string userId, string roleName)
        {
            return _userManagerFacade.IsInRoleAsync(userId, roleName);
        }

    }
}
