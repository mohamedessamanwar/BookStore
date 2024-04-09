document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('submitBtn').addEventListener('click', function () {
        var formData = {
            name: document.getElementById('Id').value,
            email: document.getElementById('count').value
        };

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/ShoppingCart/AddToCart');
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {
                var response = JSON.parse(xhr.responseText);
                // Display success alert with the message
                alert(response.message);

                // Optionally, update any UI elements based on the response
                // document.getElementById('result').innerHTML = response.data; // Assuming response contains both message and data
            } else {
                var errorMessage = "An error occurred.";
                try {
                    var responseJson = JSON.parse(xhr.responseText);
                    errorMessage = responseJson.message || "An error occurred.";
                } catch (e) {
                    errorMessage = "An error occurred.";
                }
                // Display error alert with the error message
                alert(errorMessage);
            }
        };
        xhr.onerror = function () {
            // Display error alert if there's a network error
            alert("An error occurred.");
        };
        xhr.send(JSON.stringify(formData));
    });
});
