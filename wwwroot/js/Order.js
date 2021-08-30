var datatable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("GetOrderList?status=inprocess");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("GetOrderList?status=pending");
        }
        else {
            if (url.includes("completed")) {
                loadDataTable("GetOrderList?status=completed");
            }
            else {
                if (url.includes ("rejected")) {
                    loadDataTable("GetOrderList?status=rejected");
                }
                else {
                    loadDataTable("GetOrderList?status=all");
                }
            }
        }
    }
 });
function loadDataTable(url) {
    datatable = $('#table').DataTable({
        "ajax": {
            "url": "/Admin/Order/" + url
        },
        "columns": [
            { "data": "id" },
            { "data": "nume" },
            { "data": "adresa" },
            { "data": "oras" },
            { "data": "tara" },
            { "data": "orderTotal" },
            { "data": "orderStatus" },
            { "data": "applicationUser.email"}
        ]
    });
}
function Delete(url) {
    swal({
        title: "Esti sigur ca doresti sa stergi?",
        text: "Data va fi eliminata complet!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.error) {
                        toastr.error(data.message);
                    }
                    else {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                }
            });
        }
    });
}