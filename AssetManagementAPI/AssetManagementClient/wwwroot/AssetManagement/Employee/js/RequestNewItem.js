$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44395/API/Items"
    }).done((result) => {
        itemAvailable = `<label class="form-control-label" for="input-item">Item</label>
                        <select name="Item" id="input-item" class="form-control form-control-alternative">
                            <option value="" hidden>Choose Item...</option>
                            <option value="1">Laptop - Available Items(${result.data[0].quantity})</option>
                            <option value="2">Projector - Available Items(${result.data[1].quantity})</option>
                            <option value="3">Camera - Available Items(${result.data[2].quantity})</option>
                            <option value="4">VGA Cable - Available Items(${result.data[3].quantity})</option>
                            <option value="5">HDMI Cable - Available Items(${result.data[4].quantity})</option>
                            <option value="6">Car - Available Items(${result.data[5].quantity})</option>
                            <option value="7">Bus - Available Items(${result.data[6].quantity})</option>
                            <option value="8">Motorcycle - Available Items(${result.data[7].quantity})</option>
                            <option value="9">Pick Up Car - Available Items(${result.data[8].quantity})</option>
                            <option value="10">White Board - Available Items(${result.data[9].quantity})</option>
                            <option value="11">Marker - Available Items(${result.data[10].quantity})</option>
                            <option value="12">Log Book - Available Items(${result.data[11].quantity})</option>
                            <option value="13">Multi Purpose Building (Small) - Available Items(${result.data[12].quantity})</option>
                        </select>`
        $(".item-available").html(itemAvailable);
    }).fail((error) => {
        alert("error");
    });
});

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
    }).done((success) => {
        Swal.fire(
            'Success',
            'Request Item Success ! Check Your Email For Further Information',
            'success'
        );
        $('#tableDataReqItem').DataTable().ajax.reload();
        $.ajax({
            url: "https://localhost:44395/API/Items"
        }).done((result) => {
            itemAvailable = `<label class="form-control-label" for="input-item">Item</label>
                        <select name="Item" id="input-item" class="form-control form-control-alternative">
                            <option value="" hidden>Choose Item...</option>
                            <option value="1">Laptop - Available Items(${result.data[0].quantity})</option>
                            <option value="2">Projector - Available Items(${result.data[1].quantity})</option>
                            <option value="3">Camera - Available Items(${result.data[2].quantity})</option>
                            <option value="4">VGA Cable - Available Items(${result.data[3].quantity})</option>
                            <option value="5">HDMI Cable - Available Items(${result.data[4].quantity})</option>
                            <option value="6">Car - Available Items(${result.data[5].quantity})</option>
                            <option value="7">Bus - Available Items(${result.data[6].quantity})</option>
                            <option value="8">Motorcycle - Available Items(${result.data[7].quantity})</option>
                            <option value="9">Pick Up Car - Available Items(${result.data[8].quantity})</option>
                            <option value="10">White Board - Available Items(${result.data[9].quantity})</option>
                            <option value="11">Marker - Available Items(${result.data[10].quantity})</option>
                            <option value="12">Log Book - Available Items(${result.data[11].quantity})</option>
                            <option value="13">Multi Purpose Building (Small) - Available Items(${result.data[12].quantity})</option>
                        </select>`
            $(".item-available").html(itemAvailable);
            $("#input-userId").val(null);
            $("#input-start-date").val(null);
            $("#input-end-date").val(null);
            $("#input-quantity").val(null);
            $("#input-notes").val(null);
        }).fail((error) => {
            alert("error");
        });    
    }).fail((failed) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}
