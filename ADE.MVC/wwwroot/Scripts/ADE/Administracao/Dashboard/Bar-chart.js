export function CursoDocumentoBarChart() {

    this.DownloadData = {};
    this.QuantidadeData = {};

    let DownloadDocumentosPorCurso = async function () {
        return await $.ajax({
            type: "get",
            url: "/Administracao/Dashboard/DownloadDocumentosPorCursoJson",
            success: function (data) {
                ChartData = JSON.parse(data);
                console.log({ "Download (de documentos) por curso": ChartData });
                return ChartData;
            }
        });
    };

    let QuantidadeDocumentosPorCurso = async function () {
        return await $.ajax({
            type: "get",
            url: "/Administracao/Dashboard/QuantidadeDocumentosPorCursoJson",
            success: function (data) {
                ChartData = JSON.parse(data);
                console.log({ "Quantidade de documentos por curso": ChartData });
                return ChartData;
            }
        });
    };

    this.CreateStackedBarChart = async function () {
        var ctx = document.getElementById('bar-chart-canvas').getContext('2d');

        await DownloadDocumentosPorCurso().then((dataset) => {
            try {
                dataset = JSON.parse(dataset);
                this.DownloadData = {
                    label: 'Download de documentos por curso',
                    backgroundColor: 'rgba(242, 193, 102,  0.8)',
                    borderColor: 'rgba(242, 193, 102,  0.8)',
                    borderWidth: 0,
                    data: dataset.Values
                };
            }
            catch (err) {
                console.log(err);
            }
        });

        await QuantidadeDocumentosPorCurso().then((dataset) => {
            try {
                dataset = JSON.parse(dataset);
                this.QuantidadeData = {
                    label: 'Quantidade de documentos por curso',
                    backgroundColor: 'rgba(69, 191, 85, 0.8)',
                    borderColor: 'rgba(69, 191, 85, 0.8)',
                    borderWidth: 1,
                    data: dataset.Values
                };
            }
            catch (err) {
                console.log(err);
            }

            var horizontalBarChartData = {
                labels: dataset.Labels,
                datasets: [this.DownloadData,this.QuantidadeData]
            };

            var chartOptions = {
                elements: {
                    rectangle: {
                        borderWidth: 2
                    }
                },
                responsive: true,
                legend: {
                    position: 'right'
                },
                title: {
                    display: false,
                    text: 'Cursos x Documentos'
                }
            };

            var stackedBar = new Chart(ctx, {
                type: 'horizontalBar',
                data: horizontalBarChartData,
                options: chartOptions
            });
            console.log(stackedBar);
        });
    };
}
