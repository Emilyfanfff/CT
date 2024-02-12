namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Results : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserId = c.String(),
                        DoctorName = c.String(),
                        DoctorId = c.String(),
                        ResultDescription = c.String(),
                        ResultDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResultId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Results");
        }
    }
}
