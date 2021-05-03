using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ADE.Infra.Data.Mapping
{
    public class AreaEstagioCursoConfiguration : IEntityTypeConfiguration<AreaEstagioCurso>
    {
        public AreaEstagioCursoConfiguration()
        {}

        public void Configure(EntityTypeBuilder<AreaEstagioCurso> modelBuilder)
        {
            modelBuilder.HasKey(e => new { e.Identificador })
                    .HasName("PRIMARY");

            modelBuilder.Property(e => e.IdCurso).IsRequired();
            modelBuilder.Property(e => e.Nome).IsRequired();

           modelBuilder.HasOne(d => d.IdCursoNavigation)
                    .WithMany(p => p.AreasEstagio)
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
