﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Abstractions.Managers;
using Blog.Abstractions.Repositories;
using Blog.Abstractions.ViewModels;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using Blog.Web.Controllers;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using static Blog.Web.Controllers.ManageAccountController;
using Blog.Abstractions.Facades;

namespace Blog.Web.Tests.Controllers
{
    [TestFixture]
    public class ManageAccountControllerTest : BaseControllerTest<ManageAccountController>
    {
        private Mock<IUserRepository<User, string, IdentityResult>> _repository;
        private Mock<IUserManager> _userManager;
        private Mock<IMappingManager> _mappingManager;
        private Mock<IUrlHelperFacade> _urlHelperFacade;

        public override void Init()
        {
            _repository = new Mock<IUserRepository<User, string, IdentityResult>>();
            _userManager = new Mock<IUserManager>();
            _mappingManager = new Mock<IMappingManager>();
            _urlHelperFacade = new Mock<IUrlHelperFacade>();
            _controller = new ManageAccountController(_repository.Object,
                                                        _userManager.Object, 
                                                        _mappingManager.Object,
                                                        _urlHelperFacade.Object);
        }

        [Test]
        public void IndexActionTest()
        {
            ClearModelState();

            var result = _controller.Index(ManageMessageId.ChangePasswordSuccess) as ViewResult;

            Assert.That(result.ViewBag.StatusMessage, Is.EqualTo("Your password has been changed."));

            result = _controller.Index(ManageMessageId.ChangeProfileSuccess) as ViewResult;

            Assert.That(result.ViewBag.StatusMessage, Is.EqualTo("Your profile has been changed"));

            result = _controller.Index(ManageMessageId.Error) as ViewResult;

            Assert.That(result.ViewBag.StatusMessage, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ChangePasswordActionTest()
        {
            var result = _controller.ChangePassword();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ChangePasswordShouldReturnModelErrors()
        {
            ClearModelState();

            string errorMessage = "Old password is required";
            _controller.ModelState.AddModelError("",errorMessage);

            var result = await _controller.ChangePassword(new UpdatePasswordViewModel { OldPassword = string.Empty }) as ViewResult;
            
            Assert.AreEqual(errorMessage, result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage);

            Assert.AreEqual(string.Empty, ((UpdatePasswordViewModel)result.ViewData.Model).OldPassword);
        }

        [Test]
        public async Task ChangePasswordShouldRedirectToIndexWhenSuccess()
        {
            ClearModelState();
            _userManager.Setup(r => r.UpdatePassword(It.IsAny<string>(), It.IsAny<IUpdatePasswordViewModel>()))
                       .Returns(Task.FromResult(IdentityResult.Success));

            var result = await _controller.ChangePassword(null) as RedirectToRouteResult;

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));

            Assert.That(result.RouteValues["Message"], Is.EqualTo(ManageMessageId.ChangePasswordSuccess));
        }

        [Test]
        public async Task ChangePasswordShouldCreateModelErroresWhenNotSuccess()
        {
            ClearModelState();
            string errorMessage = "Can not change the password!";
            _userManager.Setup(r => r.UpdatePassword(It.IsAny<string>(), It.IsAny<IUpdatePasswordViewModel>()))
                       .Returns(Task.FromResult(new IdentityResult(errorMessage)));
            
            var result = await _controller.ChangePassword(null) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task ChangeProfileActionTest()
        {
            ClearModelState();

            string name = "Test_Name";
            string lastName = "Test_LastName";

            _repository.Setup(r => r.GetAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult(new User()
                              {
                                 Name = name,
                                 LastName = lastName
                              }));

            _mappingManager.Setup(x => x.Map<User, UpdateProfileViewModel>(It.IsAny<User>()))
                            .Returns(new UpdateProfileViewModel
                            {
                                Name = name,
                                LastName = lastName
                            });

            var result = await _controller.ChangeProfile() as ViewResult;
            Assert.IsInstanceOf<UpdateProfileViewModel>(result.ViewData.Model);

            var model = result.ViewData.Model as UpdateProfileViewModel;

            Assert.That(model.Name, Is.EqualTo(name));

            Assert.That(model.LastName, Is.EqualTo(lastName));
        }
        

        [Test]
        public async Task ChangeProfileShouldCreateModelErrorsWhenValidationFailes()
        {
            ClearModelState();

            string errorMessage = "Name and Last name are required!";
            _controller.ModelState.AddModelError("",  errorMessage);

            var model = new UpdateProfileViewModel
            {
                Name = "",
                LastName = ""
            };

            var result = await _controller.ChangeProfile(model) as ViewResult;

            Assert.AreSame(model, result.ViewData.Model);

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                 Is.EqualTo(errorMessage));
        }

        [Test]
        public async Task ChangeProfileShouldRedirectToIndexWhenSuccess()
        {
            ClearModelState();

            _userManager.Setup(r => r.UpdateProfile(It.IsAny<string>(), It.IsAny<IUpdateProfileViewModel>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
            
            var result = await _controller.ChangeProfile(new UpdateProfileViewModel()) as RedirectToRouteResult;

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));

            Assert.That(result.RouteValues["Message"], Is.EqualTo(ManageMessageId.ChangeProfileSuccess));
        }
        
        [Test]
        public async Task ChangeProfileShouldCreateModelErrorsWhenNotSuccess()
        {
            ClearModelState();

            string errorMessage = "Can not update profile!";

            _userManager.Setup(r => r.UpdateProfile(It.IsAny<string>(), It.IsAny<IUpdateProfileViewModel>()))
                       .Returns(Task.FromResult(new IdentityResult(errorMessage)));

            var result = await _controller.ChangeProfile(null) as ViewResult;

            Assert.That(result.ViewData.ModelState.Values.First().Errors.First().ErrorMessage, 
                                                                                             Is.EqualTo(errorMessage));
        } 
    }
}
