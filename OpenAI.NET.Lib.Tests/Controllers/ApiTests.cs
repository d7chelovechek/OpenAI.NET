using NUnit.Framework;
using OpenAI.NET.Models.Api.Complete;

namespace OpenAI.NET.Lib.Tests.Controllers
{
    /// <summary>
    /// Api controller tests.
    /// </summary>
    [TestFixture]
    public class ApiTests : BaseTest
    {
        /// <summary>
        /// Test initialization.
        /// </summary>
        [SetUp]
        public void ApiSetUp()
        {
            Client.Jwt.Auth(AccessToken);
        }

        /// <summary>
        /// Async Complete test that say OK in English.
        /// </summary>
        [Test]
        public void CompleteSayOKInEnglishTest()
        {
            CompleteResponse response = Client.Api.Complete(
                new CompleteRequest()
                {
                    Engine = "davinci",
                    Prompt = "Say OK!"
                });

            Assert.That(
                response.Completion,
                Is.Not.Empty);
        }

        /// <summary>
        /// Complete test that say OK in Russian.
        /// </summary>
        [Test]
        public void CompleteSayOKInRussianTest()
        {
            CompleteResponse response = Client.Api.Complete(
                new CompleteRequest()
                {
                    Engine = "davinci",
                    Prompt = "Скажи ОК!",
                    RequestLanguage = "ru",
                    ResponseLanguage = "ru"
                });

            Assert.That(
                response.Completion,
                Is.Not.Empty);
        }

        /// <summary>
        /// Complete test without engine.
        /// </summary>
        [Test]
        public void CompleteWithoutEngineParameterTest()
        {
            Exception exception = Assert.Throws<Exception>(
                delegate
                {
                    _ = Client.Api.Complete(
                        new CompleteRequest()
                        {
                            Prompt = "Say OK!"
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Engine can not be empty or null"));
        }

        /// <summary>
        /// Complete test without prompt.
        /// </summary>
        [Test]
        public void CompleteWithoutPromptParameterTest()
        {
            Exception exception = Assert.Throws<Exception>(
                delegate
                {
                    _ = Client.Api.Complete(
                        new CompleteRequest()
                        {
                            Engine = "davinci"
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Prompt can not be empty or null"));
        }

        /// <summary>
        /// Async Complete test that say OK in English.
        /// </summary>
        [Test]
        public async Task CompleteAsyncSayOKInEnglishTest()
        {
            CompleteResponse response =
                await Client.Api.CompleteAsync(
                    new CompleteRequest()
                    {
                        Engine = "davinci",
                        Prompt = "Say OK!"
                    });

            Assert.That(
                response.Completion,
                Is.Not.Empty);
        }

        /// <summary>
        /// Async Complete test that say OK in Russian.
        /// </summary>
        [Test]
        public async Task CompleteAsyncSayOKInRussianTest()
        {
            CompleteResponse response =
                await Client.Api.CompleteAsync(
                    new CompleteRequest()
                    {
                        Engine = "davinci",
                        Prompt = "Скажи ОК!",
                        RequestLanguage = "ru",
                        ResponseLanguage = "ru"
                    });

            Assert.That(
                response.Completion,
                Is.Not.Empty);
        }

        /// <summary>
        /// Async Complete test without engine.
        /// </summary>
        [Test]
        public void CompleteAsyncWithoutEngineParameterTest()
        {
            Exception exception = Assert.ThrowsAsync<Exception>(
                async delegate
                {
                    _ = await Client.Api.CompleteAsync(
                        new CompleteRequest()
                        {
                            Prompt = "Say OK!"
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Engine can not be empty or null"));
        }

        /// <summary>
        /// Async Complete test without prompt.
        /// </summary>
        [Test]
        public void CompleteAsyncWithoutPromptParameterTest()
        {
            Exception exception = Assert.ThrowsAsync<Exception>(
                async delegate
                {
                    _ = await Client.Api.CompleteAsync(
                        new CompleteRequest()
                        {
                            Engine = "davinci"
                        });
                });

            Assert.That(
                exception.Message,
                Is.EqualTo(
                    "Exception in parameters checking: " +
                    "one or more of specified parameters was missing or invalid: " +
                    "Parameter Prompt can not be empty or null"));
        }
    }
}