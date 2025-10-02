using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposImagenS3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreArchivo",
                table: "Imagenes",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSubida",
                table: "Imagenes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubidoPor",
                table: "Imagenes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Tamano",
                table: "Imagenes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoMime",
                table: "Imagenes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlS3",
                table: "Imagenes",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaSubida",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "SubidoPor",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "Tamano",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "TipoMime",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "UrlS3",
                table: "Imagenes");

            migrationBuilder.AlterColumn<string>(
                name: "NombreArchivo",
                table: "Imagenes",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
