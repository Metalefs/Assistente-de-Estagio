using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ADE.Infra.Data.Mapping
{
    public partial class InstituicaoConfiguration : IEntityTypeConfiguration<Instituicao>
    {
        public InstituicaoConfiguration()
        { }

        public void Configure(EntityTypeBuilder<Instituicao> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Identificador)
                    .HasName("PRIMARY");

            modelBuilder.Property(e => e.Identificador).HasColumnType("int(11)");

            modelBuilder.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnType("varchar(256)");

            modelBuilder.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

            modelBuilder.Property(e => e.Logo)
                    .IsRequired();

            modelBuilder.Property(e => e.Descricao).HasColumnType("text");

            modelBuilder.Property(e => e.Diretor).HasColumnType("varchar(256)");

            modelBuilder.Property(e => e.Email)
                    .HasColumnType("varchar(256)");
            
            modelBuilder.Property(e => e.Website)
                    .HasColumnType("varchar(256)");

            modelBuilder.Property(e => e.Endereco)
                    .HasColumnType("varchar(256)");

            modelBuilder.Property(e => e.Telefone)
                    .HasColumnType("varchar(256)");

            modelBuilder.Property(e => e.DataHoraInclusao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAdd();

            modelBuilder.Property(e => e.DataHoraUltimaAlteracao)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("current_timestamp()")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Property(e => e.DataHoraExclusao)
               .HasColumnType("datetime");
        }
    }
}
