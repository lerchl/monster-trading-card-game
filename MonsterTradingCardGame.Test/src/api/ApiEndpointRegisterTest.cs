using System;
using System.Collections.Generic;
using System.IO;
using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Server;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MonsterTradingCardGame.Test {

    internal class ApiEndpointRegisterTest {

        [Test]
        public void TestExecute() {
            // Arrange
            ApiEndpointRegister register = new(typeof(DummyApiEndpoint));

            Guid userId = Guid.NewGuid();
            User user = new(userId, "test", "test", UserRole.Regular, 0, "", "", "", 0);
            Token token = SessionHandler.Instance.CreateSession(userId, "test", user.Role);
            Dictionary<string, string> headers = new() { { "Authorization", token.Bearer } };

            Destination destination = new(EHttpMethod.GET, "/dummy/anotherUser?format=plain");
            string json = System.Text.Json.JsonSerializer.Serialize(user);
            JsonTextReader body = new(new StringReader(json));
            HttpRequest httpRequest = new(destination, headers, body);

            // Act
            Response res = register.Execute(httpRequest);

            // Assert
            Assert.AreEqual(HttpCode.OK_200, res.HttpCode);
            Assert.AreEqual(userId + "anotherUser" + "plain" + user.Username, res.Body);
        }

        [Test]
        public void TestExecuteNoUrl() {
            // Arrange
            string message = ApiEndpointRegister.URL_EXCEPTION_MESSAGE
                    .Replace("{methodName}", "NoUrlTestEndpoint")
                    .Replace("{httpMethodName}", "GET");

            // Act / Assert
            var e = Assert.Throws<ProgrammerFailException>(() => new ApiEndpointRegister(typeof(NoUrlDummyApiEndpoint)))!;
            Assert.AreEqual(message, e.Message);
        }

        [Test]
        public void TestExecuteNoResponse() {
            // Arrange
            string message = ApiEndpointRegister.RETURN_TYPE_EXCEPTION_MESSAGE
                    .Replace("{methodName}", "NoResponseTestEndpoint")
                    .Replace("{httpMethodName}", "GET")
                    .Replace("{url}", "/dummy");

            // Act / Assert
            var e = Assert.Throws<ProgrammerFailException>(() => new ApiEndpointRegister(typeof(NoResponseDummyApiEndpoint)))!;
            Assert.AreEqual(message, e.Message);
        }
    }
}
