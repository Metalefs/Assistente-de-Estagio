using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assistente_de_Estagio.Models
{
    public partial class u911430744_estgContext : DbContext
    {
        public u911430744_estgContext()
        {
        }

        public u911430744_estgContext(DbContextOptions<u911430744_estgContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Requisitos> Requisitos { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        // Unable to generate entity type for table 'u911430744_estg.requisitodedocumento'. Please see the warning messages.
        // Unable to generate entity type for table 'u911430744_estg.requisitodeusuario'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=i4e7l4@1245;database=u911430744_estg");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasKey(e => e.IdCurso);

                entity.ToTable("curso", "u911430744_estg");

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

                entity.ToTable("documento", "u911430744_estg");

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

            modelBuilder.Entity<Requisitos>(entity =>
            {
                entity.HasKey(e => e.IdRequisito);

                entity.ToTable("requisitos", "u911430744_estg");

                entity.Property(e => e.IdRequisito)
                    .HasColumnName("idRequisito")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dados)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NomeRequisito)
                    .HasMaxLength(150)
                    .IsUnicode(false);

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

                entity.ToTable("usuario", "u911430744_estg");

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
