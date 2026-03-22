using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamMembershipSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncedTeamMemberships",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsLeader = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncedTeamMemberships", x => new { x.UserId, x.TeamId });
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENd+H4/UO5UsuweYxqRKQZ1ckeGD4RDg/rjcMPlKrJnbBN3pxEBjNyaK+bsd0a+R6Q==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN56Uq28T10mZqm91rIwrmKyyOJIuukJ68mJpe7Vyjb/3MIGmqsKTkmSh3GMtuZyhg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("251e0a52-d589-4845-ada8-daeb08be76ea"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELwxLIFnUF+fafXZuxptFDK+q2GHdJBug0HPqVqrvbBccBwMp2iEoFeMra8daeWKHw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHIxLYG94UqUhPPcBPUzh2B3F5ZNhRzD2/Vd6t2Y+lPiO5VeVE6JQuyIDFbwDeEXKQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI4ajlUfhyTiVzYVze0pbK+M07kinowDfjH1ra8mjR0VEs7gDL+qDVmEJy9Zc/tKPQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELV2IbSBWH3C4O+owCIprXm+vALEPw9LGF9eEczU0+9qeNnsi8Crlz234fWSqHV8UA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJdD2YwBxWAqhgtC41aF50812OmWiylRZEkT06LydtJ2ULJq8/T241cp3DMzN67OUw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDkvI1cWfocFvS0fRrn+u6NjYaooLW/voue/Y3cweVjnxhN/gaD5nRjGMmHsXzgVmQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECrHUlItrEi67j/Uh4l7Y+sTrTOL30RulfAEFt8w3GobdeM7i6m+12BapVBG197mHg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fb874eba-9c48-4340-a690-bebb2195d379"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH7iu6PoAJQmNEoWPRdtLkfXWt7kVQh1/CtyzK3VqFtqRukiMWiZN48vIxVd4R6dsw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncedTeamMemberships");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEurU6ZrLGLZfykokBpWyYEqEBP9Ga2xmzqYb8KMyWMRl2LHheX5tytt6vp3jQ33Iw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMMzfiXlWCImAILZzfdcujU5I1uWAJgHgicvKd4G80A2Fg6r0ZOW8q1UH5NyOLO4Sw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("251e0a52-d589-4845-ada8-daeb08be76ea"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAuHruPnRGlN3Xq20ky/PX5MNXAe5mrJJp8fioA34Sd4XtW85+jpgtAAKwzlTGKuGg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBmxJR6DJhSAJXMBEfA/TJZP1YHN+nr06fXrihnFYVoJrHYcniXntWWcaPfXt/LT5Q==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENUU2a07QfAav4csT7QQxJEG5qHO7BC1ocT8VCnrU0SmstwHmvDSHs1IVOl0FUMeSA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDB4a6Yo341p9w4mikJR7pU0xXf13Z5gpdtuemxdA+NTetdyl9w6dRF37jE9VE2jyw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGBfYpI3+jD07+rM2GxPKgXVtuWkZwoBn/tT/rO1Dv++Od4TmnaEU7A2RjoThhndhw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF2EjLloRAqhHZ8b1HEsgK2YIyl7XbbbqB2HV0MqHaUt7Om6Phww7bHt5wWfTbUzcQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPZvG5cBjgzAhS8jvYxpr8rtLbRZuu8FgUQuW1Pl4EVejZEv6vORf8WPAT92dhy1jw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fb874eba-9c48-4340-a690-bebb2195d379"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP/raVK8/qrnvjvsRu6cGjGOPZv1611RXWzhFYqkx9Si244Mh7KN5ggGMfltIkxSaQ==");
        }
    }
}
