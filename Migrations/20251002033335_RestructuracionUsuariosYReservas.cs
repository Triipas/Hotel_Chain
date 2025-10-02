using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class RestructuracionUsuariosYReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Usuarios_usuario_id",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.RenameColumn(
                name: "contra",
                table: "Usuarios",
                newName: "contraseña");

            migrationBuilder.AddColumn<string>(
                name: "documento",
                table: "Usuarios",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "estado",
                table: "Usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_creacion",
                table: "Usuarios",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "rol",
                table: "Usuarios",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ultimo_acceso",
                table: "Usuarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "usuario_id",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "Reservas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "estado_pago",
                table: "Reservas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_creacion",
                table: "Reservas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_modificacion",
                table: "Reservas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "numero_huespedes",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "numero_noches",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "numero_reserva",
                table: "Reservas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "solicitudes_especiales",
                table: "Reservas",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Huespedes",
                columns: table => new
                {
                    huesped_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    preferencias = table.Column<string>(type: "longtext", nullable: true),
                    notas_internas = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Huespedes", x => x.huesped_id);
                    table.ForeignKey(
                        name: "FK_Huespedes_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    staff_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    rol_detallado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    permisos_extra = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.staff_id);
                    table.ForeignKey(
                        name: "FK_Staff_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_email",
                table: "Usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_numero_reserva",
                table: "Reservas",
                column: "numero_reserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Huespedes_usuario_id",
                table: "Huespedes",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_usuario_id",
                table: "Staff",
                column: "usuario_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Usuarios_usuario_id",
                table: "Reservas",
                column: "usuario_id",
                principalTable: "Usuarios",
                principalColumn: "usuario_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Usuarios_usuario_id",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "Huespedes");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_email",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_numero_reserva",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "documento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "estado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "fecha_creacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "rol",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ultimo_acceso",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "estado_pago",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "fecha_creacion",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "fecha_modificacion",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "numero_huespedes",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "numero_noches",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "numero_reserva",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "solicitudes_especiales",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "contraseña",
                table: "Usuarios",
                newName: "contra");

            migrationBuilder.AlterColumn<int>(
                name: "usuario_id",
                table: "Reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "Reservas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    contraseña = table.Column<string>(type: "longtext", nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    puesto = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.rol_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Usuarios_usuario_id",
                table: "Reservas",
                column: "usuario_id",
                principalTable: "Usuarios",
                principalColumn: "usuario_id");
        }
    }
}
