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
                name: "Consulting_Registration",
                columns: table => new
                {
                    IdConsulting = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ConsultingType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "Study abroad consulting"),
                    ContentConsulting = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "Study in USA"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "chua tu v?n"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Consulti__67487AA91F357E45", x => x.IdConsulting);
                });

            migrationBuilder.CreateTable(
                name: "EmailHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "user"),
                    IsConsultant = table.Column<bool>(type: "bit", nullable: false),
                    ConsultingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "chua tu v?n"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__B7C92638A1EA2A1D", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    IdMessage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatMess__47AAF3043C702C38", x => x.IdMessage);
                    table.ForeignKey(
                        name: "FK__ChatMessa__Recei__6754599E",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "IdUser");
                    table.ForeignKey(
                        name: "FK__ChatMessa__Sende__66603565",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    IdNews = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleNews = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NewsDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__News__4559C72DAD5F8D18", x => x.IdNews);
                    table.ForeignKey(
                        name: "FK__News__IdUser__5CD6CB2B",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Study_abroad_catalog",
                columns: table => new
                {
                    IdcategoryStab = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamecategoryStab = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Study_ab__2CD6C429BF218044", x => x.IdcategoryStab);
                    table.ForeignKey(
                        name: "FK__Study_abr__IdUse__5629CD9C",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    IdComment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdNews = table.Column<int>(type: "int", nullable: true),
                    IdVisaService = table.Column<int>(type: "int", nullable: true),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comments__57C9AD582E41940E", x => x.IdComment);
                    table.ForeignKey(
                        name: "FK__Comments__IdNews__628FA481",
                        column: x => x.IdNews,
                        principalTable: "News",
                        principalColumn: "IdNews");
                    table.ForeignKey(
                        name: "FK__Comments__IdUser__619B8048",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    IdSchool = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageSchool = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TitleSchool = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    School_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    State_City = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Average_tuition = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdcategoryStab = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__School__B544508521507AE2", x => x.IdSchool);
                    table.ForeignKey(
                        name: "FK__School__Idcatego__59063A47",
                        column: x => x.IdcategoryStab,
                        principalTable: "Study_abroad_catalog",
                        principalColumn: "IdcategoryStab");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReceiverId",
                table: "ChatMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdNews",
                table: "Comments",
                column: "IdNews");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdUser",
                table: "Comments",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_News_IdUser",
                table: "News",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_School_IdcategoryStab",
                table: "School",
                column: "IdcategoryStab");

            migrationBuilder.CreateIndex(
                name: "IX_Study_abroad_catalog_IdUser",
                table: "Study_abroad_catalog",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D10534A39E0728",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Consulting_Registration");

            migrationBuilder.DropTable(
                name: "EmailHistories");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Study_abroad_catalog");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
