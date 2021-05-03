using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instituicao",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Nome = table.Column<string>(type: "varchar(256)", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Diretor = table.Column<string>(type: "varchar(256)", nullable: false),
                    Endereco = table.Column<string>(type: "varchar(256)", nullable: true),
                    Telefone = table.Column<string>(type: "varchar(256)", nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", nullable: true),
                    Website = table.Column<string>(type: "varchar(256)", nullable: true),
                    Color = table.Column<string>(type: "varchar(50)", nullable: false),
                    Logo = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                });

            migrationBuilder.CreateTable(
                name: "Requisito",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    NomeRequisito = table.Column<string>(type: "varchar(50)", nullable: false),
                    ElementoHTMLRequisito = table.Column<string>(unicode: false, nullable: false),
                    TipoElementoHTMLRequisito = table.Column<string>(unicode: false, nullable: false),
                    Bookmark = table.Column<string>(type: "varchar(50)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(150)", nullable: false),
                    Grupo = table.Column<string>(unicode: false, nullable: false),
                    MascaraEntrada = table.Column<string>(type: "varchar(150)", nullable: true),
                    Obrigatorio = table.Column<bool>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                });

            migrationBuilder.CreateTable(
                name: "SysLogs",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    Mensagem = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    LocalOrigem = table.Column<string>(type: "varchar(150)", nullable: false, defaultValueSql: "'Error'"),
                    Acao_IdAcao = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                });

            migrationBuilder.CreateTable(
                name: "TermoCompromisso",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    Titulo = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Termos = table.Column<string>(type: "text", nullable: false),
                    Versao = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                });

            migrationBuilder.CreateTable(
                name: "TourStep",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Posicao = table.Column<int>(type: "int(11)", nullable: false),
                    IdElemento = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Titulo = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Conteudo = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Area = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Controlador = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    View = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                name: "Curso",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    NomeCurso = table.Column<string>(type: "varchar(150)", nullable: false),
                    Sigla = table.Column<string>(type: "varchar(13)", maxLength: 10, nullable: false),
                    CoordenadorCurso = table.Column<string>(type: "varchar(150)", nullable: false),
                    EmailCoordenadorCurso = table.Column<string>(type: "varchar(256)", nullable: false),
                    DescricaoCurso = table.Column<string>(type: "tinytext", nullable: false),
                    TipoCurso = table.Column<int>(nullable: false),
                    IdInstituicao = table.Column<int>(type: "int(11)", nullable: false),
                    Alerta = table.Column<string>(type: "tinytext", nullable: true),
                    Informacao = table.Column<string>(type: "tinytext", nullable: true),
                    CargaHorariaMinimaEstagio = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "UKIdInstituicao_Curso",
                        column: x => x.IdInstituicao,
                        principalTable: "Instituicao",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpcaoRequisito",
                columns: table => new
                {
                    IdRequisito = table.Column<int>(type: "int(11)", nullable: false),
                    Valor = table.Column<string>(type: "varchar(50)", nullable: false),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Identificador = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.IdRequisito, x.Valor });
                    table.UniqueConstraint("AK_OpcaoRequisito_Identificador_IdRequisito_Valor", x => new { x.Identificador, x.IdRequisito, x.Valor });
                    table.ForeignKey(
                        name: "FK_OpcaoRequisito_Requisito_IdRequisito",
                        column: x => x.IdRequisito,
                        principalTable: "Requisito",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IdCurso = table.Column<int>(nullable: false),
                    IdInstituicao = table.Column<int>(nullable: false),
                    Logado = table.Column<bool>(nullable: false),
                    Estagiando = table.Column<bool>(nullable: false),
                    AceitouTermos = table.Column<bool>(nullable: false),
                    DataHoraInclusao = table.Column<DateTime>(nullable: false),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    TipoRecebimentoNotificacao = table.Column<int>(nullable: false),
                    IdCursoNavigationIdentificador = table.Column<int>(nullable: true),
                    IdInstituicaoNavigationIdentificador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Curso_IdCursoNavigationIdentificador",
                        column: x => x.IdCursoNavigationIdentificador,
                        principalTable: "Curso",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Instituicao_IdInstituicaoNavigationIdentificador",
                        column: x => x.IdInstituicaoNavigationIdentificador,
                        principalTable: "Instituicao",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtividadeEstagio",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Titulo = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    IdCurso = table.Column<int>(type: "int(11)", nullable: false),
                    TipoAtividade = table.Column<int>(nullable: false),
                    CondicaoVezes = table.Column<int>(nullable: false),
                    EnumEntidade = table.Column<string>(unicode: false, nullable: false),
                    IdentificadorEntidadeAtividade = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FKAtividadeEstagioIdCurso",
                        column: x => x.IdCurso,
                        principalTable: "Curso",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdCurso = table.Column<int>(type: "int(11)", nullable: false),
                    TituloDocumento = table.Column<string>(type: "varchar(150)", nullable: false),
                    DescricaoDocumento = table.Column<string>(type: "tinytext", nullable: true),
                    PosicaoDocumento = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    Assinatura = table.Column<string>(unicode: false, nullable: false),
                    Aviso = table.Column<string>(type: "varchar(500)", nullable: true),
                    Tipo = table.Column<string>(unicode: false, nullable: false),
                    Texto = table.Column<string>(type: "tinytext", nullable: true),
                    Etapa = table.Column<string>(unicode: false, nullable: false),
                    Visibilidade = table.Column<string>(unicode: false, nullable: false),
                    PossuiAssinaturaResposavelEstagio = table.Column<bool>(type: "bit(1)", nullable: false),
                    PossuiCarimboCNPJ = table.Column<bool>(type: "bit(1)", nullable: false),
                    PossuiData = table.Column<bool>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "UKIdCurso_Documento",
                        column: x => x.IdCurso,
                        principalTable: "Curso",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InformacaoCurso",
                columns: table => new
                {
                    Identificador = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    TipoInformacao = table.Column<string>(unicode: false, nullable: false),
                    IdCurso = table.Column<int>(type: "int(11)", nullable: false),
                    ConteudoInformacao = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_InformacaoCurso_Curso_IdCurso",
                        column: x => x.IdCurso,
                        principalTable: "Curso",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlteracaoEntidadesSistema",
                columns: table => new
                {
                    Identificador = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(nullable: false),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    AutorId = table.Column<string>(nullable: true),
                    IdAutor = table.Column<string>(nullable: true),
                    Entidade = table.Column<int>(nullable: false),
                    IdentificadorEntidade = table.Column<int>(nullable: false),
                    MensagemAlteracao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlteracaoEntidadesSistema", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_AlteracaoEntidadesSistema_AspNetUsers_AutorId",
                        column: x => x.AutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "AtividadeUsuario",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdUsuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    Titulo = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    Data = table.Column<DateTime>(nullable: false),
                    Visibilidade = table.Column<string>(unicode: false, nullable: false),
                    IdCurso = table.Column<int>(nullable: false),
                    TipoAtividade = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FKAtividadeUsuarioIdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FAQ",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdInstituicao = table.Column<int>(type: "int(11)", nullable: false),
                    Pergunta = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Resposta = table.Column<string>(type: "varchar(500)", nullable: true, defaultValueSql: "'error'"),
                    IdUsuarioPergunta = table.Column<string>(type: "varchar(50)", nullable: false),
                    IdUsuarioResposta = table.Column<string>(type: "varchar(50)", nullable: true),
                    Pontuacao = table.Column<int>(type: "int(11)", nullable: false),
                    Status = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_FAQ_Instituicao_IdInstituicao",
                        column: x => x.IdInstituicao,
                        principalTable: "Instituicao",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FAQ_AspNetUsers_IdUsuarioResposta",
                        column: x => x.IdUsuarioResposta,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogAcoesEspeciais",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    Mensagem = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    IdUsuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    LocalOrigem = table.Column<string>(type: "varchar(150)", nullable: false, defaultValueSql: "'Error'"),
                    AcoesSistema = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "HistoricoEspecialSistema_ibfk_1",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    IdUsuario = table.Column<string>(type: "varchar(255)", nullable: true),
                    DataHoraLogin = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataHoraLogout = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "UKIdUsuario_Logins",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MensagemIndividual",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdUsuarioRemetente = table.Column<string>(type: "varchar(50)", nullable: false),
                    IdUsuarioDestino = table.Column<string>(type: "varchar(50)", nullable: true),
                    Conteudo = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Status = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_MensagemIndividual_AspNetUsers_IdUsuarioRemetente",
                        column: x => x.IdUsuarioRemetente,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificacaoIndividual",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdUsuarioRemetente = table.Column<string>(type: "varchar(50)", nullable: false),
                    IdUsuarioDestino = table.Column<string>(type: "varchar(50)", nullable: true),
                    Cabecalho = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Conteudo = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "'error'"),
                    Status = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_NotificacaoIndividual_AspNetUsers_IdUsuarioDestino",
                        column: x => x.IdUsuarioDestino,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroDeHoras",
                columns: table => new
                {
                    Identificador = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Data = table.Column<DateTime>(nullable: false),
                    HoraInicio = table.Column<DateTime>(nullable: false),
                    HoraFim = table.Column<DateTime>(nullable: false),
                    Atividade = table.Column<string>(nullable: false),
                    CargaHoraria = table.Column<float>(nullable: false),
                    IdUsuario = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_RegistroDeHoras_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequisitoDeUsuario",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    IdRequisito = table.Column<int>(type: "int(11)", nullable: false),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    valor = table.Column<string>(type: "varchar(256)", nullable: true),
                    Identificador = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.UserId, x.IdRequisito });
                    table.UniqueConstraint("AK_RequisitoDeUsuario_Identificador_IdRequisito_UserId", x => new { x.Identificador, x.IdRequisito, x.UserId });
                    table.ForeignKey(
                        name: "FKRequisitoDeUsuarioIdRequisito",
                        column: x => x.IdRequisito,
                        principalTable: "Requisito",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitoDeUsuario_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConclusaoAtividadeCurso",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdUsuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    IdAtividade = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FKConclusaoAtividadeCursoIdAtividade",
                        column: x => x.IdAtividade,
                        principalTable: "AtividadeEstagio",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKConclusaoAtividadeCursoIdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoGeracaoDocumento",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    Descricao = table.Column<string>(type: "varchar(500)", nullable: false),
                    IdDocumento = table.Column<int>(type: "int(11)", nullable: false),
                    IdUsuario = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "HistoricoSistema_ibfk_2",
                        column: x => x.IdDocumento,
                        principalTable: "Documento",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "HistoricoSistema_ibfk_1",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InformacaoDocumento",
                columns: table => new
                {
                    Identificador = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    TipoInformacao = table.Column<string>(unicode: false, nullable: false),
                    IdDocumento = table.Column<int>(type: "int(11)", nullable: false),
                    ConteudoInformacao = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_InformacaoDocumento_Documento_IdDocumento",
                        column: x => x.IdDocumento,
                        principalTable: "Documento",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequisitoDeDocumento",
                columns: table => new
                {
                    IdDocumento = table.Column<int>(type: "int(11)", nullable: false),
                    IdRequisito = table.Column<int>(type: "int(11)", nullable: false),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    OrdemRequisito = table.Column<sbyte>(type: "tinyint(4)", nullable: false, defaultValueSql: "'1'"),
                    Identificador = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.IdDocumento, x.IdRequisito });
                    table.UniqueConstraint("AK_RequisitoDeDocumento_IdDocumento_Identificador_IdRequisito", x => new { x.IdDocumento, x.Identificador, x.IdRequisito });
                    table.ForeignKey(
                        name: "IdDocumento",
                        column: x => x.IdDocumento,
                        principalTable: "Documento",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "IdRequisito",
                        column: x => x.IdRequisito,
                        principalTable: "Requisito",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisualizacaoNotificacaoGeral",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(nullable: false),
                    DataHoraExclusao = table.Column<DateTime>(nullable: true),
                    IdUsuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    IdNotificacao = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "UKIdNotificacao_VisualizacaoNotificacao",
                        column: x => x.IdNotificacao,
                        principalTable: "AlteracaoEntidadesSistema",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "VisualizacaoNotificacao_ibfk_1",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlteracaoEntidadesSistema_AutorId",
                table: "AlteracaoEntidadesSistema",
                column: "AutorId");

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
                name: "IX_AspNetUsers_IdCursoNavigationIdentificador",
                table: "AspNetUsers",
                column: "IdCursoNavigationIdentificador");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdInstituicaoNavigationIdentificador",
                table: "AspNetUsers",
                column: "IdInstituicaoNavigationIdentificador");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeEstagio_IdCurso",
                table: "AtividadeEstagio",
                column: "IdCurso");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeUsuario_IdUsuario",
                table: "AtividadeUsuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ConclusaoAtividadeCurso_IdAtividade",
                table: "ConclusaoAtividadeCurso",
                column: "IdAtividade");

            migrationBuilder.CreateIndex(
                name: "IX_ConclusaoAtividadeCurso_IdUsuario",
                table: "ConclusaoAtividadeCurso",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Curso_IdInstituicao",
                table: "Curso",
                column: "IdInstituicao");

            migrationBuilder.CreateIndex(
                name: "UKIdCurso",
                table: "Documento",
                column: "IdCurso");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_IdInstituicao",
                table: "FAQ",
                column: "IdInstituicao");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_IdUsuarioResposta",
                table: "FAQ",
                column: "IdUsuarioResposta");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoGeracaoDocumento_IdDocumento",
                table: "HistoricoGeracaoDocumento",
                column: "IdDocumento");

            migrationBuilder.CreateIndex(
                name: "IdUsuario",
                table: "HistoricoGeracaoDocumento",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_InformacaoCurso_IdCurso",
                table: "InformacaoCurso",
                column: "IdCurso");

            migrationBuilder.CreateIndex(
                name: "IX_InformacaoDocumento_IdDocumento",
                table: "InformacaoDocumento",
                column: "IdDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_LogAcoesEspeciais_IdUsuario",
                table: "LogAcoesEspeciais",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "UKIdUsuario",
                table: "Logins",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_MensagemIndividual_IdUsuarioRemetente",
                table: "MensagemIndividual",
                column: "IdUsuarioRemetente");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacaoIndividual_IdUsuarioDestino",
                table: "NotificacaoIndividual",
                column: "IdUsuarioDestino");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroDeHoras_IdUsuario",
                table: "RegistroDeHoras",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IdDocumento",
                table: "RequisitoDeDocumento",
                column: "IdDocumento");

            migrationBuilder.CreateIndex(
                name: "IdRequisito",
                table: "RequisitoDeDocumento",
                column: "IdRequisito");

            migrationBuilder.CreateIndex(
                name: "FKRequisitoDeUsuarioIdRequisito",
                table: "RequisitoDeUsuario",
                column: "IdRequisito");

            migrationBuilder.CreateIndex(
                name: "IX_VisualizacaoNotificacaoGeral_IdNotificacao",
                table: "VisualizacaoNotificacaoGeral",
                column: "IdNotificacao");

            migrationBuilder.CreateIndex(
                name: "IX_VisualizacaoNotificacaoGeral_IdUsuario",
                table: "VisualizacaoNotificacaoGeral",
                column: "IdUsuario");
        }

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
                name: "AtividadeUsuario");

            migrationBuilder.DropTable(
                name: "ConclusaoAtividadeCurso");

            migrationBuilder.DropTable(
                name: "FAQ");

            migrationBuilder.DropTable(
                name: "HistoricoGeracaoDocumento");

            migrationBuilder.DropTable(
                name: "InformacaoCurso");

            migrationBuilder.DropTable(
                name: "InformacaoDocumento");

            migrationBuilder.DropTable(
                name: "LogAcoesEspeciais");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "MensagemIndividual");

            migrationBuilder.DropTable(
                name: "NotificacaoIndividual");

            migrationBuilder.DropTable(
                name: "OpcaoRequisito");

            migrationBuilder.DropTable(
                name: "RegistroDeHoras");

            migrationBuilder.DropTable(
                name: "RequisitoDeDocumento");

            migrationBuilder.DropTable(
                name: "RequisitoDeUsuario");

            migrationBuilder.DropTable(
                name: "SysLogs");

            migrationBuilder.DropTable(
                name: "TermoCompromisso");

            migrationBuilder.DropTable(
                name: "TourStep");

            migrationBuilder.DropTable(
                name: "VisualizacaoNotificacaoGeral");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AtividadeEstagio");

            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.DropTable(
                name: "Requisito");

            migrationBuilder.DropTable(
                name: "AlteracaoEntidadesSistema");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Curso");

            migrationBuilder.DropTable(
                name: "Instituicao");
        }
    }
}
