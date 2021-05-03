using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ADE.Infra.Data.Mapping
{
    public class RegulamentacaoCursoConfiguration : IEntityTypeConfiguration<RegulamentacaoCurso>
    {
        public RegulamentacaoCursoConfiguration()
        {}

        public void Configure(EntityTypeBuilder<RegulamentacaoCurso> modelBuilder)
        {
            modelBuilder.HasKey(e => new { e.Identificador })
                    .HasName("PRIMARY");

            modelBuilder.Property(e => e.Endereco);

            modelBuilder.Property(e => e.IdCurso).HasColumnType("int(11)");

            modelBuilder.HasOne(d => d.IdCursoNavigation)
                    .WithOne(p => p.RegulamentacaoCursos)
                    .HasForeignKey(typeof(Curso),"IdCurso")
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
