namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        AppointmentDate = c.DateTime(nullable: false),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        PatientId = c.String(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.AppointmentId);
            
            CreateIndex("dbo.Appointments", "DoctorId");
            AddForeignKey("dbo.Appointments", "DoctorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
