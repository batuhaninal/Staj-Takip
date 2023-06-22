window.onload = function () {
    var canvas = document.getElementById("signatureCanvas");
    var context = canvas.getContext("2d");

    var isDrawing = false;
    var lastX = 0;
    var lastY = 0;

    // Mouse tuş basıldığında imzalama başlar
    canvas.addEventListener("mousedown", function (event) {
        isDrawing = true;
        lastX = event.clientX - canvas.offsetLeft;
        lastY = event.clientY - canvas.offsetTop;
    });

    // Mouse hareket ettirildiğinde çizgi çizme işlemi gerçekleşir
    canvas.addEventListener("mousemove", function (event) {
        if (isDrawing) {
            var currentX = event.clientX - canvas.offsetLeft;
            var currentY = event.clientY - canvas.offsetTop;
            context.beginPath();
            context.moveTo(lastX, lastY);
            context.lineTo(currentX, currentY);
            context.strokeStyle = "#000";
            context.lineWidth = 2;
            context.stroke();
            lastX = currentX;
            lastY = currentY;
        }
    });

    // Mouse tuşu bırakıldığında imzalama sonlanır
    canvas.addEventListener("mouseup", function () {
        isDrawing = false;
    });

    // Temizle butonuna tıklanınca canvas temizlenir
    var clearButton = document.getElementById("clearButton");
    clearButton.addEventListener("click", function () {
        context.clearRect(0, 0, canvas.width, canvas.height);
    });
    var dId = 0;

    $('.signatureTest').on('click', function () {
        dId = $(this).attr('id');
        alert(dId);
    });

    // İmza kaydetme butonuna tıklanınca imza verisi alınır ve işlenir
    var saveButton = document.getElementById("saveButton");
    saveButton.addEventListener("click", function () {

        var canvas = document.getElementById("signatureCanvas");
        var imageData = canvas.toDataURL("image/png");

        var formData = new FormData();

        formData.append("signature", imageData);
        formData.append("documentId", dId);

        fetch("/InternshipDocument/UploadSignature", {
            method: "POST",
            body: formData
        }).then(function () {
            swal({
                title: 'İmzalama İşlemi Başarılı!',
                text: 'Bu bildirimi kapatabilirsiniz!',
                type: 'success',
                icon: "success",
                buttons: {
                    ok: {
                        text: "Ok",
                        value: "ok",
                    },
                }
            })
                .then((value) => {
                    switch (value) {
                        case "ok":
                            location.reload();
                            break;

                        default:
                            swal("Got away safely!");
                    }
                });
        });

    });
};