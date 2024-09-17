using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregamosColumnaALaTablaAspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "AspNetUsers");
        }
    }
}
