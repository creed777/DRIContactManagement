using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DRIContactManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DRIName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DRIEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DelegateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DelegateEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ServiceId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
