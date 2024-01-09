using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManager.Migrations
{
    [Migration(1)]
    public class _1_AgentsTableAddedMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("AgentsTable");
        }

        public override void Up()
        {
            Create
                .Table("AgentsTable")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Address").AsString();
        }
    }
}
