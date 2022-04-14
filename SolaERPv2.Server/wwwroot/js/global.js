function downloadFile(fileName, base64String) {
    const url = "data:application/octet-stream;base64," + base64String;
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}