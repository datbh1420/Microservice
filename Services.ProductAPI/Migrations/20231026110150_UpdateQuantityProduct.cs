using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuantityProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: "65c37ebd-95ab-429b-844c-1e230d1a14ec",
                column: "Quantity",
                value: 300);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: "956192cb-d2a2-4957-8c4a-3500a536019c",
                column: "Quantity",
                value: 200);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "Id",
                keyValue: "c484e233-89c5-4dd0-9759-948a09a92682",
                column: "Quantity",
                value: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "products");
        }
    }
}
