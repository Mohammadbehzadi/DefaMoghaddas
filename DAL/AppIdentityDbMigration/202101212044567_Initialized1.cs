namespace DAL.AppIdentityDbMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialized1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionLog",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        ModifyDateTime = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Description = c.String(),
                        BranchCode = c.String(),
                        ActionName = c.String(),
                        ContollerName = c.String(),
                        Ip = c.String(),
                        User = c.String(),
                        Type = c.String(),
                        Person_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        ModifyDateTime = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Description = c.String(),
                        Name = c.String(),
                        LastName = c.String(),
                        FatherName = c.String(),
                        NationalCode = c.Long(),
                        Code = c.String(),
                        BirthDate = c.DateTime(),
                        Telphone = c.String(),
                        TelFax = c.String(),
                        Mobile = c.String(),
                        Address = c.String(),
                        State = c.Boolean(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        ParentId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        ModifyDateTime = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Description = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserEmail",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        EmailId = c.Guid(nullable: false),
                        IsView = c.Boolean(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.UserId, t.EmailId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Email", t => t.EmailId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.EmailId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                        Person_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        ModifyDateTime = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        Description = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionsRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PermissionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.PermissionId })
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermissionsRoles", "RoleId", "dbo.IdentityRoles");
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.PermissionsRoles", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.UserEmail", "EmailId", "dbo.Email");
            DropForeignKey("dbo.UserEmail", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUsers", "Person_Id", "dbo.People");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ActionLog", "Person_Id", "dbo.People");
            DropForeignKey("dbo.People", "ParentId", "dbo.People");
            DropIndex("dbo.PermissionsRoles", new[] { "PermissionId" });
            DropIndex("dbo.PermissionsRoles", new[] { "RoleId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUsers", new[] { "Person_Id" });
            DropIndex("dbo.UserEmail", new[] { "EmailId" });
            DropIndex("dbo.UserEmail", new[] { "UserId" });
            DropIndex("dbo.People", new[] { "ParentId" });
            DropIndex("dbo.ActionLog", new[] { "Person_Id" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.PermissionsRoles");
            DropTable("dbo.Permissions");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.UserEmail");
            DropTable("dbo.Email");
            DropTable("dbo.People");
            DropTable("dbo.ActionLog");
        }
    }
}
