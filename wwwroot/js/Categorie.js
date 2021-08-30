var datatable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#table').DataTable({
        "ajax": {
            "url": "/Admin/Categorie/GetAll"
        },
        "columns": [
            { "data": "nume", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Categorie/Upsert/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Admin/Categorie/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
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