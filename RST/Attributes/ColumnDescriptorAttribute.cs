using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace RST.Attributes;

/// <summary>
/// Represents an <see cref="Attribute"/> used to describe a DB type
/// </summary>
public class ColumnDescriptorAttribute : ColumnAttribute
{
    private readonly IEnumerable<SqlDbType> DoubleParameterFieldTypes = new[]
    {
        SqlDbType.Decimal,
        SqlDbType.Float,
    };

    private readonly IEnumerable<SqlDbType> SingleParameterFieldTypes = new[]
    {
        SqlDbType.VarBinary,
        SqlDbType.VarChar,
        SqlDbType.NVarChar,
        SqlDbType.Char,
        SqlDbType.Binary
    };

    private string DescribeType()
    {
        var type = Enum.GetName(DbType)?.ToUpper();

        if (DoubleParameterFieldTypes.Contains(DbType))
        {
            return $"{type}({Length},{SubLength})";
        }

        if (SingleParameterFieldTypes.Contains(DbType))
        {
            return $"{type}({(Length == int.MaxValue ? "MAX" : Length)})";
        }

        return type ?? throw new NullReferenceException();
    }

    /// <summary>
    /// Creates an instance of <see cref="ColumnDescriptorAttribute"/>
    /// </summary>
    /// <param name="name"><inheritdoc cref="ColumnAttribute.ColumnAttribute(string)"/></param>
    /// <param name="dbType">Describes the DB Type</param>
    /// <param name="length">Describes the length</param>
    /// <param name="subLength">Describes the sub length</param>
    public ColumnDescriptorAttribute(string name,
        SqlDbType dbType,
        int length = int.MinValue,
        int subLength = int.MinValue)
        : base(name)
    {
        DbType = dbType;
        Length = length;
        SubLength = subLength;
        this.TypeName = DescribeType();
    }

    /// <summary>
    /// Creates an instance of <see cref="ColumnDescriptorAttribute"/>
    /// </summary>
    /// <param name="dbType">Describes the DB Type</param>
    /// <param name="length">Describes the length</param>
    /// <param name="subLength">Describes the sub length</param>
    public ColumnDescriptorAttribute(SqlDbType dbType,
        int length = int.MinValue, int subLength = int.MinValue)
    {
        DbType = dbType;
        Length = length;
        SubLength = subLength;
        this.TypeName = DescribeType();
    }

    /// <summary>
    /// Gets the Db Type
    /// </summary>
    public SqlDbType DbType { get; }

    /// <summary>
    /// Gets the length used by the Db type
    /// </summary>
    public int? Length { get; }
    /// <summary>
    /// Gets the sub length of the Db type
    /// </summary>
    public int? SubLength { get; }
}
