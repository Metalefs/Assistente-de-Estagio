const ChangeUsuarioLogTablePage = async (pageNumber, id, UserEmailOrId) => {
    await $.ajax({
        type: 'get',
        dataType: 'html',
        url: '/Administracao/ControleUsuarios/ObterPaginaLogAcaoEspecial',
        data: { PageNumber: pageNumber, UserEmailOrId }
    }).done(function (data) {
        $("#"+id).html(data);
    }).catch((err) => {
        console.error(err);
    });
};

function TogglePageControll() {
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
function ToggleSyslog() {
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