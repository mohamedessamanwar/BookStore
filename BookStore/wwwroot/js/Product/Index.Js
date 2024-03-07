$(document).ready(function () {
    // Initialize the DataTable
    $('#TableProduct').dataTable({
        "processing": true, // Enable processing indicator during AJAX request
        "serverSide": true, // Enable server-side processing
        "filter": true, // Enable filtering/searching
        "ajax": {
            "url": "/Product/Index2", // URL to fetch data from
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
            { "data": "category", "name": "Category", "autowidth": true },
            { "data": "author", "name": "Author", "autowidth": true },
            { "data": "price", "name": "Price", "autowidth": true },
            {
                "data": null,
                // Render function for the last column
                "render": function (data, type, row) {
                    // Generate HTML for delete and update buttons
                    return '<a data-id="' + row.id + '" class="btn btn-danger js-delete" onclick="return confirm(\'Are you sure you want to delete this product?\');">Delete</a>' +
                        '<a href="/Product/Update/' + row.id + '" class="btn btn-danger">Update</a>';
                },
                "orderable": false, // Disable sorting for this column
                "searchable": false // Disable searching for this column
            }
        ]
    });

    // Bind click event to the delete button
    $('#TableProduct').on('click', '.js-delete', function () {
        var $button = $(this);
        // Get the product ID from data attribute
        var productId = $button.attr('data-id');

        // Send AJAX request to delete the product
        $.ajax({
            url: `/Product/Delete/${productId}`, // URL for the delete action
            method: 'DELETE', // HTTP request type
            success: function () {
                // Show success message
                alert('Product deleted successfully.');
                // Reload the table after successful deletion
                $('#TableProduct').DataTable().ajax.reload();
            },
            error: function () {
                // Show error message
                alert('Failed to delete product.');
            }
        });
    });
});
