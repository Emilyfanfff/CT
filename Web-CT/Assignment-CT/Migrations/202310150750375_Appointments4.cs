namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointments4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookings", "UserId", c => c.String(nullable: false));
        }
    }
}
