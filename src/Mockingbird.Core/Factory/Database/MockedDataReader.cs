using System.Collections;
using System.Data.Common;

namespace Mockingbird.Factory.Database
{
    internal class MockedDataReader : DbDataReader
    {
        private int rowIndex = -1;
        private int resultIndex = 0;
        private readonly DataTable[] dataSource = Array.Empty<DataTable>();

        public MockedDataReader(IEnumerable? dataSource)
        {
            if (dataSource != null)
            {
                this.dataSource = dataSource
                    .Cast<object>()
                    .Select(d => new DataTable(d))
                    .ToArray();
            }
        }

        private DataTable? CurrentTable => resultIndex < dataSource.Length ? dataSource[resultIndex] : null;

        private object?[] CurrentRow => CurrentTable?.Data?.Length > rowIndex ? CurrentTable.Data[rowIndex] : Array.Empty<object?>();

        public override object? this[int ordinal] => CurrentRow[ordinal];

        public override object? this[string name] => CurrentRow[GetOrdinal(name)];

        public override int Depth => dataSource.Length;

        public override int FieldCount
            => CurrentTable?.Columns?.Length ?? 0;

        public override bool HasRows
            => CurrentTable?.Data?.Length > 0;

        public override bool IsClosed
            => false;

        public override int RecordsAffected
            => CurrentTable?.Data?.Length ?? 0;

        public override bool GetBoolean(int ordinal)
            => CurrentRow[ordinal] as bool? ?? false;

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
            => CurrentTable?.Types[ordinal] ?? typeof(void);

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int ordinal) => CurrentTable?.Columns[ordinal] ?? string.Empty;

        public override int GetOrdinal(string name) => CurrentTable?.Columns?.Length > 0 ? Array.IndexOf(CurrentTable.Columns, name) : 0;

        public override string GetString(int ordinal)
            => GetValue(ordinal) as string ?? string.Empty;

        public override object? GetValue(int ordinal) => CurrentRow[ordinal];

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
            => GetValue(ordinal) == null;

        public override bool NextResult()
        {
            resultIndex++;
            rowIndex = -1;
            return dataSource.Length > resultIndex;
        }

        public override bool Read()
        {
            rowIndex++;
            return dataSource.Length > resultIndex && dataSource[resultIndex].Data.Length > rowIndex;
        }
    }
}