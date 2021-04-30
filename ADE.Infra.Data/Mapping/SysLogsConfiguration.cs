using System;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADE.Infra.Data.Mapping
{
    public partial class SysLogsConfiguration : IEntityTypeConfiguration<SysLogs>
    {

        public void Configure(EntityTypeBuilder<SysLogs> entity)
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
                    .HasColumnName("Acao_IdAcao")
                    .IsRequired()
                    .HasConversion(
                        v => v.ToString(),
                        v => (EnumTipoLog)Enum.Parse(typeof(EnumTipoLog), v))
                        .IsUnicode(false);
        }
    }
}
