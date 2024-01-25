$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "ordering": false,
        "columns": [
            { data: 'email', "width": "10%" },
            { data: 'role', "width": "10%" },
            { data: 'companyName', "width": "10%" },
            { data: 'name', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },
            {
                data: { id: 'id', lockoutEnd: 'lockoutEnd' },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                        <div class="text-center w-100">
                             <a onclick=toggleLock('${data.id}') class="btn btn-danger btn-sm text-white" style="cursor:pointer; width:150px;">
                                    <i class="bi bi-lock-fill"></i> Locked
                                </a> 
                                <a class="btn btn-primary btn-sm text-white" style="cursor:pointer; width:150px;">
                                     <i class="bi bi-pencil-square"></i> Permission
                                </a>
                        </div>
                    `
                    }
                    else {
                        return `
                        <div class="text-center">
                              <a onclick=toggleLock('${data.id}') class="btn btn-success btn-sm text-white" style="cursor:pointer; width:150px;">
                                    <i class="bi bi-unlock-fill"></i> Unlocked
                                </a>
                                <a class="btn btn-primary btn-sm text-white" style="cursor:pointer; width:150px;">
                                     <i class="bi bi-pencil-square"></i> Permission
                                </a>
                        </div>
                    `
                    }
                },
                "width": "50%"
            }
        ]
    });
}

function toggleLock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/ToggleLock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}