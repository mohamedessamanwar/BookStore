$(document).ready(function () {
    $('#TableCategory').DataTable({
        "processing": true, // Enable processing indicator during AJAX request
        "serverSide": true, // Enable server-side processing
        "filter": true, // Enable filtering/searching
        "ajax": {
            "url": "/Category/Indexjson",
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
            {
                "data": null,
                // Render function for the last column
                "render": function (data, type, row) {
                    // Generate HTML for delete and update buttons
                    return '<a href="/Category/Edit/' + row.id + '" class="btn btn-danger js-details">Edit</a>';
                },
                "orderable": false, // Disable sorting for this column
                "searchable": false // Disable searching for this column
            }
        ]
    });
});
