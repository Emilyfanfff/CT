namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointments5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "DoctorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "DoctorId");
        }
    }
}
