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
        Mock<IRestServiceClient> jsonClient;
        PeopleProvider target;

        [SetUp]
        public void Initialize()
        {
            jsonClient = new Mock<IRestServiceClient>();

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

        [Test]
        public void Should_pass_user_id_as_key_parameter()
        {
            target.GetProfile("1");

            Predicate<string> paramChecker = p => p != null && p.Contains("/1?key=");

            jsonClient.Verify(c => c.GetData<GooglePlusUser>(It.Is<string>(p => paramChecker(p))));
        }

        [Test]
        public void Should_return_object_from_service()
        {
            GooglePlusUser serviceResult = new GooglePlusUser();

            jsonClient.Setup(c => c.GetData<GooglePlusUser>(It.IsAny<string>())).Returns(serviceResult);

            GooglePlusUser providerResult = target.GetProfile("1");

            Assert.AreSame(serviceResult, providerResult);
        }

        [Test]
        public void Should_return_null_if_service_returns_null()
        {
            jsonClient.Setup(c => c.GetData<GooglePlusUser>(It.IsAny<string>())).Returns(() => null);

            GooglePlusUser providerResult = target.GetProfile("1");

            Assert.IsNull(providerResult);
        }
    }
}
