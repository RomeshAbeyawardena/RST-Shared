using RST.UnitTests.TestEntities;
using RST.Mediatr.Extensions;
using MediatR;
using RST.Contracts;
using Moq;

namespace RST.UnitTests
{
    [TestFixture]
    public class RepositoryHandlerBaseTests
    {
        private record MyRequest : IRequest<IEnumerable<Customer>>, IDbCommand
        {
            public bool CommitChanges { get; set; }
        }

        private class MyTestRepositoryHandler : RepositoryHandlerBase<MyRequest, IEnumerable<Customer>, Customer>
        {
            public MyTestRepositoryHandler(IServiceProvider serviceProvider, IClockProvider clockProvider, IRepository<Customer> repository) 
                : base(serviceProvider, clockProvider, repository)
            {
            }

            public override Task<IEnumerable<Customer>> Handle(MyRequest request, CancellationToken cancellationToken)
            {
                //this.ProcessSave
                throw new NotImplementedException();
            }
        }

        private Mock<IClockProvider>? clockProviderMock;
        private Mock<IRepository<Customer>>? repositoryMock;
        private Mock<IServiceProvider>? serviceProviderMock;
        private MyTestRepositoryHandler? sut;

        [SetUp]
        public void SetUp()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
            clockProviderMock = new Mock<IClockProvider>();
            repositoryMock = new Mock<IRepository<Customer>>();
            sut = new MyTestRepositoryHandler(serviceProviderMock.Object, clockProviderMock.Object, repositoryMock.Object);
        }

        [Test]
        public async Task Test()
        {
            var id = Guid.NewGuid();
            repositoryMock!.Setup(s => s.FindAsync(It.IsAny<CancellationToken>(), id))
                .Returns(ValueTask.FromResult<Customer?>(new Customer { 
                    Id = id,
                    Firstname = "Test",
                    Lastname = "Test",
                    Middlename = "Test",
                }));

            await sut!.ProcessSave(new MyRequest(), c => { return new Customer { 
                    Id = id, 
                    Firstname = "Tes12t",
                    Lastname = "Test1",
                    Middlename = "Test2",
                    PopulatedDate = DateTime.Now,
            }; }, CancellationToken.None);
        }
    }
}
