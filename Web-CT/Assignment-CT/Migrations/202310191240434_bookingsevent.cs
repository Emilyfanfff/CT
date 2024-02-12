namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookingsevent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "EventId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "EventId");
        }
    }
}
