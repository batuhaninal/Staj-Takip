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

    // İmza kaydetme butonuna tıklanınca imza verisi alınır ve işlenir
    var saveButton = document.getElementById("saveButton");
    saveButton.addEventListener("click", function () {
        var signatureData = canvas.toDataURL("image/png");
        // İmza verisini kullanarak yapılacak işlemleri burada gerçekleştirebilirsiniz
        // Örneğin, veriyi bir sunucuya gönderebilir veya başka bir işleme tabi tutabilirsiniz
        console.log(signatureData);
    });
};