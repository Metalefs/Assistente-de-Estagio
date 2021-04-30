import { post, HandleDefault } from '/Scripts/ADE/AsyncHttpMethods.js';
import { PageHandler } from "/Scripts/ADE/PageHandler.js";
export class CalculoHoras {
    #Inicio;
    #Fim;
    #Data;
    #Atividade;
    constructor(containerId, index = 0, inicio = undefined, fim = undefined) {
        this.index = index;
        this.containerId = containerId;
        this.Inicio = inicio;
        this.Fim = fim;
        this.url = "/Principal/RegistroHoras/IncluirRegistroHoraAsync";
    }
    getContainer() {
        return $("." + this.containerId)[this.index];
    }
    getCargaHorariaInput(){
        return $("." + this.containerId + "-input")[this.index];
    }
    setInicio(input) {
        let dataInicio = new Date();
        let horas = this.ObterHoras(input.value);
        dataInicio.setHours(horas);
        let minutos = this.ObterMinutos(input.value);
        dataInicio.setMinutes(minutos);
        if (horas === 0) {
            dataFim.setDate(dataFim.getDate() - 1);
        }
        this.Inicio = dataInicio.getHours() + ":" + dataInicio.getMinutes();
        input.value = dataInicio.getHours() + ":" + dataInicio.getMinutes();
        if (!this.Calcular(input))
            input.value = "";
    }
    setFim(input) {
        let dataFim = new Date();
        let horas = this.ObterHoras(input.value);
        dataFim.setHours(horas);
        let minutos = this.ObterMinutos(input.value);
        dataFim.setMinutes(minutos);
        if (horas === 0) {
            dataFim.setDate(dataFim.getDate() + 1);
        }
        this.Fim = dataFim.getHours() + ":" + dataFim.getMinutes();
        input.value = dataFim.getHours() + ":" + dataFim.getMinutes();
        if (!this.Calcular(input))
            input.value = "";
    }
    setData(input) {
        this.Data = input.value;
    }
    setAtividade(input) {
        this.Atividade = input.value;
    }
    async Add() {
        let data = {
            HoraInicio: this.Inicio,
            HoraFim: this.Fim,
            Data: this.Data,
            Atividade: this.Atividade,
            CargaHoraria: 0
        };
        await post(this.url, data, HandleDefault);
        let ph = new PageHandler();
        await ph.refreshPage("Dashboard");
        await ph.refreshPage("TabelaHoras");
    }
    ObterHoras(value) {
        let AM = value.includes("AM");
        let H = AM ? parseInt(value.replace("AM", "").split(":")[0]) : parseInt(value.replace("PM", "").split(":")[0]) + 12;
        if (!AM && H === 12) {
            H = 0;
        }
        return H;
    }
    ObterMinutos(value) {
        return value.includes("AM") ? parseInt(value.replace("AM", "").split(":")[1]) : parseInt(value.replace("PM", "").split(":")[1]);
    }
    Calcular(input) {
        if (this.#Inicio !== undefined && this.#Fim !== undefined) {
            if (this.#Inicio.getHours() !== 0 && this.#Inicio > this.#Fim) {
                this.AddError("O Horário inicial deve ser menor que o horário final");
                this.#Inicio = undefined;
                this.#Fim = undefined;
                input.value = "";
                return false;
            }
            else if (this.#Fim < this.#Inicio && this.#Inicio.getHours() !== 0) {
                this.AddError("O Horário final deve ser maior que o horário inicial");
                this.#Inicio = undefined;
                this.#Fim = undefined;
                input.value = "";
                return false;
            }
            var msec = this.#Fim - this.#Inicio;
            var mins = Math.floor(msec / 60000);
            var hrs = Math.floor(mins / 60);
            var days = Math.floor(hrs / 24);
            var yrs = Math.floor(days / 365);

            this.getCargaHorariaInput().val = mins;
            mins = mins % 60;

            this.getContainer().textContent = "Carga: " + hrs + " horas, " + mins + " minutos";
        }

        return true;
    }


    AddError(texto) {
        var toastHTML = '<span>' + texto + '!</span>';
        M.toast({ html: toastHTML });
    }
}

export class EditorRegistroHoras {
    constructor(calculadorHoras = null) {
        this.calculo_horas = calculadorHoras;
        this.registroHTML;
    }
    async EditarRegistro(idRegistro) {
        let self = this;
        await $.ajax({
            type: 'get',
            dataType: 'html',
            url: '/Principal/RegistroHoras/ObterRegistro',
            data: { idRegistro }
        }).done(function (data) {
            self.registroHTML = data;
            self.AdicionarHTML(data);
        }).catch((err) => {
            let result = JSON.parse(err);

            if (result.hasOwnProperty("Erro") && this.calculo_horas !== null) {
                this.calculo_horas.AddError(result.Erro);
                console.error(err, result.Erro);
            }
        });
    }
    AdicionarHTML(registroHTML) {
        let edicaoContainer = document.createElement("div");
        edicaoContainer.setAttribute("id", "edicao-container");
        edicaoContainer.innerHTML = registroHTML;

        if ($("#edicao-container").length > 0) {
            $("#edicao-container").html(registroHTML);
        } else {
            let body = document.getElementsByTagName("body")[0];
            body.appendChild(edicaoContainer);
        }
    }
    async RemoverRegistro(idRegistro) {
        let self = this;
        await $.ajax({
            type: 'get',
            dataType: 'html',
            url: '/Principal/RegistroHoras/ObterModalDelecaoRegistro',
            data: { idRegistro }
        }).done(function (data) {
            self.registroHTML = data;
            self.AdicionarHTML(data);
        }).catch((err) => {
            let result = JSON.parse(err);

            if (result.hasOwnProperty("Erro") && this.calculo_horas !== null) {
                this.calculo_horas.AddError(result.Erro);
                console.error(err, result.Erro);
            }
        });
    }
}