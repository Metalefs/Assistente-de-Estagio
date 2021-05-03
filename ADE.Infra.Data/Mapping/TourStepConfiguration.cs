using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ADE.Dominio.Models.Individuais;

namespace ADE.Infra.Data.Mapping
{
    public partial class TourStepConfiguration : IEntityTypeConfiguration<TourStep>
    {

        public void Configure(EntityTypeBuilder<TourStep> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.Posicao).HasColumnType("int(11)");

            entity.Property(e => e.Titulo).IsRequired().HasColumnType("varchar(500)").HasDefaultValueSql("'error'");

            entity.Property(e => e.IdElemento).IsRequired().HasColumnType("varchar(500)").HasDefaultValueSql("'error'");

            entity.Property(e => e.Conteudo).IsRequired().HasColumnType("varchar(500)").HasDefaultValueSql("'error'");
            
            entity.Property(e => e.Area).IsRequired().HasColumnType("varchar(500)").HasDefaultValueSql("'error'");

            entity.Property(e => e.Controlador).IsRequired().HasColumnType("varchar(500)").HasDefaultValueSql("'error'");
            
            entity.Property(e => e.View).IsRequired().HasColumnType("varchar(500)").HasDefaultValueSql("'error'");
            
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
