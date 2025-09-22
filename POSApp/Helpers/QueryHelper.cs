using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace POSApp.Helpers;

public class EmptyTableException : Exception;
public class NoPreviousRecordException : Exception;
public class NoNextRecordException : Exception;
public class NoRecordException : Exception;

public enum MoveDirection
{
    First,
    Last,
    Previous,
    Next
}

public class MasterInfo<T>
{
    public required T Data { get; set; }
    public required long ActualRecordCount { get; set; }
    public required long RecordCount { get; set; }
    public required long CurrentRecordNumber { get; set; }
}

public class QueryHelper(DbHelper db)
{
    public async Task<MasterInfo<T>> GetMasterInfo<T>(
        string masterQuery,
        string tableName,
        string masterIdFieldName,
        MoveDirection moveDirection,
        Func<IDataReader, T> reader,
        string whereClause = "",
        long currentRecordId = 0
    )
    {
        var actualRecordCount = (await db.ExecuteQueryAsync(
            $"SELECT COUNT(*) as RecCount FROM {tableName} {whereClause}",
            map => map.GetInt32(0)
        )).First();

        if (actualRecordCount == 0)
        {
            throw new EmptyTableException();
        }
        
        var minRecordIdQuery = $"SELECT MIN({masterIdFieldName}) FROM ({masterQuery}) AS TempTable";
        var maxRecordIdQuery = $"SELECT MAX({masterIdFieldName}) FROM ({masterQuery}) AS TempTable";

        long targetRecordId;
        switch (moveDirection)
        {
            case MoveDirection.First:
                targetRecordId = (await db.ExecuteQueryAsync(minRecordIdQuery, reader => reader.GetInt64(0))).First();
                break;

            case MoveDirection.Last:
                targetRecordId = (await db.ExecuteQueryAsync(maxRecordIdQuery, reader => reader.GetInt64(0))).First();
                break;

            case MoveDirection.Previous:
                string previousQuery = $@"
                SELECT TOP 1 {masterIdFieldName}
                FROM ({masterQuery}) AS TempTable
                WHERE {masterIdFieldName} < @currentRecordId
                ORDER BY {masterIdFieldName} DESC";

                var previousResults = await db.ExecuteQueryAsync(
                    previousQuery,
                    map => map.GetInt64(0),
                    new SqlParameter {ParameterName  = "@currentRecordId", Value = currentRecordId }
                );

                var enumerable = previousResults as long[] ?? previousResults.ToArray();
                if (!enumerable.Any())
                    throw new NoPreviousRecordException();

                targetRecordId = enumerable.First();
                break;

            case MoveDirection.Next:
                string nextQuery = $@"
                SELECT TOP 1 {masterIdFieldName}
                FROM ({masterQuery}) AS TempTable
                WHERE {masterIdFieldName} > @currentRecordId
                ORDER BY {masterIdFieldName} ASC";

                var nextResults = await db.ExecuteQueryAsync(
                    nextQuery,
                    map => map.GetInt64(0),
                    new SqlParameter { ParameterName = "@currentRecordId", Value = currentRecordId }
                );

                var nextRecodEnumerable = nextResults as long[] ?? nextResults.ToArray();
                if (!nextRecodEnumerable.Any())
                    throw new NoNextRecordException();

                targetRecordId = nextRecodEnumerable.First();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
        }
        
        string recordCountQuery = $@"
        SELECT COUNT(*) as RecCount
        FROM ({masterQuery}) AS TempTable";
        
        var recordCount = (await db.ExecuteQueryAsync(
            recordCountQuery,
            map => map.GetInt32(0)
        )).First();
       
        string currentRecordNumberQuery = $@"
        SELECT COUNT(*) as RecCount
        FROM ({masterQuery}) AS TempTable
        WHERE {masterIdFieldName} <= @masterId";
        
        var currentRecordNumber = (await db.ExecuteQueryAsync(
            currentRecordNumberQuery,
            map => map.GetInt32(0),
            new SqlParameter {ParameterName = "@masterId", Value = targetRecordId}
        )).First();
        
        string finalQuery = $@"
        SELECT *
        FROM ({masterQuery}) AS TempTable
        WHERE {masterIdFieldName} = @masterId";
        
        

        var masterRecord = await db.ExecuteQueryAsync(
            finalQuery,
            reader,
            new SqlParameter {ParameterName = "@masterId", Value = targetRecordId}
        );

        var record = masterRecord as T[] ?? masterRecord.ToArray();
        if (!record.Any())
        {
            throw new NoRecordException();
        }

        var result = record.First();
        return new MasterInfo<T> { Data = result, CurrentRecordNumber = currentRecordNumber, ActualRecordCount = actualRecordCount, RecordCount = recordCount };
    }
}