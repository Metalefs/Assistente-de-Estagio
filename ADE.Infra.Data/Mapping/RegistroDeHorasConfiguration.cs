using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADE.Infra.Data.Mapping
{
    public partial class RegistroDeHorasConfiguration : IEntityTypeConfiguration<RegistroDeHoras>
    {
        public RegistroDeHorasConfiguration()
        {}

        public void Configure(EntityTypeBuilder<RegistroDeHoras> entity)
        {
            entity.HasKey(e => new { e.Identificador })
                .HasName("PRIMARY");

            entity.Property(e => e.IdUsuario)
             .IsRequired()
             .HasColumnType("varchar(50)");

            //RELACIONAMENTOS
            entity.HasOne(d => d.Usuario)
             .WithMany(p => p.RegistroDeHoras)
             .HasForeignKey(d => d.IdUsuario)
             .OnDelete(DeleteBehavior.ClientSetNull);


            entity.Property(d => d.Data).IsRequired();
            entity.Property(d => d.HoraInicio).IsRequired();
            entity.Property(d => d.HoraFim).IsRequired();
            entity.Property(d => d.Atividade).IsRequired();
            entity.Property(d => d.CargaHoraria).IsRequired();

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

