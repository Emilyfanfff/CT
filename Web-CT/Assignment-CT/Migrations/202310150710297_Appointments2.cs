namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointments2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        AppointmentDate = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false),
                        Image = c.Binary(),
                        AspNetUsers_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUsers_Id)
                .Index(t => t.AspNetUsers_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "AspNetUsers_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "AspNetUsers_Id" });
            DropTable("dbo.Bookings");
        }
    }
}
