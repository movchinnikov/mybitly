namespace MyBitly.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddUrl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Url",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hash = c.String(nullable: false, maxLength: 8),
                        LongUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Hash, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Url", new[] { "Hash" });
            DropTable("dbo.Url");
        }
    }
}
