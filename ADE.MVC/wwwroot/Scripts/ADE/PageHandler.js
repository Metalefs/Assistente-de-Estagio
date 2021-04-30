import { get } from "/Scripts/ADE/AsyncHttpMethods.js";
import * as PageSetup from "./PageSetup.js";

export class PageHandler {
    constructor() {
        this.urls = [
            '/Principal/UserHome/Index',
            '/Principal/RegistroHoras/Index',
            '/Principal/Atividades/Index',
            '/Principal/MeusDados/Index',
        ];
        this.pages = [];
        this.main_display = document.getElementById("main-display");
    }

    cachePage = function (name, data) {
        if (this.pages !== null) {
            if (this.pages['' + name + ''] !== undefined)
                this.pages['' + name + ''] = null;

            this.pages['' + name + ''] = data;
        }
    };

    switchPage = async function (url) {
        if (this.pages['' + url + ''] !== undefined) {
            document.getElementById("main-display").innerHTML = this.pages['' + url + ''];
            this.refreshComponents();
            await PageSetup.setupUrl(url);
            return;
        } else {
            let html = await get(url, { Partial: true });
            this.cachePage(url, html);
            window.history.pushState({ active: html }, "Assistente De Estagio");
            document.getElementById("main-display").innerHTML = this.pages['' + url + ''];
        }
        this.refreshComponents();
    };

    cachePages() {
        this.urls.forEach(async (url) => {
            let html = await get(url, { Partial: true });
            this.cachePage(url, html);
        });
    }

    async refreshPages() {
        this.pages = null;
        await this.cachePages();
    }

    async refreshPage(page,_switch = true) {
        let html;
        switch (page) {
            case "Dashboard":
                html = await get(this.urls[0], { Partial: true });
                this.cachePage(this.urls[0], html);
                if (_switch)
                    document.getElementById("main-display").innerHTML = this.pages[this.urls[0]];
            break;
            case "TabelaHoras":
                html = await get(this.urls[1], { Partial: true });
                this.cachePage(this.urls[1], html);
                if (_switch)
                    document.getElementById("main-display").innerHTML = this.pages[this.urls[1]];
            break;
            case "Atividades":
                html = await get(this.urls[2], { Partial: true });
                this.cachePage(this.urls[2], html);
                if (_switch)
                    document.getElementById("main-display").innerHTML = this.pages[this.urls[2]];
            break;
            case "Dados":
                html = await get(this.urls[3], { Partial: true });
                this.cachePage(this.urls[3], html);
                if (_switch)
                    document.getElementById("main-display").innerHTML = this.pages[this.urls[3]];
            break;
        }
    }
    
    refreshComponents() {
        $("#main-display").find("script").each(function () {
            let script = $(this).text();
            if (!script.includes("import"))
                eval($(this).text());
        });
        $('.tooltipped').tooltip();
        $('.collapsible').collapsible();

        $('select').formSelect();
        M.Tabs.init($('.tabs'), {});
        $('.dropdown-trigger').dropdown();
        $('.modal').modal();
    }
}
