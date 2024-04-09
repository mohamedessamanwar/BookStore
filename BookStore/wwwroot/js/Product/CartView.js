document.getElementById("submitBtn").addEventListener("click", () => {
    var formData = {
        ProductId: document.getElementById('Id').value,
        Count: document.getElementById('count').value
    };
    console.log(formData);
    let myRequest = new XMLHttpRequest();

    myRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
               
                // Request was successful
                var response = JSON.parse(this.responseText);
                if (response.success) {
                    Swal.fire({
                        title: "Success!",
                        text: response.message,
                        icon: "success"
                    });
                } else {
                    // Server responded with success:false
                    Swal.fire({
                        title: "Error!",
                        text: response.message,
                        icon: "error"
                    });
                }
            } else {
                // Request failed
                console.error("Request failed with status:", this.status);
                // Show an error message using SweetAlert
                Swal.fire({
                    title: "Error!",
                    text: "Failed to add to cart.",
                    icon: "error"
                });
            }
        }
    };

    myRequest.open("POST", "/ShoppingCart/AddToCart", true);
    myRequest.setRequestHeader("Content-Type", "application/json"); // Specify content type
    myRequest.send(JSON.stringify(formData));
});
