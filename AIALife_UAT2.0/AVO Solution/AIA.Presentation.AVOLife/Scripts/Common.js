$.ajaxSetup({
    beforeSend: function () {
        // show gif here, eg:
        $("#divLoading").show();
    },
    complete: function () {
        // hide gif here, eg:
        $("#divLoading").hide();
    }
});

function toggleBusyIndicator(status) {
    status = (status !== undefined && status !== null) ? status.toLowerCase() : "hide";
    if (status === "show") {
        $("#layout").css({ "opacity": "1" });
        $("#divBusyIndicator").show();
    }
    else {
        $("#layout").css({ "opacity": "1" });
        $("#divBusyIndicator").hide();
    }
}
function AlphabetsWithSpace(evt) {
   
    var event = event || window.event;
    var val = event.target.value;
    //alert(val);
    var filtered = val.replace(/[^a-zA-Z ]+/g, '');
    //alert(filtered);
    if (filtered !== val) {
        event.target.value = filtered;
        event.target.className += " error";
    }
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (!((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32)||(charCode==8)))
        return false;
    return true;
}
function AlphabetsWithSpacecommaandslash(evt) {
    //debugger;
    //var charCode = (evt.which) ? evt.which : event.keyCode
    //if (!((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32) || (charCode == 8) || (charCode == 39)))
    //    return false;
    //return true;
    var event = event || window.event;
    var val = event.target.value;
    //alert(val);
    var filtered = val.replace(/[^a-zA-Z ]+$/g, '');
    //alert(filtered);
    if (filtered !== val) {
        event.target.value = filtered;
        event.target.className += " error";
    }
    var charCode = (event.which) ? event.which : event.keyCode
    //alert(charCode);
    if (!((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32) || (charCode == 8) || (charCode == 39)))

        return false;

    //    thousandseparatorwithID($("#" + evt.id).val())
    return true;
}
function AlphabetsWithAddress(evt){
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (!((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode > 47 && charCode < 58) || (charCode == 32) || (charCode == 8) || (charCode == 44) || (charCode == 47) || (charCode == 45) || (charCode == 46) || (charCode == 35) || (charCode == 39)))
        return false;
    return true;
}
//For Enable/Disable of controls
function EnableDisableAllControls(controlID, Enable) {

    EnableAllButtons(controlID, Enable);
    var divControls = $("#" + controlID + " input[type=text]").attr("readonly", !Enable);
    var divControls = $("#" + controlID + " input[type=text]").attr("disabled", !Enable);
    var divControls = $("#" + controlID + " input[type=checkbox]").attr("disabled", !Enable);
    $("#" + controlID + " input[type=radio]").attr("disabled", !Enable);
    $("#" + controlID + " select").attr("disabled", !Enable);
    $("#" + controlID).attr("disabled", !Enable);
    //Disables datepicker
    $("#" + controlID + " span").attr("disabled", !Enable);

    $("#" + controlID + " [data-role=grid]").each(function (index) {
        var id = $(this).attr('id');
        if (Enable) {
            enableKendoGrid(id);
        }
        else {
            disableKendoGrid(id);
        }
    });

    $("#" + controlID + " textarea").attr("disabled", "disabled");
    $("#" + controlID + "[data-role=listview]").each(function (index) {
        var id = $(this).attr('id');
        
    });
}
//Added For disabling date-pickers in motor
function EnableDisableDatePicker(controlID, Enable) {
    if (Enable == false) {
        if (document.getElementById("div" + controlID) != null) {
            document.getElementById("div" + controlID).style.pointerEvents = 'none'
        }
    }
    else {
        if (document.getElementById("div" + controlID) != null) {
            document.getElementById("div" + controlID).style.pointerEvents = 'auto'
        }
    }
}
function writeSuccessMessage(type, message) {
    var container = $("#containerMain");
    var i = 0;
    var color = "";
    var imgSrc = "";
    if (type == "error") {
        color = "Red";
        imgSrc = "../../Images/error.jpg";
    }
    else if (type == "success") {
        color = "Green";
        imgSrc = "../../Images/success.png";
    }
    else if (type == "MotorSuccess") {
        color = "Green";
        imgSrc = "../../Images/success.png";
    }
    else {
        color = "Blue";
        imgSrc = "../../Images/warning.png";
    }

    var listStart = "<div><div style='float:right;cursor:pointer;font-size:medium;' onclick='closePopUp()' id='drag'><img src='" + "../../Images/error.jpg" + "'height='20px' width='20px' style='vertical-align: middle;position:fixed;margin-left:-22px;'/></div>";
    var listEnd = "</div>";
    if (type == "success") {
        listStart += "<img src='" + imgSrc + "'height='24px' width='24px' style='vertical-align: middle;'/><span style='padding-left:5px; font-weight:bold;'>" + message + "</span>";

    }
    else {
        listStart += "<img src='" + imgSrc + "'height='24px' width='24px' style='vertical-align: middle;'/><span style='padding-left:5px'>" + message + "</span>";
    }
    listStart += listEnd;
    container.css('color', color);
    
    container.html(listStart);

    // Toggle the slide based on its current visibility.
    if (container.is(":visible") && message == "") {
        // Hide - slide up.
        container.slideUp(500);
    } else if (message != "") {
        container.slideDown(500);

    }
}
//Enable.Disable buttons
function EnableAllButtons(controlID, Enable) {
    if (Enable == false) {
        $("#" + controlID + " input[type=button]").attr("disabled", "disabled");
    }
    else {
        $("#" + controlID + " input[type=button]").attr("disabled", false);
    }


}

function formatCurrency(e) {
    var controlId = e.id;
    var nStr = $("#" + controlId).val();
    if (nStr == "") {
        return;
    }
    var x3 = '';
    var str = nStr.toString();
    str = str.replace(/,/g, "");;
    var negVal = str.split('-');
    if (negVal[1] == undefined) {
        str = negVal[0];
    }
    else {
        str = negVal[1];
        x3 = '-';
    }

    str += '';
    x = str.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    var z = 0;
    var len = String(x1).length;
    var num = parseInt((len / 2) - 1);

    while (rgx.test(x1)) {
        if (z > 0) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        else {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
            rgx = /(\d+)(\d{2})/;
        }
        z++;
        num--;
        if (num == 0) {
            break;
        }
    }
    $("#" + controlId).val(x3 + x1 + x2);
    
}

//validate mailid pattern 
function EmailValidation(Mailid, msg) {

    if ($("#" + Mailid).val() != '') {
        var emailPat = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        var emailid = $("#" + Mailid).val();
        var matchArray = emailPat.test(emailid);
        if (!matchArray) {

            writeMultipleMessage("error", msg, Mailid)
            
            return false;
        }
        else {
            writeMultipleMessage("error", "", Mailid);
            return true;
        }
    } else {
        writeMultipleMessage("error", "", Mailid);
        return true;
    }
}
//validation for email id 
function validateEmailID(id, msg) {
    if ($("#" + id).val() != "") {
        var Email = $("#" + id).val();

        var EmailValid = EmailValidation(id);
        if (EmailValid == false) {
            writeMultipleMessage("error", msg, id);
            return false;
        }
        else {
            writeMultipleMessage("error", "", id);
            return true;
        }
    }
    return true;
}
//validate null values for all the textboxes
function validateMobileNo(id, msg) {
    if ($("#" + id).val() == "") {
        writeMultipleMessage("error", '', id);
        return true;
    }

    else {
        if ($("#" + id).val().length < 10) {
            writeMultipleMessage("error", msg, id);
            return false;
        }
        else {
            writeMultipleMessage("error", '', id);
            return true;

        }

    }
}
function validateTextBox(id, msg) {
    if ($("#" + id).val() == null || $("#" + id).val() == "") {
        writeMultipleMessage("error", msg, id);
        return false;
    }

    else {
        writeMultipleMessage("error", '', id);
        return true;
    }
}
function validateTextBox1(e) {
    id = e.id;
    mess = e.name.split('.');
    msg = mess[1];
    if ($("#" + id).val() == null || $("#" + id).val() == "") {
        writeMultipleMessage("error", 'Please Provide ' + msg, id);
        return false;

    }

    else {
        writeMultipleMessage("error", '', id);
        return true;

    }
}

function onlyAlphabets(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (!((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)||(charCode==8)||(charCode==32)))
        return false;
    return true;

}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (((charCode != 8 && charCode < 48) || charCode > 57))
        return false;
    return true;
}
function isNumberTabkey(event) {
    var event = event || window.event;
    var val = event.target.value;
    //alert(val);
    var filtered = val.replace(/[^0-9]/g, '');
    //alert(filtered);
    if (filtered !== val) {
        event.target.value = filtered;
        event.target.className += " error";
    }
    var charCode = (event.which) ? event.which : event.keyCode
    //alert(charCode);
    if ((charCode < 48 || charCode > 57 || charCode == 8))
        return false;

    //    thousandseparatorwithID($("#" + evt.id).val())
    return true;
}

function isNumberKeyWithDecimal(txtHeight) {
    var numPath = /^[0-9]+ ?(\'|ft|cm|meters|feet|in|inches|\")?( *[1-9]+ ?(\"|inches|in|cm)?)?$/;
    var NumericDecimalID = $("#" + txtHeight).val();
    var matchArray = numPath.test(NumericDecimalID);

    if (!matchArray) {
        writeMultipleMessage("error", "Please enter valid height", txtHeight)
        return false;
    }
    else {
        writeMultipleMessage("error", "", txtHeight);
        return true;
    }
   
}
function validateFloatKeyPress(el, evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57)) {
        return false;
    }
    if (el.value.indexOf(".") != -1) {
        var dotPostion = el.value.indexOf(".");
        dotPostion = dotPostion + 4;
        var length = el.value.length;
        if (dotPostion <= length) {
            return false;
        }
    }


    return true;
}

//this method for filling data from one address control to another address control
function CopyAddressesControlToControl(fillableAddress, filledAddress) {
    $('#' + fillableAddress + 'Address1').val($('#' + filledAddress + 'Address1').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'Address2').val($('#' + filledAddress + 'Address2').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'Address3').val($('#' + filledAddress + 'Address3').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'Pincode').val($('#' + filledAddress + 'Pincode').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'City').val($('#' + filledAddress + 'City').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'District').val($('#' + filledAddress + 'District').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'State').val($('#' + filledAddress + 'State').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'Country').val($('#' + filledAddress + 'Country').val()).attr('readonly', 'readonly');

    writeMultipleMessage("error", "", fillableAddress + 'Address1');
    writeMultipleMessage("error", "", fillableAddress + 'Address2');
    writeMultipleMessage("error", "", fillableAddress + 'Address3');
    writeMultipleMessage("error", "", fillableAddress + 'Pincode');
    writeMultipleMessage("error", "", fillableAddress + 'City');
    writeMultipleMessage("error", "", fillableAddress + 'District');
    writeMultipleMessage("error", "", fillableAddress + 'State');
    writeMultipleMessage("error", "", fillableAddress + 'Country');


}
function RemoveAddressDetails(ObjAddressControl) {
    $('#' + ObjAddressControl + 'Address1').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'Address2').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'Address3').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'Pincode').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'City').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'District').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'State').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'Country').val('').attr('readonly', false);

}

function CopyAddressesControlToControlNew(fillableAddress, filledAddress) {
  
    $('#' + fillableAddress + 'Address1').val($('#' + filledAddress + 'Address1').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'Address2').val($('#' + filledAddress + 'Address2').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'Address3').val($('#' + filledAddress + 'Address3').val()).attr('readonly', 'readonly');
    
    $('#' + fillableAddress + 'ddlProvince').val($('#' + filledAddress + 'ddlProvince').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'ddlDistrict').val($('#' + filledAddress + 'ddlDistrict').val()).attr('readonly', 'readonly');
    $('#' + fillableAddress + 'ddlDistrict').append($('<option value=\'' + $('#' + filledAddress + 'ddlDistrict').val() + '\'>' + $('#' + filledAddress + 'ddlDistrict option:selected').text() + '</option>'));
    
    $('#' + fillableAddress + 'ddlCity').append($('<option value=\'' + $('#' + filledAddress + 'ddlCity').val() + '\'>' + $('#' + filledAddress + 'ddlCity option:selected').text() + '</option>'));
    
    $('#' + fillableAddress + 'ddlCity').val($('#' + filledAddress + 'ddlCity').val()).attr('readonly', 'readonly');
  
    $('#' + fillableAddress + 'Pincode').val($('#' + filledAddress + 'Pincode').val()).attr('readonly', 'readonly');

    writeMultipleMessage("error", "", fillableAddress + 'Address1');
    writeMultipleMessage("error", "", fillableAddress + 'Address2');
    writeMultipleMessage("error", "", fillableAddress + 'ddlProvince');
    writeMultipleMessage("error", "", fillableAddress + 'ddlDistrict');
    writeMultipleMessage("error", "", fillableAddress + 'ddlCity');
    writeMultipleMessage("error", "", fillableAddress + 'Pincode');
}

function RemoveAddressDetailsNew(ObjAddressControl) {
    $('#' + ObjAddressControl + 'Address1').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'Address2').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'Address3').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'ddlProvince').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'ddlDistrict').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'ddlCity').val('').attr('readonly', false);
    $('#' + ObjAddressControl + 'Pincode').val('').attr('readonly', false);
}


function showpopup() {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title: 'Search Client Code',
        buttons: [{
            id: 'btnClose',
            label: 'Close',
            cssClass: 'btn-primary',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }],
        closable: false,
        style: "z-index:10050",
        size: 'size-large',
        cssClass: 'login-dialog',
        message: function (dialog) {
            var $message = $('<div></div>');
            var pageToLoad = dialog.getData('pageToLoad');
            $message.load(pageToLoad);

            return $message;
        },
        data: {
            'pageToLoad': '../../Policy/SearchClient'
        }
    });

}

function SearchClient() {
    $.post("../../Policy/SearchClientCode", $("#frmClientSearch").serialize())
        .done(function (data) {
            $("#ClientSearchDetail").html(data);
            $("#btnsearchId").show();
        }).
        fail(function () {
            $("#ClientSearchDetail").html("Failed to fetch data");
        });
}

// Serializing a form into an Object to Json stringify
$.fn.serializeObject = function () {
    var result = {};
    var extend = function (i, element) {
        var node = result[element.name];

        // If node with same name exists already, need to convert it to an array as it
        // is a multi-value field (i.e., checkboxes)

        if ('undefined' !== typeof node && node !== null) {
            if ($.isArray(node)) {
                node.push(element.value);
            } else {
                result[element.name] = [node, element.value];
            }
        } else {
            result[element.name] = element.value;
        }
    };

    $.each(this.serializeArray(), extend);
    return result;
};
//Added for Payment Success message
function ShowPolicySuccess() {
    if ($("#hdnProductType").val() == "Motor") {
        $('#currencyFloatbalanceAfterPayment').removeAttr("disabled");
        var RestOfFloatBal = $('#currencyFloatbalanceAfterPayment').val();
        $("#ddlTransaction").attr("disabled", false);
        $("#ddlCoverType").attr("disabled", false);
        $("#ddlBranchCode").attr("disabled", false);
        EnableDisableAllControls("pnlClientBody", true);
        EnableDisableAllControls("pnlVehicleBody", true);
        EnableDisableAllControls("pnlCoverBody", true);
        EnableDisableAllControls("pnlPrevBody", true);
        EnableDisableAllControls("pnlInsBody", true);
        EnableDisableAllControls("pnlDocumentBody", true);
        EnableDisableAllControls("VehicleDetails_CubicCapacity", true);
        EnableDisableAllControls("VehicleDetails_SeatingCapacity", true);
        EnableDisableAllControls("VehicleDetails_GVW", true);
        EnableDisableAllControls("ddlGender", true);
        EnableDisableAllControls("ChkBox13", true);
        EnableDisableAllControls("DivCover13", true);
        $("#txtnoofpassengers").attr("disabled", false);
        EnableDisableAllControls("ChkBox1", true);
        EnableDisableAllControls("ChkBox2", true);
        EnableDisableAllControls("divPermanentAddress", true);
        $("#divCity")[0].lastChild.disabled = false;
        $("#divState")[0].lastChild.disabled = false;
        $("#divDistrict")[0].lastChild.disabled = false;
        $("#divCountry")[0].lastChild.disabled = false;
        $("#CustomerTypeIndividual").attr("disabled", false);
        $("#CustomerTypeCorporate").attr("disabled", false);
        x = $("#frmProductPayment").serialize();
        $.post("../../Motor/SaveMotorMain?FloatBal=" + RestOfFloatBal, $("#motormain").serialize())
            .done(function (data) {
                $('#currencyFloatbalanceAfterPayment').attr("disabled", "disabled");
                $("#ddlTransaction").attr("disabled", true);
                $("#ddlCoverType").attr("disabled", true);
                $("#ddlBranchCode").attr("disabled", true);
                if (data.PolicyNum != "Error") {
                    if (data.PolicyNum != undefined) {
                        $("#hdnPolicyNumber").val(data.PolicyNum);
                        BootstrapDialog.show({
                            type: BootstrapDialog.TYPE_DANGER,
                            title: 'Success',
                            closable: true,
                            size: 'size-large',
                            message: "Your Policy is successfully made live.Your Policy Number is " + data.PolicyNum + ".Your ClientCode is " + data.ClientCode,
                            buttons: [{
                                label: 'OK',
                                action: function (dialogitself) {
                                    dialogitself.close();
                                }
                            }]
                        });
                    }
                    else {
                        alert("Error");
                    }
                    $("#divGeneratePDF").show();
                }
                else {
                    BootstrapDialog.show({
                        type: BootstrapDialog.TYPE_DANGER,
                        title: 'Note',
                        closable: true,
                        size: 'size-large',
                        message: "Error in creating Policy.Reason:" + data.Error,
                        buttons: [{
                            label: 'OK',
                            action: function (dialogItself) {
                                dialogItself.close();
                            }
                        }]
                    });
                }
            }).

            fail(function () {
                alert("Error in Payment");
            });
    }
    else if ($("#hdnProductType").val() == "Health") {

        $('#currencyFloatbalanceAfterPayment').removeAttr("disabled");
        var RestOfFloatBal = $('#currencyFloatbalanceAfterPayment').val();
        $.post("../../Health/SaveHealthDetailsWithFloatPayment?FloatBal=" + RestOfFloatBal, $("#frmAllHealthProducts").serialize())
            .done(function (data) {
                $('#currencyFloatbalanceAfterPayment').attr("disabled", "disabled");
                if (data.PolicyNum != "Error") {
                    if (data.PolicyNum != undefined) {
                        $("#hdnPolicyNumber").val(data.PolicyNum);
                        BootstrapDialog.show({
                            type: BootstrapDialog.TYPE_DANGER,
                            title: 'Success',
                            closable: true,
                            size: 'size-large',
                            message: "Your Policy is successfully made live.Your Policy Number is " + data.PolicyNum + ".Your ClientCode is " + data.ClientCode,
                            buttons: [{
                                label: 'OK',
                                action: function (dialogitself) {
                                    dialogitself.close();
                                }
                            }]
                        });
                    }

                    else {
                        BootstrapDialog.alert("Error");
                    }
                    $("#divGeneratePDF").show();
                }
                else {
                    BootstrapDialog.show({
                        type: BootstrapDialog.TYPE_DANGER,
                        title: 'Note',
                        closable: true,
                        size: 'size-large',
                        message: "Error in creating Policy.Reason:" + data.Error,
                        buttons: [{
                            label: 'OK',
                            action: function (dialogItself) {
                                dialogItself.close();
                            }
                        }]
                    });
                }
            }).

            fail(function () {
                BootstrapDialog.alert("Error in Payment");
            });


    }

}
function validateIsNullNumeric(elementId, message) {
    var searchReg = /^[0-9-]+$/
    if ($('#' + elementId).val() == null || $('#' + elementId).val() == "") {
        writeMultipleMessage("error", message, elementId);
        return false;
    }

    else if (!searchReg.test($('#' + elementId).val())) {
        writeMultipleMessage('error', message, elementId);
        return false;
    }
    else {
        writeMultipleMessage('error', '', elementId);
        return true;
    }
}

function validateDropDown(id, message) {
    if ($("#" + id + " :selected").val() == null || $("#" + id + " :selected").val() == "") {
        writeMultipleMessage("error", message, id);
        return false;
    } else {
        writeMultipleMessage("error", "", id);
        return true;
    }
}
//Validate the password
function ValidateNewPassword(password) {
    var isValid = "true";
    var alphabets = new RegExp('[a-zA-Z]', 'g');
    var number = new RegExp('[0-9]', 'g');
    var splChar = new RegExp("[!#$%^&*@()]", 'g');
    if (password.length >= 8) {
        isValid = alphabets.test(password);
        if (isValid) {
            isValid = number.test(password);
            if (isValid) {
                isValid = splChar.test(password);
                if (isValid) {
                    isValid = "true";
                }
                else {
                    isValid = "New Password must have atleat 1 special charecter";
                }
            }
            else {
                isValid = "New Password must have atleat 1 number";
            }
        }
        else {
            isValid = "New Password must have atleat 1 alphabet";
        }
    }
    else {
        isValid = "New Password must have atleat 8 charecters";
    }
    return isValid;
}
function GenerateIRNO() {
    ErrorCode = true;
    Validatedropdownlist("ddlInspectionAgency", "Please Select Inspection agency");
    CheckForMandatoryFields('txtClientName', 'please provide name');
 
    if (ErrorCode == true) {
        $.post("../../Panel/GetIRNo", $("#frmRequestVechileInspection"))
            .done(function (data) {
                var QuoteNumber = data.QuoteNumber;
                var AgencyName = data.AgencyName;
                var PhoneNo = data.PhoneNo;
                var IRNo = data.IRNo;
                BootstrapDialog.show({
                    type: BootstrapDialog.TYPE_DANGER,
                    title: 'Info',
                    message: "Request for Break-in Inspection is successfully raised for Quote Number" + QuoteNumber +
                    "Please contact" + AgencyName + " Inspection Agency on Phone Number/s" + PhoneNo + " for inspection of the Vehicle" +
                    "Your Vehicle inspection reference number is" + IRNo,
                    buttons: [{
                        label: 'OK',
                        action: function (dialogItself) {
                            dialogItself.close();
                        }
                    }]
                });
            })
            .fail(function (data) {
            })
    }
}
function ValidateInspectionNumber() {
}

//Validate date format by id
function ValidateDateFormat(id, msg) {
    var date = $("#" + id).val();
    if (date != "") {
        var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/);
        var result = dtRegex.test(date);
        if (!result) {
            writeMultipleMessage("error", msg, id);
            return false;
        }
        else {
            writeMultipleMessage("error", '', id);
            return true;
        }
    }
}

function ValidateRequiredAndDate(id, msg) {

    validateTextBox(id, msg)
    ValidateDateFormat(id, msg);
}


function ConvertingUTCtoDate(date) {
    var DateValue = date;

    var DateMonth = DateValue.getMonth().toString();
    DateMonth = parseInt(DateMonth) + 1;
    if (DateMonth <= 9) {
        DateMonth = "0" + DateMonth;
    }
    var DateDay = DateValue.getDate().toString();
    if (DateDay.length == 1) {
        DateDay = "0" + DateDay;
    }
    DateValue = DateDay + '-' + DateMonth + '-' + DateValue.getFullYear().toString();
    return DateValue;

}
function clearAllErrors() {
    errorArray = new Array();
    var container = $("#containerMain");
    container.html("");
    errorMessageCount = 0;
    closePopUp();
}
function isAlphaNumberKeyback(evt) {
    var event = evt || window.evt;
    var val = event.target.value;
    //alert(val);
    var filtered = val.replace(/[^a-z0-9]/gi, '');
    //alert(filtered);
    if (filtered !== val) {
        event.target.value = filtered;
        event.target.className += " error";
    }
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (!((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || (charCode >= 48 && charCode <= 57) || (charCode == 8)))
        return false;
    return true;
}
function isSpace(evt) {
    var event = evt || window.evt;
    var val = event.target.value;
    //alert(val);
    var filtered = val.replace(/\s/g, '');
    //alert(filtered);
    if (filtered !== val) {
        event.target.value = filtered;
        event.target.className += " error";
    }
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 32)
        return false;
    return true;
}

function isNumberKeyandBackSpacekey(evt) {  //Only Numeric values

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (((charCode != 8 && charCode < 48) || charCode > 57))
        return false;
    return true;
}

function isNumberKeyWithDot(evt) {   //decimal values with one dot
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (event.keyCode > 47 && event.keyCode < 58 || event.keyCode == 46) {
        var value = event.srcElement.value;
        if (value.indexOf('.') > -1 && event.keyCode == 46) {//ChecKing wether '.' is eixst in value 
            return false;
        }
        else {
            return true;
        }
    }
    return false;
}
function isNumberKeyWithDot1(evt) {
    debugger;
    //decimal values with one dot
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 8 || charCode == 46) {
        return true;
    }
    else if (charCode > 47 && charCode < 58 || charCode == 46) {
        var value = event.srcElement.value;
        if (value.indexOf('.') > -1 && charCode == 46) {//ChecKing wether '.' is eixst in value 
            return false;
        }
        else {
            return true;
        }
    }
    return false;
}

function isAlphaNumberKeybackwithOutSymbol(evt) { //Alpha numeric values with out soecial char
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (!((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || (charCode >= 48 && charCode <= 57) || (charCode == 8) || (charCode == 32)))
        return false;
    return true;
}
function isAlphaNumberKeybackwithOutSymbolSpace(evt) { //Alpha numeric values with out special char and space
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (!((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || (charCode >= 48 && charCode <= 57) || (charCode == 8)))
        return false;
    return true;
}

/**
 * This function will count total number of mandatory fields by mandatory class
 * Adds blank field highlight class for blank fields
 * @param {any} id panel id
 * @param {any} load optional if on page load send true
 */
//var load=false;
function SetMandatoryHighlightBlank(id, load) {
        var totalCount = 0;
        var actualCount = 0;
        $('#' + id + ' textarea').each(function () {
            var value = $(this).val();
            var clas = $(this).parent().closest('div').find('label').find('span').attr('class');
            if (clas != undefined && clas.toUpperCase() == "MANDATORY") {
                totalCount = totalCount + 1;
                if (value != "" && value != null) {
                    actualCount = actualCount + 1;
                }
            }
            if (value == "" || value == null) {
                $(this).addClass("blank-field-hightlight");
            }
            else {
                $(this).removeClass("blank-field-hightlight")
            }
        });
        $('#' + id + ' select').each(function () {
            var value = $(this).val();
                var clas = $(this).parent().closest('div').find('label').find('span').attr('class');
                if (clas != undefined && clas.toUpperCase() == "MANDATORY") {
                    totalCount = totalCount + 1;
                    if (value != "" && value != null) {
                        actualCount = actualCount + 1;
                    }
                }
                if (value == "" || value == null || value == "SELECT") {
                    $(this).addClass("blank-field-hightlight");
                }
                else {
                    $(this).removeClass("blank-field-hightlight")
                }
            
        });
        $('#' + id + ' input:text').each(function () {
            var value = $(this).val();
            var clas = $(this).parent().closest('div').find('label').find('span').attr('class');
            if (clas != undefined && clas.toUpperCase() == "MANDATORY") {
                totalCount = totalCount + 1;
                if (value != "" && value != null) {
                    actualCount = actualCount + 1;
                }
            }
            if (value == "" || value == null) {
                $(this).addClass("blank-field-hightlight");
            }
            else {
                $(this).removeClass("blank-field-hightlight")
            }
        });
        if (load) 
            $('#' + id).find('span#spnTotalCnt').text(totalCount);
            $('#' + id).find('span#spnActualCnt').text(actualCount);
}
function NumberKeywithtwoDecimal(event) {
    //var charCode = (event.which) ? event.which : event.keyCode
    //if (((charCode != 8 && charCode < 48) || charCode > 57))
    //    return false;
    if (event.which < 46 || event.which > 59) {
        event.preventDefault();
    } // prevent if not number/dot

    if (event.which == 46 && $(this).val().indexOf('.') != -1) {
        event.preventDefault();
    }
}

function ValidName(Name) {    
    if (Name.length >= 3) {

        if (/^\s/.test(Name)) {
            return false;
        } 
        if (/^'/.test(Name)) {
            return false;
        }
        if (/(.)\1\1/.test(Name)) {
            return false;
        } 

        if (/\s\s/.test(Name)) {
            return false;
        }
        var res = "''";
        if ((Name.indexOf(res) != -1)) {
            return false;
        }
    }
    else {
        return false;
    }
}
function ValidateAddress(Address) {   
    if (Address != null && Address != "undefined" && Address != "") {
        if (Address.length >= 3) {
            if (/^\s/.test(Address)) {
                return false;
            }
            if (/^,/.test(Address)) {
                return false;
            } 
            if (/^'/.test(Address)) {
                return false;
            }
            if (/^-/.test(Address)) {
                return false;
            } 
            if (/^#/.test(Address)) {
                return false;
            } 
            if (/^\./.test(Address)) {
                return false;
            } 
            if (/^\//.test(Address)) {
                return false;
            } 
            if (/\s\s/.test(Address)) {
                return false;
            } 
            if (/(.)\1\1/.test(Address)) {
                return false;
            } 
            var res = " -";
            var reslt = " '";
            var result = ", ";
            if ((Address.indexOf(res) != -1)) {
                return false;
            } 
            if (Address.indexOf(reslt) != -1) {
                return false;
            } 
            if (Address.indexOf(',') > -1) {
                if ((Address.indexOf(result) != -1)) {
                    return true;
                }
                else {
                    return false;
                }
            } 
        }
        else {
            return false;
        }
    }
}
function RemoveZero(id) {
    var res = $('#' + id).val();
    if (res == '')
    {
        $('#' + id).val('');
    }
   else if (res == "0") {
        $('#' + id).val('');
    }
    else if(res == "NAN"){
        $('#' + id).val('');
    }
    else {
        $('#' + id).val(parseInt(res.replace(/,/g, '')).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
    }
}
function AddComma(id) {
    var res = $('#' + id).val();
    if (res == '') {
        res="0"
        $('#' + id).val(parseInt(res));
    }
    else if (res == "0") {
        res = "0"
        $('#' + id).val(parseInt(res));
    }
    else if (res == "NAN") {
        res = "0"
        $('#' + id).val(parseInt(res));
    }
    else {
        $('#' + id).val(parseInt(res.replace(/,/g, '')).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
    }
}

//(function ($) {

//    $.fn.hideKeyboard = function () {
//        debugger;
//        var inputs = this.filter("input").attr('readonly', 'readonly'); // Force keyboard to hide on input field.
//        var textareas = this.filter("textarea").attr('disabled', 'true'); // Force keyboard to hide on textarea field.
//        setTimeout(function () {
//            inputs.blur().removeAttr('readonly');  //actually close the keyboard and remove attributes
//            textareas.blur().removeAttr('disabled');
//        }, 100);
//        return this;
//    };
//}(jQuery));


//$('#divNomineeDateOfBirth0').click(function () {
//    alert('hi');
//})
$(document).ready(function(){
$('.invisible').click(function () {
      $(this).toggleClass("checkboxactive");
})
})