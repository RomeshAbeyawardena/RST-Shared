using Moq;
using RST.Contracts;
using RST.Security.Cryptography.Extensions.Defaults;
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
                Key = Convert.ToBase64String(Encoding.UTF8.GetBytes("NmZiMjhlMjNkZDdiNGU0NmJjODgwNTVjOTYxZDY1ZTM=")),
                InitialVector = Convert.ToBase64String(Encoding.UTF8.GetBytes("YjY2OTBlMmRjYjhhMDZmOQ=="))
            };
            encryptionModuleOptions = new DefaultEncryptionModuleOptions(Enumerations.EncryptionCaseConvention.Uppercase);
            algorithmFactory = new Mock<ISymmetricAlgorithmFactory>();
            encryptor = new DefaultEncryptor(encryptionOptions,algorithmFactory.Object,encryptionModuleOptions);
            decryptor = new DefaultDecryptor(encryptionOptions, algorithmFactory.Object);
        }

        [Test]
        public void Encrypt()
        {
           
           var s = encryptor.Encrypt("Hello world", encryptionOptions);
            Assert.That(s, Is.EqualTo("s"));
        }
    }
}
