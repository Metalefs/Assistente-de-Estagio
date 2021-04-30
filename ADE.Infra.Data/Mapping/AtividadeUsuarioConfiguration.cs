using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.Enums;
using System;

namespace ADE.Infra.Data.Mapping
{
    public partial class AtividadeUsuarioConfiguration : IEntityTypeConfiguration<AtividadeUsuario>
    {

        public void Configure(EntityTypeBuilder<AtividadeUsuario> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");
             
            entity.Property(e => e.IdUsuario)
              .IsRequired()
              .HasColumnType("varchar(50)");

            entity.Property(d => d.Data).IsRequired();
            entity.Property(d => d.Descricao);
            entity.Property(d => d.IdCurso).IsRequired();
            entity.Property(d => d.Concluido).IsRequired();
            entity.Property(d => d.TipoAtividade).IsRequired().HasConversion(
                    v => v.ToString(),
                    v => (EnumTipoAtividadeEstagio)Enum.Parse(typeof(EnumTipoAtividadeEstagio), v))
                    .IsUnicode(false);

            entity.Property(d => d.Titulo).IsRequired();

            entity.Property(d => d.Visibilidade).IsRequired().HasConversion(
                    v => v.ToString(),
                    v => (EnumVisibilidadeAtividadeUsuario)Enum.Parse(typeof(EnumVisibilidadeAtividadeUsuario), v))
                    .IsUnicode(false);

            //RELACIONAMENTOS

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.AtividadesUsuario)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAtividadeUsuarioIdUsuario");

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
