namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Email", c => c.String());
            AddColumn("dbo.Results", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "Email");
            DropColumn("dbo.Bookings", "Email");
        }
    }
}
