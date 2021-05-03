using System;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;

namespace ADE.Infra.Data.Mapping
{
    public partial class FAQConfiguration : IEntityTypeConfiguration<FAQ>
    {

        public void Configure(EntityTypeBuilder<FAQ> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.Pergunta)
                .IsRequired()
                .HasColumnType("varchar(500)")
                .HasDefaultValueSql("'error'");

            entity.Property(e => e.Resposta)
                .HasColumnType("varchar(500)")
                .HasDefaultValueSql("'error'");

            entity.Property(e => e.Pontuacao).HasColumnType("int(11)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumStatusFAQ)Enum.Parse(typeof(EnumStatusFAQ), v))
                    .IsUnicode(false);

            entity.Property(e => e.IdUsuarioPergunta)
              .IsRequired()
              .HasColumnType("varchar(50)");
            
            entity.Property(e => e.IdUsuarioResposta)
             .HasColumnType("varchar(50)");

            //RELACIONAMENTOS
            entity.HasOne(a => a.Instituicao)
                 .WithMany(b => b.FAQ)
                 .HasForeignKey(b => b.IdInstituicao);

            entity.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.FAQS)
               .HasForeignKey(d => d.IdUsuarioPergunta)
               .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.FAQS)
               .HasForeignKey(d => d.IdUsuarioResposta)
               .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.IdInstituicao).HasColumnType("int(11)");
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
