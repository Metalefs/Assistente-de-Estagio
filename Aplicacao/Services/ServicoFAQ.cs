using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Utilidades.Extensions;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ADE.Aplicacao.Services
{
    public class ServicoFAQ: ServicoBase<FAQ>
    {
        private UnitOfWork unitOfWork;
        private ServicoUsuario ServicoUsuario;

        public ServicoFAQ(ref UnitOfWork _unitOfWork, UserManager<UsuarioADE> userManager) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ServicoUsuario = new ServicoUsuario(unitOfWork, userManager);
        }

        public override async Task LogCadastramento(UsuarioADE usuario, FAQ entidade)
        {
            string Mensagem = $"Sua pergunta ({entidade.Pergunta}) foi cadastrada e está no estado ({entidade.Status.GetDescription()})";
            IList<string> AutorizacoesUsuario = await ServicoUsuario.ObterAutorizacaoUsuario(usuario);
            if (AutorizacoesUsuario.Any(x=> x == EnumTipoUsuario.Admin.GetDescription()))
                await LogAlteracaoEntidade(usuario, new FAQ(), entidade, EnumEntidadesSistema.FAQ, EnumTipoLog.CriacaoFAQ, Mensagem);
            string Cabecalho = $"Sua pergunta ({entidade.Pergunta}) foi cadastrada";
            NotificacaoIndividual notificacao = new NotificacaoIndividual(entidade.IdUsuarioResposta, entidade.IdUsuarioPergunta, Cabecalho, Mensagem);
            await CriarNotificacaoIndividual(usuario, notificacao);
        }

        public override async Task LogAtualizacao(UsuarioADE usuario, FAQ Resposta, string Mensagem = null)
        {
            FAQ Pergunta = await BuscarPorId(Resposta.Identificador);
            IList<string> AutorizacoesUsuario = await ServicoUsuario.ObterAutorizacaoUsuario(usuario);
            if (AutorizacoesUsuario.Any(x => x == EnumTipoUsuario.Admin.GetDescription()))
                await LogAlteracaoEntidade(usuario, Pergunta, Resposta, EnumEntidadesSistema.FAQ, EnumTipoLog.AlteracaoFAQ, Mensagem);
            string Cabecalho = MensagemCabecalhoAtualizacaoFAQ(Resposta);
            NotificacaoIndividual notificacao = new NotificacaoIndividual(Resposta.IdUsuarioResposta, Resposta.IdUsuarioPergunta, Cabecalho, Mensagem);
            await CriarNotificacaoIndividual(usuario, notificacao);
        }

        private string MensagemCabecalhoAtualizacaoFAQ(FAQ entidade)
        {
            switch (entidade.Status)
            {
                case EnumStatusFAQ.Respondido:
                    return $"Alterações na sua pergunta ({entidade.Pergunta}) !";
                case EnumStatusFAQ.Rejeitado:
                    return $"Sua pergunta ({entidade.Pergunta}) foi rejeitada por um administrador";
                case EnumStatusFAQ.Aberto:
                    return $"Sua pergunta ({entidade.Pergunta}) sofreu alterações";
            }
            throw new Exception("FAQ em estádo inválido");
        }
        
        private string MensagemCabecalhoDelecaoFAQ(FAQ entidade)
        {
            switch (entidade.Status)
            {
                case EnumStatusFAQ.Respondido:
                    return $"Alterações na sua pergunta ({entidade.Pergunta}) !";
                case EnumStatusFAQ.Rejeitado:
                    return $"Sua pergunta ({entidade.Pergunta}) foi rejeitada por um administrador";
                case EnumStatusFAQ.Aberto:
                    return $"Sua pergunta ({entidade.Pergunta}) sofreu alterações";
            }
            throw new Exception("FAQ em estádo inválido");
        }

        public override async Task LogRemocao(UsuarioADE usuario, FAQ entidade)
        {
            IList<string> AutorizacoesUsuario = await ServicoUsuario.ObterAutorizacaoUsuario(usuario);
            if (AutorizacoesUsuario.Any(x => x == EnumTipoUsuario.Admin.GetDescription()))
            {
                string Cabecalho = MensagemCabecalhoDelecaoFAQ(entidade);
                NotificacaoIndividual notificacao = new NotificacaoIndividual(entidade.IdUsuarioResposta, entidade.IdUsuarioPergunta, Cabecalho, $"Sua pergunta '{entidade.Pergunta}' foi removida por um administrador");
                await CriarNotificacaoIndividual(usuario, notificacao);
            }
        }

        public async new Task<List<FAQ>> Filtrar(Expression<Func<FAQ, bool>> expression) // Não ideal
        {
            List<FAQ> ListaFAQS = await unitOfWork.RepositorioBase<FAQ>().Filtrar(expression);
            return ListaFAQS.Where(x => x.Status != EnumStatusFAQ.Rejeitado).ToList();
        }

        public async Task<List<FAQ>> BuscarPorIdUsuario(string IdUsuario)
        {
            List<FAQ> lista = await unitOfWork.RepositorioBase<FAQ>().Filtrar(x => x.IdUsuarioPergunta == IdUsuario || x.IdUsuarioResposta == IdUsuario);
            return lista.OrderByDescending(x => x.Pontuacao).ToList();
        }

        public async Task<List<FAQ>> BuscarPorIdInstituicao(int IdInstituicao)
        {
            List<FAQ> lista  = await unitOfWork.RepositorioBase<FAQ>().Filtrar(x => x.IdInstituicao == IdInstituicao && x.Status == EnumStatusFAQ.Respondido);
            return lista.OrderByDescending(x => x.Pontuacao).ToList();
        }

        public async Task<List<FAQ>> BuscarPorIdInstituicaoAdmin(int IdInstituicao)
        {
            return await unitOfWork.RepositorioBase<FAQ>().Filtrar(x => x.IdInstituicao == IdInstituicao && x.Status != EnumStatusFAQ.Rejeitado);
        }

        public async Task<int> Like(UsuarioADE usuario, int IdFaq)
        {
            List<FAQ> FAQ = await unitOfWork.RepositorioBase<FAQ>().Filtrar(x => x.Identificador == IdFaq && x.Status == EnumStatusFAQ.Respondido);
            int Pontuacao = 0;
            foreach(FAQ faq in FAQ)
            {
                faq.Pontuacao++;
                Pontuacao += faq.Pontuacao;
                await AtualizarAsync(usuario, faq, $"Sua pergunta {faq.Pergunta} teve 1 like! Pontuação: {faq.Pontuacao}");
            }
            return Pontuacao;
        }

        public async Task<int> Dislike(UsuarioADE usuario, int IdFaq)
        {
            List<FAQ> FAQ = await unitOfWork.RepositorioBase<FAQ>().Filtrar(x => x.Identificador == IdFaq && x.Status != EnumStatusFAQ.Rejeitado);
            int Pontuacao = 0;
            foreach (FAQ faq in FAQ)
            {
                faq.Pontuacao--;
                Pontuacao += faq.Pontuacao;
                await AtualizarAsync(usuario, faq, $"Sua pergunta {faq.Pergunta} teve 1 dislike! Pontuação: {faq.Pontuacao}");
            }
            return Pontuacao;
        }

    }
}
