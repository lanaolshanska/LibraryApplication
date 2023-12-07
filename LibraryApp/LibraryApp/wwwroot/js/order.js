$(document).ready(function () {
    const url = new URL(window.location.href);
    const searchParams = new URLSearchParams(url.search);
    let status = searchParams.get('status');
    loadDataTable(status);
});

function loadDataTable(status) {
    let url = '/admin/order/getall';
    if (status != null) {
        url = url + '?status=' + status
    }  
    dataTable = $('#tblData').DataTable({
        "ajax": { url: url },
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
                     <a href="/admin/order/details?id=${id}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>               
                    </div>`
                },
                "width": "20%"
            }
        ]
    });
}