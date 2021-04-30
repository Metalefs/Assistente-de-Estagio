import { post } from "/Scripts/ADE/AsyncHttpMethods.js";
export class PerfilComponent {
    constructor() {
        this.controller = "/Amizade";
        this.urls = {
            CadastrarContato: this.controller + "/" + "CadastrarRelacionamentoJson",
            AlterarContato: this.controller + "/" + "AlterarRelacionamentoJson",
            RemoverContato: this.controller + "/" + "RemoverRelacionamentoJson",
            Toggle: this.controller + "/" + "AdicionarOuRemoverRelacionamentoJson"
        };
    }
    AdicionarOuRemoverRelacionamento(userId, iconId) {
        this.iconId = iconId;
        post(this.urls.Toggle, {idUsuario : userId}, this.handle);
    }
    handle(result, url) {
        try {
            if (result.hasOwnProperty("sucesso")) {
                let toastHTML = '<span>' + result.sucesso + '</span>';
                M.toast({ html: toastHTML, classes: 'bg-success' });
                if (result.tipo === "remocao") {
                    document.getElementByid(this.iconId).textContent = "person_add";
                    document.getElementByid(this.iconId).setAttribute("data-tooltip","Adicionar como amigo");
                }
                else if (result.tipo === "adicao") {
                    document.getElementByid(this.iconId).textContent = "remove_circle";
                    document.getElementByid(this.iconId).setAttribute("data-tooltip","Remover amizade");
                }
            }
            if (result.hasOwnProperty("erro")) {
                let toastHTML = '<span>' + result.erro + '</span>';
                M.toast({ html: toastHTML, classes: 'bg-danger' });
            }
        } catch (ex) {
            console.log(url, result);
        }
        return;
    }
}