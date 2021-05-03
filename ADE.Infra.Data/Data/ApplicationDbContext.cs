using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ADE.Infra.Data.Mapping;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.RelacaoEntidades;

namespace Assistente_de_Estagio.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<UsuarioADE>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AlteracaoEntidadesSistema> AlteracaoEntidadesSistema { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<OpcaoRequisito> OpcaoRequisito { get; set; }   
        public virtual DbSet<Requisito> Requisito { get; set; }
        public virtual DbSet<RequisitoDeDocumento> RequisitoDeDocumento { get; set; }
        public virtual DbSet<RequisitoDeUsuario> RequisitoDeUsuario { get; set; }
        public virtual DbSet<SysLogs> SysLogs { get; set; }
        public virtual DbSet<HistoricoGeracaoDocumento> HistoricoGeracaoDocumento { get; set; }
        public virtual DbSet<LogAcoesEspeciais> LogAcoesEspeciais { get; set; }
        public virtual DbSet<Logins> Logins { get; set; }
        public virtual DbSet<InformacaoCurso> InformacaoCurso { get; set; }
        public virtual DbSet<InformacaoDocumento> InformacaoDocumento { get; set; }
        public virtual DbSet<Instituicao> Instituicao { get; set; }
        public virtual DbSet<VisualizacaoNotificacaoGeral> VisualizacaoNotificacaoGeral { get; set; }
        public virtual DbSet<FAQ> FAQ { get; set; }
        public virtual DbSet<MensagemIndividual> MensagemIndividual { get; set; }
        public virtual DbSet<NotificacaoIndividual> NotificacaoIndividual { get; set; }
        public virtual DbSet<RegistroDeHoras> RegistroDeHoras { get; set; }
        public virtual DbSet<TermoCompromisso> TermoCompromisso { get; set; }
        public virtual DbSet<AtividadeEstagio> AtividadeEstagio { get; set; }
        public virtual DbSet<AtividadeUsuario> AtividadeUsuario { get; set; }
        public virtual DbSet<ConclusaoAtividadeCurso> ConclusaoAtividadeCurso { get; set; }
        public virtual DbSet<TourStep> TourStep { get; set; }
        public virtual DbSet<ListaAmigos> ListaAmigos { get; set; }
        public virtual DbSet<AreaEstagioCurso> AreaEstagioCurso { get; set; }
        public virtual DbSet<RegulamentacaoCurso> RegulamentacaoCurso { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new CursoConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentoConfiguration());
            modelBuilder.ApplyConfiguration(new OpcaoRequisitoConfiguration());
            modelBuilder.ApplyConfiguration(new RequisitoConfiguration());
            modelBuilder.ApplyConfiguration(new RequisitoDeDocumentoConfiguration());
            modelBuilder.ApplyConfiguration(new RequisitoDeUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new SysLogsConfiguration());
            modelBuilder.ApplyConfiguration(new HistoricoGeracaoDocumentoConfiguration());
            modelBuilder.ApplyConfiguration(new LogAcoesEspeciaisConfiguration());
            modelBuilder.ApplyConfiguration(new LoginsConfiguration());
            modelBuilder.ApplyConfiguration(new InformacaoCursoConfiguration());
            modelBuilder.ApplyConfiguration(new InformacaoDocumentoConfiguration());
            modelBuilder.ApplyConfiguration(new InstituicaoConfiguration());
            modelBuilder.ApplyConfiguration(new VisualizacaoNotificacaoGeralConfiguration());
            modelBuilder.ApplyConfiguration(new FAQConfiguration());
            modelBuilder.ApplyConfiguration(new MensagemIndividualConfiguration());
            modelBuilder.ApplyConfiguration(new NotificacaoIndividualConfiguration());
            modelBuilder.ApplyConfiguration(new RegistroDeHorasConfiguration());
            modelBuilder.ApplyConfiguration(new TermoCompromissoConfiguration());
            modelBuilder.ApplyConfiguration(new AtividadeEstagioConfiguration());
            modelBuilder.ApplyConfiguration(new AtividadeUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new ConclusaoAtividadeCursoConfiguration());
            modelBuilder.ApplyConfiguration(new TourStepConfiguration());
            modelBuilder.ApplyConfiguration(new ListaAmigosConfiguration());
            modelBuilder.ApplyConfiguration(new AreaEstagioCursoConfiguration());
            modelBuilder.ApplyConfiguration(new RegulamentacaoCursoConfiguration());
        }
    }
}
