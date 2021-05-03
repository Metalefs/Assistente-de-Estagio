export class NotificationDisplay {
    constructor(notificationRecipient) {
        this.notificationRecipient = $("#" + notificationRecipient);
        this.notificationCountRecipient = $("#" + notificationRecipient + '-count');
        this.notificationLoadingRecipient = $("#" + notificationRecipient + "-loading");
        this.IndividualNotifications = [];
        this.Notifications = [];
        this.NotificationCount = 0;
        this.GeneralNotificationCount = 0;
        this.Loading = this.LoadingCircle();
        this.State;
        this.notification_spinner_id = notificationRecipient + "-loading-spinner";
        this.updating;
    }
    async ObterNotificacoesDeAlteracaoDoSistema() {
        let self = this;
        return await $.ajax({
            type: "get",
            url: "/Notificacao/ObterNotificacoesDeAlteracaoDoSistema",
            success: function (data) {
                let Notificacoes = JSON.parse(data);
                if (Notificacoes.length > 0) {
                    self.Notifications = [];
                    Notificacoes.forEach(x => {
                        self.Notifications.push(new NotificacaoDeAlteracao(x, self));
                    });
                    self.AddNotifications(self.Notifications);
                } else {
                    //self.AddEmptyNotificationMessage("Sem notificações gerais");
                    self.State = "Empty";
                }
            }
        });
    }
    async ObterNotificacoesDeUsuario() {
        let self = this;
        return await $.ajax({
            type: "get",
            url: "/Notificacao/ObterNotificacoesDeUsuario",
            success: function (data) {
                let Notificacoes = JSON.parse(data);
                if (Notificacoes.length > 0) {
                    self.IndividualNotifications = [];
                    Notificacoes.forEach(x => {
                        self.IndividualNotifications.push(new NotificacaoIndividual(x, self));
                    });
                    self.AddIndidualNotifications(self.IndividualNotifications);
                } else {
                    self.AddEmptyNotificationMessage("Sem notificações");
                    self.State = "Empty";
                }
            }
        });
    }
    LoadingCircle() {
        let preloader_wrapper = document.createElement("div"),
            spinner_layer = document.createElement("div"),
            circle_clipper = document.createElement("div"),
            circle1 = document.createElement("div"),
            gap_patch = document.createElement("div"),
            circle2 = document.createElement("div"),
            circle_clipper_right = document.createElement("div"),
            circle3 = document.createElement("div");

        preloader_wrapper.id = this.notification_spinner_id;
        preloader_wrapper.className = "preloader-wrapper small active ade_flex";
        preloader_wrapper.style.margin = "0 auto";

        spinner_layer.className = "spinner-layer spinner-blue";

        circle_clipper.className = "circle-clipper left";
        circle1.className = "circle";

        gap_patch.className = "gap-patch";
        circle2.className = "circle";

        circle_clipper_right.className = "circle-clipper right";
        circle3.className = "circle";

        circle_clipper_right.appendChild(circle3);
        gap_patch.appendChild(circle2);
        circle_clipper.appendChild(circle1);
        spinner_layer.appendChild(circle_clipper);
        preloader_wrapper.appendChild(spinner_layer);

        return preloader_wrapper;
    }
    ShowLoadingCircle() {
        this.notificationLoadingRecipient.append(this.LoadingCircle());
    }
    HideLoadingCircle() {
        this.notificationLoadingRecipient.empty();
    }
    
    MinimizeButton() {
        self = this;
        let minimize = document.createElement("i");
        minimize.className = "fa fa-window-minimize float-right cursor-pointer";
        minimize.onclick = function () { self.notificationRecipient.toggleClass("show"); };
        return minimize;
    }
    ConfigurationButton() {
        self = this;
        let config = document.createElement("i");
        config.className = "ml-auto mr-4 prefix material-icons cursor-pointer";
        config.innerHTML = "settings";
        config.title = "Configurações";
        config.onclick = function () { window.location.href = "/Acesso/Notificacoes"; };
        return config;
    }
    ReloadButton() {
        self = this;
        let config = document.createElement("i");
        config.className = "ml-auto mr-4 prefix material-icons cursor-pointer";
        config.innerHTML = "refresh";
        config.title = "Atualizar";
        config.onclick = function () { self.ObterNotificacoes(); };
        return config;
    }
    ReadAllButton() {
        self = this;
        let config = document.createElement("i");
        config.className = "ml-auto mr-4 prefix material-icons cursor-pointer";
        config.title = "Ler todas";
        config.innerHTML = "clear";
        config.onclick = function () { self.ReadAll(); };
        return config;
    }
    ReadAll() {
        let self = this;
        for (let i = 0; i < this.Notifications.length; i++) {
            this.Notifications[i].MarkAsRead();
        }
        for (let i = 0; i < this.IndividualNotifications.length; i++) {
            this.IndividualNotifications[i].MarkAsRead();
        }
    }
    DismissButton(Notification) {
        let dismiss = document.createElement("i");
        dismiss.className = "fa fa-window-minimize float-right cursor-pointer";
        dismiss.onclick = function () { Notification.Read(); };
        return dismiss;
    }
    AddHeader(message) {
        let div = document.createElement("div"),
            widget_div = document.createElement("div");
        div.className = "card-header pb-o";
        widget_div.className = "halfway-fab row mb-0";
        div.innerText = message;
        //div.appendChild(this.MinimizeButton());
        widget_div.appendChild(this.ConfigurationButton());
        widget_div.appendChild(this.ReloadButton());
        widget_div.appendChild(this.ReadAllButton());
        this.notificationRecipient.append(widget_div);
        this.notificationRecipient.append(div);
    }
    AddEmptyNotificationMessage(texto) {
        this.AddHeader(texto);
    }
    AddNotifications(Notificacoes) {
        //this.AddHeader("Notificações Gerais");
        this.GeneralNotificationCount = Notificacoes.length;
        this.RefreshNotificationCount();
        Notificacoes.forEach(x => {
            this.CreateNotificationCard(x);
        });
    }
    AddIndidualNotifications(Notificacoes) {
        this.AddHeader("Notificações Individuais");
        this.NotificationCount = Notificacoes.length;
        this.RefreshNotificationCount();
        Notificacoes.forEach(x => {
            this.CreateIndividualNotificationCard(x);
        });
    }
    RefreshNotificationCount() {
        this.notificationCountRecipient.html(this.NotificationCount + this.GeneralNotificationCount);
    }
    DecreaseNotificationCount() {
        if (this.NotificationCount > 0) {
            this.NotificationCount--;
            this.notificationCountRecipient.html(this.NotificationCount);
        } else if (this.NotificationCount === 0) {
            this.notificationCountRecipient.empty();
        }
    }
    CreateNotificationCard(Notification) {
        let self = this;
        let card = document.createElement("div");
        card.className = "card cursor-pointer input-field notification-card";
        card.id = Notification.GUID;
        card.onclick = function () { Notification.MarkAsRead(); };
        let cardHeader = document.createElement("div");
        cardHeader.id = Notification.GUID + "header";
        card.onclick = function () { self.CreateFocusedNotificationCardDisplay(Notification, Notification.Mensagem); };
        cardHeader.className = "card-header card-title";
        cardHeader.innerText = Notification.NomeAutor + "-" + Notification.TipoEntidade;
        let cardFooter = document.createElement("div");
        cardFooter.className = "card-footer";
        cardFooter.innerText = Notification.Data.toLocaleString();
        cardHeader.appendChild(this.DismissButton());
        card.appendChild(cardHeader);
        card.appendChild(cardFooter);
        this.notificationRecipient.append(card);
    }
    CreateIndividualNotificationCard(Notification) {
        let self = this;
        let card = document.createElement("div");
        card.className = "card cursor-pointer notification-card";
        card.id = Notification.GUID;
        card.onclick = function () { Notification.MarkAsRead(); };
        let cardHeader = document.createElement("div");
        cardHeader.appendChild(this.DismissButton());
        cardHeader.id = Notification.GUID + "header";
        card.onclick = function () { self.CreateFocusedNotificationCardDisplay(Notification, Notification.Conteudo); };
        cardHeader.className = "card-header card-title";
        cardHeader.innerHTML = Notification.Cabecalho + " (Clique para ler mais!)";
        let cardFooter = document.createElement("div");
        cardFooter.className = "card-footer";
        cardFooter.innerText = Notification.DataHoraInclusao.toLocaleString();
        card.appendChild(cardHeader);
        card.appendChild(cardFooter);
        this.notificationRecipient.append(card);
    }
    CreateFocusedNotificationCardDisplay(Notification, Mensagem) {
        let self = this;
        let ModalBody = document.createElement("div"),
            InfoDiv = document.createElement("div"),
            Backdrop = document.createElement("div");

        let FormSubmitButton = document.createElement("a"),
            FormDestroyButton = document.createElement("a");
        FormSubmitButton.setAttribute("class", "btn ade-add-btn btn-primary");
        FormSubmitButton.addEventListener("click", () => { window.location.href = "/Acesso/Notificacoes" });
        FormSubmitButton.innerText = "Página de alterações";
        FormDestroyButton.setAttribute("class", "btn btn-red");
        FormDestroyButton.addEventListener("click", () => { Backdrop.click(); });
        FormDestroyButton.innerText = "Fechar";

        ModalBody.setAttribute("class", "modal-body card req_card window_body");
        Backdrop.setAttribute("class", "modal-backdrop");
        let bdId = "BackdropForNotification" + Notification.Identificador;
        Backdrop.setAttribute("id", bdId);
        Backdrop.addEventListener("click", () => { $("#" + bdId).remove(); });

        let InfoText = document.createElement("h5");
        InfoText.innerHTML = "<p>" + Mensagem + "</p>";

        InfoDiv.appendChild(InfoText);
        ModalBody.appendChild(InfoDiv);
        ModalBody.appendChild(FormSubmitButton);
        Backdrop.appendChild(ModalBody);
        document.getElementsByTagName("body")[0].appendChild(Backdrop);
        Notification.Read();
    }
    async Update() {
        let self = this;
        this.updating = setInterval( async ()=>{
            await self.ObterNotificacoes();
        }, 60000);
    }
    Clear() {
        this.notificationRecipient.empty();
        this.notificationCountRecipient.empty();
        this.NotificationCount = 0;
        this.GeneralNotificationCount = 0;
    }
    StopUpdating() {
        clearInterval(this.updating);
    }
    async ObterNotificacoes() {
        this.ShowLoadingCircle();
        this.Clear();
        await this.ObterNotificacoesDeUsuario();
        await this.ObterNotificacoesDeAlteracaoDoSistema();
        this.HideLoadingCircle();
    }
}

class NotificacaoDeAlteracao {
    constructor(Notification, NotificationDisplay) {
        this.NotificationDisplay = NotificationDisplay;
        this.Identificador = Notification.Identificador;
        this.Mensagem = Notification.Mensagem;
        this.IdAutor = Notification.IdAutor;
        this.NomeAutor = Notification.NomeAutor;
        this.Data = new Date(Notification.Data);
        this.Entidade = Notification.Entidade;
        this.TipoEntidade = Notification.TipoEntidade;
        this.StorageName = "Notification" + this.Identificador + "Seen";
        this.GUID = new Date().getTime() + "-GUID-" + "Notification-" + this.Identificador;
        this.isRead = false;
    }
    MarkAsRead() {
        if (this.CanBeRead()) {
            this.Read();
        } else {
            this.SaveToSessionStorage();
        }
    }
    SaveToSessionStorage() {
        sessionStorage.setItem(this.StorageName, true);
    }
    CanBeRead() {
        return sessionStorage.getItem(this.StorageName) === "true" && !this.isRead;
    }
    async Read() {
        let self = this;
        await $.ajax({
            type: "post",
            url: "/Notificacao/CadastrarVisualizacaoNotificacao",
            data: { idNotificacao: this.Identificador },
            success: function (data) {
                self.AddMark("fa fa-check");
                self.isRead = true;
                self.NotificationDisplay.DecreaseNotificationCount();
                self.Fade();
            },
            onerror: function (data) {
                self.AddMark("fa fa-error");
            }
        });
    }
    Fade() {
        $("#" + this.GUID).toggleClass("ade-fade");
    }
    AddMark(type) {
        let check = document.createElement("i");
        check.className = type;
        $("#" + this.GUID + "header").append(check);
    }
    OpenReadAuthor() {
        alert(this.Mensagem);
    }
}
class NotificacaoIndividual {
    constructor(Notification, NotificationDisplay) {
        this.NotificationDisplay = NotificationDisplay;
        this.Identificador = Notification.Identificador;
        this.Cabecalho = Notification.Cabecalho;
        this.Conteudo = Notification.Conteudo;
        this.DataHoraInclusao = new Date(Notification.DataHoraInclusao);
        this.StorageName = "IndividualNotification" + this.Identificador + "Seen";
        this.GUID = new Date().getTime() + "-GUID-" + "Notification-" + this.Identificador;
        this.isRead = false;
    }
    MarkAsRead() {
        if (this.CanBeRead()) {
            this.Read();
        } else {
            this.SaveToSessionStorage();
        }
    }
    SaveToSessionStorage() {
        sessionStorage.setItem(this.StorageName, true);
    }
    CanBeRead() {
        return sessionStorage.getItem(this.StorageName) === "true" && !this.isRead;
    }
    async Read() {
        let self = this;
        await $.ajax({
            type: "post",
            url: "/Notificacao/CadastrarVisualizacaoNotificacaoIndividual",
            data: { idNotificacao: self.Identificador },
            success: function (data) {
                self.AddMark("fa fa-check");
                self.isRead = true;
                self.NotificationDisplay.DecreaseNotificationCount();
                self.Fade();
            },
            onerror: function (data) {
                self.AddMark("fa fa-error");
            }
        });
    }
    Fade() {
        $("#" + this.GUID).toggleClass("ade-fade");
        setTimeout(() => { $("#" + this.GUID).toggleClass("collapse"); }, 5000);
    }
    AddMark(type) {
        let check = document.createElement("i");
        check.className = type;
        $("#" + this.GUID + "header").append(check);
    }
    OpenReadAuthor() {
        alert(this.Mensagem);
    }
}