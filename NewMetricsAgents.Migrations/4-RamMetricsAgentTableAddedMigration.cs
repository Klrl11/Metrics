using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Migrations
{
    [Migration(4)]
    public class _4_RamMetricsAgentTableAddedMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("rammetricsAgent");
        }

        public override void Up()
        {
            Create
                .Table("rammetricsAgent")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
