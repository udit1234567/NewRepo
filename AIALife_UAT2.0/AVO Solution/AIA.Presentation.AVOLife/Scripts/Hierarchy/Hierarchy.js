
function SaveHierarchyBasicDetails() {    
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    var EntityTyp = $("#ddlHierarchyEntityType").val();
    var EntityValue = $("#ddlHierarchyEntityType option:selected").text();
    var SubChannelCode = "";
    var PartnerCode = "";
    if (EntityTyp != '' && EntityTyp != undefined) {
        if (EntityTyp == "1255" || EntityValue == "Sub Channel") {
            //var SubChannelCode = $("#txtHirerachyCode").val();
            var SubChannelCode = $("#hdnSubChannelCode").val();
        }
        if (EntityTyp == "1256" || EntityValue == "Partner") {
            //var PartnerCode = $("#txtHirerachyCode").val();
            var PartnerCode = $("#hdnPartnerCode").val();
        }
    }   
    if (!ValidateHierarchyBasicDetails()) {

    }
    else {
        $.post("../../Hierarchy/SaveHierarchyBasics", $("#frmHierarchyDetails").serialize())
            .done(function (data) {
                if (data.Message == "Success") {
                    if (data.EntityType == "1254") {
                        $("#hdnOrgStructureIDChannel").val(data.OrgStructureIDChannel);
                    }
                    if (data.EntityType == "1255") {
                        $("#hdnOrgStructureIDSubChannel").val(data.OrgStructureIDSubChannel);
                    }
                    var Message = '<p> Data Saved Successfully' + ' </p>';
                }
                else {
                    if (data.EntityType == "483") {
                        var Message = '<p>You Cannot Create more than One Corporate.</p>';
                    }
                    else {
                        var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                    }                    
                }
                $('#HierarchyModalBody').html(Message);
                $("#HierarchyModal").modal("show");
                if (SubChannelCode != '' && SubChannelCode != undefined) {
                    LoadGeoSubChannelParentEntity(SubChannelCode);
                }
                if (PartnerCode != '' && PartnerCode != undefined) {
                    LoadGeoPartnerParentEntity(PartnerCode);
                }
            }).
            fail(function (data) {
                if (data.EntityType == "483") {
                    BootstrapDialog.alert("You Cannot Create more than One Corporate.");
                }
                else {
                    BootstrapDialog.alert("Data Submission Failed. Try after some Time");
                }
                
            });
        $("#ddlHierarchyRepositoryType").attr("disabled", true);
    }
}
//function LoadGeoSubChannelParentEntity(SubChannelCode) {    
//    if (SubChannelCode != '' && SubChannelCode != undefined) {
//        $.post("../Hierarchy/LoadGeoSubChannelParentEntity?SubChannelCode=" + SubChannelCode)
//            .done(function (data) {
//                $('#ddlParentEntity1').html(""); // clear before appending new list
//                $('#ddlParentEntity1').append($('<option value=\'\'>Select</option>'));
//                $.each(data, function (ID, option) {
//                    $('#ddlParentEntity1').append($('<option value=\'' + option.ID + '\'>' + option.Value + '</option>'));
//                });
//            });
//    }
//}
//function LoadGeoPartnerParentEntity(PartnerCode) {    
//    if (PartnerCode != '' && PartnerCode != undefined) {
//        $.post("../Hierarchy/LoadGeoPartnerParentEntity?PartnerCode=" + PartnerCode)
//            .done(function (data) {
//                $('#ddlParentEntity1').html(""); // clear before appending new list
//                $('#ddlParentEntity1').append($('<option value=\'\'>Select</option>'));
//                $.each(data, function (ID, option) {
//                    $('#ddlParentEntity1').append($('<option value=\'' + option.ID + '\'>' + option.Value + '</option>'));
//                });
//            });
//    }
//}
function SaveHierarchyCommunicationDetails() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    if (!ValidateHierarchyCommunicationDetails()) {

    }
    else {
        $.post("../../Hierarchy/SaveHierarchyCommunication", $("#frmHierarchyDetails").serialize())
        .done(function (data) {
            if (data.Message == "Success") {                
                $("#hdnCommunicationAddressId").val(data.CommunicationAddressId);
                $("#hdnRegistrationAddressId").val(data.RegistrationAddressId);
                var Message = '<p> Data Saved Successfully' + ' </p>';                
                if (data.EntityType == "483" || data.EntityType == "1254" || data.IsPartnerHierarchy == true)
                {
                    $("#btnBasicSubmitDetails").show();
                }
                else {
                    $("#btnBasicSubmitDetails").hide();
                }
            }
            else {
                if (data.EntityType == "483") {
                    var Message = '<p>You Cannot Create more than One Corporate.</p>';
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                }
            }
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");

        }).
        fail(function (data) {
            BootstrapDialog.alert("Data Submission Failed. Try after some Time");
        });
        $("#ddlHierarchyRepositoryType").attr("disabled", true);
    }
  }
function SubmitHierarchyDetails() {    
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    var entityVal = $("#ddlHierarchyEntityType").val();
    var entityText = $("#ddlHierarchyEntityType option:selected").text();
    if (entityVal == "483" || entityText == "Corporate") {
        if (!ValidateHierarchyBasicDetails() || !ValidateHierarchyCommunicationDetails()) {

        }
        else {
            var EntityType = $("#ddlHierarchyEntityType option:selected").text();
            var EntityVal = $("#ddlHierarchyEntityType").val();
            var Code = $("#txtHirerachyCode").val();
            //var Message = '<p> ' + EntityType + "  " + Code + " is Created Successfully" + ' </p>';
            var Message = '<p>You cannot Create Corporate.</p>';
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");
        }
    }
    if (entityVal == "1254" || entityText == "Channel") {
        if (!ValidateHierarchyBasicDetails() || !ValidateHierarchyCommunicationDetails()) {

        }
        else {            
            var EntityType = $("#ddlHierarchyEntityType option:selected").text();
            var EntityVal = $("#ddlHierarchyEntityType").val();
            var Code = $("#txtHirerachyCode").val();
            var ChannelCode = $("#hdnChannelCode").val();
            var Message = '<p> ' + EntityType + "  " + ChannelCode + " is Created Successfully" + ' </p>';
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");
            $("#txtHirerachyStatus").attr("disabled", false);
            $("#txtHirerachyStatus").val(1);
            $("#txtHirerachyStatus").attr("disabled", true);
        }
    }
    if (entityVal == "1255" || entityText == "Sub Channel") {
        if (!ValidateHierarchyBasicDetails() || !ValidateHierarchyCommunicationDetails()) {

        }
        else {
            var EntityType = $("#ddlHierarchyEntityType option:selected").text();
            var EntityVal = $("#ddlHierarchyEntityType").val();
            var Code = $("#txtHirerachyCode").val();
            var SubChannelCode = $("#hdnSubChannelCode").val();
            var Message = '<p> ' + EntityType + "  " + SubChannelCode + " is Created Successfully" + ' </p>';
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");
            $("#txtHirerachyStatus").attr("disabled", false);
            $("#txtHirerachyStatus").val(1);
            $("#txtHirerachyStatus").attr("disabled", true);
        }
    }
    $("#ddlHierarchyRepositoryType").attr("disabled", true);
}  
function SaveHierarchyPrincipalDetails() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);   
        var Grid = [];
        getPointofContactDetails(Grid);
        $.post("../../Hierarchy/SaveHierarchyPointofContact", $("#frmHierarchyDetails").serialize())
            .done(function (data) {
                if (data.Message == "Success") {
                    var Message = '<p> Data Saved Successfully' + ' </p>';
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                }
                $('#HierarchyModalBody').html(Message);
                $("#HierarchyModal").modal("show");
            }).
            fail(function (data) {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
        $("#ddlHierarchyRepositoryType").attr("disabled", true);
 }
function SaveGeoUnitDetails() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    clearAllErrors();
    var Grid = [];
    getGeoUnitData(Grid);
    var GeoUnitNameCount = $("#hdnGeoUnitNameGridCount").val();
    var ParentEntityGridCount = $("#hdnParentEntityGridCount").val();
    if (GeoUnitNameCount == "1") {
        return false;
    }
    if (ParentEntityGridCount == "1") {
        return false;
    }
    $.post("../../Hierarchy/SaveHierarchyGeoUnit", $("#frmHierarchyDetails").serialize())
        .done(function (data) {            
            if (data.Message == "Success") {
                var Message = '<p> Data Saved Successfully' + ' </p>';             
                if (data.IsPartnerHierarchy == false && data.EntityType == "1255") {
                    $("#btnSubmitGEOUnitDetails").show();
                }
                else {
                    $("#btnSubmitGEOUnitDetails").hide();
                }
            }
            else {
                var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
            }
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");
        }).
        fail(function (data) {
            BootstrapDialog.alert("Data Submission Failed. Try after some Time");
        });
    $("#ddlHierarchyRepositoryType").attr("disabled", true);
}
function SaveHierarchyDocumentsDetails() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    clearAllErrors();
    //if (!ValidateDocumentData()) {

    //}
    //else {
        var Grid = [];
        var data = new FormData();
        var EntityVal = $("#ddlHierarchyEntityType").val();
        getDocumentsData(Grid, data);  
        var DocumentUpload = $("#hdnDocumentUploadCount").val();
        if (DocumentUpload == "1") {
            return false;
        }
        if (ErrorCode == false) {
            return false;
        }
        var docData = JSON.stringify(Grid);
        $.ajax({
            type: "POST",
            url: "../../Hierarchy/UploadFilePath?documentData=" + docData + "&PartnerCode=" + $("#hdnPartnerCode").val() + "&EntityType=" + EntityVal,
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {
                if (data.Message == 'Success') {
                    var Message = '<p> Data Saved Successfully' + ' </p>';
                    $("#btnSubmitDocumentDetails").show();
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                }
                $('#HierarchyModalBody').html(Message);
                $("#HierarchyModal").modal("show");
            },
            failure: function (data) {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");

            }
        });
        $("#ddlHierarchyRepositoryType").attr("disabled", true);

//}
    
}

//function ValidateDocumentData() {
//debugger
//    var DocumentUpload = 0;
//    ValidateDocumentsData();   
//    var DocumentUpload = $("#hdnDocumentUploadCount").val();
//    if (DocumentUpload == "1") {
//        //writeMultipleMessage("error", "Please Provide a file to upload", "txtFileName");
//        return false;
//    }
//    return true;
//}

function SubmitHierarchyPartner() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    //if (!ValidateHierarchyBasicDetails() || !ValidateHierarchyCommunicationDetails() || !ValidateHierarchyPointOfContactDetails() || !ValidateLicenseDetails()) {
    //    return false;
    //}
    var entityVal = $("#ddlHierarchyEntityType").val();
    var entityText = $("#ddlHierarchyEntityType option:selected").text();
    if (entityVal == "1256" || entityText == "Partner") {
        if (!ValidateHierarchyBasicDetails() || !ValidateHierarchyCommunicationDetails() || !ValidateLicenseDetails()) {

        }
        else {
            var EntityType = $("#ddlHierarchyEntityType option:selected").text();
            var EntityVal = $("#ddlHierarchyEntityType").val();
            var Code = $("#txtHirerachyCode").val();
            var PartnerCode = $("#hdnPartnerCode").val();
            var Message = '<p> ' + EntityType + "  " + PartnerCode + " is Created Successfully" + ' </p>';
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");
            $("#txtHirerachyStatus").attr("disabled", false);
            $("#txtHirerachyStatus").val(1);
            $("#txtHirerachyStatus").attr("disabled", true);
        }
    }
    $("#ddlHierarchyRepositoryType").attr("disabled", true);
}
      
function SaveHierarchyLicenseDetails() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    if (!ValidateLicenseDetails()) {

    }
    else {
        $.post("../../Hierarchy/SaveHierarchyLicense", $("#frmHierarchyDetails").serialize())
            .done(function (data) {
                if (data.Message == "Success") {
                    var Message = '<p> Data Saved Successfully' + ' </p>';
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                }
                $('#HierarchyModalBody').html(Message);
                $("#HierarchyModal").modal("show");
            }).
            fail(function (data) {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
        $("#ddlHierarchyRepositoryType").attr("disabled", true);
    }
}
function SaveHierarchyCoordinationDetails() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    $.post("../../Hierarchy/SaveHierarchyGeoUnit", $("#frmHierarchyDetails").serialize())
        .done(function (data) {
            if (data.Message == "Success") {
                var Message = '<p> Data Saved Successfully' + ' </p>';
                $("#btnCordinationsSubmitDetails").show();
            }
            else {
                var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                $("#btnCordinationsSubmitDetails").hide();
            }
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");
        }).
        fail(function (data) {
            BootstrapDialog.alert("Data Submission Failed. Try after some Time");
        });
    $("#ddlHierarchyRepositoryType").attr("disabled", true);
}

function ValidateHierarchyBasicDetails() {
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    clearAllErrors();
    var entityVal = $("#ddlHierarchyEntityType").val();
    var entityText = $("#ddlHierarchyEntityType option:selected").text();
    if (entityVal == "483" || entityText == "Corporate") {
        if (validateTextBox('txtHirerachyName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        //if (validateTextBox('dpHirerachyYearofEstablish', 'All mandatory Basic fields are required') == false) {
        //    return false;
        //}
        if (validateTextBox('txtHirerachyYearofEstablish', 'All mandatory Basic fields are required') == false) {
            return false;
        }
    }
    else if (entityVal == "1254" || entityText == "Channel") {
        if (validateTextBox('txtHirerachyName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        //if (validateTextBox('txtHirerachyCode', 'All mandatory Basic fields are required') == false) {
        //    return false;
        //}
        if (validateDropDown('ddlHierarchyRepositoryType', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateTextBox('txtHirerachyReportingName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateTextBox('txtHirerachyReportingCode', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        var HierEffectiveDate = $("#dpHierarchyEffectiveFrom").val();
        //if (HierEffectiveDate == '' && HierEffectiveDate == undefined) {
        //    writeMultipleMessage("error", "All mandatory fields are Required", 'dpHierarchyEffectiveFrom');
        //    return false;
        //}
        //if (!ValidateHierEffectivedate())
        //{
        //    writeMultipleMessage("error", "Effective date cannot be future date", 'dpHierarchyEffectiveFrom');
        //    return false;
        //}
        //else {
        //    writeMultipleMessage("error", "", 'dpHierarchyEffectiveFrom');
        //}
    }
    else if (entityVal == "1255" || entityText == "Sub Channel") {
        if (validateTextBox('txtHirerachyName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        //if (validateTextBox('txtHirerachyCode', 'All mandatory Basic fields are required') == false) {
        //    return false;
        //}
        if (validateDropDown('ddlHierarchyRepositoryType', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateTextBox('txtHirerachyReportingCode', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateTextBox('txtHirerachyReportingName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
    }
    else if (entityVal == "1256" || entityText == "Partner") {
        if (validateTextBox('txtHirerachyName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateDropDown('ddlHierarchyPartnerTypes', 'All mandatory Hierarchy Basic fields are required') == false) {
            return false;
        }
        if (validateDropDown('ddlHierarchyRepositoryType', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateTextBox('txtHirerachyReportingCode', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateTextBox('txtHirerachyReportingName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
    }
    else if (entityVal == "1257" || entityText == "Geo Unit") {
        if (validateTextBox('txtHirerachyName', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        //if (validateDropDown('ddlHierarchygeounittypes', 'All mandatory  Basic fields are required') == false) {
        //    return false;
        //}
        //if (validateTextBox('txtHirerachyCode', 'All mandatory Basic fields are required') == false) {
        //    return false;
        //}
        if (validateDropDown('ddlHierarchyRepositoryType', 'All mandatory Basic fields are required') == false) {
            return false;
        }
        if (validateTextBox('txtHirerachyReportingCode', 'All mandatory Basic fields are required') == false) {
            return false;
        }
    }
    return true;
    $("#ddlHierarchyRepositoryType").attr("disabled", true);
}
function ValidateHierarchyCommunicationDetails() {
    
        if (validateTextBox('txtMobileNo', 'All Mandatory Communication Details Are Required') == false) {
            return false;
        }
        var HierMobileNo = $("#txtMobileNo").val();
        if (HierMobileNo.length < 10) {
            writeMultipleMessage("error", " Mobile Number One should be 10 digits", "txtMobileNo");
            return false;
        }
        if (validateTextBox('txtOfficePhone1', 'All Mandatory Communication Details Are Required') == false) {
            return false;
        }
        var HierOfficeNo = $("#txtOfficePhone1").val();
        if (HierOfficeNo.length < 10) {
                writeMultipleMessage("error", "Communication Office Number 1 should be 10 digits", "txtOfficePhone1");
                return false;
            }
            if (validateMobileNo('txtOfficePhone2', 'Communication office Number 2 No Should be 9 Digits') == false) {
            return false;
        }
        var HierEmailID = $("#txtEmailId").val();
        if (HierEmailID != '' && HierEmailID != undefined) {
            if (EmailValidation('txtEmailId', 'Please Enter Valid Email ID') == false)
                return false;
        }
        if (validateTextBox('objCommunicationAddressAddress1', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }
        if (validateDropDown('objCommunicationAddressddlProvince', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }

        if (validateDropDown('objCommunicationAddressddlDistrict', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }
        if (validateDropDown('objCommunicationAddressddlCity', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }
        if (validateTextBox('objCommunicationAddressPincode', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }
        if (validateTextBox('objRegistrationAddressAddress1', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }
        if (validateDropDown('objRegistrationAddressddlProvince', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }

        if (validateDropDown('objRegistrationAddressddlDistrict', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }
        if (validateDropDown('objRegistrationAddressddlCity', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }
        if (validateTextBox('objRegistrationAddressPincode', 'All mandatory fields are required in Communication Details') == false) {
            return false;
        }

    
        return true;
}
function ValidateLicenseDetails() {
    //if (entityVal == "1256" || entityText == "Partner") {
        if (validateTextBox('txtLicenseNo', 'All mandatory  License Details are required') == false) {
            return false;
        }
        if (validateTextBox('dpLicenseIssuedate', 'All mandatory  License Details are required') == false) {
            return false;
        }
        if (validateTextBox('dpLicenseExpirydate', 'All mandatory  License Details are required') == false) {
            return false;
        }
        if (validateTextBox('dpContractFrom', 'All mandatory  License Details are required') == false) {
            return false;
        }
        if (validateTextBox('dpContractTo', 'All mandatory  License Details are required') == false) {
            return false;
        }
        return true;
    //}
}

function ValidateHierarchyPointOfContactDetails() {
  
        if (validateTextBox('txtPointPosition', 'All mandatory Point Of contact Details are required') == false) {
            return false;
        }
        if (validateDropDown('ddlPointSalutation', 'All mandatory Point Of contact Details are required') == false) {
            return false;
        }
        if (validateTextBox('txtPointFirstName', 'All mandatory Point Of contact Details are required') == false) {
            return false;
        }
        if (validateTextBox('txtPointLastName', 'All mandatory Point Of contact Details are required') == false) {
            return false;
        }
        if (validateTextBox('txtPointMobile1', 'All mandatory Point Of contact Details are required') == false) {
            return false;
        }
        var PointMobileNo = $("#txtPointMobile1").val();
        if (PointMobileNo.length < 10) {
            writeMultipleMessage("error", " Point Mobile Number One should be 10 digits", "txtPointMobile1");
            return false;
        }
        if (validateMobileNo('txtPointMobile2', 'Point Mobile No 2 Should be 10 Digits') == false) {
            return false;
        }
        if (validateMobileNo('txtPointOfficePhone', 'Point Office Should be 10 Digits') == false) {
            return false;
        }
        if (validateMobileNo('txtPointResidencePhone', 'Point Residence Mobile No  Should be 10 Digits') == false) {
            return false;
        }
        var PointEmailID = $("#txtPointofEmail").val();
        if (PointEmailID != null) {
            if (EmailValidation('txtPointofEmail', 'Please Enter Valid Email ID') == false)
                return false;
        }
        return true;
}
function ClearErrOnChange() {
    clearAllErrors();
}
//function ValidateHierEffectivedate() {    
//    var dob = $('#dpHierarchyEffectiveFrom').val();
//    var d = new Date();
//    var month = d.getMonth()+1;
//    var day = d.getDate();
//    var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
//    var HierEffectiveFromDate = $('#dpHierarchyEffectiveFrom').val();
//    var arrHIEREffectiveFromDate = HierEffectiveFromDate.split('/');
//    var HierEffectiveDay = arrHIEREffectiveFromDate[0];
//    var HierEffectiveMonth = arrHIEREffectiveFromDate[1];
//    var HierEffectiveYear = arrHIEREffectiveFromDate[2];
//    var arrCurrentDate = currentDate.split('/');
//    var currentDay = arrCurrentDate[0];
//    var currentMonth = arrCurrentDate[1];
//    var currentYear = arrCurrentDate[2];
//    if (HierEffectiveYear <= currentYear) {
//        if (HierEffectiveMonth <= currentMonth) {
//            if (HierEffectiveDay <= currentDay) {
//                return true;
//            }
//            else if (HierEffectiveDay > currentDay && HierEffectiveMonth < currentMonth) {
//                return true;
//            }
//            else if (HierEffectiveDay > currentDay && HierEffectiveMonth >= currentMonth && HierEffectiveYear < currentYear) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }
//        else if (EffectiveMonth > currentMonth && EffectiveYear < currentYear) {
//            return true;
//        }
//        else {
//            return false;
//        }
//    }
//    else {
//        return false;
//    }
//}

function ShowQuickAction() {    
    var SearchQuickActionId = $("#ddlSearchQuickAction").val();
    var SearchQuickActionVal = $("#ddlSearchQuickAction option:selected").text();
    if (SearchQuickActionId != '' && SearchQuickActionId != undefined) {
        if (SearchQuickActionId == "1" && SearchQuickActionVal == "View") {
            $("#frmHierarchySearch").attr('action', '/Hierarchy/Hierarchy');
            document.forms["frmHierarchySearch"].submit();
        }
    }   
}
function GetExpiryDate() {
    var IssueDate = $('#dpLicenseIssuedate').val();
    var arrIssueDate = IssueDate.split('/');
    var IssueDay = arrIssueDate[0];
    var IssueMonth = arrIssueDate[1];
    var IssueYear = getValue(parseInt(arrIssueDate[2]));
    var ExpiryYear = IssueYear + 1;
    var ExpiryDate = IssueDay + "/" + IssueMonth + "/" + ExpiryYear;
    if (ExpiryDate != "" || ExpiryDate != undefined) {
        $("#dpLicenseExpirydate").val(ExpiryDate);
    }
}
function getValue(val) {
    if (isNaN(val) == true) {
        return 0;
    }
    else {
        return val;
    }
}
function SaveHierarchyCoordinationDetails() {    
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    clearAllErrors();
    var Grid = [];
    var Array = [];
    GetCoordinationGridData(Grid);
    $.post("../../Hierarchy/SaveHierarchyCoordinationDetails?GetCoordinationGridData=" + JSON.stringify(Grid), $("#frmHierarchyDetails").serialize())
        .done(function (data) {
            if (data.Message == "Success") {
                var Message = '<p> Data Saved Successfully' + ' </p>';
                $("#btnCordinationsSubmitDetails").show();
            }
            else {
                var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
            }
            $('#HierarchyModalBody').html(Message);
            $("#HierarchyModal").modal("show");
        }).
        fail(function (data) {
            BootstrapDialog.alert("Data Submission Failed. Try after some Time");
        });
    $("#ddlHierarchyRepositoryType").attr("disabled", true);
}
function SubmitGeoUnitDetails() {    
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
    var entityVal = $("#ddlHierarchyEntityType").val();
    var entityText = $("#ddlHierarchyEntityType option:selected").text();
    if (entityVal == "1257" || entityText == "Geo Unit") {
        var EntityType = $("#ddlHierarchyEntityType option:selected").text();
        var EntityVal = $("#ddlHierarchyEntityType").val();
        var Code = $("#txtHirerachyCode").val();
        var GeoUnitCode = $("#hdnGeoUnitCode").val();
        var Message = '<p> ' + EntityType + "  " + GeoUnitCode + " is Created Successfully" + ' </p>';
        $('#HierarchyModalBody').html(Message);
        $("#HierarchyModal").modal("show");
        $("#txtHirerachyStatus").attr("disabled", false);
        $("#txtHirerachyStatus").val(1);
        $("#txtHirerachyStatus").attr("disabled", true);
    }
    $("#ddlHierarchyRepositoryType").attr("disabled", false);
}