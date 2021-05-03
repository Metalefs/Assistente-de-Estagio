let RequisitosObtidos = "";
let RequisitosNaoCadastrados = 0;
let ListaRequisitosNovos = [];
let ListaRequisitosCompativeis = [];
let IgnoreList = [];
const emptyArr = [];
function RequisitoCard(requisito) {
    console.log(this);
    this.listaOpcaoRequisito = [];
    let campoRequisitosNovos = document.getElementById("campo-requisitos-reconhecidos");
    let campoRequisitosCompativeis = document.getElementById("campo-requisitos-compativeis");
    this.requisito = requisito;
    this.CardDivId = "requisito_novo_" + this.requisito.Bookmark;
    this.CardTextId = "requisito_novo_text" + this.requisito.Bookmark;
    let AbrirCriacaoRequisito = () => {
        let Req = new Requisito(this.requisito);
        let ModalBody = document.createElement("div"),
            InfoDiv = document.createElement("div"),
            FormDiv = document.createElement("div"),
            Backdrop = document.createElement("div"),
            SelectDiv = document.createElement("div");

        SelectDiv.setAttribute("id", "select-div");

        let Form = document.createElement("form"),
            FormOpcaoRequisito = document.createElement("div");
        Form.setAttribute("id", "serial-input-form");
        FormOpcaoRequisito.setAttribute("id", "serial-input-form-opcao-requisito");

        let InputOpcaoRequisito = document.createElement("input");
        InputOpcaoRequisito.setAttribute("class", "input-field");
        InputOpcaoRequisito.setAttribute("placeholder", "Nome da opção");
        InputOpcaoRequisito.setAttribute("id", "input-opcao-requisito");
        InputOpcaoRequisito.setAttribute("data-toggle", "tooltip");
        InputOpcaoRequisito.setAttribute("data-placement", "top");
        InputOpcaoRequisito.setAttribute("title", " (Opcional) Insira uma opção para esse requisito");
        InputOpcaoRequisito.addEventListener("keyup", (evt) => { if (evt.keyCode === 13) CriarOpcaoRequisito(); });

        let FormSubmitButton = document.createElement("a"),
            AdicionarOpcaoRequisitoButton = document.createElement("a"),
            RemoverOpcaoRequisitoButton = document.createElement("a"),
            FormDestroyButton = document.createElement("a");
        FormSubmitButton.setAttribute("class", "btn ade-add-btn btn-primary");
        FormSubmitButton.addEventListener("click", () => { SubmitRequisitoForm(); });
        FormSubmitButton.innerText = "Cadastrar";
        AdicionarOpcaoRequisitoButton.setAttribute("class", "btn btn-primary");
        AdicionarOpcaoRequisitoButton.addEventListener("click", () => { CriarOpcaoRequisito(); });
        AdicionarOpcaoRequisitoButton.innerText = "Adicionar opção ao requisito";
        RemoverOpcaoRequisitoButton.setAttribute("class", "btn btn-warning");
        RemoverOpcaoRequisitoButton.addEventListener("click", () => { RemoverOpcoesSelect(); });
        RemoverOpcaoRequisitoButton.innerText = "Remover opção do requisito";
        FormDestroyButton.setAttribute("class", "btn btn-danger");
        FormDestroyButton.addEventListener("click", () => { DestroyRequisitoForm(); });
        FormDestroyButton.innerText = "Cancelar";

        ModalBody.setAttribute("class", "modal-body card req_card window_body");
        Backdrop.setAttribute("class", "modal-backdrop");
        FormDiv.setAttribute("class", "md-form dl-horizontal");

        let InfoText = document.createElement("h5");
        InfoText.innerHTML = "<p>Insira os dados do requisito abaixo</p>";

        let FormInputs = { inputs: "" };
        Object.keys(Req).forEach(req => { createInputs(FormInputs, req); });

        Form.innerHTML = FormInputs.inputs;
        Form.appendChild(FormSubmitButton);
        Form.appendChild(AdicionarOpcaoRequisitoButton);
        Form.appendChild(FormDestroyButton);
        FormOpcaoRequisito.appendChild(InputOpcaoRequisito);
        FormOpcaoRequisito.appendChild(SelectDiv);
        FormOpcaoRequisito.appendChild(RemoverOpcaoRequisitoButton);
        FormDiv.appendChild(Form);

        InfoDiv.appendChild(InfoText);
        ModalBody.appendChild(InfoDiv);
        ModalBody.appendChild(FormDiv);
        ModalBody.appendChild(FormOpcaoRequisito);
        Backdrop.appendChild(ModalBody);
        document.getElementById("create_requisito_div").appendChild(Backdrop);

        $('select').formSelect();
    };

    let CriarOpcaoRequisito = () => {
        let opcao = document.getElementById("input-opcao-requisito").value;
        let opcaoRequisito = new OpcaoRequisito(this.requisito.Bookmark, opcao);
        this.listaOpcaoRequisito.push(opcaoRequisito);
        console.log(this.listaOpcaoRequisito);
        let selectOpcoes = CriarSelectOpcaoRequisito(this.listaOpcaoRequisito);
        document.getElementById("select-div").innerHTML = selectOpcoes;
    };

    CriarSelectOpcaoRequisito = (lista) => {
        let SelectOpcaoRequisito = `<select class="form-control" name="opcao-requisito" id="select-opcao-requisito" title="As opção aparecerão desta forma">`;
        for (let i = 0; i < lista.length; i++) {
            SelectOpcaoRequisito += `
                <option value="`+ lista[i].valor + `">` + lista[i].valor + `</option>
            `;
        }
        SelectOpcaoRequisito += "</select>";
        return SelectOpcaoRequisito;
    };

    RemoverOpcoesSelect = () => {
        this.listaOpcaoRequisito = emptyArr;
        let selectOpcoes = CriarSelectOpcaoRequisito(this.listaOpcaoRequisito);
        document.getElementById("select-div").innerHTML = selectOpcoes;
    };

    let SubmitRequisitoForm = () => {
        let NovoRequisitoJson = $("#serial-input-form").serializeArray();
        console.log(NovoRequisitoJson);
        let NovoRequisito = new Requisito(NovoRequisitoJson[3].value, NovoRequisitoJson[1].value, NovoRequisitoJson[2].value, NovoRequisitoJson[0].value, NovoRequisitoJson[4].value);
        NovoRequisito.CriarRequisito(this.listaOpcaoRequisito);
        DestroyRequisitoForm();
        this.ChangeCardBgColorClass("bg-success");
        this.ChangeCardTitle("Sucesso ao criar requisito");
    };

    let DestroyRequisitoForm = () => {
        $("#create_requisito_div").empty();
    };

    let createInputs = (list, req) => {
        let value = "";
        if (req === "possuiOpcoes")
            return;
        else if (req === "Bookmark") {
            value = this.requisito.Bookmark;
        }
        else if (req === "ElementoHTMLRequisito") {
            list.inputs += CriarSelectElementoHTMLRequisito(req);
        }
        else if (req === "TipoElementoHTMLRequisito") {
            list.inputs += CriarSelectTipoElementoHTMLRequisito(req);
        }
        else if (req === "Grupo") {
            list.inputs += CriarSelectGrupoRequisito(req);
        }
        else {
            list.inputs += `<input type="text" placeholder="` + req + `" value="` + value + `" class="" id="` + req + `" name=` + req + ` />`;
        }
    };

    let CriarSelectElementoHTMLRequisito = (req) => {
        return `<label for="` + req + `" > Elemento de entrada de dados </label ><select title="` + req + ` do requisito"  class="" id="` + req + `" name=` + req + ` />
                                            <option value="input"> Texto pequeno
                                            </option>
                                            <option value="textarea"> Texto grande
                                            </option>
                                            <option value="select"> Múltipla escolha
                                            </option>
                                        </select>`;
    };

    let CriarSelectTipoElementoHTMLRequisito = (req) => {
        return `<label for="` + req + `" > Formato de entrada de dados </label ><select title="` + req + ` do requisito"  class="" id="` + req + `" name=` + req + ` />
                                            <option value="text"> Texto
                                            </option>
                                            <option value="date"> Data
                                            </option>
                                            <option value="number"> Numero
                                            </option>
                                            <option value="email"> Email
                                            </option>
                                        </select>`;
    };

    let CriarSelectGrupoRequisito = (req) => {
        return `<label for="` + req + `" > Grupo a qual o requisito pertence </label ><select title="` + req + ` do requisito"  class="" id="` + req + `" name=` + req + ` />
                                            <option value="1"> Aluno
                                            </option>
                                            <option value="2"> Empresa
                                            </option>
                                            <option value="3"> Faculdade
                                            </option>
                                        </select>`;
    };

    let Deletar = (idDiv) => {
        ListaRequisitosNovos = ListaRequisitosNovos.filter(item => item !== this.requisito);
        IgnoreList.push(this.requisito);
        this.ChangeCardTitle("Esse cartão será ignorado. Para inclui-lo clique para cadastrar.");
        this.ChangeCardBgColorClass("bg-danger");
    };

    this.CreateNovo = function () {
        RequisitosNaoCadastrados++;
        let CardDiv = document.createElement("div"),
            CardBody = document.createElement("div"),
            CardText = document.createElement("div"),
            DeleteButton = document.createElement("div"),
            NomeRequisito = document.createElement("span");

        DeleteButton.setAttribute("class", "card_delete_button card-header-pills");
        DeleteButton.setAttribute("title", "Deletar");
        DeleteButton.innerHTML = "<h5 class='deletebtn'>x</h5>";
        DeleteButton.addEventListener('click', function () { Deletar(this.CardDivId) });

        CardDiv.setAttribute("class", "card requisito bg-primary");
        CardDiv.setAttribute("id", this.CardDivId);
        CardBody.setAttribute("class", "card-body");
        CardText.setAttribute("class", "card-text text-light");
        CardText.setAttribute("id", this.CardTextId);
        CardText.setAttribute("data-toggle", "tooltip");
        CardText.setAttribute("data-placement", "right");
        CardText.setAttribute("title", "Clique para cadastrar esse requisito");
        NomeRequisito.innerHTML = formatarCamposCard("Bookmark", requisito.Bookmark);

        CardText.addEventListener('click', () => { AbrirCriacaoRequisito(); });

        CardDiv.appendChild(CardBody);
        CardDiv.appendChild(DeleteButton);
        CardText.appendChild(NomeRequisito);
        CardBody.appendChild(CardText);
        campoRequisitosNovos.appendChild(CardDiv);
    };

    this.ChangeCardTitle = (text) => {
        document.getElementById(this.CardTextId).setAttribute("data-original-title", text);
    };

    this.ChangeCardBgColorClass = (bg_color_class) => {
        setTimeout(() => {
            document.getElementById(this.CardDivId).className = "card requisito " + bg_color_class;
        }, 1000);
    };

    this.CreateCompatible = function () {
        let CardDiv = document.createElement("div"),
            CardBody = document.createElement("div"),
            CardText = document.createElement("div");

        let NomeRequisito = document.createElement("div"),
            ElementoHTMLRequisito = document.createElement("div"),
            TipoElementoHTMLRequisito = document.createElement("div"),
            Bookmark = document.createElement("div"),
            Descricao = document.createElement("div");

        NomeRequisito.innerHTML = formatarCamposCard("NomeRequisito", requisito.NomeRequisito);
        ElementoHTMLRequisito.innerHTML = formatarCamposCard("Elemento", requisito.ElementoHTMLRequisito);
        TipoElementoHTMLRequisito.innerHTML = formatarCamposCard("TipoElemento", requisito.TipoElementoHTMLRequisito);
        Bookmark.innerHTML = formatarCamposCard("Bookmark", requisito.Bookmark);
        Descricao.innerHTML = formatarCamposCard("Descricao", requisito.Descricao);

        CardDiv.setAttribute("class", "card requisito bg-warning");
        CardDiv.setAttribute("data-toggle", "tooltip");
        CardDiv.setAttribute("data-placement", "top");
        CardDiv.setAttribute("title", "Esse requisito será incluido automaticamente");
        CardBody.setAttribute("class", "card-body");
        CardText.setAttribute("class", "card-text");

        CardText.appendChild(NomeRequisito);
        CardText.appendChild(ElementoHTMLRequisito);
        CardText.appendChild(TipoElementoHTMLRequisito);
        CardText.appendChild(Bookmark);
        CardText.appendChild(Descricao);

        CardBody.appendChild(CardText);
        CardDiv.appendChild(CardBody);
        campoRequisitosCompativeis.appendChild(CardDiv);
    };

    function formatarCamposCard(titulo, texto) {
        return "<b>" + titulo + "</b>: " + texto + " <br>";
    };
}

class OpcaoRequisito {
    constructor(nome,valor) {
        this.nome = nome;
        this.valor = valor;
    }
}

class Requisito {
    constructor(Bookmark = "", ElementoHTMLRequisito = "", MascaraEntrada = "", Obrigatorio = "", TipoElementoRequisito = "", NomeRequisito = "", Descricao = "", Grupo = "") {
        this.NomeRequisito = NomeRequisito;
        this.ElementoHTMLRequisito = ElementoHTMLRequisito;
        this.TipoElementoHTMLRequisito = TipoElementoRequisito;
        this.Bookmark = Bookmark;
        this.Descricao = Descricao;
        this.MascaraEntrada = MascaraEntrada;
        this.Obrigatorio = Obrigatorio;
        this.Grupo = Grupo;
    }

    ChecarOpcoes(listaOpcoes) {
        if (listaOpcoes.length > 0)
            return true;
        else
            return false;
    }

    CriarRequisito(listaOpcoes) {
        console.log(this);
        let possuiOpcoes = this.ChecarOpcoes(listaOpcoes);
        $.post("/Administracao/GerenciamentoRequisito/Criar", this)
        .done((data) => {
            console.log(data);
            if (possuiOpcoes) {
                listaOpcoes.forEach(opcao => {
                    console.log(opcao);
                    this.CadastrarOpcoesRequisito(opcao);
                });
            }
        });
    }

    CadastrarOpcoesRequisito(opcao) {
        $.post("/Administracao/GerenciamentoRequisito/CriarOpcoesRequisito", opcao)
            .done(function (data) {
                console.log(data);
        });
    }
}

function AdicionarRequisitosReconhecidos(jsonRequisitos) {

    let RequisitosNovos = true;
    let RequisitosCompativeis = true;
    jsonRequisitos = jsonRequisitos === undefined ? null : jsonRequisitos;
    (function AdicionarRequisitosNovos() {
        try {
            if (jsonRequisitos !== null && jsonRequisitos.hasOwnProperty("RequisitosNovos"))
                jsonRequisitos.RequisitosNovos.forEach(x => createRequisitoNovoCard(x));
            else
                RequisitosNovos = false;
        } catch (ex) {
            console.log(ex);
        }
    })();

    (function AdicionarRequisitosCompativeis() {
        try {
            if (jsonRequisitos !== null && jsonRequisitos.hasOwnProperty("RequisitosCompativeis"))
                jsonRequisitos.RequisitosCompativeis.forEach(x => createRequisitoCompativelCard(x));
            else
                RequisitosCompativeis = false;
        } catch (ex) {
            console.log(ex);
        }
    })();

    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    function createRequisitoNovoCard(x) {
        let requisito = new Requisito(x);
        let Card = new RequisitoCard(requisito);
        Card.CreateNovo();
        ListaRequisitosNovos.push(x);
    }

    function createRequisitoCompativelCard(x) {
        let requisito = new Requisito(x.Bookmark, x.ElementoHTMLRequisito, x.TipoElementoHTMLRequisito, x.NomeRequisito, x.Descricao);
        let Card = new RequisitoCard(requisito);
        Card.CreateCompatible();
        ListaRequisitosCompativeis.push(requisito);
    }

    function ShowRequisitos(NovoFound, CompativelFound) {
        let campoRequisitos = $("#hidden-campos-requisito");
        let campoRequisitosCompativeis = $("#campo-requisitos-compativeis");
        let Mensagem = document.createElement("div");
        Mensagem.setAttribute("class","card-body");
        if (!NovoFound) {
            Mensagem.innerHTML += "<p class='text-danger card-text'>Não foram encontrados requisitos novos no documento enviado</p>";
            campoRequisitos.append(Mensagem);
        }
        else if (!CompativelFound) {
            Mensagem.innerHTML += "<br><p class='text-danger card-text'>Não foram encontrados requisitos compativeis com o sistema no documento enviado</p>";
            campoRequisitosCompativeis.append(Mensagem);
        }
        campoRequisitos.removeClass("d-none");
    }

    ShowRequisitos(RequisitosNovos, RequisitosCompativeis);
}

function ToggleLoadingAnimation() {
    $("#loading-animation").toggleClass("collapse");
}

function FileUploaded() {
    ToggleLoadingAnimation();
    var FileField = $("#CampoDocX").get(0);
    var TextoDocumentoPreview = $("#campo-texto-documento");
    var TextoDocumento = $("#Texto");
    var File = FileField.files;
    var formData = new FormData();
    for (var i = 0; i < File.length; i++) {
        formData.append(File[i].name, File[i]);
        console.log(File[i], formData);
    }
    $.ajax({
        type: "POST",
        url: "/Administracao/GerenciamentoDocumento/ChecarDocumento",
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            console.log(data);
            RequisitosObtidos = JSON.parse(data);
            AdicionarRequisitosReconhecidos(JSON.parse(RequisitosObtidos).ComparacaoRequisitos);
            TextoDocumento.html(JSON.parse(RequisitosObtidos).TextoDocumento);
            TextoDocumentoPreview.html('<div class="card-header">Conteúdo encontrado no documento</div>'+JSON.parse(RequisitosObtidos).TextoDocumentoHTML);
            ToggleLoadingAnimation();
        }
    });
}