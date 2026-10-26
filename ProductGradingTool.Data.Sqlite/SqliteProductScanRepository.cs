namespace ProductGradingTool.Data.Sqlite;

using Microsoft.Data.Sqlite;
using ProductGradingTool.Data;
using ProductGradingTool.Model;
using System.Collections.Immutable;

/// <summary>
/// 
/// </summary>
public class SqliteProductScanRepository : IProductScanRepository
{
    private readonly string _connectionString = string.Empty;

    public SqliteProductScanRepository(string connectionString)
    {
        _connectionString = connectionString;
        InitializeTables();
    }

    private void InitializeTables()
    {
        string commandText = @"
CREATE TABLE IF NOT EXISTS ProductScan (
    ProductId TEXT NOT NULL,
    UserId TEXT NOT NULL,
    TimeStamp INTEGER NOT NULL DEFAULT (unixepoch()),
    Weight TEXT NULL,
    FatValue TEXT NULL,
    LeanValue TEXT NULL,
    Grade TEXT NULL,
    SetInvalid INTEGER NOT NULL DEFAULT 0
)";
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public IEnumerable<ProductScan> QuerySaved()
    {
        List<ProductScan> list = new List<ProductScan>();
        string commandText = @"
SELECT
    ProductId,
    UserId,
    DATETIME(TimeStamp, 'unixepoch'),
    Weight,
    FatValue,
    LeanValue,
    Grade,
    SetInvalid
FROM
    ProductScan
";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(commandText, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new ProductScan
                    {
                        ProductId = reader.GetString(0),
                        UserId = reader.GetString(1),
                        TimeStamp = Convert.ToDateTime(reader.GetString(2)),
                        Weight = reader.GetString(3),
                        Grade = new GradeScan
                        {
                            FatValue = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                            LeanValue = !reader.IsDBNull(5) ? reader.GetString(5) : null, 
                            Grade = !reader.IsDBNull(6) ? reader.GetString(6) : null
                        },
                        SetInvalid = reader.GetString(7) != "0"
                    });
                }
            }
        }

        return list.ToImmutableArray();
    }

    public void Save(ProductScan scan)
    {
        string commandText = @"
INSERT INTO ProductScan (
    ProductId,
    UserId,
    Weight,
    FatValue,
    LeanValue,
    Grade,
    SetInvalid
)
VALUES(
    @productId,
    @userId,
    @weight,
    @fatValue,
    @leanValue,
    @grade,
    @setInvalid
)";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@productId", scan.ProductId);
                command.Parameters.AddWithValue("@userId", scan.UserId);
                command.Parameters.AddWithValue("@weight", scan.Weight);
                if (scan.Grade != null)
                {
                    command.Parameters.AddWithValue("@fatValue", scan.Grade.FatValue);
                    command.Parameters.AddWithValue("@leanValue", scan.Grade.LeanValue);
                    command.Parameters.AddWithValue("@grade", scan.Grade.Grade);
                }
                else
                {
                    command.Parameters.AddWithValue("@fatValue", DBNull.Value);
                    command.Parameters.AddWithValue("@leanValue", DBNull.Value);
                    command.Parameters.AddWithValue("@grade", DBNull.Value);
                }
                command.Parameters.AddWithValue("@setInvalid", scan.SetInvalid);

                command.ExecuteNonQuery();
            }
        }
    }
}