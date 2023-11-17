function autofill(url) {
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            if (data.address != null) {
                $('#Address_Name').val(data.address.name);
                $('#Address_PhoneNumber').val(data.address.phoneNumber);
                $('#Address_StreetAddress').val(data.address.streetAddress);
                $('#Address_City').val(data.address.city);
                $('#Address_State').val(data.address.state);
                $('#Address_PostalCode').val(data.address.postalCode);
            } else {
                toastr.error(data.message);
            }
        }
    })
};

