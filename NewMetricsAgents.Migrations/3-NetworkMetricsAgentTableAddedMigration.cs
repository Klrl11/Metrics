using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Migrations
{
    [Migration(3)]
    public class _3_NetworkMetricsAgentTableAddedMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("networkmetricsAgent");
        }

        public override void Up()
        {
            Create
                .Table("networkmetricsAgent")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
