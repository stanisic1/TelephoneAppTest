using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneApp.Controllers;
using TelephoneApp.Interfaces;
using TelephoneApp.Models;
using Xunit;

namespace TelephoneAppTest.Controllers
{
    public class ProducerControllerTest
    {
        [Fact]
        public void GetProducer_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IProducerRepository>();

            var controller = new ProducerController(mockRepository.Object);

            // Act
            var actionResult = controller.GetProducer(12) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }
        [Fact]
        public void GetProducers_ReturnsCollection()
        {
            //Arrange
            List<Producer> producers = new List<Producer>()
            {
                new Producer() {Id = 1, Name = "Apple", CountryOrigin = "USA"},
                new Producer() {Id = 2, Name = "Huawei", CountryOrigin = "China"}
            };

            var mockRepository = new Mock<IProducerRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(producers.AsQueryable());

            var controller = new ProducerController(mockRepository.Object);

            // Act
            var actionResult = controller.GetProducers() as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<Producer> results = (List<Producer>)actionResult.Value;

            for(int i = 0; i < results.Count; i++)
            {
                Assert.Equal(producers[i], results[i]);
            }

        }
    }
}
