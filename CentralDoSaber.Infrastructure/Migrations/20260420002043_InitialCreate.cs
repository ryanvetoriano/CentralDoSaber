using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralDoSaber.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_AUTORES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Biografia = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataNascimento = table.Column<string>(type: "NVARCHAR2(10)", nullable: true),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AUTORES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_EDITORAS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Pais = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EDITORAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_GENEROS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GENEROS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    DataNascimento = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONTEUDOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    EditoraId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    AutorId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Titulo = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DataLancamento = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NumeroPaginas = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NumeroCapitulos = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONTEUDOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_CONTEUDOS_TB_AUTORES_AutorId",
                        column: x => x.AutorId,
                        principalTable: "TB_AUTORES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_CONTEUDOS_TB_EDITORAS_EditoraId",
                        column: x => x.EditoraId,
                        principalTable: "TB_EDITORAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_USER_CONFIGURATIONS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Tema = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NotificacoesAtivas = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER_CONFIGURATIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_USER_CONFIGURATIONS_TB_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_AVALIACOES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ConteudoId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nota = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AVALIACOES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_AVALIACOES_TB_CONTEUDOS_ConteudoId",
                        column: x => x.ConteudoId,
                        principalTable: "TB_CONTEUDOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_AVALIACOES_TB_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_COMENTARIOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Texto = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ConteudoId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Disponivel = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COMENTARIOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_COMENTARIOS_TB_CONTEUDOS_ConteudoId",
                        column: x => x.ConteudoId,
                        principalTable: "TB_CONTEUDOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_COMENTARIOS_TB_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONTEUDO_GENEROS",
                columns: table => new
                {
                    ConteudoId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    GeneroId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONTEUDO_GENEROS", x => new { x.ConteudoId, x.GeneroId });
                    table.ForeignKey(
                        name: "FK_TB_CONTEUDO_GENEROS_TB_CONTEUDOS_ConteudoId",
                        column: x => x.ConteudoId,
                        principalTable: "TB_CONTEUDOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_CONTEUDO_GENEROS_TB_GENEROS_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "TB_GENEROS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_AVALIACOES_ConteudoId",
                table: "TB_AVALIACOES",
                column: "ConteudoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AVALIACOES_UserId_ConteudoId",
                table: "TB_AVALIACOES",
                columns: new[] { "UserId", "ConteudoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMENTARIOS_ConteudoId",
                table: "TB_COMENTARIOS",
                column: "ConteudoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMENTARIOS_UserId",
                table: "TB_COMENTARIOS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTEUDO_GENEROS_GeneroId",
                table: "TB_CONTEUDO_GENEROS",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTEUDOS_AutorId",
                table: "TB_CONTEUDOS",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTEUDOS_EditoraId",
                table: "TB_CONTEUDOS",
                column: "EditoraId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USER_CONFIGURATIONS_UserId",
                table: "TB_USER_CONFIGURATIONS",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USERS_Email",
                table: "TB_USERS",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AVALIACOES");

            migrationBuilder.DropTable(
                name: "TB_COMENTARIOS");

            migrationBuilder.DropTable(
                name: "TB_CONTEUDO_GENEROS");

            migrationBuilder.DropTable(
                name: "TB_USER_CONFIGURATIONS");

            migrationBuilder.DropTable(
                name: "TB_CONTEUDOS");

            migrationBuilder.DropTable(
                name: "TB_GENEROS");

            migrationBuilder.DropTable(
                name: "TB_USERS");

            migrationBuilder.DropTable(
                name: "TB_AUTORES");

            migrationBuilder.DropTable(
                name: "TB_EDITORAS");
        }
    }
}
