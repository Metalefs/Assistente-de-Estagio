export class PorcentagemConlusaoPizzaChart {
    constructor(idUsuario = null) {
        this.idUsuario = idUsuario;
    }
    
    async PorcentagemConlusao() {
        if (this.idUsuario === null) {
            return await $.ajax({
                type: "get",
                url: "/Principal/UserHome/PorcentagemConclusao/",
                success: function (data) {
                    let DataSet = JSON.parse(data);
                    console.log(DataSet);
                    return DataSet;
                }
            });
        }
        else {
            return await $.ajax({
                type: "get",
                url: "/Principal/UserHome/PorcentagemConclusao/",
                data: { idUsuario: this.idUsuario },
                success: function (data) {
                    let DataSet = JSON.parse(data);
                    console.log(DataSet);
                    return DataSet;
                }
            });
        }
    }

    async CreatePizzaChart() {
        var ctx = document.getElementById('pizza-chart-canvas').getContext('2d');
        await this.PorcentagemConlusao().then((dataset) => {
            try {
                dataset = JSON.parse(dataset);
            }
            catch (err) {
                console.log(err);
            }
            var config = {
                type: 'doughnut',
                data: {
                    labels: [
                        "/" + dataset.Total + " itens totalizados",
                        ""
                    ],
                    datasets: [{
                        data: [dataset.ProgressoUsuario, dataset.Total - dataset.ProgressoUsuario],
                        backgroundColor: [
                            "#FF6384",
                            "#AAAAAA"
                        ],
                        hoverBackgroundColor: [
                            "#FF6384",
                            "#AAAAAA"
                        ],
                        hoverBorderColor: [
                            "#FF6384",
                            "#ffffff"
                        ]
                    }]
                },
                options: {
                    responsive: true,
                    legend: {
                        display: false
                    },
                    cutoutPercentage: 80,
                    tooltips: {
                        filter: function (item, data) {
                            var label = data.labels[item.index];
                            if (label) return item;
                        }
                    }
                }
            };
            var PieChart = new Chart(ctx, config);
            console.log(PieChart);
            var PorcentagemConclusao = this.CalcularPorcentagemConclusao(dataset.Total, dataset.ProgressoUsuario);
            this.CenterText(PorcentagemConclusao.toFixed(2));
        });

    }

    CalcularPorcentagemConclusao(total, completo) {
        return completo * 100 / total;
    }

    CenterText(val) {
        Chart.pluginService.register({
            beforeDraw: function (chart) {
                var width = chart.chart.width,
                    height = chart.chart.height,
                    ctx = chart.chart.ctx;

                ctx.restore();
                var fontSize = (height / 114).toFixed(2);
                ctx.font = fontSize + "em sans-serif";
                ctx.textBaseline = "middle";

                var text = val + "%",
                    textX = Math.round((width - ctx.measureText(text).width) / 2),
                    textY = height / 2;

                ctx.fillText(text, textX, textY);
                ctx.save();
            }
        });
    }
}
