using MassTransit.EntityFrameworkCoreIntegration;

namespace Transfer.Storage
{
    public class CustomSqlLockStatementProvider :
        SqlLockStatementProvider
    {
        const string DefaultSchemaName = "dbo";

        public CustomSqlLockStatementProvider(string lockStatement)
            : base(DefaultSchemaName, lockStatement)
        {
        }
    }
}
