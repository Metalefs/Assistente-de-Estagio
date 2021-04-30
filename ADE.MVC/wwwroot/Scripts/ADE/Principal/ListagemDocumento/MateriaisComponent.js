import { get, HandleDefault } from "/Scripts/ADE/AsyncHttpMethods.js";
import { ModalAssembler } from "/Scripts/ADE/ModalAssembler.js";
import { FilterTableWithInput } from "/Scripts/ADE/InputEvents.js";
import { Formulario } from "/Scripts/ADE/Principal/ListagemDocumento/ListagemDocumentos.js";
import { addStyleSheet } from "/Scripts/ADE/PageSetup.js";

export class MateriaisComponent {
    constructor() {
        this.url = "/Principal/ListagemDocumentos/Materiais/";
        this.data = null;
    }

    async Add() {
        if (this.data !== null) {
            if ($("#material-curso").html().length < this.data.length) {
                $("#material-curso").html(this.data);
            }
            return;
        }
        let data = {};
        let html = await get(this.url, data, HandleDefault);
        //let Modal = new ModalAssembler("Materiais", html);
        //let Assembled = Modal.AssembleModal();
        //console.log(Assembled);
        ////document.body.appendChild(Assembled);

        //let range = document.createRange();
        //range.selectNode(document.getElementsByTagName("BODY")[0]);
        //let documentFragment = range.createContextualFragment(Assembled.InnerHTML);
        //document.body.appendChild(documentFragment);
        //this.data = Assembled;
        $("#material-curso").html(html);
        this.data = html;
        this.PostAdd();
    }

    PostAdd() {
        $('.sidenav').sidenav();
        $('.tooltipped').tooltip();
        $('select').formSelect();
        $('.collapsible').collapsible();
        M.Tabs.init($('.tabs'), {});
        $('.dropdown-trigger').dropdown();
        $('.modal').modal();
        addStyleSheet("/Styles/Principal/ModalPreenchimento.css");
        addStyleSheet("/Styles/Principal/SelecaoDocumento.css");
        $('#doc-table').DataTable({
            searching: false,
            fixedHeader: true,
            info: false,
            lengthChange: false,
            language: {
                url: "//cdn.datatables.net/plug-ins/1.10.20/i18n/Portuguese-Brasil.json"
            },
            responsive: true

        }).draw(true).columns.adjust();
        const PreenchimentoDocumento = new Formulario("serial-input-form");
        window.PreenchimentoDocumento = PreenchimentoDocumento;
        FilterTableWithInput("InputFiltro-doc", "doc-table");
    }
}
