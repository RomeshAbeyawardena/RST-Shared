using Microsoft.Extensions.DependencyInjection;
using Moq;
using RST.Contracts;
using RST.Security.Cryptography.Defaults;
using System.Security.Cryptography;

namespace RST.UnitTests;
[TestFixture]
public class SecuritySignatureTests
{
    private DefaultSecuritySignature? securitySignature;
    private Mock<ISignatureConfiguration> configurationMock;
    private ServiceProvider serviceProvider;
    [SetUp]
    public void SetUp()
    {
        serviceProvider = new ServiceCollection()
            .AddTransient((s) => RSA.Create()).BuildServiceProvider();
        
        securitySignature = new DefaultSecuritySignature(serviceProvider);
        configurationMock= new Mock<ISignatureConfiguration>();
    }

    [Test]
    public void Test()
    {
        var data = "2c83a9746dde4bb6b98e21fa3e682026";
        configurationMock.SetupProperty(s => s.EncryptionAlgorithm, PbeEncryptionAlgorithm.Aes256Cbc);
        configurationMock.SetupProperty(s => s.PrivateKeyPassword, "e138llRA1787!");
        configurationMock.SetupProperty(s => s.PublicKey, "MIIBCgKCAQEAx+zH/l2a2iM27f+qwh7PaKYlONcTMUv8i9D1mghh4Wz7wa1KZgKibu+NveN65hw0haz/6bnYfPQlBfx+LVgo5Nsg7NLljOiYwMh3lAdUraTVz8cauHBSNJcYjw8KAQJaxT/QIS+VjGxQImX6LKFo/oIqtLYDPLJXvVgD41YupBZ8Xma+TAD5UkB/RdCUSIBm5rcDGLUQtUCO521hj50NXVzdnUY3mOIOozrJHZbucxGTZfnDWts9NPRyZf8X+rCc4lhl7y2UWAwY6NzQqxMKBo5+5D6k/hb1/oz3os3aVKGxp94IW/Z6MYAKGG122WI8mwa1OauqUO16zy9f51RN0QIDAQAB\r\n");
        configurationMock.SetupProperty(s => s.PrivateKey, "MIIFNDBeBgkqhkiG9w0BBQ0wUTAwBgkqhkiG9w0BBQwwIwQQ4Wstoole3ugVvL1EuIJL/gIBHjAMBggqhkiG9w0CCwUAMB0GCWCGSAFlAwQBKgQQM9iHZnxQgAAh3nxxFbyHngSCBNAiMQzjsyMh/T8KRlB7nyqchydJZCcqO2i0UG1sPx8B5osKmlycFsBqAnHZgS5sFaCq+uzJXZqz/tKoAZyWEylCWHUzEqXgnwEd29AjcT3i14cKRXdupZxlW2RdvFlL9MjZAK/X+PwZ2mFIZsf419b4vaC0YfdgfmJgeHOmOJ0eXU7HA6WZZB2+poFEkjwQ226Px2DiF8mibo+ua2ZLmaeBqTLiz5BG9tkh+3rcwagTd4jvSBDH2wN9B6KFKZqS3I7FQBdjMjFfcx48NmVFwgVqiW1EFg+GE04X4J5WQHt9mtQnKWvhhVinZEdiDMaebSumZwz/LM0d+hGeJFWuryU+7Qv7j2Df7X0vJ6DE9vsFBid5fHzpfYBhGl50nWMudRasGcETn5FsMyeu0rwsxeXL4nUrv1vX7frCilL8gn7omZ3g1WrarVhk8VPwWL4p+rUCUkxI8iaR5Ee7d3FxImOyxlMFwMtOJqk2h1mQjYJQi2QIjIHrbywAbngf2s2p1110a7scWRxXE74j2h/r14H8l/91nxU4akQB064BNfIIN1/G0weU88nWU91m/tSmsiZBwWpb7eipAMyC7pSeMqJXKVzDdzHKnIFCS12U7XrXCDoZoisnGK5b76h8g/JymZxCwQc20sz5WGsq2fhfKhvCkFDf+3cEq2pQnikjA4h7dY1nMqPPgXoF8aht2vbx7AbHniQh9bauDveY8C2D+oMdwC+WhCy01nRAjlOELMgX4pTXBAXZ+lpIat1fApplB4sjG5mWRQlb/C3H64hg4p1XrlLpVL6Yng9dLFsVdL3SyGRAry7rfJLvAeHMH/bszkyLwhNpty4coVCzYyGRvfokv38Jc0haZFZD/67qkuReQZPYxHGwQNUaIlwU4VjBoM0EPI+PmcWH3sMPhlhmilXdOcNpM9y2/3mf1P1H86Ea12/Ma9Z0rQQ8mLopk2ZsjkLa2uCtpfNEb/eJlwL2wRHoIRMAELI5P3EWCPBvMkG2ZXd7AKWLA3YWsob4KTPxUjeCqTA7t2kMy1fu1mjOBU+n2tzSNJIaSfLuBi96N5sfiZjac+PyfKr0mW0X0XSn6MxpNSW9gBE+aF4vMmrh1/eXW2mAjQC0YcW9Bj0B6wmJVXaPAdCKN6T5qA29rb4nRNw7hdBiciXOffejV+jp4O+KOCrOIz9QrZYewY/+WSrIB0VprytL69hyHiNWcQhhhmsGuJ5MsqSIlLsEFSMPamE+6Evz6ZxNZtNX14j9wdQ6b47w91w3rVA0dhnjpPRj4WtkzvF7kUsAlSRsFr9j7sgvw/JWPyyyQPKncjtg8PFFKglPsdRZo0wym3encEGkfXISBh7LqxipIp4VdE3JFTbZv6QjQeOXDIIe1r5eMuV7g1TJrau9Iko8Hap8w0bJiRmtdtKza6M40dNIxenM57jRXTQXbbKZ1QmEpo4ahylDY9UdEBgfiGW1YPLj73P6jFmncY/7UJqRWbDlZNEMz7t5IQ0nwPTzZhghnzfJoMETu8M+c4MpOKlmhW9Cx/ywCvLHtDsyXLuSxtvTn4PRkSK6TeJW+vter2ht2Xpv9xVOKiNBR5O6Eh85eYOtwgQgdkaEHrgEz4nMy36BcVOKidXFkBEkbDx/LGmMZLMAsAng+A==");
        configurationMock.SetupProperty(s => s.Padding, RSASignaturePadding.Pkcs1);
        configurationMock.SetupProperty(s => s.HashAlgorithmName, HashAlgorithmName.SHA512);
        configurationMock.SetupProperty(s => s.IterationCount, 8);
        var signature = securitySignature!.SignData(data, configurationMock.Object);
        securitySignature.FlushProvider();
        Assert.That(securitySignature.VerifyData(data, signature, configurationMock.Object), Is.True);
    }
}
