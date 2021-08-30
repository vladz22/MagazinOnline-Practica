var datatable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#table').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "nume" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "companie.nume" },
            { "data": "rol" },
            {
                "data": {
                    id: "id",
                    lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var azi = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > azi) {
                        //user este blocat
                        return `
                               <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer;width:100px;">
                                    <i class="fas fa-lock-open"></i>&nbsp; Unlock
                                </a>
                            </div>
                              `;
                    }
                    else {
                        return `
                               <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-primary text-light" style="cursor:pointer;width:100px;">
                                    <i class="fas fa-lock"></i>&nbsp; Lock
                                </a>
                            </div>
                              `;
                    }
                }
            }
        ]
    });
}
function LockUnlock(id) {
                $.ajax({
                type: "POST",
                url: '/Admin/User/LockUnlock',
                data: JSON.stringify(id), //pass id
                contentType:"application/json",
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