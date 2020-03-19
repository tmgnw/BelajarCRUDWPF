namespace BelajarCRUDWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsuppliermodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_m_supplier",       //db dari table supplier
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nama = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);     //primary key
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbl_m_supplier");
        }
    }
}
