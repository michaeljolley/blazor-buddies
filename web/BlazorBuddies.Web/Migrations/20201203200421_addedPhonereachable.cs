using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorBuddies.Web.Migrations
{
    public partial class addedPhonereachable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NumberReachable",
                table: "Donors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberReachable",
                table: "Donors");
        }
    }
}
