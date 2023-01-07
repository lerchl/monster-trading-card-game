using System;
using MonsterTradingCardGame.Api.Endpoints.Users;
using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;
using Moq;
using NUnit.Framework;

namespace MonsterTradingCardGame.Test.Logic {

    internal class UserLogicTest {

        private static readonly User _user = new(Guid.NewGuid(), "username", "password", UserRole.Regular, 0, "name", "bio", "image", 0);
        private static readonly Token _token = new(_user.Id, _user.Username, UserRole.Regular);
        private static readonly Token _differentToken = new(Guid.NewGuid(), "different", UserRole.Regular);
        private static readonly Token _adminToken = new(Guid.NewGuid(), "admin", UserRole.Admin);
        private static readonly UserInfoVO _newUserInfo = new("newName", "newBio", "newImage");

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [Test]
        public void TestRegister() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindByUsername(_user.Username)).Throws(new NoResultException(""));
            userRepositoryMock.Setup(r => r.Save(_user)).Returns(_user);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act
            userLogic.Register(_user);

            // Assert
            userRepositoryMock.Verify(r => r.Save(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void TestRegisterUsernameTaken() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindByUsername(_user.Username)).Returns(_user);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act / Assert
            var e = Assert.Throws<ConflictException>(() => userLogic.Register(_user))!;
            Assert.AreEqual("Username already taken", e.Message);
        }

        [Test]
        public void TestLogin() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindByUsername(_user.Username)).Returns(_user);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act / Assert
            Assert.DoesNotThrow(() => userLogic.Login(_user));
        }

        [Test]
        public void TestLoginWrongUsername() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindByUsername(_user.Username)).Throws(new NoResultException(""));
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act / Assert
            var e = Assert.Throws<UnauthorizedException>(() => userLogic.Login(_user))!;
            Assert.AreEqual("Invalid username or password", e.Message);
        }

        [Test]
        public void TestLoginWrongPassword() {
            // Arrange
            User dbUser = new(_user.Id, _user.Username, "different", UserRole.Regular, 0, "name", "bio", "image", 0);
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindByUsername(_user.Username)).Returns(dbUser);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act / Assert
            var e = Assert.Throws<UnauthorizedException>(() => userLogic.Login(_user))!;
            Assert.AreEqual("Invalid username or password", e.Message);
        }

        [Test]
        public void TestGetInfo() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindByUsername(_user.Username)).Returns(_user);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act
            UserInfoVO userInfoVO = userLogic.GetInfo(_token, _user.Username);

            // Assert
            Assert.AreEqual(_user.Name, userInfoVO.Name);
            Assert.AreEqual(_user.Bio, userInfoVO.Bio);
            Assert.AreEqual(_user.Image, userInfoVO.Image);
        }

        [Test]
        public void TestGetInfoOtherUser() {
            // Arrange
            UserLogic userLogic = new();

            // Act / Assert
            var e = Assert.Throws<ForbiddenException>(() => userLogic.GetInfo(_differentToken, _user.Username))!;
            Assert.AreEqual("You can only access your own data", e.Message);
        }

        [Test]
        public void TestGetInfoOtherUserAsAdmin() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindByUsername(_user.Username)).Returns(_user);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act
            UserInfoVO userInfoVO = userLogic.GetInfo(_adminToken, _user.Username);

            // Assert
            Assert.AreEqual(_user.Name, userInfoVO.Name);
            Assert.AreEqual(_user.Bio, userInfoVO.Bio);
            Assert.AreEqual(_user.Image, userInfoVO.Image);
        }

        [Test]
        public void TestSetInfo() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindById(_user.Id)).Returns(_user);
            userRepositoryMock.Setup(r => r.Save(_user)).Returns(_user);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act
            UserInfoVO returnedUserInfoVO = userLogic.SetInfo(_token, _user.Username, _newUserInfo);

            // Assert
            Assert.AreEqual(_newUserInfo.Name, returnedUserInfoVO.Name);
            Assert.AreEqual(_newUserInfo.Bio, returnedUserInfoVO.Bio);
            Assert.AreEqual(_newUserInfo.Image, returnedUserInfoVO.Image);
        }

        [Test]
        public void TestSetInfoOtherUser() {
            // Arrange
            UserLogic userLogic = new();

            // Act / Assert
            var e = Assert.Throws<ForbiddenException>(() => userLogic.SetInfo(_differentToken, _user.Username, _newUserInfo))!;
            Assert.AreEqual("You can only edit your own data", e.Message);
        }

        [Test]
        public void TestSetInfoOtherUserAsAdmin() {
            // Arrange
            Mock<UserRepository> userRepositoryMock = new() { CallBase = true };
            userRepositoryMock.Setup(r => r.FindById(_user.Id)).Returns(_user);
            userRepositoryMock.Setup(r => r.Save(_user)).Returns(_user);
            UserLogic userLogic = new(userRepositoryMock.Object);

            // Act
            UserInfoVO returnedUserInfoVO = userLogic.SetInfo(_adminToken, _user.Username, _newUserInfo);

            // Assert
            Assert.AreEqual(_newUserInfo.Name, returnedUserInfoVO.Name);
            Assert.AreEqual(_newUserInfo.Bio, returnedUserInfoVO.Bio);
            Assert.AreEqual(_newUserInfo.Image, returnedUserInfoVO.Image);
        }
    }
}
