$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "ordering": false,
        "columns": [
            { data: 'email', "width": "20%" },
            { data: 'role', "width": "15%" },
            { data: 'companyName', "width": "20%" },
            { data: 'name', "width": "10%" },
            { data: 'phoneNumber', "width": "20%" },
            {
                data: 'id',
                "render": function (id) {
                    return `<div>
                     <a href="/admin/company/createorupdate?id=${id}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                    </div>`
                },
                "width": "15%"
            }
        ]
    });
}