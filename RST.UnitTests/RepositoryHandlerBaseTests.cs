﻿using RST.UnitTests.TestEntities;
using RST.Mediatr.Extensions;
using MediatR;
using RST.Contracts;
using Moq;
using RST.Defaults;

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

            modelHashFactoryMock.Setup(s => s.GetDefault())
                .Returns(modelHasherMock.Object);

            sut = new MyTestRepositoryHandler(serviceProviderMock.Object);
        }

        [Test]
        public async Task Test()
        {
            var id = Guid.NewGuid();
            var dbCustomer = new Customer
            {
                Id = id,
                Firstname = "Test",
                Lastname = "Test",
                Middlename = "Test",
            };

            repositoryMock!.Setup(s => s.FindAsync(It.IsAny<CancellationToken>(), id))
                .Returns(ValueTask.FromResult<Customer?>(dbCustomer));

            modelHasherMock.Setup(s => s.CompareHash(dbCustomer, null, "THIS_IS_THE_HASH"))
                .Returns(true);

            await sut!.ProcessSave(new MyRequest(), c => { return new Customer { 
                    Id = id, 
                    Firstname = "Tes12t",
                    Lastname = "Test1",
                    Middlename = "Test2",
                    PopulatedDate = DateTime.Now,
                    Hash = "THIS_IS_THE_HASH"
            }; }, CancellationToken.None);
        }
    }
}
