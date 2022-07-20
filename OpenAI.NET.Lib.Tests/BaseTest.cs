using NUnit.Framework;

namespace OpenAI.NET.Lib.Tests
{
    /// <summary>
    /// Base test class that initializes main properties.
    /// </summary>
    [TestFixture]
    public class BaseTest
    {
        /// <summary>
        /// Address of OpenAI.NET.Web.
        /// </summary>
        public string Address { get; private set; }
        /// <summary>
        /// Access token for authorization in OpenAI.NET.Web.
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Instance of Client.
        /// </summary>
        public Client Client { get; private set; }

        /// <summary>
        /// Test initialization.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Address =
                TestContext.Parameters["OpenAI.NET.Web.Address"];
            AccessToken = TestContext.Parameters["AccessToken"];

            Client = new Client(Address);
        }
    }
}