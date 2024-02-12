namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class event1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DoctorEvents", "DoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.DoctorEvents", new[] { "DoctorId" });
            DropColumn("dbo.DoctorEvents", "DoctorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DoctorEvents", "DoctorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DoctorEvents", "DoctorId");
            AddForeignKey("dbo.DoctorEvents", "DoctorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
