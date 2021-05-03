import { get, HandleDefault } from "/Scripts/ADE/AsyncHttpMethods.js";
import { ModalAssembler } from "/Scripts/ADE/ModalAssembler.js";
import { Atividades } from "/Scripts/ADE/PageSetup.js";

export class AtividadesComponent {
    constructor() {
        this.url = "/Principal/Atividades/Index/";
        this.paginaInicial = "_Atividades";
        this.container = document.createElement("div");
        this.container.id = "at1id";
    }

    async Add() {
        let data = { view: "_Index", Partial: true };
        let html = await get(this.url, data, HandleDefault);
        let Modal = new ModalAssembler("Atividades", html);
        let Assembled = Modal.AssembleModal();
        this.container.appendChild(Assembled);
        console.log(this.container);

        document.body.appendChild(this.container);
        await this.PostAdd();
    }

    async PostAdd() {
        Atividades();
    }

}
