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

window.initVideojs = initVideojs;

function initVideojs() {
    var video = videojs(document.getElementById('video'), {}, function () {
        console.log("Video loaded");
    });
}


function startButton(btnStartElement) {
    btnStartElement.click();
}

window.videoplayer = {
    oninit: function () {
        let vid = document.getElementById('video');
        var video = videojs(vid);
        video.reloadSourceOnError();
    }
}


window.webcam = {
    oninit: function () {

        var msg = "hello"

        function getmsg() {
            console.log(msg);
        }

        var func = getmsg();

        let start = document.getElementById('btnStart');
        let stop = document.getElementById('btnStop');
        let vidSave = document.getElementById('vidSave');

        let constraintObj = {
            audio: true,
            video: {
                facingMode: "user",
                width: { min: 640, ideal: 960, max: 1920 },
                height: { min: 480, ideal: 540, max: 1080 }
            }
        };

        // width: 1280, height: 720  -- preference only
        // facingMode: {exact: "user"}
        // facingMode: "environment"

        //handle older browsers that might implement getUserMedia in some way
        if (navigator.mediaDevices === undefined) {
            navigator.mediaDevices = {};
            navigator.mediaDevices.getUserMedia = function (constraintObj) {
                let getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
                if (!getUserMedia) {
                    return Promise.reject(new Error('getUserMedia is not implemented in this browser'));
                }
                return new Promise(function (resolve, reject) {
                    getUserMedia.call(navigator, constraintObj, resolve, reject);
                });
            }
        } else {
            navigator.mediaDevices.enumerateDevices()
                .then(devices => {
                    devices.forEach(device => {
                        console.log(device.kind.toUpperCase(), device.label);
                        //, device.deviceId
                    })
                })
                .catch(err => {
                    console.log(err.name, err.message);
                })
        }

        navigator.mediaDevices.getUserMedia(constraintObj)
            .then(function (mediaStreamObj) {
                //connect the media stream to the first video element
                let video = document.getElementById('vidInput');
                if ("srcObject" in video) {
                    video.srcObject = mediaStreamObj;
                } else {
                    //old version
                    video.src = window.URL.createObjectURL(mediaStreamObj);
                }

                video.onloadedmetadata = function (ev) {
                    //show in the video element what is being captured by the webcam
                    video.play();
                };

                //add listeners for saving video/audio

                let mediaRecorder = new MediaRecorder(mediaStreamObj);
                let chunks = [];

                let mediaLiveRecorder;

                start.addEventListener('click', (ev) => {
                    mediaRecorder.start();
                    console.log(mediaRecorder.state);
                    var streaminfo = {
                        "id": "12345",
                        "secure_stream_url": "no link",
                        "stream_url": "rtmp://139.162.24.99/live/test"
                    };

                    console.log(streaminfo);
                    console.log(window.location.protocol)

                    const ws = new WebSocket("ws://localhost:3000/rtmp/rtmp%3A%2F%2F139.162.24.99%2Flive%2Ftest");

                    ws.addEventListener('open', (e) => {
                        console.log('Websocket Open', e);
                        mediaLiveRecorder = new MediaRecorder(mediaStreamObj);

                        mediaLiveRecorder.addEventListener('dataavailable', (e) => {
                            ws.send(e.data);
                            DotNet.invokeMethodAsync('LiveStreamBlazorApp', 'OnLiveStreamDataAvailable').then(e => { console.log(e.data); });
                        });

                        mediaLiveRecorder.onstop = (ev) => {
                            ws.close(1000, "Deliberate disconnection");
                        }

                        mediaLiveRecorder.start(500); //Start recording, and dump data every .5 second

                    });
                });


                stop.addEventListener('click', (ev) => {
                    mediaLiveRecorder.stop();
                    mediaRecorder.stop();
                    console.log(mediaRecorder.state);
                });

                mediaRecorder.ondataavailable = function (ev) {
                    chunks.push(ev.data);
                }
                mediaRecorder.onstop = (ev) => {
                    let blob = new Blob(chunks, { 'type': 'video/mp4;' });
                    DotNet.invokeMethodAsync('LiveStreamBlazorApp', 'RecordedData').then(data => {
                        data.push(blob);
                        console.log("sent");
                    });
                    chunks = [];
                    let videoURL = window.URL.createObjectURL(blob);
                    vidSave.src = videoURL;
                }
            })
            .catch(function (err) {
                console.log(err.name, err.message);
            });

        /*********************************
        getUserMedia returns a Promise
        resolve - returns a MediaStream Object
        reject returns one of the following errors
        AbortError - generic unknown cause
        NotAllowedError (SecurityError) - user rejected permissions
        NotFoundError - missing media track
        NotReadableError - user permissions given but hardware/OS error
        OverconstrainedError - constraint video settings preventing
        TypeError - audio: false, video: false
        *********************************/

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

function oninitstartlivestreampage() {

    var msg = "hello";

    window.getmsg = getmsg;

    function getmsg() {
        console.log(msg);
    }

    let constraintObj = {
        audio: true,
        video: {
            facingMode: "user",
            width: { min: 640, ideal: 960, max: 1920 },
            height: { min: 480, ideal: 540, max: 1080 }
        }
    };

    //handle older browsers that might implement getUserMedia in some way

    if (navigator.mediaDevices === undefined) {
        navigator.mediaDevices = {};
        navigator.mediaDevices.getUserMedia = function (constraintObj) {
            let getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
            if (!getUserMedia) {
                return Promise.reject(new Error('getUserMedia is not implemented in this browser'));
            }
            return new Promise(function (resolve, reject) {
                getUserMedia.call(navigator, constraintObj, resolve, reject);
            });
        }
    } else {
        navigator.mediaDevices.enumerateDevices()
            .then(devices => {
                devices.forEach(device => {
                    console.log(device.kind.toUpperCase(), device.label);
                    //, device.deviceId
                })
            })
            .catch(err => {
                console.log(err.name, err.message);
            })
    }

    navigator.mediaDevices.getUserMedia(constraintObj)
        .then(function (mediaStreamObj) {
            //connect the media stream to the first video element

            window.stream = mediaStreamObj;
            let video = document.getElementById('vidInput');
            if ("srcObject" in video) {
                video.srcObject = mediaStreamObj;
            } else {
                //old version
                video.src = window.URL.createObjectURL(mediaStreamObj);
            }

            video.onloadedmetadata = function (ev) {
                //show in the video element what is being captured by the webcam
                video.play();
            };

            let mediaRecorder = new MediaRecorder(mediaStreamObj);
            let chunks = [];

            let mediaLiveRecorder;

            window.startlivestream = startlivestream;

            function startlivestream() {
                mediaRecorder.start();
                console.log(mediaRecorder.state);
                var streaminfo = {
                    "id": "12345",
                    "secure_stream_url": "no link",
                    "stream_url": "rtmp://139.162.24.99/live/test"
                };

                console.log(streaminfo);
                console.log(window.location.protocol)

                const ws = new WebSocket("ws://localhost:3000/rtmp/rtmp%3A%2F%2F139.162.24.99%2Flive%2Ftest");

                ws.addEventListener('open', (e) => {
                    console.log('Websocket Open', e);
                    mediaLiveRecorder = new MediaRecorder(mediaStreamObj);

                    mediaLiveRecorder.addEventListener('dataavailable', (e) => {
                        ws.send(e.data);
                        DotNet.invokeMethodAsync('LiveStreamBlazorApp', 'OnLiveStreamDataAvailable').then(e => { console.log(e.data); });
                    });

                    mediaLiveRecorder.onstop = (ev) => {
                        ws.close(1000, "Deliberate disconnection");
                    }

                    mediaLiveRecorder.start(500); //Start recording, and dump data every .5 second
                });

            }

            window.stoplivestream = stoplivestream;

            function stoplivestream() {
                mediaLiveRecorder.stop();
                mediaRecorder.stop();
                console.log(mediaRecorder.state);
            }
            
            window.getrecordingurl = getrecordingurl;

            function getrecordingurl() {
                console.log = "getrecordingurl"
                let vidSave = document.getElementById('vidSave');
                vidSave.src = recordingUrl;

                return recordingUrl;


            }

            mediaRecorder.ondataavailable = function (ev) {
                chunks.push(ev.data);
            }
            mediaRecorder.onstop = (ev) => {
                let blob = new Blob(chunks, { 'type': 'video/mp4' });

                DotNet.invokeMethodAsync('LiveStreamBlazorApp', 'RecordedData').then(data => {
                    console.log("sent");
                    console.log(chunks.length);
                });
                chunks = [];

                console.log("mediaRecord on stop");

                recordingUrl = window.URL.createObjectURL(blob);

                console.log(recordingUrl);
                let videoURL = window.URL.createObjectURL(blob);


                if (stream.active == true) {
                    console.log("streams active");

                    stream.getTracks().forEach(function (track) {
                        track.stop();
                    }); 
                }
            }

            window.download = download;

            function download(bloburl, filename) {
                var a = document.createElement("a");
                a.href = bloburl;
                a.download = filename;
                a.click();

            }

            function downloadtemplate(blob) {
                var url = URL.createObjectURL(blob);
                var a = document.createElement("a");
                document.body.appendChild(a);
                a.style = "display: none";
                a.href = url;
                a.download = "test.mp4";
                a.click();
                window.URL.revokeObjectURL(url);
            }

        })
        .catch(function (err) {
            console.log(err.name, err.message);
        });

    /*********************************
    getUserMedia returns a Promise
    resolve - returns a MediaStream Object
    reject returns one of the following errors
    AbortError - generic unknown cause
    NotAllowedError (SecurityError) - user rejected permissions
    NotFoundError - missing media track
    NotReadableError - user permissions given but hardware/OS error
    OverconstrainedError - constraint video settings preventing
    TypeError - audio: false, video: false
    *********************************/

}


window.tolatestmsg = tolatestmsg;

function tolatestmsg() {
    console.log(tolatestmsg);
    var objDiv = document.getElementById("chat-content");
    objDiv.scrollTop = objDiv.scrollHeight;
}