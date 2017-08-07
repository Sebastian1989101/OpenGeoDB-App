using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;
using OpenGeoDB.Core.Services;
using OpenGeoDB.UnitTests.Mocks;

namespace OpenGeoDB.UnitTests.Tests.Repository
{
    [TestFixture]
    public class LocationRepositoryFixture
    {
        private IAppSettings _appSettings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
			Mock<IAppSettings> mock = new Mock<IAppSettings>();
            mock.SetupGet(service => service.NearbyMarkerCount).Returns(10);
            mock.SetupGet(service => service.DistanceType).Returns(DistanceType.Kilometers);

            _appSettings = mock.Object;
        }

        [Test]
        public async Task GetAllAsyncUsage()
        {
            // Arrange
            IDataFileService dataFileService = ServiceMocks.GetMockDataFileService();
            LocationRepository repository = new LocationRepository(dataFileService, _appSettings);

            // Act
            Location[] response = (await repository.GetAllAsync());

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(5, response.Length);

            Assert.AreEqual(7073, response[0].ID);
            Assert.AreEqual("31582", response[0].ZipCode);
            Assert.AreEqual("Nienburg (Weser)", response[0].Village);
            Assert.AreEqual(52.6407898946597, response[0].Latitude);
            Assert.AreEqual(9.23150063371375, response[0].Longitude);

            Assert.AreEqual(7076, response[1].ID);
            Assert.AreEqual("31600", response[1].ZipCode);
            Assert.AreEqual("Uchte", response[1].Village);
            Assert.AreEqual(52.5192716743236, response[1].Latitude);
            Assert.AreEqual(8.87567370960235, response[1].Longitude);

            Assert.AreEqual(10670, response[2].ID);
            Assert.AreEqual("80687", response[2].ZipCode);
            Assert.AreEqual("München", response[2].Village);
            Assert.AreEqual(48.1432006878012, response[2].Latitude);
            Assert.AreEqual(11.5059093215982, response[2].Longitude);

            Assert.AreEqual(10672, response[3].ID);
            Assert.AreEqual("80796", response[3].ZipCode);
            Assert.AreEqual("München", response[3].Village);
            Assert.AreEqual(48.1646490940644, response[3].Latitude);
            Assert.AreEqual(11.5694707183568, response[3].Longitude);

            Assert.AreEqual(10674, response[4].ID);
            Assert.AreEqual("80798", response[4].ZipCode);
            Assert.AreEqual("München", response[4].Village);
            Assert.AreEqual(48.1571679755151, response[4].Latitude);
            Assert.AreEqual(11.5656418013965, response[4].Longitude);
        }

        [Test]
        public async Task GetAllAsyncContainsFailureData()
        {
			// Arrange
			IDataFileService dataFileService = ServiceMocks.GetMockDataFileService(true);
			LocationRepository repository = new LocationRepository(dataFileService, _appSettings);

            // Act
            Location[] response = (await repository.GetAllAsync());

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(5, response.Length);

            Assert.AreEqual(7073, response[0].ID);
            Assert.AreEqual("31582", response[0].ZipCode);
            Assert.AreEqual("Nienburg (Weser)", response[0].Village);
            Assert.AreEqual(52.6407898946597, response[0].Latitude);
            Assert.AreEqual(9.23150063371375, response[0].Longitude);

            Assert.AreEqual(7076, response[1].ID);
            Assert.AreEqual("31600", response[1].ZipCode);
            Assert.AreEqual("Uchte", response[1].Village);
            Assert.AreEqual(52.5192716743236, response[1].Latitude);
            Assert.AreEqual(8.87567370960235, response[1].Longitude);

            Assert.AreEqual(10670, response[2].ID);
            Assert.AreEqual("80687", response[2].ZipCode);
            Assert.AreEqual("München", response[2].Village);
            Assert.AreEqual(48.1432006878012, response[2].Latitude);
			Assert.AreEqual(11.5059093215982, response[2].Longitude);

			Assert.AreEqual(10672, response[3].ID);
			Assert.AreEqual("80796", response[3].ZipCode);
			Assert.AreEqual("München", response[3].Village);
			Assert.AreEqual(48.1646490940644, response[3].Latitude);
			Assert.AreEqual(11.5694707183568, response[3].Longitude);

			Assert.AreEqual(10674, response[4].ID);
			Assert.AreEqual("80798", response[4].ZipCode);
			Assert.AreEqual("München", response[4].Village);
			Assert.AreEqual(48.1571679755151, response[4].Latitude);
			Assert.AreEqual(11.5656418013965, response[4].Longitude);
        }

        [Test]
        public async Task GetAllAsyncLoadFileContentNull()
        {
            // Arrange
            Mock<IDataFileService> mock = new Mock<IDataFileService>();
            mock.Setup(service => service.LoadFileContentAsync()).Returns(Task.FromResult<string>(null));

            LocationRepository repository = new LocationRepository(mock.Object, _appSettings);

            // Act
            IEnumerable<Location> response = await repository.GetAllAsync();

            // Assert
            Assert.IsNull(response);
        }

        [Test]
        public async Task GetAllAsyncNoRelevantContent()
        {
			// Arrange
			Mock<IDataFileService> mock = new Mock<IDataFileService>();
			mock.Setup(service => service.LoadFileContentAsync()).Returns(Task.FromResult("It's not relevant"));

			LocationRepository repository = new LocationRepository(mock.Object, _appSettings);

			// Act
			IEnumerable<Location> response = await repository.GetAllAsync();

			// Assert
			Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Count());
        }

        [Test, SetUICulture("en-US")]
        public void GetAllAsyncLoadFileContentThrowsException()
        {
			// Arrange
			Mock<IDataFileService> mock = new Mock<IDataFileService>();
            mock.Setup(service => service.LoadFileContentAsync()).ThrowsAsync(new Exception("Process failed"));

			LocationRepository repository = new LocationRepository(mock.Object, _appSettings);

            // Act / Assert
            Exception exception = Assert.ThrowsAsync<Exception>(async () => await repository.GetAllAsync());

			Assert.IsNotNull(exception);
            Assert.AreEqual("Process failed", exception.Message);
        }

        [Test]
        public async Task GetNearbyEntriesUsage()
        {
            // Arrange
            IDataFileService dataFileService = ServiceMocks.GetMockDataFileService();
            LocationRepository repository = new LocationRepository(dataFileService, _appSettings);

            Location startLocation = new Location
                {
                    ID = -1, 
                    ZipCode = "31600", 
                    Village = "Uchte", 
                    Latitude = 52.5192716743236, 
                    Longitude = 8.87567370960235
                };

            // Act
            Location[] response = (await repository.GetNearbyEntries(startLocation, 2, true));

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Length);

            Assert.AreEqual(-1, response[0].ID);
            Assert.AreEqual("31600", response[0].ZipCode);
            Assert.AreEqual("Uchte", response[0].Village);
            Assert.AreEqual(52.5192716743236, response[0].Latitude);
            Assert.AreEqual(8.87567370960235, response[0].Longitude);

            Assert.AreEqual(7073, response[1].ID);
            Assert.AreEqual("31582", response[1].ZipCode);
            Assert.AreEqual("Nienburg (Weser)", response[1].Village);
            Assert.AreEqual(52.6407898946597, response[1].Latitude);
            Assert.AreEqual(9.23150063371375, response[1].Longitude);

            Assert.AreEqual(10672, response[2].ID);
            Assert.AreEqual("80796", response[2].ZipCode);
            Assert.AreEqual("München", response[2].Village);
            Assert.AreEqual(48.1646490940644, response[2].Latitude);
            Assert.AreEqual(11.5694707183568, response[2].Longitude);
        }

        [Test]
        public async Task GetNearbyEntriesUsageWithoutItself()
        {
			// Arrange
			IDataFileService dataFileService = ServiceMocks.GetMockDataFileService();
			LocationRepository repository = new LocationRepository(dataFileService, _appSettings);

            Location startLocation = new Location
	            {
	                ID = -1, 
	                ZipCode = "31600", 
	                Village = "Uchte", 
	                Latitude = 52.5192716743236, 
	                Longitude = 8.87567370960235
	            };

			// Act
			Location[] response = (await repository.GetNearbyEntries(startLocation, 2, false));

			// Assert
			Assert.IsNotNull(response);
			Assert.AreEqual(2, response.Length);

			Assert.AreEqual(7073, response[0].ID);
			Assert.AreEqual("31582", response[0].ZipCode);
			Assert.AreEqual("Nienburg (Weser)", response[0].Village);
			Assert.AreEqual(52.6407898946597, response[0].Latitude);
			Assert.AreEqual(9.23150063371375, response[0].Longitude);

			Assert.AreEqual(10672, response[1].ID);
			Assert.AreEqual("80796", response[1].ZipCode);
			Assert.AreEqual("München", response[1].Village);
			Assert.AreEqual(48.1646490940644, response[1].Latitude);
			Assert.AreEqual(11.5694707183568, response[1].Longitude);
		}

		[Test, SetUICulture("en-US")]
		public void GetNearbyEntriesLocationNullException()
		{
			// Arrange
			IDataFileService dataFileService = ServiceMocks.GetMockDataFileService();
			LocationRepository repository = new LocationRepository(dataFileService, _appSettings);

            // Act / Assert
            NullReferenceException exception = Assert.ThrowsAsync<NullReferenceException>(async () => await repository.GetNearbyEntries(null));

			Assert.IsNotNull(exception);
			Assert.AreEqual(typeof(NullReferenceException), exception.GetType());
			Assert.IsTrue(exception.Message.StartsWith("Object reference not set to an instance of an object", StringComparison.Ordinal));
		}
    }
}
