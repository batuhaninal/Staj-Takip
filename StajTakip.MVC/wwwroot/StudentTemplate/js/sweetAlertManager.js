function successSwal(message) {
    swal({
        title: "Başarılı!",
        text: message ??= "İşleminiz başarıyla gerçekleştirildi!",
        icon: "success",
        button: "Kapat",
    });
}

function errorSwal(errorMessage) {
    swal({
        title: "Hata",
        text: errorMessage ??= "İşleminiz gerçekleştirilirken bir hata ile karşılaşıldı!",
        icon: "error",
        button: "Kapat",
    });
}