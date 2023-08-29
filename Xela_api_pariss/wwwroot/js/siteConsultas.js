'use strict'

var getUrl = window.location;
var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

$(document).on('click', '.btninsertContact', function () {
    var Nombre = document.getElementById('Nombre').value;
    var Email = document.getElementById('Email').value;
    var Titulo = document.getElementById('Titulo').value;
    var Mensaje = document.getElementById('Mensaje').value;
    var pa = { 'Nombre': Nombre, 'Email': Email, 'Titulo': Titulo, 'Mensaje': Mensaje };

    var par = JSON.stringify(pa);
    var t = $("input[name='__RequestVerificationToken']").val();
    $.ajax({
        headers: {
            "RequestVerificationToken": t,
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8'
        },
        type: "POST",
        url: baseUrl + "Usuarios/InsertarContact",
        data: par,
        processData: false,
        contentType: false,
        success: function (data, textStatus, jqXHR) {
            if (data === 'sininformacion') {
                Swal.fire({
                    position: 'top-end',
                    icon: 'info',
                    title: 'No se encuentra información',
                    showConfirmButton: false,
                    timer: 2500
                });
            } else {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Su mensaje fue registrado correctamente',
                    showConfirmButton: false,
                    timer: 2500
                }).then((result) => {
                    if (result) {
                        location.reload();
                    }
                });
            }
        },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.status, errorThrown);
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'Tiempo de Respuesta insuficiente...Por favor Vuelva a generar la consulta',
                    showConfirmButton: false,
                    timer: 3000
                });
            }
        });
});
