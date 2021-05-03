$(document).ready(function () {

    let x;
    let y;
    const canvas = document.getElementById('logo-canvas');
    const c = canvas.getContext('2d');
    let container = document.getElementById("left-main");
    canvas.height = container.offsetHeight;
    canvas.width = container.clientWidth;
   
    const draw = () => {

        // earth color scheme
        var earth = [
            'rgba(0, 38, 28, x)',
            'rgba(4, 76, 41, x)',
            'rgba(22, 127, 57, x)',
            'rgba(69, 191, 85, x)',
            'rgba(150, 237, 137, x)',
        ];

        // fire color scheme
        var fire = [
            'rgba(242, 193, 102, x)',
            'rgba(242, 134, 39, x)',
            'rgba(217, 63, 7, x)',
            'rgba(140, 29, 4, x)',
            'rgba(65, 15, 4, x)',
        ];

        // water color scheme
        var water = [
            'rgba(0, 48, 90, x)',
            'rgba(0, 75, 141, x)',
            'rgba(0, 116, 217, x)',
            'rgba(65, 147, 217, x)',
            'rgba(122, 186, 242, x)',
        ];

        // collection of earth, fire and water
        var colorArray = [earth, fire, water];

        // current color accessor
        var colors = colorArray[0];

        function Circle(x, y, vx, vy, r, growth) {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
            this.r = r;
            this.color = colors[Math.floor(Math.random() * colors.length)];
            this.alpha = 1;

            this.draw = function () {
                c.beginPath();
                c.strokeStyle = this.color.replace('x', + this.alpha);
                c.arc(this.x, this.y, this.r, Math.PI * 2, false);
                c.lineWidth = 2;
                c.stroke();
                c.fillStyle = 'transparent';
                c.fill();
            }

            this.update = function () {
                this.x += this.vx;
                this.y += this.vy;
                this.alpha -= 0.015;
                this.r += growth;
                this.draw();
            }
        }

        // array of circles
        var circles = [];

        // mouse position
        var mouse = {
            x: undefined,
            y: undefined
        };

        // on mousemovement, update coordinates of mouse
       

        // change color scheme on click and rotate colorArray
        window.addEventListener('click', function () {
            colorArray.push(colorArray.shift());
            colors = colorArray[0];
            circles.push(new Circle(mouse.x, mouse.y, 0, 0, 2, 5));
        });

        // ensure canvas is always full size of browser window
        window.addEventListener('resize', function () {
            canvas.width = innerWidth;

        });
        window.addEventListener("mousemove", (evt) => {
            mouse.x = evt.x;
            mouse.y = evt.y - 53;
        });

        function animate() {
            window.requestAnimationFrame(animate);
            c.clearRect(0, 0, innerWidth, innerHeight);

            var vx = (Math.random() - 0.5) * 5 + (Math.random() < 0.5 ? -2 : 2);
            var vy = (Math.random() - 0.5) * 5 + (Math.random() < 0.5 ? -2 : 2);
            var r = Math.random() * 20 + 30;
            //circles.push(new Circle(mouse.x, mouse.y, vx, vy, r, -0.5));

            for (let i = 0; i < circles.length; ++i) {
                circles[i].update();
                // remove the circle if it is transparent or too small
                if (circles[i].alpha < 0 || circles[i].r < 3 || circles.length > 2) {
                    circles.splice(i, 1);
                }
            }
           
        }

        animate();

    };
    draw();
});