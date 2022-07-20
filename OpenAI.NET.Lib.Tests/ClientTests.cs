using NUnit.Framework;

namespace OpenAI.NET.Lib.Tests
{
    /// <summary>
    /// Client factory tests.
    /// </summary>
    [TestFixture]
    public class ClientTests : BaseTest
    {
        /// <summary>
        /// Init Client with address of OpenAI.NET.Web.
        /// </summary>
        [Test]
        public void InitClientWithAddressTest()
        {
            Client client = new(Address);
            
            Assert.Multiple(() =>
            {
                Assert.That(client.Api, Is.Not.Null);
                Assert.That(client.Jwt, Is.Not.Null);
            });

            Assert.That(
                client.Address,
                Is.EqualTo(Address));
        }

        /// <summary>
        /// Init Client with address of OpenAI.NET.Web and access token.
        /// </summary>
        [Test]
        public void InitClientWithAddressAndAccessTokenTest()
        {
            Client client = new(Address, AccessToken);

            Assert.Multiple(() =>
            {
                Assert.That(client.Api, Is.Not.Null);
                Assert.That(client.Jwt, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(
                    client.Address,
                    Is.EqualTo(Address));
                Assert.That(
                    client.AccessToken,
                    Is.EqualTo(AccessToken));
            });
        }
    }
}