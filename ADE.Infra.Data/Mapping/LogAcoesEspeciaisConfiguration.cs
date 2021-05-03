using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;

namespace ADE.Infra.Data.Mapping
{
    public partial class LogAcoesEspeciaisConfiguration : IEntityTypeConfiguration<LogAcoesEspeciais>
    {

        public void Configure(EntityTypeBuilder<LogAcoesEspeciais> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.DataHoraInclusao)
                .HasColumnType("datetime")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.LocalOrigem)
                .IsRequired()
                .HasColumnType("varchar(150)")
                .HasDefaultValueSql("'Error'");

            entity.Property(e => e.Mensagem)
                .IsRequired()
                .HasColumnType("text")
                .HasDefaultValueSql("'error'");

            entity.Property(e => e.AcoesSistema)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumTipoLog)Enum.Parse(typeof(EnumTipoLog), v))
                    .IsUnicode(false);

            entity.Property(e => e.IdUsuario)
               .IsRequired()
               .HasColumnType("varchar(50)");

            //RELACIONAMENTOS
            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.LogAcoesEspeciais)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HistoricoEspecialSistema_ibfk_1");
        }
    }
}
