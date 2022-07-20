using NUnit.Framework;
using OpenAI.NET.Models.Jwt.Auth;

namespace OpenAI.NET.Lib.Tests.Controllers
{
    /// <summary>
    /// Jwt controller tests.
    /// </summary>
    [TestFixture]
    public class JwtTests : BaseTest
    {
        private AuthRequest _authRequest;

        /// <summary>
        /// Test initialization.
        /// </summary>
        [SetUp]
        public void JwtSetUp()
        {
            _authRequest = new AuthRequest()
            {
                Name = TestContext.Parameters["UserName"],
                Password = TestContext.Parameters["Password"]
            };
        }

        /// <summary>
        /// Auth test with access token.
        /// </summary>
        [Test]
        public void AuthWithAccessTokenTest()
        {
            Client.Jwt.Auth(AccessToken);

            Assert.That(
                Client.AccessToken,
                Is.EqualTo(AccessToken));
        }

        /// <summary>
        /// Auth test with data of existent user.
        /// </summary>
        [Test]
        public void AuthWithDataOfExistentUserTest()
        {
            AuthResponse response =
                Client.Jwt.Auth(_authRequest);

            Assert.That(
                response.Name,
                Is.EqualTo(_authRequest.Name));
        }

        /// <summary>
        /// Auth test with data of non-existent user.
        /// </summary>
        [Test]
        public void AuthWithDataOfNonExistentUserTest()
        {
            string guid = Guid.NewGuid().ToString();

            Exception exception = Assert.Throws<Exception>(
                delegate
                {
                    _ = Client.Jwt.Auth(
                        new AuthRequest()
                        {
                            Name = "NonExistentUser" + guid,
                            Password = guid
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in user authorization: " +
                    "There's no user with such data"));
        }

        /// <summary>
        /// Auth test without user name.
        /// </summary>
        [Test]
        public void AuthWithoutUserNameTest()
        {
            Exception exception = Assert.Throws<Exception>(
                delegate
                {
                    _ = Client.Jwt.Auth(
                        new AuthRequest()
                        {
                            Password = _authRequest.Password
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Name can not be empty or null"));
        }

        /// <summary>
        /// Auth test without password.
        /// </summary>
        [Test]
        public void AuthWithoutPasswordTest()
        {
            Exception exception = Assert.Throws<Exception>(
                delegate
                {
                    _ = Client.Jwt.Auth(
                        new AuthRequest()
                        {
                            Name = _authRequest.Name
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Password can not be empty or null"));
        }

        /// <summary>
        /// Async Auth test with data of existent user.
        /// </summary>
        [Test]
        public async Task AuthAsyncWithDataOfExistentUserTest()
        {
            AuthResponse response =
                await Client.Jwt.AuthAsync(_authRequest);

            Assert.That(
                response.Name,
                Is.EqualTo(_authRequest.Name));
        }

        /// <summary>
        /// Async Auth test with data of non-existent user.
        /// </summary>
        [Test]
        public void AuthAsyncWithDataOfNonExistentUserTest()
        {
            string guid = Guid.NewGuid().ToString();

            Exception exception = Assert.ThrowsAsync<Exception>(
                async delegate
                {
                    _ = await Client.Jwt.AuthAsync(
                        new AuthRequest()
                        {
                            Name = "NonExistentUser" + guid,
                            Password = guid
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in user authorization: " +
                    "There's no user with such data"));
        }

        /// <summary>
        /// Async Auth test without user name.
        /// </summary>
        [Test]
        public void AuthAsyncWithoutUserNameTest()
        {
            Exception exception = Assert.ThrowsAsync<Exception>(
                async delegate
                {
                    _ = await Client.Jwt.AuthAsync(
                        new AuthRequest()
                        {
                            Password = _authRequest.Password
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Name can not be empty or null"));
        }

        /// <summary>
        /// Async Auth test without password.
        /// </summary>
        [Test]
        public void AuthAsyncWithoutPasswordTest()
        {
            Exception exception = Assert.ThrowsAsync<Exception>(
                async delegate
                {
                    _ = await Client.Jwt.AuthAsync(
                        new AuthRequest()
                        {
                            Name = _authRequest.Name
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Password can not be empty or null"));
        }
    }
}