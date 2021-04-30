export async function get(url, data) {
    return await $.ajax({
        type: "get",
        url: url,
        data: data,
        success: function (data) {
        },
        error: function (data) {
            AddError(data);
        }
    });
    function AddError(texto) {
        var toastHTML = '<span>' + Object.values(texto)[0]+ '!</span>';
        M.toast({ html: toastHTML });
    }
}

export async function post(url, data, handler) {
    let self = this;
    await $.ajax({
        type: "post",
        url: url,
        data: data,
        success: function (data) {
            handler(data, url);
        },
        error: function (data) {
            handler(data, url);
        }
    });
}

export function HandleDefault(result, url) {
    console.log(result);
    try {
        if (result.hasOwnProperty("sucesso")) {
            let toastHTML = '<span>' + result.sucesso + '</span>';
            M.toast({ html: toastHTML, classes: 'bg-success' });
        }
        if (result.hasOwnProperty("erro")) {
            let toastHTML = '<span>' + result.erro + '</span>';
            M.toast({ html: toastHTML, classes: 'bg-danger' });
        }
    } catch (ex) {
        console.log(url, result);
        let toastHTML = '<span>' + Object.values(result)[0] + '</span>';
        M.toast({ html: toastHTML, classes: 'bg-danger' });
    }
    return;
}