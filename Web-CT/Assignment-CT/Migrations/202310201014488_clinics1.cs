namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clinics1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clinics", "ClinicName", c => c.String());
            AlterColumn("dbo.Clinics", "ClinicAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clinics", "ClinicAddress", c => c.Int(nullable: false));
            AlterColumn("dbo.Clinics", "ClinicName", c => c.Int(nullable: false));
        }
    }
}
