export function UsuarioCursoPizzaChart(alpha) {
    this.alpha = alpha;

    var earth = [
        'rgba(0, 38, 28, ' + this.alpha + ')',
        'rgba(4, 76, 41, ' + this.alpha + ')',
        'rgba(22, 127, 57, ' + this.alpha + ')',
        'rgba(69, 191, 85, ' + this.alpha + ')',
        'rgba(150, 237, 137, ' + this.alpha + ')'
    ];

    var fire = [
        'rgba(242, 193, 102, ' + this.alpha + ')',
        'rgba(242, 134, 39, ' + this.alpha + ')',
        'rgba(217, 63, 7, ' + this.alpha + ')',
        'rgba(140, 29, 4, ' + this.alpha + ')',
        'rgba(65, 15, 4, ' + this.alpha + ')'
    ];

    var water = [
        'rgba(0, 48, 90, ' + this.alpha + ')',
        'rgba(0, 75, 141, ' + this.alpha + ')',
        'rgba(0, 116, 217, ' + this.alpha + ')',
        'rgba(65, 147, 217, ' + this.alpha + ')',
        'rgba(122, 186, 242, ' + this.alpha + ')'
    ];
    this.colors = [];
    for (let i = 0; i < 5; i++) {
        this.colors.push(earth[i]);
        this.colors.push(fire[i]);
        this.colors.push(water[i]);
    }


    let UsuariosPorCurso = async function () {
        return await $.ajax({
            type: "get",
            url: "/Administracao/Dashboard/UsuariosPorCursoJson",
            success: function (data) {
                let UsuariosPorCursoDataSet = JSON.parse(data);
                return UsuariosPorCursoDataSet;
            }
        });
    };

    this.CreatePizzaChart = async function () {
        var ctx = document.getElementById('pizza-chart-canvas').getContext('2d');
        let pizzaColors = [];
        await UsuariosPorCurso().then((dataset) => {
            try {
                dataset = JSON.parse(dataset);
                dataset.Values.forEach(() => {
                    let color = this.colors[Math.floor(Math.random() * this.colors.length)];

                    if (pizzaColors.includes(color)) {
                        while (pizzaColors.includes(color)) {
                            color = this.colors[Math.floor(Math.random() * this.colors.length)];
                            pizzaColors.push(color);
                        }
                    } else {
                        pizzaColors.push(color);
                    }

                });
            }
            catch (err) {
                console.log(err);
            }
            var config = {
                type: 'pie',
                data: {
                    datasets: [{
                        data: dataset.Values,
                        backgroundColor: pizzaColors,
                        label: 'Quantidade de usuarios por curso'
                    }],
                    labels: dataset.Labels
                },
                options: {
                    responsive: true
                }
            };
            var PieChart = new Chart(ctx, config);
            console.log(PieChart);
        });
    };
}
