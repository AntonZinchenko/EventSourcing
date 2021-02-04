using MassTransit.EntityFrameworkCoreIntegration;

namespace Bank.Orchestrators.Transfer.StatePersistence
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
