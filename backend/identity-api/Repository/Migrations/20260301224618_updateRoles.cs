using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class updateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1704848a-5252-4f1f-82ef-9c4a751683d9"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Manager", "MANAGER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("418b0ac3-4dba-4ccf-883e-63fdd7ac62a9"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Employee", "EMPLOYEE" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1704848a-5252-4f1f-82ef-9c4a751683d9"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Chefe", "CHEFE" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("418b0ac3-4dba-4ccf-883e-63fdd7ac62a9"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Administrador", "ADMINISTRADOR" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("52c71224-fe99-4ec0-9130-0f0de200eec9"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Funcionario", "FUNCIONARIO" });
        }
    }
}
