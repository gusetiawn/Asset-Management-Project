function RequestNewItem() {
    var Item = new Object();
    Item.accountId = $('#input-userId').val();
    Item.itemId = $('#input-item').val();
    Item.startDate = $('#input-start-date').val();
    Item.endDate = $('#input-end-date').val();
    Item.quantity = $('#input-quantity').val();
    Item.notes = $('#input-notes').val();
    $.ajax({
        type: "POST",
        url: 'https://localhost:44395/API/RequestItems/NewRequest',
        data: JSON.stringify(Item),
        contentType: "application/json; charset=utf-8",
        datatype: "json"
    }).done((result) => {
        Swal.fire(
            'Success',
            'Item Has Been Added, Cek Your Email',
            'success'
        );
        $('#tableDataReqItem').DataTable().ajax.reload();
        $("#input-userId").val(null);
        $("#input-item").val(null);
        $("#input-start-date").val(null);
        $("#input-end-date").val(null);
        $("#input-quantity").val(null);
        $("#input-notes").val(null);
            
    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}
