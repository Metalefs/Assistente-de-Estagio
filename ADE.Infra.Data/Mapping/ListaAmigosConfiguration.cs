using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Enums;
using System;
using ADE.Dominio.Models.RelacaoEntidades;

namespace ADE.Infra.Data.Mapping
{
    public partial class ListaAmigosConfiguration : IEntityTypeConfiguration<ListaAmigos>
    {

        public void Configure(EntityTypeBuilder<ListaAmigos> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");
             
            entity.Property(e => e.IdUsuario)
              .IsRequired()
              .HasColumnType("varchar(50)");

            entity.Property(e => e.IdAmigo)
             .IsRequired()
             .HasColumnType("varchar(50)");

            entity.Property(e => e.TipoRelacao)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumTipoRelacionamento)Enum.Parse(typeof(EnumTipoRelacionamento), v))
                    .IsUnicode(false);

            //RELACIONAMENTOS
            entity.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.ListaAmigos)
               .HasForeignKey(d => d.IdUsuario)
               .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.ListaAmigos)
               .HasForeignKey(d => d.IdAmigo)
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
