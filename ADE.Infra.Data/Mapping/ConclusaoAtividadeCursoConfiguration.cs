using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.RelacaoEntidades;

namespace ADE.Infra.Data.Mapping
{
    public partial class ConclusaoAtividadeCursoConfiguration : IEntityTypeConfiguration<ConclusaoAtividadeCurso>
    {

        public void Configure(EntityTypeBuilder<ConclusaoAtividadeCurso> entity)
        {
            entity.HasKey(e => e.Identificador)
                     .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");
            entity.Property(e => e.IdAtividade).HasColumnType("int(11)");
             
            entity.Property(e => e.IdUsuario)
              .IsRequired()
              .HasColumnType("varchar(50)");
            //RELACIONAMENTOS

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.ConclusaoAtividadeCurso)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKConclusaoAtividadeCursoIdUsuario");

            entity.HasOne(d => d.Atividade)
                .WithMany(p => p.ConclusoesAtividade)
                .HasForeignKey(d => d.IdAtividade)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKConclusaoAtividadeCursoIdAtividade");

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
