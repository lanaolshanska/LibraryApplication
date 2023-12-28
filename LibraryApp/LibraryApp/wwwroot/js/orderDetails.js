function loadShipmentModal(event, orderId) {
    event.preventDefault();
    $.get(`/Admin/Order/GetShipmentModal?orderId=${orderId}`, function (data) {
        $('#modalContainer').html(data);
        $('#modalContainer .modal').modal('show');
    });
};