using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Infra.Data.Mapping
{
    public partial class AlteracaoEntidadesSistemaConfiguration : IEntityTypeConfiguration<AlteracaoEntidadesSistema>
    {
        public void Configure(EntityTypeBuilder<AlteracaoEntidadesSistema> entity)
        {
            entity.HasKey(e => e.Identificador)
                    .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.IdentificadorEntidade).HasColumnType("int(11)");

            entity.Property(e => e.IdAutor)
               .IsRequired()
               .HasColumnType("varchar(50)");

            //RELACIONAMENTO
            entity.HasOne(d => d.Autor)
                .WithMany(p => p.AlteracaoEntidadesSistema)
                .HasForeignKey(d => d.IdAutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HistoricoEspecialSistema_ibfk_1");

            entity.Property(e => e.Entidade)
                   .IsRequired()
                   .HasConversion(
                       v => v.ToString(),
                       v => (EnumEntidadesSistema)Enum.Parse(typeof(EnumEntidadesSistema), v))
                       .IsUnicode(false);

            entity.Property(e => e.MensagemAlteracao)
                .IsRequired()
                .HasColumnType("varchar(500)");

            entity.Property(e => e.DataHoraInclusao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();
        }
    }
}
