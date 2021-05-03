export class TourHub {
    #defaultTemplate = `
    <div class='popover tour'>
        <div class='arrow'></div>
        <h3 class='popover-title'></h3>
        <div class='popover-content'></div>
        <div class='popover-navigation'>
            <button class='btn btn-default' data-role='prev'>« Anterior</button>
            <span data-role='separator'>|</span>
            <button class='btn btn-default' data-role='next'>Próximo »</button>
        </div>
        <button class='btn btn-default' data-role='end'>Fechar</button>
    </div>`;
    constructor() {
        this.tour = null;
        this.steps = null;
    };

    async retrieveSteps() {
        return await $.ajax({
            type: "get",
            url: "/CardTour/ObterCardTourPagina",
            data: { pathname: window.location.pathname },
            success: function (data) {
            }
        });
    }

    async retrieveStepsForPage(urlpage) {
        return await $.ajax({
            type: "get",
            url: "/CardTour/ObterCardTourPagina",
            data: { pathname: urlpage },
            success: function (data) {
            }
        });
    }

    async Start(url = "") {
        let steps = url === "" ? await this.retrieveSteps() : await this.retrieveStepsForPage(url);
        console.log(steps);
        steps = JSON.parse(steps);
        this.tour = new Tour({
            name: "tour",
            steps: steps,
            container: "body",
            smartPlacement: true,
            keyboard: true,
            storage: window.localStorage,
            debug: false,
            backdrop: false,
            backdropContainer: 'body',
            backdropPadding: 0,
            redirect: true,
            orphan: false,
            duration: false,
            delay: false,
            basePath: "",
            template: `<div class='mt-5 popover tour'>
                            <div class='arrow'></div>
                            <h3 class='popover-title card-title h3-responsive'></h3>
                            <div class='popover-content'></div>
                            <div class='popover-navigation'>
                                <button class='btn btn-default' data-role='prev'>« Anterior</button>
                                <span data-role='separator'>|</span>
                                <button class='btn btn-default' data-role='next'>Próximo »</button>
                            </div>
                            <button class='btn btn-default' data-role='end'>Fechar</button>
                        </div>`,
            afterGetState: function (key, value) { },
            afterSetState: function (key, value) { },
            afterRemoveState: function (key, value) { },
            onStart: function (tour) { },
            onEnd: function (tour) { sessionStorage.setItem("tour" + window.location.pathname, "true"); },
            onShow: function (tour) { },
            onShown: function (tour) { },
            onHide: function (tour) { },
            onHidden: function (tour) { },
            onNext: function (tour) { },
            onPrev: function (tour) { },
            onPause: function (tour, duration) { },
            onResume: function (tour, duration) { },
            onRedirectError: function (tour) { }
        });
        this.tour.restart();
        this.tour.init();
        this.tour.start();
        if (sessionStorage.getItem("tour" + window.location.pathname) !== "true") {
            this.tour.restart();
        }
    }

}
