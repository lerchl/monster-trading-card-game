using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Server;
using NUnit.Framework;

namespace MonsterTradingCardGame.Test {

    internal class ApiEndpointRegisterTest {

        [Test]
        public void TestExecute() {
            // Arrange
            ApiEndpointRegister register = new(typeof(DummyApiEndpoint));

            // Act
            register.Execute(new HttpRequest(new Destination(EHttpMethod.GET, "/dummy"), null));

            // Assert
            Assert.IsTrue(DummyApiEndpoint.Invoked);
        }
    }
}
