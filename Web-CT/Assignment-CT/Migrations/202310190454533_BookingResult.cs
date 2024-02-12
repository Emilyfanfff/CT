namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Result_ResultId", c => c.Int());
            AddColumn("dbo.Results", "BookingId", c => c.Int(nullable: false));
            CreateIndex("dbo.Bookings", "Result_ResultId");
            AddForeignKey("dbo.Bookings", "Result_ResultId", "dbo.Results", "ResultId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Result_ResultId", "dbo.Results");
            DropIndex("dbo.Bookings", new[] { "Result_ResultId" });
            DropColumn("dbo.Results", "BookingId");
            DropColumn("dbo.Bookings", "Result_ResultId");
        }
    }
}
