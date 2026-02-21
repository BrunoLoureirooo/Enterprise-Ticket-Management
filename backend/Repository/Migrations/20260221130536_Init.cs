using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1704848a-5252-4f1f-82ef-9c4a751683d9"), null, "Chefe", "CHEFE" },
                    { new Guid("418b0ac3-4dba-4ccf-883e-63fdd7ac62a9"), null, "Administrador", "ADMINISTRADOR" },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), null, "Funcionario", "FUNCIONARIO" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImageUrl", "LockoutEnabled", "LockoutEnd", "Nome", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"), 0, "5701278b-d7ec-40ba-9cd6-708c44563c16", "cazevedo@pessoal.pt", true, null, false, null, "Carla Alexandra Gomes Azevedo", "CAZEVEDO@PESSOAL.PT", null, "AQAAAAIAAYagAAAAEA/Jq/pyjdaYZPHHJpHbqp+i+5+OsgUefuEAQkvooVZb58I6rxZyxF8urU87vWNi3w==", null, false, null, null, "75da7095-038e-4aca-970a-23122ef0e9c7", false, null },
                    { new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"), 0, "e5607436-cab6-4d49-bf0a-2a3f595fd1f7", "fdias@pessoal.pt", true, null, false, null, "Filipe Manuel Marques da Silva Dias", "FDIAS@PESSOAL.PT", null, "AQAAAAIAAYagAAAAEPndwjT6ExopmCYz9Aq9PrmtPPkq9lUQdddp76wbeRyhTVzEbGN+74hauKGZUWrVng==", null, false, null, null, "bf601b92-1997-477c-a5b3-8958adc58474", false, null },
                    { new Guid("251e0a52-d589-4845-ada8-daeb08be76ea"), 0, "1635a002-94d3-44ef-86bb-57748d6b5654", "acasaco@pessoal.pt", true, null, false, null, "Ana Cristina Leite Casaco", "ACASACO@PESSOAL.PT", null, "AQAAAAIAAYagAAAAEIDVzMG1GxuV1nz0VAKeh2s5+6SNWxHgrMQVsfltRGRgB30T5Svv8i96lxbNy6O5ew==", null, false, null, null, "9dce8fcf-555e-475b-a676-0336b4a2886e", false, null },
                    { new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841"), 0, "c02f62ed-c493-4c6b-9925-7ed27623346f", "lneves@pessoal.pt", true, null, false, null, "Luís Augusto Pinto Teixeira Neves", "LNEVES@PESSOAL.PT", null, "AQAAAAIAAYagAAAAEG9Qpe1DBFgyk9AfdJGGl6tzKtSDzBGv47Q6lSlf4vzR06PNT7XDSBirnaOeXgpq8w==", null, false, null, null, "fb09937d-40b0-4999-ae4b-8c551449e296", false, null },
                    { new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b"), 0, "57635711-b231-4e09-866c-cb8970cab382", "malves@pessoal.pt", true, null, false, null, "Maria João Pereira Alves", "MALVES@PESSOAL.PT", null, "AQAAAAIAAYagAAAAEDOqoY/A9Id4uSg0FZ2Pn1IFcP59oG/uARhSavAgtFEgJ7ZcvoQkQVjjCvzc+BFQnQ==", null, false, null, null, "214c00b2-682f-4310-84ba-8cd7f466f20d", false, null },
                    { new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6"), 0, "85ce73c8-afd1-4679-bd8b-8cf37c891da0", "frebelo@pessoal.pt", true, null, false, null, "Filomena Maria Cruz Rebelo", "FREBELO@PESSOAL.PT", null, "AQAAAAIAAYagAAAAEBDhOkl+BmTH6/kLA+Lwjzg9lgE3khGaNJ13UhPR2EM5dQLs/k7mjif9+3KvB7i/wQ==", null, false, null, null, "6db4e253-246d-4ed7-b833-e0d896805c07", false, null },
                    { new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a"), 0, "e0981140-1b7c-4f33-a556-8ce519876d66", "apinto@pessoal.pt", true, null, false, null, "Ana Alexandra Valença Pinto", "APINTO@PESSOAL.PT", null, "AQAAAAIAAYagAAAAENLLmW3rVmFlf8TaNCL5neMQ2dlb4Tqdmk8wmyech/l1LR0NrhCyYPNg5GVPixZE9Q==", null, false, null, null, "82a360c3-37f9-42da-9dd2-e8af93b864fc", false, null },
                    { new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"), 0, "5e9c546c-5c8b-446b-834d-89d3fb270ac9", "jfonseca@pessoal.pt", true, null, false, null, "João Carlos Araújo Fonseca", "JFONSECA@PESSOAL.PT", null, "AQAAAAIAAYagAAAAENmFh/knB5U+tb/bOSLPIHkBqqkgHUv0msS8BrGdstihIWLsJom+6pAK5ijyjJ2yFA==", null, false, null, null, "c4225b9c-3e5c-4526-a267-b55fc1dbdde8", false, null },
                    { new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7"), 0, "e4e88c59-a13a-44b2-bedf-11fcb65c5d2b", "sconceicao@pessoal.pt", true, null, false, null, "Sandra Regina Queirós Azevedo Conceição", "SCONCEICAO@PESSOAL.PT", null, "AQAAAAIAAYagAAAAEBunlXGl4cP+eVgXp5N8WTrsPdSoFKudcKMZg47f2Ql3eJVJvOXqaYWopgeERwKVMw==", null, false, null, null, "1d4e98cc-c78e-4abe-8164-aa41358c89b0", false, null },
                    { new Guid("fb874eba-9c48-4340-a690-bebb2195d379"), 0, "be7e12e8-b4d4-4136-8be0-a022c5ae1b77", "admin@admin.pt", true, null, false, null, "Admin", "ADMIN@ADMIN.PT", null, "AQAAAAIAAYagAAAAEKCqOA4Ilzwt3W3BSnRTmiTH4ahIz+YHJYBKCC8o/a/Mj4G0InEWw244yJamcd8tCA==", null, false, null, null, "8f15dade-0a81-4238-ae5c-cad2ca8d0aae", false, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("251e0a52-d589-4845-ada8-daeb08be76ea") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216") },
                    { new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"), new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7") },
                    { new Guid("418b0ac3-4dba-4ccf-883e-63fdd7ac62a9"), new Guid("fb874eba-9c48-4340-a690-bebb2195d379") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
