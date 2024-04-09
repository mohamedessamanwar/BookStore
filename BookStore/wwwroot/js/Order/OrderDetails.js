var c = document.getElementById("Header_Carrier");
var t = document.getElementById("Header_TrackingNumber");
//document.getElementById("ShipOrder").addEventListener("click", () => {
   

//}); 


function validateInput() {
    if (c.value == "" || t.value == "") {
        Swal.fire({
            title: "Error!",
            text: "Error",
            icon: "error"
        });
        return false;
    }
    return true; 

}