


var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

connection.start().then(() => {
    console.log("Huba qosuldu")
})



console.log("salam");


$(document).on("input", "#searchValue", function (e) {


    var searchValue = $("#searchValue").val();
    let url = `Home/GetSearch?searchValue=${searchValue}`;

    fetch(url)
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error("Xeta bas verdi");
            }
        })
        .then(data => {
            console.log(data);
            let x = '';

            data.forEach(item => {
                x += ` 
                           <li>
                              <a style="color:white ; font-weight:700;" href="product/detail/${item.id}" >${item.name}</a>
                           </li>
                   `;
            });

            $("#searchResults ul").html(x);
        })
        .catch(error => {
            alert(error.message);
        });
});







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

        if (!response.ok) {
            alert("Xeta bas verdi")
        }
        else return response.text()
    }).then(data => {
        console.log(data)
        $(".minicart-inner-content").html(data)

        var count = $("#basketItems").data("count");

        $("#basketCount").html(count)
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

        var count = $("#basketItems").data("count");

        $("#basketCount").html(count)
    })
})




