namespace RST.UnitTests;

[TestFixture]
public class ServiceDefinitionOptionsTests
{
    ServiceDefinitionOptions serviceDefinitionOptions;
    [SetUp]
    public void SetUp()
    {
        serviceDefinitionOptions = new ServiceDefinitionOptions();
    }

    [Test]
    public void Ensure_cached_list_is_populated()
    {
        serviceDefinitionOptions.ConfigureCoreServices = true;
        serviceDefinitionOptions.ConfigureCryptographyExtensions = true;

        Assert.That(serviceDefinitionOptions.hasChanged, Is.True);
        Assert.That(serviceDefinitionOptions.HasAssemblies, Is.True);
        Assert.That(serviceDefinitionOptions.Assemblies.Count(), Is.EqualTo(2));
        Assert.That(serviceDefinitionOptions.hasChanged, Is.False);
    }
}
