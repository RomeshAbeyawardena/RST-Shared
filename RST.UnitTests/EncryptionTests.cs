using Moq;
using NUnit.Framework;
using RST.Defaults;
using RST.Contracts;
using RST.Security.Cryptography.Extensions.Defaults;
using System.Security.Cryptography;

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

        [Test]
        public void Encrypt_using_moduleOptions()
        {
            encryptionModuleOptions.EncryptionOptions.Add("test", encryptionOptions);
            var o = "Hello world";
            var s = encryptor.Encrypt("test", o);

            var e = decryptor.Decrypt("test", s);
            Assert.That(e, Is.EqualTo(o.ToUpperInvariant()));

            encryptionModuleOptions.EncryptionOptionsFactory.Add("test1", s => encryptionOptions);

            s = encryptor.Encrypt("test1", o);

            e = decryptor.Decrypt("test1", s);
            Assert.That(e, Is.EqualTo(o.ToUpperInvariant()));
        }

        [Test]
        public void Encrypt_using_moduleOptions_fail()
        {
            encryptionModuleOptions.EncryptionOptions.Add("test", encryptionOptions);
            var o = "Hello world";
            
            Assert.Throws<NullReferenceException>(() => encryptor.Encrypt("test2", o));
        }
    }
}
