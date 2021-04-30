using System;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;

namespace ADE.Infra.Data.Mapping
{
    public partial class LoginsConfiguration : IEntityTypeConfiguration<Logins>
    {

        public void Configure(EntityTypeBuilder<Logins> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.HasIndex(e => e.IdUsuario)
                    .HasDatabaseName("UKIdUsuario");

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Logins)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UKIdUsuario_Logins");

            entity.Property(e => e.IdUsuario).HasColumnType("varchar(255)");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.DataHoraLogin)
                    .IsRequired()
                    .HasColumnType("datetime");

            entity.Property(e => e.DataHoraLogout)
                .HasColumnType("datetime");
    
            entity.Property(e => e.DataHoraInclusao)
                .HasColumnType("datetime")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();
        }
    }
}
