using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SunriseCo.Migrations
{
    public partial class IdentityReset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DBCC CHECKIDENT('Departments',RESEED,0)");
            migrationBuilder.Sql(@"DBCC CHECKIDENT('Employees',RESEED,0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
