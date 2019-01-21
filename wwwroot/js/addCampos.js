function addCampos() {
    var opts = "";
    var i = 0;
    var NewSelect = "";
    for (var i = 0; i < DadosRequisitos.length; i++) {
        var titulo = document.createElement("p");
        titulo.setAttribute("class", "titulos");
        if (DadosRequisitos[i].Descricao != null)
            titulo.setAttribute("title", DadosRequisitos[i].Descricao);
        var spaces = (DadosRequisitos[i].NomeRequisito).replace(/-/i, ' ');
        titulo.innerHTML = spaces;
        var campo = document.createElement(DadosRequisitos[i].Tag);

        campo.setAttribute("name", DadosRequisitos[i].IdCampo);
        campo.setAttribute("type", DadosRequisitos[i].Tipo);
        
        if (DadosRequisitos[i].Descricao != null)
            campo.setAttribute("placeholder", DadosRequisitos[i].Descricao);
        campo.setAttribute("id", DadosRequisitos[i].IdCampo);
        campo.setAttribute("class", "input-campos " + DadosRequisitos[i].ClassCampo);


        campoRequisitos.appendChild(titulo);
        if (DadosRequisitos[i].Tipo == "checkbox" && DadosRequisitos[i].Opcoes != "" && DadosRequisitos[i].Tag != "select") {

            var ListaOpts = DadosRequisitos[i].Opcoes.split(',');

            for (var x = 0; x < ListaOpts.length; x++) {
                var tituloCheckbox = document.createElement("p");
                tituloCheckbox.setAttribute("class", "tituloCB");
                if (DadosRequisitos[i].Descricao != null)
                    tituloCheckbox.setAttribute("title", DadosRequisitos[i].Descricao);
                tituloCheckbox.setAttribute("id", ListaOpts[x]);
                tituloCheckbox.innerHTML = ListaOpts[x];



                var opts = document.createElement('input');
                opts.setAttribute("type", DadosRequisitos[i].Tipo);
                opts.setAttribute("name", ListaOpts[x]);
                // opts.setAttribute("value",ListaOpts[x]);
                if (DadosRequisitos[i].Descricao != null)
                    opts.setAttribute("placeholder", DadosRequisitos[i].Descricao);
                campoRequisitos.appendChild(opts);
                campoRequisitos.appendChild(tituloCheckbox);
            }

        }
        if (DadosRequisitos[i].Tag == "select" && DadosRequisitos[i].Opcoes != "") {

            var ListaOpts = DadosRequisitos[i].Opcoes.split(',');
            NewSelect = document.createElement('select');
            if (DadosRequisitos[i].Descricao != null)
                NewSelect.setAttribute("title", DadosRequisitos[i].Descricao);
            NewSelect.setAttribute("id", DadosRequisitos[i].IdCampo);
            NewSelect.setAttribute("name", DadosRequisitos[i].IdCampo);
            NewSelect.setAttribute("value", ListaOpts[x]);
            NewSelect.setAttribute("class", DadosRequisitos[i].ClassCampo);
            for (var x = 0; x < ListaOpts.length; x++) {
                //var tituloCheckbox = document.createElement("p");
                //tituloCheckbox.setAttribute("class","tituloCB");

                var selectOpts = document.createElement("option");
                selectOpts.text = ListaOpts[x];
                selectOpts.setAttribute("name", DadosRequisitos[i].IdCampo);
                selectOpts.setAttribute("value", ListaOpts[x]);

                NewSelect.add(selectOpts);
                campoRequisitos.appendChild(NewSelect);
                //campoRequisitos.appendChild(tituloCheckbox);
            }

        }


        if (DadosRequisitos[i].Tipo != "checkbox")
            campoRequisitos.appendChild(campo);

    }
}

