namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clinics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        ClinicId = c.Int(nullable: false, identity: true),
                        ClinicName = c.Int(nullable: false),
                        ClinicAddress = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClinicId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clinics");
        }
    }
}
