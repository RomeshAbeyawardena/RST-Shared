using LinqKit;
using MediatR;
using Moq;
using RST.Contracts;
using RST.Mediatr.Extensions;
using RST.UnitTests.TestEntities;

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
            public MyTestRepositoryHandler(IServiceProvider serviceProvider)
                : base(serviceProvider)
            {
            }

            public override Task<IEnumerable<Customer>> Handle(MyRequest request, CancellationToken cancellationToken)
            {
                //this.ProcessSave
                throw new NotImplementedException();
            }
        }

        private Mock<IModelHasher> modelHasherMock;
        private Mock<IPropertyTypeProviderCache>? propertyTypeProviderCacheMock;
        private Mock<IModelHasherFactory> modelHashFactoryMock;
        private Mock<IClockProvider>? clockProviderMock;
        private Mock<IRepository<Customer>>? repositoryMock;
        private Mock<IServiceProvider>? serviceProviderMock;
        private Mock<IObservable<ExpressionStarter<Customer>>> expressionObserverMock;
        private MyTestRepositoryHandler? sut;

        [SetUp]
        public void SetUp()
        {
            modelHasherMock = new Mock<IModelHasher>();
            serviceProviderMock = new Mock<IServiceProvider>();
            clockProviderMock = new Mock<IClockProvider>();
            propertyTypeProviderCacheMock = new Mock<IPropertyTypeProviderCache>();
            repositoryMock = new Mock<IRepository<Customer>>();
            serviceProviderMock.Setup(s => s.GetService(typeof(IPropertyTypeProviderCache)))
                .Returns(propertyTypeProviderCacheMock.Object);

            serviceProviderMock.Setup(s => s.GetService(typeof(IRepository<Customer>)))
                .Returns(repositoryMock.Object);

            modelHashFactoryMock = new Mock<IModelHasherFactory>();

            serviceProviderMock.Setup(s => s.GetService(typeof(IModelHasherFactory)))
                .Returns(modelHashFactoryMock.Object);
            expressionObserverMock = new Mock<IObservable<ExpressionStarter<Customer>>>();
            repositoryMock.Setup(s => s.OnReset).Returns(expressionObserverMock.Object);

            modelHashFactoryMock.Setup(s => s.GetDefault())
                .Returns(modelHasherMock.Object);

            sut = new MyTestRepositoryHandler(serviceProviderMock.Object);
        }

        [Test]
        public async Task ProcessSave()
        {
            var id = Guid.NewGuid();
            var dbCustomer = new Customer
            {
                Id = id,
                Firstname = "Test",
                Lastname = "Test",
                Middlename = "Test",
                Hash = "THIS_IS_THE_HASH"
            };

            var editedCustomer = new Customer
            {
                Id = id,
                Firstname = "Tes12t",
                Lastname = "Test1",
                Middlename = "Test2",
                PopulatedDate = DateTime.Now,
                Hash = "THIS_IS_THE_HASH"
            };

            repositoryMock!.Setup(s => s.FindAsync(It.IsAny<CancellationToken>(), id))
                .Returns(ValueTask.FromResult<Customer?>(dbCustomer));

            modelHasherMock.Setup(s => s.CompareHash(dbCustomer, null, "THIS_IS_THE_HASH"))
                .Returns(true);

            modelHasherMock.Setup(s => s.CompareHash("THIS_IS_THE_HASH", "THIS_IS_THE_HASH"))
                .Returns(true);

            modelHasherMock.Setup(s => s.CalculateHash(editedCustomer, null)).Returns("NewHash");

            await sut!.ProcessSave(new MyRequest(), c =>
            {
                return editedCustomer;
            }, CancellationToken.None);

            Assert.That(editedCustomer.Hash, Is.EqualTo("NewHash"));
        }
    }
}
