using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;

namespace OpenGeoDB.UnitTests.Tests.Repository
{
    [TestFixture]
    public class LocationRepositoryFixture
    {
        [Test]
        public async Task Usage()
        {
            // Arrange
            LocationRepository repository = new LocationRepository(new FakeDataFileService());

            // Act
            Location[] response = (await repository.GetAllAsync()).ToArray();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Length);

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
        }

        [Test]
        public async Task ContainsFailureData()
        {
			// Arrange
			LocationRepository repository = new LocationRepository(new FakeDataFileService(true));

            // Act
            Location[] response = (await repository.GetAllAsync()).ToArray();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Length);

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
        }

        [Test]
        public async Task LoadFileContentNull()
        {
            // Arrange
            Mock<IDataFileService> mock = new Mock<IDataFileService>();
            mock.Setup(service => service.LoadFileContentAsync()).Returns(Task.FromResult<string>(null));

            LocationRepository repository = new LocationRepository(mock.Object);

            // Act
            IEnumerable<Location> response = await repository.GetAllAsync();

            // Assert
            Assert.IsNull(response);
        }

        [Test]
        public async Task NoRelevantContent()
        {
			// Arrange
			Mock<IDataFileService> mock = new Mock<IDataFileService>();
			mock.Setup(service => service.LoadFileContentAsync()).Returns(Task.FromResult("It's not relevant"));

			LocationRepository repository = new LocationRepository(mock.Object);

			// Act
			IEnumerable<Location> response = await repository.GetAllAsync();

			// Assert
			Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Count());
        }

        [Test]
        public void LoadFileContentThrowsException()
        {
			// Arrange
			Mock<IDataFileService> mock = new Mock<IDataFileService>();
            mock.Setup(service => service.LoadFileContentAsync()).ThrowsAsync(new Exception("Process failed"));

			LocationRepository repository = new LocationRepository(mock.Object);

            // Act / Assert
            Exception exception = Assert.ThrowsAsync<Exception>(async () => await repository.GetAllAsync());

			Assert.IsNotNull(exception);
            Assert.AreEqual("Process failed", exception.Message);
        }

        private class FakeDataFileService : IDataFileService
        {
            private readonly bool _addFailureLine;

            public FakeDataFileService(bool addFailureLine = false)
            {
                _addFailureLine = addFailureLine;
            }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            public async Task<string> LoadFileContentAsync()
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("#loc_id\tplz\tlon\tlat\tOrt");
                builder.AppendLine("7073\t31582\t9.23150063371375\t52.6407898946597\tNienburg (Weser)");
                builder.AppendLine("7076\t31600\t8.87567370960235\t52.5192716743236\tUchte");

                if(_addFailureLine)
                    builder.AppendLine("10670\t80X687\t11.50X59093215982\t48.1432006878X012\tMünchen");

                builder.AppendLine("10670\t80687\t11.5059093215982\t48.1432006878012\tMünchen");

                return builder.ToString();
			}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		}
    }
}
