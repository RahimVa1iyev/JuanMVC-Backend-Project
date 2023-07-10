

$(document).on("click", ".modal-btn", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url).then(response => {
        if (response.ok) {
            return response.text()
        }
        else {
            alert("Xeta bas verdi")
            return
        }
    })
        .then(data => {
            $("#quick_view .modal-dialog").html(data)
        })
    $("#quick_view").modal("show")
})


$(document).on("click", ".basket-add-btn", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url).then(response => {
        console.log("salam");
        console.log(response);
        if (!response.ok) {
            alert("Xeta bas verdi")
        }
        else return response.text()
    }).then(data => {
        $(".minicart-inner-content").html(data)
    })
})

$(document).on("click", ".delete-basket-btn", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url).then(response => {
        console.log("salam");
        console.log(response);
        if (!response.ok) {
            alert("Xeta bas verdi")
        }
        else return response.text()
    }).then(data => {
        $(".minicart-inner-content").html(data)
    })
})




