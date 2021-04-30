export class ModalAssembler {
    constructor(title, body, footer = "Assistente de Estágio") {
        this.id = "Modal-" + '_' + (Date.now().toString(36) + Math.random().toString(36).substr(2, 5)).toUpperCase();    
        this.background = this.ModalBackground();
        this.title = this.ModalHeader(title);
        this.body = this.ModalBody(body);
        this.footer = this.ModalFooter(footer);
    }
    ModalBackground() {
        let div = document.createElement("div"); div.setAttribute("id", this.id); div.setAttribute("class", "modal-backdrop collapse modal show alert alert-dismissible");
        div.addEventListener('click', div.remove);
        return div;
    }
    rootClick() {
        //document.getElementById(this.id).remove();
    }
    
    modalClick(e) { e.preventDefault(); e.stopPropagation(); e.stopImmediatePropagation(); return false; };
    ModalHeader(title) {
        let self = this;
        let Ptitle = document.createElement("p");
        Ptitle.innerHTML = title;

        let close_btn = document.createElement("a");
        close_btn.setAttribute("class", "tooltipped close");
        close_btn.setAttribute("data-tooltip", "Fechar");
        close_btn.setAttribute("data-placement", "bottom");

        let close_icon = document.createElement("i");
        close_icon.setAttribute("class", "material-icons");
        close_icon.innerText = "close";

        close_btn.appendChild(close_icon);
        close_btn.addEventListener("click", () => { $("#"+ self.id).remove();});

        Ptitle.appendChild(close_btn);
        let div = document.createElement("div"); div.setAttribute("class", "window-title"); div.appendChild(Ptitle); return div;
    }
    ModalBody(body) {
        let bd = document.createElement("div");
        bd.innerHTML = body;
        let modal_show = document.createElement("div"); modal_show.setAttribute("class", "modal show");
        let modal_content = document.createElement("div"); modal_content.setAttribute("class", "modal-content"); modal_content.appendChild(this.title); modal_content.appendChild(bd);
        modal_show.appendChild(modal_content);
        modal_show.addEventListener('click', this.modalClick);
        return modal_show;
    }
    ModalFooter(footer) {
        let div = document.createElement("div"); div.setAttribute("class", "modal-footer"); div.innerHTML = footer; return div;
    }appendChild
    AssembleModal() {
        this.background.appendChild(this.body);
        this.background.appendChild(this.footer);
        return this.background;
    }
}