class FAQDisplay {
    #DisableLike = false;
    #DisableDislike = false;
    constructor(IdInstituicao, IdFAQ) {
        this.IdInstituicao = IdInstituicao;
        this.IdFAQ = IdFAQ;
        this.DivAdicionarPergunta = $("#adicionar-faq-action");
        this.BotaoDestruirPergunta = this.CreateButton("btn btn-red", "Fechar");
    }
    CreateInput(Class, placeholder, title) {
        let FAQInput = document.createElement("input");
        FAQInput.setAttribute("class", Class);
        FAQInput.setAttribute("data-toggle", "tooltip");
        FAQInput.setAttribute("data-placement", "top");
        FAQInput.setAttribute("title", title);
        FAQInput.autofocus = true;
        FAQInput.setAttribute("placeholder", placeholder);
        return FAQInput;
    }
    CreateButton(Class, text) {
        let button = document.createElement("a");
        button.setAttribute("class", Class);
        button.innerText = text;
        return button;
    }
    AssembleModalBody(Form, FAQInput, FormSubmitButton, FormDestroyButton, FormDiv, InfoText, modal) {
        let bdId = "BackdropForFAQ" + this.IdFAQ;
        let mdId = "Modal" + this.IdFAQ;
        let ModalBody = document.createElement("div"),
            Backdrop = document.createElement("div"),
            InfoDiv = document.createElement("div");
        if (modal)
            Backdrop.setAttribute("class", "modal-backdrop");
        else
            Backdrop.setAttribute("class", "");

        ModalBody.setAttribute("class", "modal-body card req_card window_body");
        ModalBody.setAttribute("id", mdId);
        Backdrop.setAttribute("id", bdId);
        Form.appendChild(FAQInput);
        Form.appendChild(FormSubmitButton);
        Form.appendChild(FormDestroyButton);
        FormDiv.appendChild(Form);
        InfoDiv.appendChild(InfoText);
        ModalBody.appendChild(InfoDiv);
        ModalBody.appendChild(FormDiv);
        Backdrop.appendChild(ModalBody);
        //Backdrop.addEventListener("click", (evt) => { if (evt.target.id !== mdId) $("#" + bdId).remove(); });
        console.log(Backdrop);
        return Backdrop;
    }
    AdicionarPergunta() {
        let self = this;
        let bdId = "BackdropForFAQ" + this.IdFAQ;

        let FormDiv = document.createElement("div");
        FormDiv.setAttribute("class", "md-form dl-horizontal");

        let Form = document.createElement("form");
        Form.setAttribute("class", "md-form");

        let FAQInput = this.CreateInput("input-group faq-input", "Pergunta", "Adicione sua pergunta");
        FAQInput.addEventListener("keyup", (evt) => { if (evt.keyCode === 13) self.AdcionarPergunta(this.value); });

        let FormSubmitButton = this.CreateButton("btn ade-add-btn btn-primary", "Adicionar");
        this.BotaoDestruirPergunta = this.CreateButton("btn btn-red", "Fechar");
        FormSubmitButton.addEventListener("click", () => { self.AdcionarPergunta(FAQInput.value); });
        this.BotaoDestruirPergunta.addEventListener("click", () => { $("#" + bdId).remove(); });

        let InfoText = document.createElement("h5");
        InfoText.innerHTML = "<p> Adicionar pergunta </p>";

        $("body").append(this.AssembleModalBody(Form, FAQInput, FormSubmitButton, this.BotaoDestruirPergunta, FormDiv, InfoText, true));
    }
    async AdcionarPergunta(pergunta) {
        let self = this;
        return await $.ajax({
            type: "post",
            url: "/Ajuda/FAQ/AdicionarPergunta",
            data: { Pergunta: pergunta, IdInstituicao: self.IdInstituicao },
            success: function (data) {
                let retorno = JSON.parse(data);
                if (retorno.hasOwnProperty("Sucesso")) {
                    $("#retorno-faq-action").html(
                        `<div class="rounded bg-light card notification">
                                `+ retorno.Sucesso + `     
                         </div>`
                    );
                } else if (retorno.hasOwnProperty("Erro")) {
                    $("#retorno-faq-action").html(
                        `<div class="rounded bg-light card notification">
                                `+ retorno.Erro + `     
                         </div>`
                    );
                }
                self.BotaoDestruirPergunta.click();
            }
        });
    }
    AdicionarResposta(pergunta) {
        let self = this;
        let bdId = "BackdropForFAQ" + this.IdFAQ;

        let FormDiv = document.createElement("div");
        FormDiv.setAttribute("class", "md-form dl-horizontal");

        let Form = document.createElement("form");
        Form.setAttribute("class", "md-form");

        let FAQInput = this.CreateInput("input-group faq-input", "Resposta para " + pergunta, "Adicione sua resposta");
        FAQInput.addEventListener("keyup", (evt) => { if (evt.keyCode === 13) self.AdcionarRespostaAsync(this.value, bdId); });

        let FormSubmitButton = this.CreateButton("btn ade-add-btn btn-primary", "Adicionar");
        let FormDestroyButton = this.CreateButton("btn btn-red", "Fechar");
        FormSubmitButton.addEventListener("click", () => { self.AdcionarRespostaAsync(FAQInput.value, bdId); });
        FormDestroyButton.addEventListener("click", () => { $("#" + bdId).remove(); });

        let InfoText = document.createElement("h5");
        InfoText.innerHTML = "<p> " + pergunta + "? </p>";

        let Backdrop = this.AssembleModalBody(Form, FAQInput, FormSubmitButton, FormDestroyButton, FormDiv, InfoText, true);
        document.getElementsByTagName("body")[0].appendChild(Backdrop);
    }
    async AdcionarRespostaAsync(resposta, idmodal) {
        let self = this;
        return await $.ajax({
            type: "post",
            url: "/Ajuda/FAQ/AdicionarResposta",
            data: { IdFAQ: self.IdFAQ, Resposta: resposta, IdInstituicao: self.IdInstituicao },
            success: function (data) {
                let retorno = JSON.parse(data);
                if (retorno.hasOwnProperty("Sucesso")) {
                    $("#retorno-faq-action").html(
                        `<div class="rounded bg-light card notification">
                                `+ retorno.Sucesso + `     
                         </div>`
                    );
                } else if (retorno.hasOwnProperty("Erro")) {
                    $("#retorno-faq-action").html(
                        `<div class="rounded bg-light card notification">
                                `+ retorno.Erro + `     
                         </div>`
                    );
                }
                $("#" + idmodal).remove();
            }
        });
    }
    async Like() {
        if (this.DisableLike) {
            return;
        }
        let self = this;
        return await $.ajax({
            type: "post",
            url: "/Ajuda/FAQ/Like",
            data: { IdFAQ: self.IdFAQ },
            success: function (score) {
                self.UpdateFAQScore(score);
                self.DisableLike = true;
                self.DisableDislike = false;
            }
        });
    }
    async Dislike() {
        if (this.DisableDislike) {
            return;
        }
        let self = this;
        return await $.ajax({
            type: "post",
            url: "/Ajuda/FAQ/Dislike",
            data: { IdFAQ: self.IdFAQ },
            success: function (score) {
                self.UpdateFAQScore(score);
                self.DisableDislike = true;
                self.DisableLike = false;
            }
        });
    }
    async Deletar() {
        let self = this;
        return await $.ajax({
            type: "delete",
            url: "/Ajuda/FAQ/Deletar",
            data: { IdFAQ: self.IdFAQ },
            success: function (data) {
                $("#pergunta-" + self.IdFAQ).empty();
            }
        });
    }
    UpdateFAQScore(score) {
        $("#FAQ-" + this.IdFAQ + "-Counter").html(score);
    }
}