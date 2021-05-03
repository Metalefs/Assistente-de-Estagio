import { PageHandler } from "/Scripts/ADE/PageHandler.js";
export class FormularioDadosAluno{
    constructor(loaderid) {
        this.loader = $("#" + loaderid);
        this.AreaCursoDiv = $("#area-curso-select");
        this.Estagiando = false;
        this.EstagiandoSet = false;
        this.ActiveTab = 0;
        this.Tabs = {};
    }

    SetEstagiando(value) {
        this.Estagiando = value;
        this.EstagiandoSet = true;
    }

    SetActiveTab(tab) {
        this.ActiveTab = tab;
    }

    async Submit() {
        this.ToggleCreationLoadingAnimation();
        let self = this;
        let DadosAluno = JSON.stringify($("#DadosAluno").serializeArray());
        let idArea = 0;
        try {
            idArea = this.AreaCursoDiv.children[0].selectedOptions[0].value;
        }
        catch (ex) {
            console.log(ex);
        }
        await $.ajax({
            type: "post",
            url: "/Principal/MeusDados/AtualizarDados",
            data: { DadosAluno: DadosAluno, Estagiando: self.Estagiando, IdArea: idArea },
            success: function (data) {
                try {
                    let res = JSON.parse(data);
                    if (res.hasOwnProperty("erro")) {
                        var toastHTML = '<span>' + res.erro + '!</span>';
                        M.toast({ html: toastHTML });
                    }
                    $("#dados-aluno-div").html(data);
                    M.toast({ html: "Sucesso ao alterar dados" });
                    self.Tabs = M.Tabs.init($('.tabs'), {});
                }
                catch (ex) {
                    $("#dados-aluno-div").html(data);
                    M.toast({ html: "Sucesso ao alterar dados" });
                    self.Tabs = M.Tabs.init($('.tabs'), {});
                }
            },
            error: function (data) {
                var toastHTML = '<span> Ocorreu um erro ao alterar dados!</span>';
                M.toast({ html: toastHTML });
            }
        });
        let ph = new PageHandler();
        await ph.refreshPages();
        await ph.switchPage("/Principal/MeusDados/Index");
        this.ToggleCreationLoadingAnimation();
        $('.tabs').tabs('select', this.ActiveTab);
    }

    async GetAreasForCourse() {
        let self = this;
        if ($("#area-curso-select").html().length < 10) {
            this.ToggleCreationLoadingAnimation();
            await $.ajax({
                type: "get",
                url: "/Account/AreasEstagioParaCurso",
                success: function (data) {
                    $("#area-curso-select").html(data);
                    self.ToggleCreationLoadingAnimation();

                    $('select').formSelect();
                }
            });
        }
    }
    ToggleCreationLoadingAnimation() {
        this.loader.toggleClass("active");
    }
}

const ChangeTablePage = async (pageNumber, idTable, idUsuario) => {
    await $.ajax({
        type: 'get',
        dataType: 'html',
        url: '/Principal/MeusDados/ObterPaginaRequisitoUsuario',
        data: { PageNumber: pageNumber, IdUsuario: idUsuario }
    }).done(function (data) {
        $("#" + idTable).html(data);
    }).catch((err) => {
        console.error(err);
    });
};