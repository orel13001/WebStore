using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;


namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_returns_view()
        {
            var product_data_mock = new Mock<IProductData>();
            product_data_mock.Setup(s => s.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(f => Enumerable.Empty<Product>());
            
            var controller = new HomeController();            
            IProductData productData = product_data_mock.Object;
            var result = controller.Index(productData);

            Assert.IsType<ViewResult>(result);
        }


        [TestMethod]
        public void ConfiguredAction_Returns_string_value()
        {
            //A-A-A = Arrange - Act - Assert

            #region Arrange
            const string id = "123";
            //const string value_1 = "qwer";
            const string expected_string = $"Hellow World {id}!";
            #endregion

            #region Act
            var controller = new HomeController();
            var actual_string = controller.ConfiguredAction(id);
            #endregion

            #region Assert
            Assert.Equal(actual_string, expected_string); 
            #endregion
        }



    }
}
