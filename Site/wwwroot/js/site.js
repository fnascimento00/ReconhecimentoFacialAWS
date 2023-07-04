(function () {
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

function RectangleOverFace(context, faceRect, scale) {
    const enlargedWidth = faceRect.width * scale;
    const enlargedHeight = faceRect.height * scale;
    const enlargedX = faceRect.x - (enlargedWidth - faceRect.width) / 2;
    const enlargedY = (faceRect.y - (enlargedHeight - faceRect.height) / 2) - 40;

    //Desenha o retângulo delimitador no canvas
    context.beginPath();
    context.rect(enlargedX, enlargedY, enlargedWidth, enlargedHeight);
    context.lineWidth = 3;
    context.strokeStyle = 'blue';
    context.stroke();
}

function ExpressionLines(context, detections) {
    // Verifica se há landmarks
    if (detections[0].landmarks) {
        const landmarks = detections[0].landmarks.positions;

        // Define o estilo das linhas de expressão
        context.strokeStyle = 'red';
        context.setLineDash([2, 2]); // Define o estilo pontilhado

        // Desenha as linhas de expressão
        context.beginPath();
        context.moveTo(landmarks[0].x, landmarks[0].y);
        for (let i = 1; i < landmarks.length; i++) {
            context.lineTo(landmarks[i].x, landmarks[i].y);
        }
        context.closePath();
        context.stroke();
    }
}
