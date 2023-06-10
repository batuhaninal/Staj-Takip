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
        }).then((response) => location.reload());

    });
    //saveButton.addEventListener("click", function () {
    //    var signatureName = document.getElementById("signatureName").value;
    //    canvas.toBlob(function (blob) {
    //        var formData = new FormData();
    //        formData.append("signatureBase", blob);
    //        formData.append("signature.Name", signatureName); // İmza adını FormData'ya ekle

    //        var xhr = new XMLHttpRequest();
    //        xhr.open("POST", "/InternshipDocument/SaveSign", true);
    //        xhr.onload = function () {
    //            // İstek tamamlandığında işlemler yapabilirsiniz
    //        };
    //        xhr.send(formData);
    //    }, "image/png"); // İmza verisinin hangi formatta blob olarak elde edileceğini belirtin (örneğin, "image/png" veya "image/jpeg")
    //});
    //var saveButton = document.getElementById("saveButton");
    //saveButton.addEventListener("click", function () {
    //    var signatureData = canvas.toDataURL('image/jpeg', 0.5);


    //    // Bu noktada, signatureData'yı sunucuya göndererek kaydedebilirsiniz
    //    // Örneğin, Ajax kullanarak bir POST isteği gönderebilirsiniz
    //    // Sunucu tarafında imza verisini alıp işlem yapmanız gerekecektir

    //    // Örneğin, imza verisini bir gizli alan içine yerleştirerek bir formu gönderme:
    //    var hiddenInput = document.createElement('input');
    //    hiddenInput.type = 'hidden';
    //    hiddenInput.name = 'signatureBase';
    //    hiddenInput.value = signatureData;

    //    var form = document.createElement('form');
    //    form.method = 'POST';
    //    form.action = '/InternshipDocument/Kaydet'; // İmzayı kaydetmek için uygun bir URL'yi belirtin
    //    form.appendChild(hiddenInput);
    //    document.body.appendChild(form);
    //    form.submit();
    //});

};