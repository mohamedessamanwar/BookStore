function initializeDataTable(status) {
    let tableProduct = document.getElementById('TableProduct');
    $(tableProduct).dataTable({
        "processing": true, // Enable processing indicator during AJAX request
        "serverSide": true, // Enable server-side processing
        "filter": true, // Enable filtering/searching
        "ajax": {
            "url": "/Order/Index?status=" + status, // URL to fetch data from
            "type": "POST", // HTTP request type
            "dataType": "json" // Data type of the response
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            // Define columns and their properties
            { "data": "id", "name": "Id", "autowidth": true },
            { "data": "name", "name": "Name", "autowidth": true },
            { "data": "email", "name": "Email", "autowidth": true },
            { "data": "phoneNumber", "name": "PhoneNumber", "autowidth": true },
            { "data": "orderStatus", "name": "OrderStatus", "autowidth": true },
            { "data": "orderTotal", "name": "OrderTotal", "autowidth": true },
            {
                "data": null,
                // Render function for the last column
                "render": function (data, type, row) {
                    // Generate HTML for delete and update buttons
                    return '<a href="/Order/Details/' + row.id + '" class="btn btn-danger js-details">Details</a>';

                },
                "orderable": false, // Disable sorting for this column
                "searchable": false // Disable searching for this column
            }
        ]
    });
}

document.addEventListener('DOMContentLoaded', function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        initializeDataTable("inprocess");
    } else if (url.includes("completed")) {
        initializeDataTable("completed");
    } else if (url.includes("pending")) {
        initializeDataTable("pending");
    } else if (url.includes("approved")) {
        initializeDataTable("approved");
    } else {
        initializeDataTable("all");
    }
});
