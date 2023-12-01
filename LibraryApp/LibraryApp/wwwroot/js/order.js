$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall' },
        "ordering": false,
        "columns": [
            { data: 'userName', "width": "10%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'email', "width": "20%" },
            { data: 'status', "width": "20%" },
            { data: 'total', "width": "10%" },
            {
                data: 'id',
                "render": function (id) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/order/update?id=${id}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                    </div>`
                },
                "width": "20%"
            }
        ]
    });
}