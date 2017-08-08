using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;
using OpenGeoDB.Core.Services;
using OpenGeoDB.Core.ViewModels;
using OpenGeoDB.UnitTests.Mocks;

namespace OpenGeoDB.UnitTests.Tests.ViewModels
{
    [TestFixture]
    public class MainViewModelFixture
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
        public async Task LoadingDataUsage()
        {
            // Arrange
            LocationRepository locationRepository = new LocationRepository(ServiceMocks.GetMockDataFileService(), _appSettings);

            Mock<IAppSettings> mockAppSettings = new Mock<IAppSettings>();
            mockAppSettings.Setup(settings => settings.OrderByZipCode).Returns(false);

            MainViewModel viewModel = new MainViewModel(locationRepository, mockAppSettings.Object);

            // Act
            await viewModel.Initialize();
            viewModel.ViewAppearing();

            // Assert
            Assert.IsNotNull(viewModel.Data);
			Assert.AreEqual(3, viewModel.Data.Count());

			Assert.AreEqual("München", viewModel.Data[0].Key);
			Assert.AreEqual(3, viewModel.Data[0].Count());

			Assert.AreEqual(10670, viewModel.Data[0].First().ID);
			Assert.AreEqual("80687", viewModel.Data[0].First().ZipCode);
			Assert.AreEqual("München", viewModel.Data[0].First().Village);
			Assert.AreEqual(48.1432006878012, viewModel.Data[0].First().Latitude);
			Assert.AreEqual(11.5059093215982, viewModel.Data[0].First().Longitude);

			Assert.AreEqual(10674, viewModel.Data[0].Last().ID);
			Assert.AreEqual("80798", viewModel.Data[0].Last().ZipCode);
			Assert.AreEqual("München", viewModel.Data[0].Last().Village);
			Assert.AreEqual(48.1571679755151, viewModel.Data[0].Last().Latitude);
			Assert.AreEqual(11.5656418013965, viewModel.Data[0].Last().Longitude);

            Assert.AreEqual("Nienburg (Weser)", viewModel.Data[1].Key);
			Assert.AreEqual(1, viewModel.Data[1].Count());

            Assert.AreEqual(7073, viewModel.Data[1].First().ID);
			Assert.AreEqual("31582", viewModel.Data[1].First().ZipCode);
			Assert.AreEqual("Nienburg (Weser)", viewModel.Data[1].First().Village);
			Assert.AreEqual(52.6407898946597, viewModel.Data[1].First().Latitude);
			Assert.AreEqual(9.23150063371375, viewModel.Data[1].First().Longitude);

			Assert.AreEqual("Uchte", viewModel.Data[2].Key);
			Assert.AreEqual(1, viewModel.Data[2].Count());

			Assert.AreEqual(7076, viewModel.Data[2].First().ID);
			Assert.AreEqual("31600", viewModel.Data[2].First().ZipCode);
			Assert.AreEqual("Uchte", viewModel.Data[2].First().Village);
			Assert.AreEqual(52.5192716743236, viewModel.Data[2].First().Latitude);
			Assert.AreEqual(8.87567370960235, viewModel.Data[2].First().Longitude);
        }

        [Test]
        public async Task FilterUsage()
        {
			// Arrange
			LocationRepository locationRepository = new LocationRepository(ServiceMocks.GetMockDataFileService(), _appSettings);

			Mock<IAppSettings> mockAppSettings = new Mock<IAppSettings>();
			mockAppSettings.Setup(settings => settings.OrderByZipCode).Returns(false);

			MainViewModel viewModel = new MainViewModel(locationRepository, mockAppSettings.Object);

            // Act
            await viewModel.Initialize();

            viewModel.Filter = "80796";
            viewModel.FilterLocationsCommand.Execute(null);

			// Assert
			Assert.IsNotNull(viewModel.Data);
			Assert.AreEqual(1, viewModel.Data.Count());

			Assert.AreEqual("München", viewModel.Data[0].Key);
			Assert.AreEqual(1, viewModel.Data[0].Count());

			Assert.AreEqual(10672, viewModel.Data[0].First().ID);
			Assert.AreEqual("80796", viewModel.Data[0].First().ZipCode);
			Assert.AreEqual("München", viewModel.Data[0].First().Village);
			Assert.AreEqual(48.1646490940644, viewModel.Data[0].First().Latitude);
			Assert.AreEqual(11.5694707183568, viewModel.Data[0].First().Longitude);
		}
    }
}
