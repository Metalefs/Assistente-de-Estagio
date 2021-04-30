using ADE.Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADE.Infra.Data.Mapping
{
    public partial class HistoricoGeracaoDocumentoConfiguration : IEntityTypeConfiguration<HistoricoGeracaoDocumento>
    {
        public void Configure(EntityTypeBuilder<HistoricoGeracaoDocumento> entity)
        {
            //CHAVE 
            entity.HasKey(e => e.Identificador)
                   .HasName("PRIMARY");

            entity.HasIndex(e => e.IdUsuario)
                .HasDatabaseName("IdUsuario");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            //CAMPOS
            entity.Property(e => e.DataHoraInclusao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DataHoraUltimaAlteracao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAddOrUpdate();

            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasColumnType("text");

            entity.Property(e => e.IdUsuario)
                .IsRequired()
                .HasColumnType("varchar(50)");

            entity.Property(e => e.Documento)
                .HasColumnName("IdDocumento")
                .IsRequired()
                .HasColumnType("int(11)");

            //RELACIONAMENTOS
            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.HistoricoGeracaoDocumento)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HistoricoSistema_ibfk_1");

            entity.HasOne(e => e.IdDocumentoNavigation)
                .WithMany(p => p.IdHistoricoGeracaoDocumento)
                .HasForeignKey(d => d.Documento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HistoricoSistema_ibfk_2");
        }
    }
}
