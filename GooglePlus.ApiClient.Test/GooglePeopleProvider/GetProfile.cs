using System;
using NUnit.Framework;
using GooglePlus.ApiClient.Contract;
using Moq;
using PeopleProvider = GooglePlus.ApiClient.Providers.GooglePeopleProvider;
using GooglePlus.ApiClient.Classes;

namespace GooglePlus.ApiClient.Test.GooglePeopleProvider
{
    [TestFixture]
    public class GetProfile
    {
        Mock<IJsonServiceClient> jsonClient;
        PeopleProvider target;

        [SetUp]
        public void Initialize()
        {
            jsonClient = new Mock<IJsonServiceClient>();

            target = new PeopleProvider
            {
                JsonDataProvider = jsonClient.Object
            };
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_on_null_argument()
        {
            target.GetProfile(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_exception_on_empty_argument()
        {
            target.GetProfile("");
        }

        [Test]
        public void Should_call_json_service_once()
        {
            target.GetProfile("1");

            jsonClient.Verify(c => c.GetData<GooglePlusUser>(It.IsAny<string>()), Times.Once());
        }
    }
}
