using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "carol.green@company.com", "Carol Green", "CAROL.GREEN@COMPANY.COM", "AQAAAAIAAYagAAAAEEurU6ZrLGLZfykokBpWyYEqEBP9Ga2xmzqYb8KMyWMRl2LHheX5tytt6vp3jQ33Iw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "philip.davis@company.com", "Philip Davis", "PHILIP.DAVIS@COMPANY.COM", "AQAAAAIAAYagAAAAEMMzfiXlWCImAILZzfdcujU5I1uWAJgHgicvKd4G80A2Fg6r0ZOW8q1UH5NyOLO4Sw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("251e0a52-d589-4845-ada8-daeb08be76ea"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "anna.collins@company.com", "Anna Collins", "ANNA.COLLINS@COMPANY.COM", "AQAAAAIAAYagAAAAEAuHruPnRGlN3Xq20ky/PX5MNXAe5mrJJp8fioA34Sd4XtW85+jpgtAAKwzlTGKuGg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "louis.newton@company.com", "Louis Newton", "LOUIS.NEWTON@COMPANY.COM", "AQAAAAIAAYagAAAAEBmxJR6DJhSAJXMBEfA/TJZP1YHN+nr06fXrihnFYVoJrHYcniXntWWcaPfXt/LT5Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "mary.allen@company.com", "Mary Allen", "MARY.ALLEN@COMPANY.COM", "AQAAAAIAAYagAAAAENUU2a07QfAav4csT7QQxJEG5qHO7BC1ocT8VCnrU0SmstwHmvDSHs1IVOl0FUMeSA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "florence.reed@company.com", "Florence Reed", "FLORENCE.REED@COMPANY.COM", "AQAAAAIAAYagAAAAEDB4a6Yo341p9w4mikJR7pU0xXf13Z5gpdtuemxdA+NTetdyl9w6dRF37jE9VE2jyw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "alice.parker@company.com", "Alice Parker", "ALICE.PARKER@COMPANY.COM", "AQAAAAIAAYagAAAAEGBfYpI3+jD07+rM2GxPKgXVtuWkZwoBn/tT/rO1Dv++Od4TmnaEU7A2RjoThhndhw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "john.foster@company.com", "John Foster", "JOHN.FOSTER@COMPANY.COM", "AQAAAAIAAYagAAAAEF2EjLloRAqhHZ8b1HEsgK2YIyl7XbbbqB2HV0MqHaUt7Om6Phww7bHt5wWfTbUzcQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "sandra.quinn@company.com", "Sandra Quinn", "SANDRA.QUINN@COMPANY.COM", "AQAAAAIAAYagAAAAEPZvG5cBjgzAhS8jvYxpr8rtLbRZuu8FgUQuW1Pl4EVejZEv6vORf8WPAT92dhy1jw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fb874eba-9c48-4340-a690-bebb2195d379"),
                columns: new[] { "Email", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "admin@admin.com", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEP/raVK8/qrnvjvsRu6cGjGOPZv1611RXWzhFYqkx9Si244Mh7KN5ggGMfltIkxSaQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "cazevedo@pessoal.pt", "Carla Alexandra Gomes Azevedo", "CAZEVEDO@PESSOAL.PT", "AQAAAAIAAYagAAAAEA/Jq/pyjdaYZPHHJpHbqp+i+5+OsgUefuEAQkvooVZb58I6rxZyxF8urU87vWNi3w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "fdias@pessoal.pt", "Filipe Manuel Marques da Silva Dias", "FDIAS@PESSOAL.PT", "AQAAAAIAAYagAAAAEPndwjT6ExopmCYz9Aq9PrmtPPkq9lUQdddp76wbeRyhTVzEbGN+74hauKGZUWrVng==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("251e0a52-d589-4845-ada8-daeb08be76ea"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "acasaco@pessoal.pt", "Ana Cristina Leite Casaco", "ACASACO@PESSOAL.PT", "AQAAAAIAAYagAAAAEIDVzMG1GxuV1nz0VAKeh2s5+6SNWxHgrMQVsfltRGRgB30T5Svv8i96lxbNy6O5ew==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4b93b0ab-aed4-4eb1-a40b-900229359841"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "lneves@pessoal.pt", "Luís Augusto Pinto Teixeira Neves", "LNEVES@PESSOAL.PT", "AQAAAAIAAYagAAAAEG9Qpe1DBFgyk9AfdJGGl6tzKtSDzBGv47Q6lSlf4vzR06PNT7XDSBirnaOeXgpq8w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("804eef0f-2ec6-4b19-8b54-3373388f122b"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "malves@pessoal.pt", "Maria João Pereira Alves", "MALVES@PESSOAL.PT", "AQAAAAIAAYagAAAAEDOqoY/A9Id4uSg0FZ2Pn1IFcP59oG/uARhSavAgtFEgJ7ZcvoQkQVjjCvzc+BFQnQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("db21f4f6-0eb4-4905-a604-ae5ab05696a6"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "frebelo@pessoal.pt", "Filomena Maria Cruz Rebelo", "FREBELO@PESSOAL.PT", "AQAAAAIAAYagAAAAEBDhOkl+BmTH6/kLA+Lwjzg9lgE3khGaNJ13UhPR2EM5dQLs/k7mjif9+3KvB7i/wQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e3a5e7de-975c-4d0f-ad17-cd563e62789a"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "apinto@pessoal.pt", "Ana Alexandra Valença Pinto", "APINTO@PESSOAL.PT", "AQAAAAIAAYagAAAAENLLmW3rVmFlf8TaNCL5neMQ2dlb4Tqdmk8wmyech/l1LR0NrhCyYPNg5GVPixZE9Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "jfonseca@pessoal.pt", "João Carlos Araújo Fonseca", "JFONSECA@PESSOAL.PT", "AQAAAAIAAYagAAAAENmFh/knB5U+tb/bOSLPIHkBqqkgHUv0msS8BrGdstihIWLsJom+6pAK5ijyjJ2yFA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("faf84edb-dd0b-4de9-90f9-3671335d01c7"),
                columns: new[] { "Email", "Nome", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "sconceicao@pessoal.pt", "Sandra Regina Queirós Azevedo Conceição", "SCONCEICAO@PESSOAL.PT", "AQAAAAIAAYagAAAAEBunlXGl4cP+eVgXp5N8WTrsPdSoFKudcKMZg47f2Ql3eJVJvOXqaYWopgeERwKVMw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fb874eba-9c48-4340-a690-bebb2195d379"),
                columns: new[] { "Email", "NormalizedEmail", "PasswordHash" },
                values: new object[] { "admin@admin.pt", "ADMIN@ADMIN.PT", "AQAAAAIAAYagAAAAEKCqOA4Ilzwt3W3BSnRTmiTH4ahIz+YHJYBKCC8o/a/Mj4G0InEWw244yJamcd8tCA==" });
        }
    }
}
