namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DesAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Description");
        }
    }
}
