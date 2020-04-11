function copyToClipboard(id) {
    const element = document.getElementById(id);
    if (element != null) {
        element.select();
        document.execCommand('copy');
        alert("Copied the text: " + element.value);
    } else {
        alert("Element does not exist");
    }
}

function createAlert(text) {
    alert(text);
}