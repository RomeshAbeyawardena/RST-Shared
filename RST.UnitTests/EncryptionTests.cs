using Moq;
using RST.Contracts;
using RST.Enumerations;
using RST.Security.Cryptography.Extensions.Defaults;
using System.Security.Cryptography;
using System.Text;

namespace RST.UnitTests
{
    [TestFixture]
    public class EncryptionTests
    {
        private DefaultEncryptor encryptor;
        private DefaultDecryptor decryptor;
        private DefaultEncryptionOptions encryptionOptions;
        private DefaultEncryptionModuleOptions encryptionModuleOptions;
        private Mock<ISymmetricAlgorithmFactory> algorithmFactoryMock;
        private Mock<IServiceProvider> serviceProviderMock;

        [SetUp]
        public void SetUp()
        {
            encryptionOptions = new DefaultEncryptionOptions()
            {
                Key = "NmZiMjhlMjNkZDdiNGU0NmJjODgwNTVjOTYxZDY1ZTM=",
                InitialVector = "YjY2OTBlMmRjYjhhMDZmOQ=="
            };
            encryptionModuleOptions = new DefaultEncryptionModuleOptions(Enumerations.EncryptionCaseConvention.Uppercase);
            algorithmFactoryMock = new Mock<ISymmetricAlgorithmFactory>();

            algorithmFactoryMock
                .Setup(s => s.GetSymmetricAlgorithm(It.IsAny<Enumerations.SymmetricAlgorithm>()))
                .Returns(Aes.Create());

            serviceProviderMock = new Mock<IServiceProvider>();

            encryptor = new DefaultEncryptor(encryptionOptions,algorithmFactoryMock.Object,encryptionModuleOptions,
                serviceProviderMock.Object);
            decryptor = new DefaultDecryptor(encryptionOptions, algorithmFactoryMock.Object,
                encryptionModuleOptions,
                serviceProviderMock.Object);
        }

        [Test]
        public void Encrypt()
        {
           var o = "Hello world";
           var s = encryptor.Encrypt(o, encryptionOptions);
           
           var e = decryptor.Decrypt(s, encryptionOptions);
           Assert.That(e, Is.EqualTo(o.ToUpperInvariant()));
        }
    }
}
