namespace BelajarCRUDWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_model_item : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tb_M_Item", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tb_M_Item", "Price", c => c.String());
        }
    }
}
