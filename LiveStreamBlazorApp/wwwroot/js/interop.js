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

function previewPlayer(videoplayer) {
    videoplayer.fluid(true);
    videoplayer.control(true);
}

window.videoplayer = {
    previewPlayer: function (elementid) {
        var video = videojs(elementid, {
            loop: 'true',
            name: 'playername'
        });
    }
}



//function InitPage() {
//    $(function () {
//        var video = videojs('previewPlayer').click(function () {
//      ...code to execute on a button click...
//    });
//});
//}