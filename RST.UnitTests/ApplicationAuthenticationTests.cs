using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using RST.AspNetCore.Extensions;
using RST.AspNetCore.Extensions.Contracts;
using RST.AspNetCore.Extensions.Defaults;
using RST.Contracts;
using RST.Defaults;
using System.Text;
using System.Text.Encodings.Web;

namespace RST.UnitTests;

[TestFixture]
public class ApplicationAuthenticationTests
{
    private DefaultApplicationAuthenticationProvider applicationAuthenticationProvider;
    private DefaultApplicationAuthenticationTokenBuilder applicationAuthenticationTokenBuilder;
    private ApplicationAuthenticationSchemeOptions options;
    private Mock<ILogger<DefaultApplicationAuthenticationProvider>> loggerMock;
    private Mock<IEncryptionModuleOptions> encryptionModuleMock;
    private Mock<IEncryptor> encryptorMock;
    private Mock<IDecryptor> decryptorMock;
    private Mock<ISecuritySignatureProvider> securitySignatureProviderMock;
    private Mock<IApplicationAuthenticationRepository> applicationAuthenticationRepositoryMock;
    private Dictionary<string, StringValues> headerDictionary;
    private Mock<IApplicationIdentity> identityMock;
    [SetUp]
    public void SetUp()
    {
        options = new ApplicationAuthenticationSchemeOptions("");
        loggerMock = new Mock<ILogger<DefaultApplicationAuthenticationProvider>>();
        identityMock = new Mock<IApplicationIdentity>();
        encryptionModuleMock = new Mock<IEncryptionModuleOptions>();
        decryptorMock = new Mock<IDecryptor>();
        securitySignatureProviderMock = new Mock<ISecuritySignatureProvider>();
        applicationAuthenticationRepositoryMock = new Mock<IApplicationAuthenticationRepository>();
        headerDictionary = new Dictionary<string, StringValues>();
        encryptorMock = new Mock<IEncryptor>();
        applicationAuthenticationProvider = new DefaultApplicationAuthenticationProvider(
             loggerMock.Object,encryptionModuleMock.Object, new HeaderDictionary(headerDictionary),
            decryptorMock.Object, securitySignatureProviderMock.Object,
            applicationAuthenticationRepositoryMock.Object);

        applicationAuthenticationTokenBuilder = new DefaultApplicationAuthenticationTokenBuilder(encryptionModuleMock.Object, encryptorMock.Object, applicationAuthenticationProvider, securitySignatureProviderMock.Object);
    }

    [Test]
    public async Task ValidateAccessToken()
    {
        options.EncryptionOptions = new DefaultEncryptionOptions
        {
            Algorithm = Enumerations.SymmetricAlgorithm.Aes,
            Encoding = Encoding.UTF8,
            Key = "VGhpc2lzdGhlZW5jcnlwdGVkYmxvYnRoYXRpc25lZWRzdG9iZWRlY3J5cHRlZGJlZm9yZWJlaW5nYWNjZXNzZWRhbmR0aGVuZXh0Yml0d2lsbGJldGhlcHVibGlja2V5Lg=="
        };
        const string publickey = "VGhpc3RoZXB1YmxpY2tleQ==";

        identityMock.SetupProperty(i => i.PublicKey, "MIIBCgKCAQEAx+zH/l2a2iM27f+qwh7PaKYlONcTMUv8i9D1mghh4Wz7wa1KZgKibu+NveN65hw0haz/6bnYfPQlBfx+LVgo5Nsg7NLljOiYwMh3lAdUraTVz8cauHBSNJcYjw8KAQJaxT/QIS+VjGxQImX6LKFo/oIqtLYDPLJXvVgD41YupBZ8Xma+TAD5UkB/RdCUSIBm5rcDGLUQtUCO521hj50NXVzdnUY3mOIOozrJHZbucxGTZfnDWts9NPRyZf8X+rCc4lhl7y2UWAwY6NzQqxMKBo5+5D6k/hb1/oz3os3aVKGxp94IW/Z6MYAKGG122WI8mwa1OauqUO16zy9f51RN0QIDAQAB");

        var token = applicationAuthenticationTokenBuilder.BuildToken(identityMock.Object, publickey, options);

        headerDictionary.Add(HeaderNames.Authorization, new StringValues(token.AuthorisationToken));
        headerDictionary.Add(HeaderNames.ETag, new StringValues(token.ETag));

        var result = await applicationAuthenticationProvider.HandleAuthenticateAsync(options);

        Assert.That(result, Is.Not.Null);
    }
}
