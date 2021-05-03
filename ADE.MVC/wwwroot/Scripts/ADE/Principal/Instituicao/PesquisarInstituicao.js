class SearchInstituicaoInput {
    constructor(inputid) {
        this.input = document.getElementById(inputid);
    }
    Search(event, value) {
        if (this.isEnter(event)) {
            this.SendRequest(value);
        }
    }
    SearchBtn() {
        this.SendRequest(this.input.value);
    }
    async SendRequest(value) {
        let div = document.getElementById("InstituicaoOptions");
        let feedback = document.getElementById("search-feedback");
        await $.ajax({
            type: 'get',
            dataType: 'html',
            url: '/Principal/Instituicao/ObterResultadoPesquisaInstituicao',
            data: { Nome: value }
        }).done(function (data) {
            console.log(data);
            if (data.length > 0 && data !== null && data !== "") {
                div.innerHTML = data;
                feedback.innerHTML = "Exibindo resultados";
            }
            else
                feedback.innerHTML = "Nenhuma Instituição com esse nome foi encontrada.";
        }).catch((err) => {
            console.error(err);
        });
    }
    isEnter(event) {
        return event.keyCode === 13 ? true : false;
    }
}
const EscolherInstituicao = (idInstituicao) => {
    window.location.href = `/Principal/Instituicao/TrocarInstituicao/?idInstituicao=${idInstituicao}`;
};
let searchInstituicao = new SearchInstituicaoInput("InputFiltro-instituicao");