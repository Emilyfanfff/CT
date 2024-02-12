namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class event3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DoctorEvents", "DoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.DoctorEvents", new[] { "DoctorId" });
            AlterColumn("dbo.DoctorEvents", "DoctorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DoctorEvents", "DoctorId");
            AddForeignKey("dbo.DoctorEvents", "DoctorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoctorEvents", "DoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.DoctorEvents", new[] { "DoctorId" });
            AlterColumn("dbo.DoctorEvents", "DoctorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DoctorEvents", "DoctorId");
            AddForeignKey("dbo.DoctorEvents", "DoctorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
