using AutoMapper;
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
    public class TelephoneControllerTest
    {
        [Fact]
        public void GetTelephone_ValidId_ReturnsObject()
        {
            //Arrange
            Telephone telephone = new Telephone()
            {
                Id = 1,
                Model = "12",
                OperatingSystem = "iOS",
                Price = 45560,
                AvailableAmount = 15,
                ProducerId = 1, Producer = new Producer() { Id = 1, Name = "Apple", CountryOrigin = "USA" }
            };

            TelephoneDTO telephoneDTO = new TelephoneDTO()
            {
                Id = 1,
                Model = "12",
                OperatingSystem = "iOS",
                Price = 45560,
                AvailableAmount = 15,
                ProducerName = "Apple"
            };

            var mockRepository = new Mock<ITelephoneRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(telephone);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new TelephoneProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new TelephoneController(mockRepository.Object, mapper);

            // Act
            var actionResult = controller.GetTelephone(1) as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            TelephoneDTO DTOResult = (TelephoneDTO)actionResult.Value;
            Assert.Equal(telephoneDTO, DTOResult);

        }

        [Fact]
        public void PutTelephone_InvalidId_ReturnsBadRequest()
        {
            //Arrange
            Telephone telephone = new Telephone()
            {
                Id = 3,
                Model = "11",
                OperatingSystem = "iOS",
                AvailableAmount = 17,
                Price = 71290.35m,
                ProducerId = 2,
                Producer = new Producer() { Id = 2, Name = "Apple", CountryOrigin = "USA" }

            };

            var mockRepository = new Mock<ITelephoneRepository>();
            
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new TelephoneProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new TelephoneController(mockRepository.Object, mapper);

            // Act
            var actionResult = controller.PutTelephone(14, telephone) as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public void SearchByPrice_ReturnsCollection()
        {
            //Arrange

            SearchPriceDTO priceDTO = new SearchPriceDTO()
            {
                Min = 40000,
                Max = 72000
            };

            List<Telephone> telephones = new List<Telephone>()
            {
                new Telephone() {Id = 1, Model = "12", OperatingSystem = "iOS", Price = 45560, AvailableAmount = 15, ProducerId = 1, Producer = new Producer() { Id = 1, Name = "Apple", CountryOrigin = "USA" }},
                new Telephone() {Id = 3, Model = "11", OperatingSystem = "iOS", Price = 71290.35m, AvailableAmount = 17, ProducerId = 2, Producer = new Producer() { Id = 2, Name = "Apple", CountryOrigin = "USA" }}
            };

            List<TelephoneDTO> telephonesDTO = new List<TelephoneDTO>()
            {
                new TelephoneDTO() {Id = 1, Model = "12", OperatingSystem = "iOS", Price = 45560, AvailableAmount = 15, ProducerName = "Apple"},
                new TelephoneDTO() {Id = 3, Model = "11", OperatingSystem = "iOS", Price = 71290.35m, AvailableAmount = 17, ProducerName = "Apple"}
            };

            var mockRepository = new Mock<ITelephoneRepository>();

            
            mockRepository.Setup(x => x.Search(priceDTO.Min, priceDTO.Max)).Returns(telephones.AsQueryable());

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new TelephoneProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new TelephoneController(mockRepository.Object, mapper);

            // Act
            var actionResult = controller.SearchByPrice(priceDTO) as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<TelephoneDTO> listResult = (List<TelephoneDTO>)actionResult.Value;

            for (int i = 0; i < listResult.Count; i++)
            {
                Assert.Equal(telephonesDTO[i], listResult[i]);
            }
        }
    }
}
    