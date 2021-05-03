using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Assistente_de_Estagio.Data;
using ADE.Dominio.Models;
using System.Linq;
using ADE.Dominio.Models.Individuais;
using ADE.Utilidades.Seeding;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using ADE.Infra.Data.UOW;
using static ADE.Utilidades.Constants.NomesDocumentos;
using Microsoft.EntityFrameworkCore;

namespace ADE.Aplicacao.Services
{
    public class SeedingService
    {
        public static async Task Seed(IServiceProvider serviceProvider, IConfiguration Configuration, IHostingEnvironment env)
        {
            ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);
            ServicoAtividadeEstagio ServicoAtividadeEstagio = new ServicoAtividadeEstagio(ref unitOfWork);
            ServicoRequisitoDeDocumento ServicoRequisitoDeDocumento = new ServicoRequisitoDeDocumento(ref unitOfWork);
           
            if (!context.TermoCompromisso.Any())
            {
                #region TermoCompromisso
                TermoCompromisso TermoCompromisso = await TermoCompromissoSeed.TermoInicial(env);
                await context.TermoCompromisso.AddAsync(TermoCompromisso);
                await context.SaveChangesAsync();
                #endregion
            }

            if (!context.TourStep.Any())
            {
                #region Tour
                List<List<TourStep>> Passos = TourStepSeed.TourBase;
                Passos.ForEach(passo => context.TourStep.AddRangeAsync(passo));
                await context.SaveChangesAsync();
                #endregion
            }

            if (!context.Instituicao.Any())
            {
                #region Instituicoes
                Instituicao Facisa = InstituicaoSeed.Facisa(env);
                await context.Instituicao.AddAsync(Facisa);
                await context.SaveChangesAsync();
                #endregion
                if (!context.Curso.Any())
                {
                    #region Cursos
                    Curso ADS = CursoSeed.ADS(Facisa);
                    await context.Curso.AddAsync(ADS);
                    
                    Curso ADM = CursoSeed.ADM(Facisa);
                    await context.Curso.AddAsync(ADM);

                    Curso CC = CursoSeed.CC(Facisa);
                    await context.Curso.AddAsync(CC);

                    Curso LET = CursoSeed.LET(Facisa);
                    await context.Curso.AddAsync(LET);

                    Curso PED = CursoSeed.PED(Facisa);
                    await context.Curso.AddAsync(PED);
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion

                    if (!context.AreaEstagioCurso.Any())
                    {
                        #region AreasEstagio
                        List<AreaEstagioCurso> areasADS = AreasEstagioSeed.AreasDeEstagioADS(ADS);
                        await context.AreaEstagioCurso.AddRangeAsync(areasADS);

                        await context.SaveChangesAsync();
                        #endregion
                    }

                    if (!context.Requisito.Any())
                    {
                        #region Requisitos
                        List<Requisito> RequisitosBase = RequisitoSeed.RequisitosBase();
                        await context.Requisito.AddRangeAsync(RequisitosBase);

                        Requisito CursoAluno = new Requisito("Curso do Aluno(a) ", "Curso do aluno aplicante do estágio", ADE.Dominio.Models.Enums.EnumElementoHTMLRequisito.input, ADE.Dominio.Models.Enums.EnumTipoRequisito.text, ADE.Utilidades.Constants.NomesRequisitos.CursoAluno_, ADE.Dominio.Models.Enums.EnumGrupoRequisito.Aluno, 11, true, null, true);
                        await context.Requisito.AddAsync(CursoAluno);

                        RequisitosBase.Add(CursoAluno);

                        await context.SaveChangesAsync();
                        #endregion

                        if (!context.Documento.Any())
                        {

                            List<Documento> DocumentosADS = DocumentoSeed.DocumentosADS(ADS);
                            await context.Documento.AddRangeAsync(DocumentosADS);

                            List<Documento> DocumentosADM = DocumentoSeed.DocumentosADM(ADM);
                            await context.Documento.AddRangeAsync(DocumentosADM);

                            List<Documento> DocumentosCC = DocumentoSeed.DocumentosCC(CC);
                            await context.Documento.AddRangeAsync(DocumentosCC);

                            List<Documento> DocumentosLET = DocumentoSeed.DocumentosLET(LET);
                            await context.Documento.AddRangeAsync(DocumentosLET);

                            List<Documento> DocumentosPED = DocumentoSeed.DocumentosPED(PED);
                            await context.Documento.AddRangeAsync(DocumentosPED);

                            await context.SaveChangesAsync();

                            if (!context.RequisitoDeDocumento.Any())
                            {
                                RequisitoDeDocumentoSeed RequisitoDeDocumentoSeed = new RequisitoDeDocumentoSeed(context);

                                List<List<Documento>> DocumentosFacisa = new List<List<Documento>>()
                                {
                                    DocumentosADS,
                                    DocumentosADM,
                                    DocumentosCC,
                                    DocumentosLET,
                                    DocumentosPED
                                };

                                foreach(List<Documento> lista in DocumentosFacisa)
                                {
                                    foreach(Documento documento in lista)
                                    {
                                        try
                                        {
                                            if (documento.TituloDocumento == DeclaracaoOpcaoAreaEstágio_)
                                            {
                                                List<RequisitoDeDocumento> DeclaracaoOpcaoAreaEstágioRequisitoDeDocumento = RequisitoDeDocumentoSeed.RequisitosDeclaracaoOpcaoAreaEstagio(documento);
                                                await ServicoRequisitoDeDocumento.CadastrarAsync(DeclaracaoOpcaoAreaEstágioRequisitoDeDocumento);
                                            }

                                            if (documento.TituloDocumento == SolicitacaoEstagioSupervisionado_)
                                            {
                                                List<RequisitoDeDocumento> SolicitacaoEstagioSupervisionadoRequisitoDeDocumento = RequisitoDeDocumentoSeed.RequisitosSolicitacaoEstagioSupervisionado(documento);
                                                await ServicoRequisitoDeDocumento.CadastrarAsync(SolicitacaoEstagioSupervisionadoRequisitoDeDocumento);
                                            }

                                            if (documento.TituloDocumento == AutorizacaoRealizacaoEstagio_ && documento.IdCurso != ADM.Identificador && documento.IdCurso != CC.Identificador)
                                            {
                                                List<RequisitoDeDocumento> AutorizacaoRealizacaoEstagioRequisitoDeDocumento = RequisitoDeDocumentoSeed.RequisitosAutorizacaoRealizacaoEstagio(documento);
                                                await ServicoRequisitoDeDocumento.CadastrarAsync(AutorizacaoRealizacaoEstagioRequisitoDeDocumento);
                                            }

                                            if (documento.TituloDocumento == AutorizacaoRealizacaoEstagioADM_ && documento.IdCurso == ADM.Identificador)
                                            {
                                                List<RequisitoDeDocumento> AutorizacaoRealizacaoEstagioRequisitoDeDocumentoADM = RequisitoDeDocumentoSeed.RequisitosAutorizacaoRealizacaoEstagioADM(documento);
                                                await ServicoRequisitoDeDocumento.CadastrarAsync(AutorizacaoRealizacaoEstagioRequisitoDeDocumentoADM);
                                            }

                                            if (documento.TituloDocumento == AutorizacaoRealizacaoEstagioCC_ && documento.IdCurso == CC.Identificador)
                                            {
                                                List<RequisitoDeDocumento> AutorizacaoRealizacaoEstagioRequisitoDeDocumentoCC = RequisitoDeDocumentoSeed.RequisitosAutorizacaoRealizacaoEstagioCC(documento);
                                                await ServicoRequisitoDeDocumento.CadastrarAsync(AutorizacaoRealizacaoEstagioRequisitoDeDocumentoCC);
                                            }

                                            if (documento.TituloDocumento == AtestadoRealizacaoEstagio_)
                                            {
                                                List<RequisitoDeDocumento> AtestadoRealizacaoEstagioRequisitoDeDocumento = RequisitoDeDocumentoSeed.RequisitosAtestadoRealizacaoEstagio(documento);
                                                await ServicoRequisitoDeDocumento.CadastrarAsync(AtestadoRealizacaoEstagioRequisitoDeDocumento);
                                            }

                                            if (documento.TituloDocumento == QuestionarioEstagio_)
                                            {
                                                List<RequisitoDeDocumento> QuestionarioEstagio = RequisitoDeDocumentoSeed.RequisitosQuestionarioEstagio(documento);
                                                await ServicoRequisitoDeDocumento.CadastrarAsync(QuestionarioEstagio);
                                            }
                                        }
                                        catch (Exception) { continue; }
                                    }
                                    await context.SaveChangesAsync();
                                }
                            }

                            #region atividades //ATIVIDADE PARA DOCUMENTOS
                            foreach (Documento doc in DocumentosADS)
                            {
                                await ServicoAtividadeEstagio.CadastrarAtividadeParaEntidade(doc, doc.IdCurso);
                            }
                            foreach(Documento doc in DocumentosADM)
                            {
                                await ServicoAtividadeEstagio.CadastrarAtividadeParaEntidade(doc, doc.IdCurso);
                            }
                            foreach(Documento doc in DocumentosCC)
                            {
                                await ServicoAtividadeEstagio.CadastrarAtividadeParaEntidade(doc, doc.IdCurso);
                            }
                            foreach(Documento doc in DocumentosLET)
                            {
                                await ServicoAtividadeEstagio.CadastrarAtividadeParaEntidade(doc, doc.IdCurso);
                            }
                            foreach(Documento doc in DocumentosPED)
                            {
                                await ServicoAtividadeEstagio.CadastrarAtividadeParaEntidade(doc, doc.IdCurso);
                            }
                            #endregion

                            #region atividades //ATIVIDADE PARA REQUISITOS
                            foreach (Requisito req in RequisitosBase)
                            {
                                await ServicoAtividadeEstagio.CadastrarAtividadeParaEntidade(req);
                            }
                            #endregion

                            #region Regulamentos
                            if (!context.RegulamentacaoCurso.Any())
                            {
                                RegulamentacaoCurso regulamentacaoADS = RegulamentacaoSeed.ADS(ADS);
                                await context.RegulamentacaoCurso.AddAsync(regulamentacaoADS);

                                RegulamentacaoCurso regulamentacaoADM = RegulamentacaoSeed.ADM(ADM);
                                await context.RegulamentacaoCurso.AddAsync(regulamentacaoADM);
                                
                                RegulamentacaoCurso regulamentacaoCC = RegulamentacaoSeed.CC(CC);
                                await context.RegulamentacaoCurso.AddAsync(regulamentacaoCC);
                                
                                RegulamentacaoCurso regulamentacaoLET = RegulamentacaoSeed.LET(LET);
                                await context.RegulamentacaoCurso.AddAsync(regulamentacaoLET);

                                RegulamentacaoCurso regulamentacaoPED = RegulamentacaoSeed.PED(PED);
                                await context.RegulamentacaoCurso.AddAsync(regulamentacaoPED);

                                await context.SaveChangesAsync();
                            }
                            #endregion
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
