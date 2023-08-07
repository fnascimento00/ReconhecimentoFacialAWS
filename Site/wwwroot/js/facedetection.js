let captureActive = true;
const scale = 1.5;

async function faceDetection(overlayCanvas, video, progressBar) {
    faceapi.matchDimensions(overlayCanvas, video, true);

    const detections = await faceapi.detectAllFaces(video, new faceapi.TinyFaceDetectorOptions())
        .withFaceLandmarks();

    const context = overlayCanvas.getContext('2d');
    context.clearRect(0, 0, overlayCanvas.width, overlayCanvas.height);

    if (detections.length > 0) {
        const faceRect = detections[0].detection.box;
        rectangleOverFace(context, faceRect);
        //expressionLines(context, detections);
        return checkFaceApproach(faceRect, progressBar);
    }
}

function rectangleOverFace(context, faceRect) {
    const enlargedWidth = faceRect.width * scale;
    const enlargedHeight = faceRect.height * scale;
    const enlargedX = (faceRect.x - (enlargedWidth - faceRect.width) / 2);
    const enlargedY = (faceRect.y - (enlargedHeight - faceRect.height) / 2) + 10;

    //Desenha o retângulo delimitador no canvas
    context.beginPath();
    context.setLineDash([5, 3]);
    context.rect(enlargedX, enlargedY, enlargedWidth, enlargedHeight);
    context.lineWidth = 2;
    context.strokeStyle = 'white';
    context.stroke();
}

function expressionLines(context, detections) {
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

function checkFaceApproach(faceRect, progressBar) {
    if (!captureActive) return false;

    const referenceSize = 210;
    const faceSize = Math.max(faceRect.width, faceRect.height);
    let percent = Math.round((faceSize * 100) / referenceSize);
    percent = percent > 100 ? 100 : percent;
    drawProgressBar(progressBar, percent);

    if (faceSize > referenceSize) return true;

    return false;
}

function drawProgressBar(progressBar, percent) {
    progressBar.style = `width: ${percent}%`;
    progressBar.setAttribute("aria-valuenow", percent);
    progressBar.innerHTML = percent + "%";
}

async function getImage(video, detectionOptions) {
    const detections = await faceapi.detectSingleFace(video, detectionOptions)
        .withFaceLandmarks()
        .withFaceDescriptor();

    if (detections) {
        const faceRect = detections.detection.box;

        const enlargedWidth = faceRect.width * scale;
        const enlargedHeight = faceRect.height * scale;
        const enlargedX = (faceRect.x - (enlargedWidth - faceRect.width) / 2);
        const enlargedY = (faceRect.y - (enlargedHeight - faceRect.height) / 2) - 40;

        const canvas = document.createElement('canvas');
        canvas.width = enlargedWidth;
        canvas.height = enlargedHeight;

        const context = canvas.getContext('2d');
        context.drawImage(video, enlargedX, enlargedY, enlargedWidth, enlargedHeight, 0, 0, canvas.width, canvas.height);

        return canvas.toDataURL('image/png');
    }

    return null;
}