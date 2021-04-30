import { post } from "/Scripts/ADE/AsyncHttpMethods.js";
document.getElementById("ADE-body").style.background = "#687a8d";

export class LoginComponent {
    #EmailLogin = "";
    #PasswordLogin = "";
    #RememberMe = "";
    #EmailRegistro = "";
    #PasswordRegistro = "";
    #EmailForgotPassword = "";
    constructor() {
        this.controller = "Account";
        this.urls = {
            login: this.controller + "/" + "Login",
            registrar: this.controller + "/" + "Registrar",
            forgotPassword: this.controller + "/" + "ForgotPassword"
        };
        this.Tabs = $(".tab-multi-tab");
    }

    toggleInputVisibility(id) {
        let input = document.getElementById(id);
        if (input.type === "password") {
            input.type = "text";
        } else {
            input.type = "password";
        }
    }

    SetEmailLogin(value) {
        this.EmailLogin = value;
    }
    SetPasswordLogin(value) {
        this.PasswordLogin = value;
    }
    SetRememberMe(value) {
        this.RememberMe = value;
    }
    SetEmailRegistro(value) {
        this.EmailRegistro = value;
    }
    SetPasswordRegistro(value) {
        this.PasswordRegistro = value;
    }
    SetEmailForgotPassword(value) {
        this.EmailForgotPassword = value;
    }

    PasswordRecoveryTab() {
        this.SelectTab(0);
    }
    LoginTab() {
        this.SelectTab(1);
    }

    SelectTab(index) {
        switch (index) {
            case 0:
                this.Tabs[0].className = "tab-multi-tab hide-left";
                this.Tabs[1].className += " active";
                break;
            case 1:
                this.Tabs[1].className = "tab-multi-tab hide-right";
                this.Tabs[0].className += " active";
                break;
        }
    }

    async CheckLoginSubmit(e) {
        if (e.keyCode === 13)
            await this.Login();
    }
    async Login() {
        let data = {
            Email: this.EmailLogin,
            Password: this.PasswordLogin,
            ExternalLogins: "",
            RememberMe: this.RememberMe,
            jsonRQ: "true"
        };
        post(this.urls.login, data, this.handle);
    }
    async CheckRegistrarSubmit(e) {
        if (e.keyCode === 13)
            await this.Registro();
    }
    async Registro() {
        let data = {
            RegistrarEmail: this.EmailRegistro,
            RegistrarPassword: this.PasswordRegistro,
            jsonRQ: "true"
        };
        post(this.urls.registrar, data, this.handle);
    }
    async CheckForgotPasswordSubmit(e) {
        if (e.keyCode === 13)
            await this.ForgotPassword();
    }
    async ForgotPassword() {
        let data = { Email: this.EmailForgotPassword, jsonRQ: true };
        post(this.urls.forgotPassword, data, this.handle);
    }

    handle(result, url) {
        let toastHTML = "";
        let self = new LoginComponent();
        try {
            console.log(url === (self.urls.login), url, (self.urls.login));
            if (url === (self.urls.login)) {
                console.log(url, "login", result);
                try {
                    if (result.hasOwnProperty("sucesso")) {
                        window.location.pathname = "Principal/UserHome/Index";
                    }
                    if (result.hasOwnProperty("erro")) {
                        let options = {};
                        if (result.erro.includes("Falha ao autenticar o usuário")) {
                            options.displayLength = 10 * 100;
                        }
                        options.classes = 'bg-danger';
                        toastHTML = '<span>' + result.erro + '</span>';
                        M.toast({ html: toastHTML, options });
                    }
                } catch (ex) {
                    if (result.hasOwnProperty("erro")) {
                        toastHTML = '<span>' + result.erro + '</span>';
                        M.toast({ html: toastHTML, classes: 'bg-danger' });
                    }
                }
                return;
            }
            else if (url === (self.urls.registrar)) {
                console.log(url, result);
                try {
                    if (result.hasOwnProperty("sucesso")) {
                        toastHTML = '<span>' + result.sucesso + '</span>';
                        M.toast({ html: toastHTML, classes: 'bg-success' });
                        window.location.pathname = "Principal/UserHome/Index";
                    }
                    if (result.hasOwnProperty("erro")) {
                        toastHTML = '<span>' + result.erro + '</span>';
                        M.toast({ html: toastHTML, classes: 'bg-danger' });
                    }
                } catch (ex) {
                    if (result.hasOwnProperty("erro")) {
                        toastHTML = '<span>' + result.erro + '</span>';
                        M.toast({ html: toastHTML, classes: 'bg-danger' });
                    }
                }
                return;
            }
            else if (url === (self.urls.forgotPassword)) {
                console.log(url, result);
                try {
                    if (result.hasOwnProperty("sucesso")) {
                        toastHTML = '<span>' + result.sucesso + '</span>';
                        M.toast({ html: toastHTML, classes: 'bg-success' });
                    }
                    if (result.hasOwnProperty("erro")) {
                        toastHTML = '<span>' + result.erro + '</span>';
                        M.toast({ html: toastHTML, classes: 'bg-danger' });
                    }
                } catch (ex) {
                    console.log(ex);
                    if (result.hasOwnProperty("erro")) {
                        toastHTML = '<span>' + result.erro + '</span>';
                        M.toast({ html: toastHTML, classes: 'bg-danger' });
                    }
                }
                return;
            }
        }
        catch (ex) {
            console.log(ex);
            console.log(url, result);
            toastHTML = '<span>' + Object.values(result)[0] + '</span>';
            M.toast({ html: toastHTML, classes: 'bg-danger' });
        }
    }

}
export class ImageUpdater {
    constructor(client) {
        this.JSONImagens = {};
        this.seed = 0;
        this.ImageClient = document.getElementById(client);
    }
    async GetBackgroundImages() {
        let self = this;
        return await $.ajax({
            type: "get",
            url: "/Account/GetCarrouselImageUrl",
            success: function (data) {
                self.JSONImagens = JSON.parse(data);
                if (!self.JSONImagens.hasOwnProperty("Erro")) {
                    self.UpdateCarrouselDesktop(self.JSONImagens);
                    self.UpdateCarrouselMobile(self.JSONImagens);
                } else {
                    self.ImageClient.style.background = "white";
                }
                return data;
            }
        });
    }
    UpdateCarrouselMobile(url) {
        if (window.innerWidth < 780) {
            $("#" + this.ImageClient.id).toggleClass("animate");
            $("#carouselExampleControls").empty();
            this.ImageClient.style.background = "url(" + url[1].Caminho + ")";
            this.ImageClient.style.backgroundSize = "cover";
        }
    };
    UpdateCarrouselDesktop(urls) {
        if (window.innerWidth > 780) {
            $("#" + this.ImageClient.id).toggleClass("animate");
            this.CreateImageCarrousel(urls);
        }
    }
    CreateImageCarrousel(url) {
        //let carrousel = "";
        //let div = document.createElement("div");
        //div.setAttribute("id", "carouselExampleControls");
        //div.setAttribute("class", "carousel slide");
        //div.setAttribute("data-ride", "carousel");
        //carrousel = `
        //    <div class="carousel-inner">`
        //    ;
        //carrousel += `
        //      <div class="carousel-item active">
        //            <img class="d-block w-100" src="`+ url[2].Caminho + `" alt="` + url[2].Nome + `">
        //      </div>  
        //    `;
        //for (let i = 1; i < url.length; i++) {
        //    carrousel += `
        //      <div class="carousel-item">
        //            <img class="d-block w-100" src="`+ url[2].Caminho + `" alt="` + url[2].Nome + `">
        //      </div>  
        //    `;
        //}
        //carrousel += `
        //    </div>`
        //    ;
        //div.innerHTML = carrousel;
        //this.ImageClient.appendChild(div);
        this.ImageClient.style.background = "url(" + url[0].Caminho + ")";
        this.ImageClient.style.backgroundSize = "cover";
    }
    Shuffle() {
        let self = this;
        this.GetBackgroundImages().then(x => {
            setInterval(function () {
                //this.Seed = Math.floor(Math.random() * (this.JSONImagens.length - 0)) + 0;
                self.UpdateCarrouselMobile(/*this.JSONImagens[this.Seed].Caminho*/);
            }, 15000);
        });
    }
}
