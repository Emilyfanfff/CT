namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _event : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorEvents",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoctorEvents", "DoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.DoctorEvents", new[] { "DoctorId" });
            DropTable("dbo.DoctorEvents");
        }
    }
}
