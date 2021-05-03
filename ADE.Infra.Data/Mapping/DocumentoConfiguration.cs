using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ADE.Infra.Data.Mapping
{
    public partial class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
    {
        public DocumentoConfiguration()
        {

        }

        public void Configure(EntityTypeBuilder<Documento> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Identificador)
                    .HasName("PRIMARY");

            modelBuilder.HasIndex(e => e.IdCurso)
                    .HasDatabaseName("UKIdCurso");

            modelBuilder.HasOne(e => e.IdCursoNavigation)
               .WithMany(p => p.Documento)
               .HasForeignKey(d => d.IdCurso)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("UKIdCurso_Documento");

            modelBuilder.Property(e => e.Identificador).HasColumnType("int(11)");

            modelBuilder.Property(e => e.Arquivo);
            modelBuilder.Property(e => e.ArquivoPDF);

            modelBuilder.Property(e => e.DescricaoDocumento).HasColumnType("text");

            modelBuilder.Property(e => e.IdCurso).HasColumnType("int(11)");

            modelBuilder.Property(e => e.PosicaoDocumento).HasColumnType("tinyint(4)").HasDefaultValueSql("'1'");

            modelBuilder.Property(e => e.Texto).HasColumnType("text");

            modelBuilder.Property(e => e.TituloDocumento).IsRequired().HasColumnType("varchar(150)");

            //modelBuilder.Property(e => e.Assinatura).IsRequired().HasColumnType("enum('NDE', 'Empresa', 'Aluno')");

            modelBuilder.Property(e => e.Assinatura)
               .IsRequired()
               .HasConversion(
                   v => v.ToString(),
                   v => (EnumAssinaturaDocumento)Enum.Parse(typeof(EnumAssinaturaDocumento), v))
                   .IsUnicode(false);

            modelBuilder.Property(e => e.Tipo)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumTipoDocumento)Enum.Parse(typeof(EnumTipoDocumento), v))
                    .IsUnicode(false);

            modelBuilder.Property(e => e.Etapa)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumEtapaDocumento)Enum.Parse(typeof(EnumEtapaDocumento), v))
                    .IsUnicode(false);

            modelBuilder.Property(e => e.Aviso).HasColumnType("varchar(500)");
            
            modelBuilder.Property(e => e.Visibilidade)
                .HasConversion(
                     v => v.ToString(),
                     v => (EnumVisibilidade)Enum.Parse(typeof(EnumVisibilidade), v))
                     .IsUnicode(false);

            modelBuilder.Property(e => e.PossuiAssinaturaResposavelEstagio).HasColumnType("bit(1)");
            modelBuilder.Property(e => e.PossuiCarimboCNPJ).HasColumnType("bit(1)");
            modelBuilder.Property(e => e.PossuiData).HasColumnType("bit(1)");

            modelBuilder.Property(e => e.DataHoraInclusao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();

            modelBuilder.Property(e => e.DataHoraUltimaAlteracao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Property(e => e.DataHoraExclusao).HasColumnType("datetime");
        }
    }
}
