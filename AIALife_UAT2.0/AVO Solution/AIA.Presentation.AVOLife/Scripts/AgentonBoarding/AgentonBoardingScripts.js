function GenerateProspect() {
    var ProspectFirstName = $("#txtProspectFirstName").val();
    var ProspectLastName = $("#txtProspectLastName").val();
    var ProspectDob = $("#dpProspectdob").val();
    var ProspectAge = parseInt($("#txtProspectAge").val());
    var ProspectGenderId = $("#ddlRecruitmentGender").val();
    var ProspectGenderValue = $("#ddlRecruitmentGender option:selected").text()
    var ProspectProfession = $("#ddlProspectProfession").val();
    var ProspectNeedIncome = $("#ddlProspectIncome").val();
    var ProspectMobile1 = parseInt($("#txtProspectMobileNumber").val());
    var ProspectAddress1 = $("#txtProspectAddress1").val();
    var ProspectPincode = $("#txtProspectpincode").val();
    if (!ValidateProspectDetails()) {

    }
    else if (ProspectAge < 18 || isNaN(ProspectAge)) {
        writeMultipleMessage("error", "Min Age of prospect must be 18 years", "txtProspectAge");
        return false;
    }
    else {
        var s1 = $("#frmRecruitmentSearch").serialize();
        writeMultipleMessage("error", "", "txtProspectAge");
        //$.post("../../AgentonBoarding/SaveProspect?DOB=" + ProspectDob, $("#frmRecruitmentSearch").serialize())
        $.post("../../AgentonBoarding/SaveProspect", $("#frmRecruitmentSearch").serialize())
            .done(function (data) {
                if (data.Message == 'Success') {
                    var ProspectCode = $("#hdnProspectCode").val();
                    if (data.IsAgentPendingProspect == true) {
                        var Message = '<p>Some mandatory fields are missing,Prospect ' + ProspectCode + " Code Moved to Pending Stage" + ' </p>';
                    }
                    else if (data.IsProspectNICBlocked == true)
                    {
                        var Message = '<p>SSN Number is Under Black List,Prospect ' + ProspectCode + " Code Moved to Audit/Finance/HRM Departments" + ' </p>';
                    }
                    else {
                        var Message = '<p>Prospect ' + ProspectCode + " is created successfully" + ' </p>';
                    }                   
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';                    
                }
                $('#AgentonBoardingProspectModalBody').html(Message);
                $("#AgentonBoardingProspectModal").modal("show");
            }).
            fail(function () {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
    }

    //var NICNos = $("#txtProspectNIC").val();
    //if (NICNos != null && NICNos != "") {
    //    $.ajax({
    //        url: '../../AgentonBoarding/CheckingNICNumberExist',
    //        type: "POST",
    //        data: { NICNo: NICNos },
    //        success: function (data) {                
    //            if (data == "success") {
    //                writeMultipleMessage("error", "", "txtProspectNIC");
    //                $.post("../../AgentonBoarding/SaveProspect", $("#frmRecruitmentSearch").serialize())
    //                    .done(function (data) {
    //                        if (data.Message == 'Success') {
    //                            var ProspectCode = $("#hdnProspectCode").val();
    //                            if (data.IsAgentPendingProspect == true) {
    //                                var Message = '<p>Some mandatory fields are missing,Prospect ' + ProspectCode + " Code Moved to Pending Stage" + ' </p>';
    //                            }
    //                            else {
    //                                var Message = '<p> Data Saved Successfully,Prospect ' + ProspectCode + " Code Confirmed Successfully" + ' </p>';
    //                            }
    //                            $('#AgentonBoardingProspectModalBody').html(Message);
    //                            $("#AgentonBoardingProspectModal").modal("show");
    //                        }
    //                        else {
    //                            var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
    //                            $('#AgentonBoardingProspectModalBody').html(Message);
    //                            $("#AgentonBoardingProspectModal").modal("show");
    //                        }

    //                    }).
    //                    fail(function () {
    //                        $("").html("Data Submission Failed. Try after some Time");
    //                    });
    //            }
    //            else {
    //                writeMultipleMessage("error", "SSN Number already exist,you cannot create duplicate prospect code", "txtProspectNIC");
    //                return false;
    //            }
    //        },
    //        fail: function (data) {
    //            return false;
    //        }
    //    });
    //}    
}
function GenerateSuspect() {
    if (!ValidateSuspectDetails()) {

    }
    else {
        $.post("../../AgentonBoarding/SaveSuspect", $("#frmRecruitmentSuspect").serialize())
       .done(function (data) {
           if (data.Message == 'Success') {
               var SuspectCode = $("#hdnSuspectCode").val();
               var Message = '<p>Suspect ' + SuspectCode + " is created successfully" + ' </p>';               
           }
           else {
               var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';               
           }
           $('#AgentonBoardingSuspectModalBody').html(Message);
           $("#AgentonBoardingSuspectModal").modal("show");
       }).
       fail(function () {
           BootstrapDialog.alert("Data Submission Failed. Try after some Time");
       });
    }
}
function SaveAgetonBoardingDetails() {
    if (!ValidateBasicDetails()) {

    }
    else {
        $.post("../../AgentonBoarding/SaveAgentonBoarding", $("#frmAgentRecruitment").serialize())
            .done(function (data) {
                if (data.Message == 'Success') {
                    var Message = '<p> Data Saved Successfully' + ' </p>';                                      
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                }
                $('#AgentonBoardingModalBody').html(Message);
                $("#AgentonBoardingModal").modal("show");
            }).
            fail(function () {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
    }
}
function SaveAgetonBoardingEducationDetails() {    
    clearAllErrors();    
    if (!ValidateEducationDetails()) {

    }
    else {
        var Grid = [];
        var Array = [];      
        getEducationDetails(Grid);   
        FetchSpecialAchievements(Array);
        //var personalDOB = $("#dpdob").val();     
        //$.post("../../AgentonBoarding/SaveAgetonBoardingEducation?PersonalDOB=" + personalDOB +"&getEducationDetails=" + JSON.stringify(Grid), $("#frmAgentRecruitment").serialize())
        $.post("../../AgentonBoarding/SaveAgetonBoardingEducation?getEducationDetails=" + JSON.stringify(Grid), $("#frmAgentRecruitment").serialize())
        .done(function (data) {
                if (data.Message == 'Success') {
                    var IsFail = $("#hdnSubjectFailCount").val();
                    if (IsFail == "1") {
                        var Message = '<p> Prospect must pass all Complusory Subjects.' + ' </p>';
                        $("#tabReferenceDetails").hide();
                        $("#liInterviewDetails").hide();
                    }
                    else {
                        var Message = '<p> Data Saved Successfully' + ' </p>';
                        $("#tabReferenceDetails").show();
                        $("#liInterviewDetails").show();
                        FetchInterviewLevels();
                    }                   
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';                    
                }
                $('#AgentonBoardingModalBody').html(Message);
                $("#AgentonBoardingModal").modal("show");
            }).
            fail(function () {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
    }
}
function SaveAgentonBoardingReference() {
       if (!ValidateReferenceDetails()) {

    }
       else {
           //var personalDOB = $("#dpdob").val();
           //$.post("../../AgentonBoarding/SaveAgentonBoardingReferences?PersonalDOB=" + personalDOB, $("#frmAgentRecruitment").serialize())
           $.post("../../AgentonBoarding/SaveAgentonBoardingReferences", $("#frmAgentRecruitment").serialize())
            .done(function (data) {
                if (data.Message == 'Success') {
                    var Message = '<p> Data Saved Successfully' + ' </p>';                    
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';                    
                }
                $('#AgentonBoardingModalBody').html(Message);
                $("#AgentonBoardingModal").modal("show");
            }).
            fail(function () {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
    }
}
function SaveAgentNomineeDetails() {
    var Grid = [];
    getAgentNomineeDetails(Grid);
    //var personalDOB = $("#dpdob").val();
    //$.post("../../AgentonBoarding/SaveAgetonBoardingNominee?PersonalDOB=" + personalDOB + "&getAgentNomineeDetails=" + JSON.stringify(Grid), $("#frmAgentRecruitment").serialize())
    $.post("../../AgentonBoarding/SaveAgetonBoardingNominee?getAgentNomineeDetails=" + JSON.stringify(Grid), $("#frmAgentRecruitment").serialize())
    .done(function (data) {
            if (data.Message == 'Success') {
                var Message = '<p> Data Saved Successfully' + ' </p>';                
            }
            else {
                var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';                
            }
            $('#AgentonBoardingModalBody').html(Message);
            $("#AgentonBoardingModal").modal("show");
        }).
        fail(function () {
            BootstrapDialog.alert("Data Submission Failed. Try after some Time");
        });
}
function SaveAgentTraningDetails() {
    var Grid = [];
    if (!ValidateTrainingDetails()) {

    }
    else {
        var startdate = $("#dpTrainingStartDate").val();
        var enddate = $("#dpTrainingEndDate").val();
        //var personalDOB = $("#dpdob").val();
        //$.post("../../AgentonBoarding/SaveAgetonBoardingTraning?StartDate=" + startdate + "&EndDate=" + enddate +"&PersonalDOB=" + personalDOB, $("#frmAgentRecruitment").serialize())
        $.post("../../AgentonBoarding/SaveAgetonBoardingTraning?StartDate=" + startdate + "&EndDate=" + enddate, $("#frmAgentRecruitment").serialize())
                .done(function (data) {
                    if (data.Message == 'Success') {
                        var Message = '<p> Data Saved Successfully' + ' </p>';                       
                    }
                    else {
                        var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';                        
                    }
                    $('#AgentonBoardingModalBody').html(Message);
                    $("#AgentonBoardingModal").modal("show");
                }).
                fail(function () {
                    BootstrapDialog.alert("Data Submission Failed. Try after some Time");
                });
    }
}
function ValidateSuspectDetails() {
    clearAllErrors();
    if (validateTextBox('txtSuspectFirstName', 'All mandatory fields are required') == false) {
        return false;
    }
    if (validateTextBox('txtSuspectLastName', 'All mandatory fields are required') == false) {
        return false;
    }
    if (validateTextBox('txtSuspectMobile', 'All mandatory fields are required') == false) {
        return false;
    }
    var SuspectMobile = $("#txtSuspectMobile").val();
    if (SuspectMobile.length < 10) {
        writeMultipleMessage("error", " Mobile no cannot be less than 10 digits", "txtSuspectMobile");
        return false;
    }
    var EmailID = $("#txtSuspectEmail").val();
    if (EmailID != null) {
        if (EmailValidation('txtSuspectEmail', 'Please enter correct email Id') == false)
            return false;
    }
    return true;
}
function ValidateProspectDetails() {
    if (validateTextBox('txtProspectFirstName', 'All mandatory fields are required') == false) {
        return false;
    }
    if (validateTextBox('txtProspectLastName', 'All mandatory fields are required') == false) {
        return false;
    }
    var DOB = ageCount();
    if (DOB == false) {
        return false;
    }
    var ProspectAge = parseInt($("#txtProspectAge").val());
    if (ProspectAge > 99) {
        writeMultipleMessage("error", "Age Should not be greater than 99", "txtProspectAge");
        return false;
    }
    //if (!ValidateDOB('dpProspectdob')) {
    //    writeMultipleMessage("error", "Please enter valid Date Of Birth. Future date", dpProspectdob);
    //    return false;            
    //}     
    var EmailID = $("#txtProspectEmailId").val();
    if (EmailID != null && EmailID != "") {
        if (EmailValidation('txtProspectEmailId', 'Please enter correct email Id') == false)
            return false;
    }
    if ($("#ddlRecruitmentGender option:selected").text() == "Select" && $("#ddlProspectProfession option:selected").text() == "Select" && $("#ddlProspectIncome option:selected").text() == "Select" && !ValidateDOB('dpProspectdob') && isNaN(ProspectAge)) {
        writeMultipleMessage("error", "Please select atleast one among Date of Birth,Age,Gender,Present Profession and Need for Income to generate prospect code", "dpProspectdob")
        return false;
    }
    else {
        writeMultipleMessage("error", "", "dpProspectdob")
    }
    var ProspectMobile1 = $("#txtProspectMobileNumber").val();
    if (ProspectMobile1.length < 10) {
        writeMultipleMessage("error", "Mobile no cannot be less than 10 digits", "txtProspectMobileNumber");
        return false;
    }
    if (validateTextBox('txtProspectMobileNumber', 'All mandatory fields are required') == false) {
        return false;
    }
    if (validateMobileNo('txtProspectAltMobileNumber', 'Mobile no cannot be less than 10 digits') == false) {
        return false;
    }
    if (validateMobileNo('txtProspectOfficialPhone', 'Mobile no cannot be less than 10 digits') == false) {
        return false;
    }
    if (validateMobileNo('txtProspectResidencePhone', 'Mobile no cannot be less than 10 digits') == false) {
        return false;
    }
    var Profession = $("#ddlProspectProfession option:selected").text();
    var otherProfession = $("#txtProspectSpecifyProfession").val();
    if (Profession == "Others" && (otherProfession == "" || otherProfession == null)) {
        validateTextBox('txtProspectSpecifyProfession', 'All mandatory fields are required');
        return false;
    }
    else {
        validateTextBox('txtProspectSpecifyProfession', '');
    }
    var Income = $("#ddlProspectIncome option:selected").text();
    var otherIncome = $("#txtOthersNeedforIncome").val();
    if (Income == "Others" && (otherIncome == null || otherIncome == "")) {
        validateTextBox('txtOthersNeedforIncome', 'All mandatory fields are required');
        return false;
    }
    else {
        validateTextBox('txtOthersNeedforIncome', '');
    }
    var NICNo = $("#txtProspectNIC").val();
    if (NICNo != null && NICNo != "") {
        if (NICNo.length == 12 || NICNo.length == 10) {
            //writeMultipleMessage("error", "", "txtProspectNIC");
            //var NICID = CheckNICNumberFormat("txtProspectNIC", "dpProspectdob", "ddlRecruitmentGender");
            //if (NICID == false) {
            //    return false;
            //}
        }
        else {
            writeMultipleMessage("error", "SSN Number Should be 12 or 9 Digits", "txtProspectNIC");
            return false;

        }
    }
    else {
        writeMultipleMessage("error", "", "txtProspectNIC");
    }
    //if (validateTextBox('objCommunicationAddressAddress1', 'All mandatory fields are required in Communication Details') == false) {
    //    return false;
    //}
    //if (validateDropDown('objCommunicationAddressddlProvince', 'All mandatory fields are required in Communication Details') == false) {
    //    return false;
    //}

    //if (validateDropDown('objCommunicationAddressddlDistrict', 'All mandatory fields are required in Communication Details') == false) {
    //    return false;
    //}
    //if (validateDropDown('objCommunicationAddressddlCity', 'All mandatory fields are required in Communication Details') == false) {
    //    return false;
    //}
    //if (validateTextBox('objCommunicationAddressPincode', 'All mandatory fields are required in Communication Details') == false) {
    //    return false;
    //}

    return true;
}
function ageCount() {
    debugger;
    var dob = $("#dpProspectdob").val();
    if (dob != '' && !ValidateDOB("dpProspectdob")) {
        writeMultipleMessage("error", "Please Enter Valid Date Of Birth", "dpProspectdob");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "dpProspectdob");
        if (dob != '') {
            var d = new Date();
            var currentYear = d.getFullYear();
            var currentMonth = d.getMonth() + 1;
            var currentDate = d.getDate();
            var arr = dob.split('/');
            var birthYear = arr[2];
            var birthMonth = arr[1];
            var birthdate = arr[0];
            var calYear = currentYear - birthYear;
            var CalMonth = currentMonth - birthMonth;
            var calcAge = 0;
            if (CalMonth < 0 || (CalMonth === 0 && currentDate < birthdate)) {
                calYear--;
            }
            $("#txtProspectAge").val(calYear+1);
            return true;
        }
    }
}
function InterviewrageCount() {
    var dob = $("#dpdob").val();
    if (dob != '' && !ValidateDOB("dpdob")) {
        writeMultipleMessage("error", "Please Enter Valid Date Of Birth", "dpdob");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "dpdob");
        if (dob != '') {
            var d = new Date();
            var currentYear = d.getFullYear();
            var currentMonth = d.getMonth() + 1;
            var currentDate = d.getDate();
            var arr = dob.split('/');
            var birthYear = arr[2];
            var birthMonth = arr[1];
            var birthdate = arr[0];
            var calYear = currentYear - birthYear;
            var CalMonth = currentMonth - birthMonth;
            var calcAge = 0;
            if (CalMonth < 0 || (CalMonth === 0 && currentDate < birthdate)) {
                calYear--;
            }
            $("#hdnAge").val(calYear + 1);
            $("#hdnCurrentAge").val(calYear);
            
            return true;
        }
    }
}
function ValidateDOB(datepickerId) {
    var dob = $('#' + datepickerId).val();
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
    var arrDOB = dob.split('/');
    var birthDay = arrDOB[0];
    var birthMonth = arrDOB[1];
    var birthYear = arrDOB[2];
    var arrCurrentDate = currentDate.split('/');
    var currentDay = arrCurrentDate[0];
    var currentMonth = arrCurrentDate[1];
    var currentYear = arrCurrentDate[2];
    if (birthYear <= currentYear) {
        if (birthMonth <= currentMonth) {
            if (birthDay <= currentDay) {
                return true;
            }
            else if (birthDay > currentDay && birthMonth < currentMonth) {
                return true;
            }
            else if (birthDay > currentDay && birthMonth >= currentMonth && birthYear < currentYear) {
                return true;
            }
            else {
                return false;
            }
        }
        else if (birthMonth > currentMonth && birthYear < currentYear) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}
function ValidateBasicDetails() {
    if (validateDropDown('ddlAgentType', 'All mandatory fields are required in Basic Details') == false) {
        return false;
    }
    if (validateDropDown('ddlRecruitmenttype', 'All mandatory fields are required in Basic Details') == false) {
        return false;
    }
    return true;
}
function SaveAgetonBoardingPersonalDetails() {
    if (!ValidatePersonalDetails()) {

    }
    else {
        //var personalDOB = $("#dpdob").val();
        //$.post("../../AgentonBoarding/SaveAgentonBoarding?PersonalDOB=" + personalDOB, $("#frmAgentRecruitment").serialize())
        $.post("../../AgentonBoarding/SaveAgentonBoarding", $("#frmAgentRecruitment").serialize())
        .done(function (data) {                
                if (data.Message == 'Success') {
                    if (data.IsProspectNICBlocked == true) {
                        var Message = '<p>'+"SSN Number is Under Black List,Prospect Code Moved to Audit/Finance/HRM Departments" + ' </p>';
                        $("#tabCommunicationDetails").hide();
                        $("#tabEducationDetails").hide();
                        $("#tabReferenceDetails").hide();
                        $("#liInterviewDetails").hide();                       
                    }
                    else{
                        var Message = '<p> Data Saved Successfully' + ' </p>';
                        $("#tabCommunicationDetails").show();
                        $("#tabEducationDetails").show();
                        $("#tabReferenceDetails").show();
                        $("#liInterviewDetails").show();
                        FetchInterviewResult();
                        FetchInterviewLevels();
                    }
                }
                else {
                        var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
                    }
                    $('#AgentonBoardingModalBody').html(Message);
                    $("#AgentonBoardingModal").modal("show");
                
            }).
            fail(function () {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
    }
}
function FetchInterviewResult()
{
    var IBSlNo = $("#txtSLIIRegNo").val();
    if (IBSlNo != null && IBSlNo != "") {
        $.post("../../AgentonBoarding/GetRecruitmentInterviewResult?IBSLNo=" + IBSlNo)
         .done(function (data) {
             var Result=data.LstInterviewerResult;
             $('#ddlInterviewLevelResult').html("");
             $('#ddlInterviewLevelResult').append($('<option value=\'\'>Select</option>'));
             $.each(Result, function (ID, option) {
                 $('#ddlInterviewLevelResult').append($('<option value=\'' + option.ID + '\'>' + option.Value + '</option>'));
             });
         }).
         fail(function () {
             BootstrapDialog.alert("Data Submission Failed. Try after some Time");
         });
    }
}
function SaveAgetonBoardingCommunicationDetails() {
    if (!ValidateCommunicationDetails()) {

    }
    else {
        //var personalDOB = $("#dpdob").val();        
        //$.post("../../AgentonBoarding/SaveAgentonBoarding?PersonalDOB=" + personalDOB, $("#frmAgentRecruitment").serialize())
        $.post("../../AgentonBoarding/SaveAgentonBoarding", $("#frmAgentRecruitment").serialize())
        .done(function (data) {
                if (data.Message == 'Success') {
                    var Message = '<p> Data Saved Successfully' + ' </p>';                   
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';                    
                }
                $('#AgentonBoardingModalBody').html(Message);
                $("#AgentonBoardingModal").modal("show");
            }).
            fail(function () {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
    }
}
function ValidatePersonalDetails() {
    if (validateDropDown('ddlRecruitmentSalutation', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    if (validateTextBox('txtPersonalFirstName', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    if (validateTextBox('txtPersonalLastName', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    if (validateTextBox('dpdob', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    if (!ValidateDOB('dpdob')) {
        writeMultipleMessage("error", "Min age of Prospect Should be 18 years", 'dpdob');
        return false;
    }
    //if (validateDropDown('ddlPersonalRecruitmentGender', 'Please Select Gender') == false) {
    //    return false;
    //}
    if (validateTextBox('ddlMaritalStatus', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    if (validateDropDown('ddlNationality', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    if (validateTextBox('txtNICNo', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    //var NICID = CheckNICNumberFormat("txtNICNo", "dpdob", "ddlRecruitmentGender");
    //if (NICID == false) {
    //    return false;
    //}    
    if (validateDropDown('ddlProfession', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    if (validateDropDown('ddlSalesExp', 'All mandatory fields are required in Personal Details') == false) {
        return false;
    }
    var RecruitmenttypeId = $("#ddlRecruitmenttype").val();
    var RecruitmenttypeValue = $("#ddlRecruitmenttype option:selected").text()
    if (RecruitmenttypeId == 2 && RecruitmenttypeValue == 'Industry') {
        if (validateDropDown('ddlIndustryExp', 'All mandatory fields are required in Personal Details') == false) {
            return false;
        }
    }
    InterviewrageCount();
    var Age = parseInt($("#hdnAge").val());
    if (Age < 18 || isNaN(Age)) {
        writeMultipleMessage("error", "Age Should not be less than 18 years to Generate Agent Code", "dpdob");
        return false;
    }
    //if (validateTextBox('txtSLIIRegNo', 'Please select IBSL License Number') == false) {
    //    return false;
    //}
    return true;
}
function ValidateEducationDetails() {    
    if (validateDropDown('ddlHighestQualification', 'All mandatory fields are required in Education Details') == false) {
        return false;
    }
    if (validateTextBox('txtYearMonthPassed', 'All mandatory fields are required in Education Details') == false) {
        return false;
    }
    var DropValue = $("#ddlHighestQualification option:selected").text();
    var otherstext = $("#txtEducationSpecify").val();
    if (DropValue == "Others" && (otherstext == null || otherstext == "")) {
        validateTextBox('txtEducationSpecify', 'Please Provide Highest Qualification')
        return false;
    }
    else {
        validateTextBox('txtEducationSpecify', '');
    }
    //if (validateTextBox('txtSchoolname', 'please provide School Name') == false) {
    //    return false;
    //}
    if (validateTextBox('txtSchoolGrade', 'All mandatory fields are required in Education Details') == false) {
        return false;
    }
    if (validateDropDown('txtSLIIQualification', 'All mandatory fields are required in Education Details') == false) {
        return false;
    }
    var currentdate = new Date();
    var currentYear = currentdate.getFullYear();
    var currentMonth = currentdate.getMonth() + 1;
    var currentDate = currentdate.getDay();
    var PassingYearMonth = $("#txtYearMonthPassed").val();
    var PassYear = PassingYearMonth.substring(0, 4);
    var Slash = PassingYearMonth.substring(5, 4);
    var PassMonth = PassingYearMonth.substring(7, 5);
    var ctDate = new Date(currentYear, currentMonth, 0)
    var passingDate = new Date(PassYear, PassMonth, 0);
    if ((PassYear != '' || PassYear != null) && (Slash != '/' || Slash != null) && (PassMonth != '' || PassMonth != null)) {
        if (passingDate > ctDate) {
            writeMultipleMessage("error", "Year of Passing Cannot be Greater than Current Year and Month", "txtYearMonthPassed");
            return false;
        }
        if (Slash != '/') {
            writeMultipleMessage("error", "Wrong Format. Please Enter Year/Month of Passing in YYYY/MM format.", "txtYearMonthPassed");
            return false;
        }
        if (PassMonth == '' || PassMonth > 12) {
            writeMultipleMessage("error", "Wrong Format. Please Enter Year/Month of Passing in YYYY/MM format.", "txtYearMonthPassed");
            return false;
        }
    }    
    var Mathematics = 0;
    var Language = 0;
    var SubjectFail = 0;
    ValidateMandatorySubjects();
    var Qualification = $("#hdnQualificationValidate").val();
    if (Qualification == "1") {        
        return false;
    }
    var Subjects = $("#hdnSubjectValidate").val();
    if (Subjects == "1") {
        return false;
    }
    var grades = $("#hdnGradevalidate").val();
    if (grades == "1") {
        return false;
    }      
    var Mathematics = $("#hdnMathematicsCount").val();
    var Language = $("#hdnLanguageCount").val();
    var SubjectFail = $("#hdnSubjectFailCount").val();
    if (Mathematics == "4") {

    }
    else {
        writeMultipleMessage("error", "Mathematics, English, History and Science Subjects are compulsory and should not repeat", "jhghj");
        return false;
    }
    if(Language == "1")
    {
       
    }
    else {
        writeMultipleMessage("error", "One language and One mathematic subject is mandatoy", "gg");
        return false;
    }    
    if (SubjectFail == "1") {
        return true;
    }
    return true;
}
function ValidateReferenceDetails() {
    clearAllErrors();
    if ($("#chkIsFirstReferenceDetails").is(":checked")) {
        var res1 = ValidateReferenceoneEmployee();
        if (res1 == false) {
            return false;
        }
    }
    else {
        var res1 = ValidateReferenceoneEmployee();
        if (res1 == false) {
            return false;
        }
    }
    if ($("#chkIsSecondReferenceDetails").is(":checked")) {
        var res1 = ValidateReferenceTwoEmployee();
        if (res1 == false) {
            return false;
        }
    }
    else {
        var res1 = ValidateReferenceTwoEmployee();
        if (res1 == false) {
            return false;
        }
    }
    var NonREF1Code = $("#txtNonRelatedRef1Name").val();
    var NonREF1Occupation = $("#ddlNonRelatedRef1Occupation option:selected").text();
    var NonREF1Mobile = $("#txtNonRelatedRef1MobileNumber").val();
    var NonREF1Address1 = $("#objReferenceOneDetailsAddressAddress1").val();
    var NonREF1Address2 = $("#objReferenceOneDetailsAddressAddress2").val();
    var NonREF1Provience = $("#objReferenceOneDetailsAddressddlProvince option:selected").text();
    var NonREF1District = $("#objReferenceOneDetailsAddressddlDistrict option:selected").text();
    var NonREF1City = $("#objReferenceOneDetailsAddressddlCity option:selected").text();
    var NonREF1Pincode = $("#objReferenceOneDetailsAddressPincode").val();

    var count = 0;
    if ((NonREF1Code != null && NonREF1Code != "") || (NonREF1Occupation != "Select" && NonREF1Occupation != "") || (NonREF1Mobile != null && NonREF1Mobile != "") || (NonREF1Address1 != "Select" && NonREF1Address1 != "") || (NonREF1Address2 != "Select" && NonREF1Address2 != "") || (NonREF1Provience != "Select" && NonREF1Provience != "") || (NonREF1District != "Select" && NonREF1District != "") || (NonREF1City != "Select" && NonREF1City != "") || (NonREF1Pincode != null && NonREF1Pincode != "")) {
        if (NonREF1Code != null && NonREF1Code != "") {
            count++;
        }
        if (NonREF1Occupation != "Select" && NonREF1Occupation != "") {
            count++;
        }

        if (NonREF1Mobile != null && NonREF1Mobile != "") {
            count++;
        }
        if (NonREF1Address1 != null && NonREF1Address1 != "") {
            count++;
        }
        if (NonREF1Address2 != null && NonREF1Address2 != "") {
            count++;
        }
        if (NonREF1Provience != "Select" && NonREF1Provience != "") {
            count++;
        }
        if (NonREF1District != "Select" && NonREF1District != "") {
            count++;
        }
        if (NonREF1City != "Select" && NonREF1City != "") {
            count++;
        }
        if (NonREF1Pincode != null && NonREF1Pincode != "") {
            count++;
        }
        if (count < 9) {
            writeMultipleMessage("error", "All mandatory fields are required in Non Related Employee Reference 1", 'QW')
            return false;
        }
        else {
            writeMultipleMessage("error", "", 'QW')
        }
    }
    else {
        writeMultipleMessage("error", "", 'QW')
    }
    var NonREF2Code = $("#txtNonRelatedRef2Name").val();
    var NonREF2Occupation = $("#ddlNonRelatedRef2Occupation option:selected").text();
    var NonREF2Mobile = $("#txtNonRelatedRef2MobileNumber").val();
    var NonREF2Address1 = $("#objReferenceEmpTwoDetailsAddressAddress1").val();
    var NonREF2Address2 = $("#objReferenceEmpTwoDetailsAddressAddress2").val();
    var NonREF2Provience = $("#objReferenceEmpTwoDetailsAddressddlProvince option:selected").text();
    var NonREF2District = $("#objReferenceEmpTwoDetailsAddressddlDistrict option:selected").text();
    var NonREF2City = $("#objReferenceEmpTwoDetailsAddressddlCity option:selected").text();
    var NonREF2Pincode = $("#objReferenceEmpTwoDetailsAddressPincode").val();

    var Index = 0;
    if ((NonREF2Code != null && NonREF2Code != "") || (NonREF2Occupation != "Select" && NonREF2Occupation != "") || (NonREF2Mobile != null && NonREF2Mobile != "") || (NonREF2Address1 != "Select" && NonREF2Address1 != "") || (NonREF2Address2 != "Select" && NonREF2Address2 != "") || (NonREF2Provience != "Select" && NonREF2Provience != "") || (NonREF2District != "Select" && NonREF2District != "") || (NonREF2City != "Select" && NonREF2City != "") || (NonREF2Pincode != null && NonREF2Pincode != "")) {
        if (NonREF2Code != null && NonREF2Code != "") {
            Index++;
        }
        if (NonREF2Occupation != "Select" && NonREF2Occupation != "") {
            Index++;
        }

        if (NonREF2Mobile != null && NonREF2Mobile != "") {
            Index++;
        }
        if (NonREF2Address1 != null && NonREF2Address1 != "") {
            Index++;
        }
        if (NonREF2Address2 != null && NonREF2Address2 != "") {
            Index++;
        }
        if (NonREF2Provience != "Select" && NonREF2Provience != "") {
            Index++;
        }
        if (NonREF2District != "Select" && NonREF2District != "") {
            Index++;
        }
        if (NonREF2City != "Select" && NonREF2City != "") {
            Index++;
        }
        if (NonREF2Pincode != null && NonREF2Pincode != "") {
            Index++;
        }
        if (Index < 9) {
            writeMultipleMessage("error", "All mandatory fields are required in Non Related Employee Reference 2", 'PO')
            return false;
        }
        else {
            writeMultipleMessage("error", "", 'PO')
        }
    }
    else {
        writeMultipleMessage("error", "", 'PO')
    }
    //if (validateDropDown('ddlEmpRef1Company', 'Please Select Reference1 Company') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlEmpRef1Relationship', 'Please Select Reference1 Relationship') == false) {
    //    return false;
    //}
    //if (validateTextBox('txtEmpRef2Code', 'please provide Reference2 Code') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlEmpRef2Company', 'Please Select Reference2 Company') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlEmpRef2Relationship', 'Please Select Reference2 Relationship') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef1Occupation', 'Please Select Non Related Reference1 Occupation') == false) {
    //    return false;
    //}
    if (validateMobileNo('txtNonRelatedRef1MobileNumber', 'Non Related Reference Mobile No Should be 9 Digits') == false) {
        return false;
    }
    //if (validateTextBox('objReferenceOneDetailsAddressAddress1', 'please provide Non Related Reference1 Address1') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef1Provience', 'Please Select Non Related Reference1 Province') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef1District', 'Please Select Non Related Reference1 District') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef1CityTown', 'Please Select Non Related Reference1 City/Town') == false) {
    //    return false;
    //}
    //if (validateTextBox('txtNonRelatedRef1pincode', 'Please Provide Non Related Reference1 Pincode') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef2Occupation', 'Please Select Non Related Reference2 Occupation') == false) {
    //    return false;
    //}
    if (validateMobileNo('txtNonRelatedRef2MobileNumber', 'Non Related Refernce  Mobile No Should be 9 Digits') == false) {
        return false;
    }
    //if (validateTextBox('objReferenceEmpTwoDetailsAddressAddress1', 'please provide Non Related Reference2 Address1') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef2Provience', 'Please Select Non Related Reference2 Province') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef2District', 'Please Select Non Related Reference2 District') == false) {
    //    return false;
    //}
    //if (validateDropDown('ddlNonRelatedRef2City_Town', 'Please Select Non Related Reference2 City/Town') == false) {
    //    return false;
    //}
    //if (validateTextBox('txtNonRelatedRef2Pincode', 'please provide Non Related Reference2 Pincode') == false) {
    //    return false;
    //}
    var FirstRef = $("#ddlNonRelatedRef1Occupation option:selected").text();
    var FirstRefName = $("#txtOccupationFirstRef").val();
    if (FirstRef == "Others" && (FirstRefName == "" || FirstRefName == null)) {
        validateTextBox('txtOccupationFirstRef', 'Please Enter First Non Reference  Present Profession');
        return false;
    }
    else {
        validateTextBox('txtOccupationFirstRef', '');
    }
    var SecondRef = $("#ddlNonRelatedRef2Occupation option:selected").text();
    var SecondRefName = $("#txtOccupationSecondRef").val();
    if (SecondRef == "Others" && (SecondRefName == "" || SecondRefName == null)) {
        validateTextBox('txtOccupationSecondRef', 'Please Enter Second Non Reference Present Profession');
        return false;
    }
    else {
        validateTextBox('txtOccupationSecondRef', '');
    }
    return true;
}
function ValidateCommunicationDetails() {
    if (validateTextBox('txtMobileNumber', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    var MobileNo = $("#txtMobileNumber").val();
    if (MobileNo.length < 10) {
        writeMultipleMessage("error", "Mobile no cannot be less than 10 digits", "txtMobileNumber");
        return false;
    }
    if (validateMobileNo('txtAltMobileNumber', 'Mobile no cannot be less than 10 digits') == false) {
        return false;
    }
    if (validateMobileNo('txtOfficePhone', 'Mobile no cannot be less than 10 digits') == false) {
        return false;
    }
    if (validateMobileNo('txtResidencePhone', 'Mobile no cannot be less than 10 digits') == false) {
        return false;
    }
    if (validateTextBox('txtEmailId', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    var EmailID = $("#txtEmailId").val();
    if (EmailID != null) {
        if (EmailValidation('txtEmailId', 'Please enter correct email Id') == false)
            return false;
    }
    if (validateTextBox('txtEmergencyFirstName', 'All mandatory fields are required in Communication Details ') == false) {
        return false;
    }
    if (validateTextBox('txtEmergencyLastName', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateTextBox('objEmergencyContactAddressAddress1', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateDropDown('objEmergencyContactAddressddlProvince', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateDropDown('objEmergencyContactAddressddlDistrict', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateDropDown('objEmergencyContactAddressddlCity', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateTextBox('objEmergencyContactAddressPincode', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateTextBox('txtEmergencyMobileNumber', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    var Emergencymobile = $("#txtEmergencyMobileNumber").val();
    if (Emergencymobile.length < 10) {
        writeMultipleMessage("error", "Mobile no cannot be less than 10 digits", "txtEmergencyMobileNumber");
        return false;
    }
    if (validateMobileNo('txtEmergencyAltMobileNumber', 'Mobile no cannot be less than 10 digits') == false) {
        return false;
    }
    if (validateTextBox('txtEmergencyEmailId', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    var PermanentEmailID = $("#txtEmailId").val();
    if (PermanentEmailID != null) {
        if (EmailValidation('txtEmergencyEmailId', 'Please enter correct email Id') == false)
            return false;
    }
    if (validateDropDown('ddlEmergencyRelationship', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    var RelationshipData = $("#ddlEmergencyRelationship option:selected").text();
    var OtherRelationship = $("#txtEmergencyRelationshipOthers").val();
    if (RelationshipData == "Others" && (OtherRelationship == "" || OtherRelationship == null)) {
        validateTextBox('txtEmergencyRelationshipOthers', 'All mandatory fields are required in Communication Details');
        return false;
    }
    else {
        validateTextBox('txtEmergencyRelationshipOthers', '');
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
    //if (validateTextBox('objCommunicationAddressPincode', 'All mandatory fields are required in Communication Details') == false) {
    //    return false;
    //}
    if (validateTextBox('objPermanentAddressAddress1', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateDropDown('objPermanentAddressddlProvince', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }

    if (validateDropDown('objPermanentAddressddlDistrict', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    if (validateDropDown('objPermanentAddressddlCity', 'All mandatory fields are required in Communication Details') == false) {
        return false;
    }
    //if (validateTextBox('objPermanentAddressPincode', 'All mandatory fields are required in Communication Details') == false) {
    //    return false;
    //}
    return true;
}
function ShowIndustryExpMandatory() {
    var RecruitmenttypeId = $("#ddlRecruitmenttype").val();
    var RecruitmenttypeValue = $("#ddlRecruitmenttype option:selected").text()
    if (RecruitmenttypeId == 2 && RecruitmenttypeValue == 'Industry') {

        $("#divIndustryexp").show();
    }
    else {
        //$("#divIndustryexpMandatory").hide();
        $("#divIndustryexp").hide();
    }
    FetchInterviewLevels();
}
function FetchMaritalStatus() {
    var MaritalStatusId = $("#ddlMaritalStatus").val();
    var MaritalStatusValue = $("#ddlMaritalStatus option:selected").text();    
    FetchInterviewLevels();
}
function SaveAgentonBoardingOfficial() {
    if (!ValidateOfficialDetails()) {
    }
    else {
        var Effectivedate = $("#dpReportingEffectiveFrom").val();
        //var personalDOB = $("#dpdob").val();
        //$.post("../../AgentonBoarding/SaveAgentonBoardingOfficial?EffectiveDate=" + Effectivedate + "&PersonalDOB=" + personalDOB, $("#frmAgentRecruitment").serialize())
        $.post("../../AgentonBoarding/SaveAgentonBoardingOfficial?EffectiveDate=" + Effectivedate, $("#frmAgentRecruitment").serialize())
            .done(function (data) {
                if (data.Message == 'Success') {
                    var Message = '<p> Data Saved Successfully' + ' </p>';                    
                }
                else {
                    var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';                   
                }
                $('#AgentonBoardingModalBody').html(Message);
                $("#AgentonBoardingModal").modal("show");
            }).
            fail(function () {
                BootstrapDialog.alert("Data Submission Failed. Try after some Time");
            });
    }
}
function ValidateOfficialDetails() {
    if (validateTextBox('txtPrintName', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    //if (validateDropDown('ddlOfficialCareerLevel', 'Please Select Career Level') == false) {
    //    return false;
    //}
    if (validateTextBox('txtOfficialCareerLevel', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    //if (validateDropDown('ddlOfficialPosition', 'Please Select Interview Position') == false) {
    //    return false;
    //}
    if (validateTextBox('txtOfficialPosition', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    //if (validateDropDown('ddlOfficialDesignation', 'Please Select Interview Designation') == false) {
    //    return false;
    //}
    if (validateTextBox('txtOfficialDesignation', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    if (validateTextBox('txtReportingCode', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    if (validateTextBox('txtOfficialName', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    if (validateTextBox('txtReportingPosition', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    if (validateTextBox('txtReportingDesignation', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    //if (validateTextBox('txtReportingBranchCode', 'All mandatory fields are required in Official Details') == false) {
    //    return false;
    //}
    //if (validateTextBox('txtOfficialBranchName', 'All mandatory fields are required in Official Details') == false) {
    //    return false;
    //}
    if (validateTextBox('txtRegionCode', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    if (validateTextBox('txtRegionName', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    if (validateTextBox('dpReportingEffectiveFrom', 'All mandatory fields are required in Official Details') == false) {
        return false;
    }
    if (!ValidateEffectiveDate()) {
        writeMultipleMessage("error", "Please Enter Valid Date. Date Cannot be Prior to Application Date", 'dpReportingEffectiveFrom');
        return false;
    }
    return true;
}
function FetchInterviewLevels() {
    $.post("../../AgentonBoarding/GetRecruitmentInterviewGuideDetails", $("#frmAgentRecruitment").serialize())
        .done(function (data) {
            $("#divGridInterviewGuidDetails").html(data);
        }).
        fail(function () {
            BootstrapDialog.alert("Data Submission Failed. Try after some Time");
        });
}
function CheckNICNumberFormat(NIC, dob, gender) {
    var error = '';
    if ($('#' + dob).val() == "") {
        writeMultipleMessage("error", "Please Select Date of Birth", dob);
        return false;
    }
    var NICNo = $('#' + NIC).val();
    if (NICNo == "") {
        writeMultipleMessage("error", "Please Enter SSN Number", NIC);
        return false;
    }
    var dob = $('#' + dob).val();
    var id = NICNo.substr(0, 7);
    var id1 = NICNo.substr(0, 5);
    var arr = dob.split('/');
    var year = arr[2];
    var month = arr[1];
    var day = arr[0];
    var idnumber;
    var idnumber1;
    var gender = $("#" + gender + " option:selected").text();
    var dayCount = 0;
    //$.post("../../AgentonBoarding/FetchNoofDays?date=" + dob + "&Year=" + year)
    //    .done(function (data) { 

    //var date1 = new Date("01/01/" + year);
    //var date2 = new Date(dob);
    var date1 = "01/01/" + year;
    //var diffDays = Math.round(Math.abs((date2.getTime() - date1.getTime()) / (1000 * 3600 * 24)));
    var diffDays = daydiff(parseDate(date1), parseDate(dob));
    //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    //var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)); 
    dayCount = parseInt(diffDays + 1);
    var yearType = year % 4;
    if (gender == 'Female') {
        if (yearType == '0') {
            idnumber = year + (dayCount + 499);
            idnumber1 = year.substr(2, 2) + (dayCount + 499);
        } else {
            idnumber = year + (dayCount + 500);
            idnumber1 = year.substr(2, 2) + (dayCount + 500);
        }
    } else {
        if (dayCount < 10) {
            idnumber = year + '00' + dayCount;
            idnumber1 = year.substr(2, 2) + '00' + dayCount;
        }
        else if (dayCount < 100) {
            idnumber = year + '0' + dayCount;
            idnumber1 = year.substr(2, 2) + '0' + dayCount;
        }
        else {
            idnumber = year + dayCount;
            idnumber1 = year.substr(2, 2) + dayCount;
        }
    }
    if (id == idnumber || id1 == idnumber1) {
        writeMultipleMessage("error", "", NIC);
        return true;
    } else {
        writeMultipleMessage("error", "Your SSN Number, Gender or Date of Birth Invalid", NIC);
        return false;
    }
    //}); 

}
function getDayCount(date, Year) {

    var dayCount = 0;
    $.post("../../AgentonBoarding/FetchNoofDays?date=" + date + "&Year=" + Year)
        .done(function (data) {
            dayCount = data;
            return dayCount;
        });
    //    var month = month;
    //    var day = parseInt(day);
    //    if (month == 1) {
    //        dayCount = day;
    //    } else if (month == 2) {
    //        dayCount = 31 + day;
    //    } else if (month == 3) {
    //        dayCount = 59 + day;
    //    } else if (month == 4) {
    //        dayCount = 90 + day;
    //    } else if (month == 5) {
    //        dayCount = 120 + day;
    //    } else if (month == 6) {
    //        dayCount = 151 + day;
    //    } else if (month == 7) {
    //        dayCount = 181 + day;
    //    } else if (month == 8) {
    //        dayCount = 212 + day;
    //    } else if (month == 9) {
    //        dayCount = 243 + day;
    //    } else if (month == 10) {
    //        dayCount = 273 + day;
    //    } else if (month == 11) {
    //        dayCount = 304 + day;
    //    } else if (month == 12) {
    //        dayCount = 335 + day;
    //    }
    //    if ((month > 2)) {
    //        return dayCount + 1;
    //    }
    //    else {
    //        return dayCount;
    //    }
}
function FetchDOBGenderfromNIC(e, dob, gender, Age) {    
    clearAllErrors();
    var NICNo = $('#' + e.id).val();
    if (NICNo != null && NICNo != "") {
        if (NICNo == "") {
            writeMultipleMessage("error", "Please Enter SSN Number", e.id);
            return false;
        }
        if (NICNo.length == 12 || NICNo.length == 10) {
            if (NICNo.length == 12) {
                var nofdays = NICNo.substr(4, 3);
                var dobyears = NICNo.substr(0, 4);
            }
            else if (NICNo.length == 10) {
                var nofdays = NICNo.substr(2, 3);
                var dobyears = NICNo.substr(0, 2);
            }
            $.post("../../AgentonBoarding/FetchDateMonth?NoofDays=" + nofdays + "&Year=" + dobyears)
                .done(function (data) {                    
                    if (data != "Error") {
                        if (dob == "dpdob") {
                            $("#" + dob).attr("readonly", false);
                            $("#" + dob).val(data);
                            $("#" + dob).attr("readonly", true);
                        }
                        else {
                            $("#" + dob).val(data);
                        }
                        if (nofdays <= 366 && nofdays >= 0) {
                            $("#" + gender).val(15);
                        }
                        else if (nofdays > 500 && nofdays <= 866) {
                            $("#" + gender).val(16);
                        }
                        if (dob != "dpdob") {
                            ageCount();
                        }
                        else {
                            InterviewrageCount();
                        }
                        writeMultipleMessage("error", "", e.id);
                        return true;
                    }
                    else {
                        writeMultipleMessage("error", "Your SSN Number, Gender or Date of Birth Invalid", e.id);
                        return false;
                    }
                });
        }
        else {
            writeMultipleMessage("error", "SSN Number should be 12 or 9 Digits", e.id);
            return false;
        }
    }
}

function parseDate(str) {
    var mdy = str.split('/')
    return new Date(mdy[2], mdy[1], mdy[0] - 1);
}

function daydiff(first, second) {
    return (second - first) / (1000 * 60 * 60 * 24);
}
function NICNumberDuplicateCheckingLogic(NIC) {
    var NICNos = $("#" + NIC).val();
    if (NICNos != null && NICNos != "") {
        $.ajax({
            url: '../../AgentonBoarding/CheckingNICNumberExist',
            type: "POST",
            data: { NICNo: NICNos },
            success: function (data) {
                if (data == "success") {
                    writeMultipleMessage("error", "", NIC);
                    return true;
                }
                else {
                    writeMultipleMessage("error", "SSN Number already exist,you cannot create duplicate prospect code", NIC);
                    return false;
                }
            },
            fail: function (data) {
                return false;
            }
        });
    }
    else {
        return true;
    }
}
function ValidateTrainingEndDate() {
    var StartDate = $('#dpTrainingStartDate').val();
    var EndDate = $('#dpTrainingEndDate').val();
    var arrStartDate = StartDate.split('/');
    var StartDay = arrStartDate[0];
    var StartMonth = arrStartDate[1];
    var StartYear = arrStartDate[2];
    var arrEndDate = EndDate.split('/');
    var EndDay = arrEndDate[0];
    var EndMonth = arrEndDate[1];
    var EndYear = arrEndDate[2];
    var year = EndYear - StartYear;
    var Month = EndMonth - StartMonth;
    var startDate = new Date(StartYear, StartMonth, StartDay);
    var endDate = new Date(EndYear, EndMonth, EndDay);
    if (endDate < startDate) {
        return false;
    }
    return true;
}


function ValidateTrainingDetails() {    
    if (validateDropDown('ddlTrainingMode', 'Please Select Training Mode') == false) {
        return false;
    }
    //if (validateTextBox('dpTrainingStartDate', 'Please Enter Start Date') == false) {
    //    return false;
    //}
    var SDate = $("#dpTrainingStartDate").val();
    if (SDate == '' || SDate == null) {
        writeMultipleMessage("error", "Please Select Training Start Date", 'dpTrainingStartDate');
        return false;
    }
    var EDate = $("#dpTrainingEndDate").val();
    if (EDate == '' || EDate == null) {
        writeMultipleMessage("error", "Please Select Training End Date", 'dpTrainingEndDate');
        return false;
    }
    if (!ValidateTrainingEndDate()) {
        writeMultipleMessage("error", "Please Select Valid Training End Date,It Should Not Be Less Than Training Start Date", 'dpTrainingEndDate');
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'dpTrainingEndDate');
    }
    if (validateTextBox('txtTrainingPlace', 'Please Provide Place') == false) {
        return false;
    }
    if (validateTextBox('txtTrainingCandidateNo', 'PleaseProvide Candidate Number') == false) {
        return false;
    }
    if (validateDropDown('ddlTraningStatus', 'Please Select Status') == false) {
        return false;
    }
    return true;
}
//function ValidateEffectiveDate() {

//    var ApplicationDate = $('#dpApplicationDate').val();
//    var EffectiveFrom = $('#dpReportingEffectiveFrom').val();
//    var arrApplicationDate = ApplicationDate.split('/');
//    var ApplicationDay = arrApplicationDate[0];
//    var ApplicationMonth = arrApplicationDate[1];
//    var ApplicationYear = arrApplicationDate[2];
//    var arrEffectiveFromDate = EffectiveFrom.split('/');
//    var EffectiveDay = arrEffectiveFromDate[0];
//    var EffectiveMonth = arrEffectiveFromDate[1];
//    var EffectiveYear = arrEffectiveFromDate[2];
//    if (EffectiveYear >= ApplicationYear) {
//        if (EffectiveMonth >= ApplicationMonth) {
//            if (EffectiveDay >= ApplicationDay) {
//                return true;
//            }
//            else if (EffectiveMonth < ApplicationMonth && EffectiveYear > ApplicationYear) {
//            return true;
//        }
//            else if (EffectiveDay < ApplicationDay && EffectiveYear > ApplicationYear) {
//            return true;
//        }
//        else {
//            return false;
//        }

//        }


//    }

//}
function ValidateEffectiveDate() {
    var ApplicationDate = $('#dpApplicationDate').val();
    var EffectiveFromDate = $('#dpReportingEffectiveFrom').val();
    var arrApplicationDate = ApplicationDate.split('/');
    var ApplicationDay = arrApplicationDate[0];
    var ApplicationMonth = arrApplicationDate[1];
    var ApplicationYear = arrApplicationDate[2];
    var arrEffectiveFromDate = EffectiveFromDate.split('/');
    var EffectiveDay = arrEffectiveFromDate[0];
    var EffectiveMonth = arrEffectiveFromDate[1];
    var EffectiveYear = arrEffectiveFromDate[2];
    var year = EffectiveYear - ApplicationYear;
    var Month = EffectiveMonth - ApplicationMonth;
    var ApplicationDate = new Date(ApplicationYear, ApplicationMonth, ApplicationDay);
    var effectiveFromDate = new Date(EffectiveYear, EffectiveMonth, EffectiveDay);
    if (effectiveFromDate < ApplicationDate) {
        return false;
    }
    return true;
}

function ValidateReferenceoneEmployee() {
    var REF1Code = $("#txtEmpRef1Code").val();
    var REF1Name = $("#txtEmpRef1Name").val();
    var REF1Company = $("#txtFirstRef1Company").val();
    var REF1Designation = $("#txtEmpRef1Designation").val();
    var REF1Relationship = $("#ddlEmpRef1Relationship option:selected").text();
    var count = 0;
    if ($("#chkIsFirstReferenceDetails").is(":checked")) {
        if ((REF1Code == null || REF1Code == "") || (REF1Name == null || REF1Name == "") || (REF1Company == null || REF1Company == "") || (REF1Designation == null || REF1Designation == "") || (REF1Relationship == "select" || REF1Relationship == "")) {
            writeMultipleMessage("error", "All mandatory fields are required in Employee Reference 1", 'HG')
            return false;
        }
        else {
            writeMultipleMessage("error", "", 'HG')
        }
    }
    else {
        if ((REF1Code != null && REF1Code != "") || (REF1Name != null && REF1Name != "") || (REF1Company != null && REF1Company != "") || (REF1Designation != null && REF1Designation != "") || (REF1Relationship != "select" && REF1Relationship != "")) {
            if (REF1Code != null && REF1Code != "") {
                count++;
            }
            if (REF1Name != null && REF1Name != "") {
                count++;
            }
            if (REF1Company != null && REF1Company != "") {
                count++;
            }
            if (REF1Designation != null && REF1Designation != "") {
                count++;
            }
            if (REF1Relationship != "select" && REF1Relationship != "") {
                count++;
            }
            if (count < 5) {
                writeMultipleMessage("error", "All mandatory fields are required in Employee Reference 1", 'HG')
                return false;
            }
            else {
                writeMultipleMessage("error", "", 'HG')
            }
        }
        else {
            writeMultipleMessage("error", "", 'HG')
        }
    }
    return true;
}
function ValidateReferenceTwoEmployee() {
    var REF2Code = $("#txtEmpRef2Code").val();
    var REF2Name = $("#txtEmpRef2Name").val();
    var REF2Company = $("#txtEmpRef2Company").val();
    var REF2Designation = $("#txtEmpRef2Designation").val();
    var REF2Relationship = $("#ddlEmpRef2Relationship option:selected").text();
    var count = 0;
    if ($("#chkIsSecondReferenceDetails").is(":checked")) {
        if ((REF2Code == null || REF2Code == "") || (REF2Name == null || REF2Name == "") || (REF2Company == null || REF2Company == "") || (REF2Designation == null || REF2Designation == "") || (REF2Relationship == "select" || REF2Relationship == "")) {
            writeMultipleMessage("error", "All mandatory fields are required in Employee Reference Two", 'AS')
            return false;
        }
        else {
            writeMultipleMessage("error", "", 'AS')
        }
    }
    else {
        if ((REF2Code != null && REF2Code != "") || (REF2Name != null && REF2Name != "") || (REF2Company != null && REF2Company != "") || (REF2Designation != null && REF2Designation != "") || (REF2Relationship != "select" && REF2Relationship != "")) {
            if (REF2Code != null && REF2Code != "") {
                count++;
            }
            if (REF2Name != null && REF2Name != "") {
                count++;
            }
            if (REF2Company != null && REF2Company != "") {
                count++;
            }
            if (REF2Designation != null && REF2Designation != "") {
                count++;
            }
            if (REF2Relationship != "select" && REF2Relationship != "") {
                count++;
            }
            if (count < 5) {
                writeMultipleMessage("error", "All mandatory fields are required in Employee Reference 2", 'AS')
                return false;
            }
            else {
                writeMultipleMessage("error", "", 'AS')
            }
        }
    }
    return true;
}

function SaveAuditClearanceDetails() {
    if (!ValidateAuditClearenceDetails()) {

    }
    else {
        $.post("../../AgentonBoarding/SaveAuditDepartmentClearance", $("#frmAgentNICVerification").serialize())
           .done(function (data) {
               if (data.Message == 'Success') {
                   var ProspectCode = $("#hdnProspectCode").val();
                   var Message = '<p>Audit Clearance for ' + ProspectCode + " is Success" + ' </p>';
               }
               else {
                   var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
               }
               $('#AuditClearanceModalBody').html(Message);
               $("#AuditClearanceModal").modal("show");
           }).
           fail(function () {
               BootstrapDialog.alert("Data Submission Failed. Try after some Time");
           });
    }
}
function SaveHRMClearanceDetails() {
    if (!ValidateHRMDDetails()) {

    }
    else {
        $.post("../../AgentonBoarding/SaveHRMDepartmentClearance", $("#frmAgentNICVerification").serialize())
           .done(function (data) {
               if (data.Message == 'Success') {
                   var ProspectCode = $("#hdnProspectCode").val();
                   var Message = '<p>HRM Clearance for ' + ProspectCode + " is Success" + ' </p>';
               }
               else {
                   var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
               }
               $('#HRMClearanceModalBody').html(Message);
               $("#HRMClearanceModal").modal("show");
           }).
           fail(function () {
               BootstrapDialog.alert("Data Submission Failed. Try after some Time");
           });
    }
}
function SaveFinanceClearanceDetails() {
    var Grid = [];
    getFinanceDepartment(Grid);
    $.post("../../AgentonBoarding/SaveFinanceDepartmentClearance", $("#frmAgentNICVerification").serialize())
       .done(function (data) {
           if (data.Message == 'Success') {
               var ProspectCode = $("#hdnProspectCode").val();
               var Message = '<p>Finance Clearance for ' + ProspectCode + " is Success" + ' </p>';             
           }
           else {
               var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';               
           }
           $('#FinanaceClearanceModalBody').html(Message);
           $("#FinanaceClearanceModal").modal("show");
       }).
       fail(function () {
           BootstrapDialog.alert("Data Submission Failed. Try after some Time");
       });
}
function SubmitNICVerification() {
    $.post("../../AgentonBoarding/SubmitNICVerification", $("#frmAgentNICVerification").serialize())
        .done(function (data) {
            if (data.Message == 'Success') {
                var ProspectCode = $("#hdnProspectCode").val();
                var Message = '<p>NIC Verification for Prospect ' + ProspectCode + " is Success" + ' </p>';
            }
            else {
                var Message = '<p> Data Submission Failed. Try after some Time' + ' </p>';
            }
            $('#FinanaceClearanceModalBody').html(Message);
            $("#FinanaceClearanceModal").modal("show");
        }).
        fail(function () {
            BootstrapDialog.alert("Data Submission Failed. Try after some Time");
        });
}

function ValidateAuditClearenceDetails() {
    if (validateTextBox('txtAuditTarget', 'All mandatory fields are required in Audit Clearence Details') == false) {
        return false;
    }
    if (validateTextBox('txtAuditAchievement', 'All mandatory fields are required in Audit Clearence Details') == false) {
        return false;
    }
    if (validateTextBox('txtAuditVariance', 'All mandatory fields are required in Audit Clearence Details') == false) {
        return false;
    }
    if (validateTextBox('txtAuditRemarks', 'All mandatory fields are required in Audit Clearence Details') == false) {
        return false;
    }
    return true;
}

function ValidateHRMDDetails() {
    if (validateTextBox('txtHRMNICNo', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('txtHRMName', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('txtHRMCode', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('txtHRMEPFNo', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('dpHRMAppointment', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
     }
    if (validateTextBox('dpHRMResignation', 'All mandatory fields are required in HRMD Department') == false) {
         return false;
    }
    if (validateTextBox('txtHRMResonforLeaving', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('dpHRMPeriodofEmployement', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('txtHRMPreviousSalary', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('txtHRMPreviousTravel', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    if (validateTextBox('txtHRMRecomendation', 'All mandatory fields are required in HRMD Department') == false) {
        return false;
    }
    return true;
}
function ClearErrOnChange() {
    clearAllErrors();
}
