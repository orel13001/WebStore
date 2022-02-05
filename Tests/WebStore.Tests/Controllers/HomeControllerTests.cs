using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using WebStore.Controllers;
using Assert = Xunit.Assert;


namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void ConfiguredAction_Returns_string_value()
        {
            const string id = "123";
            //const string value_1 = "qwer";
            const string expected_string = $"Hellow World {id}!";

            var controller = new HomeController();
            var actual_string = controller.ConfiguredAction(id);

            Assert.Equal(actual_string, expected_string);
        }
    }
}
