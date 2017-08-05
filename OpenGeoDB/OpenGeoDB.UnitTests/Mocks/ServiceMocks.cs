using System.Text;
using Moq;
using OpenGeoDB.Core.DependencyServices;

namespace OpenGeoDB.UnitTests.Mocks
{
    public static class ServiceMocks
    {
        public static IDataFileService GetMockDataFileService(bool addFailureLine = false)
        {
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("#loc_id\tplz\tlon\tlat\tOrt");
			builder.AppendLine("7073\t31582\t9.23150063371375\t52.6407898946597\tNienburg (Weser)");
			builder.AppendLine("7076\t31600\t8.87567370960235\t52.5192716743236\tUchte");

			if (addFailureLine)
				builder.AppendLine("10670\t80X687\t11.50X59093215982\t48.1432006878X012\tMünchen");

			builder.AppendLine("10670\t80687\t11.5059093215982\t48.1432006878012\tMünchen");
            builder.AppendLine("10672\t80796\t11.5694707183568\t48.1646490940644\tMünchen");
            builder.AppendLine("10674\t80798\t11.5656418013965\t48.1571679755151\tMünchen");

            Mock<IDataFileService> dataFileService = new Mock<IDataFileService>();
            dataFileService.Setup(service => service.LoadFileContentAsync()).ReturnsAsync(builder.ToString());

            return dataFileService.Object;
        }
    }
}
