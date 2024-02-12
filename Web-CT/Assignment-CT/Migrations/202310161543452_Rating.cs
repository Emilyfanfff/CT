namespace Assignment_CT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Rating", c => c.Int());
            AddColumn("dbo.Bookings", "Rated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "RatingComment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "RatingComment");
            DropColumn("dbo.Bookings", "Rated");
            DropColumn("dbo.Bookings", "Rating");
        }
    }
}
