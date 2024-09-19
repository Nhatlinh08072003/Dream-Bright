using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dream_Bridge.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__admin__43AA4141467AED95", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__countrie__7E8CD055B5ED544E", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__C52E0BA8886FEE9A", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "schools",
                columns: table => new
                {
                    school_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    school_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    country_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__schools__27CA6CF4AAF2F826", x => x.school_id);
                    table.ForeignKey(
                        name: "FK__schools__country__4222D4EF",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "country_id");
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    country_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__students__2A33069AD38804B1", x => x.student_id);
                    table.ForeignKey(
                        name: "FK__students__countr__3C69FB99",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "country_id");
                });

            migrationBuilder.CreateTable(
                name: "consultations",
                columns: table => new
                {
                    consultation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    student_id = table.Column<int>(type: "int", nullable: true),
                    employee_id = table.Column<int>(type: "int", nullable: true),
                    consultation_date = table.Column<DateOnly>(type: "date", nullable: true),
                    consultation_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__consulta__650FE0FB83277122", x => x.consultation_id);
                    table.ForeignKey(
                        name: "FK__consultat__emplo__45F365D3",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id");
                    table.ForeignKey(
                        name: "FK__consultat__stude__44FF419A",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id");
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    contact_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    student_id = table.Column<int>(type: "int", nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__contacts__024E7A8637DE8559", x => x.contact_id);
                    table.ForeignKey(
                        name: "FK__contacts__studen__5BE2A6F2",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id");
                });

            migrationBuilder.CreateTable(
                name: "testimonials",
                columns: table => new
                {
                    testimonial_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    student_id = table.Column<int>(type: "int", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__testimon__2E3B190CDD7FED37", x => x.testimonial_id);
                    table.ForeignKey(
                        name: "FK__testimoni__stude__4AB81AF0",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__admin__AB6E616498076E62",
                table: "admin",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultations_employee_id",
                table: "consultations",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_consultations_student_id",
                table: "consultations",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_student_id",
                table: "contacts",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_schools_country_id",
                table: "schools",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_country_id",
                table: "students",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_testimonials_student_id",
                table: "testimonials",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "consultations");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "schools");

            migrationBuilder.DropTable(
                name: "testimonials");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
