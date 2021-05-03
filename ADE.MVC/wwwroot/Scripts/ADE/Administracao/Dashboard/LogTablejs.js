export class Table {
    constructor() {

    }
    ChangeAdmMessageTablePage = async (pageNumber, id) => {
        await $.ajax({
            type: 'get',
            dataType: 'html',
            url: '/Administracao/Dashboard/ObterPaginaLogAcaoEspecial',
            data: { PageNumber: pageNumber }
        }).done(function (data) {
            $("#" + id).html(data);
            TogglePageControll();
        }).catch((err) => {
            console.error(err);
        });
    };
    ChangeLogTablePage = async (pageNumber, id) => {
        await $.ajax({
            type: 'get',
            dataType: 'html',
            url: '/Administracao/Dashboard/ObterPaginaSysLog',
            data: { PageNumber: pageNumber }
        }).done(function (data) {
            $("#" + id).html(data);
        }).catch((err) => {
            console.error(err);
        });
    };
    TogglePageControll() {
        var controll = document.getElementById("table-page-control");
        var controll2 = document.getElementById("log-table-body");
        var eye = document.getElementById("page-controll-eye");
        if (controll.style.display === "") {
            controll.style.display = "none";
            controll2.style.display = "none";
            eye.style.color = "black";
        }
        else {
            controll.style.display = "";
            controll2.style.display = "";
            eye.style.color = "white";
        }
    }
    ToggleSyslog() {
        var controll = document.getElementById("syslog-controll");
        var eye = document.getElementById("page-controll-eye-syslog");
        if (controll.style.display === "") {
            controll.style.display = "none";
            eye.style.color = "black";
        }
        else {
            controll.style.display = "";
            eye.style.color = "white";
        }
    }
}
