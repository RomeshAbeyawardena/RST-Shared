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
        private Mock<ISymmetricAlgorithmFactory> algorithmFactory;

        [SetUp]
        public void SetUp()
        {
            encryptionOptions = new DefaultEncryptionOptions()
            {
                Key = "NmZiMjhlMjNkZDdiNGU0NmJjODgwNTVjOTYxZDY1ZTM=",
                InitialVector = "YjY2OTBlMmRjYjhhMDZmOQ=="
            };
            encryptionModuleOptions = new DefaultEncryptionModuleOptions(Enumerations.EncryptionCaseConvention.Uppercase);
            algorithmFactory = new Mock<ISymmetricAlgorithmFactory>();

            algorithmFactory
                .Setup(s => s.GetSymmetricAlgorithm(It.IsAny<Enumerations.SymmetricAlgorithm>()))
                .Returns(Aes.Create());

            encryptor = new DefaultEncryptor(encryptionOptions,algorithmFactory.Object,encryptionModuleOptions);
            decryptor = new DefaultDecryptor(encryptionOptions, algorithmFactory.Object);
        }

        [Test]
        public void Encrypt()
        {
           var o = "Hello world";
           var s = encryptor.Encrypt(o, encryptionOptions);
           
           var e = decryptor.Decrypt(s, encryptionOptions);
           Assert.That(e, Is.EqualTo(o));
        }
    }
}
