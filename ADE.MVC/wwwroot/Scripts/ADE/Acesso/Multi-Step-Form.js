export class PosRegistroMultiStepFormMini {
    #LastStep = 0;
    #InstituicaoChanged = false;
    #CursoChanged = false;
    constructor(steps, cursoDiv) {
        this.Steps = steps - 1;
        this.CurrentStep = 0;
        this.Tabs = $(".tab-multi-tab");
        this.ControlContainer = document.getElementById("multi-step-control");
        this.StepIndicators = $(".multi-tab-step-indicator");
        this.BtnSave = document.createElement("a");
        this.BtnNext = document.createElement("a");
        this.BtnPrev = document.createElement("a");
        this.BtnVoltar = document.createElement("a");
        this.Instituicao;
        this.Curso;
        this.IdCurso = 0;
        this.CursoDiv = $("#" + cursoDiv);
        this.Error = false;
        this.HasInputError = false;
    }
    Start() {
        this.CreateControls();
        this.ShowCurrentTab();
    }
    NextStep() {
        if (this.CurrentStep ==1 && this.Instituicao === undefined) {
            var toastHTML = '<span>Escolha a sua instituição para prosseguir</span>';
            M.toast({ html: toastHTML });
            return;
        }
        if (this.CurrentStep < this.Steps) {
            this.Step(++this.CurrentStep);
            this.PulseNext("off");
        }
    }
    PreviousStep() {
        //if (this.Instituicao !== undefined) {
            --this.CurrentStep;
            this.Step(this.CurrentStep);
        //}
    }
    Step(step) {
        if (step <= this.Steps) {
            this.CurrentStep = step;
            this.ShowCurrentTab();
        }
        this.HandleButtonBehaviour();
    }
    ShowCurrentTab() {
        this.HandleTabVisibility();
        this.HighlightStepIndicator();
        this.HandleButtonBehaviour();
    }
    async GetCoursesForInstituition() {
        if (this.InstituicaoChanged) {
            let self = this;
            this.ToggleLoadingAnimation();
            console.log(this.Instituicao);
            await $.ajax({
                type: "get",
                url: "/Account/CursosParaInstituicao",
                data: { IdInstituicao: self.Instituicao },
                success: function (data) {
                    self.CursoDiv.html(data);
                    self.InstituicaoChanged = false;
                    self.ToggleLoadingAnimation();
                    $('select').formSelect();
                }
            });
        }
    }
    SetInstituicao(instituicao) {
        this.InstituicaoChanged = true;
        if (instituicao.selectedOptions[0].value !== "") {
            this.Instituicao = instituicao.selectedOptions[0].value;
            this.GetCoursesForInstituition();
            this.PulseNext("on");
        }
    }
    SetCurso(curso) {
        this.CursoChanged = true;
        let self = this;
        if (curso.selectedOptions[0].value !== "") {
            this.IdCurso = curso.selectedOptions[0].value;
            this.PulseNext("on");
            this.BtnSave.className = "h1 badge badge-success multitab-form-btn pulse scale-transition scale-out scale-in";
            this.BtnSave.innerText = "Salvar";
            this.BtnSave.onclick = () => { self.Submit(); };
            this.ControlContainer.appendChild(this.BtnSave);
        }
    }
    PulseNext(type) {
        switch (type) {
            case "on":
                this.BtnNext.className = "h1 badge badge-primary multitab-form-btn btn pulse";
                this.BtnNext.style.padding = "initial !important";
                break;
            case "off":
                this.BtnNext.className = "h1 badge badge-primary multitab-form-btn";
                break;
        }
    }
    HandleTabVisibility() {
        for (i = 0; i <= this.Steps; i++) {
            if (i !== this.CurrentStep) {
                if (this.CurrentStep > i)
                    this.Tabs[i].className = "tab-multi-tab hide-right";
                else
                    this.Tabs[i].className = "tab-multi-tab hide-left";
            }
            else {
                if (this.CurrentStep >= this.Steps)
                    this.Tabs[this.CurrentStep].className += " active";
                else {
                    this.Tabs[this.CurrentStep].className += " active-reverse";
                }
            }
        }
    }
    HandleButtonBehaviour() {
        let self = this;
        if (this.CurrentStep > 0) {
            this.BtnPrev.style.display = "inline-block";
        } else {
            this.BtnPrev.style.display = "none";
        }
        if (this.CurrentStep < this.Steps)
            this.BtnNext.classList.remove("disabled");
        else if (this.CurrentStep === this.Steps)
            this.BtnNext.className += " disabled";

        if (this.CurrentStep === this.Steps) {
            this.BtnSave.className = "h1 badge badge-success multitab-form-btn pulse scale-transition scale-out scale-in";
            this.BtnSave.innerText = "Salvar";
            this.BtnSave.onclick = () => { self.Submit(); };
            this.ControlContainer.appendChild(this.BtnSave);

            this.BtnNext.disabled = true;

            this.BtnPrev.className = "h1 badge badge-primary multitab-form-btn scale-transition scale-in";
            this.BtnPrev.style.display = "inline-block";
        }
        else {
            this.BtnNext.disabled = false;
            this.BtnPrev.className = "h1 badge badge-primary multitab-form-btn scale-transition scale-in";

        }
    }
    HighlightStepIndicator() {
        for (i = 0; i < this.StepIndicators.length; i++) {
            if (i !== this.CurrentStep)
                this.StepIndicators[i].className = "multi-tab-step-indicator";
        }
        this.StepIndicators[this.CurrentStep].className = "multi-tab-step-indicator active";
    }
    CreateControls() {
        let self = this;
        this.BtnNext.id = "Proximo";
        this.BtnNext.innerText = "Próximo";
        this.BtnNext.className = "h1 badge badge-primary multitab-form-btn ";
        this.BtnNext.onclick = () => { self.NextStep(); };

        //this.BtnVoltar.id = "Voltar";
        //this.BtnVoltar.className = "h1 badge badge-light multitab-form-btn ";
        //this.BtnVoltar.innerText = "Voltar";
        //this.BtnVoltar.onclick = () => { window.history.back() };

        this.BtnPrev.id = "Anterior";
        this.BtnPrev.innerText = "Anterior";
        this.BtnPrev.className = "h1 badge badge-primary multitab-form-btn ";
        this.BtnPrev.onclick = () => { self.PreviousStep(); };
        //this.ControlContainer.appendChild(this.BtnVoltar);
        this.ControlContainer.appendChild(this.BtnPrev);
        this.ControlContainer.appendChild(this.BtnNext);
    }
    ToggleLoadingAnimation() {
        $("#loading-animation").toggleClass("collapse");
    }
    ToggleCreationLoadingAnimation() {
        $("#final-loading-animation").toggleClass("collapse");
    }
    async Submit() {
        let self = this;
        if (this.IdCurso === 0) {
            var toastHTML = '<span>Selecione um curso da lista</span>';
            M.toast({ html: toastHTML });
            return;
        }
        this.PulseNext("off");
        //if (this.CheckFormError() === false) {
        this.ToggleCreationLoadingAnimation();
        await $.ajax({
            type: "post",
            url: "/Account/RegistroTemporarioMini",
            data: { IdFaculdade: self.Instituicao, IdCurso: self.IdCurso },
            success: function (data) {
                self.ToggleCreationLoadingAnimation();
                window.location.pathname = "/";
            }
        });
        //}
        //else {
        //    this.HasInputError = true;
        //    toastHTML = '<span>Existem campos vazios!</span>';
        //    M.toast({ html: toastHTML });
        //}
    }
    CheckFormError() {
        let errors = 0;
        for (let i = 0; i < this.Inputs.length; i++) {
            if (this.InputIsNull(this.Inputs[i]) && this.InputIsRequired(this.Inputs[i])) {
                this.Error = true;
                this.AddInputError(this.Inputs[i]);
                errors++;
            } else {
                this.Inputs[i].style.backgroundColor = "white";
            }
        }
        return errors > 0 ? true : false;
    }
    AddInputError(input) {
        if (!this.HasInputError) {
            input.insertAdjacentHTML("beforeBegin", "<p class='text-danger'>*Campo Obrigatório</p>");
        }
    }
    InputIsNull(input) {
        return input.value === undefined || input.value === "" || input.value === null || input.value.length === 0;
    }
    InputIsRequired(input) {
        return input.required === true;
    }
}


export class PosRegistroMultiStepForm {
    #LastStep = 0;
    #InstituicaoChanged = false;
    #CursoChanged = false;
    constructor(steps, cursoDiv, areaCursoDiv) {
        this.Steps = steps;
        this.CurrentStep = 0;
        this.Estagiando = false;
        this.EstagiandoSet = false;
        this.Tabs = $(".tab-multi-tab");
        this.StepIndicators = $(".multi-tab-step-indicator");
        this.ControlContainer = document.getElementById("multi-step-control");
        this.BtnSave = document.createElement("a");
        this.BtnNext = document.createElement("a");
        this.BtnPrev = document.createElement("a");
        this.BtnVoltar = document.createElement("a");
        this.Instituicao;
        this.Curso;
        this.IdCurso = 0;
        this.CursoDiv = $("#" + cursoDiv);
        this.AreaCursoDiv = $("#" + areaCursoDiv);
        this.Error = false;
        this.HasInputError = false;
        this.inputs;
    }
    Start() {
        this.CreateControls();
        this.ShowCurrentTab();
    }
    NextStep() {
        if (this.Instituicao !== undefined && this.CurrentStep < this.Steps - 1) {
            this.Step(++this.CurrentStep);
            this.PulseNext("off");
        }
        else if (this.Instituicao === undefined) {
            var toastHTML = '<span>Escolha a sua instituição e curso para prosseguir</span>';
            M.toast({ html: toastHTML });
        }
    }
    PreviousStep() {
        if (this.Instituicao !== undefined) {
            --this.CurrentStep;
            this.Step(this.CurrentStep);
        }
    }
    Step(step) {
        if (this.Instituicao !== undefined && step <= this.Steps) {
            this.CurrentStep = step;
            this.ShowCurrentTab();
        }
        this.HandleButtonBehaviour();
    }
    ShowCurrentTab() {
        this.HandleTabVisibility();
        this.HighlightStepIndicator();
        this.HandleButtonBehaviour();
    }
    async GetCoursesForInstituition() {
        if (this.InstituicaoChanged) {
            let self = this;
            this.ToggleLoadingAnimation();
            console.log(this.Instituicao);
            await $.ajax({
                type: "get",
                url: "/Account/CursosParaInstituicao",
                data: { IdInstituicao: self.Instituicao },
                success: function (data) {
                    self.CursoDiv.html(data);
                    self.InstituicaoChanged = false;
                    self.ToggleLoadingAnimation();
                    $('select').formSelect();
                }
            });
        }
    }
    async GetAreasForCourse() {
        if (this.CursoChanged) {
            let self = this;
            this.ToggleLoadingAnimation();
            console.log(this.IdCurso);
            await $.ajax({
                type: "get",
                url: "/Account/AreasEstagioParaCurso",
                data: { IdCurso: self.IdCurso },
                success: function (data) {
                    self.AreaCursoDiv.html(data);
                    self.CursoChanged = false;
                    self.ToggleLoadingAnimation();
                    self.PulseNext("on");
                    $('select').formSelect();
                }
            });
        }
    }
    SetEstagiando(value) {
        this.Estagiando = value;
        this.EstagiandoSet = true;
    }
    SetInstituicao(instituicao) {
        this.InstituicaoChanged = true;
        if (instituicao.selectedOptions[0].value !== "") {
            this.Instituicao = instituicao.selectedOptions[0].value;
            this.GetCoursesForInstituition();
            this.PulseNext("on");
        }
    }
    SetCurso(curso) {
        this.CursoChanged = true;
        if (curso.selectedOptions[0].value !== "") {
            this.IdCurso = curso.selectedOptions[0].value;
            //this.GetAreasForCourse();
            this.PulseNext("off");
        }
    }
    PulseNext(type) {
        switch (type) {
            case "on":
                this.BtnNext.className = "h1 badge badge-primary multitab-form-btn btn pulse";
                this.BtnNext.style.padding = "initial !important";
                break;
            case "off":
                this.BtnNext.className = "h1 badge badge-primary multitab-form-btn";
                break;
        }
    }
    HandleTabVisibility() {
        for (i = 0; i < this.Steps; i++) {
            if (i !== this.CurrentStep) {
                if (this.CurrentStep > i)
                    this.Tabs[i].className = "tab-multi-tab hide-right";
                else
                    this.Tabs[i].className = "tab-multi-tab hide-left";
            }
            else {
                if (this.CurrentStep >= this.LastStep)
                    this.Tabs[this.CurrentStep].className += " active";
                else {
                    this.Tabs[this.CurrentStep].className += " active-reverse";
                }
            }
        }
    }
    HandleButtonBehaviour() {
        this.BtnPrev.disabled = this.CurrentStep > 0 ? false : true;

        if (this.CurrentStep < this.Steps - 1)
            this.BtnNext.classList.remove("disabled");
        else if (this.CurrentStep === this.Steps - 1)
            this.BtnNext.className += " disabled";

        if (this.CurrentStep === this.Steps - 1) {
            this.BtnSave.className = "h1 badge badge-success multitab-form-btn pulse scale-transition scale-out scale-in";
            this.BtnSave.innerText = "Salvar";
            this.BtnSave.onclick = () => { this.Submit(); };
            this.ControlContainer.appendChild(this.BtnSave);

            this.BtnNext.disabled = true;

            this.BtnPrev.className = "h1 badge badge-primary multitab-form-btn scale-transition scale-in";
            this.BtnPrev.style.display = "inline-block";
        }
        else {
            this.BtnNext.disabled = false;
            this.BtnPrev.className = "h1 badge badge-primary multitab-form-btn scale-transition scale-in";
            setTimeout(() => { this.BtnPrev.style.display = "none"; }, 500);

        }
    }
    HighlightStepIndicator() {
        for (i = 0; i < this.StepIndicators.length; i++) {
            if (i !== this.CurrentStep)
                this.StepIndicators[i].className = "multi-tab-step-indicator";
        }
        this.StepIndicators[this.CurrentStep].className = "multi-tab-step-indicator active";
    }
    CreateControls() {
        this.BtnNext.innerText = "Próximo";
        this.BtnVoltar.innerText = "Voltar";
        this.BtnPrev.innerText = "Anterior";
        this.BtnPrev.className = "h1 badge badge-primary multitab-form-btn ";
        this.BtnPrev.onclick = () => { this.PreviousStep(); };
        this.BtnNext.className = "h1 badge badge-primary multitab-form-btn ";
        this.BtnVoltar.className = "h1 badge badge-light multitab-form-btn ";
        this.BtnNext.onclick = () => { this.NextStep(); };
        this.BtnVoltar.onclick = () => { window.history.back() };
        this.ControlContainer.appendChild(this.BtnVoltar);
        this.ControlContainer.appendChild(this.BtnPrev);
        this.ControlContainer.appendChild(this.BtnNext);
    }
    ToggleLoadingAnimation() {
        $("#loading-animation").toggleClass("collapse");
    }
    ToggleCreationLoadingAnimation() {
        $("#final-loading-animation").toggleClass("collapse");
    }
    async Submit() {
        let self = this;
        this.Inputs = $("#DadosAluno")[0].elements;
        if (this.IdCurso === 0) {
            var toastHTML = '<span>Selecione um curso da lista</span>';
            M.toast({ html: toastHTML });
        }
        this.PulseNext("off");
        if (this.CheckFormError() === false) {
            this.ToggleCreationLoadingAnimation();
            console.log({ IdFaculdade: self.Instituicao, IdCurso: self.IdCurso });
            await $.ajax({
                type: "post",
                url: "/Account/RegistroTemporario",
                data: { IdFaculdade: self.Instituicao, IdCurso: self.IdCurso },
                success: function (data) {
                    self.ToggleCreationLoadingAnimation();
                    window.location.pathname = "/";
                }
            });
        }
        else {
            this.HasInputError = true;
            toastHTML = '<span>Existem campos vazios!</span>';
            M.toast({ html: toastHTML });
        }
    }
    CheckFormError() {
        let errors = 0;
        for (let i = 0; i < this.Inputs.length; i++) {
            if (this.InputIsNull(this.Inputs[i]) && this.InputIsRequired(this.Inputs[i])) {
                this.Error = true;
                this.AddInputError(this.Inputs[i]);
                errors++;
            } else {
                this.Inputs[i].style.backgroundColor = "white";
            }
        }
        return errors > 0 ? true : false;
    }
    AddInputError(input) {
        if (!this.HasInputError) {
            input.insertAdjacentHTML("beforeBegin", "<p class='text-danger'>*Campo Obrigatório</p>");
        }
    }
    InputIsNull(input) {
        return input.value === undefined || input.value === "" || input.value === null || input.value.length === 0;
    }
    InputIsRequired(input) {
        return input.required === true;
    }
}