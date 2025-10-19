using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Hoteles",
                columns: table => new
                {
                    hotel_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    direccion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "longtext", nullable: true),
                    telefono_contacto = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Estado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Pais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Latitud = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Longitud = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Calificacion = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CantidadResenas = table.Column<int>(type: "int", nullable: true),
                    PrecioMin = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PrecioMax = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Moneda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    CheckInTime = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    CheckOutTime = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    PoliticaCancelacion = table.Column<string>(type: "longtext", nullable: true),
                    MascotasPermitidas = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    FumarPermitido = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ContactoEmail = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hoteles", x => x.hotel_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Avatar = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    documento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    contraseña = table.Column<string>(type: "longtext", nullable: false),
                    rol = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    DireccionCalle = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    DireccionCiudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    DireccionEstado = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CodigoPostal = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    DireccionPais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ContactoEmergenciaNombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ContactoEmergenciaTelefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    ContactoEmergenciaRelacion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    ultimo_acceso = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.usuario_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Habitaciones",
                columns: table => new
                {
                    habitacion_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    numero_habitacion = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    tipo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    capacidad = table.Column<int>(type: "int", nullable: false),
                    CapacidadAdultos = table.Column<int>(type: "int", nullable: true),
                    CapacidadNinos = table.Column<int>(type: "int", nullable: true),
                    CantidadCamas = table.Column<int>(type: "int", nullable: true),
                    TipoCama = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    TamanoM2 = table.Column<int>(type: "int", nullable: true),
                    PrecioBase = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PrecioImpuestos = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    precio_noche = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Moneda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    disponible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HabitacionesDisponibles = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    hotel_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitaciones", x => x.habitacion_id);
                    table.ForeignKey(
                        name: "FK_Habitaciones_Hoteles_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "Hoteles",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HotelAmenidades",
                columns: table => new
                {
                    AmenidadHotelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    Amenidad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelAmenidades", x => x.AmenidadHotelId);
                    table.ForeignKey(
                        name: "FK_HotelAmenidades_Hoteles_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteles",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HotelCaracteristicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    Caracteristica = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelCaracteristicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelCaracteristicas_Hoteles_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteles",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                name: "PermisosUsuarios",
                columns: table => new
                {
                    PermisoUsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Permiso = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosUsuarios", x => x.PermisoUsuarioId);
                    table.ForeignKey(
                        name: "FK_PermisosUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
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
                    permisos_extra = table.Column<string>(type: "longtext", nullable: true),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    Departamento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Fechadeingreso = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.staff_id);
                    table.ForeignKey(
                        name: "FK_Staff_Hoteles_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteles",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staff_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HabitacionAmenidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    HabitacionId = table.Column<int>(type: "int", nullable: false),
                    Amenidad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitacionAmenidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitacionAmenidades_Habitaciones_HabitacionId",
                        column: x => x.HabitacionId,
                        principalTable: "Habitaciones",
                        principalColumn: "habitacion_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NombreArchivo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: true),
                    HabitacionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imagenes_Habitaciones_HabitacionId",
                        column: x => x.HabitacionId,
                        principalTable: "Habitaciones",
                        principalColumn: "habitacion_id");
                    table.ForeignKey(
                        name: "FK_Imagenes_Hoteles_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteles",
                        principalColumn: "hotel_id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    reserva_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    numero_reserva = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    habitacion_id = table.Column<int>(type: "int", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: true),
                    fecha_inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    numero_huespedes = table.Column<int>(type: "int", nullable: false),
                    GuestsAdults = table.Column<int>(type: "int", nullable: true),
                    GuestsChildren = table.Column<int>(type: "int", nullable: true),
                    GuestFirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    GuestLastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    GuestEmail = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    GuestPhone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    numero_noches = table.Column<int>(type: "int", nullable: false),
                    RoomRate = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    precio_total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Taxes = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    estado = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    estado_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    solicitudes_especiales = table.Column<string>(type: "longtext", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.reserva_id);
                    table.ForeignKey(
                        name: "FK_Reservas_Habitaciones_habitacion_id",
                        column: x => x.habitacion_id,
                        principalTable: "Habitaciones",
                        principalColumn: "habitacion_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Hoteles_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteles",
                        principalColumn: "hotel_id");
                    table.ForeignKey(
                        name: "FK_Reservas_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Resena",
                columns: table => new
                {
                    ResenaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Calificacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comentario = table.Column<string>(type: "longtext", nullable: false),
                    VecesUtil = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resena", x => x.ResenaId);
                    table.ForeignKey(
                        name: "FK_Resena_Hoteles_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hoteles",
                        principalColumn: "hotel_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resena_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "reserva_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resena_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_HabitacionAmenidades_HabitacionId",
                table: "HabitacionAmenidades",
                column: "HabitacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitaciones_hotel_id",
                table: "Habitaciones",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_HotelAmenidades_HotelId",
                table: "HotelAmenidades",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelCaracteristicas_HotelId",
                table: "HotelCaracteristicas",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Huespedes_usuario_id",
                table: "Huespedes",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_HabitacionId",
                table: "Imagenes",
                column: "HabitacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_HotelId",
                table: "Imagenes",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosUsuarios_UsuarioId",
                table: "PermisosUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Resena_HotelId",
                table: "Resena",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Resena_ReservaId",
                table: "Resena",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Resena_UsuarioId",
                table: "Resena",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_habitacion_id",
                table: "Reservas",
                column: "habitacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_HotelId",
                table: "Reservas",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_numero_reserva",
                table: "Reservas",
                column: "numero_reserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_usuario_id",
                table: "Reservas",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_HotelId",
                table: "Staff",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_usuario_id",
                table: "Staff",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_email",
                table: "Usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitacionAmenidades");

            migrationBuilder.DropTable(
                name: "HotelAmenidades");

            migrationBuilder.DropTable(
                name: "HotelCaracteristicas");

            migrationBuilder.DropTable(
                name: "Huespedes");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "PermisosUsuarios");

            migrationBuilder.DropTable(
                name: "Resena");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Habitaciones");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Hoteles");
        }
    }
}
