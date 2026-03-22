using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedSyncedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SyncedUsers",
                columns: new[] { "UserId", "Email", "Name" },
                values: new object[,]
                {
                    { new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"), "carol.green@company.com", "Carol Green" },
                    { new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"), "philip.davis@company.com", "Philip Davis" },
                    { new Guid("251e0a52-d589-4845-ada8-daeb08be76ea"), "anna.collins@company.com", "Anna Collins" },
                    { new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841"), "louis.newton@company.com", "Louis Newton" },
                    { new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b"), "mary.allen@company.com", "Mary Allen" },
                    { new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6"), "florence.reed@company.com", "Florence Reed" },
                    { new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a"), "alice.parker@company.com", "Alice Parker" },
                    { new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"), "john.foster@company.com", "John Foster" },
                    { new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7"), "sandra.quinn@company.com", "Sandra Quinn" },
                    { new Guid("fb874eba-9c48-4340-a690-bebb2195d379"), "admin@admin.com", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("251e0a52-d589-4845-ada8-daeb08be76ea"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7"));

            migrationBuilder.DeleteData(
                table: "SyncedUsers",
                keyColumn: "UserId",
                keyValue: new Guid("fb874eba-9c48-4340-a690-bebb2195d379"));
        }
    }
}
