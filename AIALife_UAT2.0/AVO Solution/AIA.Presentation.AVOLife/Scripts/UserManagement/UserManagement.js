
var ErrorCode = true;
function validateIsNullText(elementId) {
    var searchReg = /^[a-zA-Z0-9-]+$/
    if ($('#' + elementId).val() == null || $('#' + elementId).val() == "") {
        writeMultipleMessage("error", "Please provide an input value", elementId);
        return false;
    }
    else if (!searchReg.test($('#' + elementId).val())) {
        writeMultipleMessage("error", "Please provide an input value", elementId);
        return false;
    }
    else {
        writeMultipleMessage("error", "", elementId);
    }
}

function validateIsNullNumeric(elementId) {
    var searchReg = /^[0-9-]+$/
    if ($('#' + elementId).val() == null || $('#' + elementId).val() == "") {
        writeMultipleMessage("error", "Please provide an input value", elementId);
        return false;
    }

    else if (!searchReg.test($('#' + elementId).val())) {
        writeMultipleMessage('error', 'Please provide an input value', elementId);
        return false;
    }
    else {
        writeMultipleMessage('error', '', elementId);
    }
}

function validateOnlyNumber(elementId, e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||

            (e.keyCode == 65 && e.ctrlKey === true) ||

            (e.keyCode >= 35 && e.keyCode <= 40)) {

        return;
    }
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}


function submitPayementDetails() {

    if (!$("input[name='IsCdt']:checked").val()) {
        writeMultipleMessage('error', 'Please select a payment mode', 'rdbtnCDTPaymntMode');

    }
    else {
        writeMultipleMessage('error', '', 'rdbtnCDTPaymntMode');
    }


    if (!$("input[name='userExtension.FloatType']:checked").val()) {
        writeMultipleMessage('error', 'Please select a float type', 'rdbtnCentralised');

    }
    else {
        writeMultipleMessage('error', '', 'rdbtnCentralised');
    }

    if (validateIsNullText('txtFloatNumber') || validateIsNullNumeric('txtFloatLimit') || validateIsNullNumeric('txtLimitDays') || validateIsNullNumeric('txtAvailableFloat')) {
        return false;
    }
    $('#btnPaymentSubmit').submit()

}

function validateAllDetails() {
    validateType();
    validateLevel();
    validateCode();
    validateOffice();
    validateChannel();
    validateUserDetails();
    validateParent();
    validateSelectSM();
    validateDirRep();
    validateGender();
    validateFname();
    validateLname();
    validateDOB();
    validateEmailID();
    IsValidNumber();
    validateLicenceNo();
    validateLoginName();
}
function validateType() {
    if ($("#ddlUserType").val() == "") {
        writeMultipleMessage("error", "please select type", 'ddlUserType');
    }
    else {
        writeMultipleMessage("error", "", 'ddlUserType');
    }
}
function validateLevel() {
    if ($("#ddlUserLevel").val() == "") {
        writeMultipleMessage("error", "please select level", 'ddlUserLevel');
    }
    else {
        writeMultipleMessage("error", "", 'ddlUserLevel');
    }
}
function validateCode() {
    if ($("#txtUserCode").val() == "") {
        writeMultipleMessage("error", "please enter code", 'txtUserCode');
    }
    else {
        writeMultipleMessage("error", "", 'txtUserCode');
    }
}
function validateOffice() {
    if ($("#txtBranchCode").val() == "") {
        writeMultipleMessage("error", "please select office", 'txtBranchCode');
    }
    else {
        writeMultipleMessage("error", "", 'txtBranchCode');
    }
}
function validateChannel() {
    if ($("#txtChannel").val() == "") {
        writeMultipleMessage("error", "please select channel", 'txtChannel');
    }
    else {
        writeMultipleMessage("error", "", 'txtChannel');
    }
}
function validateParent() {
    if ($("#ddlUserLevel").val() == "L1" || $("#ddlUserLevel").val() == "L2") {
        if ($("#ddlSubUserParent1233").val() == "") {
            writeMultipleMessage("error", "please enter parent,value is must", 'ddlSubUserParent1233');
        }
        else {
            writeMultipleMessage("error", "", 'ddlSubUserParent1233');
        }
    }
    else {
        writeMultipleMessage("error", "", 'ddlSubUserParent1233');
    }
}
function validateSelectSM() {
    if ($("#txtSM").val() == "") {
        writeMultipleMessage("error", "please select sm code", 'txtSM');
    }
    else {
        writeMultipleMessage("error", "", 'txtSM');
    }
}
function validateUserDetails() {
    if ($("#txtChannel").val() == "Banca" && ($("#ddlUserType").val() != "Corporate" && $("#ddlUserType") != "Referral")) {
        if ($("#txtReferalName").val() == "" || $("#txtReferalName").val() == "undefined") {
            writeMultipleMessage("error", "please select referal value", 'txtReferalName');
        }
        else {

            writeMultipleMessage("error", "", 'txtReferalName');
        }
    }
    else {
        writeMultipleMessage("error", "", 'txtReferalName');
    }
}
function validateDirRep() {
    if ($("#ddlDirRep").val() == "") {
        writeMultipleMessage("error", "please select direct reporting value", 'ddlDirRep');
    }
    else {
        writeMultipleMessage("error", "", 'ddlDirRep');
    }
}
function validateOfficeDetails() {
    if ($("#ddlDirRep").val() == $("#ddlDotRep").val()) {
        writeMultipleMessage("error", "direct and dotted reporting values cannot be same", 'ddlDotRep');
    }
    else {
        writeMultipleMessage("error", "", 'ddlDotRep');
    }
}
function validateFname() {
    if ($("#txtFirstName").val() == "") {
        writeMultipleMessage("error", "please enter first name", 'txtFirstName');
    }
    else {
        writeMultipleMessage("error", "", 'txtFirstName');
    }
}
function validateLname() {
    if ($("#txtLastName").val() == "") {
        writeMultipleMessage("error", "please enter last name", 'txtLastName');
    }
    else {
        writeMultipleMessage("error", "", 'txtLastName');
    }
}
function validateGender() {

    if ($("#ddlSalutation").val() == "") {
        writeMultipleMessage("error", "please select title", 'ddlSalutation');
    }
    else {
        writeMultipleMessage("error", "", 'ddlSalutation');
        if ($("#ddlSalutation").val() == "Mr") {
            $("#divGender").show();
            document.getElementById("rbGenderMale").checked = true;
        }
        if (($("#ddlSalutation").val() == "Ms") || ($("#ddlSalutation").val() == "Miss")) {
            $("#divGender").show();
            document.getElementById("rbGenderFemale").checked = true;

        }
        if ($("#ddlSalutation").val() == "Messrs") {

            $("#divGender").hide();
        }
    }

}
function ValidateDOB() {
    if ($("#txtDOB").val() == "") {

        writeMultipleMessage("error", "please select your birth date", 'txtDOB');
    }
    else {

        writeMultipleMessage("error", "", 'txtDOB');
    }
}

function ValidateMobileNumber() {
    writeMultipleMessage("error", "", 'txtmobile');
    if ($("#txtmobile").val() == "") {
        writeMultipleMessage("error", "Please enter your mobile number", 'txtmobile');
        return false;
    }
    else if ($("#txtmobile").val().length < 10) {
        writeMultipleMessage("error", "Please enter a ten digit number", 'txtmobile');
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'txtmobile');
        return true;
    }
    //else if ($("#txtmobile").val() != "") {
    //    var MobNoPat = /^[7-9][0-9]{9}$/;
    //    var mobno = $("#txtmobile").val();
    //    var matchArray = MobNoPat.test(mobno);
    //    if (!matchArray) {
    //        writeMultipleMessage("error", "Please enter mobile number starting with 7/8/9", 'txtmobile');
    //        return false;
    //    }
    //    else {
    //        writeMultipleMessage("error", "", 'txtmobile');
    //        return true;
    //    }

    //}
}




function validateFlatNo() {
    if ($("#objuserAddressAddress1").val() == "") {
        writeMultipleMessage("error", "please enter flat no", 'objuserAddressAddress1');
    }
    else {
        writeMultipleMessage("error", "", 'objuserAddressAddress1');
    }
}
function validateState() {
    if ($("#objuserAddressddlState").val() == "") {
        writeMultipleMessage("error", "please select state", 'objuserAddressddlState');
    }
    else {
        writeMultipleMessage("error", "", 'objuserAddressddlState');
    }
}
function validateDistrict() {
    if ($("#objuserAddresslblddlDistrict").val() == "") {
        writeMultipleMessage("error", "please select district", 'objuserAddresslblddlDistrict');
    }
    else {
        writeMultipleMessage("error", "", 'objuserAddresslblddlDistrict');
    }
}
function validateCity() {
    if ($("#objuserAddresslblddlCity").val() == "") {
        writeMultipleMessage("error", "please select city", 'objuserAddresslblddlCity');
    }
    else {
        writeMultipleMessage("error", "", 'objuserAddresslblddlCity');
    }
}
function validateAreaOrPincode() {
    if ($("#objuserAddresslblddlArea").val() == "") {
        writeMultipleMessage("error", "please select area pincode", 'objuserAddresslblddlArea');
    }
    else {
        writeMultipleMessage("error", "", 'objuserAddresslblddlArea');
    }
}
function validateLicenceNo() {
    if ($("#txtLicenceNumber").val() == "") {
        writeMultipleMessage("error", "please enter licence no", 'txtLicenceNumber');
    }
    else {
        writeMultipleMessage("error", "", 'txtLicenceNumber');
    }
}
function validateLoginName() {
    if ($("#txtLoginID").val() == "") {
        writeMultipleMessage("error", "please enter login name", 'txtLoginID');
    }
    else {
        writeMultipleMessage("error", "", 'txtLoginID');
    }
}

function ShowOrg() {
    $("#divFirstName").hide();
    $("#divLastName").hide();
    $("#divCorporatename").show();
    $("#divContact").show();
    $("#divMaritalStatus").hide();
    $("#IMDdob").hide();
    $("#divContactPerson").show();
    $("#divSalutation").hide();
    $("#divGender").hide();
    writeMultipleMessage("error", "", 'dpdob');
}
function ShowIndividual() {
    $("#divFirstName").show();
    $("#divLastName").show();
    $("#IMDdob").show();
    $("#divMaritalStatus").show();
    $("#divCorporatename").hide();
    $("#divContact").hide();
    $("#divContactPerson").hide();
    $("#divSalutation").show();
    $("#divGender").show();
}
function ShowLTDS() {
    $(".divLTDS").show();
    $("#txtTDS").val('0');
}
function HideLTDS() {
    $(".divLTDS").hide();
    $("#txtTDS").val('5');
}
function ValidateChangeStatus() {
    var Remarks = $("#ddlImdstatus option:selected").val();
    writeMultipleMessage("error", "", 'ddlImdstatus');
    var remarksText = $("#txtImdRemarks").val();
    if (Remarks != "1" && Remarks != "" && remarksText == "") {
        writeMultipleMessage("error", "Please enter Remarks", 'txtImdRemarks');
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'txtImdRemarks');
        return true;
    }
}

function ValidateDateofBirth() {
    var dob = $("#dpdob").val();
    var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/);
    var result = dtRegex.test(dob);
    if (result) {
        var DateOfBirth = (dob).split("/");
    }
    else {
        var DateOfBirth = (dob).split("-");
    }
    Year = DateOfBirth[2];
    if (dob == "") {
        writeMultipleMessage("error", "please enter the date of birth", 'dpdob');
        return false;
    }
    else if (dob != "") {
        result = ValidateDateFormat('dpdob', 'Invalid date of birth format');
        if (!result) {
            return false;
        }
    }
    if ((Year) < 1900) {
        writeMultipleMessage("error", "year cannot be less than 1900", 'dpdob');
        return false;
    }
    else if (DateOfBirth != "") {
        dobYear = DateOfBirth[2];
        dobMonth = DateOfBirth[1];
        dobDay = DateOfBirth[0];
        today_date = new Date();
        today_year = today_date.getYear();
        today_month = today_date.getMonth();
        today_day = today_date.getDate();
        age = (today_year + 1900) - dobYear;
        if (today_month < (dobMonth - 1))
        { age--; } if (((dobMonth - 1) == today_month) && (today_day < dobDay))
        { age--; }
        if (age < 18) {
            writeMultipleMessage("error", "Age should be equal to or more than 18 years.", 'dpdob');
            return false;
        }
        else {
            writeMultipleMessage("error", "", 'dpdob');
            return true;
        }
    }

}
function ValidateSmcode() {

    var smcode = $("#ddlbranch option:selected").val();
    writeMultipleMessage("error", "", 'ddlbranch');
    if ($('#ddlsmcode').val() == "") {
        writeMultipleMessage("error", "smcode is mandatory", 'ddlsmcode');
    }
    else {
        return true
    }
}
function FillLicenceExpiryDate() {

    var StartDate = ($("#dpLicenseIssue").val());
    var imdType = $("#ddlImdtype option:selected").val();
    //var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/);
    //var result = dtRegex.test(StartDate);
    //if (result) {
    //    var StartDate = (StartDate).split("/");
    //}
    //else {
    //    var StartDate = (StartDate).split("-");
    //}

    if ($('#hdnImdDetailsId').val() == 0 || $('#hdnImdDetailsId').val() == null) {
        $.ajax({
            type: "GET",
            url: "../../UM/UserManagement/SetLicenseExpiryDate?LicenseIssueDate=" + StartDate + "&ImdType=" + imdType,
            contentType: 'application/json',
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.ErrMsg == "") {
                    $("#dpLicenseExpiry").val(data.LicenseEndDate);
                    //$("#dpImdTermination").val(data.ImdTerminationDate);
                }
            }
        });
    }
    else {
        $.ajax({
            type: "GET",
            url: "../../UserManagement/SetLicenseExpiryDate?LicenseIssueDate=" + StartDate + "&ImdType=" + imdType,
            contentType: 'application/json',
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.ErrMsg == "") {
                    $("#dpLicenseExpiry").val(data.LicenseEndDate);
                    //$("#dpImdTermination").val(data.ImdTerminationDate);
                }
            }
        });
    }

    //startYear = StartDate[2];
    //var EndYear = parseInt(startYear) + 3;
    //EndDate = StartDate[0] + "/" + StartDate[1] + "/" + EndYear;
    //$("#dpLicenseExpiry").val(EndDate);
    //FillImdTerminationDate()
}
//function FillImdTerminationDate() {
//    var StartDate = ($("#dpLicenseExpiry").val());
//    var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/);
//    var result = dtRegex.test(StartDate);
//    if (result) {
//        var StartDate = (StartDate).split("/");
//    }
//    else {
//        var StartDate = (StartDate).split("-");
//    }
//    startYear = StartDate[2];
//    var EndYear = parseInt(startYear) + 1;
//    EndDate = StartDate[0] + "/" + StartDate[1] + "/" + EndYear;
//    $("#dpImdTermination").val(EndDate);
//}
function ValidateContractType() {
    var contractType = $("#ddlpremium option:selected").val();
    writeMultipleMessage("error", "", 'ddlpremium');
    if ($('#ddlcontract').val() == "") {
        writeMultipleMessage("error", "smcode is mandatory", 'ddlcontract');
    }
    else {
        return true
    }
}
//function ValidateContractType() {

//    var contractType = $("#ddlcontract option:selected").val();
//    writeMultipleMessage("error", "", 'ddlcontract');
//    if ($('#ddlcommission').val() == "") {
//        writeMultipleMessage("error", "smcode is mandatory", 'ddlcommission');
//    }
//    else {

//        return true
//    }
//}
function ShowChildId() {
    $('#IMDChilddetails').show()
}

function showHistory() {
    $('#IMDHistory').show()
}
function ShowIsChilId() {
    $("#divImdcode").hide();
    $("#divImdName").hide();
    $("#divSearchBtn").hide();
    $("#btnChildIDS").hide();
    $("#divReportingUserName").show();
    $("#divReportingUserId").show();
}
function ShowIsParentId() {
    $("#divReportingUserName").hide();
    $("#divReportingUserId").hide();
    $("#divImdcode").show();
    $("#divImdName").show();
    $("#divSearchBtn").show();
    $("#btnChildIDS").show();
}
function ShowImdEnquiry() {
    $('#showimdId').modal('toggle');

    $('#showimdId').modal('show');
}
function ShowChildEnquiry() {
    $('#showchildId').modal('toggle');

    $('#showchildId').modal('show');
}
function showHistory() {
    $('#showhistoryId').modal('toggle');

    $('#showhistoryId').modal('show');
}

function showSearchClient() {

    $('#showClientSearch').modal('toggle');

    $('#showClientSearch').modal('show');
}
function ValidateLicenceNumber() {
    var contractType = $("#ddlImdtype option:selected").val();
    writeMultipleMessage("error", "", 'ddlImdtype');
    if ((contractType == "2" || contractType == "82" || contractType == "83" || contractType == "84" || contractType == "85" || contractType == "86" || contractType == "87" || contractType == "88" || contractType == "89") && $("#txtlicencenumber").val() == "") {
        writeMultipleMessage("error", "Licence Number is mandatory", 'txtlicencenumber');
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'txtlicencenumber');
        return true;
    }
}

function ValidateLicenseIssueDate() {
    var ImdType = $("#ddlImdtype option:selected").val();
    writeMultipleMessage("error", "", 'ddlImdtype');
    writeMultipleMessage("error", "", 'dpLicenseIssue');
    writeMultipleMessage("error", "", 'dpLicenseExpiry');
    if ((ImdType == "1" || ImdType == "2" || ImdType == "82" || ImdType == "83" || ImdType == "84" || ImdType == "85" || ImdType == "86" || ImdType == "87" || ImdType == "88" || ImdType == "89") && $("#dpLicenseIssue").val() == "") {
        writeMultipleMessage("error", "License Issue Date is mandatory", 'dpLicenseIssue');
        return false;
    }
    else if ($("#dpLicenseIssue").val() != "") {
        result = ValidateDateFormat('dpLicenseIssue', 'Invalid License Issue Date format');
        if (result) {
            writeMultipleMessage("error", "", 'dpLicenseIssue');
            FillLicenceExpiryDate();
            return true;
        }
    }
    else {
        writeMultipleMessage("error", "", 'dpLicenseIssue');
        return true;
    }
}

function showSearchImd() {
    $('#showsearchImd').modal('toggle');

    $('#showsearchImd').modal('show');
}
function showNsdlFetch() {
    //$('#fetchNsdl').modal('toggle');
    //$('#fetchNsdl').modal('show');
    $("#txtPanHolderName").val('');
    var PanNo = $("#txtPan").val();
    $.ajax({
        type: "GET",
        url: "../../UM/UserManagement/PanVerification?PanNo=" + PanNo,
        contentType: 'application/json',
        dataType: "json",
        // async: false,
        beforeSend: function () {
            // show gif here, eg:
            $("#divLoading").show();
        },
        complete: function () {
            // hide gif here, eg:
            $("#divLoading").hide();
        },
        success: function (data) {
            if (data == "Error") {
                writeMultipleMessage("error", " Please enter a valid PAN Number", "txtPan");
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", "", "txtPan");
                $("#txtPanHolderName").val(data.PanHolderName);
                $("#txtPan").attr("disabled", "disabled");
                document.getElementById("chkPan").checked = true;
                $("#chkPan").attr("disabled", false);
                return true;
            }
        }
    });
}
function toggleChecked(chkid, divid) {
    if ($('#' + chkid).prop('checked') == true) {
        $('#' + divid + ' input').each(function () {
            $(this).prop('checked', true);
        });
    }
    else {
        $('#' + divid + ' input').each(function () {
            $(this).prop('checked', false);
        });
    }
}


function toggleChecked(chkid, divid) {
    if ($('#' + chkid).prop('checked') == true) {
        $('#' + divid + ' input').each(function () {
            $(this).prop('checked', true);
        });
    }
    else {
        $('#' + divid + ' input').each(function () {
            $(this).prop('checked', false);
        });
    }
}

function clearBankDetails() {
    $(".tbBankDetails").val("");
    $('#ddlPaymentType option:first-child').attr("selected", "selected");
    $('#ddlAcountType option:first-child').attr("selected", "selected");

}

function ValidatePhoneNumber() {
    var phoneNumber = $("#txtofficeno").val();
    if (phoneNumber == "") {
        writeMultipleMessage("error", " Please enter your phone number", "txtofficeno");
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'txtofficeno');
        return true;
    }
    //else if ($("#txtofficeno").val().length < 10) {
    //    writeMultipleMessage("error", "Please enter a ten digit number", 'txtofficeno');
    //    return false;
    //}
    //else if (phoneNumber != "") {
    //    var regExp = /^[7-9][0-9]{9}$/;
    //    var phone = phoneNumber.match(regExp);
    //    if (!phone) {
    //        writeMultipleMessage("error", "please provide valid Phone No", "txtofficeno");
    //        return false;
    //    }
    //    else {
    //        writeMultipleMessage("error", "", 'txtofficeno');
    //        return true;
    //    }
    //}
}

function validateEmail() {
    var result = false;
    var email = $("#txtemail").val();
    if (email == "") {
        writeMultipleMessage("error", " Please enter your email", "txtemail");
        return false;
    }
    else if (email != "") {
        result = validateEmailID('txtemail', 'please provide valid Email Id');
        if (result) {
            writeMultipleMessage("error", "", "txtemail");
        }
        return result;
    }
}

function validateAppointmentDate() {
    var appointmentDate = $("#dpAppointmentdate").val();
    var result = isInRICode();
    var contractType = $("#ddlImdtype option:selected").val();
    var licenseIssueDate = $("#dpLicenseIssue").val();
    var licenseExpiryDate = $("#dpLicenseExpiry").val();

        if (appointmentDate == "" && !result) {
            writeMultipleMessage("error", "please select your Appointment Date", 'dpAppointmentdate');
            return false;
        }
        else if (appointmentDate != "") {
            result = ValidateDateFormat('dpAppointmentdate', 'Invalid Appointment Date format');
            if (!result) {
                return result;
            }
            else if ((contractType == "2" || contractType == "3") && licenseIssueDate == "") {
                result = ValidateLicenseIssueDate();
                if (!result) {
                    return result;
                }
                else {
                    writeMultipleMessage("error", "", 'dpLicenseIssue');
                    return true;
                }
            }
            else if ((contractType == "2" || contractType == "3") && licenseIssueDate != "") {
                FillLicenceExpiryDate();
                return true;
            }
            else {
                writeMultipleMessage("error", "", 'dpAppointmentdate');
                return true;
            }
        }
}
function validateLicenseExpiryDate() {
    var licenseIssueDate = $("#dpLicenseIssue").val();
    var licenseExpiryDate = $("#dpLicenseExpiry").val();
    var contractType = $("#ddlImdtype option:selected").val();

    if ((contractType == "1" || contractType == "2" || contractType == "82" || contractType == "83" || contractType == "84" || contractType == "85" || contractType == "86" || contractType == "87") && licenseExpiryDate == "") {
        writeMultipleMessage("error", "License Expiry Date is mandatory", 'dpLicenseExpiry');
        return false;
    }
    else if (licenseExpiryDate != "") {
        result = ValidateDateFormat('dpLicenseExpiry', 'Invalid License Expiry Date format');
        if (!result) {
            return result;
        }
        else {
            writeMultipleMessage("error", "", 'dpLicenseExpiry');
        }
    }

    if ((contractType == "1" || contractType == "2" || contractType == "82" || contractType == "83" || contractType == "84" || contractType == "85" || contractType == "86" || contractType == "87") && licenseIssueDate != "" && licenseExpiryDate != "") {
        result = CompareTwoDates(licenseIssueDate, licenseExpiryDate, 'dpLicenseExpiry', 'License Expiry Date Should be greater than License Issue Date');
        if (!result) {
            return result;
        }
        else {
            writeMultipleMessage("error", "", 'dpLicenseExpiry');
        }
    }
}

function validateImdTerminationDate() {

    var contractType = $("#ddlImdtype option:selected").val();
    var licenseIssueDate = $("#dpLicenseIssue").val();
    var imdTerminationDate = $("#dpImdTermination").val();

    if ((contractType == "1" || contractType == "2" || contractType == "82" || contractType == "83" || contractType == "84" || contractType == "85" || contractType == "86" || contractType == "87") && imdTerminationDate == "") {
        writeMultipleMessage("error", "Imd Termination Date is mandatory", 'dpImdTermination');
        return false;
    }
    else if (imdTerminationDate != "") {
        result = ValidateDateFormat('dpImdTermination', 'Invalid Imd Termination Date format');
        if (!result) {
            return result;
        }
        else {
            writeMultipleMessage("error", "", 'dpImdTermination');
        }
    }

    if ((contractType == "1" || contractType == "2" || contractType == "82" || contractType == "83" || contractType == "84" || contractType == "85" || contractType == "86" || contractType == "87") && licenseIssueDate != "" && imdTerminationDate != "") {
        result = CompareTwoDates(licenseIssueDate, imdTerminationDate, 'dpImdTermination', 'Imd Termination Date Should be greater than License Issue Date');
        if (!result) {
            return result;
        }
        else {
            writeMultipleMessage("error", "", 'dpImdTermination');
            return true;
        }
    }
}

function ValidatePANNo() {
    var panNo = $("#txtPan").val();
    if (panNo == "") {
        writeMultipleMessage("error", " Please enter your PAN", "txtPan");
        return false;
    }
    else if (panNo != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        if (panNo.search(panPat) == -1) {
            writeMultipleMessage("error", 'Please enter valid Pan No', "txtPan");
            return false;
        }
        else {
            writeMultipleMessage("error", "", "txtPan");
            return true;
        }
    }
}

function ValidateAadhaarNo() {
    var result = false;
    var aadhaarNo = $("#txtAadhaarNo").val();
    if (aadhaarNo == "") {
        writeMultipleMessage("error", " Please enter your Aadhaar No", "txtAadhaarNo");
        result = false;
        return false;
    }
    else if (aadhaarNo.length < 12) {
        writeMultipleMessage("error", 'Aadhaar No Must be 12.', "txtAadhaarNo");
        result = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtAadhaarNo");
        result = true;
        //return true;
    }
    //else if (aadhaarNo != "") {
    //    var aadhaarNoPat = /^\d{4}\s\d{4}\s\d{4}$/;
    //    if (aadhaarNo.search(aadhaarNoPat) == -1) {
    //        writeMultipleMessage("error", 'Please enter valid Aadhaar No', "txtAadhaarNo");
    //        return false;
    //    }
    //    else {
    //        writeMultipleMessage("error", "", "txtAadhaarNo");
    //        return true;
    //    }
    //}
    return result;
}

function validateReportsTo() {
    var result = false;
    var reportsTo = $("#txtReportsTo").val();
    if (reportsTo == "") {
        writeMultipleMessage("error", " Please enter your Reports To", "txtReportsTo");
        result = false;
        return false;
    }
    else if (reportsTo != "" && reportsTo.length < 8) {
        writeMultipleMessage("error", " Please enter Reports To length must be 8", "txtReportsTo");
        result = false;
        return false;
    }
    else {
        $.ajax({
            type: "GET",
            url: "../../UserManagement/CheckForIMDActiveStatus?imdCode=" + reportsTo,
            contentType: 'application/json',
            dataType: "json",
            async: false,
            success: function (data) {
                if (data == "False") {
                    writeMultipleMessage("error", 'IMD' + " " + reportsTo + " " + 'not activate in AVO.', "txtReportsTo");
                    result = false;
                    return false;
                }
                else {
                    writeMultipleMessage("error", "", "txtReportsTo");
                    result = true;
                    //return true;
                }
            }
        });
    }
    return result;
}

//function ValidatePANNo() {

//    writeMultipleMessage("error", "", "txtPan");
//    var result = false;
//    var panNo = $("#txtPan").val();
//    if (panNo == "") {
//        writeMultipleMessage("error", " Please enter your PAN", "txtPan");
//        result = false;
//	ErrorCode = false;
//        return false;
//    }
//    else if (panNo != "") {
//        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
//        if (panNo.search(panPat) == -1) {
//            writeMultipleMessage("error", 'Please enter valid Pan No', "txtPan");
//            result = false;
//	     ErrorCode = false;
//            return false;
//        }
//        else {
//            writeMultipleMessage("error", "", "txtPan");
//            if ($("#txtclientcode").val().trim() == "") {
//                $.post("../../usermanagement/checkduplicatepanno?panno=" + panNo)
//                .done(function (data) {

//                    if (data == true || data == "true") {

//                        writeMultipleMessage("error", 'pan number already exists..!', "txtPan");
//                        result = false;
//			   ErrorCode = false;
//                        return false;
//                    }
//                    else {
//                        writeMultipleMessage("error", "", "txtPan");
//                        result = true;
//                        return true;
//                    }
//		}).
//             fail(function (data) {

//                 writeMultipleMessage("error", 'Error..!', "txtPan");
//                 return false
//                });
//            }
//            else {
//                writeMultipleMessage("error", '', "txtPan");
//                ErrorCode = true;
//                result = true;
//            }
//        }
//    }
//    return result;
//}
function ValidatePinCode() {
    var pinCode = $("#AddressLine1Pincode").val();
    if (pinCode == "") {
        writeMultipleMessage("error", " Please enter your PinCode", "AddressLine1Pincode");
        return false;
    }
    else if (pinCode != "") {
        if (pinCode.length < 6) {
            writeMultipleMessage("error", 'Please enter valid PinCode', "AddressLine1Pincode");
            return false;
        }
        else {
            writeMultipleMessage("error", "", "AddressLine1Pincode");
            return true;
        }
    }
}

//Validating Commission Details Grid in IMD Creation.
var result = false;
function ValidateGirdCommissionDetails() {
    var IsValid = true;
    var count = 0;
    var id;
    var premiumclass;
    var ContractType;
    var Commission;
    $("#Commissiondetails tbody").find("tr").each(function () {
        premiumclass = $(this).find("[data-name=premiumclass]").find("option:selected").val();

        //if (premiumclass == "" || premiumclass == null) {
        //    writeMultipleMessage("error", 'Please select Premium Class in Commission Details', "premiumclass");
        //    $('#collapse4').addClass('in').removeClass('collapse');
        //    result = false;
        //    return false;
        //}
        //else {
        //    writeMultipleMessage("error", '', "premiumclass");
        //    result = true;
        //}
        ContractType = $(this).find("[data-name=ContractType]").find("option:selected").val();
        if (premiumclass != "" && ContractType == "") {
            writeMultipleMessage("error", 'Please enter Contract Type in Commission Details', "ContractType");
            $('#collapse4').addClass('in').removeClass('collapse');
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "ContractType");
            result = true;
        }
        //Commission = $(this).find("[data-name=Commission]").find("option:selected").val();
        Commission = $(this).find("[data-name=Commission]").find("input[type=text]").val();
        if (premiumclass != "" && ContractType != "" && Commission == "") {
            writeMultipleMessage("error", 'Please select Commission in Commission Details', "Commission");
            $('#collapse4').addClass('in').removeClass('collapse');
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "Commission");
            result = true;
        }
    });
    return result;
}

//Validating FG Branch details Grid in IMD Creation.
function ValidateGirdProductDetails() {
    var IsValid = true;
    var count = 0;
    var lob;
    var product;
    var contractType;
    var sumInsuredLimit;

    count = $("#ProductDetails tbody").find("tr").size();
    if (count == 0) {
        writeMultipleMessage("error", 'Please add atleast one row in Product Details Grid.', "rowgrid");
        result = false;
        //return false;
    }
    else {
        writeMultipleMessage("error", '', "rowgrid");
        result = true;
    }
    $("#ProductDetails tbody").find("tr").each(function () {

        lob = $(this).find("[data-name=lob]").find("option:selected").val();

        if (lob == "" || lob == null) {
            writeMultipleMessage("error", 'Please select LOB in Product Details.', "lob");
            result = false;
            //return false;
        }
        else {
            writeMultipleMessage("error", '', "lob");
            result = true;
        }

        product = $(this).find("[data-name=product]").find("option:selected").val();
        if (product == "" || product == null) {
            writeMultipleMessage("error", 'Please select product in Product Details', "product");
            result = false;
            //return false;
        }
        else {
            writeMultipleMessage("error", '', "product");
            result = true;
        }

        contractType = $(this).find("[data-name=contractType]").find("option:selected").val();
        if (contractType == "" || contractType == null) {
            writeMultipleMessage("error", 'Please Enter Contract Type in Product Details.', "contractType");
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "contractType");
            result = true;
        }

        sumInsuredLimit = $(this).find("[data-name=sumInsuredLimit]").find("input[type=text]").val();
        if (sumInsuredLimit == "" || sumInsuredLimit == null) {
            writeMultipleMessage("error", 'Please enter Sum Insured Limit in Product Details.', "sumInsuredLimit");
            result = false;
            //return false;
        }
        else {
            writeMultipleMessage("error", '', "sumInsuredLimit");
            result = true;
        }

    });
    return result;
}

//Validating Branch Details Grid in IMD Creation.
function ValidateGirdBranchDetails() {
    //var IsValid = true;
    var count = 0;
    var result = false;

    var count = $("#tblGridBranchDetails > tbody > tr").length;
    if (count > 0) {
        result = true;
    }
    else {
        writeMultipleMessage("error", 'Please add atleast one row in Branch Details Grid.', "rowgrid");
        result = false;
        return false;
    }

    //return result;
    var Branch;
    var Smcode;
    var Empcode;
    var row = [];
    var allBranchObj = [];
    var strRow;

    //count = $("#Branchdetails tbody").find("tr").size();
    //if (count == 0) {
    //    writeMultipleMessage("error", 'Please add atleast one row in Branch Details Grid.', "rowgrid");
    //    result = false;
    //    return false;
    //}
    //else {
    //    writeMultipleMessage("error", '', "rowgrid");
    //    result = true;
    //}

    $("#tblGridBranchDetails tbody").find("tr").each(function () {
        row = [];
        Branch = $(this).find('.branchCode').val();
        if (Branch == "" || Branch == null) {
            writeMultipleMessage("error", 'Please select Branch Code in Branch Details', "ddlbranch");
            $('#collapse6').addClass('in').removeClass('collapse');
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "ddlbranch");
            result = true;
            row.push(Branch);
        }

        Smcode = $(this).find('.smcode').val();
        if ((Smcode == "" || Smcode == null) && $("#ddlImdtype option:selected").val() != 5) {
            writeMultipleMessage("error", 'Please enter SM Code in Branch Details', "ddlsmcode");
            $('#collapse6').addClass('in').removeClass('collapse');
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "ddlsmcode");
            result = true;
            row.push(Smcode);
        }

        Empcode = $(this).find('.employeeCode').val();
        if (Empcode == "" || Empcode == null) {
            writeMultipleMessage("error", 'Please Select Employee Code in Branch Details', "ddlempcode");
            $('#collapse6').addClass('in').removeClass('collapse');
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "ddlempcode");
            result = true;
            row.push(Empcode);
        }
        strRow = [];
        strRow = row.join();
        allBranchObj.push(strRow);

        var sorted_arr = allBranchObj.slice().sort();
        var results = [];
        for (var i = 0; i < allBranchObj.length - 1; i++) {
            if (sorted_arr[i + 1] == sorted_arr[i]) {
                results.push(sorted_arr[i]);
            }
        }

        if (results.length > 0) {
            writeMultipleMessage("error", 'Please remove duplicate entries in Branch Details.', "ddlempcode");
            $('#collapse6').addClass('in').removeClass('collapse');
            result = false;
            return false;
        } else {
            writeMultipleMessage("error", '', "ddlempcode");
            result = true;
            row.push(Empcode);
        }
    });

    return result;
}

//Validating GEB Details Grid in IMD Creation.
function ValidateGirdGEBDetails() {
    var IsValid = true;
    var count = 0;
    var Datefrom;
    var Dateto;
    var Ricession;
    var Ricommission;
    var res = isInRICode();

    count = $("#GEBDetails tbody").find("tr").size();
    if (count == 0) {
        writeMultipleMessage("error", 'Please add atleast one row in GEB Details Grid.', "rowgrid");
        result = false;
        return false;
    }
    else {
        writeMultipleMessage("error", '', "rowgrid");
        result = true;
    }
    if ($("#RbtnFlag").is(':checked') && !res) {
        $("#GEBDetails  tbody").find("tr").each(function () {
            Datefrom = $(this).find("[data-name=Datefrom]").find("input[type=text]").val();
            if (Datefrom == "" || Datefrom == null) {
                writeMultipleMessage("error", 'Please select Date From in GEB Details', "Datefrom");
                $('#collapse7').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else if (Datefrom != "") {
                writeMultipleMessage("error", '', "Datefrom");
                result = ValidateDateFormatByPassingValue(Datefrom, "Invalid from date format GEB Details");
                if (!result) {
                    return result;
                }
                else {
                    writeMultipleMessage("error", '', "Datefrom");
                }
            }

            Dateto = $(this).find("[data-name=Dateto]").find("input[type=text]").val();
            if (Dateto == "" || Dateto == null) {
                writeMultipleMessage("error", 'Please select Date To in GEB Details', "Dateto");
                $('#collapse7').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else if (Dateto != "") {
                writeMultipleMessage("error", '', "Dateto");
                result = ValidateDateFormatByPassingValue(Dateto, "Invalid to date format GEB Details");
                if (!result) {
                    return result;
                }
                else {
                    writeMultipleMessage("error", '', "Dateto");
                }
            }
            if (Datefrom != "" || Datefrom != null && Dateto != "" || Dateto != null) {
                result = CompareTwoDates(Datefrom, Dateto, 'dateto', 'To date Should be greater than From date in GEB Details');
                if (!result) {
                    return result;
                }
                else {
                    writeMultipleMessage("error", "", 'dateto');
                }
            }
            Ricession = $(this).find("[data-name=Ricession]").find("input[type=text]").val();
            if (Ricession == "" || Ricession == null) {
                writeMultipleMessage("error", 'Please enter Ricession in GEB Details', "Ricession");
                $('#collapse7').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", '', "Ricession");
                result = true;
            }
            Ricommission = $(this).find("[data-name=Ricommission]").find("input[type=text]").val();
            if (Ricommission == "" || Ricommission == null) {
                writeMultipleMessage("error", 'Please enter Ricommission in GEB Details', "Ricommission");
                $('#collapse7').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", '', "Ricommission");
                result = true;
            }

        });
        return result;
    }
    else if ($("#RbtndeFlag").is(':checked')) {
        writeMultipleMessage("error", '', "Datefrom");
        writeMultipleMessage("error", '', "Dateto");
        writeMultipleMessage("error", '', "Ricession");
        writeMultipleMessage("error", '', "Ricommission");
        return true;
    }
}

//Validating UIN Details Grid in IMD Creation.
function ValidateGirdUINDetails() {
    var IsValid = true;
    var count = 0;
    var datefrom;
    var UINNo;
    var dateto;

    //count = $("#Uindetails tbody").find("tr").size();
    //if (count == 0) {
    //    writeMultipleMessage("error", 'Please add atleast one row in UIN Details Grid.', "rowgrid");
    //    result = false;
    //    return false;
    //}
    //else {
    //    writeMultipleMessage("error", '', "rowgrid");
    //    result = true;
    //}
    $("#Uindetails tbody").find("tr").each(function () {
        datefrom = $(this).find("[data-name=Datefrom]").find("input[type=text]").val();
        dateto = $(this).find("[data-name=Dateto]").find("input[type=text]").val();
        UINNo = $(this).find("[data-name=uinNumber]").find("input[type=text]").val();

        var agentIds = ['20', '21', '22', '23', '24', '71', '60', '61', '62', '63', '64', '72', '14', '25', '26', '27', '28', '29', '30', '31', '32', '33', '34', '35', '36', '37', '38', '39', '40', '41', '42', '43', '44', '45', '46'];
        result = isInArray($("#ddlImdtype option:selected").val(), agentIds);
        var debtorType = !$("input[name='DebtorType']:checked").val();

        if (result && debtorType) {
            writeMultipleMessage("error", 'Please Select Debtor Type in UIN Details', "Rdndebtor");
            $('#collapse8').addClass('in').removeClass('collapse');
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "Rdndebtor");
            result = true;
        }

        if ($("#Rdndebtor").is(':checked')) {
            result = validateDropDown('ddlTerritory', 'please select D/Territory in UIN Details');
            if (!result) {
                $('#collapse8').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", '', "ddlTerritory");
                result = true;
            }
        }
        else {
            writeMultipleMessage("error", '', "ddlTerritory");
        }

        var res = checkImdTypesForUINNumber();
        if (res && $("#RdndeDebtor").is(':checked')) {
            result = true;
        }
        else if (($("#Rdndebtor").is(':checked') && ($("#ddlTerritory option:selected").val() != "" && $("#ddlTerritory option:selected").val() != "63"))) {
            if (UINNo == "") {
                writeMultipleMessage("error", 'Please enter UIN Number in UIN Details', "UINNo");
                $('#collapse8').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", '', "UINNo");
                result = true;
            }

            //writeMultipleMessage("error", '', "dateto");
            //if (datefrom == "" && dateto == "") {
            //result = true;
            //}
            if (UINNo != "" && datefrom == "") {
                result = ValidateDateFormatByPassingValue(dateto, "Invalid to date format UIN Details");
                writeMultipleMessage("error", 'Please enter from date in UIN Details', "datefrom");
                $('#collapse8').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else if (UINNo != "" && dateto == "") {
                writeMultipleMessage("error", '', "datefrom");
                result = ValidateDateFormatByPassingValue(datefrom, "Invalid from date format UIN Details");
                writeMultipleMessage("error", 'Please enter to date in UIN Details', "dateto");
                $('#collapse8').addClass('in').removeClass('collapse');
                result = false;
                return false;
            }
            else if (datefrom != "" && dateto != "") {
                result = CompareTwoDates(datefrom, dateto, 'dateto', 'To date Should be greater than From date in UIN Details');
                if (!result) {
                    return result;
                }
                else {
                    writeMultipleMessage("error", "", 'dateto');
                }
            }
        }
        else {
            result = true;
        }
    });
    return result;
}

//Validating FG Branch details Grid in User Creation.
function ValidateGirdFGBranchDetails() {
    var IsValid = true;
    var count = 0;
    var BranchCode;
    var BancaBranchCode;
    var BancaBranchDescription;
    var Receipting;
    var AreaCode;
    var checkProp;

    count = $("#BranchDetail tbody").find("tr").size();
    if (count == 0) {
        writeMultipleMessage("error", 'Please add atleast one row in FG Branch Details Grid.', "rowgrid");
        result = false;
        //return false;
    }
    else {
        writeMultipleMessage("error", '', "rowgrid");
        result = true;
    }
    $("#BranchDetail tbody").find("tr").each(function () {
        var p = $(this).find("[data-name=FGBranchCode]").find("option:selected");

        BranchCode = $(this).find("[data-name=FGBranchCode]").find("option:selected").val();
        checkProp = $(this).find(".fgBranchCode").attr('readonly') == 'readonly' || $(this).find(".fgBranchCode").attr('disabled') == 'disabled';

        if ((BranchCode == "" || BranchCode == null) && !checkProp) {
            writeMultipleMessage("error", 'Please select FG Branch Code.', "ddlFGBranchCode");
            result = false;
            //return false;
        }
        else {
            writeMultipleMessage("error", '', "ddlFGBranchCode");
            result = true;
        }

        BancaBranchCode = $(this).find("[data-name=FGBancaBranchCode]").find("option:selected").val();
        checkProp = $(this).find(".BancaBranchCode").attr('disabled') == 'disabled' || $(this).find(".BancaBranchCode").attr('readonly') == 'readonly';
        if ((BancaBranchCode == "" || BancaBranchCode == null) && !checkProp) {
            writeMultipleMessage("error", 'Please select FG Banca Branch Code.', "txtFGBancaBranchCode");
            result = false;
            //return false;
        }
        else {
            writeMultipleMessage("error", '', "txtFGBancaBranchCode");
            result = true;
        }

        BancaBranchDescription = $(this).find("[data-name=FGBancaBranchDescription]").find("input[type=text]").val();
        checkProp = $(this).find(".BancaBranchDescription").attr('readonly') == 'readonly' || $(this).find(".BancaBranchDescription").attr('disabled') == 'disabled';
        if ((BancaBranchDescription == "" || BancaBranchDescription == null) && !checkProp) {
            writeMultipleMessage("error", 'Please Enter Banca Branch Description.', "ddlFGBancaBranchDescription");
            result = false;
            return false;
        }
        else {
            writeMultipleMessage("error", '', "ddlFGBancaBranchDescription");
            result = true;
        }

        AreaCode = $(this).find("[data-name=AreaCode]").find("option:selected").val();
        if (AreaCode == "Select Area Code" || AreaCode == null || AreaCode == "") {
            writeMultipleMessage("error", 'Please select Area Code.', "ddlAreaCode");
            result = false;
            //return false;
        }
        else {
            writeMultipleMessage("error", '', "ddlAreaCode");
            result = true;
        }

        Receipting = $(this).find("[data-name=Receipting]").find("option:selected").val();
        if (Receipting == "" || Receipting == "Select Receipting" || Receipting == null) {
            writeMultipleMessage("error", 'Please select Receipting.', "ddlReceipting");
            result = false;
            //return false;
        }
        else {
            writeMultipleMessage("error", '', "ddlReceipting");
            result = true;
        }
    });
    return result;
}

//Check the no is availabvle in a array or not 
function isInArray(value, array) {
    return array.indexOf(value) > -1;
}

function ValidateBankDetails() {
    var paymentType = $("#ddlPaymentType :selected").val();
    var acountType = $("#ddlAcountType :selected").val();
    var accountNo = $("#txtAccountNo").val();

    if (accountNo != "" && acountType == "") {
        writeMultipleMessage("error", "please select Acount Type in Bank Details", "ddlAcountType");
        $('#collapse5').addClass('in').removeClass('collapse');
        result = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlAcountType");
        result = true;
    }

    if (accountNo == "" && paymentType == 1) {
        writeMultipleMessage("error", "please enter Account No in Bank Details", "txtAccountNo");
        $('#collapse5').addClass('in').removeClass('collapse');
        result = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtAccountNo");
        result = true;
    }
    return result;
}

function ValidateIfscCode() {
    var accountNo = $("#txtAccountNo").val();
    var ifscCode = $("#txtifsccode").val();
    if (accountNo != "" && ifscCode == "") {
        writeMultipleMessage("error", "please enter IFSC Code in Bank Details", "txtifsccode");
        $('#collapse5').addClass('in').removeClass('collapse');
        result = false;
        return false;
    }
    else if (ifscCode != "") {
        ifscCode = ifscCode.trim();
        //var IFSCRegex = /[A-Z|a-z]{4}[0][\d]{6}$/; //REGULAR EXPRESSION of IFSC Code
        var IFSCRegex = /[A-Z|a-z]{4}[0][A-Z|a-z|0-9]{6}$/; //REGULAR EXPRESSION of IFSC Code For UNITED BANK OF INDIA
        if (ifscCode.search(IFSCRegex) == -1) {
            writeMultipleMessage("error", "please enter valid IFSC Code in Bank Details", "txtifsccode");
            result = false;
            return false;
        }
        else {
            $.ajax({
                type: "GET",
                url: "../../UserManagement/SearchIfscCode?ifsccode=" + ifscCode,
                async: false,
                success: function (data) {
                    if (data.Status) {
                        $("#txtmicrcode").val(data.MicrCode);
                        $("#txtbankcode").val(data.BankCode);
                        $("#txtbranchname").val(data.BranchName);
                    }
                }
            });
            writeMultipleMessage("error", '', "txtifsccode");
            result = true;
        }
    }
    else {
        writeMultipleMessage("error", '', "txtifsccode");
    }
    return result;
}

function ValidatePanVerified() {
    var result = true;
    //var panVerified = !$("input[name='Panverified']:checked").val();
    //var res = isInRICode();
    //if (panVerified && !res) {
    //    writeMultipleMessage("error", 'Please Check Pan Verified', "chkPan");
    //    return false;
    //}
    //else {
    //    writeMultipleMessage("error", '', "chkPan");
    //    return true;
    //}
    if (!($('#chkPan').is(':checked')) == true) {
        $("#txtPan").attr("disabled", false);
        $("#chkPan").attr("disabled", true);
        result = true;
    }

    return result;
}

//Return only number
function NumberOnly(id) {
    $("#" + id).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}

//Return only numbers & /
function DateFormat(id) {
    $("#" + id).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && e.which != 47 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}

//Block White Spaces
function NoWhiteSpaces(id) {
    $("#" + id).keypress(function (e) {
        if (e.which == 32) {
            return false;
        }
    });
}

//Block Symbols
function NoSymbols(id) {
    $("#" + id).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && (e.which < 65 || e.which > 90) && (e.which < 97 || e.which > 122)) {
            return false;
        }
    });
}

//Alpha numeric with spaces.
function AlphaNumeric(id) {
    $("#" + id).keypress(function (e) {
        if (e.which != 8 && e.which != 32 && e.which != 0 && (e.which < 48 || e.which > 57) && (e.which < 65 || e.which > 90) && (e.which < 97 || e.which > 122)) {
            return false;
        }
    });
}

//Decimal Number Format
function DecimalNoFormat(id) {
    $("#" + id).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
}

//Validate date format by value
function ValidateDateFormatByPassingValue(value, msg) {
    if (value != "") {
        var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/);
        var result = dtRegex.test(value);
        if (!result) {
            writeMultipleMessage("error", msg, 'dateformat');
            return false;
        }
        else {
            writeMultipleMessage("error", '', 'dateformat');
            return true;
        }
    }
}

//Compare two dates
function CompareTwoDates(toDate, fromDate, id, msg) {
    var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/);
    var result = dtRegex.test(toDate);
    if (result) {
        var toDate = (toDate).split("/");
    }
    else {
        var toDate = (toDate).split("-");
    }

    var result = dtRegex.test(fromDate);
    if (result) {
        var fromDate = (fromDate).split("/");
    }
    else {
        var fromDate = (fromDate).split("-");
    }

    toDateYear = toDate[2];
    toDateMonth = toDate[1];
    toDateDay = toDate[0];

    fromDateyear = fromDate[2];
    fromDatemonth = fromDate[1];
    fromDateday = fromDate[0];

    var toDate = new Date(toDateYear, toDateMonth, toDateDay);
    var fromDate = new Date(fromDateyear, fromDatemonth, fromDateday);
    var diff = (fromDate - toDate) / 1000 / 60 / 60 / 24;

    if (diff <= 0) {
        writeMultipleMessage("error", msg, id);
        return false;
    }
    else {
        writeMultipleMessage("error", '', id);
        return true;
    }
}

//Check for same financial year
function CheckForSameFinancialYear(toDate, fromDate, id, msg) {
    var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/);
    var result = dtRegex.test(toDate);
    if (result) {
        var toDate = (toDate).split("/");
    }
    else {
        var toDate = (toDate).split("-");
    }

    var result = dtRegex.test(fromDate);
    if (result) {
        var fromDate = (fromDate).split("/");
    }
    else {
        var fromDate = (fromDate).split("-");
    }

    var today = new Date();
    var currentYear = today.getFullYear();
    var currentMonth = parseInt(today.getMonth()) + 1;

    toDateYear = toDate[2];
    toDateMonth = parseInt(toDate[1]) - 1;
    toDateDay = toDate[0];

    fromDateyear = fromDate[2];
    fromDatemonth = parseInt(fromDate[1]) - 1;
    fromDateday = fromDate[0];

    var nextFinancialMonth = 02;
    var nextFinancialDay = 31;
    var nextFinancialYear;
    var nextYear = parseInt(fromDateyear) + 1;

    var isNextYear;
    if (fromDateyear == currentYear) {
        isNextYear = false;
    }
    else {
        isNextYear = true;
    }

    var nextFinancialDate = new Date();
    //nextFinancialDate.setFullYear(2020, 10, 3);

    var toDate = new Date(toDateYear, toDateMonth, toDateDay);
    var fromDate = new Date(fromDateyear, fromDatemonth, fromDateday);

    if ((fromDateyear == currentYear) && parseInt(fromDatemonth) >= 3) {
        nextFinancialYear = parseInt(fromDateyear) + 1;
        nextFinancialDate = new Date(nextFinancialYear, nextFinancialMonth, nextFinancialDay);
    }
    else if ((fromDateyear == currentYear) && parseInt(fromDatemonth) <= 4) {
        nextFinancialYear = fromDateyear;
        nextFinancialDate = new Date(nextFinancialYear, nextFinancialMonth, nextFinancialDay);
    }
    else if ((parseInt(fromDateyear) + 1 == nextYear) && parseInt(fromDatemonth) < 4) {
        if (isNextYear) {
            nextFinancialYear = parseInt(nextYear) - 1;
        }
        else {
            nextFinancialYear = nextYear;
        }
        var nextFinancialDate = new Date(nextFinancialYear, nextFinancialMonth, nextFinancialDay);
    }
    else if ((fromDateyear > nextYear) || (toDateYear > nextYear)) {
        writeMultipleMessage("error", msg, id);
        return false;
    }

    var diff = (nextFinancialDate - toDate) / 1000 / 60 / 60 / 24;
    if (diff < 0) {
        writeMultipleMessage("error", msg, id);
        return false;
    }
    else {
        writeMultipleMessage("error", '', id);
        return true;
    }
}

function ValidateAllDetailsIMD() {

    var result;
    writeMultipleMessage("error", "", 'dpAppointmentdate');
    result = validateDropDown('ddlImdtype', 'please select Intermediary Type');
    if (!result) {
        return false;
    }
    result = validateDropDown('ddlImdstatus', 'please select Imd status');
    if (!result) {
        return false;
    }
    result = ValidateChangeStatus();
    if (!result) {
        return false;
    }
    result = validateTextBox('txtImdDescription', 'please provide imd description');
    if (!result) {
        return false;
    }
    //if ($('#hdnImdDetailsId').val() == 0 || $('#hdnImdDetailsId').val() == null) {
    //    result = CheckIMD();
    //    if (!result) {
    //        return false;
    //    }
    //}

    // Client Code validation
    if ($('#hdnImdDetailsId').val() == 0) {
        result = CheckClientCode();
        if (!result) {
            return false;
        }
    }

    if ($("#ClientTypeIndividual").is(':checked')) {
        writeMultipleMessage("error", "", "CorpName");
        writeMultipleMessage("error", "", "Contact");
        result = validateDropDown('ddlsalutation', 'Please Select Salutation');
        if (!result) {
            return false;
        }
        result = validateTextBox('txtFirstName', 'please provide First Name');
        if (!result) {
            return false;
        }
        result = validateTextBox('txtLastName', 'please provide Last Name');
        if (!result) {
            return false;
        }
    }
    else {
        writeMultipleMessage("error", "", "ddlsalutation");
        writeMultipleMessage("error", "", "txtFirstName");
        writeMultipleMessage("error", "", "txtLastName");
        result = validateTextBox('CorpName', 'please provide Corporate Name');
        if (!result) {
            return false;
        }
        result = validateTextBox('Contact', 'Please Provide Contact Person');
        if (!result) {
            return false;
        }
    }

    if ($("#ClientTypeIndividual").is(':checked')) {
        result = validateDropDown('ddlgender', 'please select Gender');
        if (!result) {
            return false;
        }
    }
    else {
        writeMultipleMessage("error", '', "ddlgender");
    }
    if ($("#ClientTypeIndividual").is(':checked')) {
        result = validateDropDown('ddlmaritalstatus', 'please select Marital Status');
        if (!result) {
            return false;
        }
    }
    else {
        writeMultipleMessage("error", '', "ddlmaritalstatus");
    }
    if ($("#ClientTypeIndividual").is(":checked")) {
        result = ValidateDateofBirth();
        if (!result) {
            return false;
        }
    }
    else {
        writeMultipleMessage("error", '', "dpdob");
    }

    //result = validateTextBox1(AddressLine1Address1);
    //if (!result) {
    //    return false;
    //}

    result = validateTextBox('AddressLine1Address1', 'Please enter Address1');
    if (!result) {
        return false;
    }
    result = ValidatePinCode();
    if (!result) {
        return false;
    }
    result = validateTextBox('AddressLine1City', 'Please enter City');
    if (!result) {
        return false;
    }
    result = ValidatePhoneNumber();
    if (!result) {
        return false;
    }
    result = ValidateMobileNumber();
    if (!result) {
        return false;
    }
    result = validateEmail();
    if (!result) {
        return false;
    }

    //ErrorCode = true;
    result = ValidatePANNo();
    if (!result) {
        return false;
    }

    //if (ErrorCode == true)
    //    result = true;
    //else
    //    result = false;

    //if (!result) {
    //    return false;
    //}

    result = ValidatePanVerified();
    if (!result) {
        return false;
    }

    //Commented as per the new enxhancement CR for 85/88/89
    result = ValidateLicenceNumber();
    if (!result) {
        return false;
    }

    var ImdType = $('#ddlImdtype option:selected').val();
    var licenseExpiryDate = $("#dpLicenseExpiry").val();
    var LicenseIssueDate = $("#dpLicenseIssue").val();

    writeMultipleMessage("error", "", 'ddlImdtype');
    writeMultipleMessage("error", "", 'dpLicenseIssue');
    writeMultipleMessage("error", "", 'dpLicenseExpiry');
    if ((ImdType == "1" || ImdType == "2" || ImdType == "82" || ImdType == "83" || ImdType == "84" || ImdType == "85" || ImdType == "86" || ImdType == "87" || ImdType == "88" || ImdType == "89") && $("#dpLicenseIssue").val() == "") {
        writeMultipleMessage("error", "License Issue Date is mandatory", 'dpLicenseIssue');
        return false;
    }
    else if ((ImdType == "1" || ImdType == "2" || ImdType == "82" || ImdType == "83" || ImdType == "84" || ImdType == "85" || ImdType == "86" || ImdType == "87" || ImdType == "88" || ImdType == "89") && licenseExpiryDate == "") {
        writeMultipleMessage("error", "License Expiry Date is mandatory", 'dpLicenseExpiry');
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'dpLicenseIssue');
        writeMultipleMessage("error", "", 'dpLicenseExpiry');

        result = ValidateLicenseIssueDate();
        if (!result) {
            return false;
        }
    }

    result = validateAppointmentDate();
    if (!result) {
        return false;
    }

    if ((ImdType == "1" || ImdType == "2") && $('#dpLicenseExpiry').val() == "" && $('#dpLicenseExpiry').val() == "") {
        result = validateLicenseExpiryDate();
        if (!result) {
            return false;
        }
        //result = validateImdTerminationDate();
        //if (!result) {
        //    return false;
        //}
    }
    result = validateDropDown('ddlFGchannel', 'please select FG Channel');
    if (!result) {
        return false;
    }

    //result = validateDropDown('ddlIrdaChannel', 'please select IRDA Channel');
    //if (!result) {
    //    return false;
    //}

    //Validation for IMD Type 88
    if (($("#ddlImdtype option:selected").val() == 88)) {
        result = validateReportsTo();
        if (!result) {
            return false;
        }
        result = ValidateAadhaarNo();
        if (!result) {
            return false;
        }
    }

    if ($("#divProductDetails").is(":visible")) {
        result = ValidateGirdProductDetails();
        if (!result) {
            return false;
        }
    }
    result = ValidateGirdBranchDetails();
    if (!result) {
        return false;
    }
    if ($("#RbtnFlag").is(':checked')) {
        result = ValidateGirdGEBDetails();
        if (!result) {
            return false;
        }
    }
    result = ValidateGirdCommissionDetails();
    if (!result) {
        return false;
    }

    //result = validateDropDown('ddlPaymentType', 'please select Payment Type');
    //if (!result) {
    //    $('#collapse5').addClass('in').removeClass('collapse');
    //    return false;
    //}

    result = ValidateBankDetails();
    if (!result) {
        return false;
    }
    var accountNo = $("#txtAccountNo").val();
    if (accountNo != "" || accountNo != null) {
        result = ValidateIfscCode();
        if (!result) {
            return false;
        }
    }

    //TDS Details validation
    result = ValidateTDSDetails();
    if (!result) {
        return false;
    }

    // if ($("#Rdndebtor").is(':checked') || $("#RdndeDebtor").is(':checked')) {
    result = ValidateGirdUINDetails();
    if (!result) {
        return false;
    }
    //}
    //Disable button after form submission
    $("#btnsubmit").prop("disabled", true);
    return true;
}

function ValidateAllDetailsUser() {
    var result;
    if ($("#RbtnParentID").is(':checked')) {
        result = validateTextBox('txtImdCode', 'Please provide IMD Code');
        if (!result) {
            result = false;
        }
    }
    else {
        writeMultipleMessage("error", '', "txtImdCode");
        result = true;
    }
    if ($("#RbtnParentID").is(':checked')) {
        result = validateTextBox('txtImdName', 'Please provide IMD Name');
        if (!result) {
            result = false;
        }
    }
    else {
        writeMultipleMessage("error", '', "txtImdName");
        result = true;
    }
    if ($('#hdnUserDetailsID').val() == 0 || $('#hdnUserDetailsID').val() == null) {
        result = CheckUserIDName();
        if (!result) {
            result = false;
        }
    }
    if ($("#deRbtnParentid").is(':checked')) {
        result = validateDropDown('txtReportusrId', 'please select Reporting User ID');
        if (!result) {
            result = false;
        }
    }
    else {
        writeMultipleMessage("error", '', "txtReportusrId");
        result = true;
    }
    if ($("#deRbtnParentid").is(':checked')) {
        result = validateTextBox('txtReportusrName', 'Please Enter Reporting User Name');
        if (!result) {
            result = false;
        }
    }
    else {
        writeMultipleMessage("error", '', "txtReportusrName");
        result = true;
    }
    result = ValidateMobileNumber();
    if (!result) {
        result = false;
    }
    result = validateEmail();
    if (!result) {
        result = false;
    }
    result = ValidateGirdFGBranchDetails();
    if (!result) {
        result = false;
    }
    if ($('#ddlSecretQstn').attr('disabled') != 'disabled' && $('#txtSecretAns').attr('disabled') != 'disabled') {
        result = validateDropDown('ddlSecretQstn', 'Please Select Secret Question ');
        if (!result) {
            result = false;
        }
        result = validateTextBox('txtSecretAns', 'please provide Secret Answer');
        if (!result) {
            result = false;
        }
    }
    else {
        writeMultipleMessage("error", '', "ddlSecretQstn");
        writeMultipleMessage("error", '', "txtSecretAns");
    }

    //Disable button after form submission
    if (result) {
        $("#btnModify").prop("disabled", true);
    }
    else {
        $("#btnModify").prop("disabled", false);
    }

    return result;
}

$(document).ready(function () {
    //Delete Commission Row
    $(document.body).on("click", ".deleteCommissionRow", (function () {
        $("#Commissiondetails .grid-row-selected").remove();
    }));

    //Delete Branchdetails Row
    var result = false;
    $(document.body).on("click", ".deleteBranchRow", (function () {
        var id = this.id;

        if (id == "btnImdDelete") {
            result = DeleteBranchGridRow(this);
        }
        else {
            $("#Branchdetails .grid-row-selected").remove();
            $("#btnBranchRowAdd").attr('disabled', false);
        }
    }));

    //Delete GEB Row
    $(document.body).on("click", ".deleteGebRow", (function () {
        $("#GEBDetails .grid-row-selected").remove();
    }));

    //Delete UIN Row
    $(document.body).on("click", ".deleteUinRow", (function () {
        $("#Uindetails .grid-row-selected").remove();
    }));

    //Delete Product Row
    $(document.body).on("click", ".deleteProductRow", (function () {
        $("#ProductDetails .grid-row-selected").remove();
    }));
});
function ValidateInternalUser() {
    var result = true;

    result = validateTextBox('txtStaffName', 'Please Enter the Staff Name');
    if (!result) {
        result = false;
    }
    result = validateTextBox('txtStaffCode', 'Please Enter the Staff Code');
    if (!result) {
        result = false;
    }
    result = validateTextBox('txtStaffCorEmailID', 'Please Enter the Staff Corporate EmailID');
    if (!result) {
        result = false;
    }
     
    result = validateEmailID('txtStaffCorEmailID', 'please provide valid Email Id');
    if (result) {
        writeMultipleMessage("error", "", "txtemail");
    }
    else {
        return false;
    } 

    result = validateDropDown('ddlChannelCode', 'Please Select Channel Code');
    if (!result) {
        return false;
    }
    result = validateDropDown('ddlFunction', 'Please Select Function');
    if (!result) {
        result = false;
    }
    result = validateDropDown('ddlTitle', 'Please Select Title');
    if (!result) {
        result = false;
    }
    //result = validateDropDown('ddlLineMangStaffCode', 'Please Select Line Manager StaffCode');
    //if (!result) {
    //    result = false;
    //}
    return result;
}
function GetLineManagerStaffName() {
    var StaffCode = $('#txtLineMangStaffCode').val();
    $("#txtLineManager").val("");
    $.post("../../UM/UserManagement/GetLineManagerName?StaffCode=" + StaffCode)
        .done(function (data) {
            if (data == null || data == "") {

                BootstrapDialog.alert("There is no Existing LineManager StaffCode to fetch LineManagerName");
            }
            else {
                $("#txtLineManager").val(data);
            }


        }).
        fail(function () {
            $("#txtLineManager").html("Failed to fetch StaffName");
        });

}
function GetTitle() {
    var result = validateDropDown('ddlFunction', 'Please Select Function');
    if (result == false) {
        return false;
    }
    var FunctionId = $('#ddlFunction option:selected').val();
    $.post("../../UM/UserManagement/GetTitleByFunction?FunctionId=" + FunctionId)
       .done(function (data) {
           $('#ddlTitle').html('');
           $.each(data, function (ID, option) {
               $('#ddlTitle').append($('<option></option>').val(option.ID).html(option.Value));
           })

       }).
       fail(function () {
           BootstrapDialog.alert("Failed to fill Title");
       });
    DisableBranchAndChannel();
}
//this function will fire if function is underwriter and title is underwriter-motor. 21/01/2016
//by default select the all channel and all branchs and disable rest of values 
function DisableBranchAndChannel() {
    var result = validateDropDown('ddlTitle', 'Please Select Title');
    if (result == false) {
        return false;
    }
    var FunctionId = $('#ddlFunction option:selected').val();
    var TitleId = $('#ddlTitle option:selected').val();
    if (FunctionId == "4" && TitleId == "8") {
        $('#ddlBranchCode').find('option').each(function (i, e) {
            if ($(e).text() != "ALL BRANCH") {
                $("#ddlBranchCode option:contains(" + $(e).text() + ")").prop('disabled', true)
            }
            else {
                $("#ddlBranchCode option:contains('ALL BRANCH')").prop('disabled', false)
                $('#ddlBranchCode').prop('selectedIndex', i);
            }
        });
        $('#ddlChannelCode').find('option').each(function (i, e) {
            if ($(e).text() != "ALL Channel") {
                $("#ddlChannelCode option:contains(" + $(e).text() + ")").prop('disabled', true);
            }
            else {
                $("#ddlChannelCode option:contains('ALL Channel')").prop('disabled', false);
                $('#ddlChannelCode').prop('selectedIndex', i);
            }
        });

    }
    else {
        $('#ddlBranchCode').find('option').each(function (i, e) {
            //uncomment below code if do u want disable by default all branch value in the list
            //if ($(e).text() == "ALL BRANCH") {
            //    $("#ddlBranchCode option:contains('ALL BRANCH')").prop('disabled', false);
            //    $("#ddlBranchCode option:contains('ALL BRANCH')").prop('selected', false);
            //}
            //else {
            $("#ddlBranchCode option:contains(" + $(e).text() + ")").prop('disabled', false);

            // }
        });
        $('#ddlChannelCode').find('option').each(function (i, e) {
            //uncomment below code if do u want disable by default all channel value in the list
            //if ($(e).text() == "ALL Channel") {
            //    $("#ddlChannelCode option:contains('ALL Channel')").prop('disabled', false)
            // $("#ddlChannelCode option:contains('ALL Channel')").prop('selected', false);
            //}
            //else {
            $("#ddlChannelCode option:contains(" + $(e).text() + ")").prop('disabled', false);

            //}
        });
    }

}
//this function executed if all channels has selected disable rest of values
function chkAllChannel() {

    if ($("#ddlChannelCode option:selected").text() == "ALL Channel") {
        $('#ddlChannelCode').find('option').each(function (i, e) {
            if ($(e).text() == "ALL Channel") {
                $("#ddlChannelCode option:contains('ALL Channel')").prop('disabled', false)
                $('#ddlChannelCode option').prop('selectedIndex', i);
            }
            else {
                $("#ddlChannelCode option:contains(" + $(e).text() + ")").prop('disabled', true)
            }
        });

    }
    else {
        $('#ddlChannelCode').find('option').each(function (i, e) {
            if ($(e).text() == "ALL Channel") {
                $("#ddlChannelCode option:contains(" + $(e).text() + ")").prop('disabled', true);
            }
            else {
                $("#ddlChannelCode option:contains(" + $(e).text() + ")").prop('disabled', false);
                $("#ddlChannelCode option:contains('ALL Channel')").prop('selected', false);
            }
        });
    }

}
//check all branch value has selected or not.
function chkAllBranch() {

    if ($("#ddlBranchCode option:selected").text() == "ALL BRANCH") {
        $('#ddlBranchCode').find('option').each(function (i, e) {
            if ($(e).text() == "ALL BRANCH") {
                $("#ddlBranchCode option:contains('ALL BRANCH')").prop('disabled', false)
                $('#ddlBranchCode').prop('selectedIndex', i);
            }
            else {
                $("#ddlBranchCode option:contains(" + $(e).text() + ")").prop('disabled', true)
            }
        });

    }
    else {
        $('#ddlBranchCode').find('option').each(function (i, e) {
            if ($(e).text() == "ALL BRANCH") {
                $("#ddlBranchCode option:contains(" + $(e).text() + ")").prop('disabled', true);
            }
            else {
                $("#ddlBranchCode option:contains(" + $(e).text() + ")").prop('disabled', false);
                $("#ddlBranchCode option:contains('ALL BRANCH')").prop('selected', false);
            }
        });

    }

}

function DisableSmCode() {
    $("#ddlFGchannel").empty();
    writeMultipleMessage("error", "", 'ddlImdtype');
    //Check if the IMDType is 5, then disable the SMCode.
    if ($("#ddlImdtype option:selected").val() == 5) {
        $(".smcode").prop("disabled", true);
        $(".smcode").val('');
        $(".employeeCode").attr("readonly", false);
        $(".employeeCode").prop("disabled", false);
    }
    else {
        $(".smcode").prop("disabled", false);
        $(".employeeCode").attr("readonly", true);
        $(".employeeCode").prop("disabled", true);
    }
    //Disable Licence no when IMDType is 1
    if ($("#ddlImdtype option:selected").val() == 1) {
        $("#txtlicencenumber").attr("readonly", true);
        $("#divTransferCase").show();
        $("#rdYes").prop("disabled", false);
        $("#rdNo").prop("disabled", false);
    }
    else {
        $("#txtlicencenumber").attr("readonly", false);
        $("#divTransferCase").show();
        $("#rdYes").prop("disabled", true);
        $("#rdNo").prop("disabled", true);
    }
    //Selection of client type base on the client type
    if ($("#ddlImdtype option:selected").val() == 3) {
        $("#ClientTypeIndividual").attr("checked", true);
        EnableDisableAllControls("ClientTypeIndividual", false);
        EnableDisableAllControls("ClientTypeorganization", false);
        ShowIndividual();
    }
    else if ($("#ddlImdtype option:selected").val() == 4) {
        $("#ClientTypeorganization").attr("checked", true);
        EnableDisableAllControls("ClientTypeIndividual", false);
        EnableDisableAllControls("ClientTypeorganization", false);
        ShowOrg();
    }
    else {
        $("#ClientTypeIndividual").attr("checked", true);
    }

    //Show Reports To, Aadhar No when IMDType is 88
    if (($("#ddlImdtype option:selected").val() == 88)) {
        $("#divReportsToAadhaarNo").show();
    }
    else {
        $("#divReportsToAadhaarNo").hide();
    }

    //Show Product Details when IMDType is 88/85
    if ($("#ddlImdtype option:selected").val() == 85 || $("#ddlImdtype option:selected").val() == 88) {
        $("#divProductDetails").show();
    }
    else {
        $("#divProductDetails").hide();
    }

    //Auto Populated FG Channel According to the IMDType
    var imdType = $("#ddlImdtype option:selected").val();
    var TempID = 0;
    if (!imdType == "") {
        $.ajax({
            type: "GET",
            url: "../../UserManagement/GetFGChannel?ImdType=" + imdType,
            async: false,
            success: function (data) {
                if (data != "Error") {
                    var html = '<select class="form-control" data-val="true" data-val-number="The field FGChannel must be a number." data-val-required="The FGChannel field is required." id="ddlFGchannel" name="FGChannel" onblur="validateDropDown(\'ddlFGchannel\',\'please select FG Channel\')" onchange="ResetBranchSMMapping()" style="border-color: red;"><option value="">Select</option>';
                    $.each(data, function () {

                        html = html + '<option value="' + data[TempID].ID + '">' + data[TempID].Value + '</option>';
                        TempID = TempID + 1;
                    });
                    html = html + '</select>';
                    $("#divFGChannel").empty();
                    $(html).appendTo($("#divFGChannel"))
                }
                else {
                    writeMultipleMessage("error", "There is no FG Channels available for this IMDType", 'ddlImdtype');
                    return false;
                }
            }
        });
    }

    //Auto Populated LOB in Product Details to the IMDType
    var TempID = 0;
    if (imdType == "85") {
        $.ajax({
            type: "GET",
            url: "../../UserManagement/GetProductDetailsForIMDTypes?ImdType=" + imdType,
            async: false,
            success: function (data) {

                $('#ProductDetails').html("");
                $('#ProductDetails').html(data);
            }
        });
    }

    $("#ddlImdtype").prop("disabled", true);
}

function GetSmCodeForBranch(gridindex) {
    $("#btnBranchRowAdd").attr('disabled', false);
    var branchid = $("#ddlbranch" + gridindex + " option:selected").val();
    var FGChannelCode = $("#ddlFGchannel option:selected").val();
    if (FGChannelCode == "" || FGChannelCode == null) {
        writeMultipleMessage("error", "Please select FG Channel", 'ddlFGchannel');
        return false
    }
    else if (branchid == "" || branchid == null) {
        writeMultipleMessage("error", "Please select Branch Code", 'ddlbranch' + gridindex);
        return false
    }
    else {
        writeMultipleMessage("error", "", 'ddlbranch' + gridindex);
        writeMultipleMessage("error", "", 'ddlFGchannel');
    }

    var tempvar = 0;
    var imdType = $("#ddlImdtype option:selected").val();
    var TempID = 0;
    if (imdType != "5") {
        $(".smcode").prop("disabled", false);
        $.ajax({
            type: "GET",
            url: "../../UserManagement/FetchSmCode?branchID=" + branchid + "&FGChannelcode=" + FGChannelCode,
            async: false,
            success: function (data) {


                if (data != "Error") {
                    $("#btnBranchRowAdd").attr('disabled', false);
                    $('#ddlsmcode' + gridindex).empty();
                    $('#ddlsmcode' + gridindex).append($('<option></option>').val(" ").html("Select"));
                    $.each(data, function () {

                        $('#ddlsmcode' + gridindex).append($('<option></option>').val(data[tempvar].ID).html(data[tempvar].Value));
                        tempvar = tempvar + 1;
                        writeMultipleMessage("error", "", 'ddlsmcode' + gridindex);
                    });

                }
                else {
                    $("#btnBranchRowAdd").attr('disabled', true);
                    $('#ddlsmcode' + gridindex).empty();
                    writeMultipleMessage("error", "There is no SM's available for this Branch", 'ddlsmcode' + gridindex);
                    return false;
                }

            }

        });
    }

}

//Checking mandatory for organization / RI code
function isInRICode() {
    if ($("#ddlImdtype option:selected").val() != "") {
        writeMultipleMessage("error", "", 'ddlImdtype');
        var agentIds = ['8', '9', '11', '20', '23', '25', '28', '30', '32', '37', '45', '60', '71'];
        var res = isInArray($("#ddlImdtype option:selected").val(), agentIds);
        if ($("#ClientTypeorganization").is(":checked") || res) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        writeMultipleMessage("error", "please select Intermediary Type", 'ddlImdtype');
        return false;
    }
}

//Checking Imd types for UIN number
function checkImdTypesForUINNumber() {
    var agentIds = ['20', '21', '22', '23', '24', '71', '60', '61', '62', '63', '64', '72', '14', '25', '26', '27', '28', '29', '30', '31', '32', '33', '34', '35', '36', '37', '38', '39', '40', '41', '42', '43', '44', '45', '46'];
    var res = isInArray($("#ddlImdtype option:selected").val(), agentIds);
    if (res) {
        return true;
    }
    else {
        return false;
    }
}

//function FetchsmMappingDetails() {
//    var empCode = $("#txtempcode").val();
//    var empName = $("#txtempname").val();

//    if (empCode == null || empCode == "") {
//        writeMultipleMessage("error", "please enter employee code", 'txtempcode');
//        return false;
//    }
//    else {
//        writeMultipleMessage("error", "", 'txtempcode');
//        $.ajax({
//            type: "GET",
//            url: "../../UserManagement/FetchsmMappingDetailsTogrid?empCode=" + empCode + "&empName=" + empName,
//            async: false,
//            success: function (data) {
//                $("#DivGridSmDetail").html(data);
//            }
//        });
//    }
//}

//If Agent Status In Inactive 
function GetIMDTerminationDate() {
    if ($("#ddlImdstatus option:selected").val() == 2) {
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        var currentDate = d + "/" + m + "/" + y;
        $('#dpImdTermination').val(currentDate);
        $("#dpImdTermination").attr("readonly", true);
    }
    else {
        $("#dpImdTermination").val("");
        $("#dpImdTermination").attr("readonly", false);
    }
}

//Get gender accoring to the salutation 
function GetGenderBySalutation() {
    var salutation = $("#ddlsalutation option:selected").val();
    if (salutation == 7 || salutation == 467 || salutation == 468) {
        $("#ddlgender").val(20);
        $("#ddlgender").attr("disabled", true);
    }
    else if (salutation == 8 || salutation == 11 || salutation == 462 || salutation == 469) {
        $("#ddlgender").val(21);
        $("#ddlgender").attr("disabled", true);
    }
    else {
        $("#ddlgender").val("");
        $("#ddlgender").attr("disabled", false);
    }
}

function DeleteBranchGridRow(e) {

    var id = e.id;
    var idt = $(".grid-row-selected").index();
    var BranchCode = $("#ddlbranch" + idt).val();
    var BranchCode1 = $("#ddlbranch" + idt + " :selected").val();
    var imdcode = $("#txtImdCode").val();
    $.ajax({
        type: "Post",
        url: "../../UserManagement/deleteImdBranch?ImdCode=" + imdcode + "&BranchCode=" + BranchCode,
        async: false,
        success: function (data) {
            if (data != "success") {
                // $("#ddlbranch" + idt).attr("disabled", true);
                writeMultipleMessage("error", "Cannot delete Branch. Child user For " + imdcode + " is configured for branch code " + data + " Please delete Branch from Child user First", "ddlbranch" + idt);
                return false;
            }
            else {
                writeMultipleMessage("error", "", "ddlbranch" + idt);
                $("#Branchdetails .grid-row-selected").remove();
                $("#btnBranchRowAdd").attr('disabled', false);
                return true;

            }

            return true;
        }
    });

}

function CheckClientCode() {
    var result = false;
    var clientCode = $('#txtclientcode').val();
    if (clientCode != "") {
        if (clientCode.length < 8) {
            writeMultipleMessage("error", "Please enter atleast 8 character.", "txtclientcode");
            result = false;
            return false;
        }
        else {

            writeMultipleMessage("error", "", "txtclientcode");
            $.ajax({
                type: "GET",
                url: "../UserManagement/CheckClientCode?ClientCode=" + clientCode,
                contentType: 'application/json',
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data == "") {
                        result = true;
                        writeMultipleMessage("error", 'Invalid Client Code!!', "txtclientcode");
                        result = false;
                        return false;
                    }
                    else {
                        writeMultipleMessage("error", "", "txtclientcode");
                        result = true;
                    }
                }
            });
        }
    }
    else {
        writeMultipleMessage("error", "", "txtclientcode");
        return true;
    }

    return result;
}

function ValidateTDSDetails() {
    var result = false;
    var lowTDSRates = $('#txtLowTDSRates').val();
    var imdtype = $("#ddlImdtype option:selected").val();
    var serviceTaxRegNo = $('#txtServiceTaxRegistrationNumber').val();
    var fromDate = $("#dpValidityPeriodFrom").val();
    var toDate = $('#dpValidityPeriodTo').val();
    var lTDSCertificateReceivedDate = $('#dpLTDSCertificateReceivedDate').val();

    $('#collapse9').addClass('in').removeClass('collapse');

    if ((imdtype == 2 || imdtype == 87) && serviceTaxRegNo == "") {
        writeMultipleMessage("error", "Please enter service tax registration number.", "txtServiceTaxRegistrationNumber");
        result = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtServiceTaxRegistrationNumber");
        result = true;
    }

    if ($("#LTDSYes").is(':checked')) {
        result = validateTextBox('txtLowTDSRates', 'Please provide Low TDS Rates');
        if (!result) {
            result = false;
            return false;
        }
        else {
            result = true;
        }
        if (lowTDSRates < 5) {
            result = validateTextBox('txtLowerTDSCertificateNumber', 'Please provide Lower TDS Certificate Number');
            if (!result) {
                result = false;
                return false;
            }
            else {
                result = true;
            }

            if (fromDate == '') {
                writeMultipleMessage("error", "Please enter validity period from.", 'dpValidityPeriodFrom');
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", "", 'dpValidityPeriodFrom');
                result = true;
            }

            if (toDate == '') {
                writeMultipleMessage("error", "Please enter validity period to.", 'dpValidityPeriodTo');
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", "", 'dpValidityPeriodTo');
                result = true;
            }

            if (fromDate != "" && toDate != "") {
                if (!CheckForSameFinancialYear(toDate, fromDate, 'dateto', 'To date & From date should be Same Financial Year in TDS Details')) {
                    result = false;
                    return false;
                }
                else {
                    writeMultipleMessage("error", "", 'dpValidityPeriodTo');
                    result = true;
                }

                if (!CompareTwoDates(fromDate, toDate, 'dateto', 'To date Should be greater than From date in TDS Details')) {
                    result = false;
                    return false;
                }
                else {
                    writeMultipleMessage("error", "", 'dpValidityPeriodTo');
                    result = true;
                }
            }

            if (lTDSCertificateReceivedDate == "") {
                writeMultipleMessage("error", "Please enter LTDS Certificate Received Date", 'dpLTDSCertificateReceivedDate');
                result = false;
                return false;
            }
            else {
                writeMultipleMessage("error", "", 'dpLTDSCertificateReceivedDate');
                result = true;
            }

            result = validateTextBox('txtThresholdAmount', 'Please provide Threshold Amount');
            if (!result) {
                result = false;
                return false;
            }
            else {
                result = true;
            }
        }
        else {
            writeMultipleMessage("error", "", 'txtLowerTDSCertificateNumber');
            writeMultipleMessage("error", "", 'dpValidityPeriodTo');
            writeMultipleMessage("error", "", 'dpLTDSCertificateReceivedDate');
            writeMultipleMessage("error", "", 'txtThresholdAmount');
        }
    }
    else {
        writeMultipleMessage("error", "", 'txtLowTDSRates');
        writeMultipleMessage("error", "", 'txtLowerTDSCertificateNumber');
        writeMultipleMessage("error", "", 'dpValidityPeriodTo');
        writeMultipleMessage("error", "", 'dpLTDSCertificateReceivedDate');
        writeMultipleMessage("error", "", 'txtThresholdAmount');
    }

    return result;
}

function ResetBranchSMMapping() {
    console.log("Hitting ResetBranchSMMapping");
    $(".branchCode").val("");
    $(".smcode").empty();
    $(".employeeCode").val("");
}