using System;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;

namespace ADE.Infra.Data.Mapping
{
    public partial class TermoCompromissoConfiguration : IEntityTypeConfiguration<TermoCompromisso>
    {

        public void Configure(EntityTypeBuilder<TermoCompromisso> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.DataHoraInclusao)
                .HasColumnType("datetime")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Termos)
                .IsRequired()
                .HasColumnType("text");

            entity.Property(e => e.Titulo)
                .IsRequired()
                .HasColumnType("varchar(500)")
                .HasDefaultValueSql("'error'");

            entity.Property(e => e.Versao)
             .IsRequired()
                .HasColumnType("varchar(500)")
                .HasDefaultValueSql("'error'");
        }
    }
}
