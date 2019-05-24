
function ValidateAssuredMemberGrid(index) {
    var RelationWithProposer = $('#ddlRelationWithProposer' + index).val();
    var Message = "All mandatory fields are required for 'Life to be Assured' under Row " + (index + 1) + "<br />Please edit member details for missed fields.";

    if ($('#ddlRelationWithProposer' + index).val() == "0" || $('#ddlRelationWithProposer' + index).val() == "") {
        writeMultipleMessage("error", Message, "ddlRelationWithProposer" + index + "");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlRelationWithProposer" + index + "");
    }

    if ($("#dtDOB" + index + "").val() == "") {
        writeMultipleMessage("error", Message, "dtDOB" + index + "");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "dtDOB" + index + "");
    }
    if ($("#ddlGender" + index + "").val() == "") {
        writeMultipleMessage("error", Message, "ddlGender" + index + "");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlGender" + index + "");
    }

    if (RelationWithProposer != "269" && RelationWithProposer != "270") {

        if ($("#txtNewNicNo" + index + "").val() == "") {
            writeMultipleMessage("error", Message, "txtNewNicNo" + index + "");
            $("#checkboxDeclaration").prop("checked", false);
            return false;
        }
        else {
            writeMultipleMessage("error", "", "txtNewNicNo" + index + "");
        }


        if ($("#txtComAddress1" + index + "").val() == "") {
            writeMultipleMessage("error", Message, "txtComAddress1" + index + "");
            $("#checkboxDeclaration").prop("checked", false);
            return false;
        }
        else {
            writeMultipleMessage("error", "", "txtComAddress1" + index + "");
        }

        if ($("#txtComPincode" + index + "").val() == "") {
            writeMultipleMessage("error", Message, "txtComPincode" + index + "");
            $("#checkboxDeclaration").prop("checked", false);
            return false;
        }
        else {
            writeMultipleMessage("error", "", "txtComPincode" + index + "");
        }

        var pincodevalue = $("#txtComPincode" + index + "").val();
        if (pincodevalue != "YYYYY|") {
            if ($("#txtComDistrict" + index + "").val() == "") {
                writeMultipleMessage("error", Message, "txtComDistrict" + index + "");
                $("#checkboxDeclaration").prop("checked", false);
                return false;
            }
            else {
                writeMultipleMessage("error", "", "txtComDistrict" + index + "");
            }
        }
    }

    if ($("#ddlNationality" + index + "").val() == "") {
        writeMultipleMessage("error", Message, "ddlNationality" + index + "");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlNationality" + index + "");
        return true;
    }
    //if ($("#ddlNationality option:selected").text() == "Sri lanka") {
    //    if ($("#txtNewNicNo").val() == "") {
    //        writeMultipleMessage("error", "Please Enter New NIC Number.", "txtNewNicNo");
    //        return false;
    //    }
    //    else {
    //        writeMultipleMessage("error", "", "txtNewNicNo");
    //    }
    //}

    if ($("#ddlResidentialStatus" + index + "").val() == "") {
        writeMultipleMessage("error", Message, "ddlResidentialStatus" + index + "");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlResidentialStatus" + index + "");
        return true;
    }

    if ($("#ddlCountryofOccupation" + index + "").val() == "") {
        writeMultipleMessage("error", Message, "ddlCountryofOccupation" + index + "");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlCountryofOccupation" + index + "");
        return true;
    }


}

function lifestylequestionCheck(MI) {

    var questionalert = 0, questionalerttxt = 0, questionalertSelect = 0, subques = 0, subquesSelect = 0, tabid;

    $('#fromProposalPage').children().find('.lifestyle-questions').each(function () {

        if ($(this).children().find("input:radio[class='med-radio-button']:checked").length == 0) {

            questionalert = 1;
        }
        else {
            questionalert = 0;
        }

        $(this).siblings('.lifestyle-subquestion-txt').each(function () {

            if ($(this).css('display') != 'none') {

                var txt = $(this).children().find('input:text');

                if (txt.val() != null && txt.val() != '') {
                    subques = 0;
                }
                else {
                    subques = 1;
                }
            }
        })


    })

    if (questionalert == 1) {

        writeMultipleMessage("error", "All Life Style Questions and specific answers are mandatory", 'LifeStyleQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'LifeStyleQuestions');
    }
    if (subques == 1) {

        writeMultipleMessage("error", "All Life Style Questions and specific answers are mandatory", 'LifeStyleQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'LifeStyleQuestions');
    }

    if (subquesSelect == 1) {

        writeMultipleMessage("error", "All Life Style Questions and specific answers are mandatory", 'LifeStyleQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'LifeStyleQuestions');
    }
    return true;
}

function medquestionCheck(MI) {

    var questionalert = 0, questionalerttxt = 0, subques = 0, subquesSelect = 0, tabid;

    $('#fromProposalPage').children().find('.medical-questions').each(function () {

        if ($(this).children().find("input:radio[class='medical-radio-button']:checked").length == 0) {

            var Questions = $(this).children().find('input:text').val();
            if (Questions == "" && Questions != undefined) {
                questionalert++;
            }
            var Drop = $(this).children().find('select').val();
            if (Drop == "" || Drop == "Select") {
                questionalert++;
            }

            if (Questions == undefined && Drop == undefined) {
                questionalert++;
            }
        }

        $(this).siblings('.med-subquestion-txt').each(function () {
            if ($(this).css('display') != 'none') {
                var TextData = $(this).children().find('input:text').val();
                if ((TextData != null && TextData != '') || TextData == undefined) {

                }
                else {

                    subques++;
                }


                var Child = $(this).children().find('select').val();
                if (Child == "" && Child == "Select") {

                    subquesSelect++;
                }

            }
        })
    })

    if (questionalert > 0) {
        writeMultipleMessage("error", "All Medical History Questions and specific answers are mandatory", 'MedicalHistrory');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'MedicalHistrory');
    }

    if (subques > 0) {
        writeMultipleMessage("error", "All Medical History Questions and specific answers are mandatory1", 'MedicalHistrory');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'MedicalHistrory');
    }

    if (subquesSelect > 0) {
        writeMultipleMessage("error", "All Medical History Questions and specific answers are mandatory2", 'MedicalHistrory');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'MedicalHistrory');
    }
    return true;
}

function addquestionCheck(MI) {
    var DivID = '#tab_6_' + MI;
    var AssuredName = $('#hdnAssuredName' + MI).val()
    var questionalert = 0, questionalerttxt = 0, subques = 0, mainquesSelect = 0, mainques = 0, subquesSelect = 0, mainquestion = 0, mainquesSelect = 0, tabid;
    var checkboxadditional = 0;
    var dateAdditional = 0;
    $('#fromProposalPage').children().find('.additional-questions').each(function () {



        if ($(this).children().find("input:radio[class='add-radio-button']:checked").length == 0) {

            var Questions = $(this).children().find('input:text').val();
            if (Questions == "" && Questions != undefined) {
                var lablename = $(this).children().find('.question-text').text();
                //added by ramana
                if ($(this).attr("id").indexOf("1195") > 0 && lablename == " Other (please specify)") {
                    checkboxadditional++;
                }
                else {
                    questionalert++;
                }
                //end
            }
            var Drop = $(this).find('select').val();
            if (Drop == "" && Drop == "Select") {

                questionalert++;
            }
            //if (Questions == undefined && Drop == undefined) {
            //    questionalert++;
            //}
        }

        //added by ramana
        $(this).find('.chkadd-check-button').each(function () {

            if ($("#" + $(this).attr("id")).is(":checked") == false) {
                checkboxadditional++;
            }
        });
        //end
        $(this).find('.additional-questions').each(function () {

            if ($(this).css('display') != 'none') {

                $(this).children().find('input:text').each(function () {

                    if (($(this).val() != null && $(this).val() != '') || $(this).val() == undefined) {

                    }
                    else {
                        mainques++;
                    }
                });
                $(this).children().find('select').each(function () {

                    if ($(this).val() == "" || $(this).val() == "Select") {

                        mainquesSelect++;
                    }
                })

            }
        })
        $(this).siblings('.additional-subquestion-txt').each(function () {
            if ($(this).css('display') != 'none') {
                var ADDData = $(this).children().find('input:text').val();
                if ((ADDData != null && ADDData != '') || ADDData == undefined) {

                }
                else {
                    subques++;
                }

                var ADDChild = $(this).children().find('select').val();
                if (ADDChild == "" && ADDChild == "Select") {

                    subquesSelect++;
                }
            }
        });
        var lablename = $(this).children().find('.question-text').text();
        $(this).find('.datepicker').each(function () {

            if (lablename == " Date of Issue:") {
                if (($(this).val() != null && $(this).val() != '') || $(this).val() == undefined) {
                    var CurrentDate = new Date();
                    var currentYear = CurrentDate.getFullYear();
                    var currentMonth = CurrentDate.getMonth() + 1;
                    var currentDate = CurrentDate.getDate();
                    var date = currentDate + "/" + currentMonth + "/" + currentYear
                    if ($(this).val() > date) {
                        dateAdditional++;

                    }
                }

            }
        });
    });
    if (checkboxadditional == 5) {
        writeMultipleMessage("error", "All Additional Questions and specific answers are mandatory ", 'AdditionalQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'AdditionalQuestions');
    }
    if (dateAdditional > 0) {
        writeMultipleMessage("error", "Date of issue should not be greater than current date in Additional Questions", 'AdditionalQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'AdditionalQuestions');
    }


    if (questionalert > 0) {

        writeMultipleMessage("error", "All Additional Questions and specific answers are mandatory 1  ", 'AdditionalQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'AdditionalQuestions');
    }


    if (subques > 0) {

        writeMultipleMessage("error", "All Additional Questions and specific answers are mandatory 2  ", 'AdditionalQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'AdditionalQuestions');
    }
    if (subquesSelect > 0) {

        writeMultipleMessage("error", "All Additional Questions and specific answers are mandatory 3  ", 'AdditionalQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'AdditionalQuestions');
    }
    if (mainques > 0) {

        writeMultipleMessage("error", "All Additional Questions and specific answers are mandatory 4 ", 'AdditionalQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'AdditionalQuestions');
    }
    if (mainquesSelect > 0) {

        writeMultipleMessage("error", "All Additional Questions and specific answers are mandatory 5 ", 'AdditionalQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'AdditionalQuestions');
    }
    return true;

}

function ValidateFamiyHistoryDetails(Count) {
    var questionalert = 0, questionalerttxt = 0, subques = 0, subquesSelect = 0, tabid;
    $('#fromProposalPage').children().find('.family-questions').each(function () {
        if ($(this).children().find("input:radio[class='med-radio-button']:checked").length == 0) {

            questionalert = 1;
        }
        else {
            questionalert = 0;
        }

        $(this).siblings('.family-subquestion-txt').each(function () {

            if ($(this).css('display') != 'none') {

                var txt = $(this).children().find('input:text');

                if (txt.val() != null && txt.val() != '') {
                    subques = 0;
                }
                else {
                    subques = 1;
                }
            }
        })
        //if ($(this).children().find("input:radio[class='med-radio-button']:checked").val() == "true") {            
        //    Count = (Count - 1).toString();
        //    if ($('#DivFamilyBackGroundHistoryDetails' + Count+ ' tr.grid-empty-text').length > 0) {
        //        $('#DivFamilyBackGroundHistoryDetails' + Count+ ' tr.grid-empty-text').remove();
        //    }
        //    var length = $("#DivFamilyBackGroundHistoryDetails" + Count + " tbody tr").length;
        //    if (length > 0) {
        //        $("#DivFamilyBackGroundHistoryDetails" + Count + " tbody").find("tr").each(function (index) {                    
        //            var grid = $(this).children().find('input:text').val();
        //            if (grid == "") {
        //                subquesSelect++;
        //            }
        //            else {
        //                subquesSelect = 0;
        //            }
        //        });
        //    }
        //    else {
        //        subquesSelect++;
        //    }
        //}
        $(this).siblings('.family-subquestion-select').each(function () {
            if ($(this).css('display') != 'none') {
                if ($(this).children().find('select').val() == "") {
                    subquesSelect = 1;
                }
                else {
                    subquesSelect = 0;
                }
            }
        })
    })


    if (questionalert == 1) {

        writeMultipleMessage("error", "All Family  Questions and specific answers are mandatory", 'FamilyQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'FamilyQuestions');
    }
    if (subques == 1) {

        writeMultipleMessage("error", "All Family Questions and specific answers are mandatory", 'FamilyQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'FamilyQuestions');
    }

    if (subquesSelect == 1) {

        writeMultipleMessage("error", "All Family Questions and specific answers are mandatory", 'FamilyQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'FamilyQuestions');
    }
    return true;
}

function ValidateInsuranceDetails(MI) {

    var questionalert = 0, questionalerttxt = 0, subques = 0, subquesSelect = 0, mainques = 0, mainquesSelect = 0, tabid;

    $('#fromProposalPage').children().find('.Insurance-questions').each(function () {
        if ($(this).children().find("input:radio[class='med-radio-button']:checked").length == 0) {

            var Questions = $(this).children().find('input:text').val();
            if (Questions == "" && Questions != undefined) {

                questionalert++;
            }
            var Drop = $(this).children().find('select').val();
            if (Drop == "" || Drop == "Select") {
                questionalert++;
            }

            if (Questions == undefined && Drop == undefined) {
                questionalert++;
            }

        }
        $(this).find('.Insurance-questions').each(function () {
            if ($(this).css('display') != 'none') {
                $(this).children().find('input:text').each(function () {
                    if (($(this).val() != null && $(this).val() != '') || $(this).val() == undefined) {

                    }
                    else {
                        mainques++;
                    }
                });
                $(this).children().find('select').each(function () {
                    if ($(this).val() == "" || $(this).val() == "Select") {
                        mainquesSelect++;
                    }
                })

            }
        })

        $(this).siblings('.Insurance-subquestion-txt').each(function () {

            if ($(this).css('display') != 'none') {
                var txt = $(this).children().find('input:text');
                for (var t = 0; t < txt.length; t++) {
                    //    if (txt[t].innerText != '') {
                    //        subques = 0;
                    //    }
                    //    else {
                    //        subques = 1;
                    //    }
                    var displayFlag = true;
                    if (txt[t].parentElement.tagName == "TD") {
                        if (txt[t].parentElement.parentElement.style.display == "none")
                            displayFlag = false;
                    }
                    else {
                        if (txt[t].style.display == "none")
                            displayFlag = false;
                    }
                    if (txt[t].value == '' && displayFlag) {
                        var ADDData = $(this).children().find('input:text').val();
                        if ((ADDData != null && ADDData != '') || ADDData == undefined) {

                        }
                        else {
                            subques++;
                        }
                    }
                }


                //var ADDData = $(this).children().find('input:text').val();
                //if ((ADDData != null && ADDData != '') || ADDData == undefined) {

                //}
                //else {
                //    subques++;
                //}

                //var ADDChild = $(this).children().find('select').val();
                //if (ADDChild == "" && ADDChild == "Select") {

                //    subquesSelect++;
                //}

                //$(this).children().find('input:text').each(function () {
                //    if (($(this).val() != null && $(this).val() != '') || $(this).val() == undefined) {

                //    }
                //    else {
                //        subques++;
                //    }
                //});
                //$(this).children().find('select').each(function () {
                //    if ($(this).val() == "" || $(this).val() == "Select") {

                //    }
                //    else {
                //        subquesSelect++;
                //    }

                //})

            }
        })

    })

    if (questionalert > 0) {

        writeMultipleMessage("error", "All Insurance  Questions and specific answers are mandatory", 'InsuranceQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'InsuranceQuestions');
    }
    if (subques > 0) {

        writeMultipleMessage("error", "All Insurance Questions and specific answers are mandatory", 'InsuranceQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'InsuranceQuestions');
    }


    if (subquesSelect > 0) {

        writeMultipleMessage("error", "All Insurance Questions and specific answers are mandatory", 'InsuranceQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'InsuranceQuestions');
    }

    if (mainques > 0) {

        writeMultipleMessage("error", "All Insurance Questions and specific answers are mandatory", 'InsuranceQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'InsuranceQuestions');
    }
    if (mainquesSelect > 0) {

        writeMultipleMessage("error", "All Insurance Questions and specific answers are mandatory", 'InsuranceQuestions');
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", 'InsuranceQuestions');
    }
    return true;
}

function validateNomineeDetails() {
 debugger
    var index = 0;
    if ($('#divNomineeDetailsGrid tr.grid-empty-text').length > 0) {
        $('#divNomineeDetailsGrid tr.grid-empty-text').remove();
    }
    var NoOfRows = $("#divNomineeDetailsGrid  tbody tr").length;
    var isdeleted = "";
    var Percentage = 0;
    var total = 0;
    var IsValid = true;
    var LoadingArray = [];
    //$('#fromProposalPage').children().find('.divNomineeDetailsGrid').each(function () {
    //    $(this).find
    //})
 
    if (NoOfRows > 0) {
        var count = 0;
        //$("#divNomineeDetailsGrid   tbody").find("tr").each(function (index) {
        for (var i = 0; i < NoOfRows; i++) {
           
            var NomineeIsDeleted = $('#NomineeIsDeleted' + index).val();
            if (NomineeIsDeleted == "False" || NomineeIsDeleted == "") {
                for (var j = 0; j < NoOfRows; j++) {
                    if (i != j) {
                        if ($("#DDLNomineeRelationship" + i).val() == $("#DDLNomineeRelationship" + j).val() &&
                            $("#NomineeName" + i).val().toUpperCase() == $("#NomineeName" + j).val().toUpperCase() &&
                            $("#NomineeNICNo" + i).val() == $("#NomineeNICNo" + j).val())
                        {
                            count++;

                        }

                    }

                }
            

                if ($("#DDLNomineeRelationship" + index + " option:selected").text() == "Select" || $("#DDLNomineeRelationship" + index + " option:selected").text() == "" || $("#DDLNomineeRelationship" + index).val() == "") {

                    writeMultipleMessage("error", "Please Enter Nominee Relationship.", "DDLNomineeRelationship" + index);
                    $("#checkboxDeclaration").prop("checked", false);
                    IsValid = false;
                }
                else if ($("#DDLNomineeSalutation" + index + " option:selected").text() == "Select" || $("#DDLNomineeSalutation" + index + " option:selected").text() == "" || $("#DDLNomineeSalutation" + index).val() == "") {
                    writeMultipleMessage("error", " Please Enter Nominee  Salutation.", "DDLNomineeSalutation" + index);
                    $("#checkboxDeclaration").prop("checked", false);
                    IsValid = false;
                }
                else if ($("#NomineeName" + index).val() == "" || $("#NomineeName" + index).val() == null) {
                    writeMultipleMessage("error", "Please Enter Nominee Given Name.", "NomineeName" + index);
                    $("#checkboxDeclaration").prop("checked", false);
                    IsValid = false;
                }
                else if ($("#NomineeSurname" + index).val() == "" || $("#NomineeSurname" + index).val() == null) {
                    writeMultipleMessage("error", "Please Enter Nominee Sur Name.", "NomineeSurname" + index);
                    $("#checkboxDeclaration").prop("checked", false);
                    IsValid = false;
                }
                    //else if ($("#NomineeNICNo" + index).val() == "" || $("#NomineeNICNo" + index).val() == null) {
                    //    writeMultipleMessage("error", "All fields are required in Nominee Beneficiary Details.", "NomineeNICNo" + index);
                    //    IsValid = false;
                    //}
                else if ($("#NomineeDOB" + index).val() == "" || $("#NomineeDOB" + index).val() == null) {
                    writeMultipleMessage("error", "Please Enter Nominee Date Of Birth.", "NomineeDOB" + index);
                    $("#checkboxDeclaration").prop("checked", false);
                    IsValid = false;
                }
                else if ($("#DDLNomineeGender" + index + " option:selected").text() == "Select" || $("#DDLNomineeGender" + index + " option:selected").text() == "" || $("#DDLNomineeGender" + index).val() == "") {
                    writeMultipleMessage("error", " Please Enter Nominee Gender.", "DDLNomineeSalutation" + index);
                    $("#checkboxDeclaration").prop("checked", false);
                    IsValid = false;
                }
                else if ($("#ddlNomineeMaritalStatus" + index + " option:selected").text() == "Select" || $("#ddlNomineeMaritalStatus" + index + " option:selected").text() == "" || $("#ddlNomineeMaritalStatus" + index).val() == "") {
                    writeMultipleMessage("error", "Please Enter Nominee Marital Status.", "ddlNomineeMaritalStatus" + index);
                         IsValid = false;
                     }            
                else if ($("#NomineeAddress" + index).val() == "" || $("#NomineeAddress" + index).val() == null) {
                        writeMultipleMessage("error", "Please Enter Nominee Address.", "NomineeAddress" + index);
                         IsValid = false;
                     }
                    else if ($("#NomineeTelephone" + index).val() == "" || $("#NomineeTelephone" + index).val() == null) {
                        writeMultipleMessage("error", "Please Enter Nominee Telephone.", "NomineeTelephone" +index);
                        IsValid = false;
                    }
                else if ($("#NomineePercentage" + index).val() == "" || $("#NomineePercentage" + index).val() == "0") {
                    writeMultipleMessage("error", "Please Enter Nominee Percentage.", "NomineePercentage" + index);
                    $("#checkboxDeclaration").prop("checked", false);
                    IsValid = false;
                }
               
                var prevPercentagevalue = $("#NomineePercentage" + index).val();
                LoadingArray.push(prevPercentagevalue);
                var Salutation = $('#DDLNomineeSalutation' + index).val();
                var Gender = $('#DDLNomineeGender' + index).val();
                var Relation = $('#DDLNomineeRelationship' + index).val();

                if (Salutation == "Al Haj" || Salutation == "Ayur Dr Mr" || Salutation == "Father" || Salutation == "Master" || Salutation == "Mr" || Salutation == "Rector" || Salutation == "Reverent" || Salutation == "Reverent Father" || Salutation == "Sir") {
                    if (Gender == "F") {
                        writeMultipleMessage("error", "Salutation does not match with the Gender in Beneficiary Details.", 'DDLNomineeSalutation' + index);
                        $("#checkboxDeclaration").prop("checked", false);
                        IsValid = false;

                    }
                  
                }
                else if (Salutation == "Ayur Dr Mrs" || Salutation == "Ayur Dr Ms" || Salutation == "Dame" || Salutation == "Dr Mrs" || Salutation == "Dr Ms" || Salutation == "Lady" || Salutation == "Madam" || Salutation == "Miss" || Salutation == "Mrs" || Salutation == "Ms" || Salutation == "Reverent Sister") {
                    if (Gender == "M") {
                        writeMultipleMessage("error", "Salutation does not match with the Gender in Beneficiary Details.", 'DDLNomineeSalutation' + index);
                        $("#checkboxDeclaration").prop("checked", false);
                        IsValid = false;

                    }
                   
                }
                if (Relation == "2492" || Relation == "2498" || Relation == "2500" || Relation == "2506" || Relation == "2508" || Relation == "2512" || Relation == "2514" || Relation == "2516" || Relation == "2520" || Relation == "2522" || Relation == "2526") {
                    if (Gender == "M") {
                        writeMultipleMessage("error", "Relationship to lifeassured does not match with the Gender in Beneficiary Details.", 'DDLNomineeSalutation' + index);
                        $("#checkboxDeclaration").prop("checked", false);
                        IsValid = false;
                    }
                   
                }
                else if (Relation == "2493" || Relation == "2494" || Relation == "2504" || Relation == "2505" || Relation == "2507" || Relation == "2509" || Relation == "2511" || Relation == "2515" || Relation == "2521" || Relation == "2524" || Relation == "2525") {
                    if (Gender == "F") {
                        writeMultipleMessage("error", "Relationship to lifeassured does not match with the Gender in Beneficiary Details.", 'DDLNomineeSalutation' + index);
                        $("#checkboxDeclaration").prop("checked", false);
                        IsValid = false;
                    }
                  
                }

            }


            index = index + 1;
        }
        //});
        if (count > 0) {
            writeMultipleMessage("error", "Duplicate Beneficaries are not allowed", "DDLNomineeRelationship" + index);
            $("#checkboxDeclaration").prop("checked", false);
            IsValid = false;
       
        }
        //var prevPercentagevalue = $("#NomineePercentage" + index).val();
        // LoadingArray.push(prevPercentagevalue);
        var NomineePercentageIndex;
        $("#divNomineeDetailsGrid   tbody").find("tr").each(function (index) {
            NomineePercentageIndex = index;
        });
        var NomineeIsDeleted = $('#NomineeIsDeleted' + NomineePercentageIndex).val();
        if (NomineeIsDeleted == "False" || NomineeIsDeleted == "") {
            for (var i = 0; i < LoadingArray.length; i++) {
                total += parseFloat(LoadingArray[i]);
            }
            if (total > 100 || total < 100) {
                writeMultipleMessage("error", "Beneficiary Details Percentage Must be 100 %");
                $("#checkboxDeclaration").prop("checked", false);
                IsValid = false;
            }
        }

        //if (Percentage != 100) {
        //    writeMultipleMessage("error", "Sum of Nominee Percentage should be 100.", "NomineePercentage" + index);
        //    IsValid = false;
        //}
    }

    return IsValid;
}

function validatePremiumPayingDetails() {

    var PaymentMethod = $("#ddlPaymentMethod option:selected").text();
    var PaymentPaidBy = $("#ddlPaymentPaidBy option:selected").text();

    if (PaymentMethod == "Select" || PaymentMethod == "") {
        writeMultipleMessage("error", "Please select Payment Method", "ddlPaymentMethod");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlPaymentMethod");
    }

    if (PaymentPaidBy == "Select" || PaymentPaidBy == "") {
        writeMultipleMessage("error", "Please select Payment PaidBy", "ddlPaymentPaidBy");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlPaymentPaidBy");
    }

    return true;
}


function PreferredCommunicationMethodDetails() {

    var CommunicationMethod = $("#ddldocLanguage option:selected").text();
    var PremiumMethodCommunication = $("#ddlmodeofcom option:selected").text();

    if (CommunicationMethod == "Select" || CommunicationMethod == "") {
        writeMultipleMessage("error", "Please select Preferred language for Policy Document & Correspondence", "ddldocLanguage");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddldocLanguage");
    }

    if (PremiumMethodCommunication == "Select" || PremiumMethodCommunication == "") {
        writeMultipleMessage("error", "Please select Premium Method Communication", "ddlmodeofcom");
        $("#checkboxDeclaration").prop("checked", false);
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlmodeofcom");
    }

    return true;
}



function ShowChildrenDetails() {
    if ($("#chkcriticalillness").is(":checked")) {
        $("#divChildrenDetails").show();
    }
    else {
        $("#divChildrenDetails").hide();
    }
}




function ModifyProposal() {
    $("#DivFinalCalculatebutton").show();
}

function DownloadProposal() {
    $("#fromProposalPage").attr('action', '/Policy/GenerateReport');
    document.forms["fromProposalPage"].submit();
}
function GetGridIndex(DivID) {
    var _index = 0;
    var HasData = false;
    $("#" + DivID + " tbody").find("tr").each(function (index) {
        _index = index;
        HasData = true;
    });
    if (HasData) {
        _index = _index + 1;
    }
    return _index;
}
function ValidateLifeStyleDetails(index) {
    var Relationship = $('#ddlRelationWithProposer' + index).val();
    if (Relationship == '267' || Relationship == '268') {
        if ($('#txtHeight' + index).val() == '' || $('#txtHeight' + index).val() == '0') {
            writeMultipleMessage("error", 'Please Enter Height in Life Style Details', "txtHeight" + index);
            $("#checkboxDeclaration").prop("checked", false);
            return false;
        }
        else {
            writeMultipleMessage("error", '', "txtHeight" + index);
        }


        if ($('#txtWeight' + index).val() == '' || $('#txtWeight' + index).val() == '0') {
            writeMultipleMessage("error", 'Please Enter Weight in Life Style Details', "txtWeight" + index);
            $("#checkboxDeclaration").prop("checked", false);
            return false;
        }
        else {
            writeMultipleMessage("error", '', "txtWeight" + index);
        }
        var SmokeDeleted = 0;
        var SmokeModalPRop = 'objMemberDetails[' + 0 + '].objLifeStyleQuetions.IsSmoker';
        var SmokeModalPRop1 = 'objMemberDetails[' + 1 + '].objLifeStyleQuetions.IsSmoker';
        for (var i = 0; i < $('#DivSmokeTable' + index + ' tr').length ; i++) {
            IsSmokeDeleted = $("#txtIsDeleted" + index + i).val();
            if (IsSmokeDeleted == "true") {
                SmokeDeleted++
            }
        }
        if ($('#rdbSmokerYes' + index).prop("checked") == true) {



            if ($('#DivSmokeTable' + index + ' tr').length - SmokeDeleted - 1 == 0) {
                //tr:visible
                writeMultipleMessage("error", 'Please Enter Smoke Details in Life Style Details', "DummySmoke" + index);
                $("#checkboxDeclaration").prop("checked", false);
                return false;
            }

            else {
                writeMultipleMessage("error", '', "DummySmoke" + index);
            }


        }
        //if ($("input[name='" + SmokeModalPRop1 + "']:checked").val() == 'True') {
        //    if ($('#DivSmokeTable1').children('tbody').children('tr').length == 0) {
        //        writeMultipleMessage("error", 'Please Enter Smoke Details in Life Style Details', "DummySmoke" + index);
        //        $("#checkboxDeclaration").prop("checked", false);
        //        return false;
        //    }
        //    else {
        //        writeMultipleMessage("error", '', "DummySmoke" + index);
        //    }


        //}

        var AlcoholDeleted = 0;
        var AlcoholModalPRop = 'objMemberDetails[' + 0 + '].objLifeStyleQuetions.IsAlcholic';
        var AlcoholModalPRop1 = 'objMemberDetails[' + 1 + '].objLifeStyleQuetions.IsAlcholic';
        if ($("#rdbAlcoholYes" + index).prop("checked") == true) {
            //$('#txtIsDeleted' + MemberIndex + index).val(true);
            //$('#txtAlIsDeleted' + MemberIndex + index)

            //if ($('#DivAlcoholTable0').children('tbody').children('tr').length == 0) {
            for (var i = 0; i < $('#DivAlcoholTable' + index + ' tr').length ; i++) {
                var IsAlcoholDeleted = $("#txtAlIsDeleted" + index + i).val();
                if (IsAlcoholDeleted == "true") {
                    AlcoholDeleted++
                }
            }
            if ($('#DivAlcoholTable' + index + ' tr').length - AlcoholDeleted - 1 == 0) {
                writeMultipleMessage("error", 'Please Enter Alcohol  Details in Life Style Details', "DummyAlcohol" + index);
                $("#checkboxDeclaration").prop("checked", false);
                return false;
            }
            else {
                writeMultipleMessage("error", '', "DummyAlcohol" + index);
            }

        }
        //if ($("input[name='" + AlcoholModalPRop1 + "']:checked").val() == 'True') {

        //    if ($('#DivAlcoholTable1').children('tbody').children('tr').length==0) {
        //        writeMultipleMessage("error", 'Please Enter Alcohol  Details in Life Style Details', "DummyAlcohol" + index);
        //        $("#checkboxDeclaration").prop("checked", false);
        //        return false;
        //    }
        //    else {
        //        writeMultipleMessage("error", '', "DummyAlcohol" + index);
        //    }
        //}
        //else {
        //    writeMultipleMessage("error", '', "DummyAlcohol" + index);
        //}




    }


    return true;
}
function f1(id) {

    var res = $('#' + id).val();
    if (res === "") {
        res = "0";
    }
    $('#' + id).val(parseInt(res.replace(/,/g, '')).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
    $('#hdn' + id).val(res.replace(/,/g, ''));
}
function f2(id) {

    var res = $('#hdn' + id).val();
    $('#' + id).val(parseInt(res.replace(/,/g, '')).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
}
var SubmitProposalCount = 0;
function SubmitProposal() {
    if (SubmitProposalCount === 0) {
        SubmitProposalCount = SubmitProposalCount + 1;
        $("#divBusyIndicator").hide();
        $("#divBusyIndicator").show();
        $.post("../Policy/SubmitUWRemarks", $("#fromProposalPage").serialize(), function (data) {
            $("#divBusyIndicator").hide();
            $('#QuoteModalBody').empty();
            var Image = "";
            if (data.ErrorMessage === null || data.ErrorMessage === "") {
                switch (data.Decision) {
                    case "184"://Accept
                    case "185": // Accept with loading
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The Proposal is Accepted - Policy number ' + data.ProposalNo + '</p>';
                        break;
                    case "186":// Counter Offer
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The proposal is Sent to Wealth Planner with Counter Offer</p>';
                        break;
                    case "187":// Decline
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The proposal is declined - Refund Amount Initiated</p>';
                        break;
                    case "1176":// Not Taken Up
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The Proposal is Not Taken Up - Refund Amount Initiated</p>';
                        break;
                    case "1177":// Outstanding Requirement
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The proposal is Sent to Wealth Planner for Pending Requirements</p>';
                        break;
                    case "1449":// Postpone
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The Proposal is Postponed for ' + Duration + ' month duration - Refund Amount Initiated</p>';
                        break;
                    case "2299":// Withdrawn
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The Proposal is Withdrawn - Refund Amount Initiated</p>';
                        break;
                    case "2298":// Refer to Underwriter
                        Image = '<img class="Img-align" src="../Images/checked1.png" />';
                        Message = '<p class="text-center">The proposal is referred to ' + RefferedUnderwriterName + ' Underwriter</p>';
                        break;

                }
                $('#QuoteModalBody').append(Image);
                $('#QuoteModalBody').append(Message);
                $("#QuoteModal").modal("show");
            }
            else {
                $('#QuoteModalBody').empty();
                var Message = '<p class="text-center"> ' + data.ErrorMessage + '</p>';
                var Image1 = '<img class="Img-align" src="../Images/cancel1.png" />';
                $('#QuoteModalBody').append(Image1);
                $('#QuoteModalBody').append(Message);
                $("#QuoteModal").modal("show");
            }
            DisableRiskInformation();
        });
    }
}
function ValidateUWSectionDetails(CounterOffercase) {
    // Validate Member Level Data
    for (var i = 0; i < MemberCount; i++) {
        var Name = $('#hdnAssuredName' + i).val();
        var MemberLevelDecision = $('#ddlUWMemberlevelDecision' + i).val();

        if (MemberLevelDecision === '') {
            writeMultipleMessage("error", "Please Select  Member Level Decision for " + Name + ".", "ddlUWMemberlevelDecision" + i);
            return false;
        }
        else {
            writeMultipleMessage("error", "", "ddlUWMemberlevelDecision" + i);
        }
        if (MemberLevelDecision === "187" || MemberLevelDecision === "1176" || MemberLevelDecision === "1449" || MemberLevelDecision === "2299") // in Decline case  Underwriting Reason is mandatory.
        {
            if ($('#ddlUWMemberLevelReason' + i).val() === '' || $('#ddlUWMemberLevelReason' + i).val() === 'Select') {
                writeMultipleMessage("error", "Please Select reason for Underwriting Reason for " + Name + ".", "ddlUWMemberLevelReason" + i);
                return false;
            }
            else {
                writeMultipleMessage("error", "", "ddlUWMemberLevelReason" + i);

            }
            if (MemberLevelDecision === "1449" || MemberLevelDecision === "187") {
                if ($('#ddlUWMemberLevelMedicalCodes_' + i).val() === '' || $('#ddlUWMemberLevelMedicalCodes_' + i).val() === 'None Selected') {
                    writeMultipleMessage("error", "Please Select reason for Underwriting MedicalCodes for " + Name + ".", "ddlUWMemberLevelMedicalCodes_" + i);
                    return false;
                }
                else {
                    writeMultipleMessage("error", "", "ddlUWMemberLevelMedicalCodes_" + i);
                }

            }
        }
        var CommencementDate = $('#dtCommencementDt' + i).val();
        if (MemberLevelDecision !== "186" && MemberLevelDecision !== "1177" && MemberLevelDecision !== "2298") // Counter Offer/Outstanding Requirement/Refer to Underwriter
        {
            if (CommencementDate === '') {
                writeMultipleMessage("error", "Please Enter CommencementDate for " + Name + ".", "dtCommencementDt" + i);
                return false;
            }
            else {
                writeMultipleMessage("error", "", "dtCommencementDt" + i);
            }
        }
        // Validation for Member Level Deviation
        var MemeberlevelDeviation = false;
        $('#DeviationDecision_' + i).find('.MemberDeviation').each(function () {

            if ($(this).val() === "") {
                writeMultipleMessage("error", "Please Select Status against Member level Deviation for  " + Name + ".", "Dummy" + i);
                MemeberlevelDeviation = true;
                return false;
            }
            else {
                writeMultipleMessage("error", "", "Dummy" + i);
            }
        });
        if (MemeberlevelDeviation) {
            return false;
        }
        // ddlDeviation
        // till here


        var MedicaalGridID = 'MedicalGrid_' + i;
        var NonMedicaalGridID = 'NonMedicalGrid_' + i;


        // Medical Grid
        var MedicalDocError = false;
        $("#" + MedicaalGridID + " tbody").find("tr").each(function (index) {
            if ($('#ddlStatusMedical' + i + index).val() == undefined) {
                writeMultipleMessage("error", "Please Select Document Status for all  Medical Documents  for " + Name + ".", "ddlStatusMedical" + i + index);
                MedicalDocError = true;
                return false;
            }
            else if ($('#ddlStatusMedical' + i + index).val() == '') {
                writeMultipleMessage("error", "Please Select Document Status for all  Medical Documents  for " + Name + ".", "ddlStatusMedical" + i + index);
                MedicalDocError = true;
                return false;
            }
            else {
                writeMultipleMessage("error", "", "ddlStatusMedical" + i + index);
            }
        });
        if (MedicalDocError) {
            return false;
        }
        //Non Medical Grid
        var NonMedicalDocError = false;
        $("#" + NonMedicaalGridID + " tbody").find("tr").each(function (index) {
            if ($('#ddlStatusNonMedical' + i + index).val() === undefined) {
                writeMultipleMessage("error", "Please Select Document Status for all  Non Medical Documents  for " + Name + ".", "ddlStatusNonMedical" + i + index);
                NonMedicalDocError = true;
                return false;
            }
            else if ($('#ddlStatusNonMedical' + i + index).val() === '') {
                writeMultipleMessage("error", "Please Select Document Status for all Non Medical Documents  for " + Name + ".", "ddlStatusNonMedical" + i + index);
                NonMedicalDocError = true;
                return false;
            }
            else {
                writeMultipleMessage("error", "", "ddlStatusNonMedical" + i + index);
            }
        });
        if (NonMedicalDocError) {
            return false;
        }
        // till here
        if (MemberLevelDecision === "1449") // Counter Offer/Outstanding Requirement/Refer to Underwriter
        {
            var Duration = parseInt($('#txtUWMemberLevelMonth' + i).val());
            if (isNaN(Duration)) {
                writeMultipleMessage("error", "Please Enter Duration for  " + Name, "txtUWMemberLevelMonth" + i);
                return false;
            }
            else {
                writeMultipleMessage("error", "", "txtNoofDays");
            }
        }
    }


    //if( CounterOffercase != 'True')
    //{
    // Validate Policy level Data
    if ($('#ddlPolicylevelDecision').val() === '' || $('#ddlPolicylevelDecision').val() === undefined) {
        writeMultipleMessage("error", "Please Select  Policy Level Decision.", "ddlPolicylevelDecision");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "ddlPolicylevelDecision");
    }
    if ($('#ddlPolicylevelDecision').val() === '2298')  // Under writer Reffered Case
    {
        if ($('#ddlSelectUW').val() === '2298') {
            writeMultipleMessage("error", "Please Select Under Writer to be reffered.", "ddlSelectUW");
            return false;
        }
        else {
            writeMultipleMessage("error", "", "ddlSelectUW");
        }
    }
    //else if($('#ddlPolicylevelDecision').val()==  '1449')
    //{ // Ppostpone case
    //    if($('#txtNoofDays').val() ==  '')
    //    {
    //        writeMultipleMessage("error", "Please enter No of Days.", "txtNoofDays");
    //        return false;
    //    }
    //    else{
    //        writeMultipleMessage("error", "", "txtNoofDays");
    //    }
    //}

    //}



}

function OnChangeofPolicylevelDecision() {
    if ($('#ddlPolicylevelDecision').val() === '2298') {
        $('#UWSelect').show(); $('#CounterofferQuote').hide();
        //$('#UWPostPone').hide();
        //$('#DivUWFinalbutton').show();
    }
    else if ($('#ddlPolicylevelDecision').val() === '186') {
        $('#CounterofferQuote').show(); $('#UWSelect').hide();
        //$('#UWPostPone').hide();
        // $('#DivUWFinalbutton').hide();
    }
    else if ($('#ddlPolicylevelDecision').val() === '1449') {       //$('#UWPostPone').show();
        $('#CounterofferQuote').hide();
        //$('#UWSelect').hide();
        // $('#DivUWFinalbutton').hide();
    }
    else {
        $('#CounterofferQuote').hide(); $('#UWSelect').hide();
        //$('#UWPostPone').hide();
        // $('#DivUWFinalbutton').show();
    }


}

function GenerateQuoteForCounterOffer(_QuoteNo) {
    if ($("#hdnIsApplyLoading").val() === "True") {
        $('#GenerateQuoteModalBody').empty();
        var Message = '<p class="text-center"> Applied Loading will be removed as you are generating a new quotation. Are you sure?</p>';
        var Image = '<img class="Img-align" src="../Images/warning1.png" />';
        $('#GenerateQuoteModalBody').append(Image);
        $('#GenerateQuoteModalBody').append(Message);
        $("#GenerateQuoteModal").modal("show");
    }
    else {
        ContinueGenerateQuoteForCounterOffer(_QuoteNo);
    }
}
function ContinueGenerateQuoteForCounterOffer(_QuoteNo) {

    if (ValidateUWSectionDetails('True') !== false) {
        EnableRiskInformation();
        $('#txtProposalNo').prop('disabled', false);

        $("#fromProposalPage").attr('action', '/Policy/GenerateQuoteCla');
        document.forms["fromProposalPage"].submit();
    }
}
function Re_LoadMedicalCodes(index) {
    $('select', $('#ddlUWMedicalCodes_' + index)).multiselect('deselectAll', false);
    $('#ddlUWMedicalCodes_' + index).find('.multiselect').prop("title", "None Selected");
    $('#ddlUWMedicalCodes_' + index).find('.multiselect-selected-text').text('None Selected');
    $('#ddlUWMedicalCodes_' + index).find('.multiselect-container').find('input').prop("disabled", false);
    $('#txtUWMemberLevelMonth' + index).prop("disabled", false);
    $('#ddlUWMedicalCodes_' + index).find('option:selected').prop('selected', false);
    $('#ddlUWMedicalCodes_' + index).find('.multiselect-container').find('input').prop('checked', false);
}
function Re_LoadUWReasonandMedicalCodes(index) {
    $('select', $('#ddlUWMedicalCodes_' + index)).multiselect('deselectAll', false);
    $('#ddlUWMedicalCodes_' + index).find('.multiselect').prop("title", "None Selected");
    $('#ddlUWMedicalCodes_' + index).find('.multiselect-selected-text').text('None Selected');
    $('#ddlUWMedicalCodes_' + index).find('.multiselect-container').find('input').prop("disabled", false);
    $('#txtUWMemberLevelMonth' + index).prop("disabled", false);
    //$('#ddlUWMemberLevelMedicalCodes_' + index + 'option:selected').prop('selected', false);
    $('#ddlUWMedicalCodes_' + index).find('option:selected').prop('selected', false);
    $('#ddlUWMedicalCodes_' + index).find('.multiselect-container').find('input').prop('checked', false);
}