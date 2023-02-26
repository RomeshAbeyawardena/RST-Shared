using AutoMapper;
using MediatR;
using Moq;
using RST.AspNetCore.Extensions;

namespace RST.UnitTests;

[TestFixture]
public class ApiControllerTests
{
    class MyApiController : ApiController
    {
        public MyApiController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }

    private Mock<IMediator> mediatorMock;
    private Mock<IMapper> mapperMock;
    private Mock<IServiceProvider> serviceProviderMock;
    private MyApiController? apiController;

    [SetUp]
    public void SetUp()
    {
        mediatorMock = new Mock<IMediator>();
        mapperMock = new Mock<IMapper>();
        serviceProviderMock = new Mock<IServiceProvider>();
        apiController = new MyApiController(serviceProviderMock.Object);
    }

    [Test]
    public void Initialize()
    {
        serviceProviderMock.Setup(s => s.GetService(typeof(IMapper)))
            .Returns(mapperMock.Object).Verifiable();
        serviceProviderMock.Setup(s => s.GetService(typeof(IMediator)))
            .Returns(mediatorMock.Object).Verifiable();

        serviceProviderMock.Verify(s => s.GetService(typeof(IMapper)), Times.Once);
        serviceProviderMock.Verify(s => s.GetService(typeof(IMediator)), Times.Once);
    }
}
