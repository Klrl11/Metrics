using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Migrations
{
    [Migration(2)]
    public class _2_HddMetricsTableAddedMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("hddmetrics");
        }

        public override void Up()
        {
            Create
                .Table("hddmetrics")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }
    }
}
