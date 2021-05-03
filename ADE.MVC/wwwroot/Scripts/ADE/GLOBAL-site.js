import { CreateDatePicker, CreateTimePicker } from "./MaterializeDates.js";
import { addStyleSheet } from "/Scripts/ADE/PageSetup.js";
import { PageHandler } from "/Scripts/ADE/PageHandler.js";
import { HandleDefault } from "/Scripts/ADE/AsyncHttpMethods.js";
import { AtividadeAluno } from '/Scripts/ADE/Principal/Atividades/AtividadesAluno.js';
import * as Horas from '/Scripts/ADE/Principal/RegistroHoras/TabelaHoras.js';
import { Formulario } from "/Scripts/ADE/Principal/ListagemDocumento/ListagemDocumentos.js";
import { EditorRegistroHoras } from '/Scripts/ADE/Principal/RegistroHoras/TabelaHoras.js';
import { FormularioDadosAluno } from "/Scripts/ADE/Principal/MeusDados/DadosAluno.js";
import { FilterTableWithInput } from "/Scripts/ADE/InputEvents.js";
import { LoginComponent } from "/Scripts/ADE/Acesso/login.js";


window.FormularioDados = new FormularioDadosAluno("dados-aluno-loader");
window.EditorRegistroHoras = EditorRegistroHoras;
window.AtividadeAluno = AtividadeAluno;
window.Formulario = Formulario;
window.PreenchimentoDocumento = new Formulario("serial-input-form");
window.Horas = Horas;
window.addStyleSheet = addStyleSheet;
window.FilterTableWithInput = FilterTableWithInput;
window.Login = new LoginComponent();


window.AceitarTermos = async function () {
    $.post("/Account/AceitarTermosCompromisso", {}, HandleDefault);
    window.PreenchimentoDocumento.DocumentoAberto = 0;
    $("#Modal-Termo-Compromisso").hide();
    let ph = new PageHandler();
    await ph.refreshPages();
};

$("#accept").click( async () => {
    await AceitarTermos();
    window.location.reload();
});

window.changeTheme = function (type) {
    if (type !== null) {
        document.getElementById("site-theme").setAttribute("href", "/Styles/" + type + "-theme.css");
        sessionStorage.setItem("theme", type);
    }
};

window.PageHandler = new PageHandler();
window.PageHandler.cachePages();
if (window.location.hash !== null) {
    $(window.location.hash).click();
}
const main_display = document.getElementById("main-display");
if (main_display !== null)
window.history.pushState({ active: main_display.innerHTML }, "Assistente De Estagio");
if (window.history.state !== null)
    if (window.history.state.hasOwnProperty("active"))
        if (main_display !== null)
            main_display.innerHTML = window.history.state.active;

changeTheme(sessionStorage.getItem("theme"));
document.addEventListener('DOMContentLoaded', function () {
    $(document).ready(function () {
        $('.sidenav').sidenav();
        $('select').formSelect();
        $('.tooltipped').tooltip();
        $('.collapsible').collapsible();
        M.Tabs.init($('.tabs'), {});
        $('.dropdown-trigger').dropdown();
        $('.modal').modal();
        $('.tap-target').tapTarget();
        CreateDatePicker();
        CreateTimePicker();
        if ('serviceWorker' in navigator) {
            console.log('Service Worker is supported');
            navigator.serviceWorker.register('/Scripts/ADE/ADE-ServiceWorker.js')
                .then(function (swReg) {
                    console.log('Service Worker is registered from site.js', swReg);
                })
                .catch(function (error) {
                    console.error('Service Worker Error from site.js', error);
                });
        }
        else {
            console.error('Service Worker Not Supported');
        }

    }, false);
});

function dragElement(elmnt) {
    let elmntid = elmnt;
    elmnt = document.getElementById(elmnt);
    var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0, chatbox = 0;
    if ($("#" + elmntid + "-window-basic-control").length > 0) {
        document.getElementById(elmntid + "-window-basic-control").onmousedown = dragMouseDown;
    } else {
        elmnt.onclick = dragMouseDown;
    }
    function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();
        pos3 = e.clientX;
        pos4 = e.clientY;
        document.onmouseup = closeDragElement;
        document.onmousemove = elementDrag;
    }
    function elementDrag(e) {
        e = e || window.event;
        e.preventDefault();
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        chatbox = pos3;
        if (window.innerWidth > chatbox || chatbox > 0) {
            if (window.innerWidth > 1200) {
                if (e.clientX + window.innerWidth > window.innerWidth + 300 && e.clientX + 300 < window.innerWidth) {
                    elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
                } else {
                    elmnt.style.left = "inherit";
                }
            }
        }
    }
    let checkPosition = () => {
        if (window.innerWidth < elmnt.style.left || elmnt.style.left < 0) {
            document.getElementById(elmntid).style.left = "inherit";
        }
    };
    setInterval(checkPosition, 3000);
    function closeDragElement() {
        document.onmouseup = null;
        document.onmousemove = null;
    }
}

window.dragElement = dragElement;