export function TrafegoUsuarioChart() {
    let TrafegoSite = async function () {
        return await $.ajax({
            type: "get",
            url: "/Administracao/Dashboard/TrafegoSiteJson",
            success: function (data) {
                let TrafegoSiteDataSet = JSON.parse(data);
                console.log(TrafegoSiteDataSet);
                return TrafegoSiteDataSet;
            }
        });
    };

    this.CreateLineChart = async function () {
        await TrafegoSite().then((dataset) => {
            try {
                dataset = JSON.parse(dataset);
            }
            catch (err) {
                console.log(err);
            }
            var options = {
                series: [{
                    name: "Logins",
                    data: dataset.Values
                }],
                chart: {
                    height: 350,
                    type: 'line',
                    zoom: {
                        enabled: false
                    }
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: 'straight'
                },
                title: {
                    text: '',
                    align: 'left'
                },
                grid: {
                    row: {
                        colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                        opacity: 0.5
                    },
                },
                xaxis: {
                    categories: dataset.Labels,
                }
            };

            var LineChart = new ApexCharts(document.querySelector("#line-chart-canvas"), options);
            LineChart.render();
            console.log(LineChart);
        });
    };
}
