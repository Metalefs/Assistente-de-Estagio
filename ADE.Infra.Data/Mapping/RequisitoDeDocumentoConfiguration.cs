using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADE.Infra.Data.Mapping
{
    public partial class RequisitoDeDocumentoConfiguration : IEntityTypeConfiguration<RequisitoDeDocumento>
    {
        public RequisitoDeDocumentoConfiguration()
        {}

        public void Configure(EntityTypeBuilder<RequisitoDeDocumento> entity)
        {
            entity.HasKey(e => new { e.IdDocumento, e.IdRequisito })
                .HasName("PRIMARY");

            entity.HasIndex(e => e.IdDocumento)
                .HasDatabaseName("IdDocumento");

            entity.HasIndex(e => e.IdRequisito)
                .HasDatabaseName("IdRequisito");

            entity.Property(e => e.IdDocumento).HasColumnType("int(11)");

            entity.Property(e => e.IdRequisito).HasColumnType("int(11)");

            entity.Property(e => e.OrdemRequisito)
                .HasColumnType("tinyint(4)")
                .HasDefaultValueSql("'1'");

            entity.HasOne(d => d.IdDocumentoNavigation)
                .WithMany(p => p.RequisitoDeDocumento)
                .HasForeignKey(d => d.IdDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdDocumento");

            entity.HasOne(d => d.IdRequisitoNavigation)
                .WithMany(p => p.RequisitoDeDocumento)
                .HasForeignKey(d => d.IdRequisito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdRequisito");

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

