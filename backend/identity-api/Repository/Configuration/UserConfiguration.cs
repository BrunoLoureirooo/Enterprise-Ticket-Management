using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Entities.Models;

namespace backend.Repository.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    internal static readonly Guid ANA_PINTO = Guid.Parse("e3a5e7de-975c-4d0f-ad17-cd563e62789a");
    internal static readonly Guid ANA_CASACO = Guid.Parse("251e0a52-d589-4845-ada8-daeb08be76ea");
    internal static readonly Guid CARLA_AZEVEDO = Guid.Parse("024e414c-d9d9-477a-bcb5-c6777bc7d3f9");
    internal static readonly Guid FILIPE_DIAS = Guid.Parse("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae");
    internal static readonly Guid FILOMENA_REBELO = Guid.Parse("db21f4f6-0eb4-4905-a604-ae5ab05696a6");
    internal static readonly Guid JOAO_FONSECA = Guid.Parse("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216");
    internal static readonly Guid MARIA_ALVES = Guid.Parse("804eef0f-2ec6-4b19-8b54-3373388f122b");
    internal static readonly Guid SANDRA_CONCEICAO = Guid.Parse("faf84edb-dd0b-4de9-90f9-3671335d01c7");
    internal static readonly Guid LUIS_NEVES = Guid.Parse("4b93b0ab-aed4-4eb1-a40b-900229359841");
    internal static readonly Guid OLGA_CUNHA = Guid.Parse("620d12b3-1d00-4d7f-a262-795f004f83fe");
    internal static readonly Guid ADMIN = Guid.Parse("fb874eba-9c48-4340-a690-bebb2195d379");

    internal static readonly string DEV_PASSWORD = "Trigenius2025#";

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

        ApplicationUser anaPinto = new ApplicationUser()
        {
            Id = ANA_PINTO,

            Email = "apinto@pessoal.pt",
            NormalizedEmail = "APINTO@PESSOAL.PT",

            Nome = "Ana Alexandra Valença Pinto",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAENLLmW3rVmFlf8TaNCL5neMQ2dlb4Tqdmk8wmyech/l1LR0NrhCyYPNg5GVPixZE9Q==",

            ConcurrencyStamp = Guid.Parse("e0981140-1b7c-4f33-a556-8ce519876d66").ToString(),
            SecurityStamp = Guid.Parse("82a360c3-37f9-42da-9dd2-e8af93b864fc").ToString()
            
        };
        //anaPinto.PasswordHash = passwordHasher.HashPassword(anaPinto, DEV_PASSWORD);
        builder.HasData(anaPinto);

        ApplicationUser anaCasaso = new ApplicationUser()
        {
            Id = ANA_CASACO,

            Email = "acasaco@pessoal.pt",
            NormalizedEmail = "ACASACO@PESSOAL.PT",

            Nome = "Ana Cristina Leite Casaco",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAEIDVzMG1GxuV1nz0VAKeh2s5+6SNWxHgrMQVsfltRGRgB30T5Svv8i96lxbNy6O5ew==",

            ConcurrencyStamp = Guid.Parse("1635a002-94d3-44ef-86bb-57748d6b5654").ToString(),
            SecurityStamp = Guid.Parse("9dce8fcf-555e-475b-a676-0336b4a2886e").ToString()
        };
        //anaCasaso.PasswordHash = passwordHasher.HashPassword(anaCasaso, DEV_PASSWORD);
        builder.HasData(anaCasaso);

        ApplicationUser carlaAzevedo = new ApplicationUser()
        {
            Id = CARLA_AZEVEDO,

            Email = "cazevedo@pessoal.pt",
            NormalizedEmail = "CAZEVEDO@PESSOAL.PT",

            Nome = "Carla Alexandra Gomes Azevedo",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAEA/Jq/pyjdaYZPHHJpHbqp+i+5+OsgUefuEAQkvooVZb58I6rxZyxF8urU87vWNi3w==",

            ConcurrencyStamp = Guid.Parse("5701278b-d7ec-40ba-9cd6-708c44563c16").ToString(),
            SecurityStamp = Guid.Parse("75da7095-038e-4aca-970a-23122ef0e9c7").ToString()
        };
        //carlaAzevedo.PasswordHash = passwordHasher.HashPassword(carlaAzevedo, DEV_PASSWORD);
        builder.HasData(carlaAzevedo);

        ApplicationUser filipeDias = new ApplicationUser()
        {
            Id = FILIPE_DIAS,

            Email = "fdias@pessoal.pt",
            NormalizedEmail = "FDIAS@PESSOAL.PT",

            Nome = "Filipe Manuel Marques da Silva Dias",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAEPndwjT6ExopmCYz9Aq9PrmtPPkq9lUQdddp76wbeRyhTVzEbGN+74hauKGZUWrVng==",

            ConcurrencyStamp = Guid.Parse("e5607436-cab6-4d49-bf0a-2a3f595fd1f7").ToString(),
            SecurityStamp = Guid.Parse("bf601b92-1997-477c-a5b3-8958adc58474").ToString()
        };
        //filipeDias.PasswordHash = passwordHasher.HashPassword(filipeDias, DEV_PASSWORD);
        builder.HasData(filipeDias);

        ApplicationUser filomenaRebelo = new ApplicationUser()
        {
            Id = FILOMENA_REBELO,

            Email = "frebelo@pessoal.pt",
            NormalizedEmail = "FREBELO@PESSOAL.PT",

            Nome = "Filomena Maria Cruz Rebelo",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAEBDhOkl+BmTH6/kLA+Lwjzg9lgE3khGaNJ13UhPR2EM5dQLs/k7mjif9+3KvB7i/wQ==",

            ConcurrencyStamp = Guid.Parse("85ce73c8-afd1-4679-bd8b-8cf37c891da0").ToString(),
            SecurityStamp = Guid.Parse("6db4e253-246d-4ed7-b833-e0d896805c07").ToString()
        };
        //filomenaRebelo.PasswordHash = passwordHasher.HashPassword(filomenaRebelo, DEV_PASSWORD);
        builder.HasData(filomenaRebelo);

        ApplicationUser joaoFonseca = new ApplicationUser()
        {
            Id = JOAO_FONSECA,

            Email = "jfonseca@pessoal.pt",
            NormalizedEmail = "JFONSECA@PESSOAL.PT",

            Nome = "João Carlos Araújo Fonseca",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAENmFh/knB5U+tb/bOSLPIHkBqqkgHUv0msS8BrGdstihIWLsJom+6pAK5ijyjJ2yFA==",

            ConcurrencyStamp = Guid.Parse("5e9c546c-5c8b-446b-834d-89d3fb270ac9").ToString(),
            SecurityStamp = Guid.Parse("c4225b9c-3e5c-4526-a267-b55fc1dbdde8").ToString()
        };
        //joaoFonseca.PasswordHash = passwordHasher.HashPassword(joaoFonseca, DEV_PASSWORD);
        builder.HasData(joaoFonseca);

        ApplicationUser mariaAlves = new ApplicationUser()
        {
            Id = MARIA_ALVES,

            Email = "malves@pessoal.pt",
            NormalizedEmail = "MALVES@PESSOAL.PT",

            Nome = "Maria João Pereira Alves",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAEDOqoY/A9Id4uSg0FZ2Pn1IFcP59oG/uARhSavAgtFEgJ7ZcvoQkQVjjCvzc+BFQnQ==",

            ConcurrencyStamp = Guid.Parse("57635711-b231-4e09-866c-cb8970cab382").ToString(),
            SecurityStamp = Guid.Parse("214c00b2-682f-4310-84ba-8cd7f466f20d").ToString()
        };
        //mariaAlves.PasswordHash = passwordHasher.HashPassword(mariaAlves, DEV_PASSWORD);
        builder.HasData(mariaAlves);

        ApplicationUser sandraConceicao = new ApplicationUser()
        {
            Id = SANDRA_CONCEICAO,

            Email = "sconceicao@pessoal.pt",
            NormalizedEmail = "SCONCEICAO@PESSOAL.PT",

            Nome = "Sandra Regina Queirós Azevedo Conceição",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAEBunlXGl4cP+eVgXp5N8WTrsPdSoFKudcKMZg47f2Ql3eJVJvOXqaYWopgeERwKVMw==",

            ConcurrencyStamp = Guid.Parse("e4e88c59-a13a-44b2-bedf-11fcb65c5d2b").ToString(),
            SecurityStamp = Guid.Parse("1d4e98cc-c78e-4abe-8164-aa41358c89b0").ToString()
        };
        //sandraConceicao.PasswordHash = passwordHasher.HashPassword(sandraConceicao, DEV_PASSWORD);
        builder.HasData(sandraConceicao);

        ApplicationUser luisNeves = new ApplicationUser()
        {
            Id = LUIS_NEVES,

            Email = "lneves@pessoal.pt",
            NormalizedEmail = "LNEVES@PESSOAL.PT",

            Nome = "Luís Augusto Pinto Teixeira Neves",

            EmailConfirmed = true,
            
            PasswordHash = "AQAAAAIAAYagAAAAEG9Qpe1DBFgyk9AfdJGGl6tzKtSDzBGv47Q6lSlf4vzR06PNT7XDSBirnaOeXgpq8w==",

            ConcurrencyStamp = Guid.Parse("c02f62ed-c493-4c6b-9925-7ed27623346f").ToString(),
            SecurityStamp = Guid.Parse("fb09937d-40b0-4999-ae4b-8c551449e296").ToString()
        };

        //luisNeves.PasswordHash = passwordHasher.HashPassword(luisNeves, DEV_PASSWORD);
        builder.HasData(luisNeves);

        ApplicationUser admin = new ApplicationUser()
        {
            Id = ADMIN,

            Email = "admin@admin.pt",
            NormalizedEmail = "ADMIN@ADMIN.PT",

            Nome = "Admin",

            EmailConfirmed = true,

            PasswordHash = "AQAAAAIAAYagAAAAEKCqOA4Ilzwt3W3BSnRTmiTH4ahIz+YHJYBKCC8o/a/Mj4G0InEWw244yJamcd8tCA==",

            ConcurrencyStamp = Guid.Parse("be7e12e8-b4d4-4136-8be0-a022c5ae1b77").ToString(),
            SecurityStamp = Guid.Parse("8f15dade-0a81-4238-ae5c-cad2ca8d0aae").ToString()
        };
        //admin.PasswordHash = passwordHasher.HashPassword(admin, DEV_PASSWORD);
        builder.HasData(admin);
    }
}