
//future criticare validation
//validation for transaction type
function ValidateTransactionType(id, message) {

    if ($("#" + id).val() == "") {
        writeMultipleMessage("error", "" + message, id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", '' + id);

        return true;
    }

}
//check sum insured is empty or not
function IsEmpty(id, message) {
    if ($("#txtSumInsured").val() == "") {
        writeMultipleMessage("error", "please enter Sum ", 'txtSumInsured');
    }
    else {
        writeMultipleMessage("error", "", 'txtSumInsured');
    }
}
//document upload validation
function ValidateDocument() {

    if ($("#ddlDocumentName").val() == "Select" || $("#ddlDocumentName").val() == "") {
        BootstrapDialog.alert("Please Select document");
        return false;
    }
}
//family health history entry screen validation
//validations for all date pickers
function validateDate(id, message) {
    if ($("#" + id).val() == "") {
        writeMultipleMessage("error", "" + message, id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", '' + id);

        return true;
    }

}
//added for policy search page
function validatePolicySearch() {
    if (($("#txtCustName").val() == "") && ($("#txtPolicyNumber").val() == "") && ($("#dtCustomerDOB").val() == "") && ($("#txtCustomerCode").val() == "")) {

        writeMultipleMessage("error", "please provide atleast one field value for searching policy", 'txtCustomerCode');
    }
    else {
        writeMultipleMessage("error", "", 'txtCustomerCode');
    }
}
//added for quote search page
function validateQuoteSearch() {

    if (($("#txtFirstName").val() == "") && ($("#txtLastName").val() == "") && ($("#dtQuoteCreationDateFrom").val() == "") && ($("#dtQuoteCreationDateTo").val() == "") && ($("#txtQuoteNumber").val() == "") && ($("#dtCustomerDOB").val() == "")) {
        writeMultipleMessage("error", "please provide atleast one field value for searching quote", 'CustomerDOB');
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'CustomerDOB');
        return true;
    }
}
function QuoteSearchGrid() {

    var result;
    result = validateQuoteSearch();
    if (result == true) {
        var x = $("#formSearchQuote").serialize();
        $.post("../../Health/SearchQuoteGrid", $("#formSearchQuote").serialize())
             .done(function (data) {
                 $("#divSearchQuoteGrid").html(data);
             }).
             fail(function () {
                 $("#divSearchQuoteGrid").html("Failed to fetch data");
             });
    }

}
function PolicySearchGrid() {

    var x = $("#formSearchPolicy").serialize();
    $.post("../../Health/SearchPolicyGrid", $("#formSearchPolicy").serialize())
         .done(function (data) {
             $("#divSearchPolicyGrid").html(data);
         }).
         fail(function () {
             $("#divSearchPolicyGrid").html("Failed to fetch data");
         });

}
//calculate the age based on dateof birth selection
function ageCount() {

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    var today = dd + '/' + mm + '/' + yyyy;
    var current = today.split("/");
    var currentYear = current[2];
    var dob = ($("#dtDOB").val()).split("/");
    var year = dob[2];
    var age = currentYear - year;
    $("#txtAge").val(age);
    return true;
}
function ViewQuoteSchedule() {

    //$('#formSearchQuote').attr('action', '/PAS/Health/Proposal');
    //document.forms["formSearchQuote"].submit();
    $('#formSearchQuote').attr('action', 'CreatePDF');
    document.forms["formSearchQuote"].submit();


}
function funcViewPolicySchedule() {

    $('#formSearchPolicy').attr('action', '/Health/Proposal');
    document.forms["formSearchPolicy"].submit();


}
function saveQuote() {
    var jsonObj = [];
    var IsValid = true;
    var count = 0;
    for (var index = 0; index < quoteGridData.length; index++) {
        var id = quoteGridData[index].Index;
        var FirstName = $('#FirstName' + id).val();
        var LastName = $('#LastName' + id).val();
        var DateOfBirth = $('#DateOfBirth' + id).val();
        var SumInsured = $('#ddlSumInsured' + id).val();
        var Relationship = $('#Relationship' + id).val();
        jsonObj.push({ FirstName: FirstName, LastName: LastName, DateOfBirth: DateOfBirth, Individual_sumInsured: SumInsured, RelationshipWithProposer: Relationship });
        count++;

    }
    var x = $("#formQuotation").serialize();
    $.ajax({

        type: "post",
        url: '/Health/saveHealthPolicy',
        type: 'POST',
        data:
            JSON.stringify({
                objHealth: $("#formQuotation").serializeObject(),
                objPolicyClient: jsonObj
            }),
        dataType: 'html',
        contentType: 'application/json; charset=utf-8', success: function (data) {
        }
    });
}
//If Transaction type is Renewel we have to show policysearch
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
function GridSelect(e) {

    clientid = SearchClientId;
    $.post("../../Health/FillSearchedData", { ClientID: clientid })
       .done(function (data) {
           $("#showClientSearch").dialog('close')
           var dateString = data.Client_DOB.substr(6);
           var currentTime = new Date(parseInt(dateString));
           var month = currentTime.getMonth() + 1;
           var day = currentTime.getDate();
           var year = currentTime.getFullYear();
           data.Client_DOB = day + "/" + month + "/" + year;
           $("#divIndividualFloat .grid-row-selected [data-name='FirstName'] :input").val(data.Client_FirstName);
           $("#divIndividualFloat .grid-row-selected [data-name='LastName'] :input").val(data.LastName);
           $("#divIndividualFloat .grid-row-selected [data-name='DateOfBirth'] :input").val(data.Client_DOB);
           $("#hdnCustomerCode").val(clientid);

       }).
            fail(function () {
                $("#divClientDetailsId").html("Failed to fetch data");
            });
}

function RenderPaymentPage(e) {
    var x = e.value;
    if ($('input[type=radio][name=IsClientPayment]:checked').val() == "True") {
        IsClient = true;
    }
    else if ($('input[type=radio][name=IsClientPayment]:checked').val() == "False") {
        IsClient = false;
    }
    $("#frmAllHealthProducts").attr('action', 'PaymentPageForProduct?PaymentMode=' + x + '&IsClient=' + IsClient);
    document.forms["frmAllHealthProducts"].submit();

}
function GeneratePDF() {
    $('#frmProductPayment').attr('action', 'IssuePolicySchedule');
    document.forms["frmProductPayment"].submit();
}
function ShowMobileNumber() {
    if ($('#chkSMS').is(':checked') == true) {
        $('#divMobileNumber').show();
        $('[name="IsSMSReq"]:hidden').remove()
    }
    else {
        $('#divMobileNumber').hide();

    }
}
function ShowEmailId() {
    if ($('#chkEmail').is(':checked') == true) {
        $('#divEmailId').show();
        $('[name="ISMailReq"]:hidden').remove()
    }
    else {
        $('#divEmailId').hide();
    }
}
//Health Surplus
function TransactionTypeChange() {

    if ($('#ddlBusinessTypeId').val() == "26")//Renewal
    {
        $('#divPanels').hide();
        $('#divRenewalPolicy').show();


    }
    else {
        $('#divPanels').show();
        $('#divRenewalPolicy').hide();

    }

}
function PolicyTypeChange() {
    debugger;
    $('#divRenewalPolicy').hide();
    $("#divPanels").empty();
    if (($("#ddlPolicyType option:selected").text()) == "--Select--") {
        writeMultipleMessage("error", "Please select Policy Type", "ddlPolicyType");
    }
    else {
        $.post("../../Health/QuickQuotePanels", $("#frmHealthQuickQuotes").serialize())
       .done(function (data) {

           $("#divPanels").html(data);
       }).
       fail(function () {
           $("#divPanels").html("Failed to fetch data");
       });
        writeMultipleMessage("error", "", "ddlPolicyType");
    }

}
function CheckProductCode(ProductCode, PolicyType) {

    $('#Healthcommongrid').load('../Health/LoadCommonQuoteGridHealth?ProductCode=' + ProductCode + '&PolicyType=' + PolicyType);

}
var quoteGridDataIndividualFloatData = [];
function AddCommonGridRow(e) {

    ErrorCode = true;
    var Productcode = $('#hdnProductCode').val();
    var PolicyType = $('#ddlPolicyType').val();
    if (PolicyType == "89") {
        Delterow = false;

        if ($("#divIndividualFloat tbody").children('tr').length > 5) {
            $('#btnAddGridRow').attr("disabled", "disabled");
            return false;
        }
    }
    else {

        Delterow = false;
        if ($("#divIndividualFloat tbody").children('tr').length > 3) {
            $('#btnAddGridRow').attr("disabled", "disabled");
            return false;
        }
    }

    if (ErrorCode == true) {
        var quoteGridDataIndividualFloat = [];
        var newRow;
        quoteGridDataIndividualFloat = quoteGridDataIndividualFloatData;
        var length = (quoteGridDataIndividualFloat.length) - 1;
        if (quoteGridDataIndividualFloat.length == 0) {
            length = 0;
        }
        else {
            length = quoteGridDataIndividualFloat[length].Index + 1;
        }
        quoteGridDataIndividualFloat.push({ Index: length });
        var btnClientSearchId;
        var deleteGridRowId;
        var DeleteRowId;


        btnClientSearchId = 'btnHSClientSearch' + length;
        deleteGridRowId = 'btnHSDeleteGridRow' + length;
        DeleteRowId = 'btnHSDeleteGridRow';



        var FirstNameId = 'txtFirstName' + length;
        var LastNameId = 'txtLastName' + length;
        var DateOfBirthId = 'DateOfBirth' + length;
        var RelationshipId = 'ddlRelationship' + length;


        var dobMarkup = "<td class='grid-cell' data-name='DateOfBirth'><div id='divDateOfBirth" + length + "' class='input-group date'= " +
           "data-date-format='DD/MM/YYYY'><input data-role='htmlDatePicker' id='DateOfBirth" + length + "' name='DateOfBirth" + length + "' onchange = 'ValidateAge(this)' " +
           "type='text' style='width:100%' class='form-control' onblur='ValidateDateOfBirth(this)'><span class='input-group-addon' style='padding:0px'><span class='glyphicon-calendar glyphicon'></span></span></input></div></td> ";


        newRow = "<tr class='grid-row'><td class='grid-cell text-center' data-name='ClientSearch'><input type='button' style='display:None;' class='btn btn-default' value='Client Search' id=\'" + btnClientSearchId + "\' onclick='showpopup()'></input></td><td class='grid-cell' data-name='FirstName'><input name='FirstName' type='text'  style='width:100%'  class='form-control' onblur='ValidateFirstName(this)' id=\'" + FirstNameId + "\'></input></td><td class='grid-cell' data-name='LastName'><input name='LastName'  style='width:100%'  type='text' class='form-control' onblur='ValidateLastName(this)' id=\'" + LastNameId + "\'></input></td>" + dobMarkup + "</td>" +
          //  "<td class='grid-cell' data-name='Plan'><select name='Plan' class='form-control' onblur = 'ValidatePlan(this)' onchange='ValidateHSPlan(this)' id=\'" + PlanId + "\'><option>Select</option></select></td><td class='grid-cell' data-name='Individual_sumInsured'><select name='Individual_sumInsured' class='form-control' onblur='ValidateSumInsured(this)' id=\'" + SumInsuredId + "\' ><option>Select</option></select></td>
            "<td class='grid-cell' data-name='RelationshipWithProposer'><select name='RelationshipWithProposer'  style='width:100%' class='form-control' onblur = 'ValidateRelationship(this)' id=\'" + RelationshipId + "\'><option>Select</option></select></td>" +
            "<td class='grid-cell text-center' data-name='Delete'><input class='btn btn-default' id=\'" + deleteGridRowId + "\' style='margin-top: 3px;' type='button' value='Delete' onclick='DeleteRow(this,\"" + DeleteRowId + "\")' /></td></tr>";

        $('#divIndividualFloat tbody:last').append(newRow);

        items = "";
        $.each(relationshipData, function (index, itemData) {
            items += '<option Value=' + itemData.ID + '>' + itemData.Value + '</option>';
        });
        $("#" + RelationshipId).append(items);


        $(function () { $('#DateOfBirth' + length).datetimepicker({ format: 'DD/MM/YYYY', useCurrent: false, pickTime: false }); });
    }

}
function ValidateDateOfBirth(e) {
    var dob = $('#' + e.id).val();
    var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/);
    var result = dtRegex.test(dob);
    if (!result) {
        writeMultipleMessage("error", "This Field Must be valid Date Of Birth", e.id);
        return false;
    }
    else {
        writeMultipleMessage("error", "", e.id);
        return true;

    }
}
//var hdnDiscrepency = true;
function CalculateHealthPremium() {
    debugger;
    ErrorCode = true;
    ValidateDropDown("ddlBranchName", "Please Select Branch Name");
    var Productcode = $('#hdnProductCode').val();
    var PolicyType = $('#ddlPolicyType option:selected').text();
    ValidateIndividualGrid(Productcode);
    var quoteGridDataIndividualFloat = [];
    quoteGridDataIndividualFloat = quoteGridDataIndividualFloatData;
    var gridid = "";
    var indexReference;
    gridid = "#divIndividualFloat";
    indexReference = quoteGridDataIndividualFloat;
    var jsonObj = [];
    GetgrdQuoteDetails(jsonObj, Productcode);
    if (ErrorCode == true) {
        $.ajax({
            url: "../Health/CalculatePremiumHealth",
            type: 'POST',
            data:
                JSON.stringify({
                    objHealth: $("#frmHealthQuickQuotes").serializeObject(),
                    objInsuredDetails: jsonObj,
                    ProductCode: Productcode
                }),

            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {


                $('#divSave').show();
                $('#btnCreatePolicy').hide();
                $('#btnBack').hide();
                $("#PlanandPremiumSummary").html(data);
            },
            fail: function () {
                $("#pnlPremiumBody").html("Data sumbission Failed. Try after some Time");
            }
        });
    }
}


function ValidateIndividualGrid(Productcode) {
    if (quoteGridDataIndividualFloatData.length < 1) {
        BootstrapDialog.alert("Please Fill atleast One Row");
        ErrorCode = false;
        return false;
    }
    ValidateCommonRowGrid(Productcode);
}
var RelationshipHSString = '';
var childCount;
var AdultCount;
function ValidateCommonRowGrid(Productcode) {
    var count = 0;
    var FirstName;
    var LastName;
    var FirstNameId;
    var LastNameId;
    var DateOfBirth;
    var DateOfBirthId;
    var Relationship;
    var RelationshipId;


    $("#divIndividualFloat tbody").find("tr").each(function (index) {
        index = index + 1;
        FirstName = $(this).find("[data-name=FirstName]").find("input[type=text]").val();
        FirstNameId = $(this).find("[data-name=FirstName]").find("input[type=text]").attr('id');
        LastName = $(this).find("[data-name=LastName]").find("input[type=text]").val();
        LastNameId = $(this).find("[data-name=LastName]").find("input[type=text]").attr('id');
        DateOfBirth = $(this).find("[data-name=DOB]").find("input[type=text]").val();
        DateOfBirthId = $(this).find("[data-name=DOB]").find("input[type=text]").attr('id');

        Relationship = $(this).find("[data-name=RelationshipWithProposer]").find("option:selected").val();
        RelationshipId = $(this).find("[data-name=RelationshipWithProposer]").find("select").attr('id');



        if (FirstName == "") {
            writeMultipleMessage('error', 'Please Enter FirstName in ' + index + ' Row', FirstNameId);
            ErrorCode = false;
            //return false;
        }
        if (LastName == "") {
            writeMultipleMessage('error', 'Please Enter LastName in ' + index + ' Row', LastNameId);
            ErrorCode = false;
            //return false;
        }
        if (DateOfBirth == "") {
            writeMultipleMessage('error', 'Please Enter DateOfBirth in ' + index + ' Row', DateOfBirthId);
            ErrorCode = false;
            //return false;
        }
        if (Relationship == "" || Relationship == "Select") {
            writeMultipleMessage('error', 'Please Select Relationship in ' + index + ' Row', RelationshipId);
            ErrorCode = false;
            //return false;
        }

    });


}
function ChkHCCSumInsured(SumInsuredValue, SumInsuredId) {
    if (($("#ddlSumInsured" + parseInt(SumInsuredId.replace('ddlSumInsured', '')) + " option:selected").text()) > 500000) {
        writeMultipleMessage('error', 'Agent Portal Will Not Process,If SumInsured MoreThan 500000', SumInsuredId);
        ErrorCode = false
        return false;
    }
    else {
        writeMultipleMessage('error', '', SumInsuredId);
        ErrorCode = true;
        return true;
    }
}
function ChkHSRelationships(RelationName, id, FloaterPlanType, DateOfBirth) {
    if (FloaterPlanType == "2 Adults") {
        if (RelationName == "Self" || RelationName == "Mother" || RelationName == "Father" || RelationName == "Spouse") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            // var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            //if (diffYear > 25) {
            //    writeMultipleMessage("error", "The Age of Self/Mother/Father is not below 25 Years ", id);
            //    ErrorCode = false;
            //    return false;

            //}

            if (IsIdMatch >= 0) {
                writeMultipleMessage("error", RelationName + " already exist. ", id);
                ErrorCode = false;
                return false;
            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                AdultCount = AdultCount + 1;
                writeMultipleMessage("error", "", id);
                return true;

            }
        }
        else if (RelationName == "Son" || RelationName == "Child" || RelationName == "Daughter") {
            writeMultipleMessage("error", "Please Select the Two Adults like Self and Spouse", id);
            ErrorCode = false;
            return false;

        }

    }
    else if (FloaterPlanType == "2 Adults + 1 Child") {
        if (RelationName == "Self" || RelationName == "Mother" || RelationName == "Father" || RelationName == "Spouse") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            //var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            //if (diffYear > 25) {
            //    writeMultipleMessage("error", "The Age of Self/Mother/Father is not below 25 Years ", id);
            //    ErrorCode = false;
            //    return false;

            //}
            if (AdultCount == 2) {
                writeMultipleMessage("error", "Per Quote Only 2 entries for Self and Spouse", id);
                ErrorCode = false;
                return false;

            }
            if (IsIdMatch >= 0) {
                writeMultipleMessage("error", RelationName + " already exist. ", id);
                ErrorCode = false;
                return false;
            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                AdultCount = AdultCount + 1;
                writeMultipleMessage("error", "", id);
                return true;

            }
        }
        else if (RelationName == "Son" || RelationName == "Child" || RelationName == "Daughter") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            // var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));
            if (diffYear > 25) {
                writeMultipleMessage("error", "The Age of Son/Child/Daughter is not below 25 Years ", 'DateOfBirth' + parseInt(id.replace('ddlRelationship', '')));
                ErrorCode = false;
                return false;

            }
            if (childCount == 1) {
                writeMultipleMessage("error", "Per Quote Only 1 entries for Son/Daughter/Child", id);
                ErrorCode = false;
                return false;

            }

            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                childCount = childCount + 1;
                writeMultipleMessage("error", "", id);
                strCount = true;
                return true;
            }
        }
    }
    else if (FloaterPlanType == "2 Adults + 2 Children") {
        if (RelationName == "Self" || RelationName == "Mother" || RelationName == "Father" || RelationName == "Spouse") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            //  var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            //if (diffYear > 25) {
            //    writeMultipleMessage("error", "The Age of Self/Mother/Father is not below 25 Years ", id);
            //    ErrorCode = false;
            //    return false;

            //}
            if (AdultCount == 2) {
                writeMultipleMessage("error", "Per Quote Only 2 entries for Self and Spouse", id);
                ErrorCode = false;
                return false;

            }
            if (IsIdMatch >= 0) {
                writeMultipleMessage("error", RelationName + " already exist. ", id);
                ErrorCode = false;
                return false;
            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                writeMultipleMessage("error", "", id);
                return true;

            }
        }
        else if (RelationName == "Son" || RelationName == "Child" || RelationName == "Daughter") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            // var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            if (diffYear > 25) {
                writeMultipleMessage("error", "The Age of Son/Child/Daughter is not below 25 Years ", 'DateOfBirth' + parseInt(id.replace('ddlRelationship', '')));
                ErrorCode = false;
                return false;

            }
            if (childCount == 2) {
                writeMultipleMessage("error", "Per Quote Only 2 entries for Son/Daughter/Child", id);
                ErrorCode = false;
                return false;

            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                writeMultipleMessage("error", "", id);
                childCount = childCount + 1;
                return true;
            }
        }
    }
    else if (FloaterPlanType == "1 Adult + 1 Child") {
        if (RelationName == "Self" || RelationName == "Mother" || RelationName == "Father" || RelationName == "Spouse") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            // var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            //if (diffYear > 25) {
            //    writeMultipleMessage("error", "The Age of Self/Mother/Father is not below 25 Years ", id);
            //    ErrorCode = false;
            //    return false;

            //}
            if (AdultCount == 1) {
                writeMultipleMessage("error", "Per Quote Only 1 entries for Self/Spouse", id);
                ErrorCode = false;
                return false;

            }
            if (IsIdMatch >= 0) {
                writeMultipleMessage("error", RelationName + " already exist. ", id);
                ErrorCode = false;
                return false;
            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                AdultCount = AdultCount + 1;
                writeMultipleMessage("error", "", id);
                return true;

            }
        }
        else if (RelationName == "Son" || RelationName == "Child" || RelationName == "Daughter") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            // var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            if (diffYear > 25) {
                writeMultipleMessage("error", "The Age of Son/Child/Daughter is not below 25 Years ", 'DateOfBirth' + parseInt(id.replace('ddlRelationship', '')));
                ErrorCode = false;
                return false;

            }
            if (childCount == 1) {
                writeMultipleMessage("error", "Per Quote Only 1 entries for Son/Daughter/Child", id);
                ErrorCode = false;
                return false;

            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                writeMultipleMessage("error", "", id);
                childCount = childCount + 1;
                return true;
            }



        }//1 Adult + 2 Children
    }
    else if (FloaterPlanType == "1 Adult + 2 Children") {
        if (RelationName == "Self" || RelationName == "Mother" || RelationName == "Father" || RelationName == "Spouse") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            // var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            //if (diffYear > 25) {
            //    writeMultipleMessage("error", "The Age of Self/Mother/Father is not below 25 Years ", id);
            //    ErrorCode = false;
            //    return false;

            //}
            if (AdultCount == 1) {
                writeMultipleMessage("error", "Per Quote Only 1 entries for Self/Spouse", id);
                ErrorCode = false;
                return false;

            }
            if (IsIdMatch >= 0) {
                writeMultipleMessage("error", RelationName + " already exist. ", id);
                ErrorCode = false;
                return false;
            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                AdultCount = AdultCount + 1;
                writeMultipleMessage("error", "", id);
                return true;

            }
        }
        else if (RelationName == "Son" || RelationName == "Child" || RelationName == "Daughter") {
            var IsIdMatch = $.inArray(RelationName, RelationshipHSString.split(','));
            // var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
            var dOB = DateOfBirth.split('/');
            var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
            var currentDate = new Date();
            var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

            if (diffYear > 25) {
                writeMultipleMessage("error", "The Age of Son/Child/Daughter is not below 25 Years ", 'DateOfBirth' + parseInt(id.replace('ddlRelationship', '')));
                ErrorCode = false;
                return false;

            }
            if (childCount == 2) {
                writeMultipleMessage("error", "Per Quote Only 2 entries for Son/Daughter/Child", id);
                ErrorCode = false;
                return false;

            }
            else {
                RelationshipHSString = RelationshipHSString + RelationName + ',';
                writeMultipleMessage("error", "", id);
                childCount = childCount + 1;
                return true;
            }

        }
        else if (RelationName == "Select" || RelationName == "") {
            writeMultipleMessage("error", "Please Select the Relatonship", id);
            ErrorCode = false;
            return false;


        }
    }
}
function ValidateFloaterFields(PolicyType, ProductCode) {
    if (ProductCode == "FGHSPFF" || ProductCode == "FGHSFF" || ProductCode == "FGHCCFF" || ProductCode == "FGHCFF") {

        if (ProductCode == "FGHSFF" || ProductCode == "FGHSPFF" || ProductCode == "FGHCCFF") {
            ValidateDropDown('ddlSumInsured', 'Please select SumInsured');
            if (ProductCode == "FGHCCFF") {
                var index = '';
                var SumInsuredValue = $('#ddlSumInsured option:selected').text();
                ChkHCCSumInsured(SumInsuredValue, index);
            }
        }
        if (ProductCode != "FGHCCFF") {
            ValidateDropDown('ddlPlan', 'Please select Plan');
        }
        //if (ProductCode == "FGHSPFF") {
        //    ValidateDropDown('ddlPlan', 'Please select Plan');
        //}
        if (ProductCode == "FGHSPFF") {
            //ValidateDropDown('ddlDeductible', 'Please select Deductible ');
        }
        if (ProductCode == "FGHSFF") {
            ValidateDropDown('ddlFloaterPlanType', 'Please select FloaterPlanType');
        }
        if (ProductCode == "FGHCFF") {
            //ValidateDropDown('ddlPerdayHospitalisation', 'Please select PerdayHospitalisation');
            ValidateDropDown('ddlFloaterPlanOption', 'Please select FloaterPlanOption');
        }

    }
}
function GetgrdQuoteDetails(jsonObj, Productcode) {

    var IsValid = true;
    var count = 0;
    var FirstName;
    var LastName;
    var DOB;
    var Relationship;
    var DOBId;

    RelationshipString = "";
    cnt = 0;
    $("#divIndividualFloat tbody").find("tr").each(function (index) {

        FirstName = $(this).find("[data-name=FirstName]").find("input[type=text]").val();
        LastName = $(this).find("[data-name=LastName]").find("input[type=text]").val();
        DOB = $(this).find("[data-name=DOB]").find("input[type=text]").val();
        DOBId = $(this).find("[data-name=DOB]").find("input[type=text]").attr('id');
        Relationship = $(this).find("[data-name=RelationshipWithProposer]").find("option:selected").val();
        RelationshipId = $(this).find("[data-name=RelationshipWithProposer]").find("select").attr('id');
        var RelationshipName = $(this).find("[data-name=RelationshipWithProposer]").find("option:selected").text();
        ChkRelationships(RelationshipName, RelationshipId, DOB, DOBId);

        if (FirstName != "" && LastName != "" && DOB != "" && Relationship != "") {

            jsonObj.push({ FirstName: FirstName, LastName: LastName, DOB: DOB, RelationshipWithProposer: Relationship });//, Plan: Plan, PlanOption: PlanOption, PerdayHospitalisation: PerdayHospitalisation, Deductible: Deductible });
        }


    });
}
var HCPlan = '';
var ISHCPlan = false;
function chkHCIPlan(Plan, id, index) {
    index = index + 1;
    if (ISHCPlan == false) {
        HCPlan = Plan;
        ISHCPlan = true;
        return true;
    }
    else if (HCPlan != Plan) {
        writeMultipleMessage("error", "Please select the " + HCPlan + " in the " + index + " row .Because Self/Spouse selected plan should be maintained acrros the Grid", id);
        ErrorCode = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
        return true;

    }
}
var HCPlanOption = '';
var ISHCPlanOption = false;
function chkHCIPlanOption(PlanOption, id, index) {
    index = index + 1;
    if (ISHCPlanOption == false) {
        HCPlanOption = PlanOption;
        ISHCPlanOption = true;
        return true;
    }
    else if (HCPlanOption != PlanOption) {
        writeMultipleMessage("error", "Please Select The " + HCPlanOption + " in the " + index + " row", id);
        ErrorCode = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
        return true;

    }
}
var Status = "";
var hdnDiscrepency = "";
function SaveAllHealthQuoteDetails() {

    ErrorCode = true;
    var IsConvertQuotetoPolicy = true;
    var jsonObj = [];
    var IsValid = true;
    var count = 0;
    var quoteGridDataIndividualFloat = [];
    if ($('#chkEmail').is(':checked') == true) {
        if ($('#txtEmail').val() == "") {
            writeMultipleMessage("error", "Please Enter Email Id", "txtEmail");
            return false;
        }
    }
    if ($('#chkSMS').is(':checked') == true) {
        if ($('#txtMobile').val() == "") {
            writeMultipleMessage("error", "Please Enter Mobile Number", "txtMobile");
            return false;
        }

    }
    quoteGridDataIndividualFloat = quoteGridDataIndividualFloatData;

    if (RelationshipString.split(',').indexOf('Self') == -1) {
        hdnDiscrepency = hdnDiscrepency + "Self is not Covered";
        $('#hdnDiscrepancy').val(hdnDiscrepency);
        Status = "Underwriting Referred";

    }
    else {
        Status = "Pending";
    }

    var Productcode = $('#hdnProductCode').val();

    GetgrdQuoteDetails(jsonObj, Productcode);
    $.ajax({

        type: "post",
        url: '/Health/SaveAllHealthProducts',
        type: 'POST',
        data:
            JSON.stringify({
                objHealth: $("#frmHealthQuickQuotes").serializeObject(),
                lstInsuredDetails: jsonObj,
                Status: Status,
                Underwriter: $("#hdnUnderwriterFlow").val()
            }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',

        success: function (data) {

            if (Status == "Underwriting Referred") {
                $('#btnCreatePolicy').hide();
                $('#btnCreatePolicy').hide();
                $('#btnBack').hide();
                BootstrapDialog.show({
                    type: BootstrapDialog.TYPE_SUCCESS,
                    title: 'Success',
                    closable: true,
                    size: 'size-large',
                    message: 'Quote no " ' + 'Q123456' + ' "is Successfully Saved.' + '<br />' + 'First Insured person is not Self, So this Quote Referred to Underwriter',
                    buttons: [{
                        label: 'OK',
                        action: function (dialogItself) {

                            window.location.href = "../../Agent/AgentHome";
                        }
                    }]
                });


            }
            else {
                $('#btnCreatePolicy').show();
                $('.modal-body').html('Quote no " ' + data.QuoteNO + ' "is Successfully Saved.' + '<br />' + 'Proceed to fill the proposal');
                $('#btnCreatePolicy').show();
                $('#btnBack').show();
            }
            debugger;
            $('#hdnPolicyID').val(data.PolicyID);
            $('#hdnQuoteNo').val(data.QuoteNO);
            $("#QuoteDialog").modal("show");



        }
    });
}
function ShowProposerPanels() {

    $('#frmHealthQuickQuotes').attr('action', 'CreatePolicyPanels');
    document.forms["frmHealthQuickQuotes"].submit();

}
function SetPolicyEndDate() {

    var result = validateTextBox('dpPolicyStartDate', 'Please select the PolicyStartDate');
    var StartDate = $("#dpPolicyStartDate").val();
    if (StartDate.indexOf('-') > -1) {
        StartDate = StartDate.replace(/-/g, "/");
    }
    if (result == false) {
        return false;
    }
    var date = new Date();
    var CurrentDate = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
    if (Date.parse(StartDate.toString()) < Date.parse(CurrentDate.toString())) {
        writeMultipleMessage("error", "PolicyStartDate Cannot be Prior to Todays Date", 'dpPolicyStartDate')
        return false;
    }

    $.ajax({
        url: "../Health/SetPolicyEndDate",
        type: "POST",
        cache: false,
        data: { PolicyStartDate: StartDate },
        success: function (data) {

            $("#dpPolicyEndDate").val(data);
        }
    });
}
function FamilyHistory(HealthMemberId) {
    BootstrapDialog.show({
        title: 'Family Health History',
        closable: true,
        size: 'size-large',
        message: function (dialog) {
            var $message = $('<div></div>');
            var pageToLoad = dialog.getData('pageToLoad');
            $message.load(pageToLoad);

            return $message;
        },
        data: {
            'pageToLoad': '../Health/FamilyHealthHistory?HealthMemberId=' + HealthMemberId
        }
    });

}
//var ChkLifestyleDetails = false;
function LifestyleDetails(HealthMemberId) {

    collectClickedYesLifestyle(HealthMemberId);
    collectAll();

    writeMultipleMessage('error', '', 'btnLifestyleDetails' + HealthMemberId);
    ChkIsViewQuote = $('#hdnIsViewQuote').val();
    var Gender = $('#ddlGender option:selected').val();

    BootstrapDialog.show({
        title: 'Lifestyle Questions',
        closable: false,
        size: 'size-large',
        message: function (dialog) {
            var $message = $('<div></div>');
            var pageToLoad = dialog.getData('pageToLoad');
            $message.load(pageToLoad);

            return $message;
        },
        data: {
            'pageToLoad': '../Health/FetchLifestyleQuestions?HealthMemberId=' + HealthMemberId + '&IsQuoteView=' + ChkIsViewQuote
        }
    });

}
//var ChkHealthQuestions = true;
function HealthQuestions(HealthMemberId) {

    collectClickedYes(HealthMemberId);
    collectAll();
    writeMultipleMessage("error", "", "btnHealthQuestions" + HealthMemberId);
    ChkIsViewQuote = $('#hdnIsViewQuote').val();
    var Gender = $('#ddlGender' + HealthMemberId + ' option:selected').val();
    $.ajax({
        url: '../Health/FetchAllHealthQuestions?HealthMemberId=' + HealthMemberId + "&Gender=" + Gender + '&IsQuoteView=' + ChkIsViewQuote,
        type: 'POST',
        dataType: 'html',
        async: false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            BootstrapDialog.show({
                title: 'Health Questions',
                closable: true,
                draggable: true,
                style: "z-index:10050, width:150%",
                size: 'size-large',
                //onshow: function (dialog) {
                //    $('.hideParent').parent().parent().hide();
                //},
                message: function (dialog) {
                    var $message = $('<div> </div>');
                    $message.append(data);
                    return $message;
                },

            });
        }

    });
}

var HealthQuestionsGridData = [];
var NonSTPQns = "";
function ChkUWProposalQns(QId, id) {

    if (QId == 34 || QId == 35 || QId == 36 || QId == 43 || QId == 44 || QId == 45 || QId == 46 || QId == 47 || QId == 58) {
        // if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
        // NonSTPQns=NonSTPQns+"Are you presently in good health and fully functioning with work home and school life and entirely free from any physical and mental impairments or deformities. ^"
        // $('#QuestionsPopUp').html('We are  unable to process the proposal. Kindly get in touch with our nearest local branch office. ');
        //// + data.QuoteNo + ' "is Successfully Saved.' + '<br />' + 'Proceed to fill the proposal');
        // }
        if (QId == 34 || QId == 38) {
            if ($('#rdbtnAnswerTextNo' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "No.Are you presently in good health and fully functioning with work home and school life and entirely free from any physical and mental impairments or deformities. ^";
                return false;
            }
        }
        else if (QId == 35) {
            if ($('#rdbtnAnswerTextNo' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "No.Are  you suffering from any physical deformity, or suffer / suffered from any illness / Accidental injury/ Underwent any surgery/ Mental Disorder/ Weight loss/ in the present or past. ^";
                return false;
            }
        }
        else if (QId == 36) {
            if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "Yes.Have you ever suffered /are you suffering from Gynecological problems, underwent Abortion, Caesarean Section for delivery, breast disorders?.^";
                return false;
            }
        }
        else if (QId == 43) {
            if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "Yes.Diabetes Mellitus ^";
                return false;
            }
        }
        else if (QId == 44) {
            if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "Yes.High Blood Pressure, Heart Disease including Ischemic Heart Disease, Rheumatic Heart Disease. ^";
                return false;
            }
        }
        else if (QId == 45) {
            if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "Yes.Chest pain stroke, Asthma, Any respiratory conditions, Cancer , Tumor or lump of any kind,Hepastitis, disorder of kidneys, blood disorder, any mental or psychiatric conditions, any disease of the brain or nervous system, fits(epilepsy, slipped disc, back ache, any congenital birth defects/disease, AIDS or tested Positive for HIV. ^";
                return false;
            }
        }
        else if (QId == 46) {
            if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "Yes. received treatment/medication due to medical conditions ^";
                return false;
            }
        }
        else if (QId == 47) {
            if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "Yes.other diseases or ailments not mentioned above ^";
                return false;
            }
        }
        else if (QId == 58) {
            if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
                NonSTPQns = NonSTPQns + "Yes.Has any proposal for life, critical illness or health related insurance on your life ever been postponed, declined or accepted on special terms ^";
                return false;
            }
        }
    }
    else if (QId == 37 || QId == 48 || QId == 54 || QId == 56 || QId == 57) {
        if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
            BootstrapDialog.alert("Coverage cannot be provided for female customers who are pregnant - they can apply for insurance 6 months after the delivery.");
            return false;
        }
    }
    else if (QId == 38) {
        if ($('#rdbtnAnswerTextNo' + id).is(':checked') == true) {
            BootstrapDialog.alert(" We are unable to process the proposal. Kindly get in touch with our nearest local Branch office .because <br/> No,Are you in good health and free from any physical and mental disease or infirmity or medical complaints of deformity?");

            return false;
        }
    }
    else if (QId == 41 || QId == 49 || QId == 30 || QId == 31 || QId == 32 || QId == 33) {
        if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
            if (QId == 30) {
                hdnDiscrepency = hdnDiscrepency + "occupation associated with any specific hazard ,"; //refer ^ for spliting string.
                return false;
            }
            else if (QId == 31) {
                hdnDiscrepency = hdnDiscrepency + "employed in armed/paramilitary/Police forces ,";
                return false;

            }
            else if (QId == 32) {
                hdnDiscrepency = hdnDiscrepency + "have hobbies/take part in activities that could be dangerous in any way ,";
                return false;

            }
            else if (QId == 33) {
                hdnDiscrepency = hdnDiscrepency + "have consumed Alcohol/tobacco or narcotic ,";
                return false;

            }
            else if (QId == 41) {
                hdnDiscrepency = hdnDiscrepency + "consume/smoke Tobacco/Alcohol ,";
                return false;
            }
            else if (QId == 49) {
                hdnDiscrepency = hdnDiscrepency + "regular smoker ,";
                return false;
            }
            Status = "Underwriting Referred";
        }
    }
}
var LifestyleQuestionsGridData = [];
var SubstanceGridData = [];
function SaveLifestyleQuestions() {
    var jsonObj = [];
    var IsValid = true;
    var count = 0;
    var quoteData = LifestyleQuestionsGridData;
    for (var index = 0; index < quoteData.length; index++) {
        var id = quoteData[index].Index;
        var HealthMemberId = $('#hdnHealthMemberId').val();
        var QId = $('#hdnQId' + id).val();
        ChkUWProposalQns(QId, id);
        //if(NonSTPQns!=""||NonSTPQns!=null)
        //{
        //    var ReasonsString='';
        //    var Discrepancies=[];
        //    Discrepancies=NonSTPQns.split('^');
        //    j=0;
        //    for(var i=0; Discrepancies.length>i;i++)
        //    {

        //        ReasonsString=ReasonsString+ j++ +')'+Discrepancies[i];
        //    }
        //    BootstrapDialog.show({
        //        type: BootstrapDialog.TYPE_DANGER,
        //        title: 'Note',
        //        closable: true,
        //        size: 'size-large',
        //        message: "We are unable to process this proposal, kindly get in touch with the nearest local branch  </br>  due to following reasons" +ReasonsString,
        //        buttons: [{
        //            label: 'OK',
        //            action: function (dialogItself) {
        //                dialogItself.close();
        //            }
        //        }]
        //    });

        //    return false;
        //}
        var AnswerTextYes = $('#rdbtnAnswerTextYes' + id).is(':checked');
        var AnswerTextNo = $('#rdbtnAnswerTextNo' + id).is(':checked');
        if (AnswerTextYes == true) {
            AnswerTextYes = $('#rdbtnAnswerTextYes' + id).val();
            jsonObj.push({ HealthMemberId: HealthMemberId, QId: QId, AnswerType: AnswerTextYes });
        }
        else {
            AnswerTextNo = $('#rdbtnAnswerTextNo' + id).val();
            jsonObj.push({ HealthMemberId: HealthMemberId, QId: QId, AnswerType: AnswerTextNo });

        }
        count++;

    }
    var jsonSubstanceObj = [];

    GetSubstanceData(jsonSubstanceObj);
    $.ajax({

        url: '/Health/SaveLifestyleQuestions',
        type: 'POST',
        data:
            JSON.stringify({
                lstLifestyleQuestions: jsonObj,
                lstSubstances: jsonSubstanceObj,
                Status: Status
            }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',

        success: function (data) {
            BootstrapDialog.alert("Lifestyle Questions Answered Successfully");
            $('#btnClose').click();
        },
        fail: function (data) {
            BootstrapDialog.alert("Lifestyle Questions Not Answered Successfully ");
        }
    });
}
function GetSubstanceData(jsonSubstanceObj) {
    var Substance;
    var QuantityPerday;
    var NoOfYears;
    $("#divChildGridLifestyleDetail tbody").find("tr").each(function (index) {
        Substance = $(this).find("[data-name=Substance]").find("option:selected").val();
        QuantityPerday = $(this).find("[data-name=QuantityPerday]").find("input[type=text]").val();
        NoOfYears = $(this).find("[data-name=NoOfYears]").find("input[type=text]").val();

        if (Substance != "" && QuantityPerday != "" && NoOfYears != "") {
            jsonSubstanceObj.push({ Substance: Substance, QuantityPerday: QuantityPerday, NoOfYears: NoOfYears });
        }

    })
}


var hdnQuestionsDiscrepency = '';
function SaveHealthQuestions() {

    hdnDiscrepency = '';
    ErrorCode = true;
    NonSTPQns = "";
    // var HealthQuestionsGridData=[];
    var jsonObj = [];
    var IsValid = true;
    var count = 0;
    var HealthQuestionsData;

    ValidateQnsAns();
    if (ErrorCode == false) {
        return false;
    }
    HealthQuestionsData = HealthQuestionsGridData;
    for (var index = 0; index < HealthQuestionsData.length; index++) {
        var id = HealthQuestionsData[index].Index;
        var HealthMemberId = $('#hdnHealthMemberId').val();
        var QId = $('#hdnQId' + id).val();
        var QText = $('[data-name="QText"]').val();

        ChkUWProposalQns(QId, id);

        var AnswerTextYes = "";
        if ($('#rdbtnAnswerTextYes' + id).is(':checked') == true) {
            AnswerTextYes = $('#rdbtnAnswerTextYes' + id).val();
        }
        else if ($('#rdbtnAnswerTextNo' + id).is(':checked') == true) {
            AnswerTextYes = $('#rdbtnAnswerTextNo' + id).val();
        }
        else if ($('#txtAnswerTextYes' + id).val() != "" && $('#txtAnswerTextYes' + id).val() != null) {
            AnswerTextYes = $('#txtAnswerTextYes' + id).val();
        }
        else if ($('#dpAnswerText' + id).val() != "" && $('#dpAnswerText' + id).val() != null) {
            AnswerTextYes = $('#dpAnswerText' + id).val();
        }
        jsonObj.push({ HealthMemberId: HealthMemberId, QId: QId, AnswerType: AnswerTextYes });
        count++;
    }

    if (hdnDiscrepency != '' || hdnDiscrepency != null) {
        $('#hdnDiscrepancy').val(hdnDiscrepency);
        Status = "Underwriting Referred";
    }

    $.ajax({
        url: '/Health/SaveHealthQuestions',
        type: 'POST',
        data:
            JSON.stringify({
                lstHealthQuestions: jsonObj,
                Status: Status
            }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',

        success: function (data) {
            if ((hdnDiscrepency != null && hdnDiscrepency != "") || NonSTPQns != "" && NonSTPQns != undefined) {
                if (hdnDiscrepency != null && hdnDiscrepency != "") {
                    var ReasonsString = '';
                    var Discrepancies = [];
                    Discrepancies = hdnDiscrepency.split(',');
                    j = 1;
                    for (var i = 0; Discrepancies.length > i; i++) {
                        ReasonsString = ReasonsString + j + '. ' + Discrepancies[i];
                        j++;
                    }
                    BootstrapDialog.show({
                        type: BootstrapDialog.TYPE_DANGER,
                        title: 'Information',
                        closable: true,
                        size: 'size-large',
                        message: "The Proposal is refered to underwriter due to following reasons  </br>" + ReasonsString,
                        buttons: [{
                            label: 'OK',
                            action: function (dialogItself) {
                                dialogItself.close();

                            }
                        }]
                    });
                    $('#btnClose').click();
                }
                if (NonSTPQns != "" && NonSTPQns != undefined) {
                    var ReasonsString = '';
                    var Discrepancies = [];
                    Discrepancies = NonSTPQns.split('^');
                    j = 1;
                    for (var i = 0; Discrepancies.length > i; i++) {
                        ReasonsString = ReasonsString + j + '. ' + Discrepancies[i];
                        j++;
                    }
                    BootstrapDialog.show({
                        type: BootstrapDialog.TYPE_DANGER,
                        title: 'Information',
                        closable: true,
                        size: 'size-large',
                        message: "We are unable to process this proposal, kindly get in touch with the nearest local branch due to following reasons </br>" + ReasonsString,
                        buttons: [{
                            label: 'OK',
                            action: function (dialogItself) {
                                window.location.href = "../../Agent/AgentHome";
                            }
                        }]
                    });
                    $('#btnClose').click();
                    $('#btnSave').hide();

                    return false;
                }
            }
            else {
                BootstrapDialog.alert("Health Questions Answered Successfully");
                $('#btnClose').click();
                $('#btnSave').show();
            }

            //   $('#divHealthQuestionsPOPUp').reset();
        },
        fail: function (data) {
            BootstrapDialog.alert("Health Questions Not Answered Successfully ");
        }
    });
}

function ValidateQnsAns()      //yes, no ,qtext
{
    var id1;
    var id2;
    $("#divHealthQns tbody").find("tr").each(function (index) {

        var QText = $(this).find("[data-name=QText]").text();
        var QId = $(this).find("[data-name=QId]").find("[type=hidden]").val();
        var AnswerType = $(this).find("[data-name=AnswerType]").find("[type=hidden]").val();
        if (AnswerType == "Option") {
            id1 = $(this).find("[data-name=AnswerText]").find("[id=rdbtnAnswerTextYes" + QId + "]").is(':checked');
            id2 = $(this).find("[data-name=AnswerText]").find("[id=rdbtnAnswerTextNo" + QId + "]").is(':checked');
            if (((id1 == false) && (id2 == false)) && QId != "64") {
                writeMultipleMessage("error", "Please Check/uncheck Question is " + QText, 'rdbtnAnswerTextYes' + QId);
                ErrorCode = false;
                // return false;
            }
            else {
                writeMultipleMessage("error", "", 'rdbtnAnswerTextYes' + QId);
                // return true;
            }
            if (QId == "64") {
                if (((id1 == false) && (id2 == false)) && $('#rdbtnAnswerTextYes47').is(':checked') == true) {
                    writeMultipleMessage("error", "Please Check/uncheck Question is " + QText, 'rdbtnAnswerTextYes' + QId);
                    ErrorCode = false;
                    // return false;
                }
                else {
                    writeMultipleMessage("error", "", 'rdbtnAnswerTextYes' + QId);
                    // return true;
                }
            }

        }
        else if (AnswerType == "TextBox") {
            id1 = $(this).find("[data-name=AnswerText]").find("[type=text]").val();
            if ($('#rdbtnAnswerTextYes50').is(':checked') == true || $('#rdbtnAnswerTextYes41').is(':checked') == true || $('#rdbtnAnswerTextYes47').is(':checked') == true || $('#rdbtnAnswerTextYes55').is(':checked') == true || $('#rdbtnAnswerTextYes56').is(':checked') == true || $('#rdbtnAnswerTextYes57').is(':checked') == true || $('#rdbtnAnswerTextYes58').is(':checked') == true) {
                if (($('#rdbtnAnswerTextYes41').is(':checked') == true && (QId == "59" || QId == "60")) || ($('#rdbtnAnswerTextYes47').is(':checked') == true && (QId == "61" || QId == "62")) || ($('#rdbtnAnswerTextYes55').is(':checked') == true && (QId == "66")) || ($('#rdbtnAnswerTextYes56').is(':checked') == true && (QId == "67"))
                    || ($('#rdbtnAnswerTextYes57').is(':checked') == true && (QId == "68" || QId == "69")) || ($('#rdbtnAnswerTextYes57').is(':checked') == true && (QId == "71"))) {  //order of subquestions is  health suraksha,surplus 
                    if (id1 == "") {
                        writeMultipleMessage("error", "Please Fill the Question is " + QText, 'txtAnswerTextYes' + QId);
                        ErrorCode = false;
                        // return false;
                    }
                    else {
                        writeMultipleMessage("error", "", 'txtAnswerTextYes' + QId);
                        // return true;
                    }
                }
            }
            else {
                //if (QId == "51" || QId == "53" ) {
                //    if (id1 == "") {
                //        writeMultipleMessage("error", "Please Fill the Question is " + QText, 'txtAnswerTextYes' + QId);
                //        ErrorCode = false;
                //        // return false;
                //    }
                //    else {
                //        writeMultipleMessage("error", "", 'txtAnswerTextYes' + QId);
                //        // return true;
                //    }
                //  }
            }

        }
        else if (AnswerType == "DatePicker") {
            id1 = $(this).find("[data-name=AnswerText]").find("[type=text]").val();
            if (($('#rdbtnAnswerTextYes47').is(':checked') == true && QId == "63") || ($('#rdbtnAnswerTextYes57').is(':checked') == true && (QId == "70"))) {
                if (id1 == "") {
                    writeMultipleMessage("error", "Please Fill the Question is " + QText, 'dpAnswerTextYes' + QId);
                    ErrorCode = false;
                    // return false;
                }
                else {
                    writeMultipleMessage("error", "", 'dpAnswerTextYes' + QId);
                    // return true;
                }
            }
        }

    });
}

function showChilds(child) {

    if (child != null) {
        var array = child.split(';');
        var Qset = array[1];
        $('.' + Qset).parent().parent().show();

    }
    else
        return false;
}
function hideChilds(child) {
    if (child != null) {
        var array = child.split(';');
        var Qset = array[1];
        $('.' + Qset).parent().parent().hide();

    }
    else
        return false;
}
// show and hide div family doctor details
function EnableChildGrid(SeqNo) {
    if (SeqNo == '9' || SeqNo == '1') {
    }
}
function DisableChildGrid(SeqNo) {
    if (SeqNo == '9' || SeqNo == '1') {
    }
}
function EnableButton(HealthMemberId) {
    writeMultipleMessage('error', '', 'rdnYesOtherHealthPolicies' + HealthMemberId);

    $('#divShowOtherHealthPolicies' + HealthMemberId).show();

}
function DisableButton(HealthMemberId) {
    writeMultipleMessage('error', '', 'rdnYesOtherHealthPolicies' + HealthMemberId);

    $('#divShowOtherHealthPolicies' + HealthMemberId).hide();
}
//global variable to check this button s clicked or not
var chkOtherHealthPolicy = false;
function EnterOtherHealthDetails(HealthMemberId) {

    collectClickedYesOtherHealth(HealthMemberId);
    collectAll();
    ChkIsViewQuote = $('#hdnIsViewQuote').val();
    writeMultipleMessage("error", "", "rdnYesOtherHealthPolicies" + HealthMemberId);
    $.ajax({
        url: '../Health/OtherPolicyHealthInsuranceInfo?HealthMemberId=' + HealthMemberId + '&IsQuoteView=' + ChkIsViewQuote,
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            BootstrapDialog.show({
                title: 'Other Health Details',
                closable: false,
                message: function (dialog) {
                    var $message = $('<div id="divModel"></div>');
                    $message.append(data);
                    return $message;
                },

            });
        }

    });
}

function checkEnableAndDisableControls(ProductCode) {

    if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF") {
        $('#divPolicyType').show();
        $('#divFloaterSumInsured').show();

    }
    else if (ProductCode == "FGHSFF" || ProductCode == "FGHSI") {
        $('#divPolicyType').show();
        $('#divFloaterSumInsured').show();
        $('#divPlanFloaterPolicies').show();
        $('#divFloaterPlanType').show();

    }
    else if (ProductCode == "FGHCFF" || ProductCode == "FGHCI") {

        $('#divPlanFloaterPolicies').show();
        $('#divPlanOption').show();
    }
    else if (ProductCode == "FGHSPFF" || ProductCode == "FGHSPI") {
        $('#divPolicyType').show();
        if (ProductCode == "FGHSPFF") {
            $('#divFloaterSumInsured').show();
            $('#divPlanFloaterPolicies').removeAttr("style");
        }

    }


}
function FamilySufferWithill() {

    writeMultipleMessage('error', '', 'rbYesAnyFamilyMemSufferWithIll');

    if ($('#rbYesAnyFamilyMemSufferWithIll').is(':checked') == true) {
        $('#divRelationshipAilment').show();


    }
    else if ($('#rbNoAnyFamilyMemSufferWithIll').is(':checked') == true) {
        $('#divRelationshipAilment').hide();

    }



}
function CheckOtherExistingHSPPolicy() {

    if ($('#rbYesIsOtherExistingHSPPolicy').is(':checked') == true) {
        $('#divHSPExistingPolicy').show();
        writeMultipleMessage("error", "", "rbYesIsOtherExistingHSPPolicy");
        return true;
    }
    else if ($('#rbNoIsOtherExistingHSPPolicy').is(':checked') == true) {
        $('#divHSPExistingPolicy').hide();
        writeMultipleMessage("error", "", "rbYesIsOtherExistingHSPPolicy");
        return true;

    }
}
function CheckOtherExistingHSPolicy() {


    if ($('#rbYesIsOtherExistingHSPolicy').is(':checked') == true) {
        $('#divHSExistingPolicy').show();
        $('#divHSExistingCustomerNo').show();
        writeMultipleMessage("error", "", "rbYesIsOtherExistingHSPolicy");
        return true;
    }
    else if ($('#rbNoIsOtherExistingHSPolicy').is(':checked') == true) {
        $('#divHSExistingPolicy').hide();
        $('#divHSExistingCustomerNo').hide();
        writeMultipleMessage("error", "", "rbYesIsOtherExistingHSPolicy");
        return true;
    }
}
function CheckOtherExistingHCPolicy() {
    if ($('#rbYesIsOtherExistingHCPolicy').is(':checked') == true) {
        $('#divHCExistingPolicy').show();
        $('#divHCExistingCustomerNo').show();
        writeMultipleMessage("error", "", "rbYesIsOtherExistingHCPolicy");
        return true;
    }
    else if ($('#rbNoIsOtherExistingHCPolicy').is(':checked') == true) {
        $('#divHCExistingPolicy').hide();
        $('#divHCExistingCustomerNo').hide();
        writeMultipleMessage("error", "", "rbYesIsOtherExistingHCPolicy");
        return true;
    }
}

function CheckIsAllFamilyMembersCovered() {

    writeMultipleMessage('error', '', 'rbYesIsAllFamilyMembersCovered');

    if ($('#rbYesIsAllFamilyMembersCovered').is(':checked') == true) {
        BootstrapDialog.alert("System to Proceed");
        writeMultipleMessage('error', '', 'rbYesIsAllFamilyMembersCovered');
        return true;
    }
    else if ($('#rbNoIsAllFamilyMembersCovered').is(':checked') == true) {
        writeMultipleMessage('error', '', 'rbYesIsAllFamilyMembersCovered');
        Status = "Underwriting Referred";
        BootstrapDialog.alert("All the family members have to be covered Selection of members is not allowed. and it is to be referred to Retail UW, However the process must continue");
        return true;
    }
}
function GetPaymentView() {
    $('#currencyGrossPremium').removeAttr("disabled");
    $('#pnlPayment').show();
    $.post("../Health/Payment", $("#frmAllHealthProducts").serialize())
       .done(function (data) {
           $("#pnlProposerDetails").toggle();
           $("#pnlDetailsOfInsured").toggle();
           $("#pnlDocumentUpload").toggle();
           $("#btnPayment").hide();
           $("#pnlPaymentBody").html(data);
           $("#btnSave").hide();
           $('#currencyGrossPremium').attr("disabled", "disabled");
       }).
       fail(function () {
           $("#pnlPaymentBody").html("Data sumbission Failed. Try after some Time");
           $('#currencyGrossPremium').attr("disabled", "disabled");
       });


}
var InsuredDetailsGridData = [];
var ErrorCode = false;
var chkbtnModifyUwClick = false;
function CalCulatePremiumHome() {
    debugger;
    ErrorCode = true;
    var ProductCode = $('#hdnProductCode').val();
    // ValidateInsuredProposerDetails();
    var result = SetPolicyEndDate();
    if (result == false) {
        return false;
    }

    var quoteData;

    if (ErrorCode == true) {

        var quoteData = InsuredDetailsGridData;
        var jsonObj = [];
        var IsValid = true;
        var count = 0;
        for (var index = 0; index < quoteData.length; index++) {
            var id = quoteData[index].Index;
            var HealthMemberId = $('#hdnHealthMemberId' + id).val();
            var Gender = $('#ddlGender' + id).val();
            var RelationshipWithProposer = $('#hdnRelationshipWithProposerId' + id).val();
            var Height = $('#txtHeight' + id).val();
            var Weight = $('#txtWeight' + id).val();
            var NomineeName = $('#txtNomineeName' + id).val();
            var NomineeRelationshipId = $('#ddlRelationshipNomineeWithInsured' + id).val();
            var DateOfBirth = $('#DateOfBirth' + count).val();
            var Plan = $('#ddlPlan' + count).val();
            var IsOtherPoliciesExist = null;
            if (ProductCode != "FGHCI") {
                if (ProductCode != "FGHCFF") {
                    if ($('#rdnYesOtherHealthPolicies' + id).is(':checked') == true) {
                        IsOtherPoliciesExist = true;
                    }
                    else {
                        IsOtherPoliciesExist = false;
                    }
                }
            }
            var NationalityId = $('#ddlNationality' + id).val();
            jsonObj.push({ HealthMemberId: HealthMemberId, Gender: Gender, DateOfBirth: DateOfBirth, Plan: Plan, RelationshipWithProposer: RelationshipWithProposer, Height: Height, Weight: Weight, Nominee_Name: NomineeName, RelationshipNomineeWithInsured: NomineeRelationshipId, Nationality: NationalityId, IsOtherHealthPoliciesExist: IsOtherPoliciesExist });
            count++;
        }
        debugger;
        EnableDisableAddress('ObjCommunicationAddress', false);
        var ConvertQuotePolicy = true;

        $.ajax({
            url: "../Health/CalculatePremiumHealth",
            type: 'POST',
            data:
                JSON.stringify({
                    objHealth: $("#frmAllHealthProducts").serializeObject(),
                    objInsuredDetails: jsonObj,
                    ProductCode: ProductCode
                }),
            dataType: 'html',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#PlanandPremiumSummary').html(data);
                $('#pnlDocumentUpload').show();
                $('#btnSave').show();
            },
            fail: function () {
                $("#pnlPremiumBody").html("Data sumbission Failed. Try after some Time");
            }
        });

    }

    else {
        return false;
    }

}

function SaveProposerInsuredDetails(e, formId) {
    debugger;
    if ($('#hdnUnderwriterFlow').val() == 'True') {
        var result = ValidateDropDown('ddlUWDecision', 'Please Select the UnderWriterDecision');
        if (result == false)
            return false;
        result = CheckForMandatoryFields('txtUWRemarks', 'Please Enter the Underwriter Remarks');
        if (result == false)
            return false;
    }
    if (chkbtnModifyUwClick != true && $('#hdnUnderwriterFlow').val() == 'True') {
        var Decision = $('#ddlUWDecision option:selected').text();
        if ($('#hdnUnderwriterFlow').val() == 'True') {

            $.post("../Health/SaveUnderwriterRemarksForCorporate", $("#" + formId).serialize(), function (data) {
                BootstrapDialog.show({
                    type: BootstrapDialog.TYPE_SUCCESS,
                    title: 'Success',
                    closable: true,
                    size: 'size-large',
                    message: "Proposal refered to UnderWriter has " + Decision,
                    buttons: [{
                        label: 'OK',
                        action: function (dialogItself) {
                            window.location.href = "../../Policy/UnderwriterInbox";
                        }
                    }]
                });
                // BootstrapDialog.alert("" + Decision);
                // window.location.href = "../../Policy/UnderwriterInbox";
                return true;
            }, "json");
            return true;
        }
        return true;
    }

    ErrorCode = true;
    DocumentNameString = '';

    var ProductCode = $('#hdnProductCode').val();
    if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF") {
        chkNonSTPProposal();
    }
    ValidateInsuredProposerDetails();
    var result = SetPolicyEndDate();
    if (result == false) {
        return false;
    }
    if ($('#hdnUnderwriterFlow').val() != 'True') {
        var result = ValidateDocumentUpload();

        if (result == false || ErrorCode == false) {
            return false;
        }
    }
    $('#hdnDiscrepancy').val(hdnDiscrepency);  //captures all uw referal reason
    if (Status != "Underwriting Referred" || Status == "Pending") {
        Status = "Converted";
    }
    var quoteData;

    if (ErrorCode == true) {
        if ($('#hdnUnderwriterFlow').val() != 'True') {
            $('#btnUploadFiles').click();
        }
        var quoteData = InsuredDetailsGridData;
        var jsonObj = [];
        var IsValid = true;
        var count = 0;
        debugger;
        for (var index = 0; index < quoteData.length; index++) {
            var id = quoteData[index].Index;
            var HealthMemberId = $('#hdnHealthMemberId' + id).val();
            var Gender = $('#ddlGender' + id).val();
            var RelationshipWithProposer = $('#hdnRelationshipWithProposerId' + id).val();
            //var Height = $('#txtHeight' + id).val();
            //var Weight = $('#txtWeight' + id).val();
            var NomineeName = $('#txtNomineeName' + id).val();
            var NomineeRelationshipId = $('#ddlRelationshipNomineeWithInsured' + id).val();
            var DateOfBirth = $('#DateOfBirth' + count).val();

            jsonObj.push({ HealthMemberId: HealthMemberId, Gender: Gender, DOB: DateOfBirth, Plan: Plan, RelationshipWithProposer: RelationshipWithProposer, Height: Height, Weight: Weight, Nominee_Name: NomineeName, RelationshipNomineeWithInsured: NomineeRelationshipId, Nationality: NationalityId, IsOtherHealthPoliciesExist: IsOtherPoliciesExist });
            count++;
        }
        EnableDisableAddress('ObjCommunicationAddress', false);
        var ConvertQuotePolicy = true;
        debugger;
        $.post("../../Health/SaveAllHealthProductsWithDoc?strInsuredDetails=" + JSON.stringify(jsonObj) +
                 '&ConvertQuotePolicy=' + ConvertQuotePolicy + '&Status=' + Status, $("#frmAllHealthProducts").serialize(), function (data) {
                     debugger;
                     if ((Status == "Underwriting Referred") && (hdnDiscrepency != null && hdnDiscrepency != "")) {

                         BootstrapDialog.show({
                             type: BootstrapDialog.TYPE_SUCCESS,
                             title: 'Success',
                             closable: true,
                             size: 'size-large',
                             message: "Data Saved Successfully and Proposal Referred to UnderWriter",
                             buttons: [{
                                 label: 'OK',
                                 action: function (dialogItself) {
                                     window.location.href = "../../Home/Index";
                                 }
                             }]
                         });
                         // BootstrapDialog.alert("Data Saved Successfully and Proposal Referred to UnderWriter due to following Reasons <br/>" + hdnDiscrepency);
                     }
                     else {
                         if ($('#hdnUnderwriterFlow').val() == 'True') {
                             var Decision = $('#ddlUWDecision option:selected').text();
                             BootstrapDialog.show({
                                 type: BootstrapDialog.TYPE_SUCCESS,
                                 title: 'Success',
                                 closable: true,
                                 size: 'size-large',
                                 message: "Proposal refered to UnderWriter has " + Decision,
                                 buttons: [{
                                     label: 'OK',
                                     action: function (dialogItself) {
                                         window.location.href = "../../Policy/UnderwriterInbox";
                                     }
                                 }]
                             });
                         }
                         else {
                             $("#btnPayment").show();
                             BootstrapDialog.alert("Data Saved Successfully");
                         }

                     }

                     return true;
                 }, "json");
    }
    else {
        return false;
    }
    EnableDisableAddress('ObjCommunicationAddress', true);
}
function SaveUWData() {
    $('#divSave').show();
    $('#btnSave').click();
    $('#divSave').hide();
}
function EnableDisableAddress(ObjCommunication, EnableOrDisable) {
    $('#' + ObjCommunication + 'City').prop('readonly', EnableOrDisable);
    $('#' + ObjCommunication + 'City').prop('disabled', EnableOrDisable);
    $('#' + ObjCommunication + 'District').prop('readonly', EnableOrDisable);
    $('#' + ObjCommunication + 'District').prop('disabled', EnableOrDisable);
    $('#' + ObjCommunication + 'State').prop('readonly', EnableOrDisable);
    $('#' + ObjCommunication + 'State').prop('disabled', EnableOrDisable);
    $('#' + ObjCommunication + 'Country').prop('readonly', EnableOrDisable);
    $('#' + ObjCommunication + 'Country').prop('disabled', EnableOrDisable);
}


function chkNonSTPProposal() {
    if ($('#rdnNoAgeProofSubmitted').is(':checked') == true) {
        return false;
    }

}
function chkUWProposal() {
    if ($('#rdnYesAnyFamilyMemExpired').is(':checked') == true) {
        Status = "Underwriting Referred";
        hdnDiscrepency = hdnDiscrepency + "Yes Any Family Member got expired below 60";
        return false;
    }
    if ($('#rbYesAnyFamilyMemSufferWithIll').is(':checked') == true) {
        Status = "Underwriting Referred";
        hdnDiscrepency = hdnDiscrepency + "Yes Any Family Member Suffer with illness";
        return false;
    }
}
function SaveOtherHealthDetails() {

    ErrorCode = true;
    var ProductCode = $('#hdnProductCode').val();
    CheckForMandatoryFields('txtPolicyNumber', 'Please Enter the PolicyNumber');
    CheckForMandatoryFields('txtSumInsured', 'Please Enter the SumInsured');
    ValidateProposerDropDown('ddlInsuranceCompanyName', "Please Select Insurance Company Name");
    if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF") {
        ValidateRadiobutton('rdnYesMedicalTestConducted', 'rdnNoMedicalTestConducted', 'Please check/uncheck the IsMedicalTestConducted');
        ValidateProposerDropDown('ddlDecision', "Please Select Decision");
        ValidateProposerDropDown('ddlPolicyStatus', "Please Select PolicyStatus");
    }
    else if (ProductCode == "FGHSPI" || ProductCode == "FGHSPFF" || ProductCode == "FGHSI" || ProductCode == "FGHSFF") {
        ValidateInsurateDate('dpInsuranceFromDate', 'Please enter the InsuranceFromDate');
        ValidateInsurateDate('dpInsuranceToDate', 'Please enter the InsuranceToDate');
        if (ProductCode == "FGHSPI" || ProductCode == "FGHSPFF") {
            CheckForMandatoryFields('txtCBAmount', 'Please Enter the CBAmount');
            CheckForMandatoryFields('txtCBPercentage', 'Please Enter the CBPercentage');
        }
        ValidateRadiobutton('rdnYesIsClaimsReceived', 'rdnNoIsClaimsReceived', 'Please check/uncheck the IsClaimsReceived');
        if ($('#rdnYesIsClaimsReceived').is(":checked") == true) {
            CheckForMandatoryFields('txtClaimsReceivedReason', 'Please Enter the ClaimsReceivedReason');
            CheckForMandatoryFields('txtClaimsReceivedAmount', 'Please Enter the ClaimsReceivedAmount');
        }

    }
    if (ErrorCode != false) {
        $.ajax({
            url: '/Health/SavePolicyHealthInsuranceInfo',
            type: 'POST',
            data:
                 objPolicyHealthInsuranceInfo = $("#frmOtherHealthDetails").serialize(),
            success: function (data) {
                BootstrapDialog.alert("Other Health Policy Information saved Successfully");
                $('#btnClose').click();
            },
            failure: function (data) {
                BootstrapDialog.alert("Other Health Policy Information not saved Successfully");

            }
        });
    }
}
function ValidateInsurateDate(id, msg) {
    if ($('#' + id).val() == "" || $('#' + id).val() == undefined || $('#' + id).val() == null) {
        writeMultipleMessage("error", "" + msg, id);
        ErrorCode = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
        var dob = $('#' + id).val();
        var dtRegex = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|1[012])[\/]\d{4}$/);
        var result = dtRegex.test(dob);
        if (!result) {
            writeMultipleMessage("error", "This Field Must be valid Date Of Birth", id);
            return false;
            ErrorCode = false;
        }
        else {
            writeMultipleMessage("error", "", id);
            return true;

        }

    }
}
function ValidateRadiobutton(id1, id2, msg) {
    if ($('#' + id1).is(":checked") == false && $('#' + id2).is(":checked") == false) {
        writeMultipleMessage("error", "" + msg, id1);
        ErrorCode = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", id1);
        return true;

    }
}
function ShowClaimsReceivedAmount() {
    $('#divClaimsReceivedAmount').show();
    $('#divClaimsReceivedReason').show();
    ClearErrorMessage("rdnYesIsClaimsReceived");
}
function HideClaimsReceivedAmount() {
    $('#divClaimsReceivedAmount').hide();
    $('#divClaimsReceivedReason').hide();
    ClearErrorMessage("rdnYesIsClaimsReceived");
}
function ButtonValidation() {
    var ProductCode = $('#hdnProductCode').val();
    if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF") {

        if (ChkHealthQuestions == false || chkOtherHealthPolicy == false || ChkLifestyleDetails == false) {
            writeMultipleMessage("error", "Please click the HealthQuestions,EnterOtherHealth Details,and Lifestyle buttons");
            return false;
        } else {
            writeMultipleMessage("error", " ");
            return true;

        }
    }
    else if (ProductCode == "FGHSPI" || ProductCode == "FGHSPFF" || ProductCode == "FGHCI" || ProductCode == "FGHCFF" || ProductCode == "FGHSI" || ProductCode == "FGHSFF") {
        if (ChkHealthQuestions == false || chkOtherHealthPolicy == false) {
            writeMultipleMessage("error", "Please click the HealthQuestions and EnterOtherHealth Details Buttons");
            return false;
        }

        else {
            writeMultipleMessage("error", " ");
            return true;

        }
    }

}
function ValidateMobileNumber() {

    if ($("#txtMobile").val() != "") {
        if ($("#txtMobile").val().length < 10) {
            writeMultipleMessage("error", "Please enter a ten digit number", 'txtPhoneNo');
            return false;
        }
        //else if ($("#txtMobile").val() != "") {
        //    var MobNoPat = /^[7-9][0-9]{9}$/;
        //    var mobno = $("#txtMobile").val();
        //    var matchArray = MobNoPat.test(mobno);
        //    if (!matchArray) {
        //        writeMultipleMessage("error", "Please enter mobile number starting with 7/8/9", 'txtMobile');
        //        return false;
        //    }
        //    else {
        //        writeMultipleMessage("error", "", 'txtMobile');
        //        return true;
        //    }
        //}
    }
    else {
        writeMultipleMessage("error", "Please enter mobile number", 'txtMobile');
        return true;
    }
}
function ValidatePhoneNumber() {

    var PhoneNumber = $('#txtTelephone').val();

    if (PhoneNumber.length > 10 || PhoneNumber.length > 8) {
        writeMultipleMessage("error", "enter 10 or 8 characters", "PhoneNumber"); return false;
    }
    else {
        writeMultipleMessage("error", " ", "PhoneNumber"); return false;

    }
    if (PhoneNumber.charAt(0) != "9" || PhoneNumber.charAt(0) != "2") {
        writeMultipleMessage("error", "it should start with 9 or 2 ", "PhoneNumber");
        return false
    }
    else {
        writeMultipleMessage("error", " ", "PhoneNumber"); return false;

    }
}
function ValidateAge(e) {
    debugger;
    var Productcode = $('#hdnProductCode').val();
    var id = e.id;
    var DateOfBirth = $('#' + id + '').val();
    var dOB = DateOfBirth.split('/');
    var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
    var currentDate = new Date();
    if (DateOfBirth == "") {
        writeMultipleMessage("error", "Please Select The DateOfBirth ", id);

    }
    else {
        writeMultipleMessage("error", "", id);

    }
    if (Productcode == "FGHSPI" || Productcode == "FGHSPFF" || Productcode == "FGHSI" || Productcode == "FGHSFF" || Productcode == "FGHCI" || Productcode == "FGHCFF") {
        var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds 
        var diff = Math.floor((currentDate.getTime() - dob.getTime()) / (oneDay));
        var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

        if (diff < 90) {
            writeMultipleMessage("error", " Age should not be less than 90 days for issuing a Policy", id);
            ErrorCode == false;
            //return false;
        }
        else if (diffYear > 65) {
            writeMultipleMessage("error", " Age should not be greater than 65 years for issuing a Policy", id);
            ErrorCode == false;
            // return false;
            if (Productcode == "FGHSI" || Productcode == "FGHSFF") {
                if (diffYear > 50) {
                    writeMultipleMessage("error", " Age should not be greater than 50 years for issuing a Policy", id);
                    ErrorCode == false;
                }

            }
        }
        else {
            writeMultipleMessage("error", "", id);

            // return true;
        }

    }
    else if (Productcode == "FGHCCI" || Productcode == "FGHCCFF") {

        var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));
        //$.post('Health/LoadSumInsured', diffYear).done(function (data) {
        //    $('#ddlSumInsured' + id.charAt(id.length - 1)).val(data);
        //});
        var age = diffYear;
        $.ajax({

            url: '/Health/LoadSumInsured?age=' + age,
            type: 'GET',
            // data: age,
            cache: false,
            async: false,

            success: function (data) {
                //$('#ddlSumInsured' + id.charAt(id.length - 1)).val(data);
                $('#ddlSumInsured' + parseInt(id.replace('DateOfBirth', ''))).html('');
                $('#ddlSumInsured').html('');

                $.each(data, function (ID, option) {
                    if (Productcode == "FGHCCI") {
                        $('#ddlSumInsured' + parseInt(id.replace('DateOfBirth', ''))).append($('<option></option>').val(option.ID).html(option.Value));
                    }
                    else {
                        $('#ddlSumInsured').append($('<option></option>').val(option.ID).html(option.Value));

                    }

                })
            },
            failure: function () {
                BootstrapDialog.alert("Failed Load the SumInsured");

            }
        });
        if (diffYear < 6) {
            writeMultipleMessage("error", " Age should not be less than 6 years for issuing a Policy", id);
            return false;
        }
        else if (diffYear > 65) {
            writeMultipleMessage("error", " Age should not be greater than 65 years for issuing a Policy", id);
            return false;
        }
        else {
            writeMultipleMessage('error', '', id);
            return true;
        }
    }

}
function ValidateFirstName(e) {

    var id = e.id;
    var FirstName = $('#' + id + '').val();
    if (FirstName == "" || FirstName == undefined) {
        writeMultipleMessage("error", " Please Enter First Name.", id);

    }
    else {
        writeMultipleMessage("error", "", id);

    }

}
function ValidateLastName(e) {
    var id = e.id;
    var LastName = $('#' + id + '').val();

    if (LastName == "" || LastName == undefined) {
        writeMultipleMessage("error", " Please Enter Last Name.", id);

    }
    else {
        writeMultipleMessage("error", "", id);

    }

}
var Delterow = false;
var IsRowDeleted = false; //global variable to check the delete row is true
function DeleteRow(e, Constid) {

    var id = parseInt(e.id.replace(Constid, ''));
    var RelationshipId = $('#ddlRelationship' + id + ' option:selected').text();
    if (Delterow == true) {

        if (RelationshipId != "") {
            RelationshipString = RelationshipString.replace(RelationshipId, null);
        }
        quoteGridDataIndividualFloatData.splice(id, 1);
        // quoteGridDataIndividualFloatData.pop({ Index: id });
    }
    else {
        quoteGridDataIndividualFloatData.splice(id, 1);
        // quoteGridDataIndividualFloatData.pop({ Index: id });
    }
    $('#btnAddGridRow').attr("disabled", false);
    $('#' + e.id).parents('tr').remove()
    //$('#divIndividualFloat .grid-row-selected').remove();
    Delterow = true;
    IsRowDeleted = true;
}
var RelationshipString = '';
var cnt = 0;
function validateTopRow(e, Productcode) {

    var id;
    if (quoteGridDataIndividualFloatData.length == 0) { //if length=0 ,it will give error like unable to get the length
        BootstrapDialog.alert("grid length null");
    }
    var max = quoteGridDataIndividualFloatData.length;
    max = max - 1;
    id = quoteGridDataIndividualFloatData[max].Index;

    if (Productcode == "FGHCCI" || Productcode == "FGHCI" || Productcode == "FGHSPI" || Productcode == "FGHSI" || Productcode == "FGHSFF" || Productcode == "FGHCFF" || Productcode == "FGHSPFF" || Productcode == "FGHCCFF") {
        //var FirstName = $('#txtFirstName' + id).val();
        //var LastName = $('#txtLastName' + id).val();
        //var DateOfBirth = $('#DateOfBirth' + id).val();
        //var Relationship = $('#ddlRelationship' + id + ' option:selected').text();
        CheckForMandatoryFields('txtFirstName' + id, 'Please Enter The FirstName ' + (max + 1) + ' Row.');
        CheckForMandatoryFields('txtLastName' + id, 'Please Enter The LastName ' + (max + 1) + ' Row.');
        CheckForMandatoryFields('DateOfBirth' + id, 'Please Enter The DateOfBirth ' + (max + 1) + ' Row.');
        ValidateDropDown('ddlRelationship' + id, 'Please select the Relationship');

        if (Productcode == "FGHCI") {
            ValidateDropDown('ddlPlan' + id, 'Please select the Plan');
            ValidateDropDown('ddlPlanOption' + id, 'Please select the PlanOption');
            //ValidateDropDown('ddlPerdayHospitalisation' + id, 'Please select the PerdayHospitalisation');

        }
        else if (Productcode == "FGHSPI") {
            ValidateDropDown('ddlPlan' + id, 'Please select the Plan');
            // ValidateDropDown('ddlSumInsured' + id, 'Please select the SumInsured');
            //ValidateDropDown('ddlDeductible' + id, 'Please select the Deductible');

        }
        else if (Productcode == "FGHSI") {
            ValidateDropDown('ddlPlan' + id, 'Please select the Plan');
            ValidateDropDown('ddlSumInsured' + id, 'Please select the SumInsured');
            // ValidateDropDown('PerdayHospitalisation' + id, 'Please select the PerdayHospitalisation');

        }
        else if (Productcode == "FGHCCI") {
            ValidateDropDown('ddlSumInsured' + id, 'Please select the SumInsured');

        }
        var Relationship = $('#ddlRelationship' + id + ' option:selected').text();

        if (Relationship != "") {
            ChkRelationships(Relationship, 'ddlRelationship' + id, DateOfBirthId);
        }
        if (Relationship == "Select" || Relationship == "") {
            writeMultipleMessage("error", " Please Select Relationship in The " + (max + 1) + " Row.", 'ddlRelationship' + id);
            ErrorCode = false;
            return false;
        }
        else {
            writeMultipleMessage("error", "", 'ddlRelationship' + id);
            // return true;
        }

    }
}
function ValidateDropDown(id, msg) {
    if (($('#' + id + ' option:selected').text()) == "Select" || ($('#' + id + ' option:selected').text()) == "") {
        writeMultipleMessage("error", "" + msg, id);
        ErrorCode = false;
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);

        return true;
    }

}
function ValidateGender(e) {
    var id = e.id;
    if (($('#' + id + ' option:selected').text()) == "Select" || ($('#' + id + ' option:selected').text()) == "") {
        writeMultipleMessage("error", "Please select the Gender", id);
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
        return true;
    }

}
var IsSelfOrSpouse = false;//global variable for checking the raltionship is selected self or spouse inthe first row
function ChkRelationships(Relationship, RelationshipId, DateOfBirth, DateOfBirthId) {
    if ((Relationship != "Self") && (Relationship != "Spouse")) {
        if (IsSelfOrSpouse == false) {
            writeMultipleMessage("error", "Please Select Self/Spouse in the first Row of Relationship", RelationshipId);
            ErrorCode = false;
            IsSelfOrSpouse = true;
            return false;
        }
    }
    if (Relationship == "Self" || Relationship == "Mother" || Relationship == "Father" || Relationship == "Spouse") {
        var IsIdMatch = $.inArray(Relationship, RelationshipString.split(','));

        if (IsIdMatch >= 0) {
            writeMultipleMessage("error", Relationship + " already exist. ", RelationshipId);
            ErrorCode = false;
            return false;
        }
        else {
            RelationshipString = RelationshipString + Relationship + ',';
            if (Relationship == "Self") {
                //var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
                var dOB = DateOfBirth.split('/');
                var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
                var currentDate = new Date();
                var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));
                if (diffYear < 18) {
                    writeMultipleMessage("error", "The Age of Self does not below 18 Years ", DateOfBirthId);
                    ErrorCode = false;
                    return false;

                }

            }
            writeMultipleMessage("error", "", RelationshipId);
            return true;

        }
    }
    else if (Relationship == "Son" || Relationship == "Child" || Relationship == "Daughter") {

        if (cnt != null) {
            // RelationshipString.indexOf(Relationship, idx + 1)

            if (cnt == 2) {

                writeMultipleMessage("error", "Per Quote Only Two Entries for child/daughter/son", RelationshipId);
                ErrorCode = false;
                return false;
            }
            else {

                //var DateOfBirth = $('#DateOfBirth' + id.charAt(id.length - 1)).val();
                if (DateOfBirth != "" || DateOfBirth != undefined) {
                    var dOB = DateOfBirth.split('/');
                    var dob = new Date(dOB[2], dOB[1] - 1, dOB[0]);
                    var currentDate = new Date();
                    var diffYear = Math.floor((currentDate.getFullYear() - dob.getFullYear()));

                    if (diffYear > 25) {
                        writeMultipleMessage("error", "The Age of Child/Son/Daughter is must below 25 Years ", DateOfBirthId);
                        ErrorCode = false;
                        return false;

                    }
                    RelationshipString = RelationshipString + Relationship + ',';
                    cnt += 1;
                    writeMultipleMessage("error", "", RelationshipId);
                    return true;
                }

            }
        }
    }
}
var SelfSumInsured = 0; //global variable for storing self suminsured
function ChkSumInsured(SumInsured, Id, Relationship) {

    if (Relationship == "Self") {
        SelfSumInsured = SumInsured;
        return true;
    }
    else {
        if (SelfSumInsured == 0 || SelfSumInsured == undefined) {
            return false;
        }
        if (parseInt(SumInsured) > parseInt(SelfSumInsured)) {
            writeMultipleMessage("error", "The SumInsured of the " + Relationship + " must be lessthan of Self ", Id);
            ErrorCode = false;
            return false;
        }
    }
}
function DocumentNameChange() {
    $('#btnFile').clear();
}
function ValidateHeight(e) {

    var id = e.id;
    var Height = $('#' + id + '').val();

    if (Height == "" || Height == undefined) {

        writeMultipleMessage("error", "Please enter a Height in Centimeters", id);
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
        return true;
    }

}
function ValidateWeight(e) {
    var id = e.id;
    var weight = $('#' + id + '').val();

    if (weight == "" || weight == undefined) {
        writeMultipleMessage("error", "Please enter a weight in Kgs", id);
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
        return true;
    }


}
function ValidateNomineeName(e) {

    var id = e.id;
    var NomineeName = $('#' + id + '').val();
    if (NomineeName == "" || NomineeName == undefined) {
        writeMultipleMessage("error", " Please Enter Nominee Name.", id);
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
        return true;
    }

}
function ValidateInsuredProposerDetails() {
    ErrorCode = true;
    if ($("#currencyGrossPremium").val() >= 100000) {
        ValidatePAN();
    }
    //if (($('#hdnProposerFirstName').val() != $('#txtFirstName').val()) || ($('#hdnProposerLastName').val() != $('#txtLastName').val())) {
    //    Status = "Underwriting Referred";
    //    hdnDiscrepency = hdnDiscrepency + "Self/Primary Insured is not Proposer";
    //}
    CheckForMandatoryFields('dpPolicyStartDate', 'Please Select the policystartdate');
    CheckForMandatoryFields('txtFirstName', 'Please Enter the FirstName');
    CheckForMandatoryFields('txtLastName', 'Please Enter the LastName');
    CheckForMandatoryFields('dtDOB', 'Please Select the DateOfBirth');
    // CheckForMandatoryFields('txtAge', 'Please Enter the Age');

    ValidateProposerDropDown('ddlGender', "Please Select Gender");
    ValidateProposerDropDown('ddlMaritalStatus', "Please Select MaritalStatus");
    // ValidateProposerDropDown('ddlOccupation', "Please Select Occupation");
    ValidateProposerDropDown('ddlSalutation', "Please Select Title");
    ValidateProposerDropDown('ddlNationality', "Please Select Nationality");
    CheckForMandatoryFields('objAddressAddress1', 'Please Enter Address1');
    CheckForMandatoryFields('txtMobile', 'Please Enter Mobile No');
    CheckForMandatoryFields('objAddressPincode', 'Please Enter Pincode');
    // ValidateAddress1();
    // ValidatePinCode();
    var ProductCode = $('#hdnProductCode').val();
    collectAll();
    var quoteData = InsuredDetailsGridData;
    if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF") {
        CheckRadioButtonMandatoryFields('rdnYesAgeProofSubmitted', 'rdnNoAgeProofSubmitted', 'Please Check/Uncheck Is Age proof is Submitted or Not.');
        CheckRadioButtonMandatoryFields('rdnYesAnyFamilyMemExpired', 'rdnNoAnyFamilyMemExpired', 'Please Check/Uncheck Is Any family members(Father,Mother,Brother,Sister) expired before the age of 60 years');
        CheckRadioButtonMandatoryFields('rbYesAnyFamilyMemSufferWithIll', 'rbNoAnyFamilyMemSufferWithIll', 'Please Check/Uncheck Whether any family member suffered from any illness in the past  or is presently suffering from any illness.');
        //checking discrepency case
        if ($('#rdnNoAgeProofSubmitted').is(':checked') == true) {
            //BootstrapDialog.show({
            //    type: BootstrapDialog.TYPE_DANGER,
            //    title: 'Note',
            //    closable: true,
            //    size: 'size-large',
            //    message: "We are unable to process this proposal, kindly get in touch with the nearest local branch",
            //    buttons: [{
            //        label: 'OK',
            //        action: function (dialogItself) {
            //            dialogItself.close();
            //        }
            //    }]
            //});

        }
        if ($('#rdnYesAnyFamilyMemExpired').is(':checked') == true) {

            hdnDiscrepency = hdnDiscrepency + "family members expired before the age of 60 years ,";
            Status = "Underwriting Referred";
            //  $('#hdnDiscrepancy').val(hdnDiscrepency);

        }
        if ($('#rbYesAnyFamilyMemSufferWithIll').is(':checked') == true) {
            hdnDiscrepency = hdnDiscrepency + "family member suffered from any illness in the past or is presently suffering ,";
            Status = "Underwriting Referred";
        }

    }
    else if (ProductCode == "FGHSPI" || ProductCode == "FGHSPFF") {
        CheckRadioButtonMandatoryFields('rbYesIsOtherExistingHSPPolicy', 'rbNoIsOtherExistingHSPPolicy', 'Please Check/Uncheck Whether Do You have any existing Health Surplus policy in Future Generali');
    }
    else if (ProductCode == "FGHSI" || ProductCode == "FGHSFF") {
        //  CheckRadioButtonMandatoryFields('rbYesIsOtherExistingHSPolicy', 'rbNoIsOtherExistingHSPolicy', 'Please Check/Uncheck Whether Do You have any existing Health Suraksha policy in Future Generali');
    }
    else if (ProductCode == "FGHCI" || ProductCode == "FGHCFF") {
        CheckRadioButtonMandatoryFields('rbYesIsOtherExistingHCPolicy', 'rbNoIsOtherExistingHCPolicy', 'Please Check/Uncheck Whether Do You have any existing Hosphi Cash policy in Future Generali');
        CheckForMandatoryFields("txtMonthlyIncome", "Please Provide the MonthlyIncome");
        chkMonthlyIncome($('#txtPolicyType').val(), $('#txtPlan').val(), $('#txtMonthlyIncome').val(), "Minimum monthly income of the insured to opt for coverage under the Plan C must not be less than Rs 50000/-", "Minimum monthly income of the insured to opt for coverage under the Plan D must not be less than Rs 75000/- ");
    }

    var count = 0;
    var RowNo = 1;

    for (var index = 0; index < quoteData.length; index++) {
        var id = quoteData[index].Index;

        if (ProductCode == "FGHCI" || ProductCode == "FGHCFF" || ProductCode == "FGHCCI" || ProductCode == "FGHCCFF" || ProductCode == "FGHSI" || ProductCode == "FGHSFF" || ProductCode == "FGHSPI" || ProductCode == "FGHSPFF") {

            CheckForMandatoryFields('ddlGender' + id, 'Please Select Gender ' + RowNo + ' Row');
            if (ProductCode == "FGHSPI" || ProductCode == "FGHSPFF") {
                CheckForMandatoryFields('ddlNationality' + id, 'Please Select Nationality ' + RowNo + ' Row');
            }
            if (ProductCode == "FGHSI" || ProductCode == "FGHSFF" || ProductCode == "FGHCI" || ProductCode == "FGHCFF") {
                CheckForMandatoryFields('ddlRelationshipNomineeWithInsured' + id, 'Please Select NomineeRelationshipWithInsured ' + RowNo + ' Row');

            }


        }
        CheckForMandatoryFields('txtNomineeName' + id, 'Please Enter Nominee Name ' + RowNo + ' Row');

        RowNo = RowNo + 1;
        count++;
    }
}
var strAll = '';//common global variable to capture the grid row ids for button validations
var strClicked = '';//this is mediator to pickup the ids and push is their own string
var clickedYes = '';//this is for storing HealthQuestions ids string
var clickedYesLifestyle = '';//lifestyleids storing
var clickedYesEnterOtherDetails = ''//other health policies ids storing
//var clickedNo = '';
function collectClickedYes(id) {
    // clickedYes = '';
    clickedYes = clickedYes + id + ',';
    // clickedYes = clickedYes+id;

}
function collectClickedYesLifestyle(id) {
    // strClicked = '';
    // clickedYesLifestyle = '';
    clickedYesLifestyle = clickedYesLifestyle + id + ',';
    //clickedYesLifestyle = strClicked;

}
function collectClickedYesOtherHealth(id) {
    //strClicked = '';
    //clickedYesEnterOtherDetails = '';
    clickedYesEnterOtherDetails = clickedYesEnterOtherDetails + id + ',';
    // clickedYesEnterOtherDetails = strClicked;

}
function collectAll() {
    strAll = '';
    for (var index = 0; index < InsuredDetailsGridData.length; index++) {
        strAll = strAll + InsuredDetailsGridData[index].Index + ',';
    }

}
//Added to check whether all the mandatory fields are not null
function CheckForMandatoryFields(id, msg) {
    if ($("#" + id).val() == null || $("#" + id).val() == "") {
        writeMultipleMessage("error", msg, id);
        ErrorCode = false;
        return false;
    }

    else {
        writeMultipleMessage("error", '', id);
        return true;
    }
}
function CheckRadioButtonMandatoryFields(id1, id2, msg) { //yes,No,Msg

    var ProductCode = $('#hdnProductCode').val();
    if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF") {

        if ($("#" + id1).is(':checked') == false && $("#" + id2).is(':checked') == false) {
            writeMultipleMessage("error", msg, id1);
            ErrorCode = false;
            return false;
        }
        else if ($("#" + id1).is(':checked') == true) {
            //CheckForMandatoryFields('ddlRelationshipId', '');
            // CheckForMandatoryFields('txtAilment', 'Please Mention The Ailment');
            // ErrorCode = false;
            return false;
        }

            //else if ($("#" + id2).is('.checked') == true) {
            //    if($("#"+
            //    alert("We are unable to process this proposal, kindly get in touch with the nearest local branch");
            //    ErrorCode = false;
            //    return false;
            //}
        else {
            writeMultipleMessage("error", '', id1);
            return true;
        }
    }
    else if (ProductCode == "FGHSI" || ProductCode == "FGHSFF") {
        if ($("#" + id1).is(':checked') == false && $("#" + id2).is(':checked') == false) {
            writeMultipleMessage("error", msg, id1);
            ErrorCode = false;
        }
        else if ($("#" + id1).is('.checked') == true) {
            CheckForMandatoryFields('txtOtherExistingHSPolicyNO', 'Please Enter Existing PolicyNo in Future Suraksha');
            CheckForMandatoryFields('txtOtherExistingHSCustomerID', 'Please Enter Existing CustomerId in Future Suraksha');

        }
        else {
            writeMultipleMessage("error", '', id1);
        }
    }
    else if (ProductCode == "FGHSPI" || ProductCode == "FGHSPFF") {
        if ($("#" + id1).is(':checked') == false && $("#" + id2).is(':checked') == false) {
            writeMultipleMessage("error", msg, id1);
            ErrorCode = false;
        }
        else if ($("#" + id1).is(':checked') == true) {
            CheckForMandatoryFields('txtOtherExistingHSPPolicy', 'Please Enter Existing PolicyNo in Future Surplus');
            // CheckForMandatoryFields('txtOtherExistingHSCustomerID', 'Please Enter Existing CustomerId in Future Suraksha');

        }

        else {
            writeMultipleMessage("error", '', id1);
        }
    }
    else if (ProductCode == "FGHCI" || ProductCode == "FGHCFF") {
        if ($("#" + id1).is(':checked') == false && $("#" + id2).is(':checked') == false) {
            writeMultipleMessage("error", msg, id1);
            ErrorCode = false;
        }
        else if ($("#" + id1).is(':checked') == true) {
            CheckForMandatoryFields('txtOtherExistingHCPolicyNO', 'Please Enter Existing PolicyNo in Future Hospicash');
            // CheckForMandatoryFields('txtOtherExistingHSCustomerID', 'Please Enter Existing CustomerId in Future Suraksha');

        }

        else {
            writeMultipleMessage("error", '', id1);
        }
    }
}

function CheckRadioBtnGridMandatoryFields(id1, id2, msg1, msg2, btnid, HealthMemberId) { //yes,No,Msg1,msg2 for grid validations

    if ($("#" + id1).is(':checked') == false && $("#" + id2).is(':checked') == false) {
        writeMultipleMessage("error", msg1, id1);
        ErrorCode = false;
        return false;
    }
    else if ($("#" + id1).is(':checked') == true) {
        Status = "Underwriting Referred";
        if (clickedYesEnterOtherDetails == null || clickedYesEnterOtherDetails == '') {
            writeMultipleMessage('error', msg2, id1);
            ErrorCode = false
            return false;

        }

        else if (clickedYesEnterOtherDetails != '') {
            var IsIdMatch = $.inArray(HealthMemberId, clickedYesEnterOtherDetails.split(','));
            if (clickedYesEnterOtherDetails.length == strAll.length) {
                writeMultipleMessage("error", "", id1);
                return true;
            }
            else if (IsIdMatch > -1) {
                writeMultipleMessage("error", "", id1);
                return true;
            }
            else {
                writeMultipleMessage("error", msg2, id1);
                ErrorCode = false;
                return false;
            }
        }
        else {
            writeMultipleMessage("error", msg2, id1);
            //alert("Refer to Underwriter");
            return true;
        }
    }
    else {
        writeMultipleMessage("error", '', id1);
        return true;
    }

}
function SelectGender() {
    var Title = $('#ddlSalutation option:selected').text();
    $.ajax({

        url: '/Health/LoadGender?Title=' + Title,
        type: 'GET',
        // data: age,
        cache: false,
        async: false,

        success: function (data) {
            $('#ddlGender').html('');
            $.each(data, function (ID, option) {
                $('#ddlGender').append($('<option></option>').val(option.ID).html(option.Value));
            })
        },
        failure: function () {
            BootstrapDialog.alert(" Other Health Policy Information saved Successfully");

        }
    });

    //if (($('#ddlSalutation option:selected').text()) == "Mr." || ($('#ddlSalutation option:selected').text()) == "Mrs.") {
    //    $('#ddlGender option:selected').val() = "Male";
    //}
    //else if (($('#ddlSalutation option:selected').text()) == "Miss." || ($('#ddlSalutation option:selected').text()) == "Ms.") {
    //    $('#ddlGender option:selected').val() = "FeMale";
    //}
}
function ValidatePAN() {

    if ($("#currencyGrossPremium").val() >= 100000) {

        writeMultipleMessage("error", "Please Provide PAN Number", "txtPan");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtPan");
        return true;

    }

}
function ValidateProposerDropDown(id, msg) {
    if ($("#" + id).val() == "Select" || $("#" + id).val() == "") {
        writeMultipleMessage("error", "" + msg, "" + id);
        ErrorCode = false;
        return false;
    }

    else {
        writeMultipleMessage("error", "", '' + id);

        return true;
    }

}
function ValidateTitle() {
    if (($("#ddlSalutation option:selected").text()) == "Select" || ($("#ddlSalutation option:selected").text()) == "") {
        writeMultipleMessage("error", "Please Select Title", "ddlSalutation");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlSalutation");

        return true;
    }

}
function CheckButtonClicked(id, msg, HealthMemberId) {

    // clickedNo = strAll - clickedYes;
    if (clickedYes == null || clickedYes == '') {
        writeMultipleMessage('error', msg, id);
        ErrorCode = false
        return false;

    }

    else if (clickedYes != '') {
        var IsIdMatch = $.inArray(HealthMemberId, clickedYes.split(','));
        if (clickedYes.length == strAll.length) {
            writeMultipleMessage("error", "", id);
            return true;
        }
        else if (IsIdMatch > -1) {
            writeMultipleMessage("error", "", id);
            return true;
        }
        else {
            writeMultipleMessage("error", msg, id);
            ErrorCode = false;
            return false;
        }
    }
}
function CheckLifestyleBtnsClicked(id, msg, HealthMemberId) {

    // clickedNo = strAll - clickedYes;
    if (clickedYesLifestyle == null || clickedYesLifestyle == '') {
        writeMultipleMessage('error', msg, id);
        ErrorCode = false
        return false;

    }

    else if (clickedYesLifestyle != '') {
        var IsIdMatch = $.inArray(HealthMemberId, clickedYesLifestyle.split(','));
        if (clickedYesLifestyle.length == strAll.length) {
            writeMultipleMessage("error", "", id);
            return true;
        }
        else if (IsIdMatch > -1) {
            writeMultipleMessage("error", "", id);
            return true;
        }
        else {
            writeMultipleMessage("error", msg, id);
            ErrorCode = false;
            return false;
        }
    }
}
function ClearErrorMessage(id) {
    debugger
    writeMultipleMessage("error", "", id);
    return true;
}
function ValidateIsAgeProofSubmit(id) {
    writeMultipleMessage("error", "", id);
    if ($('#rdnNoAgeProofSubmitted').is(':checked') == true) {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_DANGER,
            title: 'Note',
            closable: true,
            size: 'size-large',
            message: "We are unable to process this proposal, kindly get in touch with the nearest local branch",
            buttons: [{
                label: 'OK',
                action: function (dialogItself) {
                    dialogItself.close();
                }
            }]
        });
    }
}
function AllowedSumInsured(e) {
    var id = e.id;
    if (($("#" + id + " option:selected").text()) > 500000) {
        writeMultipleMessage('error', 'Agent Portal Will Not Process,If SumInsured MoreThan 500000', id);
        ErrorCode = false
        return false;
    }
    else {
        writeMultipleMessage('error', '', id);
        ErrorCode = true;
        return true;
    }
}
function ValidateRelationship(e) {
    var id = e.id;
    if ($("#" + id).val() == "Select" || $("#" + id).val() == "") {
        writeMultipleMessage("error", "Please Select The Relationship", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);
        return true;
    }
}
function ValidatePlan(e) {

    var id = e.id;
    if ($("#" + id).val() == "Select" || $("#" + id).val() == "") {
        writeMultipleMessage("error", "Please Select The Plan", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);

        return true;
    }
}
function ValidatePlanOption(e) {

    var id = e.id;
    if ($("#" + id).val() == "Select" || $("#" + id).val() == "") {
        writeMultipleMessage("error", "Please Select The PlanOption", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);

        return true;
    }
}
function ValidatePerdayHospitalisation(e) {

    var id = e.id;
    if ($("#" + id).val() == "Select" || $("#" + id).val() == "") {
        writeMultipleMessage("error", "Please Select The PerdayHospitalisation", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);

        return true;
    }
}
function ValidateSumInsured(e) {

    var id = e.id;
    if ($("#" + id).val() == "Select" || $("#" + id).val() == "") {
        writeMultipleMessage("error", "Please Select The SumInsured", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);

        return true;
    }
}
function ValidateDeductible(e) {

    var id = e.id;
    if ($("#" + id).val() == "Select" || $("#" + id).val() == "") {
        writeMultipleMessage("error", "Please Select The Deductible", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);

        return true;
    }
}
function DoumentNameChange(e) {

    var id = e.id;
    if ($('#' + id + ' option:selected').text() == "Select" || $('#' + id + ' option:selected').text() == "") {
        writeMultipleMessage("error", "Please Select The Document Name", id);
        return false;
    }

    else {
        // DocumentNameString = DocumentNameString + $('#' + id + ' option:selected').text() + ',';

        writeMultipleMessage("error", "", id);
        return true;
    }
}
var data = [];
function chkDocumentName(e) {
    var id = e.id;
    var lastIndex = id.charAt(id.length - 1)
    if ($('#ddlDocumentName' + lastIndex + ' option:selected').text() == "Select" || $('#ddlDocumentName' + lastIndex + ' option:selected').text() == "") {
        writeMultipleMessage('error', 'Please Select the Document Name', id);
        return false;

    }
    else {
        writeMultipleMessage('error', '', id);
        // return true;
    }

    //var fileName = $('#'+id).val();

    //if (fileName == "") {
    //    writeMultipleMessage("error", "Please Provide a file to upload", "fileUpload");
    //    return false;
    //}
    //var grdRowID = $('#hdnDocumentId' + id.charAt(id.length - 1)).val();
    //if (grdRowID == undefined ||grdRowID=="") {
    //    grdRowID = 0;
    //}

    //if (fileName != "") {
    //    var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
    //    if (ext != "") {

    //        if (ext == "pdf" || ext == "docx" || ext == "odt" || ext == "gif") {
    //            var data;
    //            var isAppend = true;
    //            if (typeof FormData == "undefined") {
    //                data = [];
    //                isAppend = false;
    //            }
    //            else {
    //                data = new FormData();
    //            }

    //            var files = $("#"+id).get(0).files;

    //            data.append('file_' + 0, $('input[id="'+id+'"]')[0].files[0]);

    //            $.ajax({
    //                type: "POST",
    //                url: "../../Health/UploadFile?grdRowID=" + grdRowID,
    //                contentType: false,
    //                processData: false,
    //                data: data,
    //                success: function (result) {
    //                    $("#hdnDocumentId" + id.charAt(id.length - 1)).val(result);
    //                }
    //            });
    //        }
    //        else {

    //            alert("File format is invalid. Please upload the proper file");
    //        }
    //    }
    //}
}
var documentUploadGridData = [];
var DocumentNameString = '';
function AddNewDocumentUpload(e) {
    ErrorCode = true;
    // ValidateDocumentName(e);
    if (ErrorCode == true) {
        var newRow;
        var length = (documentUploadGridData.length) - 1;
        if (documentUploadGridData.length == 0) {
            length = 0;
        }
        else {
            length = documentUploadGridData[length].Index + 1;
        }
        //var length = (documentUploadGridData.length) + 1;
        //length = length - 1;
        documentUploadGridData.push({ Index: length });

        var documentData = documentUploadGridData;
        var DocumentRowId = 'hdnDocumentId' + length;
        var DocumentNameId = 'ddlDocumentName' + length;
        var FileNameId = 'txtFileName' + length;
        var ExistingFileNameId = 'lnkExistingFileName' + length;

        //var UploadId = 'btnUpload' + length;
        var DeleteId = 'btnDelete' + length;

        newRow = "<tr class='grid-row'><td class='grid-cell' style='display:none' data-name='Index'><input type='hidden' id=\'" + DocumentRowId + "\' ></td><td class='grid-cell' data-name='Document_Name'><select class='form-control'  id=\'" + DocumentNameId + "\'><option>Select</option></select></td><td class='grid-cell text-center' data-name='FileName' ><input name='File' class='form-control' type='file' onchange='chkDocumentName(this)' id=\'" + FileNameId + "\' /></td><td class='grid-cell text-center' data-name='ExistingFileName' style='display:None'><input id=\'" + ExistingFileNameId + "\' type='label' /></td><td class='grid-cell' data-name='Delete'><input class='btn btn-default' id=\'" + DeleteId + "\' style='margin-top: 3px;' type='button' value='Delete' onclick='DeleteUploadRow(this)'/></td></tr>";

        var documentNameData = LoadDocumentName();
        $('#divDocumentUpload tbody:last').append(newRow);

        var items = "";

        $.each(documentNameData, function (index, itemData) {
            items += '<option Value=' + itemData.ID + '>' + itemData.Value + '</option>';
        });
        $("#" + DocumentNameId).append(items);
    }
}

function DeleteUploadRow(e) {
    var CnstControlId = "btnDelete";
    var id = parseInt(e.id.replace(CnstControlId, ''));
    var DocumentName = $('#ddlDocumentName' + id + ' option:selected').text();
    if (DocumentName != "") {
        DocumentNameString = DocumentNameString.replace(DocumentName, null);
    }
    documentUploadGridData.splice(id, 1);
    $('#' + e.id).parents('tr').remove();
    // $("#divDocumentUpload .grid-row-selected").remove();

}
function UploadFiles() {
    // var documentData = documentUploadGridData;
    var count;
    var jsonDocumentUpload = [];
    var data = new FormData();
    GetDocuments(jsonDocumentUpload, data);

    //for (var index = 0; index < documentData.length; index++) {
    //    var id = documentData[index].Index;
    //    var DocumentId = $(' #hdnDocumentId' + id).val();
    //    var DocumentNameId = $(' #ddlDocumentName' + id).val();
    //    var FileNameId = $(' #txtFileName' + id).val();

    //    if (FileNameId == "") {
    //        writeMultipleMessage("error", "Please Provide a file to upload", "fileUpload");
    //        return false;
    //    }
    //    var grdRowID = DocumentNameId;
    //    if (grdRowID == undefined || grdRowID == "") {
    //        grdRowID = 0;
    //    }

    //    if (FileNameId != "") {
    //        var ext = FileNameId.substring(FileNameId.lastIndexOf('.') + 1);
    //        if (ext != "") {

    //            if (ext == "pdf" || ext == "docx" || ext == "odt" || ext == "gif") {
    //                //data;
    //                var isAppend = true;
    //                if (typeof FormData == "undefined") {
    //                    data = [];
    //                    isAppend = false;
    //                }
    //                else {
    //                    // data = new FormData();
    //                }

    //                var files = $('#txtFileName' + id).get(0).files;

    //                data.append('file_' + index, $('input[id="txtFileName' + id + '"]')[0].files[0]);
    //            }
    //        }
    //    }
    //    jsonDocumentUpload.push({ Document_Name: DocumentNameId, FileName: FileNameId });
    //    count++;

    //}
    if (ErrorCode != false) {
        var docData = JSON.stringify(jsonDocumentUpload);
        $.ajax({
            type: "POST",
            url: "../../Health/SaveDocuments?documentData=" + docData + '&PolicyId=' + $('#hdnPolicyId').val(),
            contentType: false,
            processData: false,
            data: data,

        });
        return true;
    }
    return false;
}
function GetDocuments(jsonDocumentUpload, data) {

    $("#divDocumentUpload tbody").find("tr").each(function (index) {
        // documentUploadGridData
        var Document_Name = $(this).find("[data-name=Document_Name]").find("option:selected").val();
        var FileNameId = $(this).find("[data-name=FileName]").find("input[type=file]").val();
        // var ExistingFileId = $(this).find("[data-name=ExistingFileName]").find("a[class=Id]").text();

        if (FileNameId == "") {
            //if (ExistingFileId != "") {
            //    ExistingFileId = FileNameId;
            //}
            writeMultipleMessage("error", "Please Provide a file to upload", "txtFileName" + index);
            return false;
        }
        if (Document_Name != "" && FileNameId != "" && FileNameId != undefined) {
            if (FileNameId != "") {

                var ext = FileNameId.substring(FileNameId.lastIndexOf('.') + 1);
                if (ext != "") {

                    if (ext == "pdf" || ext == "docx" || ext == "xls" || ext == "xlsx" || ext == "doc" || ext == "jpeg" || ext == "jpg" || ext == "png" || ext == "gif") {
                        //data;
                        var isAppend = true;
                        if (typeof FormData == "undefined") {
                            // data = [];
                            isAppend = false;
                        }
                        else {
                            // data = new FormData();
                        }
                        var files = $('#txtFileName' + index).get(0).files;
                        data.append('file_' + index, $('input[id="txtFileName' + index + '"]')[0].files[0]);
                    }
                    else {
                        BootstrapDialog.alert("File format is invalid. Please upload the proper file like pdf,doc,xls,jpeg etc.");
                        ErrorCode = false;
                    }
                }
            }

            jsonDocumentUpload.push({ Document_Name: Document_Name, FileName: FileNameId });//, Plan: Plan, PlanOption: PlanOption, PerdayHospitalisation: PerdayHospitalisation, Deductible: Deductible });
        }

    });
}
var arr = new Array();
function ValidateDocumentUpload() {

    var ProductCode = $('#hdnProductCode').val();
    $("#divDocumentUpload tbody").find("tr").each(function (index) {
        var documentName = $(this).find("[data-name=Document_Name]").find("option:selected").text();
        var FileNameId = $(this).find("[data-name=FileName]").find("input[type=file]").val();
        // var ExistingFileId = $(this).find("[data-name=ExistingFileName]").find("a[class=Id]").text();
        if (FileNameId == "") {
            //if (ExistingFileId != "") {
            //    FileNameId = ExistingFileId;
            //}
            //else {
            writeMultipleMessage("error", "Please Provide a file to upload", "txtFileName" + index);
            return false;
            ErrorCode = false;
            // }
        }

        if (documentName != "" && FileNameId != "" && FileNameId != undefined) {
            if (FileNameId != "") {

                var ext = FileNameId.substring(FileNameId.lastIndexOf('.') + 1);
                if (ext != "") {

                    if (ext == "pdf" || ext == "docx" || ext == "xls" || ext == "xlsx" || ext == "doc" || ext == "jpeg" || ext == "jpg" || ext == "png" || ext == "gif") {
                    }
                    else {
                        writeMultipleMessage("error", "File format is invalid in " + ++index + " Row Please upload the proper file like pdf,doc,xls,jpeg etc.", "txtFileName" + index);
                        // BootstrapDialog.alert("File format is invalid in"+ ++index +" Row Please upload the proper file like pdf,doc,xls,jpeg etc.");
                        ErrorCode = false;
                    }
                }
            }
        }

        if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF" || ProductCode == "FGHSI" || ProductCode == "FGHSFF") {
            if (documentName == "Proposal Form" || documentName == "Age Proof") {
                var IsIdMatch = $.inArray(documentName, DocumentNameString.split(','));
                if (IsIdMatch >= 0) {
                    writeMultipleMessage("error", documentName + " already exist. ", 'ddlDocumentName' + index);
                    ErrorCode = false;
                    return false;
                }
                else {
                    DocumentNameString = DocumentNameString + documentName + ',';
                    writeMultipleMessage("error", "", 'ddlDocumentName' + index);
                    // return true;

                }
            }

        }
        else if (ProductCode == "FGHSPI" || ProductCode == "FGHSPFF") {
            if (documentName == "Proposal Form") {
                var IsIdMatch = $.inArray(documentName, DocumentNameString.split(','));
                if (IsIdMatch >= 0) {
                    writeMultipleMessage("error", documentName + " already exist. ", 'ddlDocumentName' + index);
                    ErrorCode = false;
                    return false;
                }
                else {
                    DocumentNameString = DocumentNameString + documentName + ',';
                    writeMultipleMessage("error", "", 'ddlDocumentName' + index);
                    // return true;

                }
            }
        }
        else if (ProductCode == "FGHCI" || ProductCode == "FGHCFF") {
            if (documentName == "Proposal Form" || documentName == "Income Proof") {


                var IsIdMatch = $.inArray(documentName, DocumentNameString.split(','));

                if (IsIdMatch >= 0) {
                    writeMultipleMessage("error", documentName + " already exist. ", 'ddlDocumentName' + index);
                    ErrorCode = false;
                    return false;
                }
                else {
                    DocumentNameString = DocumentNameString + documentName + ',';

                    writeMultipleMessage("error", "", 'ddlDocumentName' + index);
                    // return true;

                }
            }
        }
        //arr = arr + documentName + ',';

    });
    if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF" || ProductCode == "FGHSI" || ProductCode == "FGHSFF" || ProductCode == "FGHSPI" || ProductCode == "FGHSPFF" || ProductCode == "FGHCI" || ProductCode == "FGHCFF") {
        if ($.inArray("Proposal Form", DocumentNameString.split(',')) == -1) {

            BootstrapDialog.alert("Please Upload the Proposal Form");
            ErrorCode = false;
            return false;
        }
    }
    else {
        if (ProductCode == "FGHCCI" || ProductCode == "FGHCCFF" || ProductCode == "FGHSI" || ProductCode == "FGHSFF") {
            if ($.inArray("Age Proof", DocumentNameString.split(',')) == -1) {

                BootstrapDialog.alert("Please Upload the Age Proof");
                ErrorCode = false;
                return false;
            }
        }
        else if (ProductCode == "FGHCI" || ProductCode == "FGHCFF") {
            if ($.inArray("Income Proof", DocumentNameString.split(',')) == -1) {
                BootstrapDialog.alert("Please Upload the Income Proof");
                ErrorCode = false;
                return false;
            }
        }
    }

}
function ValidateNationality(e) {
    var id = e.id;
    if ($('#' + id + ' option:selected').text() == "Select" || $('#' + id + ' option:selected').text() == "") {
        writeMultipleMessage("error", "Please Select The Nationality", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);
        return true;
    }
}
function ValidateNomineeRelationship(e) {
    var id = e.id;
    if ($('#' + id + ' option:selected').text() == "Select" || $('#' + id + ' option:selected').text() == "") {
        writeMultipleMessage("error", "Please Select The NomineeRelationship with Insured", id);
        return false;
    }

    else {
        writeMultipleMessage("error", "", id);
        return true;
    }
}
function ValidateNationality() {
    if ($('#ddlNationality').val() == "" || $('#ddlNationality').val() == "Select") {
        writeMultipleMessage('error', 'Please Select the Nationality', 'ddlNationality');
        return false;
    }
    else if ($('#ddlNationality option:selected').text() == "Non Resident Indian" || $('#ddlNationality option:selected').text() == "Foreigner") {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_DANGER,
            title: 'Note',
            closable: true,
            size: 'size-large',
            message: "We are unable to process this proposal, kindly get in touch with the nearest local branch",
            buttons: [{
                label: 'OK',
                action: function (dialogItself) {
                    dialogItself.close();
                }
            }]
        });
        return false;
    }
    else {
        writeMultipleMessage('error', '', 'ddlNationality');
        return true;
    }
}
function ValidatePANNo() {
    if ($("#currencyGrossPremium").val() >= 100000) {

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
}
function ValidatePinCode() {

    var pinCode = $("#ObjCommunicationAddressPincode").val();
    if (pinCode == "") {
        writeMultipleMessage("error", " Please enter your PinCode", "ObjCommunicationAddressPincode");
        ErrorCode = false;
        return false;
    }
    else if (pinCode != "") {
        if (pinCode.length < 6) {
            writeMultipleMessage("error", 'Please enter valid PinCode', "ObjCommunicationAddressPincode");
            ErrorCode = false;
            return false;
        }
        else {
            writeMultipleMessage("error", "", "ObjCommunicationAddressPincode");
            return true;
        }
    }
} //ObjCommunicationAddressAddress1
function ValidateAddress1() {

    if ($("#ObjCommunicationAddressAddress1").val() == "") {
        writeMultipleMessage("error", "Please enter Address1", "ObjCommunicationAddressAddress1");
        return false;
    }

    else {
        writeMultipleMessage("error", "", "ObjCommunicationAddressAddress1");
        return true;
    }

}
function ValidateHSPPlan(e) {
    var id = e.id;
    var PlanId = $('#' + id).val();
    if (PlanId == "" || PlanId == "Select") {
        writeMultipleMessage("error", "Please Select The Plan", id);
        return false;
    }
    else {
        writeMultipleMessage("error", "", id);
    }

    var PolicyType = $('#ddlPolicyType').val();
    var ProductCode = $('#hdnProductCode').val();
    $.ajax({

        url: '/Health/LoadHSPSumInsured?PlanId=' + PlanId + '&PolicyType=' + PolicyType + '&ProductCode=' + ProductCode,
        type: 'GET',
        // data: age,
        cache: false,
        async: false,

        success: function (data) {
            if (PolicyType == "89") {
                $('#ddlSumInsured' + parseInt(id.replace('ddlPlan', ''))).val(data.SumInsured);
                $('#ddlDeductible' + parseInt(id.replace('ddlPlan', ''))).val(data.Deductible);
            }
            else {
                $('#ddlSumInsured').val(data.SumInsured);
                $('#ddlDeductible').val(data.Deductible);

            }
        },
        fail: function () {
            BootstrapDialog.alert("Failed to fill SumInsured and Deductible");

        }
    });
}
function ValidateHSPlan(e) {
    var result = ValidatePlan(e);
    if (result == false)
        return false
    var id = e.id;
    var PlanId = $('#' + id).val();
    var PolicyType = $('#ddlPolicyType').val();
    var ProductCode = $('#hdnProductCode').val();
    var FloaterPlanType = $('#ddlFloaterPlanType').val();
    if (PolicyType == "90") {
        if (FloaterPlanType == "Select" || FloaterPlanType == "") {
            writeMultipleMessage("error", "Please Select FloaterPlan type", "ddlFloaterPlanType");
            return false;
        }
    }

    $.ajax({
        url: '/Health/LoadHSSumInsured?PlanId=' + PlanId + '&PolicyType=' + PolicyType + '&ProductCode=' + ProductCode + '&FloaterPlanId=' + FloaterPlanType,
        type: 'GET',
        // data: age,
        cache: false,
        async: false,

        success: function (data) {

            $('#ddlSumInsured' + parseInt(id.replace('ddlPlan', ''))).html('');
            $('#ddlSumInsured').html('');

            $.each(data, function (ID, option) {
                if (PolicyType == "89") {
                    $('#ddlSumInsured' + parseInt(id.replace('ddlPlan', ''))).append($('<option></option>').val(option.ID).html(option.Value));
                }
                else {
                    $('#ddlSumInsured').append($('<option></option>').val(option.ID).html(option.Value));

                }

            })

        },
        failure: function () {
            BootstrapDialog.alert("Failed to fill SumInsured and Deductible");

        }
    });
}
function ValidatedFloaterPlanType() {

    RelationshipHSString = '';
    var count = 0;
    var FloaterPlanType = $('#ddlFloaterPlanType option:selected').text();

    if (FloaterPlanType == "Select" || FloaterPlanType == "") {
        writeMultipleMessage("error", "Please Select Floater Plan Type", "ddlFloaterPlanType");
        return false;
    }

    else {
        writeMultipleMessage("error", "", "ddlFloaterPlanType");

    }
    var length = $("#divIndividualFloat tbody").children('tr').length;
    if (FloaterPlanType == "2 Adults" || FloaterPlanType == "1 Adult + 1 Child") {

        for (; length <= 2; length++) {
            if (length == 2) {
                $('#btnAddGridRow').attr("disabled", "disabled");
                return true;
            }
            else {
                $('#btnAddGridRow').click();
                $('.HSDeleteColumn').css("background-color", "grey");
                $('.HSDeleteColumn').attr("disabled", "disabled");
            }

        }
        for (; length > 1;) {
            if (length == 2) {
                $('#btnAddGridRow').attr("disabled", "disabled");
                return true;
            }
            var id = length - 1;
            length = length - 1;
            // quoteGridDataIndividualFloatData.length = quoteGridDataIndividualFloatData.length - 1;
            $('#btnHCCFloatDeleteGridRow' + id).click();
            $('#divIndividualFloat .grid-row-selected').remove();

        }

        // if (quoteGridDataIndividualFloatData.length < 1) {

        //}
        //if (quoteGridDataIndividualFloatData.length != 2) {
        //    $('#btnAddGridRow').click();
        //    if (quoteGridDataIndividualFloatData.length == 2) {
        //        $('#btnAddGridRow').attr("disabled", "disabled");

        //    }
        //}

    }
    else if (FloaterPlanType == "2 Adults + 1 Child" || FloaterPlanType == "1 Adult + 2 Children") {
        for (; length <= 3; length++) {
            if (length == 3) {
                $('#btnAddGridRow').attr("disabled", "disabled");
                return true;
            }
            else {
                $('#btnAddGridRow').click();

                $('.HSDeleteColumn').css("background-color", "grey");
                $('.HSDeleteColumn').attr("disabled", "disabled");
            }
        }
        for (; length > 1;) {
            if (length == 3) {
                $('#btnAddGridRow').attr("disabled", "disabled");
                return true;
            }
            else {
                var id = length - 1;
                length = length - 1;
                //quoteGridDataIndividualFloatData.length = quoteGridDataIndividualFloatData.length - 1;
                $('#btnHCCFloatDeleteGridRow' + id).click();
                $('#divIndividualFloat .grid-row-selected').remove();
            }

        }
        //if (quoteGridDataIndividualFloatData.length < 2) {

        //}
    }
    else if (FloaterPlanType == "2 Adults + 2 Children") {
        for (; length <= 4; length++) {
            if (length == 4) {
                $('#btnAddGridRow').attr("disabled", "disabled");
                return true;
            }
            else {
                $('#btnAddGridRow').click();
                $('.HSDeleteColumn').css("background-color", "grey");
                $('.HSDeleteColumn').attr("disabled", "disabled");
            }

        }
    }
    $('#btnAddGridRow').attr("disabled", "disabled");

}
function ValidateHCPlan(e) {
    ValidatePlan(e);
    var id = e.id;
    var PlanId = $('#' + id).val();
    var PolicyType = $('#ddlPolicyType').val();
    var ProductCode = $('#hdnProductCode').val();

    $.ajax({

        url: '/Health/LoadHCPerDayHospitalisation?PlanId=' + PlanId + '&PolicyType=' + PolicyType + '&ProductCode=' + ProductCode,
        type: 'GET',
        // data: age,
        cache: false,
        async: false,

        success: function (data) {
            // $('#txtPerDayHospitalisation' + id.charAt(id.length - 1)).html('');
            //$('#txtPerDayHospitalisation').html('');

            if (PolicyType == "89") {
                $('#txtPerDayHospitalization' + parseInt(id.replace('ddlPlan', ''))).val(data.PerDayHospitalisation);
            }
            else {
                $('#txtPerDayHospitalisation').val(data.PerDayHospitalisation);

            }

        },
        failure: function () {
            BootstrapDialog.alert("Failed to fill PerDayHospitalisation");

        }
    });
}
function chkMonthlyIncome(PolicyType, Plan, MonthlyIncome, msg1, msg2) {

    if (Plan == "Plan C") {
        if (MonthlyIncome < 50000) {
            writeMultipleMessage("error", "" + msg1, "txtMonthlyIncome");
            ErrorCode = false;
            return false;
        }
    }
    else if (Plan == "Plan D") {
        if (MonthlyIncome < 75000) {
            writeMultipleMessage("error", "" + msg2, "txtMonthlyIncome");
            ErrorCode = false;
            return false;
        }
        else {
            writeMultipleMessage("error", "", "txtMonthlyIncome");
            return true;
        }

    }

}
function ValidateMonthlyIncome() {
    if ($('#txtMonthlyIncome').val() == "") {
        writeMultipleMessage("error", "Please Provide MonthlyIncome", "txtMonthlyIncome");
        return true;
    }
    else {
        writeMultipleMessage("error", "", "txtMonthlyIncome");
        return true;
    }

}
function ClosePremium() {
    $('#pnlPremium').hide();
    $('#divSave').hide();
    $('#pnlHealthPlan').hide();

}
//this function used to display subset questions based on parent answer for future reference
function DisplayHideQns() {
    var AnswerTextYes;
    var AnswerTextNo;
    var AnswerType;
    $("#divHealthQns tbody").find("tr").each(function () {
        var QId = $(this).find("[data-name=QId]").find('input[type=hidden]').val();
        AnswerType = $(this).find("[data-name=AnswerType]").find("[type=hidden]").val();
        if (AnswerType == "Option") {
            AnswerTextYes = $(this).find("[data-name=AnswerText]").find("[id=rdbtnAnswerTextYes" + QId + "]").is(':checked');
            AnswerTextNo = $(this).find("[data-name=AnswerText]").find("[id=rdbtnAnswerTextNo" + QId + "]").is(':checked');
        }
        if ((QId == "41" && AnswerTextYes == true) || (QId == "47" && AnswerTextYes == true) || (QId == "50" && AnswerTextYes == true) || (QId == "50" && AnswerTextYes == true) ||
            (QId == "55" && AnswerTextYes == true) || (QId == "56" && AnswerTextYes == true) || (QId == "57" && AnswerTextYes == true) || (QId == "58" && AnswerTextYes == true)) {
            $('#rdbtnAnswerTextYes' + QId).change();

        }
    });

}
