$(document).ready(function () {
    toggleCompanySelect();

    $('#Input_Role').change(function () {
        toggleCompanySelect();
    })
});

function toggleCompanySelect() {
    var selection = $('#Input_Role Option:Selected').text();
    if (selection == 'Company') {
        $('#Input_CompanyId').show();
    }
    else {
        $('#Input_CompanyId').hide();
    }
}