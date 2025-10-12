using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaFotosResenas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FotosReseñas",
                columns: table => new
                {
                    FotoResenaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ResenaId = table.Column<int>(type: "int", nullable: false),
                    UrlFoto = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotosReseñas", x => x.FotoResenaId);
                    table.ForeignKey(
                        name: "FK_FotosReseñas_Reseñas_ResenaId",
                        column: x => x.ResenaId,
                        principalTable: "Reseñas",
                        principalColumn: "ResenaId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FotosReseñas_ResenaId",
                table: "FotosReseñas",
                column: "ResenaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FotosReseñas");
        }
    }
}
