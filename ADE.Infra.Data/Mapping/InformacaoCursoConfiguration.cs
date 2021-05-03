using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ADE.Infra.Data.Mapping
{
    public class InformacaoCursoConfiguration : IEntityTypeConfiguration<InformacaoCurso>
    {
        public InformacaoCursoConfiguration()
        {}

        public void Configure(EntityTypeBuilder<InformacaoCurso> modelBuilder)
        {
            modelBuilder.HasKey(e => new { e.Identificador })
                    .HasName("PRIMARY");

            modelBuilder.Property(e => e.IdCurso).HasColumnType("int(11)");

            modelBuilder.Property(e => e.TipoInformacao)
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumTipoInformacao)Enum.Parse(typeof(EnumTipoInformacao), v))
                    .IsUnicode(false);

            modelBuilder.Property(e => e.ConteudoInformacao).HasColumnType("varchar(250)");

            modelBuilder.HasOne(d => d.IdCursoNavigation)
                    .WithMany(p => p.InformacaoCursos)
                    .HasForeignKey(d => d.IdCurso)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Property(e => e.DataHoraInclusao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();

            modelBuilder.Property(e => e.DataHoraUltimaAlteracao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Property(e => e.DataHoraExclusao)
               .HasColumnType("datetime");
        }
    }
}
