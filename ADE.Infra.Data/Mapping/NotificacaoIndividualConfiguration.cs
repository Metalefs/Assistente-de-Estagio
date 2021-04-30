using System;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;

namespace ADE.Infra.Data.Mapping
{
    public partial class NotificacaoIndividualConfiguration : IEntityTypeConfiguration<NotificacaoIndividual>
    {

        public void Configure(EntityTypeBuilder<NotificacaoIndividual> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.Conteudo)
                .IsRequired()
                .HasColumnType("varchar(500)")
                .HasDefaultValueSql("'error'");
            
            entity.Property(e => e.Cabecalho)
                .IsRequired()
                .HasColumnType("varchar(500)")
                .HasDefaultValueSql("'error'");
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumStatusNotificacaoIndividual)Enum.Parse(typeof(EnumStatusNotificacaoIndividual), v))
                    .IsUnicode(false);

            entity.Property(e => e.IdUsuarioRemetente)
              .IsRequired()
              .HasColumnType("varchar(50)");
            
            entity.Property(e => e.IdUsuarioDestino)
             .HasColumnType("varchar(50)");

            //RELACIONAMENTOS
            entity.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.NotificacoesIndividuais)
               .HasForeignKey(d => d.IdUsuarioRemetente)
               .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.NotificacoesIndividuais)
               .HasForeignKey(d => d.IdUsuarioDestino)
               .OnDelete(DeleteBehavior.ClientSetNull);
            //

            entity.Property(e => e.DataHoraInclusao)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("current_timestamp()")
                    .ValueGeneratedOnAdd();

            entity.Property(e => e.DataHoraUltimaAlteracao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAddOrUpdate();

            entity.Property(e => e.DataHoraExclusao)
               .HasColumnType("datetime");
        }
    }
}
