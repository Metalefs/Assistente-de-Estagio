var open = false;

function MostraPreenchimento(){
    if(userId == null && open==false){
          getElementById("interface-preenchimento-popup").className += "show";
    } 
}
getElementById("body").addEventListener('click', function (evt) {

    if (evt.id == "Main-Container" || evt.id == "Side-Container") {
        open = false;
    }
});

function Download(idDocumento,userId) {


    if (userId != null) {
        $.ajax({
            type: 'post',
            dataType: 'text',
            url: serviceURL,
            data: {
                userId: userId,
                idDocumento: idDocumento
            },
            success: function () {

            }
        });
    } else {
        MostraPreenchimento();
    }
}
