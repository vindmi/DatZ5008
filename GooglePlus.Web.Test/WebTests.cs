using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace GooglePlus.Web.Test
{
    [TestClass]
    public class WebTests
    {
        private static string portalLandingPage;
        private IWebDriver webDriver;

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            portalLandingPage = ConfigurationManager.AppSettings["uri"];
        }

        [TestInitialize]
        public void TestSetUp()
        {
            webDriver = new FirefoxDriver();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            webDriver.Quit();
        }

        [TestMethod]
        public void NewUserRegistrationTest()
        {
            var userInfo = GetUserNameAndPassword();

            Register(userInfo.Item1, userInfo.Item2);

            var usernameField = webDriver.FindElement(By.Id("Username"));

            Assert.AreEqual(userInfo.Item1, usernameField.GetAttribute("value"));
        }

        [TestMethod]
        public void UserAdditionalDataOnRegistrationTest()
        {
            var userInfo = GetUserNameAndPassword();

            string birthday = new DateTime(1990, 1, 13).ToShortDateString();
            const string education = "LU, Datorikas fakultāte";
            const string location = "Riga";

            Register(userInfo.Item1, userInfo.Item2, birthday, education, location);

            Assert.AreEqual(birthday, DateTime.Parse(GetValue(webDriver, By.Id("BirthDay"))).ToShortDateString());
            Assert.AreEqual(education, GetValue(webDriver, By.Id("Education")));
            Assert.AreEqual(location, GetValue(webDriver, By.Id("Location")));
        }

        [TestMethod]
        public void UserAdditionalDataChangeTest()
        {
            var userInfo = GetUserNameAndPassword();

            string birthday = new DateTime(1990, 1, 13).ToShortDateString();
            const string education = "LU, Datorikas fakultāte";
            const string location = "Riga";

            Register(userInfo.Item1, userInfo.Item2);

            webDriver.FindElement(By.Id("BirthDay")).SendKeys(birthday);
            webDriver.FindElement(By.Id("Education")).SendKeys(education);
            webDriver.FindElement(By.Id("Location")).SendKeys(location);

            webDriver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();

            Assert.AreEqual(birthday, birthday, DateTime.Parse(GetValue(webDriver, By.Id("BirthDay"))).ToShortDateString());
            Assert.AreEqual(education, GetValue(webDriver, By.Id("Education")));
            Assert.AreEqual(location, GetValue(webDriver, By.Id("Location")));
        }

        private string GetValue(IWebDriver driver, By elementLocator)
        {
            return driver.FindElement(elementLocator).GetAttribute("value");
        }

        private Tuple<string, string> GetUserNameAndPassword()
        {
            var name = Guid.NewGuid().ToString().Replace("-", String.Empty);
            return new Tuple<string, string>(name, name);
        }

        private void Register(
            string username, 
            string password, 
            string birthDay = "",
            string education = "",
            string location = "")
        {
            webDriver.Url = portalLandingPage;

            webDriver.FindElement(By.PartialLinkText("Register")).Click();
            webDriver.FindElement(By.Id("UserName")).SendKeys(username);
            webDriver.FindElement(By.Id("Password")).SendKeys(password);
            webDriver.FindElement(By.Id("ConfirmPassword")).SendKeys(password);
            webDriver.FindElement(By.Id("BirthDay")).SendKeys(birthDay);
            webDriver.FindElement(By.Id("Education")).SendKeys(education);
            webDriver.FindElement(By.Id("Location")).SendKeys(location);

            webDriver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();

            Assert.IsTrue(webDriver.Url.EndsWith("/Users/Main"));
        }
    }
}
