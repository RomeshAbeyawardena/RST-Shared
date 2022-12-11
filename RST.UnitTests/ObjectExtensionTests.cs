using RST.Extensions;
using RST.UnitTests.TestEntities;

namespace RST.UnitTests;

/// <summary>
/// 
/// </summary>
public class ObjectExtensionTests
{
    /// <summary>
    /// 
    /// </summary>
    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    [Test]
    public void HasChanges()
    {
        var customer = new Customer
        {
            Firstname = "John",
            Middlename = "Elliot",
            Lastname = "Doe",
            Id = Guid.NewGuid()
        };
        Assert.Multiple(() =>
        {
            Assert.That(customer.HasChanges(new Customer
            {
                Firstname = "John",
                Middlename = "Elliot",
                Lastname = "Doe",
                Id = Guid.NewGuid()
            }, out var changes), Is.True);

            Assert.That(changes.Count(), Is.EqualTo(1));

            Assert.That(customer.HasChanges(new Customer
            {
                Firstname = "John",
                Lastname = "Doe",
                Id = Guid.NewGuid()
            }, out changes), Is.True);

            Assert.That(changes.Count(), Is.EqualTo(2));

            Assert.That(customer.HasChanges(new Customer
            {
                Firstname = "Jane",
                Middlename = "Elliot",
                Lastname = "Doe",
                Id = Guid.NewGuid()
            }, out changes), Is.True);

            Assert.That(changes.Count(), Is.EqualTo(2));

            Assert.That(customer.HasChanges(new Customer
            {
                Firstname = "John",
                Middlename = "Elliot",
                Lastname = "Doe",
                Id = customer.Id
            }, out changes), Is.False);

            Assert.That(changes, Is.Empty);
        });
    }
}