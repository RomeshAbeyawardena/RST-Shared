using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RST.AspNetCore.Extensions;
using RST.AspNetCore.Extensions.Contracts;
using RST.AspNetCore.Extensions.Defaults;
using RST.Contracts;
using System.Text.Encodings.Web;

namespace RST.UnitTests;

[TestFixture]
public class ApplicationAuthenticationTests
{
    private DefaultApplicationAuthenticationProvider applicationAuthenticationProvider;
    private ApplicationAuthenticationSchemeOptions options;
    private Mock<ILogger<DefaultApplicationAuthenticationProvider>> loggerMock;
    private Mock<ISystemClock> clockMock;
    private Mock<IEncryptionModuleOptions> encryptionModuleMock;
    private Mock<IDecryptor> decryptorMock;
    private Mock<ISecuritySignatureProvider> securitySignatureProviderMock;
    private Mock<IApplicationAuthenticationRepository> applicationAuthenticationRepositoryMock;
    private Mock<IHeaderDictionary> headerDictionaryMock;
    [SetUp]
    public void SetUp()
    {
        options = new ApplicationAuthenticationSchemeOptions("");
        loggerMock = new Mock<ILogger<DefaultApplicationAuthenticationProvider>>();
        clockMock = new Mock<ISystemClock>();
        encryptionModuleMock = new Mock<IEncryptionModuleOptions>();
        decryptorMock = new Mock<IDecryptor>();
        securitySignatureProviderMock = new Mock<ISecuritySignatureProvider>();
        applicationAuthenticationRepositoryMock = new Mock<IApplicationAuthenticationRepository>();
        headerDictionaryMock = new Mock<IHeaderDictionary>();
        applicationAuthenticationProvider = new DefaultApplicationAuthenticationProvider(options,
             loggerMock.Object,encryptionModuleMock.Object, headerDictionaryMock.Object,
            decryptorMock.Object, securitySignatureProviderMock.Object,
            applicationAuthenticationRepositoryMock.Object);

    }

    [Test]
    public async Task ValidateAccessToken()
    {
        await applicationAuthenticationProvider.HandleAuthenticateAsync();
    }
}
