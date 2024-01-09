using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(1)]
    public class _1_CpuMetricsAgentTableAddedMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("cpumetricsAgent");
        }

        public override void Up()
        {
            Create
                .Table("cpumetricsAgent")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
