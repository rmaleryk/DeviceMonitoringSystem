namespace DMS.Monitor.Infrastructure.Persistence.Common;

public class SqlInitializationData
{
    public SqlInitializationData(string connectionString) => ConnectionString = connectionString;

    public string ConnectionString { get; }
}
