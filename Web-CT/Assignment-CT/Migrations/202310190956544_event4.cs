namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class event4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DoctorEvents", "Booked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DoctorEvents", "Booked");
        }
    }
}
