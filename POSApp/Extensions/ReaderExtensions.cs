using System.Data;

namespace POSApp.Extensions;

public static class ReaderExtensions
{
    public static T? GetNullable<T>(this IDataReader reader, string column) where T : struct =>
        reader.IsDBNull(reader.GetOrdinal(column)) ? null : (T?)reader.GetValue(reader.GetOrdinal(column));
}
