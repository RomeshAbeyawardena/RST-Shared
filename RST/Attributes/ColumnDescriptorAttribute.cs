using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace RST.Attributes;

public class ColumnDescriptorAttribute : ColumnAttribute
{
	private IEnumerable<SqlDbType> DoubleParameterFieldTypes = new[]
	{
		SqlDbType.Decimal,
        SqlDbType.Float,
    };

    private IEnumerable<SqlDbType> SingleParameterFieldTypes = new[]
    {
        SqlDbType.VarBinary,
        SqlDbType.VarChar,
        SqlDbType.NVarChar,
        SqlDbType.Char,
        SqlDbType.Binary
    };

    private string DescribeType()
	{
		var type = Enum.GetName(DbType);

		if (DoubleParameterFieldTypes.Contains(DbType))
		{
			return $"{type}({Length},{SubLength})";
		}

        if (SingleParameterFieldTypes.Contains(DbType))
        {
            return $"{type}({Length})";
        }

		return type ?? throw new NullReferenceException();
    }

	public ColumnDescriptorAttribute(SqlDbType dbType, int length = int.MinValue, int subLength  = int.MinValue)
	{
		DbType = dbType;
		Length = length;
		SubLength = subLength;
		this.TypeName = DescribeType();
	}

	public SqlDbType DbType { get; }
	public int? Length { get; }
	public int? SubLength { get; }
}
