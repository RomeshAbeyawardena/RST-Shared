using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Moq;
using RST.Extensions.Configuration;
using System.Text;

namespace RST.UnitTests;

[TestFixture]
public class ConfigurationExtensionTests
{
    private ConfigurationManager configurationManager;
    private Mock<IFileInfo> fileInfoMock;
    private Mock<IFileProvider> fileProviderMock;
    private IConfiguration configuration;
    [SetUp]
    public void SetUp()
    {
        configurationManager = new ConfigurationManager();
        fileInfoMock = new Mock<IFileInfo>();
        fileInfoMock.Setup(a => a.Exists).Returns(true);
        
        var ms = new MemoryStream();
        var sw = new StreamWriter(ms);
        sw.Write(@"{
  ""encryption"": {
    ""config"": {
      ""testkey"": {
        ""privateKey"": ""YzJiMGQzOGY3YjhjNDJhMzk4NjE2ODg2NzZjMDMxNTQK"",
        ""publicKey"": ""YjRiZGRiYTZlZThmNGE1N2I4OGJlNzIyMmFjNWMwMTI="",
        ""enabled"": true
      },
      ""testkey2"": {
        ""privateKey"": ""ZTY4NDFjODg4NTRlNDNhOWEyYjZhYWQzZWZmYTYwOTI="",
        ""publicKey"": ""YzJiMGQzOGY3YjhjNDJhMzk4NjE2ODg2NzZjMDMxNTQK"",
        ""enabled"": true
      },
      ""testkey3"": {
        ""privateKey"": ""ZGQ4YTNiODFjOTUxNGZhMmE0ZjQ4N2IyZmUxMzc1ODA="",
        ""publicKey"": ""MDcyMjQwMTQxZTYzNGNlZjlhMThmZWVmNjRiMjA3ODQ="",
        ""enabled"": true
      }
    }
  }
}");
        sw.Flush();
        ms.Position = 0;
        fileInfoMock.Setup(a => a.CreateReadStream())
            //.Throws<FileNotFoundException>();
            .Returns(ms).Verifiable();
        fileInfoMock.Setup(a => a.Length).Returns(ms.Length);
        fileProviderMock = new Mock<IFileProvider>();
        fileProviderMock.Setup(a => a.GetFileInfo("appsetting.json")).Returns(fileInfoMock.Object);

        var test = Encoding.UTF8.GetString(ms.ToArray());
        
        configuration = configurationManager.Add<JsonConfigurationSource>(c => {
            c.FileProvider = fileProviderMock.Object;
            c.Path = "appsetting.json";
            c.Optional = false;
            c.OnLoadException = (a) => throw new FileNotFoundException(a.Exception.Message);
            c.ReloadOnChange = false;
        }).Build();

    }
    
    [Test]
    public void GetFromPath()
    {
        var config = configuration
            .GetFromPath("encryption/config/testkey").AsEnumerable();

        fileProviderMock.Verify(a => a.GetFileInfo("appsetting.json"), Times.Once);
    }
}
