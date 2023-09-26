$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "ordering": false,
        "columns": [
            { data: 'title', "width": "15%" },
            { data: 'author', "width": "15%" },
            { data: 'description', "width": "30%" },
            { data: 'category.name', "width": "10%" },
            { data: 'price', "width": "10%" },
            {
                data: 'id',
                "render": function (id) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/createorupdate?id=${id}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=deleteItem('/admin/product/delete/${id}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
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