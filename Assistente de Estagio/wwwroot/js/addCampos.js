function addCampos(){
            var opts = "";
            var i=0;
            var NewSelect="";
            for (var i=0;i<DadosRequisitos.length;i++){
                var titulo = document.createElement("h4");
                titulo.setAttribute("class","titulos");
                if(DadosRequisitos[i][0].Descricao!=null)
                titulo.setAttribute("title",DadosRequisitos[i][0].Descricao);
                var spaces = (DadosRequisitos[i][0].NomeRequisito).replace(/-/i,' ');
                titulo.innerHTML = spaces;
                var campo = document.createElement(DadosRequisitos[i][0].tag);
                
                campo.setAttribute("name",DadosRequisitos[i][0].NomeRequisito);
                campo.setAttribute("type",DadosRequisitos[i][0].Tipo);
                if(DadosRequisitos[i][0].Descricao!=null)
                campo.setAttribute("placeholder",DadosRequisitos[i][0].Descricao);
                campo.setAttribute("id",DadosRequisitos[i][0].NomeRequisito);
                campo.setAttribute("class","input-campos");
                
                
                campoRequisitos.appendChild(titulo);
                if(DadosRequisitos[i][0].Tipo == "checkbox" && DadosRequisitos[i][0].Opcoes != "" && DadosRequisitos[i][0].tag != "select"){
                    
                    var ListaOpts = DadosRequisitos[i][0].Opcoes.split(',');
                    
                    for (var x=0; x<ListaOpts.length; x++){
                            var tituloCheckbox = document.createElement("p");
                            tituloCheckbox.setAttribute("class","tituloCB");
                            if(DadosRequisitos[i][0].Descricao!=null)
                            tituloCheckbox.setAttribute("title",DadosRequisitos[i][0].Descricao);
                            tituloCheckbox.setAttribute("id",ListaOpts[x]);
                            tituloCheckbox.innerHTML = ListaOpts[x];
                    
                       
                            
                            var opts = document.createElement('input');
                            opts.setAttribute("type",DadosRequisitos[i][0].Tipo);
                            opts.setAttribute("name",ListaOpts[x]);
                           // opts.setAttribute("value",ListaOpts[x]);
                            if(DadosRequisitos[i][0].Descricao!=null)
                            opts.setAttribute("placeholder",DadosRequisitos[i][0].Descricao);
                            campoRequisitos.appendChild(opts);
                            campoRequisitos.appendChild(tituloCheckbox);
                    }
                    
                }
                if(DadosRequisitos[i][0].tag == "select" && DadosRequisitos[i][0].Opcoes != ""){
                    
                    var ListaOpts = DadosRequisitos[i][0].Opcoes.split(',');
                    NewSelect = document.createElement('select');
                    if(DadosRequisitos[i][0].Descricao!=null)
                            NewSelect.setAttribute("title",DadosRequisitos[i][0].Descricao);
                            NewSelect.setAttribute("id",ListaOpts[x]);
                            NewSelect.setAttribute("name",DadosRequisitos[i][0].NomeRequisito);
                            NewSelect.setAttribute("value",ListaOpts[x]);
                    for (var x=0; x<ListaOpts.length; x++){
                            //var tituloCheckbox = document.createElement("p");
                            //tituloCheckbox.setAttribute("class","tituloCB");
    
                            var selectOpts = document.createElement("option");
                            selectOpts.text = ListaOpts[x];
                            selectOpts.setAttribute("name",DadosRequisitos[i][0].NomeRequisito);
                            selectOpts.setAttribute("value",ListaOpts[x]);
                            
                            NewSelect.add(selectOpts);
                            campoRequisitos.appendChild(NewSelect);
                            //campoRequisitos.appendChild(tituloCheckbox);
                    }
                    
                }
                
                
                if(DadosRequisitos[i][0].Tipo != "checkbox")
                campoRequisitos.appendChild(campo);
                
            }
        }
        
        