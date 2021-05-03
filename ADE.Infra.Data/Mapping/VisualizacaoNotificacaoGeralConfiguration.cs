using System;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.RelacaoEntidades;

namespace ADE.Infra.Data.Mapping
{
    public partial class VisualizacaoNotificacaoGeralConfiguration : IEntityTypeConfiguration<VisualizacaoNotificacaoGeral>
    {
        public void Configure(EntityTypeBuilder<VisualizacaoNotificacaoGeral> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.DataHoraInclusao)
                .HasColumnType("datetime")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.IdUsuario)
               .IsRequired()
               .HasColumnType("varchar(50)");

            entity.Property(e => e.IdNotificacao)
               .IsRequired()
               .HasColumnType("int(11)");

            //RELACIONAMENTOS
            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.NotificacoesVisualizadas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VisualizacaoNotificacao_ibfk_1");

            entity.HasOne(x => x.IdNotificacaoNavigation)
              .WithMany(p => p.Notificacao)
              .HasForeignKey(x => x.IdNotificacao)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("UKIdNotificacao_VisualizacaoNotificacao");

        }
    }
}
