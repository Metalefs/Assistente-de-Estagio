

function createDocument() {

    var errors = document.getElementById("errors");
    var error = false;
    var thisInput = document.getElementById("campos-documento").elements;
    var errorText = document.createElement("p");
    var dadosAlunoTxt = new Array();
    for (var i = 0; i < thisInput.length; i++) {
        dadosAlunoTxt[i] = thisInput[i].value;
    }
    for (var i = 0; i < thisInput.length; i++) {
        // if(thisInput[i].type != "checkbox"){
        if (thisInput[i].value == undefined || thisInput[i].value == "" || thisInput[i].value == null) {
            error = true;
            thisInput[i].style.backgroundColor = "#ffc1c1";
        } else {
            thisInput[i].style.backgroundColor = "white";
        }
        //}
    }
    //Test if had any error
    if (error == true && accept == false) {
        errorText.innerHTML = "Existem Campos vazios! corrija se quiser, caso contrario continue.";
        errors.appendChild(errorText);
        accept = true;
    }
    else if (accept == true || error == false) {
        var dadosAluno = JSON.stringify($(campoRequisitos).serializeArray());

        if (accept == true)
            errorText.innerHTML = "Gerando documento com campos vazios";
        if (accept == false)
            errorText.innerHTML = "Seu download iniciará em alguns instantes";
        $.ajax({
            type: 'post',
            dataType: 'text',
            url: serviceURL,
            data: {
                dados: dadosAluno,
                idDocumento: idDocumento
            },
            success: function (msg) {
                alert(msg);
            }
        });
    }
}