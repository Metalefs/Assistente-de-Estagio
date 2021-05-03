using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ADE.Dominio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADE.Infra.Data.Mapping
{
    public class OpcaoRequisitoConfiguration : IEntityTypeConfiguration<OpcaoRequisito>
    {
        public OpcaoRequisitoConfiguration()
        {}

        public void Configure(EntityTypeBuilder<OpcaoRequisito> modelBuilder)
        {
            modelBuilder.HasKey(e => new { e.IdRequisito, e.Valor })
                    .HasName("PRIMARY");

            modelBuilder.Property(e => e.IdRequisito).HasColumnType("int(11)");

            modelBuilder.Property(e => e.Valor).HasColumnType("varchar(50)");

            modelBuilder.HasOne(d => d.IdRequisitoNavigation)
                    .WithMany(p => p.OpcaoRequisito)
                    .HasForeignKey(d => d.IdRequisito)
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
