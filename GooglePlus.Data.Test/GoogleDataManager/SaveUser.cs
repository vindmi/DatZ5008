using System;
using GooglePlus.Data.Model;
using Moq;
using NUnit.Framework;
using GooglePlus.Data.Contract;

namespace GooglePlus.Data.Test.GoogleDataManager
{
    [TestFixture]
    public class SaveUser
    {
        Mock<IGoogleDataAdapter> dataAdapter;
        Managers.GoogleDataManager target;

        [SetUp]
        public void Initialize()
        {
            dataAdapter = new Mock<IGoogleDataAdapter>();

            target = new Managers.GoogleDataManager(dataAdapter.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_on_null_argument()
        {
            target.SaveUser(null);
        }

        [Test]
        public void Should_call_data_adapter_save_user()
        {
            target.SaveUser(new User());

            dataAdapter.Verify(a => a.SaveUser(It.IsAny<User>()), Times.Once());
        }
    }
}
