using System;
using NUnit.Framework;
using Moq;
using GooglePlus.ApiClient.Contract;
using ActivitiesProvider = GooglePlus.ApiClient.Providers.GoogleActivitiesProvider;
using GooglePlus.ApiClient.Classes;

namespace GooglePlus.ApiClient.Test.GoogleActivitiesProvider
{
    [TestFixture]
    public class GetActivities
    {
        Mock<IRestServiceClient> jsonClient;
        ActivitiesProvider target;

        [SetUp]
        public void Initialize()
        {
            jsonClient = new Mock<IRestServiceClient>();

            target = new ActivitiesProvider
            {
                JsonDataProvider = jsonClient.Object
            };
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_on_null_argument()
        {
            target.GetActivities(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_exception_on_empty_argument()
        {
            target.GetActivities("");
        }

        [Test]
        public void Should_call_json_service_once()
        {
            target.GetActivities("1");

            jsonClient.Verify(c => c.GetData<GooglePlusActivitiesList>(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Should_pass_user_id_as_uri_parameter()
        {
            target.GetActivities("1");

            Predicate<string> paramChecker = p => p != null && p.Contains("/1/activities/public?key=");

            jsonClient.Verify(c => c.GetData<GooglePlusActivitiesList>(It.Is<string>(p => paramChecker(p))));
        }

        [Test]
        public void Should_return_object_from_service()
        {
            GooglePlusActivitiesList serviceResult = new GooglePlusActivitiesList();

            jsonClient.Setup(c => c.GetData<GooglePlusActivitiesList>(It.IsAny<string>())).Returns(serviceResult);

            GooglePlusActivitiesList providerResult = target.GetActivities("1");

            Assert.AreSame(serviceResult, providerResult);
        }

        [Test]
        public void Should_return_null_if_service_returns_null()
        {
            jsonClient.Setup(c => c.GetData<GooglePlusActivitiesList>(It.IsAny<string>())).Returns(() => null);

            GooglePlusActivitiesList providerResult = target.GetActivities("1");

            Assert.IsNull(providerResult);
        }
    }
}
