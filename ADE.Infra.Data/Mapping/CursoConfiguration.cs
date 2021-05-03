using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADE.Infra.Data.Mapping
{
    public partial class CursoConfiguration : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> entity)
        {
            entity.HasKey(e => e.Identificador)
                .HasName("PRIMARY");

            entity.Property(e => e.Identificador).HasColumnType("int(11)");

            entity.Property(e => e.CoordenadorCurso)
                .IsRequired()
                .HasColumnType("varchar(150)");

            entity.Property(e => e.EmailCoordenadorCurso)
                .IsRequired()
                .HasColumnType("varchar(256)");

            entity.Property(e => e.NomeCurso)
                .IsRequired()
                .HasColumnType("varchar(150)");

            entity.Property(e => e.Sigla)
                .IsRequired()
                .HasColumnType("varchar(13)");

            entity.Property(e => e.Alerta)
                .HasColumnType("tinytext");
            entity.Property(e => e.Informacao)
                .HasColumnType("tinytext");
            entity.Property(e => e.CargaHorariaMinimaEstagio)
                .HasColumnType("int(11)");

            entity.Property(e => e.DescricaoCurso)
                .IsRequired()
                .HasColumnType("text");

            //RELACIONAMENTOS
            entity.HasOne(e => e.Instituicao)
               .WithMany(p => p.Cursos)
               .HasForeignKey(d => d.IdInstituicao)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("UKIdInstituicao_Curso");

            entity.Property(e => e.IdInstituicao).HasColumnType("int(11)");

            entity.HasMany(e => e.Atividades)
               .WithOne(p => p.Curso)
               .HasForeignKey(d => d.IdCurso)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("UKIdAtividade_Curso");

            entity.HasMany(e => e.AreasEstagio)
               .WithOne(p => p.IdCursoNavigation)
               .HasForeignKey(d => d.IdCurso)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("UKIdAreasEstagio_Curso");
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
