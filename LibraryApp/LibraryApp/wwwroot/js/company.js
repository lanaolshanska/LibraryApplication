$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "ordering": false,
        "columns": [
            { data: 'name', "width": "20%" },
            { data: 'state', "width": "20%" },
            { data: 'city', "width": "20%" },
            { data: 'streetAddress', "width": "20%" },
            {
                data: 'id',
                "render": function (id) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/company/createorupdate?id=${id}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=deleteItem('/admin/company/delete/${id}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "20%"
            }
        ]
    });
}

function deleteItem(url) {
    var isConfirmed = confirm('Are you sure you want to delete this item ?');
    if (isConfirmed) {
        $.ajax({
            url: url,
            type: 'DELETE',
            success: function (data) {
                dataTable.ajax.reload();
                toastr.warning(data.message);
            }
        })
    }
}