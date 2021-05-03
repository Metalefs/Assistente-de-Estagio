using System;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Enums;

namespace ADE.Infra.Data.Mapping
{
    public partial class RequisitoConfiguration : IEntityTypeConfiguration<Requisito>
    {
        public void Configure(EntityTypeBuilder<Requisito> entity)
        {
            entity.HasKey(e => e.Identificador)
                    .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.NomeRequisito)
                .IsRequired()
                .HasColumnType("text");

            entity.Property(e => e.Bookmark)
                .IsRequired()
                .HasColumnType("varchar(50)");

            entity.Property(e => e.Descricao)
               .IsRequired()
               .HasColumnType("varchar(150)");

            entity.Property(e => e.MascaraEntrada)
               .HasColumnType("varchar(150)");
            
            entity.Property(e => e.Obrigatorio)
               .IsRequired()
               .HasColumnType("bit(1)");

            entity.Property(e=>e.Size).IsRequired();
            entity.Property(e => e.InText).IsRequired();
            
            entity.Property(e => e.ElementoHTMLRequisito)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumElementoHTMLRequisito)Enum.Parse(typeof(EnumElementoHTMLRequisito), v))
                    .IsUnicode(false);

            entity.Property(e => e.TipoElementoHTMLRequisito)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumTipoRequisito)Enum.Parse(typeof(EnumTipoRequisito), v))
                    .IsUnicode(false);
            
            entity.Property(e => e.Grupo)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumGrupoRequisito)Enum.Parse(typeof(EnumGrupoRequisito), v))
                    .IsUnicode(false);

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
