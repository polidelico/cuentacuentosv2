
$(function () {
    $("#flash-messages").flashMessage();
    $('#email-share button[type="submit"]').click(shareViaEmail);
    imagePreview();


});

function shareViaEmail(e) {
    e.preventDefault();
    var $form = $(this.closest('form'));
    $.post($form.attr('action'), $form.serialize(), function () {
        $('#flash-messages').flashMessage({ message: "Mensaje enviado.", alert: 'success' });
    }).fail(function () {
        $('#flash-messages').flashMessage({ message: "Ha ocurrido un error, intente mas tarde.", alert: 'error' });
    }).always(function(){
        $('#share-via-email').modal('hide');
    });
    
}

function imagePreview() {
    var element = $('.preview-file-upload');

    element.find('input').on('change', function () {
        var $this = $(this);
        var files = !!this.files ? this.files : [];
        if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support

        if (/^image/.test(files[0].type)) { // only image file
            var reader = new FileReader(); // instance of the FileReader
            reader.readAsDataURL(files[0]); // read the local file

            reader.onloadend = function () { // set image data as background of div
                element.find('.image-preview').attr('src', this.result);
            }
        }
    })
}

function initLoader(){
    var loaderAnimation = $("#html5Loader").LoaderAnimation({
        onComplete: function () {
        }
    });
    $.html5Loader({
        filesToLoad: 'loader/files.json',
        onComplete: function () {
            console.log("All the assets are loaded!");
        },
        onUpdate: loaderAnimation.update
    });
}

//Function for destribuute pages on chuunks of tw pieces
function chunks(array, size) {
    var results = [];
    while (array.length) {
        results.push(array.splice(0, size));
    }
    return results;
}



//Shares
function postToFeed(title, desc, url, image) {
    var obj = { method: 'feed', link: url, picture: image, name: title, description: desc };
    function callback(response) { }
    FB.ui(obj, callback);
}
function facebookShare(event) {
    var title = $(this).data('title');
    var desc = $(this).data('description');
    var img = $(this).data('img');
    postToFeed(title, desc, window.location.href, img);
    return false;
}
function TwitterShare() {
    var url = window.location.href;     // Returns full URL
    var text = $(this).data('text');
    var loc = encodeURIComponent(url);
    window.open('http://twitter.com/share?url=' + loc + '&text=' + text + '&', 'twitterwindow', 'height=450, width=550, top=' + ($(window).height() / 2 - 225) + ', left=' + $(window).width() / 2 + ', toolbar=0, location=0, menubar=0, directories=0, scrollbars=0');
}


function URLEncode(text) {
    var plaintext = text;
    var result = "";
    result = encodeURIComponent(plaintext);
    return result;
}


function openModalToLogin() {
    console.log("TEST");
    swal({
        title: "No esta logueado",
        text: "Para el uso de esta herramienta necesita estar, log-in.",
        confirmButtonColor: "#33AEAE",
        type: "info",
        showCancelButton: true,
        confirmButtonColor: "#33AEAE",
        confirmButtonText: "Mi Cuenta",
        closeOnConfirm: false
    },
    function () {
        location.href = linkToLogin;
    });
}