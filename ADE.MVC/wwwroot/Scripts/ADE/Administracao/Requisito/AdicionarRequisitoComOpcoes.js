import { ModalAssembler } from "/Scripts/ADE/ModalAssembler.js";

class OpcaoRequisito {
    constructor(nome, valor) {
        this.nome = nome;
        this.valor = valor;
    }
}

export class Requisito {
    constructor(Bookmark = "", ElementoHTMLRequisito = "", MascaraEntrada = "", Obrigatorio = "",TipoElementoRequisito = "", NomeRequisito = "", Descricao = "", Grupo = "") {
        this.NomeRequisito = NomeRequisito;
        this.ElementoHTMLRequisito = ElementoHTMLRequisito;
        this.TipoElementoHTMLRequisito = TipoElementoRequisito;
        this.Bookmark = Bookmark;
        this.Descricao = Descricao;
        this.MascaraEntrada = MascaraEntrada;
        this.Obrigatorio = Obrigatorio;
        this.Grupo = Grupo;
        this.listaOpcaoRequisito = [];
        this.OpcaoRequisitoSelectDiv = document.getElementById("select-div");
    }

    HasBookmark() { return this.Bookmark !== ""; }

    CriarOpcaoRequisito() {
        let opcao = document.getElementById('input-opcao-requisito').value;
        if (this.ElementoHTMLRequisito !== "select") {
            this.ReturnValidationMessageCreateOpcaoRequisitoSemSerSelect();
            return;
        }
        if (!this.HasBookmark()) {
            this.ReturnValidationMessageCreateOpcaoRequisitoSemBookmark();
            return;
        }
        if (opcao !== "") {
            let opcaoRequisito = new OpcaoRequisito(this.Bookmark, opcao);
            this.listaOpcaoRequisito.push(opcaoRequisito);
            console.log(this.listaOpcaoRequisito);
            let selectOpcoes = this.CriarSelectOpcaoRequisito(this.listaOpcaoRequisito);
            this.OpcaoRequisitoSelectDiv.innerHTML = selectOpcoes;
            opcao = "";
        } else {
            this.ReturnValidationMessageCreateOpcaoRequisitoSemValor();
        }
    }

    ReturnValidationMessageCreateOpcaoRequisitoSemSerSelect() {
        this.RemoverOpcoesSelect();
        this.OpcaoRequisitoSelectDiv.innerHTML = "<p class='text-danger'>O requisito precisa ser do tipo 'Lista de opções' para se adicionar uma opção!</p>";
    }

    ReturnValidationMessageCreateOpcaoRequisitoSemBookmark(){
        this.RemoverOpcoesSelect();
        this.OpcaoRequisitoSelectDiv.innerHTML = "<p class='text-danger'>O campo 'Bookmark' deve ter um valor antes de se adicionar uma opção!</p>"; 
    }

    ReturnValidationMessageCreateOpcaoRequisitoSemValor(){
        this.OpcaoRequisitoSelectDiv.innerHTML = "<p class='text-danger'>Não é possivel adicionar uma opção sem valor</p>";
    }

    CriarSelectOpcaoRequisito = (lista) => {
        let SelectOpcaoRequisito = `<select class="" name="opcao-requisito" id="select-opcao-requisito" title="As opções aparecerão desta forma">`;
        for (let i = 0; i < lista.length; i++) {
            SelectOpcaoRequisito += `
                <option value="`+ lista[i].valor + `">` + lista[i].valor + `</option>
            `;
        }
        SelectOpcaoRequisito += "</select>";
        return SelectOpcaoRequisito;
    };

    RemoverOpcoesSelect = () => {
        this.listaOpcaoRequisito = [];
        let selectOpcoes = this.CriarSelectOpcaoRequisito(this.listaOpcaoRequisito);
        this.OpcaoRequisitoSelectDiv.innerHTML = selectOpcoes;
    };

    PossuiOpcoes() {
        if (this.listaOpcaoRequisito.length > 0 && this.ElementoHTMLRequisito === "select")
            return true;
        else
            return false;
    }

    CriarRequisito() {
        console.log(this);
        let PostValue = {
            NomeRequisito: this.NomeRequisito,
            ElementoHTMLRequisito: this.ElementoHTMLRequisito,
            TipoElementoHTMLRequisito: this.TipoElementoHTMLRequisito,
            Bookmark: this.Bookmark,
            Descricao: this.Descricao,
            Grupo: this.Grupo
        };
        $.post("/Administracao/GerenciamentoRequisito/Criar", PostValue)
        .done((data) => {
            console.log(data);
            let html = new ModalAssembler("Retorno", data);

            $("body").append(html.AssembleModal());

            if (this.PossuiOpcoes()) {
                this.listaOpcaoRequisito.forEach(opcao => {
                    console.log(opcao);
                    this.CadastrarOpcoesRequisito(opcao);
                });
            }
        });
    }

    CadastrarOpcoesRequisito(opcao) {
        $.post("/Administracao/GerenciamentoRequisito/CriarOpcoesRequisito", opcao)
        .done(function (data) {
            data = JSON.parse(data);
            console.log(data);
            let resultado = document.createElement("p");
            resultado.innerText = data.Status;
            $("body").append(resultado);
        });
    }

    CadastrarOpcoesRequisitoExisitente() {
        this.listaOpcaoRequisito.forEach((opcao) => {
            $.post("/Administracao/GerenciamentoRequisito/CriarOpcoesRequisito", opcao)
            .done(function (data) {
                data = JSON.parse(data);
                console.log(data);
                let resultado = document.createElement("p");
                resultado.innerText = data.Status;
                $("body").append(resultado);
            });
        });
    }
}
requisito = new Requisito();
requisito.Bookmark = $("#Bookmark").val();
requisito.ElementoHTMLRequisito = $("#ElementoHTMLRequisito").val();