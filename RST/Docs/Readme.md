# RST
## Attributes
### ColumnDescriptorAttribute
Represents an `System.Attribute` used to describe a DB type

#### Usage
```c#
[ColumnDescriptor(System.Data.SqlDbType.Nvarchar, 200)]
public string Name { get; set; }
```

### RegisterAttribute
Represents an Attribute used to decorate services for dependency injection
#### Usage
```c#
[Register(Microsoft.Extensions.DependencyInjection.Transient)]
public class MyRegisterableClass()
{
}
```

## Contracts
### IClockProvider
Represents a clock provider for mocking in unit tests
When implemented in a registered `System.IServiceProvider`
returns the system clock.
### ICreated

Represents a creation timestamp.
When used within an instance of `IRepository<T>` passes a timestamp during an insert operation

#### Specific Usage
```c#
public record MyTable : RST.Contracts.ICreated
{
	public System.DateTimeOffset Created { get;set; }
}
```
#### Generic Usage
```c#
public record MyTable : RST.Contracts.ICreated<DateTime>
{
	public System.DateTime Created { get;set; }
}
```

### IDateRangeQuery
Represent a query comparing against a date range when implemented in a DB query
```c#
public record MyQuery : IDateRangeQuery
{
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
}
```
### IDbCommand
When inherited in a record or class represents a Db command
### IDbQuery
When inherited in a record or class represents a DB query
### IDecryptor
When inherited in a class represents a decryptor used to decrypt a string using a known encryption mechanism
#### Example Usage
```c#
static System.IServiceProvider ConfigureServices(
	IServiceCollection services)...//configure services

static void Main(string[] args)
{
	ConfigureServices().GetRequiredService<IDecryptor>()
		.Decrypt("Hello Word", "MyEncryptProfile");
}
```

### IDictionaryBuilder
### IEncryptionModuleOptions
### IEncryptionOptions
### IEncryptor
### IIdentity
### IListBuilder
### ILookupValue
### IModified
### IObjectChange
### IOrderByQuery
### IPagedQuery
### IPagedRequest
### IPagedResult
### IRepository
### IResult
### IServiceDefinitionOptions
### ISymmetricAlgorithmFactory

## Enumerations
### EncryptionCaseConvention
### EncryptionMode
### SortOrder
### SymmetricAlgorithm

## Extensions
### ArrayStringExtensions
### EnumExtensions
### ObjectExtensions
### PropertyInfoExtensions
### TypeExtensions

## Options
### RepositoryBase
### ServiceDefinitionOptions

## ServiceDefinitions
### S