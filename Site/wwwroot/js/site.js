﻿(function () {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
})()

function ShowAlert(message) {
    $.ajax({
        url: _caminhoSite + 'Alert/LoadModal',
        cache: false,
        type: 'get',
        dataType: 'html',
        data: { message: message },
        success: function (result) {
            $('#divModalAlert').html(result);
            $('#modalAlert').modal('show');
        }
    });
}

$(document).ready(function () {
    $(document).on('click', '#btnDesconectar', function (e) {
        e.preventDefault();
        Disconnect();
    });
});

function Disconnect() {
    $('#divAguarde').show();

    $.ajax({
        url: _caminhoSite + 'Login/Disconnect',
        async: true,
        cache: false,
        type: 'post',
        dataType: 'json',
        success: function (result) {
            window.location.href = _caminhoSite;
        }
    });
}
