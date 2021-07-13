
import { get, HandleDefault } from "/Scripts/ADE/AsyncHttpMethods.js";
import { ModalAssembler } from "/Scripts/ADE/ModalAssembler.js";
import { CalculoHoras } from '/Scripts/ADE/Principal/RegistroHoras/TabelaHoras.js';

export class RegistroHorasComponent {
    constructor() {
        this.url = "/Principal/RegistroHoras/ObterPagina/";
        this.paginaInicial = "_RegistroHoras";
        this.container = document.createElement("div");
        this.container.id = "rh1id";
    }

    async Add() {
        let data = { view: "_RegistroHoras" };
        let html = await get(this.url, data, HandleDefault);
        let Modal = new ModalAssembler("Registrar Horas", html);
        let Assembled = Modal.AssembleModal();
        this.container.appendChild(Assembled);
        console.log(this.container);

        document.body.appendChild(this.container);
        await this.PostAdd();
    }

    async PostAdd() {
        let calculo_horas = new CalculoHoras("calculo-carga-horaria");
        window.calculo_horas = calculo_horas;
        const moduleSpecifier = '/Scripts/ADE/MaterializeDates.js';
        const module = await import(moduleSpecifier);
        (async () => {
            module.CreateTimePicker();
        })();
        $("#rh1id").find("script").each(function () {
            eval($(this).text());
        });
    }

}
