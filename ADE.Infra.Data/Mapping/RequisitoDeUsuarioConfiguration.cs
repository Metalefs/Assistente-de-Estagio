using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADE.Infra.Data.Mapping
{
    public partial class RequisitoDeUsuarioConfiguration : IEntityTypeConfiguration<RequisitoDeUsuario>
    {

        public void Configure(EntityTypeBuilder<RequisitoDeUsuario> entity)
        {
            entity.HasKey(e => new { e.UserId, e.IdRequisito })
                    .HasName("PRIMARY");

            entity.HasIndex(e => e.IdRequisito)
                .HasDatabaseName("FKRequisitoDeUsuarioIdRequisito");

            entity.Property(e => e.UserId).HasColumnType("varchar(255)");

            entity.Property(e => e.IdRequisito).HasColumnType("int(11)");

            entity.Property(e => e.Valor)
                .HasColumnName("valor")
                .HasColumnType("varchar(256)");

            entity.HasOne(d => d.IdRequisitoNavigation)
                .WithMany(p => p.RequisitoDeUsuario)
                .HasForeignKey(d => d.IdRequisito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKRequisitoDeUsuarioIdRequisito");

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
