using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.Enums;
using System;

namespace ADE.Infra.Data.Mapping
{
    public partial class AtividadeEstagioConfiguration : IEntityTypeConfiguration<AtividadeEstagio>
    {

        public void Configure(EntityTypeBuilder<AtividadeEstagio> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");
             
            entity.Property(e => e.IdCurso)
              .IsRequired()
              .HasColumnType("int(11)");
            
            entity.Property(e => e.IdentificadorEntidadeAtividade)
              .IsRequired()
              .HasColumnType("int(11)");

            entity.Property(d => d.Descricao);
            entity.Property(d => d.TipoAtividade).IsRequired();
            entity.Property(d => d.CondicaoVezes);
            entity.Property(d => d.EnumEntidade).IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumEntidadesSistema)Enum.Parse(typeof(EnumEntidadesSistema), v))
                    .IsUnicode(false);
            entity.Property(d => d.Titulo).IsRequired();

            //RELACIONAMENTOS

            entity.HasOne(d => d.Curso)
                .WithMany(p => p.Atividades)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAtividadeEstagioIdCurso");

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
