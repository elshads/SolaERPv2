//function downloadFile(fileName, base64String) {
//    const url = "data:application/octet-stream;base64," + base64String;
//    const anchorElement = document.createElement('a');
//    anchorElement.href = url;
//    anchorElement.download = fileName ?? '';
//    anchorElement.click();
//    anchorElement.remove();
//}

async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}