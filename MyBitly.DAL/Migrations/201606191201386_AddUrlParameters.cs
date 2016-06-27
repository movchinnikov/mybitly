namespace MyBitly.DAL.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddUrlParameters : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Url", "Title", c => c.String(nullable: false));
			AddColumn("dbo.Url", "CreateDate", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
			AddColumn("dbo.Url", "Clicks", c => c.Int(nullable: false, defaultValue:0));
		}
		
		public override void Down()
		{
			DropColumn("dbo.Url", "Clicks");
			DropColumn("dbo.Url", "CreateDate");
			DropColumn("dbo.Url", "Title");
		}
	}
}
