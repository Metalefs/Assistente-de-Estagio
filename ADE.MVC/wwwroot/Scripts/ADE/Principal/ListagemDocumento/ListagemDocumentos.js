import { PageHandler } from "/Scripts/ADE/PageHandler.js";
import { refresh_mask } from '/Scripts/Externo/jquery.mask.js';
export class Formulario {
    #DocumentoAberto = undefined;
    constructor(id) {
        this.id = id;
        this.Changed = false;
        this.DownloadClicked = false;
        this.ImpressaoClicked = false;
        this.AcceptError = false;
        this.Error = false;
        this.ErrorDiv;
        this.SuccessDiv;
        this.Inputs;
        this.FormData = new Array();
        this.DownloadURL = "/Principal/ListagemDocumentos/DownloadDocumento/";
        let self = this;
        $(window).bind("beforeunload", function () {
            if (self.FormularioAlterado()) {
                alert("Os dados não foram salvos!");
            }
        });

        window.addEventListener('click', (event) => {
            if (event.target === document.getElementById("preeenchimento-modal-backdrop-close")) {
               self.FecharBackground();
            }
        });
    }
    SetChanged() {
        this.Changed = true;
    }
    Download(IdDocumento) {
        this.DownloadClicked = true;

        this.Inputs = $("#" + this.id)[0].elements;
        for (var i = 0; i < this.Inputs.length; i++) {
            this.FormData[i] = this.Inputs[i].value;
        }

        if (this.CheckFormError()) {
            this.DownloadDocument(IdDocumento);
        } else {
            this.AddError("<p>Para continuar, clique em Baixar novamente.</p>","bg-info");
        }

        this.Changed = false;
    }

    Print(IdDocumento,Mimetype) { //TODO select MIMETYPE
        this.ImpressaoClicked = true;
        this.GetFile(IdDocumento, Mimetype);
    }

    CheckFormError() {
        for (let i = 0; i < this.Inputs.length; i++) {
            if (this.InputIsNull(this.Inputs[i])) {
                this.Error = true;
                this.Inputs[i].style.borderTop = "1px solid rgba(255,50,50,.5)";
            } else {
                this.Inputs[i].style.borderTop = "initial";
            }
        }
        if (this.IsUnacceptedError()) {
            this.AddError("<span>Existem campos vazios, corrija-os se quiser.</span>","bg-warning ade-fade");
            this.AcceptError = true;
            return false;
        } else {
            return true;
        }
    }

    InputIsNull(input) {
        return input.value === undefined || input.value === "" || input.value === null || input.value.length === 0;
    }

    IsUnacceptedError() {
        return this.Error === true && this.AcceptError === false;
    }

    AddError(errortext, classe) {
        this.ErrorDiv = document.getElementById("errors");
        var Text = document.createElement("p");
        Text.innerHTML = errortext;
        try {
            this.ErrorDiv.appendChild(Text);
        }
        catch (ex) {}
        M.toast({ html: errortext, classes: classe+" rounded text-white" });
    }

    IsNotSpam() {
        return this.Changed === true;
    }

    async DownloadDocument(IdDocumento) {
        if (this.ValidDownloadAttempt()) {
            var DadosDePreenchimento = JSON.stringify($("#" + this.id).serializeArray());
            if (this.AcceptError === true)
                this.AddError("<span>Gerando documento com campos vazios.</span>","bg-warning");
            else
                this.AddSuccessMessage();

            await this.GetFile(DadosDePreenchimento, IdDocumento, "docx");
        }
        else {
            this.AddError("<hr><p>Tente alterar o formulário para provar que não é um Robô</p>","bg-warning ade-fade");
        }
    }

    async GetFile(DadosPreenchimento, IdDocumento, tipo) {
        let dataType;
        if (tipo === "docx")
            dataType = "application/msword";

        let data = { dadosAluno: DadosPreenchimento, IdDocumento, MimeType: tipo };
        console.log(data)
        $.get({
            url: this.DownloadURL,
            data: data,
            success: function (data) {
                console.log({ success: data });
                var response = JSON.parse(data);
                if (response.erro !== undefined) {
                    M.toast({ html: "<span class='bg-danger' style='color:white !important'>" + response.erro + "</span>", classes: 'bg-danger rounded' });
                } else {
                    window.location = '/Principal/ListagemDocumentos/Download?fileGuid=' + response.FileGuid
                        + '&filename=' + response.FileName + '&MimeType=' + dataType;
                }
            },
            error: function (data) {
                console.log({ error: data });
            }
        });
        let ph = new PageHandler();
        await ph.refreshPage("Atividades",false);
        await ph.refreshPage("Dados",false);
        await ph.refreshPage("Dashboard",false);
    }

    AddSuccessMessage() {
        this.SuccessDiv = document.getElementById("success");
        var Text = document.createElement("p");
        Text.innerHTML = "<span class='bg-success' style='color:white !important'>Seu download iniciará em alguns instantes.</span>";
        try {
            this.SuccessDiv.appendChild(Text);
        }
        catch (ex) {}
        M.toast({ html: Text.innerHTML, classes: 'bg-success rounded' });
    }

    ValidDownloadAttempt() {
        return this.AcceptError === true || this.Error === false;
    }

    ObterRequisitosDeDocumentoPreenchidos = async (idDocumento) => {
        let self = this;
        await $.ajax({
            type: 'get',
            dataType: 'html',
            url: '/Principal/ListagemDocumentos/ObterRequisitosDeDocumento',
            data: { idDocumento }
        }).done(function (data) {
            $("#preenchimento-container").html(data);
            self.DocumentoAberto = idDocumento;
        }).catch((err) => {
            console.error(err);
        });
        refresh_mask();
    };

    AbrirPaginaPrenchimento = async (idDocumento) => {
        if (this.DocumentoAberto !== idDocumento) {
            this.ObterRequisitosDeDocumentoPreenchidos(idDocumento);
            
        }
        if (this.DocumentoAberto !== undefined && this.DocumentoAberto > 0) {
            if (this.FormularioAlterado()) {
                alert("Os dados do último fomulário não foram salvos!");
            }
            this.AbrirBackground(idDocumento); 
        }
    };

    CarregarPDFPaginaPreenchimento = (idDocumento) => {
        var pdfjsLib = window['pdfjs-dist/build/pdf'];
        pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
        var loadingTask = pdfjsLib.getDocument("/Principal/ListagemDocumentos/PDF/?" + idDocumento);
        loadingTask.promise.then(function (pdf) {
            var ctx = document.createElement('canvas').getContext('2d', { alpha: false });
            console.log('PDF loaded');
            var pageNumber = 1;
            pdf.getPage(pageNumber).then(function (page) {
                console.log('Page loaded');
                var scale = 1.5;
                var viewport = page.getViewport(scale);
                var canvas = document.getElementById('pdf-canvas');
                var context = canvas.getContext('2d');
                canvas.height = viewport.height;
                canvas.width = viewport.width;
                var renderContext = {
                    canvasContext: context,
                    viewport: viewport
                };
                var renderTask = page.render(renderContext);
                renderTask.then(function () {
                    console.log('Page rendered');
                });
                page.getTextContent({ normalizeWhitespace: true }).then(function (textContent) {
                    textContent.items.forEach(function (textItem) {
                        var tx = pdfjsLib.Util.transform(
                            pdfjsLib.Util.transform(viewport.transform, textItem.transform),
                            [1, 0, 0, -1, 0, 0]
                        );
                        var style = textContent.styles[textItem.fontName];
                        var fontSize = Math.sqrt((tx[2] * tx[2]) + (tx[3] * tx[3]));
                        if (style.ascent) {
                            tx[5] -= fontSize * style.ascent;
                        } else if (style.descent) {
                            tx[5] -= fontSize * (1 + style.descent);
                        } else {
                            tx[5] -= fontSize / 2;
                        }
                        if (textItem.width > 0) {
                            ctx.font = tx[0] + 'px ' + style.fontFamily;

                            var width = ctx.measureText(textItem.str).width;

                            if (width > 0) {
                                tx[0] = (textItem.width * viewport.scale) / width;
                            }
                        }
                    });
                });
            });
        }, function (reason) {
            console.error(reason);
            openDocumentWindow('preeenchimento-modal-backdrop-close');
            $("redundancia-previa-documento").removeClass("collapse");
        });
        this.FecharAnimacao();
    }

    AbrirBackground(idDocumento) {
        $("#modal-documento-" + idDocumento).toggleClass("show");
        $("#modal-documento-" + idDocumento).show();
    }

    FecharBackground() {
        document.getElementById("preeenchimento-modal-backdrop-close").style.display = "none";
        open = false;
        if (this.FormularioAlterado()) {
            alert("Os dados não foram salvos!");
        }
    }

    AbrirAnimacao = () => {
        $("#loading-div").show();
    };

    FecharAnimacao = () => {
        $("#loading-div").removeClass("show");
        $("#loading-div").hide();
    };

    FormularioAlterado = function () {
        return this.Changed === true && this.DownloadClicked !== true && this.ImpressaoClicked !== true;
    };
}

