using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackEnd.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLocalPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageLocalPath", "ImagePath", "Name", "Price" },
                values: new object[,]
                {
                    { "65c37ebd-95ab-429b-844c-1e230d1a14ec", "Keychain", "Keychains are a more convenient key decoration and hanging product, helping you show off the difference in your key cluster. Using a key hanger will bring you more convenience and ease when storing and carrying with you, limiting misplacement or loss.", "Images\\ShibaHipHop.png", "https://localhost:7001/Images/ShibaHiphop.png", "SHIBA HIPHOP FLEXIBLE KEYCHAIN", 35000.0 },
                    { "956192cb-d2a2-4957-8c4a-3500a536019c", "Lego", "Nowadays, instead of playing with iPads, cell phones, watching TV, etc., which increases the possibility of screen light harming your eyes, playing with educational toys will help you both relax and Develop thinking ability and creativity!", "Images\\AstronomyLego.png", "https://localhost:7001/Images/AstronomyLego.png", "LEGO ASTRONOMY IN FLY", 135000.0 },
                    { "c484e233-89c5-4dd0-9759-948a09a92682", "Stuffed animal", "Tired, tense, stressed,...after every hour of studying, taking exams, working??? Let To relieve your tired feeling with these extremely funny and cute elastic toys!", "Images\\CriticChickens.png", "https://localhost:7001/Images/CriticChickens.png", "STRESS RELEASING ELASTIC TOYS CUTE CRITIC CHICKENS", 68000.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
