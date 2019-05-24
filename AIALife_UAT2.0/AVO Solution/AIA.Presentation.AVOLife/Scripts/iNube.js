var errorArray = {};
var errorMessageCount = 0;
//Added For address control
function SelectAutoFillAddress(obj, datum, textId) {
    var SelectedAutoCompleteValue = datum.item.value;
    $.ajax({
        type: "GET",
        url: "../../API/Master/GetAutoFillAddress",
        data: { Pincode: SelectedAutoCompleteValue },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#" + textId + "City").val(data.City);
            $("#" + textId + "State").val(data.State);
            $("#" + textId + "District").val(data.District);
            $("#" + textId + "Country").val("India");
            $("#" + textId + "City").attr("readonly", "readonly");
            $("#" + textId + "State").attr("readonly", "readonly");
            $("#" + textId + "District").attr("readonly", "readonly");
            $("#" + textId + "Country").attr("readonly", "readonly");
        },
        error: function () {
        }
    });
}
//Error message control: can write multiple error messages to the error message console
function writeMultipleMessage(type, message, elementId) {    
    var container = $("#containerMain");
    var i = 0;
    var errorFlag = true;
    var color = "";
    var imgSrc = "";
    if (type == "error") {
        color = "Red";
        imgSrc1 = "../Content/Images/danger-icon-2.png";
        imgSrc = "../Content/Images/danger-icon-1.png";
        //imgSrc = "../Content/Images/error.jpg";
        $("#" + elementId).css("border-color", color);
        // if (errorArray[elementId] != "" && errorArray[elementId] != undefined && errorArray[elementId] != null) { Object.keys(errorArray).length
        errorArray[elementId] = message;
        //}
    }
    else if (type == "success") {
        color = "Green";
        imgSrc = "../Content/Images/success.png";
        $("#" + elementId).css("border-color", "#94C0D2");
    }
    else {
        color = "Yellow";
        imgSrc = "../Content/Images/warning.png";
        $("#" + elementId).css("border-color", color);
    }
    errorMessageCount = 0;
    for (element in errorArray) {
        if (element != "" && errorArray[element] != undefined && errorArray[element] != "") {
            errorFlag = false;
            errorMessageCount++;
        }
    }

    if (container.is(":visible") && message == "") {
        if (errorFlag) {
            container.slideUp(500);
        }
        $("#" + elementId).css("border-color", "#94C0D2");
    } else if (message != "") {
        if (errorMessageCount > 2 && errorMessageCount < 4) {
            var containerHeight = container.height();
            if (containerHeight <= 50) {
                container.height(containerHeight * 2);
                //$("#divContent").height(containerHeight * 2);
            }
        }
        container.slideDown(500);
        if (type == "success") {
            setTimeout(function () {
                container.slideUp(500);
            }, 2000);
        }
    }
    if (message == "") {
        $("#" + elementId).css("border-color", "#94C0D2");
        $('#containerMain').hide();
    }
    $('#containerMain').css('position', 'fixed');
   
    //  var listStart = "<div><div style='float:right;cursor:pointer;font-size:medium' onclick='closePopUp()' id='drag'>x</div>";
     var listStart = "<div class='alert alert-danger alert-dismissable row'><div class='col-lg-1 col-md-1 col-sm-2 col-xs-2'><img src='" + imgSrc + "' onclick='closePopUp()' /></div>";
     var listEnd = "</div><div class='col-lg-1 col-md-1 col-sm-2 col-xs-2'><img style='float:right;cursor:pointer;font-size:medium' onclick='closePopUp()' id='drag' src='" + imgSrc1 + "'/></div></div><div class='clearfix'></div>";
     listStart += "<div class='ErrorStyle col-lg-10 col-md-10 col-sm-8 col-xs-8'>"
     for (element in errorArray) {
        if (element != "" && errorArray[element] != undefined && errorArray[element] != "") {
            listStart += "<div style='padding-left:5px;cursor:pointer;color:#fff;padding-top:10px' onclick='setFocus(\"" + element + "\")'>" + errorArray[element] + "</div>";

        }
    }  
    listStart += listEnd;
    //  container.css('color', color);
    //$("#containerMain").append("<div class='alert alert-" + 'danger' + " alert-dismissable'><a href='#' class='close'" +
    //       "data-dismiss='alert' aria-hidden='true' >&times;</a> " + 'Dummy'+ "</div> ");
    container.html(listStart);
   
}

//Method to close the error message console
function closePopUp() {
    //close the popup
    var container = $("#containerMain");
    if (container.is(":visible")) {
        // Hide - slide up.
        container.slideUp(500);
    } else {
        // Show - slide down.
        // container.slideDown(500);
    }
}
function isNumberKey(evt) {

var charCode = (evt.which) ? evt.which : event.keyCode
    if (((charCode != 8 && charCode < 48) || charCode > 57))
        return false;
    return true;
}

function setFocus(id) {
    $("#" + id).focus();
}