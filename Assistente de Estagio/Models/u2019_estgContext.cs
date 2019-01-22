using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assistente_de_Estagio.Models
{
    public partial class u2019_estgContext : DbContext
    {
        public u2019_estgContext()
        {
        }

        public u2019_estgContext(DbContextOptions<u2019_estgContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Requisito> Requisito { get; set; }
        public virtual DbSet<Requisitodedocumento> Requisitodedocumento { get; set; }
        public virtual DbSet<Requisitodeusuario> Requisitodeusuario { get; set; }
        public virtual DbSet<Requisitos> Requisitos { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;user=root;password=i4e7l4@1245;database=u2019_estg");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasKey(e => e.IdCurso);

                entity.ToTable("curso", "u2019_estg");

                entity.HasIndex(e => e.DocumentoIdDocumento)
                    .HasName("fk_Curso_Documento_idx");

                entity.Property(e => e.IdCurso)
                    .HasColumnName("idCurso")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CoordenadorCurso)
                    .HasColumnName("coordenadorCurso")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DescricaoCurso)
                    .HasColumnName("descricaoCurso")
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoIdDocumento)
                    .HasColumnName("Documento_idDocumento")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FaculdadeCurso)
                    .HasColumnName("faculdadeCurso")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("FacisaBH");

                entity.Property(e => e.NomeCurso)
                    .HasColumnName("nomeCurso")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("Curso Padrão");
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.HasKey(e => new { e.IdDocumento, e.DescricaoDocumento });

                entity.ToTable("documento", "u2019_estg");

                entity.Property(e => e.IdDocumento)
                    .HasColumnName("idDocumento")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DescricaoDocumento)
                    .HasColumnName("descricaoDocumento")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.AutorDocumento)
                    .IsRequired()
                    .HasColumnName("autorDocumento")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("FacisaBH");

                entity.Property(e => e.CaminhoDocumento)
                    .IsRequired()
                    .HasColumnName("caminhoDocumento")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CursoIdCurso)
                    .HasColumnName("Curso_idCurso")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PosicaoDocumento)
                    .HasColumnName("posicaoDocumento")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PreenchimentoDocumento)
                    .IsRequired()
                    .HasColumnName("preenchimentoDocumento")
                    .IsUnicode(false);

                entity.Property(e => e.RequisitoDocumento)
                    .HasColumnName("requisitoDocumento")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TiposRequisitos)
                    .HasColumnName("tiposRequisitos")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TituloDocumento)
                    .IsRequired()
                    .HasColumnName("tituloDocumento")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Requisito>(entity =>
            {
                entity.HasKey(e => e.IdRequisito);

                entity.ToTable("requisito", "u2019_estg");

                entity.HasIndex(e => e.UsuarioIdUsuario)
                    .HasName("Usuario_idUsuario");

                entity.Property(e => e.IdRequisito)
                    .HasColumnName("idRequisito")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Celular)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .HasColumnName("CPF")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.CursoMatriculado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dddcel)
                    .HasColumnName("DDDCel")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Dddtel)
                    .HasColumnName("DDDTel")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EscolhaArea)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NomeAluno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NomeEmpresa)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NomeSupervisor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Periodo)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Ra)
                    .HasColumnName("RA")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Rg)
                    .HasColumnName("RG")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Turma)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Turno)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioIdUsuario)
                    .HasColumnName("Usuario_idUsuario")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Requisitodedocumento>(entity =>
            {
                entity.HasKey(e => e.IdRequisitoDeDocumento);

                entity.ToTable("requisitodedocumento", "u2019_estg");

                entity.Property(e => e.IdRequisitoDeDocumento).HasColumnType("int(11)");

                entity.Property(e => e.DocumentoIdDocumento)
                    .HasColumnName("documento_idDocumento")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OrdemRequisito)
                    .HasColumnName("ordemRequisito")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.RequisitosIdRequisito)
                    .HasColumnName("requisitos_idRequisito")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Requisitodeusuario>(entity =>
            {
                entity.HasKey(e => e.IdRequisistoDeUsuario);

                entity.ToTable("requisitodeusuario", "u2019_estg");

                entity.Property(e => e.IdRequisistoDeUsuario).HasColumnType("int(11)");

                entity.Property(e => e.Dados).IsUnicode(false);

                entity.Property(e => e.RequisitosIdRequisito)
                    .HasColumnName("requisitos_idRequisito")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UsuarioIdUsuario)
                    .HasColumnName("usuario_idUsuario")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Requisitos>(entity =>
            {
                entity.HasKey(e => e.IdRequisito);

                entity.ToTable("requisitos", "u2019_estg");

                entity.Property(e => e.IdRequisito)
                    .HasColumnName("idRequisito")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClassRequisito)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("input-group");

                entity.Property(e => e.Dados)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IdCampo)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NomeRequisito)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("EMPTY");

                entity.Property(e => e.Opcoes).IsUnicode(false);

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasColumnType("enum('input','select','textarea')");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("usuario", "u2019_estg");

                entity.HasIndex(e => e.DocumentoIdDocumento)
                    .HasName("fk_Usuario_Documento1_idx");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("idUsuario")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.DataCriacaoUltimoDocumento)
                    .HasColumnName("dataCriacaoUltimoDocumento")
                    .HasColumnType("date");

                entity.Property(e => e.DocumentoIdDocumento)
                    .HasColumnName("Documento_idDocumento")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmailUsuario)
                    .HasColumnName("emailUsuario")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.NomeUsuario)
                    .HasColumnName("nomeUsuario")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.SenhaUsuario)
                    .HasColumnName("senhaUsuario")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });
        }
    }
}
