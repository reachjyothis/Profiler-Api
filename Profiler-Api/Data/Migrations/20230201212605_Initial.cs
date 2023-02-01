using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfilerApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordSalt = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Date", "Email", "PasswordHash", "PasswordSalt", "Role", "Username" },
                values: new object[] { 1, new DateTime(2023, 2, 2, 2, 56, 5, 323, DateTimeKind.Local).AddTicks(725), "admin@localhost.com", "1D57743E41C84192A747A6A2F692FC97B5A2C3D1DB7CDCA328463B85FFB7DBF2457C419486640E458D0EC1664AA2E80EA0E7289F8EF79DBEE9DF1BCCDF2388A7", "4C501A810366409E230AECB8A57D1DB01F834DDDA53416B32D1E7C9EDB9A7DB8B07932EFA3FD773552BCFE097426E713E1D3D525E5B632D3447C0A91EC4860D6", "A", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
