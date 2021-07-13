export async function addStyleSheet (url, id) {
    if ($("#" + id).length > 0) {
        $("#" + id).attr("href", url);
    } else {
        let style = document.createElement("link");
        style.rel = "stylesheet";
        style.href = url;
        style.id = id;
        document.head.appendChild(style);
    }
}

export async function setupUrl(url) {
    if (url === "/Principal/Atividades/Index") {
        await Atividades();
    }
    else if (url === "/Principal/UserHome/Index") {
        await UserHome();
        const { PorcentagemConlusaoPizzaChart } = await import('/Scripts/ADE/Principal/UsuarioHome/PorcentagemConclusaoChart.js');
        const { AtividadesUsuarioChart } = await import('/Scripts/ADE/Principal/UsuarioHome/AtividadesLineChart.js');
        try {
            if (!location.pathname.includes('/Principal/')) {
                location.pathname = "/Principal/UserHome";
            }
            MateriaisController.Add();
            FilterTableWithInput("InputFiltro-doc", "doc-table");

            $('#doc-table').DataTable({
                searching: false,
                fixedHeader: true,
                info: false,
                lengthChange: false,
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.10.20/i18n/Portuguese-Brasil.json"
                },
                responsive: true

            }).draw(true).columns.adjust();

            let PorcentagemConclusaoChart = new PorcentagemConlusaoPizzaChart();
            PorcentagemConclusaoChart.CreatePizzaChart();
            let AtividadesChart = new AtividadesUsuarioChart("line-chart-canvas");
            
        }
        catch (ex) { }
    }
    else if (url === "/Principal/RegistroHoras/Index") {
        await RegistroHoras();
    }
    else if (url === "/Principal/MeusDados/Index") {
        await DadosAluno();
    }
}

export async function Atividades() {
    if (window.PreenchimentoDocumento === undefined) {
        const moduleSpecifier = "/Scripts/ADE/Principal/ListagemDocumento/ListagemDocumentos.js";
        const module = await import(moduleSpecifier);
        try {
            (async () => {
                const PreenchimentoDocumento = new module.Formulario("serial-input-form");
                window.PreenchimentoDocumento = PreenchimentoDocumento;
            })();
        }
        catch (ex) {}
    }
    addStyleSheet("/Styles/Principal/ModalPreenchimento.css", "listagem_documentos");
}
export async function UserHome() {
    const modulespecifier = "/Scripts/ADE/Principal/ListagemDocumento/MateriaisComponent.js";
    const module = await import(modulespecifier);

    const { FilterTableWithInput } = await import('/Scripts/ADE/InputEvents.js');
    const module2 = await import("/Scripts/ADE/Principal/RegistroHoras/RegistroHorasComponent.js");

    try {
        FilterTableWithInput("inputfiltro-doc", "doc-table");

        const MateriaisController = new module.MateriaisComponent();
        window.MateriaisController = MateriaisController;

        const RegistroHorasController  = new module2.RegistroHorasComponent();
        window.RegistroHorasController = RegistroHorasController;

        MateriaisController.Add();
    }
    catch (ex) {}

}
export async function RegistroHoras() {
    const moduleSpecifier = '/Scripts/ADE/Principal/RegistroHoras/TabelaHoras.js';
    const MD = await import("/Scripts/ADE/MaterializeDates.js");
    const module = await import(moduleSpecifier);

    try {
        let calculo_horas = new module.CalculoHoras("calculo-carga-horaria");
        window.calculo_horas = calculo_horas;

        MD.CreateTimePicker();
    }
    catch (ex) {}
}
export async function DadosAluno() {
    const moduleSpecifier = "/Scripts/ADE/Principal/MeusDados/DadosAluno.js";
    const module = await import(moduleSpecifier);
    const mask = await import('/Scripts/Externo/jquery.mask.js');

    try {
        if (window.mask === undefined)
            window.mask = mask;

        mask.refresh_mask();
        window.FormularioDados = new module.FormularioDadosAluno("dados-aluno-loader");
    }
    catch (ex) {}
}
