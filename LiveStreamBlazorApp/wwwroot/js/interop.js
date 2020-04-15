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
        var player = videojs(elementid, { "controls": true, "preload": "none"});
    }
}

function loadPlayer(elementid, url) {
    var player = videojs(elementid);
    player.src({
        src: url,
        type: 'application/x-mpegURL'
    });
}



//function InitPage() {
//    $(function () {
//        var video = videojs('previewPlayer').click(function () {
//      ...code to execute on a button click...
//    });
//});
//}