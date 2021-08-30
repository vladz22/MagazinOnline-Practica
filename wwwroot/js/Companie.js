var datatable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#table').DataTable({
        "ajax": {
            "url": "/Admin/Companie/GetAll"
        },
        "columns": [
            { "data": "nume" },
            { "data": "adresa"},
            { "data": "oras" },
            { "data": "tara" },
            { "data": "codPostal"},
            { "data": "nrTelefon" },
            {
                "data": "isAutorizat",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked style="width:100%;height:2rem"/>`
                    }
                    else {
                        return `<input type="checkbox" disabled style="width:100%;height:2rem"/>`
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Companie/Upsert/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>&nbsp;
                                </a>
                                <a onclick=Delete("/Admin/Companie/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>&nbsp;
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
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
                type:"DELETE",
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