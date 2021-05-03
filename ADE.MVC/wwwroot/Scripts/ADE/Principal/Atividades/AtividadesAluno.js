import { post } from "/Scripts/ADE/AsyncHttpMethods.js";

export class AtividadeAluno {
    constructor(Identificador, Titulo, Descricao, Tipo, Data, IdContainer) {
        this.Identificador = Identificador;
        this.Titulo = Titulo;
        this.Descricao = Descricao;
        this.Tipo = Tipo;
        this.Data = Data;
        this.Container = $("#"+IdContainer);
    }
    async setTitle(value) {
        this.Titulo = value;
        $("#tit-au-" + this.Identificador).html(this.Titulo);
        await this.AtualizarAtividades();
    }
    async setDesc(value) {
        this.Descricao=value;
        await this.AtualizarAtividades();
    }
    async setType(value) {
        this.Tipo=value;
        await this.AtualizarAtividades();
    }
    async setDate(value) {
        this.Data = value;
        $("#date-au-" + this.Identificador).html(this.Data);
        await this.AtualizarAtividades();
    }
    async ConcluirAtividade() {
        let id = this.Identificador;
        post("/Principal/Atividades/ConcluirAtividade", { IdAtividade: id }, function (data,url) {
            try {
                data = JSON.parse(data);
                var toastHTML = '<span>' + data.retorno + '</span>';
                M.toast({ html: toastHTML });
            } catch (ex) {
                M.toast({ html: data.retorno });
            }
        });
    }
    async AtualizarAtividades() {
        let self = {
            Identificador: this.Identificador,
            Titulo: this.Titulo,
            Descricao: this.Descricao,
            Tipo: this.Tipo,
            Data: this.Data,
            IdCurso:0
        };
        post("/Principal/Atividades/AtualizarAtividade", self, function (data, url) {
            try {
                data = JSON.parse(data);
                var toastHTML = '<span>' + data.retorno + '</span>';
                M.toast({ html: toastHTML });
            } catch (ex) {
                M.toast({ html: data.retorno });
            }
        });
    }
}