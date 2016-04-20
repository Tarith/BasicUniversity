namespace BasicUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplexDataModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Budget = c.Decimal(nullable: false, storeType: "money"),
                        StartDate = c.DateTime(nullable: false),
                        InstructorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instructor", t => t.InstructorId)
                .Index(t => t.InstructorId);
            
            CreateTable(
                "dbo.Instructor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OfficeAssignment",
                c => new
                    {
                        InstructorId = c.Int(nullable: false),
                        Location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.InstructorId)
                .ForeignKey("dbo.Instructor", t => t.InstructorId)
                .Index(t => t.InstructorId);
            
            CreateTable(
                "dbo.InstructorCourse",
                c => new
                    {
                        Instructor_Id = c.Int(nullable: false),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Instructor_Id, t.Course_Id })
                .ForeignKey("dbo.Instructor", t => t.Instructor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Course", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Instructor_Id)
                .Index(t => t.Course_Id);

            //create a department for course to point to
            Sql("INSERT INTO dbo.Department (Name, Budget, StartDate) VALUES (' - ', 0.00, GETDATE())");
            //default value for FK points to department created above


            //AddColumn("dbo.Course", "DepartmentId", c => c.Int(nullable: false));
            AddColumn("dbo.Course", "DepartmentId", c => c.Int(nullable: false, defaultValue: 1));
            AlterColumn("dbo.Course", "Title", c => c.String(maxLength: 50));
            AlterColumn("dbo.Student", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Student", "FirstName", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Course", "DepartmentId");
            AddForeignKey("dbo.Course", "DepartmentId", "dbo.Department", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Course", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Department", "InstructorId", "dbo.Instructor");
            DropForeignKey("dbo.OfficeAssignment", "InstructorId", "dbo.Instructor");
            DropForeignKey("dbo.InstructorCourse", "Course_Id", "dbo.Course");
            DropForeignKey("dbo.InstructorCourse", "Instructor_Id", "dbo.Instructor");
            DropIndex("dbo.InstructorCourse", new[] { "Course_Id" });
            DropIndex("dbo.InstructorCourse", new[] { "Instructor_Id" });
            DropIndex("dbo.OfficeAssignment", new[] { "InstructorId" });
            DropIndex("dbo.Department", new[] { "InstructorId" });
            DropIndex("dbo.Course", new[] { "DepartmentId" });
            AlterColumn("dbo.Student", "FirstName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Student", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Course", "Title", c => c.String());
            DropColumn("dbo.Course", "DepartmentId");
            DropTable("dbo.InstructorCourse");
            DropTable("dbo.OfficeAssignment");
            DropTable("dbo.Instructor");
            DropTable("dbo.Department");
        }
    }
}
