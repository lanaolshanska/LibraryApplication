$(document).ready(function () {
    $('#Input_Role').change(function () {
        var selection = $('#Input_Role Option:Selected').text();
        if (selection == 'Company') {
            $('#Input_CompanyId').show();
        }
        else {
            $('#Input_CompanyId').hide();
        }
    })
});