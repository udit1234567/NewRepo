function CurrentTotal(id) {
    f1(id);
    var Land = parseInt($('#hdntxtAssets0').val(), 10);
    var FixedDeposit = parseInt($('#hdntxtAssets1').val(), 10);
    var Shares = parseInt($('#hdntxtAssets2').val(), 10);
    var Vehicles = parseInt($('#hdntxtAssets3').val(), 10);
    var Other = parseInt($('#hdntxtAssets4').val(), 10);
    var Jewellery = parseInt($('#hdntxtAssets5').val(), 10);
    if (isNaN(Land)) {
        Land = 0;
    }
    if (isNaN(FixedDeposit)) {
        FixedDeposit = 0;
    }
    if (isNaN(Shares)) {
        Shares = 0;
    }
    if (isNaN(Vehicles)) {
        Vehicles = 0;
    }
    if (isNaN(Jewellery)) {
        Jewellery = 0;
    }
    if (isNaN(Other)) {
        Other = 0;
    }
    var currentTotal = Land + FixedDeposit + Shares + Vehicles + Jewellery + Other;
    $("#hdntxtCurrentTotal").val(currentTotal.toFixed(0));
    f2("txtCurrentTotal");
    compareAssest();
}
function CurrentTotalNC() {    
    var Land = parseInt($('#hdntxtAssets0').val(), 10);
    var FixedDeposit = parseInt($('#hdntxtAssets1').val(), 10);
    var Shares = parseInt($('#hdntxtAssets2').val(), 10);
    var Vehicles = parseInt($('#hdntxtAssets3').val(), 10);
    var Other = parseInt($('#hdntxtAssets4').val(), 10);
    var Jewellery = parseInt($('#hdntxtAssets5').val(), 10);
    if (isNaN(Land)) {
        Land = 0;
    }
    if (isNaN(FixedDeposit)) {
        FixedDeposit = 0;
    }
    if (isNaN(Shares)) {
        Shares = 0;
    }
    if (isNaN(Vehicles)) {
        Vehicles = 0;
    }
    if (isNaN(Jewellery)) {
        Jewellery = 0;
    }
    if (isNaN(Other)) {
        Other = 0;
    }
    var currentTotal = Land + FixedDeposit + Shares + Vehicles + Jewellery + Other;
    $("#hdntxtCurrentTotal").val(currentTotal.toFixed(0));
    f2("txtCurrentTotal");
    compareAssest();
}
function LiabilityTotal1(id) {
    f1(id);
    var Bank1 = parseInt($('#hdntxtliabilityone0').val(), 10);
    var Borrowings1 = parseInt($('#hdntxtliabilityone1').val(), 10);
    var Lease1 = parseInt($('#hdntxtliabilityone2').val(), 10);
    var Other1 = parseInt($('#hdntxtliabilityone3').val(), 10);
    if (isNaN(Bank1)) {
        Bank1 = 0;
    }
    if (isNaN(Borrowings1)) {
        Borrowings1 = 0;
    }
    if (isNaN(Lease1)) {
        Lease1 = 0;
    }
    if (isNaN(Other1)) {
        Other1 = 0;
    }
    var LiabilityTotal1 = Bank1 + Borrowings1 + Lease1 + Other1;
    $("#hdntxtLiabilityTotal1").val(LiabilityTotal1.toFixed(0));
    f2("txtLiabilityTotal1");
    var currentTotal = $("#hdntxtCurrentTotal").val();
    if (isNaN(currentTotal)) {
        currentTotal = 0;
    }
    var NetAssests = currentTotal - LiabilityTotal1;
    if (NetAssests >= 0) {
        $('#txtNetAssests').removeClass("blank-field-hightlight");
        $('#txtNetAssests').removeClass("negative-field-hightlight");
        $('#txtNetAssests').addClass("positive-field-hightlight");
        $('#txtNetAssests').css('color', '#32CD32');
    }
    if (NetAssests < 0) {
        $('#txtNetAssests').removeClass("blank-field-hightlight");
        $('#txtNetAssests').removeClass("positive-field-hightlight");
        $('#txtNetAssests').addClass("negative-field-hightlight");
        $('#txtNetAssests').css('color', 'red');
        NetAssests *= -1;
    }
    $("#hdntxtNetAssests").val(NetAssests.toFixed(0));
    f2("txtNetAssests");
    compareAssest();
}
function LiabilityTotal1NC() {
    var Bank1 = parseInt($('#hdntxtliabilityone0').val(), 10);
    var Borrowings1 = parseInt($('#hdntxtliabilityone1').val(), 10);
    var Lease1 = parseInt($('#hdntxtliabilityone2').val(), 10);
    var Other1 = parseInt($('#hdntxtliabilityone3').val(), 10);
    if (isNaN(Bank1)) {
        Bank1 = 0;
    }
    if (isNaN(Borrowings1)) {
        Borrowings1 = 0;
    }
    if (isNaN(Lease1)) {
        Lease1 = 0;
    }
    if (isNaN(Other1)) {
        Other1 = 0;
    }
    var LiabilityTotal1 = Bank1 + Borrowings1 + Lease1 + Other1;
    $("#hdntxtLiabilityTotal1").val(LiabilityTotal1.toFixed(0));
    f2("txtLiabilityTotal1");
    var currentTotal = $("#hdntxtCurrentTotal").val();
    if (isNaN(currentTotal)) {
        currentTotal = 0;
    }
    var NetAssests = currentTotal - LiabilityTotal1;
    $("#hdntxtNetAssests").val(NetAssests.toFixed(0));
    f2("txtNetAssests");
    compareAssest();
}
function LiabilityTotal2(id) {
    f1(id);
    var Bank2 = parseInt($('#hdntxtliabilitytwo0').val(), 10);
    var Borrowings2 = parseInt($('#hdntxtliabilitytwo1').val(), 10);
    var Lease2 = parseInt($('#hdntxtliabilitytwo2').val(), 10);
    var Other2 = parseInt($('#hdntxtliabilitytwo3').val(), 10);
    if (isNaN(Bank2)) {
        Bank2 = 0;
    }
    if (isNaN(Borrowings2)) {
        Borrowings2 = 0;
    }
    if (isNaN(Lease2)) {
        Lease2 = 0;
    }
    if (isNaN(Other2)) {
        Other2 = 0;
    }
    var LiabilityTotal2 = Bank2 + Borrowings2 + Lease2 + Other2;
    $("#hdntxtLiabilityTotal2").val(LiabilityTotal2.toFixed(0));
    f2("txtLiabilityTotal2");
    var LiabilityTotal1 = $("#hdntxtLiabilityTotal1").val();
    if (isNaN(LiabilityTotal1)) {
        LiabilityTotal1 = 0;
    }
    var LumpsumRequirement = LiabilityTotal1 - LiabilityTotal2;
    $("#hdntxtLumpsumRequirement").val(LumpsumRequirement.toFixed(0));
    f2("txtLumpsumRequirement");
    GapTotal();
    Policy41nc();
    Policy42nc();
}
function LiabilityTotal2NC() {
    var Bank2 = parseInt($('#hdntxtliabilitytwo0').val(), 10);
    var Borrowings2 = parseInt($('#hdntxtliabilitytwo1').val(), 10);
    var Lease2 = parseInt($('#hdntxtliabilitytwo2').val(), 10);
    var Other2 = parseInt($('#hdntxtliabilitytwo3').val(), 10);
    if (isNaN(Bank2)) {
        Bank2 = 0;
    }
    if (isNaN(Borrowings2)) {
        Borrowings2 = 0;
    }
    if (isNaN(Lease2)) {
        Lease2 = 0;
    }
    if (isNaN(Other2)) {
        Other2 = 0;
    }
    var LiabilityTotal2 = Bank2 + Borrowings2 + Lease2 + Other2;
    $("#hdntxtLiabilityTotal2").val(LiabilityTotal2.toFixed(0));
    f2("txtLiabilityTotal2");
    var LiabilityTotal1 = $("#hdntxtLiabilityTotal1").val();
    if (isNaN(LiabilityTotal1)) {
        LiabilityTotal1 = 0;
    }
    var LumpsumRequirement = LiabilityTotal1 - LiabilityTotal2;
    $("#hdntxtLumpsumRequirement").val(LumpsumRequirement.toFixed(0));
    f2("txtLumpsumRequirement");
    GapTotal();
    Policy41nc();
    Policy42nc();
}
function AnnualIncomeTotalIntrest() {
   
    var Salary = parseInt($('#hdntxtIncome0').val(), 10);
    var Interest = parseInt($('#hdntxtIncome1').val(), 10);
    var Rent = parseInt($('#hdntxtIncome2').val(), 10);
    var Other = parseInt($('#hdntxtIncome3').val(), 10);
    var IntrestRate = parseInt($('#txtInterestrateFNA').val(), 10)
    if (isNaN(Salary)) {
        Salary = 0;
    }
    if (isNaN(Interest)) {
        Interest = 0;
    }
    if (isNaN(Rent)) {
        Rent = 0;
    }
    if (isNaN(Other)) {
        Other = 0;
    }
    if (isNaN(IntrestRate)) {
        Other = 0;
    }
    var AnnualIncomeTotal = Salary + Interest + Rent + Other;
    $("#hdntxtAnnualIncomeTotal").val(AnnualIncomeTotal.toFixed(0));
    f2("txtAnnualIncomeTotal");
    var AnnualLumpsumRequirement = (Salary * 100) / IntrestRate;
    $("#hdntxtAnnualLumpsumRequirement").val(AnnualLumpsumRequirement.toFixed(0));
    f2("txtAnnualLumpsumRequirement");
    compareExpense();
    GapTotal();
    Policy41nc();
    Policy42nc();
}
function AnnualIncomeTotal(id) {
    f1(id);
    var Salary = parseInt($('#hdntxtIncome0').val(), 10);
    var Interest = parseInt($('#hdntxtIncome1').val(), 10);
    var Rent = parseInt($('#hdntxtIncome2').val(), 10);
    var Other = parseInt($('#hdntxtIncome3').val(), 10);
    var IntrestRate = parseInt($('#txtInterestrateFNA').val(), 10)
    if (isNaN(Salary)) {
        Salary = 0;
    }
    if (isNaN(Interest)) {
        Interest = 0;
    }
    if (isNaN(Rent)) {
        Rent = 0;
    }
    if (isNaN(Other)) {
        Other = 0;
    }
    if (isNaN(IntrestRate)) {
        Other = 0;
    }
    var AnnualIncomeTotal = Salary + Interest + Rent + Other;
    $("#hdntxtAnnualIncomeTotal").val(AnnualIncomeTotal.toFixed(0));
    f2("txtAnnualIncomeTotal");
    var AnnualLumpsumRequirement = (Salary * 100) / IntrestRate;
    $("#hdntxtAnnualLumpsumRequirement").val(AnnualLumpsumRequirement.toFixed(0));
    f2("txtAnnualLumpsumRequirement");
    compareExpense();
    GapTotal();
    Policy41nc();
    Policy42nc();
}
function AnnualIncomeTotalNC() {
    var Salary = parseInt($('#hdntxtIncome0').val(), 10);
    var Interest = parseInt($('#hdntxtIncome1').val(), 10);
    var Rent = parseInt($('#hdntxtIncome2').val(), 10);
    var Other = parseInt($('#hdntxtIncome3').val(), 10);
    var IntrestRate = parseInt($('#txtInterestrateFNA').val(), 10)
    if (isNaN(Salary)) {
        Salary = 0;
    }
    if (isNaN(Interest)) {
        Interest = 0;
    }
    if (isNaN(Rent)) {
        Rent = 0;
    }
    if (isNaN(Other)) {
        Other = 0;
    }
    if (isNaN(IntrestRate)) {
        Other = 0;
    }
    var AnnualIncomeTotal = Salary + Interest + Rent + Other;
    $("#hdntxtAnnualIncomeTotal").val(AnnualIncomeTotal.toFixed(0));
    f2("txtAnnualIncomeTotal");
    var AnnualLumpsumRequirement = (Salary * 100) / IntrestRate;
    $("#hdntxtAnnualLumpsumRequirement").val(AnnualLumpsumRequirement.toFixed(0));
    f2("txtAnnualLumpsumRequirement");
    compareExpense();
    GapTotal();
    Policy41nc();
    Policy42nc();
}
function LivingAnnualExpensesTotal(id) {
    f1(id);
    var AnnualLivingExpenses = parseInt($('#hdntxtexpense0').val(), 10);
    var AnnualVaction = parseInt($('#hdntxtexpense1').val(), 10);
    var InstalmentsApartmentsVehicles = parseInt($('#hdntxtexpense2').val(), 10);
    var LoanPayment = parseInt($('#hdntxtexpense3').val(), 10);
    var LivingAnnualExpensesOther = parseInt($('#hdntxtexpense5').val(), 10);
    var LivingAnnualExpensesVehicle = parseInt($('#hdntxtexpense4').val(), 10);
    if (isNaN(AnnualLivingExpenses)) {
        AnnualLivingExpenses = 0;
    }
    if (isNaN(AnnualVaction)) {
        AnnualVaction = 0;
    }
    if (isNaN(InstalmentsApartmentsVehicles)) {
        InstalmentsApartmentsVehicles = 0;
    }
    if (isNaN(LoanPayment)) {
        LoanPayment = 0;
    }
    if (isNaN(LivingAnnualExpensesOther)) {
        LivingAnnualExpensesOther = 0;
    }
    if (isNaN(LivingAnnualExpensesVehicle)) {
        LivingAnnualExpensesVehicle = 0;
    }
   
    var LivingAnnualExpensesTotal = AnnualLivingExpenses + AnnualVaction + InstalmentsApartmentsVehicles + LoanPayment + LivingAnnualExpensesOther + LivingAnnualExpensesVehicle;
    $("#hdntxtLivingAnnualExpensesTotal").val(LivingAnnualExpensesTotal.toFixed(0));
    f2("txtLivingAnnualExpensesTotal");
    var AnnualIncomeTotal = $("#hdntxtAnnualIncomeTotal").val();
    if (isNaN(AnnualIncomeTotal)) {
        AnnualIncomeTotal = 0;
    }
    
    var SurPlusDeficit = AnnualIncomeTotal - LivingAnnualExpensesTotal;
    if (SurPlusDeficit >= 0) {
        $('#txtSurPlusDeficit').removeClass("blank-field-hightlight");
        $('#txtSurPlusDeficit').removeClass("negative-field-hightlight");
        $('#txtSurPlusDeficit').addClass("positive-field-hightlight");
        $('#txtSurPlusDeficit').css('color', '#32CD32');
    }
    if (SurPlusDeficit < 0) {
        $('#txtSurPlusDeficit').removeClass("blank-field-hightlight");
        $('#txtSurPlusDeficit').removeClass("positive-field-hightlight");
        $('#txtSurPlusDeficit').addClass("negative-field-hightlight");
        $('#txtSurPlusDeficit').css('color', 'red');
        SurPlusDeficit *= -1;
    }
    $("#hdntxtSurPlusDeficit").val(SurPlusDeficit.toFixed(0));
    f2("txtSurPlusDeficit");
    compareExpense();
}
function LivingAnnualExpensesTotalNC() {
    var AnnualLivingExpenses = parseInt($('#hdntxtexpense0').val(), 10);
    var AnnualVaction = parseInt($('#hdntxtexpense1').val(), 10);
    var InstalmentsApartmentsVehicles = parseInt($('#hdntxtexpense2').val(), 10);
    var LoanPayment = parseInt($('#hdntxtexpense3').val(), 10);
    var LivingAnnualExpensesOther = parseInt($('#hdntxtexpense5').val(), 10);
    var LivingAnnualExpensesVehicle = parseInt($('#hdntxtexpense4').val(), 10);
    if (isNaN(AnnualLivingExpenses)) {
        AnnualLivingExpenses = 0;
    }
    if (isNaN(AnnualVaction)) {
        AnnualVaction = 0;
    }
    if (isNaN(InstalmentsApartmentsVehicles)) {
        InstalmentsApartmentsVehicles = 0;
    }
    if (isNaN(LoanPayment)) {
        LoanPayment = 0;
    }
    if (isNaN(LivingAnnualExpensesOther)) {
        LivingAnnualExpensesOther = 0;
    }
    if (isNaN(LivingAnnualExpensesVehicle)) {
        LivingAnnualExpensesVehicle = 0;
    }

    var LivingAnnualExpensesTotal = AnnualLivingExpenses + AnnualVaction + InstalmentsApartmentsVehicles + LoanPayment + LivingAnnualExpensesOther + LivingAnnualExpensesVehicle;
    $("#hdntxtLivingAnnualExpensesTotal").val(LivingAnnualExpensesTotal.toFixed(0));
    f2("txtLivingAnnualExpensesTotal");
    var AnnualIncomeTotal = $("#hdntxtAnnualIncomeTotal").val();
    if (isNaN(AnnualIncomeTotal)) {
        AnnualIncomeTotal = 0;
    }

    var SurPlusDeficit = AnnualIncomeTotal - LivingAnnualExpensesTotal;
    if (SurPlusDeficit >= 0) {
        $('#txtSurPlusDeficit').removeClass("blank-field-hightlight");
        $('#txtSurPlusDeficit').removeClass("negative-field-hightlight");
        $('#txtSurPlusDeficit').addClass("positive-field-hightlight");
        $('#txtSurPlusDeficit').css('color', '#32CD32');
    }
    if (SurPlusDeficit < 0) {
        $('#txtSurPlusDeficit').removeClass("blank-field-hightlight");
        $('#txtSurPlusDeficit').removeClass("positive-field-hightlight");
        $('#txtSurPlusDeficit').addClass("negative-field-hightlight");
        $('#txtSurPlusDeficit').css('color', 'red');
        SurPlusDeficit *= -1;
    }
    $("#hdntxtSurPlusDeficit").val(SurPlusDeficit.toFixed(0));
    f2("txtSurPlusDeficit");
    compareExpense();
}
function RequirementTotal(id) {
    f1(id);
    var Education1 = parseInt($('#hdntxtreqtable5').val(), 10);
    var PensionFund1 = parseInt($('#hdntxtreqtable4').val(), 10);
    var CarProperty1 = parseInt($('#hdntxtreqtable3').val(), 10);
    var DreamOther1 = parseInt($('#hdntxtreqtable2').val(), 10);
    var Wedding1 = parseInt($('#hdntxtreqtable1').val(), 10);
    if (isNaN(Education1)) {
        Education1 = 0;
    }
    if (isNaN(PensionFund1)) {
        PensionFund1 = 0;
    }
    if (isNaN(CarProperty1)) {
        CarProperty1 = 0;
    }
    if (isNaN(DreamOther1)) {
        DreamOther1 = 0;
    }
    if (isNaN(Wedding1)) {
        Wedding1 = 0;
    }
    var DreamTotal1 = Education1 + PensionFund1 + CarProperty1 + DreamOther1 + Wedding1;
    $("#hdntxtDreamTotal1").val(DreamTotal1.toFixed(0));
    f2("txtDreamTotal1");
    var Rate = parseInt($("#txtInflationrateFNA").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyearFNA").val(), 10);

    if (isNaN(Time)) {
        Time = 10;
    }
    var Education2 = Education1 * Math.pow((1 + (Rate / 100)), Time);
    if (isNaN(Education2)) {
        Education2 = 0;
    }
    $("#hdntxtesttable5").val(Education2.toFixed(0));
    f2("txtesttable5");
    var PensionFund2 = PensionFund1 * Math.pow((1 + (Rate / 100)), Time);
    if (isNaN(PensionFund2)) {
        PensionFund2 = 0;
    }
    $("#hdntxtesttable4").val(PensionFund2.toFixed(0));
    f2("txtesttable4");
    var CarProperty2 = CarProperty1 * Math.pow((1 + (Rate / 100)), Time);
    if (isNaN(CarProperty2)) {
        CarProperty2 = 0;
    }
    $("#hdntxtesttable3").val(CarProperty2.toFixed(0));
    f2("txtesttable3");
    var DreamOther2 = DreamOther1 * Math.pow((1 + (Rate / 100)), Time);
    if (isNaN(DreamOther2)) {
        DreamOther2 = 0;
    }
    
    $("#hdntxtesttable2").val(DreamOther2.toFixed(0));
    f2("txtesttable2");
    var Wedding2 = Wedding1 * Math.pow((1 + (Rate / 100)), Time);
    if (isNaN(Wedding2)) {
        Wedding2 = 0;
    }
    $("#hdntxtesttable1").val(Wedding2.toFixed(0));
    f2("txtesttable1");
    AmountTotal();
    Lumpsumrequirement2017();
    GapTotal();
    Policy41nc();
    Policy42nc();
}
function AmountTotal() {

    var Education2 = parseInt($('#hdntxtesttable5').val(), 10);
    var PensionFund2 = parseInt($('#hdntxtesttable4').val(), 10);
    var CarProperty2 = parseInt($('#hdntxtesttable3').val(), 10);
    var DreamOther2 = parseInt($('#hdntxtesttable2').val(), 10);
    var Wedding2 = parseInt($('#hdntxtesttable1').val(), 10);
    if (isNaN(Education2)) {
        Education2 = 0;
    }
    if (isNaN(PensionFund2)) {
        PensionFund2 = 0;
    }
    if (isNaN(CarProperty2)) {
        CarProperty2 = 0;
    }
    if (isNaN(DreamOther2)) {
        DreamOther2 = 0;
    }
    if (isNaN(Wedding2)) {
        Wedding2 = 0;
    }
    var DreamTotal2 = Education2 + PensionFund2 + CarProperty2 + DreamOther2 + Wedding2;
    $("#hdntxtDreamTotal2").val(DreamTotal2.toFixed(0));
    f2("txtDreamTotal2");
    EducationGap();
    WeddingGap();
    PensionGap();
    CarPropertyGap();
    DreamOtherGap();
    GapTotal();
    Policy41nc();
    Policy42nc();
}
function FundTotal(id) {
    f1(id);
    var Education3 = parseInt($('#hdntxttfundable5').val(), 10);
    var PensionFund3 = parseInt($('#hdntxttfundable4').val(), 10);
    var CarProperty3 = parseInt($('#hdntxttfundable3').val(), 10);
    var DreamOther3 = parseInt($('#hdntxttfundable2').val(), 10);
    var Wedding3 = parseInt($('#hdntxttfundable1').val(), 10);
    if (isNaN(Education3)) {
        Education3 = 0;
    }
    if (isNaN(PensionFund3)) {
        PensionFund3 = 0;
    }
    if (isNaN(CarProperty3)) {
        CarProperty3 = 0;
    }
    if (isNaN(DreamOther3)) {
        DreamOther3 = 0;
    }
    if (isNaN(Wedding3)) {
        Wedding3 = 0;
    }
    var DreamTotal3 = Education3 + PensionFund3 + CarProperty3 + DreamOther3 + Wedding3;
    $("#hdntxtDreamTotal3").val(DreamTotal3.toFixed(0));
    f2("txtDreamTotal3");
    EducationGap();
    WeddingGap();
    PensionGap();
    CarPropertyGap();
    DreamOtherGap();
    GapTotal();
    Lumpsumrequirement2017();
    Policy41nc();
    Policy42nc();
}
function EducationGap() {

    var Education2 = parseInt($('#hdntxtesttable5').val(), 10);
    var Education3 = parseInt($('#hdntxttfundable5').val(), 10);
    if (isNaN(Education2)) {
        Education2 = 0;
    }
    if (isNaN(Education3)) {
        Education3 = 0;
    }
    var Education4 = Education2 - Education3;
    $("#hdntxtgaptable5").val(Education4.toFixed(0));
    f2("txtgaptable5");

}
function WeddingGap() {

    var Wedding2 = parseInt($('#hdntxtesttable4').val(), 10);
    var Wedding3 = parseInt($('#hdntxttfundable4').val(), 10);
    if (isNaN(Wedding2)) {
        Wedding2 = 0;
    }
    if (isNaN(Wedding3)) {
        Wedding3 = 0;
    }
    var Wedding4 = Wedding2 - Wedding3;
    $("#hdntxtgaptable4").val(Wedding4.toFixed(0));
    f2("txtgaptable4");

}
function PensionGap() {

    var PensionFund2 = parseInt($('#hdntxtesttable3').val(), 10);
    var PensionFund3 = parseInt($('#hdntxttfundable3').val(), 10);
    if (isNaN(PensionFund2)) {
        PensionFund2 = 0;
    }
    if (isNaN(PensionFund3)) {
        PensionFund3 = 0;
    }
    var PensionFund4 = PensionFund2 - PensionFund3;
    $("#hdntxtgaptable3").val(PensionFund4.toFixed(0));
    f2("txtgaptable3");

}
function CarPropertyGap() {

    var CarProperty2 = parseInt($('#hdntxtesttable2').val(), 10);
    var CarProperty3 = parseInt($('#hdntxttfundable2').val(), 10);
    if (isNaN(CarProperty2)) {
        CarProperty2 = 0;
    }
    if (isNaN(CarProperty3)) {
        CarProperty3 = 0;
    }
    var CarProperty4 = CarProperty2 - CarProperty3;
    $("#hdntxtgaptable2").val(CarProperty4.toFixed(0));
    f2("txtgaptable2");

}
function DreamOtherGap() {

    var DreamOther2 = parseInt($('#hdntxtesttable1').val(), 10);
    var DreamOther3 = parseInt($('#hdntxttfundable1').val(), 10);
    if (isNaN(DreamOther2)) {
        DreamOther2 = 0;
    }
    if (isNaN(DreamOther3)) {
        DreamOther3 = 0;
    }
    var DreamOther4 = DreamOther2 - DreamOther3;
    $("#hdntxtgaptable1").val(DreamOther4.toFixed(0));
    f2("txtgaptable1");

}
function GapTotal() {
    var Education4 = parseInt($('#hdntxtgaptable5').val(), 10);
    var PensionFund4 = parseInt($('#hdntxtgaptable4').val(), 10);
    var CarProperty4 = parseInt($('#hdntxtgaptable3').val(), 10);
    var DreamOther4 = parseInt($('#hdntxtgaptable2').val(), 10);
    var Wedding4 = parseInt($('#hdntxtgaptable1').val(), 10);
    if (isNaN(Education4)) {
        Education4 = 0;
    }
    if (isNaN(PensionFund4)) {
        PensionFund4 = 0;
    }
    if (isNaN(CarProperty4)) {
        CarProperty4 = 0;
    }
    if (isNaN(DreamOther4)) {
        DreamOther4 = 0;
    }
    if (isNaN(Wedding4)) {
        Wedding4 = 0;
    }
    var DreamTotal4 = Education4 + PensionFund4 + CarProperty4 + DreamOther4 + Wedding4;
    if (isNaN(DreamTotal4)) {
        DreamTotal4 = 0;
    }
    
    $("#hdntxtDreamTotal4").val(DreamTotal4.toFixed(0));
    f2("txtDreamTotal4");
    var LumpsumRequirement = $("#hdntxtLumpsumRequirement").val();
    if (isNaN(LumpsumRequirement)) {
        LumpsumRequirement = 0;
    }
    if (LumpsumRequirement == "") {
        LumpsumRequirement = 0;
    }
   
    $("#hdntxtProtectYourWealth").val(LumpsumRequirement);
    f2("txtProtectYourWealth");
    var AnnualLumpsumRequirement = $("#hdntxtAnnualLumpsumRequirement").val();
    if (isNaN(AnnualLumpsumRequirement)) {
        AnnualLumpsumRequirement = 0;
    }
    if (AnnualLumpsumRequirement == "") {
        AnnualLumpsumRequirement = 0;
    }
    $("#hdntxtProtectYourMonthlyIncome").val(AnnualLumpsumRequirement);
    f2("txtProtectYourMonthlyIncome");
    $("#hdntxtProtectYourDream").val(DreamTotal4.toFixed(0));
    f2("txtProtectYourDream");
    if (isNaN(DreamTotal4)) {
        DreamTotal4 = 0;
    }
    if (isNaN(AnnualLumpsumRequirement)) {
        AnnualLumpsumRequirement = 0;
    }
    if (isNaN(LumpsumRequirement)) {
        LumpsumRequirement = 0;
    }
    
    var Total1Dream = parseInt(DreamTotal4) + parseInt(AnnualLumpsumRequirement) + parseInt(LumpsumRequirement);
    if (isNaN(Total1Dream)) {
        Total1Dream = 0;
    }
    $("#hdntxtTotal1Dream").val(Total1Dream.toFixed(0));
    f2("txtTotal1Dream");
    $("#txtTotal1Dream").css('border-color', 'red');
    $("#hdntxtProtectYourDream2").val(DreamTotal4.toFixed(0));
    f2("txtProtectYourDream2");

}
function Total1Dream2() {

    var ProtectYourWealth2 = parseInt($('#hdntxtProtectYourWealth2').val(), 10);
    var ProtectYourMonthlyIncome2 = parseInt($('#txtProtectYourMonthlyIncome2').val(), 10);
    var ProtectYourDream2 = parseInt($('#hdntxtProtectYourDream2').val(), 10);
    if (isNaN(ProtectYourWealth2)) {
        ProtectYourWealth2 = 0;
    }
    if (isNaN(ProtectYourMonthlyIncome2)) {
        ProtectYourMonthlyIncome2 = 0;
    }
    if (isNaN(ProtectYourDream2)) {
        ProtectYourDream2 = 0;
    }
    
    var Total1Dream2 = ProtectYourWealth2 + ProtectYourMonthlyIncome2 + ProtectYourDream2;
    $("#hdntxtTotal1Dream2").val(Total1Dream2.toFixed(0));
    
    f2("txtTotal1Dream2");
    $("#txtTotal1Dream2").css('border-color', 'red');
}
function Policy41(id) {
    debugger;
    f1(id);
    var Policy11 = parseInt($('#hdntxtPolicy11').val(), 10);
    var Policy21 = parseInt($('#hdntxtPolicy21').val(), 10);
    var Policy31 = parseInt($('#hdntxtPolicy31').val(), 10);
    if (isNaN(Policy11)) {
        Policy11 = 0;
    }
    if (isNaN(Policy21)) {
        Policy21 = 0;
    }
    if (isNaN(Policy31)) {
        Policy31 = 0;
    }
    //var Policy41 = Policy11 + Policy21 + Policy31;
    var Policy41 = $("#hdntxtPolicy41").val();
    //f2("txtPolicy41");
    var Total1Dream = $("#hdntxtTotal1Dream").val();
    if (isNaN(Total1Dream)) {
        Total1Dream = 0;
    }
    Gaptotal1 = Total1Dream - Policy41;
    $("#hdntxtGaptotal1").val(Gaptotal1.toFixed(0));
    f2("txtGaptotal1");
    $("#txtGaptotal1").css('border-color', 'red');

}
function Policy41nc() {
    debugger;
    var Policy11 = parseInt($('#hdntxtPolicy11').val(), 10);
    var Policy21 = parseInt($('#hdntxtPolicy21').val(), 10);
    var Policy31 = parseInt($('#hdntxtPolicy31').val(), 10);
    if (isNaN(Policy11)) {
        Policy11 = 0;
    }
    if (isNaN(Policy21)) {
        Policy21 = 0;
    }
    if (isNaN(Policy31)) {
        Policy31 = 0;
    }
    var Policy41 = Policy11 + Policy21 + Policy31;
    $("#hdntxtPolicy41").val(Policy41.toFixed(0));
    f2("txtPolicy41");
    var Total1Dream = $("#hdntxtTotal1Dream").val();
    if (isNaN(Total1Dream)) {
        Total1Dream = 0;
    }
    Gaptotal1 = Total1Dream - Policy41;
    $("#hdntxtGaptotal1").val(Gaptotal1.toFixed(0));
    f2("txtGaptotal1");
    $("#txtGaptotal1").css('border-color', 'red');

}

function Policy42nc() {
    var Policy12 = parseInt($('#hdntxtPolicy12').val(), 10);
    var Policy22 = parseInt($('#hdntxtPolicy22').val(), 10);
    var Policy32 = parseInt($('#hdntxtPolicy32').val(), 10);
    if (isNaN(Policy12)) {
        Policy12 = 0;
    }
    if (isNaN(Policy22)) {
        Policy22 = 0;
    }
    if (isNaN(Policy32)) {
        Policy32 = 0;
    }
    var Policy42 = Policy12 + Policy22 + Policy32;
    $("#hdntxtPolicy42").val(Policy42.toFixed(0));
    f2("txtPolicy42");
    var Total1Dream2 = parseInt($("#hdntxtTotal1Dream2").val(), 10);
    if (isNaN(Total1Dream2)) {
        Total1Dream2 = 0;
    }
    var Gaptotal2 = Total1Dream2 - Policy42;
    $("#hdntxtGaptotal2").val(Gaptotal2.toFixed(0));
    f2("txtGaptotal2");
    $("#txtGaptotal2").css('border-color', 'red');

}
function Policy42(id) {
    debugger;
    f1(id);
    var Policy12 = parseInt($('#hdntxtPolicy12').val(), 10);
    var Policy22 = parseInt($('#hdntxtPolicy22').val(), 10);
    var Policy32 = parseInt($('#hdntxtPolicy32').val(), 10);
    if (isNaN(Policy12)) {
        Policy12 = 0;
    }
    if (isNaN(Policy22)) {
        Policy22 = 0;
    }
    if (isNaN(Policy32)) {
        Policy32 = 0;
    }
  //  var Policy42 = Policy12 + Policy22 + Policy32;
    var Policy42 = $("#hdntxtPolicy42").val();
    //f2("txtPolicy42");
    var Total1Dream2 = parseInt($("#hdntxtTotal1Dream2").val(), 10);
    if (isNaN(Total1Dream2)) {
        Total1Dream2 = 0;
    }
    var Gaptotal2 = Total1Dream2 - Policy42;
    $("#hdntxtGaptotal2").val(Gaptotal2.toFixed(0));
    f2("txtGaptotal2");
    $("#txtGaptotal2").css('border-color', 'red');

}
function Totalmonthlyexpensescomma(id) {

    f1(id);
    var Food1 = parseInt($("#hdntxtFood1").val(), 10);
    var Water1 = parseInt($("#hdntxtWater1").val(), 10);
    var Rent1 = parseInt($("#hdntxtRent1").val(), 10);
    var LeaseInstallment1 = parseInt($("#hdntxtLeaseInstallment1").val(), 10);
    var Transport1 = parseInt($("#hdntxtTransport1").val(), 10);
    var Medicine1 = parseInt($("#hdntxtMedicine1").val(), 10);
    var EducationExpense1 = parseInt($("#hdntxtEducationExpense1").val(), 10);
    var Clothes1 = parseInt($("#hdntxtClothes1").val(), 10);
    var Entertainment1 = parseInt($("#hdntxtEntertainment1").val(), 10);
    var CharityDonation1 = parseInt($("#hdntxtCharityDonation1").val(), 10);
    var OtherExpense1 = parseInt($("#hdntxtOtherExpense1").val(), 10);
    if (isNaN(Food1)) {
        Food1 = 0;
    }
    if (isNaN(Water1)) {
        Water1 = 0;
    }
    if (isNaN(Rent1)) {
        Rent1 = 0;
    }
    if (isNaN(LeaseInstallment1)) {
        LeaseInstallment1 = 0;
    }
    if (isNaN(Transport1)) {
        Transport1 = 0;
    }
    if (isNaN(Medicine1)) {
        Medicine1 = 0;
    }
    if (isNaN(EducationExpense1)) {
        EducationExpense1 = 0;
    }
    if (isNaN(Clothes1)) {
        Clothes1 = 0;
    }
    if (isNaN(Entertainment1)) {
        Entertainment1 = 0;
    }
    if (isNaN(CharityDonation1)) {
        CharityDonation1 = 0;
    }
    if (isNaN(OtherExpense1)) {
        OtherExpense1 = 0;
    }
    var Totalmonthlyexpenses = Food1 + Water1 + Rent1 + LeaseInstallment1 + Transport1 + Medicine1 + EducationExpense1 + Clothes1 + Entertainment1 + CharityDonation1 + OtherExpense1;
    $("#hdntxtTotalmonthlyexpenses").val(Totalmonthlyexpenses);
    f2("txtTotalmonthlyexpenses");

    var Rate = parseInt($("#txtInflationrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var food2 = Food1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(food2)) {
        food2 = 0;
    }
    $("#hdntxtFood2").val(food2.toFixed(0));
    f2("txtFood2");
    var Water2 = Water1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Water2)) {
        food2 = 0;
    }
    $("#hdntxtWater2").val(Water2.toFixed(0));
    f2("txtWater2");
    var Rent2 = Rent1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Rent2)) {
        Rent2 = 0;
    }
    $("#hdntxtRent2").val(Rent2.toFixed(0));
    f2("txtRent2");
    var LeaseInstallment2 = LeaseInstallment1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(LeaseInstallment2)) {
        LeaseInstallment2 = 0;
    }
    $("#hdntxtLeaseInstallment2").val(LeaseInstallment2.toFixed(0));
    f2("txtLeaseInstallment2");
    var Transport2 = Transport1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Transport2)) {
        Transport2 = 0;
    }
    $("#hdntxtTransport2").val(Transport2.toFixed(0));
    f2("txtTransport2");
    var Medicine2 = Medicine1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Medicine2)) {
        Medicine2 = 0;
    }
    $("#hdntxtMedicine2").val(Medicine2.toFixed(0));
    f2("txtMedicine2");
    var EducationExpense2 = EducationExpense1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(EducationExpense2)) {
        EducationExpense2 = 0;
    }
    $("#hdntxtEducationExpense2").val(EducationExpense2.toFixed(0));
    f2("txtEducationExpense2");
    var Clothes2 = Clothes1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Clothes2)) {
        Clothes2 = 0;
    }
    $("#hdntxtClothes2").val(Clothes2.toFixed(0));
    f2("txtClothes2");
    var Entertainment2 = Entertainment1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Entertainment2)) {
        Entertainment2 = 0;
    }
    $("#hdntxtEntertainment2").val(Entertainment2.toFixed(0));
    f2("txtEntertainment2");
    var CharityDonation2 = CharityDonation1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(CharityDonation2)) {
        CharityDonation2 = 0;
    }
    $("#hdntxtCharityDonation2").val(CharityDonation2.toFixed(0));
    f2("txtCharityDonation2");
    var OtherExpense2 = OtherExpense1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(OtherExpense2)) {
        OtherExpense2 = 0;
    }
    $("#hdntxtOtherExpense2").val(OtherExpense2.toFixed(0));
    f2("txtOtherExpense2");
    Estimatemonthlyexpenses();
}
function Totalmonthlyexpenses() {
  
    var Food1 = parseInt($("#hdntxtFood1").val(), 10);
    var Water1 = parseInt($("#hdntxtWater1").val(), 10);
    var Rent1 = parseInt($("#hdntxtRent1").val(), 10);
    var LeaseInstallment1 = parseInt($("#hdntxtLeaseInstallment1").val(), 10);
    var Transport1 = parseInt($("#hdntxtTransport1").val(), 10);
    var Medicine1 = parseInt($("#hdntxtMedicine1").val(), 10);
    var EducationExpense1 = parseInt($("#hdntxtEducationExpense1").val(), 10);
    var Clothes1 = parseInt($("#hdntxtClothes1").val(), 10);
    var Entertainment1 = parseInt($("#hdntxtEntertainment1").val(), 10);
    var CharityDonation1 = parseInt($("#hdntxtCharityDonation1").val(), 10);
    var OtherExpense1 = parseInt($("#hdntxtOtherExpense1").val(), 10);
    if (isNaN(Food1)) {
        Food1 = 0;
    }
    if (isNaN(Water1)) {
        Water1 = 0;
    }
    if (isNaN(Rent1)) {
        Rent1 = 0;
    }
    if (isNaN(LeaseInstallment1)) {
        LeaseInstallment1 = 0;
    }
    if (isNaN(Transport1)) {
        Transport1 = 0;
    }
    if (isNaN(Medicine1)) {
        Medicine1 = 0;
    }
    if (isNaN(EducationExpense1)) {
        EducationExpense1 = 0;
    }
    if (isNaN(Clothes1)) {
        Clothes1 = 0;
    }
    if (isNaN(Entertainment1)) {
        Entertainment1 = 0;
    }
    if (isNaN(CharityDonation1)) {
        CharityDonation1 = 0;
    }
    if (isNaN(OtherExpense1)) {
        OtherExpense1 = 0;
    }
    var Totalmonthlyexpenses = Food1 + Water1 + Rent1 + LeaseInstallment1 + Transport1 + Medicine1 + EducationExpense1 + Clothes1 + Entertainment1 + CharityDonation1 + OtherExpense1;
    $("#hdntxtTotalmonthlyexpenses").val(Totalmonthlyexpenses);
    f2("txtTotalmonthlyexpenses");

    var Rate = parseInt($("#txtInflationrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var food2 = Food1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(food2)) {
        food2 = 0;
    }
    $("#hdntxtFood2").val(food2.toFixed(0));
    f2("txtFood2");
    var Water2 = Water1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Water2)) {
        food2 = 0;
    }
    $("#hdntxtWater2").val(Water2.toFixed(0));
    f2("txtWater2");
    var Rent2 = Rent1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Rent2)) {
        Rent2 = 0;
    }
    $("#hdntxtRent2").val(Rent2.toFixed(0));
    f2("txtRent2");
    var LeaseInstallment2 = LeaseInstallment1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(LeaseInstallment2)) {
        LeaseInstallment2 = 0;
    }
    $("#hdntxtLeaseInstallment2").val(LeaseInstallment2.toFixed(0));
    f2("txtLeaseInstallment2");
    var Transport2 = Transport1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Transport2)) {
        Transport2 = 0;
    }
    $("#hdntxtTransport2").val(Transport2.toFixed(0));
    f2("txtTransport2");
    var Medicine2 = Medicine1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Medicine2)) {
        Medicine2 = 0;
    }
    $("#hdntxtMedicine2").val(Medicine2.toFixed(0));
    f2("txtMedicine2");
    var EducationExpense2 = EducationExpense1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(EducationExpense2)) {
        EducationExpense2 = 0;
    }
    $("#hdntxtEducationExpense2").val(EducationExpense2.toFixed(0));
    f2("txtEducationExpense2");
    var Clothes2 = Clothes1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Clothes2)) {
        Clothes2 = 0;
    }
    $("#hdntxtClothes2").val(Clothes2.toFixed(0));
    f2("txtClothes2");
    var Entertainment2 = Entertainment1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Entertainment2)) {
        Entertainment2 = 0;
    }
    $("#hdntxtEntertainment2").val(Entertainment2.toFixed(0));
    f2("txtEntertainment2");
    var CharityDonation2 = CharityDonation1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(CharityDonation2)) {
        CharityDonation2 = 0;
    }
    $("#hdntxtCharityDonation2").val(CharityDonation2.toFixed(0));
    f2("txtCharityDonation2");
    var OtherExpense2 = OtherExpense1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(OtherExpense2)) {
        OtherExpense2 = 0;
    }
    $("#hdntxtOtherExpense2").val(OtherExpense2.toFixed(0));
    f2("txtOtherExpense2");
    Estimatemonthlyexpenses();
}
function Estimatemonthlyexpenses() {
    
    var Food2 = parseInt($("#hdntxtFood2").val(), 10);
    var Water2 = parseInt($("#hdntxtWater2").val(), 10);
    var Rent2 = parseInt($("#hdntxtRent2").val(), 10);
    var LeaseInstallment2 = parseInt($("#hdntxtLeaseInstallment2").val(), 10);
    var Transport2 = parseInt($("#hdntxtTransport2").val(), 10);
    var Medicine2 = parseInt($("#hdntxtMedicine2").val(), 10);
    var EducationExpense2 = parseInt($("#hdntxtEducationExpense2").val(), 10);
    var Clothes2 = parseInt($("#hdntxtClothes2").val(), 10);
    var Entertainment2 = parseInt($("#hdntxtEntertainment2").val(), 10);
    var CharityDonation2 = parseInt($("#hdntxtCharityDonation2").val(), 10);
    var OtherExpense2 = parseInt($("#hdntxtOtherExpense2").val(), 10);
    if (isNaN(Food2)) {
        Food2 = 0;
    }
    if (isNaN(Water2)) {
        Water2 = 0;
    }
    if (isNaN(Rent2)) {
        Rent2 = 0;
    }
    if (isNaN(LeaseInstallment2)) {
        LeaseInstallment2 = 0;
    }
    if (isNaN(Transport2)) {
        Transport2 = 0;
    }
    if (isNaN(Medicine2)) {
        Medicine2 = 0;
    }
    if (isNaN(EducationExpense2)) {
        EducationExpense2 = 0;
    }
    if (isNaN(Clothes2)) {
        Clothes2 = 0;
    }
    if (isNaN(Entertainment2)) {
        Entertainment2 = 0;
    }
    if (isNaN(CharityDonation2)) {
        CharityDonation2 = 0;
    }
    if (isNaN(OtherExpense2)) {
        OtherExpense2 = 0;
    }
    var Estimatemonthlyexpenses = Food2 + Water2 + Rent2 + LeaseInstallment2 + Transport2 + Medicine2 + EducationExpense2 + Clothes2 + Entertainment2 + CharityDonation2 + OtherExpense2;
    $("#hdntxtEstimatemonthlyexpenses").val(Estimatemonthlyexpenses.toFixed(0));
    f2("txtEstimatemonthlyexpenses");

    var Estimatemonthlyexpenses = parseInt($("#hdntxtEstimatemonthlyexpenses").val());
    if (isNaN(Estimatemonthlyexpenses)) {
        Estimatemonthlyexpenses = 0;
    }
    var EstimatedAnnuallivingExpenses = Estimatemonthlyexpenses * 12;
    $("#hdntxtEstimatedAnnuallivingExpenses").val(EstimatedAnnuallivingExpenses.toFixed(0));
    f2("txtEstimatedAnnuallivingExpenses");


}
function ExistingOtherIncome() {
    var Estimatemonthlyexpenses = parseInt($("#hdntxtEstimatemonthlyexpenses").val());
    if (isNaN(Estimatemonthlyexpenses)) {
        Estimatemonthlyexpenses = 0;
    }
    var EstimatedAnnuallivingExpenses = Estimatemonthlyexpenses * 12;
    $("#hdntxtEstimatedAnnuallivingExpenses").val(EstimatedAnnuallivingExpenses.toFixed(0));
    f2("txtEstimatedAnnuallivingExpenses");
}
function GratuityFundComma(id) {
    f1(id);
    var Salary = parseInt($("#hdntxtRetireSalary").val(), 10);
    if (isNaN(Salary)) {
        Salary = 0;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var GratuityFund1 = parseInt($("#hdntxtGratuityFund1").val());
    if (isNaN(GratuityFund1)) {
        GratuityFund1 = 0;
    }
    
    var GratuityFund2 = ((Salary / 2) * Time) + GratuityFund1;
    $("#hdntxtGratuityFund2").val(GratuityFund2);
    f2("txtGratuityFund2");

    var EPFbalanceFund2 = parseInt($("#hdntxtEPFbalanceFund2").val());
    if (isNaN(EPFbalanceFund2)) {
        EPFbalanceFund2 = 0;
    }
    var GratuityFund2 = parseInt($("#hdntxtGratuityFund2").val());
    if (isNaN(GratuityFund2)) {
        GratuityFund2 = 0;
    }
    var EPFbalance2 = parseInt($("#hdntxtEPFbalance2").val());
    if (isNaN(EPFbalance2)) {
        EPFbalance2 = 0;
    }
    $("#txtEPFbalance2").val(EPFbalance2.toFixed(0));
    f2("txtEPFbalance2");
    var TotalMonthlyExpense = parseInt(EPFbalance2) + parseInt(GratuityFund2) + parseInt(EPFbalanceFund2);
    $("#hdntxtTotalMonthlyExpense").val(TotalMonthlyExpense.toFixed(0));
    f2("txtTotalMonthlyExpense");
    TotalFinancial();
}
function GratuityFund() {
    var Salary = parseInt($("#hdntxtRetireSalary").val(), 10);
    if (isNaN(Salary)) {
        Salary = 0;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var GratuityFund1 = parseInt($("#hdntxtGratuityFund1").val());
    if (isNaN(GratuityFund1)) {
        GratuityFund1 = 0;
    }
    
    var GratuityFund2 = ((Salary / 2) * Time) + GratuityFund1;
    $("#hdntxtGratuityFund2").val(GratuityFund2);
    f2("txtGratuityFund2");

    var EPFbalanceFund2 = parseInt($("#hdntxtEPFbalanceFund2").val());
    if (isNaN(EPFbalanceFund2)) {
        EPFbalanceFund2 = 0;
    }
    var GratuityFund2 = parseInt($("#hdntxtGratuityFund2").val());
    if (isNaN(GratuityFund2)) {
        GratuityFund2 = 0;
    }
    var EPFbalance2 = parseInt($("#hdntxtEPFbalance2").val());
    if (isNaN(EPFbalance2)) {
        EPFbalance2 = 0;
    }
    $("#txtEPFbalance2").val(EPFbalance2.toFixed(0));
    f2("txtEPFbalance2");
    var TotalMonthlyExpense = parseInt(EPFbalance2) + parseInt(GratuityFund2) + parseInt(EPFbalanceFund2);
    $("#hdntxtTotalMonthlyExpense").val(TotalMonthlyExpense.toFixed(0));
    f2("txtTotalMonthlyExpense");
    TotalFinancial();
}
//function EstimatedGratuityFund()
//{
//
//    var EPFbalancefund2 = parseInt( $("#txtEPFbalancefund2").val(), 10);
//    var GratuityFund1 = parseInt( $("#txtGratuityFund1").val(), 10);
//    if (isNaN(EPFbalancefund2)) {
//        EPFbalancefund2 = 0;
//    }
//    if (isNaN(GratuityFund1)) {
//        GratuityFund1 = 0;
//    }
//    GratuityFund2 = EPFbalancefund2+GratuityFund1;
//    $("#txtGratuityFund2").val(GratuityFund2);
//}
function EstimatedEPFbalanceComma(id) {
    f1(id);
    var EPFbalance1 = parseInt($("#hdntxtEPFbalance1").val(), 10);
    if (isNaN(EPFbalance1)) {
        EPFbalance1 = 0;
    }
    var Rate = parseInt($("#txtInterestrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var EPFbalance2 = EPFbalance1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(EPFbalance2)) {
        EPFbalance2 = 0;
    }
    $("#hdntxtEPFbalance2").val(EPFbalance2.toFixed(0));
    f2("txtEPFbalance2");

    var GratuityFund2 = parseInt($("#hdntxtGratuityFund2").val());
    if (isNaN(GratuityFund2)) {
        GratuityFund2 = 0;
    }
    var EPFbalanceFund2 = parseInt($("#hdntxtEPFbalanceFund2").val());
    if (isNaN(EPFbalanceFund2)) {
        EPFbalanceFund2 = 0;
    }
    var TotalMonthlyExpense = parseInt(EPFbalance2) + parseInt(GratuityFund2) + parseInt(EPFbalanceFund2);
    $("#hdntxtTotalMonthlyExpense").val(TotalMonthlyExpense.toFixed(0));
    f2("txtTotalMonthlyExpense");
    TotalFinancial();
}
function EstimatedEPFbalance() {
    
    var EPFbalance1 = parseInt($("#hdntxtEPFbalance1").val(), 10);
    if (isNaN(EPFbalance1)) {
        EPFbalance1 = 0;
    }
    var Rate = parseInt($("#txtInterestrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var EPFbalance2 = EPFbalance1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(EPFbalance2)) {
        EPFbalance2 = 0;
    }
    $("#hdntxtEPFbalance2").val(EPFbalance2.toFixed(0));
    f2("txtEPFbalance2");

    var GratuityFund2 = parseInt($("#hdntxtGratuityFund2").val());
    if (isNaN(GratuityFund2)) {
        GratuityFund2 = 0;
    }
    var EPFbalanceFund2 = parseInt($("#hdntxtEPFbalanceFund2").val());
    if (isNaN(EPFbalanceFund2)) {
        EPFbalanceFund2 = 0;
    }
    var TotalMonthlyExpense = parseInt(EPFbalance2) + parseInt(GratuityFund2) + parseInt(EPFbalanceFund2);
    $("#hdntxtTotalMonthlyExpense").val(TotalMonthlyExpense.toFixed(0));
    f2("txtTotalMonthlyExpense");
    TotalFinancial();
}
function EstimatedEPFbalanceFundComma(id) {
    f1(id);
    var EPFbalanceFund1 = parseInt($("#hdntxtEPFbalanceFund1").val(), 10);
    if (isNaN(EPFbalanceFund1)) {
        EPFbalanceFund1 = 0;
    }
    var Rate = parseInt($("#txtInterestrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var EPFbalanceFund2 = EPFbalanceFund1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(EPFbalanceFund2)) {
        EPFbalanceFund2 = 0;
    }
    $("#hdntxtEPFbalanceFund2").val(EPFbalanceFund2.toFixed(0));
    f2("txtEPFbalanceFund2");
    var EPFbalance2 = parseInt($("#hdntxtEPFbalance2").val());
    if (isNaN(EPFbalance2)) {
        EPFbalance2 = 0;
    }
    var GratuityFund2 = parseInt($("#hdntxtGratuityFund2").val());
    if (isNaN(GratuityFund2)) {
        GratuityFund2 = 0;
    }

    var TotalMonthlyExpense = parseInt(EPFbalance2) + parseInt(GratuityFund2) + parseInt(EPFbalanceFund2);
    $("#hdntxtTotalMonthlyExpense").val(TotalMonthlyExpense.toFixed(0));
    f2("txtTotalMonthlyExpense");
    TotalFinancial();
}
function EstimatedEPFbalanceFund() {
    
    var EPFbalanceFund1 = parseInt($("#hdntxtEPFbalanceFund1").val(), 10);
    if (isNaN(EPFbalanceFund1)) {
        EPFbalanceFund1 = 0;
    }
    var Rate = parseInt($("#txtInterestrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var EPFbalanceFund2 = EPFbalanceFund1 * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(EPFbalanceFund2)) {
        EPFbalanceFund2 = 0;
    }
    $("#hdntxtEPFbalanceFund2").val(EPFbalanceFund2.toFixed(0));
    f2("txtEPFbalanceFund2");
    var EPFbalance2 = parseInt($("#hdntxtEPFbalance2").val());
    if (isNaN(EPFbalance2)) {
        EPFbalance2 = 0;
    }
    var GratuityFund2 = parseInt($("#hdntxtGratuityFund2").val());
    if (isNaN(GratuityFund2)) {
        GratuityFund2 = 0;
    }

    var TotalMonthlyExpense = parseInt(EPFbalance2) + parseInt(GratuityFund2) + parseInt(EPFbalanceFund2);
    $("#hdntxtTotalMonthlyExpense").val(TotalMonthlyExpense.toFixed(0));
    f2("txtTotalMonthlyExpense");
    TotalFinancial();
}
function TotalFinancialValue(id) {
  
    f1(id);
    var Financial0 = parseInt($("#hdntxtFinancial0").val(), 10);
    var Financial1 = parseInt($("#hdntxtFinancial1").val(), 10);
    var Financial2 = parseInt($("#hdntxtFinancial2").val(), 10);
    var Financial3 = parseInt($("#hdntxtFinancial3").val(), 10);
    var Financial4 = parseInt($("#hdntxtFinancial4").val(), 10);
    if (isNaN(Financial0)) {
        Financial0 = 0;
    }
    if (isNaN(Financial1)) {
        Financial1 = 0;
    }
    if (isNaN(Financial2)) {
        Financial2 = 0;
    }
    if (isNaN(Financial3)) {
        Financial3 = 0;
    }
    if (isNaN(Financial4)) {
        Financial4 = 0;
    }
    var TotalMonthlyExpense = parseInt($("#hdntxtTotalMonthlyExpense").val());
    f2("txtTotalMonthlyExpense");
    if (isNaN(TotalMonthlyExpense)) {
        TotalMonthlyExpense = 0;
    }
    var FundBalanceTotal = TotalMonthlyExpense - Financial0 - Financial1 - Financial2 - Financial3 - Financial4;
    $("#hdntxtFundBalanceTotal").val(FundBalanceTotal.toFixed(0));
    f2("txtFundBalanceTotal");
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var Rate = parseInt($("#txtInterestrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    PerAnnumIncome = (FundBalanceTotal  * Rate)/100;
    $("#hdntxtPerAnnumIncome").val(PerAnnumIncome.toFixed(0));
    f2("txtPerAnnumIncome");
    AnnualIncomeSurplusTotal();
}
function TotalFinancial() {
    
    var Financial0 = parseInt($("#hdntxtFinancial0").val(), 10);
    var Financial1 = parseInt($("#hdntxtFinancial1").val(), 10);
    var Financial2 = parseInt($("#hdntxtFinancial2").val(), 10);
    var Financial3 = parseInt($("#hdntxtFinancial3").val(), 10);
    var Financial4 = parseInt($("#hdntxtFinancial4").val(), 10);
    if (isNaN(Financial0)) {
        Financial0 = 0;
    }
    if (isNaN(Financial1)) {
        Financial1 = 0;
    }
    if (isNaN(Financial2)) {
        Financial2 = 0;
    }
    if (isNaN(Financial3)) {
        Financial3 = 0;
    }
    if (isNaN(Financial4)) {
        Financial4 = 0;
    }
    var TotalMonthlyExpense = parseInt($("#hdntxtTotalMonthlyExpense").val());
    f2("txtTotalMonthlyExpense");
    if (isNaN(TotalMonthlyExpense)) {
        TotalMonthlyExpense = 0;
    }
    var FundBalanceTotal = TotalMonthlyExpense - Financial0 - Financial1 - Financial2 - Financial3 - Financial4;
    $("#hdntxtFundBalanceTotal").val(FundBalanceTotal.toFixed(0));
    f2("txtFundBalanceTotal");
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var Rate = parseInt($("#txtInterestrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    PerAnnumIncome = (FundBalanceTotal * Rate) / 100;
    $("#hdntxtPerAnnumIncome").val(PerAnnumIncome.toFixed(0));
    f2("txtPerAnnumIncome");
    AnnualIncomeSurplusTotal();
}
function AnnualIncomeSurplusTotalcomma(id) {
    
    f1(id);
    var Exsitingotherincome = parseInt($("#hdntxtExsitingotherincome").val());
    if (isNaN(Exsitingotherincome)) {
        Exsitingotherincome = 0;
    }
    var PerAnnumIncome = parseInt($("#hdntxtPerAnnumIncome").val());
    if (isNaN(PerAnnumIncome)) {
        PerAnnumIncome = 0;
    }
    var EstimatedAnnuallivingExpenses = parseInt($("#hdntxtEstimatedAnnuallivingExpenses").val());
    if (isNaN(EstimatedAnnuallivingExpenses)) {
        EstimatedAnnuallivingExpenses = 0;
    }
    var AnnualIncomeSurplus =parseInt(EstimatedAnnuallivingExpenses) - parseInt(PerAnnumIncome) - parseInt(Exsitingotherincome) ;
    $("#hdntxtAnnualIncomeSurplus").val(AnnualIncomeSurplus.toFixed(0));
    f2("txtAnnualIncomeSurplus");
    MonthlyPensionGap();
}
function AnnualIncomeSurplusTotal() {
    var Exsitingotherincome = parseInt($("#hdntxtExsitingotherincome").val());
    if (isNaN(Exsitingotherincome)) {
        Exsitingotherincome = 0;
    }
    var PerAnnumIncome = parseInt($("#hdntxtPerAnnumIncome").val());
    if (isNaN(PerAnnumIncome)) {
        PerAnnumIncome = 0;
    }
    var EstimatedAnnuallivingExpenses = parseInt($("#hdntxtEstimatedAnnuallivingExpenses").val());
    if (isNaN(EstimatedAnnuallivingExpenses)) {
        EstimatedAnnuallivingExpenses = 0;
    }
    var AnnualIncomeSurplus =parseInt(EstimatedAnnuallivingExpenses) - parseInt(PerAnnumIncome) - parseInt(Exsitingotherincome) ;
    $("#hdntxtAnnualIncomeSurplus").val(AnnualIncomeSurplus.toFixed(0));
    f2("txtAnnualIncomeSurplus");
    MonthlyPensionGap();
}

function MonthlyPensionGap() {
   
    //var Exsitingotherincome = parseInt($("#hdntxtExsitingotherincome").val());
    //if (isNaN(Exsitingotherincome)) {
    //    Exsitingotherincome = 0;
    //}
    var AnnualIncomeSurplus = parseInt($("#hdntxtAnnualIncomeSurplus").val());
    if (isNaN(AnnualIncomeSurplus)) {
        AnnualIncomeSurplus = 0;
    }
    var MonthlyPensionGap = (parseInt(AnnualIncomeSurplus))/ 12;
    $("#hdntxtMonthlyPensionGap").val(MonthlyPensionGap.toFixed(0));
    f2("txtMonthlyPensionGap");
}
function DirectEstimatemonthlyexpensescomma(id) {
    f1(id);
    var Totalmonthlyexpenses = parseInt($("#hdntxtTotalmonthlyexpenses").val());
    if (isNaN(Totalmonthlyexpenses)) {
        Totalmonthlyexpenses = 0;
    }
    var Rate = parseInt($("#txtInflationrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var Estimatemonthlyexpenses = Totalmonthlyexpenses * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Totalmonthlyexpenses)) {
        EPFbalance2 = 0;
    }
    $("#hdntxtEstimatemonthlyexpenses").val(Estimatemonthlyexpenses.toFixed(0));
    f2("txtEstimatemonthlyexpenses");
    ExistingOtherIncome()
}
function DirectEstimatemonthlyexpenses() {
    
    var Totalmonthlyexpenses = parseInt($("#hdntxtTotalmonthlyexpenses").val());
    if (isNaN(Totalmonthlyexpenses)) {
        Totalmonthlyexpenses = 0;
    }
    var Rate = parseInt($("#txtInflationrate").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyear").val(), 10);
    if (isNaN(Time)) {
        Time = 10;
    }
    var Estimatemonthlyexpenses = Totalmonthlyexpenses * Math.pow((1 + (Rate / 100)), Time)
    if (isNaN(Totalmonthlyexpenses)) {
        EPFbalance2 = 0;
    }
    $("#hdntxtEstimatemonthlyexpenses").val(Estimatemonthlyexpenses.toFixed(0));
    f2("txtEstimatemonthlyexpenses");
    ExistingOtherIncome()
}

function CalculateYear() {
    clearValidation();
    writeMultipleMessage("error", "", "txtnoofyear");
    var noofyear = parseInt($("#txtnoofyear").val());
    if (noofyear > 25 || noofyear < 10) {
        writeMultipleMessage("error", "Year ranges from 10 to 25 years", "txtnoofyear");

    }
    else {
        var FromYear = parseInt($("#txtFromYear").val());
        if (isNaN(FromYear)) {
            FromYear = 0;
        }
        var Time = parseInt($("#txtnoofyear").val(), 10);
        if (isNaN(Time)) {
            Time = 10;
        }
        var ToYear = parseInt(FromYear) + parseInt(Time);
        if (isNaN(ToYear)) {
            ToYear = 0;
        }
        $("#txtToYear").val(ToYear);
        if ($("#txtFood1").is(":visible")) {
            Totalmonthlyexpenses();
        }
        else {
            DirectEstimatemonthlyexpenses();
        }
        EstimatedEPFbalance();
        EstimatedEPFbalanceFund();
        GratuityFund();
        TotalFinancial();
    }
}

function IncrementInflationrate() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInflationrate");
    var Inflationrate = parseInt($("#txtInflationrate").val());
    if (Inflationrate >= 15) {
        writeMultipleMessage("error", "Inflation Rate maximum limit is 15", "txtInflationrate");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate += 1;
        $("#txtInflationrate").val(Inflationrate);

        if ($("#txtFood1").is(":visible")) {
            Totalmonthlyexpenses();
        }
        else {
            DirectEstimatemonthlyexpenses();
        }
    }
}
function DecrementInflationrate() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInflationrate");

    var Inflationrate = parseInt($("#txtInflationrate").val());
    if (Inflationrate <= 5) {
        writeMultipleMessage("error", "Inflation Rate minimum limit is 5", "txtInflationrate");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate -= 1;
        $("#txtInflationrate").val(Inflationrate);
        DirectEstimatemonthlyexpenses();
        if ($("#txtFood1").is(":visible")) {
            Totalmonthlyexpenses();
        }
        else {
            DirectEstimatemonthlyexpenses();
        }
    }
}
function DecrementYear() {
    clearValidation();
    writeMultipleMessage("error", "", "txtnoofyear");
    var Year = parseInt($("#txtnoofyear").val());
    if (Year <= 10) {
        writeMultipleMessage("error", "Year minimum limit is 10", "txtnoofyear");

    }
    else {
        if (isNaN(Year)) {
            Year = 0;
        }
        Year -= 1;
        $("#txtnoofyear").val(Year);
        CalculateYear()
        if ($("#txtFood1").is(":visible")) {
            Totalmonthlyexpenses();
        }
        else {
            DirectEstimatemonthlyexpenses();
        }
        EstimatedEPFbalance();
        EstimatedEPFbalanceFund();
        GratuityFund();
        TotalFinancial();
    }
}
function IncrementYear() {
    clearValidation();
    writeMultipleMessage("error", "", "txtnoofyear");

    var Year = parseInt($("#txtnoofyear").val());
    if (Year >= 25) {
        writeMultipleMessage("error", "Year maximum limit is 25", "txtnoofyear");
    }
    else {
        if (isNaN(Year)) {
            Year = 0;
        }
        Year += 1;
        $("#txtnoofyear").val(Year);
        CalculateYear()
        if ($("#txtFood1").is(":visible")) {
            Totalmonthlyexpenses();
        }
        else {
            DirectEstimatemonthlyexpenses();
        }
        EstimatedEPFbalance();
        EstimatedEPFbalanceFund();
        GratuityFund();
        TotalFinancial();
    }
}
function DecrementInterestrate() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInterestrate");

    var Interestnrate = parseInt($("#txtInterestrate").val());
    if (Interestnrate <= 4) {
        writeMultipleMessage("error", "Interest Rate minimum limit is 4", "txtInterestrate");
    }
    else {
        if (isNaN(Interestnrate)) {
            Interestnrate = 8;
        }
        Interestnrate -= 1;
        $("#txtInterestrate").val(Interestnrate);
        EstimatedEPFbalance();
        EstimatedEPFbalanceFund();
        GratuityFund();
        TotalFinancial();
    }
}
function IncrementInterestrate() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInterestrate");

    var Interestnrate = parseInt($("#txtInterestrate").val());
    if (Interestnrate >= 18) {
        writeMultipleMessage("error", "Interest Rate maximum limit is 18", "txtInterestrate");
    }
    else {


        if (isNaN(Interestnrate)) {
            Interestnrate = 8;
        }
        Interestnrate += 1;
        $("#txtInterestrate").val(Interestnrate);
        EstimatedEPFbalance();
        EstimatedEPFbalanceFund();
        GratuityFund();
        TotalFinancial();
    }
}
function ValidateInflationrate() {
    clearValidation();
    var Inflationrate = $("#txtInflationrate").val();
    if (Inflationrate > 15 || Inflationrate < 5) {
        writeMultipleMessage("error", "Inflation rate ranges from 5 to 15", "txtInflationrate");
    }
    else {
        writeMultipleMessage("error", "", "txtInflationrate");
        DirectEstimatemonthlyexpenses();
        if ($("#txtFood1").is(":visible")) {
            Totalmonthlyexpenses();
        }
        else {
            DirectEstimatemonthlyexpenses();
        }

    }
}
function ValidateInterestrate() {
    clearValidation();
    var Interestrate = $("#txtInterestrate").val();
    if (Interestrate > 18 || Interestrate < 4) {
        writeMultipleMessage("error", "Interest rate ranges from 4 to 18", "txtInterestrate");
    }
    else {
        writeMultipleMessage("error", "", "txtInterestrate");
        EstimatedEPFbalance();
        EstimatedEPFbalanceFund();
        GratuityFund();
        TotalFinancial();
    }
}

function ageProspectCountInfo() {
    var dob = $('#dtProspectDOBInfo').val();
    var controlid = 'dtProspectDOBInfo';
    writeMultipleMessage("error", "", controlid);
    if (dob == "") {
        $('#txtAgeInfo').val('');
        writeMultipleMessage("error", "Please enter valid Date Of Birth.", controlid);

    }
    else {
        writeMultipleMessage("error", "", controlid);
    }
    if (!ValidateDOB(controlid)) {
        $('#txtAgeInfo').val('');
        writeMultipleMessage("error", "Please enter valid Date Of Birth. It cannot be future date.", controlid);
    }
    else {
        if (dob != '') {
            writeMultipleMessage("error", "", controlid);
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
            $('#txtAgeInfo').val(calYear);
            var ProspectAge = $('#txtAgeInfo').val();
            $('#txtprospectAge').val(ProspectAge);
            $('#ProspectDOB').val(dob);
        }
    }
}
function Lumpsumrequirement2017() {

    var ProtectYourWealth = parseInt($("#hdntxtProtectYourWealth").val());
    if (isNaN(ProtectYourWealth)) {
        ProtectYourWealth = 0;
    }
    var Rate = parseInt($("#txtInflationrateFNA").val(), 10);
    if (isNaN(Rate)) {
        Rate = 8;
    }
    var Time = parseInt($("#txtnoofyearFNA").val(), 10);
    if (isNaN(Time)) {
        Time = 0;
    }
    var ProtectYourWealth2 = ProtectYourWealth * Math.pow((1 + (Rate / 100)), Time);
    $("#hdntxtProtectYourWealth2").val(ProtectYourWealth2.toFixed(0));
    f2("txtProtectYourWealth");

    var ProtectYourMonthlyIncome = parseInt($("#txtProtectYourMonthlyIncome").val());
    if (isNaN(ProtectYourMonthlyIncome)) {
        ProtectYourMonthlyIncome = 0;
    }
    var ProtectYourMonthlyIncome2 = ProtectYourMonthlyIncome * Math.pow((1 + (Rate / 100)), Time);
    if (isNaN(ProtectYourMonthlyIncome2)) {
        ProtectYourMonthlyIncome2 = 0;
    }
    //$("#txtProtectYourMonthlyIncome2").val(ProtectYourMonthlyIncome2.toFixed(0));
    Total1Dream2();
}
function DecrementYearFNA() {
    clearValidation();
    writeMultipleMessage("error", "", "txtnoofyearFNA");
    var Year = parseInt($("#txtnoofyearFNA").val());
    if (Year <= 10) {
        writeMultipleMessage("error", "Year minimum limit is 10", "txtnoofyearFNA");

    }
    else {
        if (isNaN(Year)) {
            Year = 0;
        }
        Year -= 1;
        $("#txtnoofyearFNA").val(Year);
        CalculateYearFNA();
        RequirementTotal();
    }
}
function IncrementYearFNA() {
    clearValidation();
    writeMultipleMessage("error", "", "txtnoofyearFNA");

    var Year = parseInt($("#txtnoofyearFNA").val());
    if (Year >= 25) {
        writeMultipleMessage("error", "Year maximum limit is 25", "txtnoofyearFNA");
    }
    else {
        if (isNaN(Year)) {
            Year = 0;
        }
        Year += 1;
        $("#txtnoofyearFNA").val(Year);
        CalculateYearFNA();
        RequirementTotal();
    }
}
function CalculateYearFNA() {
    clearValidation();
    writeMultipleMessage("error", "", "txtnoofyearFNA");
    var noofyear = $("#txtnoofyearFNA").val();
    if (noofyear > 25 || noofyear < 10 || noofyear == "" ) {
        writeMultipleMessage("error", "Year ranges from 10 to 25 years", "txtnoofyearFNA");

    }
    else {
        var FromYear = parseInt($("#txtFromYearFNA").val());
        if (isNaN(FromYear)) {
            FromYear = 0;
        }
        var Time = parseInt($("#txtnoofyearFNA").val(), 10);
        if (isNaN(Time)) {
            Time = 10;
        }
        var ToYear = parseInt(FromYear) + parseInt(Time);
        if (isNaN(ToYear)) {
            ToYear = 0;
        }
        $("#txtToYearFNA").val(ToYear);
        RequirementTotal();
    }
}
function ValidateInflationrateFNA() {
    
    clearValidation();
    var Inflationrate = $("#txtInflationrateFNA").val();
    if (Inflationrate > 15 || Inflationrate < 5 || Inflationrate == "") {
        writeMultipleMessage("error", "Inflation rate ranges from 5 to 15", "txtInflationrateFNA");
    }
    else {
        writeMultipleMessage("error", "", "txtInflationrateFNA");
        RequirementTotal();
    }
}
function ValidateInterestrateFNA() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInterestrateFNA");

    var Interestrate = $("#txtInterestrateFNA").val();
    if (Interestrate > 18 || Interestrate < 4 || Interestrate == "") {
        writeMultipleMessage("error", "Intrest rate ranges from 4 to 18", "txtInterestrateFNA");

    }
}
function IncrementInflationrateFNA() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInflationrateFNA");
    var Inflationrate = parseInt($("#txtInflationrateFNA").val());
    if (Inflationrate >= 15) {
        writeMultipleMessage("error", "Inflation Rate maximum limit is 15", "txtInflationrateFNA");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate += 1;
        $("#txtInflationrateFNA").val(Inflationrate);
        RequirementTotal();

    }
}
function DecrementInflationrateFNA() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInflationrateFNA");

    var Inflationrate = parseInt($("#txtInflationrateFNA").val());
    if (Inflationrate <= 5) {
        writeMultipleMessage("error", "Inflation Rate minimum limit is 5", "txtInflationrateFNA");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate -= 1;
        $("#txtInflationrateFNA").val(Inflationrate);
        RequirementTotal();
    }
}
function DecrementInterestrateFNA() {    
    clearValidation(); 
    writeMultipleMessage("error", "", "txtInterestrateFNA");

    var Interestnrate = parseInt($("#txtInterestrateFNA").val());
    if (Interestnrate <= 4) {
        writeMultipleMessage("error", "Interest Rate minimum limit is 4", "txtInterestrateFNA");
    }
    else {
        if (isNaN(Interestnrate)) {
            Interestnrate = 8;
        }
        Interestnrate -= 1;
        $("#txtInterestrateFNA").val(Interestnrate);

    }
}
function IncrementInterestrateFNA() {
    clearValidation();
    writeMultipleMessage("error", "", "txtInterestrateFNA");

    var Interestnrate = parseInt($("#txtInterestrateFNA").val());
    if (Interestnrate >= 18) {
        writeMultipleMessage("error", "Interest Rate maximum limit is 18", "txtInterestrateFNA");
    }
    else {
        if (isNaN(Interestnrate)) {
            Interestnrate = 8;
        }
        Interestnrate += 1;
        $("#txtInterestrateFNA").val(Interestnrate);
        AnnualIncomeTotalIntrest();
    }
}
function ageSpouseProspectCountInfo() {
    var dob = $('#dtSpouseDOBInfo').val();
    var controlid = 'dtSpouseDOBInfo';
    if (dob == "") {
        $('#txtSpouseAgeInfo').val('');
        writeMultipleMessage("error", "Please enter valid Date Of Birth.", controlid);

    }
    else {
        writeMultipleMessage("error", "", controlid);
    }
    if (!ValidateDOB(controlid)) {
        $('#txtSpouseAgeInfo').val('');
        writeMultipleMessage("error", "Please enter valid Date Of Birth. It cannot be future date.", controlid);
    }
    else {
        if (dob != '') {
            writeMultipleMessage("error", "", controlid);
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
            $('#txtSpouseAgeInfo').val(calYear + 1);
            $('#hdnSposeCurrentAge').val(calYear);
            if ($('#txtSpouseAgeInfo').val() < 19 || $('#txtSpouseAgeInfo').val() > 61) {
                writeMultipleMessage("error", "Please enter valid Date Of Birth. Age Next Birthday should be between 19 to 61", controlid);

            }
            //var ProspectAge = $('#txtAgeInfo').val();
            //$('#txtprospectAge').val(ProspectAge);
            //$('#ProspectDOB').val(dob);
        }
    }
}
function CalculateageInfo(i) {    
    var dob = $('#dtdependantDOB' + i).val();
    var controlid = 'dtdependantDOB' + i;
    if (dob == "") {
        $('#txtdependantAge' + i).val('');
        writeMultipleMessage("error", "Please enter valid Date Of Birth.", controlid);

    }
    else {
        writeMultipleMessage("error", "", controlid);

        if (!ValidateDependantDOB(controlid)) {
            $('#txtdependantAge' + i).val('');
            writeMultipleMessage("error", "Please enter valid Date Of Birth. It cannot be future date.", controlid);
        }
        else {
            if (dob != '') {
                writeMultipleMessage("error", "", controlid);
                var d = new Date();
                var currentYear = d.getFullYear();
                var currentMonth = d.getMonth() + 1;
                var currentDate = d.getDate();
                var arr = dob.split('/');
                var birthdate = arr[0];
                var birthMonth = arr[1];
                var birthYear = arr[2];
                if (birthYear > currentYear) {
                    writeMultipleMessage("error", "Please enter valid Date Of Birth. It cannot be future date.", controlid);
                    return false;
                }
                var calYear = currentYear - birthYear;
                var CalMonth = currentMonth - birthMonth;
                var calcAge = 0;
                if (CalMonth < 0 || (CalMonth === 0 && currentDate < birthdate)) {
                    calYear--;
                }
                $('#txtdependantAge' + i).val(calYear + 1);
                //var ProspectAge = $('#txtAgeInfo').val();
                //$('#txtprospectAge').val(ProspectAge);
                //$('#ProspectDOB').val(dob);
            }
        }
    }
}


function CriticalIllnessGap(id) {
    
    f1(id);
    var CriticalIllnessRequirement = parseInt($("#hdntxtCriticalIllnessRequirement").val());
    if (isNaN(CriticalIllnessRequirement)) {
        CriticalIllnessRequirement = 0;
    }
    var CriticalIllnessAvailable = parseInt($("#hdntxtCriticalIllnessAvailable").val());
    if (isNaN(CriticalIllnessAvailable)) {
        CriticalIllnessAvailable = 0;
    }
    var CriticalIllnessGap = CriticalIllnessRequirement - CriticalIllnessAvailable;
    if (isNaN(CriticalIllnessGap)) {
        CriticalIllnessGap = 0;
    }
    $("#hdntxtCriticalIllnessGap").val(parseInt(CriticalIllnessGap));
    f2("txtCriticalIllnessGap")
    TotalRequirement();
    TotalAvailable();
    TotalGap();
    Policy41nc();
    Policy42nc();
}
function f1(id) 
{    
    var res = $('#' + id).val();
    if (res == "") {
        res = "0";
    }
    $('#' + id).val(parseInt(res.replace(/,/g, '')).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
    $('#hdn' + id).val(res.replace(/,/g, ''));
}
function f2(id) {
     
    var res = $('#hdn' + id).val();
    if (res != undefined) {
        $('#' + id).val(parseInt(res.replace(/,/g, '')).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ','));
    }
}
function HospitalGap(id) {
    f1(id);
    var HospitalRequirement = parseInt($("#hdntxtHospitalRequirement").val());
    if (isNaN(HospitalRequirement)) {
        HospitalRequirement = 0;
    }
    var HospitalAvailable = parseInt($("#hdntxtHospitalAvailable").val());
    if (isNaN(HospitalAvailable)) {
        HospitalAvailable = 0;
    }
    var HospitalGap = HospitalRequirement - HospitalAvailable;
    if (isNaN(HospitalGap)) {
        HospitalGap = 0;
    }
    $("#hdntxtHospitalGap").val(parseInt(HospitalGap));
    f2("txtHospitalGap")
    TotalRequirement();
    TotalAvailable();
    TotalGap();
}
function AdditionalGap(id) {
    
    f1(id);
    var AdditionalRequirement = parseInt($("#hdntxtAdditionalRequirement").val());
    if (isNaN(AdditionalRequirement)) {
        AdditionalRequirement = 0;
    }
    //var HospitalAvailable = parseInt($("#txtHospitalAvailable").val());
    //if (isNaN(HospitalAvailable)) {
    //    HospitalAvailable = 0;
    //}
    //var CriticalIllnessAvailable = parseInt($("#txtCriticalIllnessAvailable").val());
    //if (isNaN(CriticalIllnessAvailable)) {
    //    CriticalIllnessAvailable = 0;
    //}
    //var AdditionalAvailable = HospitalAvailable + CriticalIllnessAvailable;
    var AdditionalAvailable = parseInt($("#hdntxtAdditionalAvailable").val());
    if (isNaN(AdditionalAvailable)) {
        AdditionalAvailable = 0;
    }
    //$("#txtAdditionalAvailable").val(parseInt(AdditionalAvailable));

    var AdditionalGap = AdditionalRequirement - AdditionalAvailable;
    if (isNaN(AdditionalGap)) {
        AdditionalGap = 0;
    }
    $("#hdntxtAdditionalGap").val(parseInt(AdditionalGap));
    f2("txtAdditionalGap");
    TotalRequirement();
    TotalAvailable();
    TotalGap();
   
}
function Salary(id) {
    f1(id);
    var Salary = parseInt($("#hdntxtRetireSalary").val());
    if (isNaN(Salary)) {
        Salary = 0;
    }
    var MonthlyAllocation20 = 0.2 * Salary;
    $("#hdntxtMonthlyAllocation20").val(parseInt(MonthlyAllocation20).toFixed(0));
    f2("txtMonthlyAllocation20");

    var MonthlyAllocation3 = 0.03 * Salary;
    $("#hdntxtMonthlyAllocation3").val(parseInt(MonthlyAllocation3).toFixed(0));
    f2("txtMonthlyAllocation3");
}
function TotalRequirement() {
    
    var CriticalIllnessRequirement = parseInt($("#hdntxtCriticalIllnessRequirement").val());
    if (isNaN(CriticalIllnessRequirement)) {
        CriticalIllnessRequirement = 0;
    }
    var HospitalRequirement = parseInt($("#hdntxtHospitalRequirement").val());
    if (isNaN(HospitalRequirement)) {
        HospitalRequirement = 0;
    }
    var TotalRequirement = CriticalIllnessRequirement + HospitalRequirement;
    $("#hdntxtTotalRequirement").val(TotalRequirement);
    f2("txtTotalRequirement");
}
function TotalAvailable() {

    var CriticalIllnessAvailable = parseInt($("#hdntxtCriticalIllnessAvailable").val());
    if (isNaN(CriticalIllnessAvailable)) {
        CriticalIllnessAvailable = 0;
    }
    var HospitalAvailable = parseInt($("#hdntxtHospitalAvailable").val());
    if (isNaN(HospitalAvailable)) {
        HospitalAvailable = 0;
    }
    var TotalAvailable = CriticalIllnessAvailable + HospitalAvailable;
    $("#hdntxtTotalAvailable").val(TotalAvailable);
    f2("txtTotalAvailable");
}
function TotalGap() {

    var CriticalIllnessGap = parseInt($("#hdntxtCriticalIllnessGap").val());
    if (isNaN(CriticalIllnessGap)) {
        CriticalIllnessGap = 0;
    }
    var HospitalGap = parseInt($("#hdntxtHospitalGap").val());
    if (isNaN(HospitalGap)) {
        HospitalGap = 0;
    }
    var TotalGap = CriticalIllnessGap + HospitalGap;
    $("#hdntxtTotalGap").val(TotalGap);
    f2("txtTotalGap");
}
function compareAssest() {

    var assestTotal = parseInt($('#hdntxtCurrentTotal').val());
    if (isNaN(assestTotal)) {
        assestTotal = 0;
    }
    var liabilitiesTotal = parseInt($('#hdntxtLiabilityTotal1').val());
    if (isNaN(liabilitiesTotal)) {
        liabilitiesTotal = 0;
    }
    if (assestTotal > liabilitiesTotal) {

        $('#balance-stand').css({ 'transform': 'rotate(-15deg)', 'transition': 'transform 2s' });
        $('#balance-stand').removeClass('specialbalance specialafter');
        $('#balance-stand').addClass('specialbefore');


    }
    else if (parseInt(assestTotal, 10) < parseInt(liabilitiesTotal, 10)) {

        $('#balance-stand').css({ 'transform': 'rotate(15deg)', 'transition': 'transform 2s' });
        $('#balance-stand').removeClass('specialbalance specialbefore');
        $('#balance-stand').addClass('specialafter');
    }
    else {
        $('#balance-stand').css({ 'transform': 'rotate(0deg)', 'transition': 'transform 2s' });
        $('#balance-stand').removeClass('specialbefore specialafter');
        $('#balance-stand').addClass('specialbalance');
    }
}
function compareExpense() {

    var currentIncome = parseInt($('#hdntxtAnnualIncomeTotal').val());
    if (isNaN(currentIncome)) {
        currentIncome = 0;
    }
    var livingExpense = parseInt($('#hdntxtLivingAnnualExpensesTotal').val());
    if (isNaN(livingExpense)) {
        livingExpense = 0;
    }
    if (currentIncome > livingExpense) {
        $('.seesaw').css({ 'transform': 'rotate(-10deg)', 'transition': 'transform 2s' });
    }
    else if (currentIncome < livingExpense) {
        $('.seesaw').css({ 'transform': 'rotate(10deg)', 'transition': 'transform 2s' });
    }
    else {
        $('.seesaw').css({ 'transform': 'rotate(0deg)', 'transition': 'transform 2s' });
    }
}
function HealthCriticalRequiremenent(id) {
    f1(id);
    var CriticalRequiremenent = parseInt($("#hdntxtCriticalRequiremenent").val());
    if (isNaN(CriticalRequiremenent)) {
        CriticalRequiremenent = 0;
    }
    var CriticalFund = parseInt($("#hdntxtCriticalFund").val());
    if (isNaN(CriticalFund)) {
        CriticalFund = 0;
    }
    var CriticalGap = CriticalRequiremenent - CriticalFund;
    $("#hdntxtCriticalGap").val(CriticalGap.toFixed(0));
    f2("txtCriticalGap");
    HealthAmChart();

}

function HealthHospitalRequiremenent(id) {
    f1(id);
    var HospitalizationRequiremenent = parseInt($("#hdntxtHospitalizationRequiremenent").val());
    if (isNaN(HospitalizationRequiremenent)) {
        HospitalizationRequiremenent = 0;
    }
    var HospitalizationFund = parseInt($("#hdntxtHospitalizationFund").val());
    if (isNaN(HospitalizationFund)) {
        HospitalizationFund = 0;
    }
    var HospitalizationGap = HospitalizationRequiremenent - HospitalizationFund;
    $("#hdntxtHospitalizationGap").val(HospitalizationGap.toFixed(0));
    f2("txtHospitalizationGap");
    HealthAmChart();
}

function addexpReq(id) {
    f1(id);
    var AdditionalexpenseRequiremenent = parseInt($("#hdntxtAdditionalexpenseRequiremenent").val());
    if (isNaN(AdditionalexpenseRequiremenent)) {
        AdditionalexpenseRequiremenent = 0;
    }
    var AdditionalexpenseFund = parseInt($("#hdntxtAdditionalexpenseFund").val());
    if (isNaN(AdditionalexpenseFund)) {
        AdditionalexpenseFund = 0;
    }
    var AdditionalexpenseGap = AdditionalexpenseRequiremenent - AdditionalexpenseFund;
    $("#hdntxtAdditionalexpenseGap").val(AdditionalexpenseGap.toFixed(0));
    f2("txtAdditionalexpenseGap");
    HealthAmChart();
}
function ValidateIntrestProtect()
{
     
    var Inflationrate = $("#txtProIntrestRate").val();
    if (Inflationrate > 18 || Inflationrate < 4) {
        writeMultipleMessage("error", "Inflation rate ranges from 4 to 18", "txtProIntrestRate");
    }
    else {
        writeMultipleMessage("error", "", "txtProIntrestRate");
        EstMonthIncome();
    }
    
}
function DecrementIntrestPro()
{
    clearValidation();
    writeMultipleMessage("error", "", "txtProIntrestRate");

    var Interestnrate = parseInt($("#txtProIntrestRate").val());
    if (Interestnrate <= 4) {
        writeMultipleMessage("error", "Interest Rate minimum limit is 4", "txtProIntrestRate");
    }
    else {
        if (isNaN(Interestnrate)) {
            Interestnrate = 8;
        }
        Interestnrate -= 1;
        $("#txtProIntrestRate").val(Interestnrate);
        EstMonthIncome();
    }
}
function IncrementIntrestPro()
{
    clearValidation();
    writeMultipleMessage("error", "", "txtProIntrestRate");

    var Interestnrate = parseInt($("#txtProIntrestRate").val());
    if (Interestnrate >= 18) {
        writeMultipleMessage("error", "Interest Rate maximum limit is 18", "txtProIntrestRate");
    }
    else {


        if (isNaN(Interestnrate)) {
            Interestnrate = 8;
        }
        Interestnrate += 1;
        $("#txtProIntrestRate").val(Interestnrate);
        EstMonthIncome();
    }

}
function EstMonthIncomecomma(id) {
    f1(id);
    var MonthlyEarning = parseInt($("#hdntxtMonthlyEarning").val());
    if (isNaN(MonthlyEarning)) {
        MonthlyEarning = 0;
    }
    var YearsofEarning = parseInt($("#txtYearsofEarning").val());
    if (isNaN(YearsofEarning)) {
        YearsofEarning = 0;
    }
    var ProIntrestRate = $("#txtProIntrestRate").val();
    if (isNaN(ProIntrestRate)) {
        ProIntrestRate = 8;
    }
    EstimatedIncome = MonthlyEarning * YearsofEarning * 12;
    $("#hdntxtEstimatedIncome").val(EstimatedIncome.toFixed(0));
    f2("txtEstimatedIncome");

    var FutureFund = (EstimatedIncome * 100) / ProIntrestRate;
    $("#hdntxtFutureFund").val(FutureFund.toFixed(0));
    f2("txtFutureFund");
    var AvailableFund = parseInt($("#hdntxtAvailableFund").val());
    if (isNaN(AvailableFund)) {
        AvailableFund = 0;
    }
    var EmergencyFund = parseInt($("#hdntxtFutureFund").val()) - AvailableFund;
    $("#hdntxtEmergencyFund").val(EmergencyFund);
    f2("txtEmergencyFund");
    ProtectAmChart();
}
function EstMonthIncome() {
    var MonthlyEarning = parseInt($("#hdntxtMonthlyEarning").val());
    if (isNaN(MonthlyEarning)) {
        MonthlyEarning = 0;
    }
    var YearsofEarning = parseInt($("#txtYearsofEarning").val());
    if (isNaN(YearsofEarning)) {
        YearsofEarning = 0;
    }
    var ProIntrestRate = $("#txtProIntrestRate").val();
    if (isNaN(ProIntrestRate)) {
        ProIntrestRate = 8;
    }
    EstimatedIncome = MonthlyEarning * YearsofEarning * 12;
    $("#hdntxtEstimatedIncome").val(EstimatedIncome.toFixed(0));
    f2("txtEstimatedIncome");

    var FutureFund = (EstimatedIncome * 100) / ProIntrestRate;
    $("#hdntxtFutureFund").val(FutureFund.toFixed(0));
    f2("txtFutureFund");
    var AvailableFund = parseInt($("#hdntxtAvailableFund").val());
    if (isNaN(AvailableFund)) {
        AvailableFund = 0;
    }
    var EmergencyFund = parseInt($("#hdntxtFutureFund").val()) - AvailableFund;
    $("#hdntxtEmergencyFund").val(EmergencyFund);
    f2("txtEmergencyFund");
    ProtectAmChart();
}
function EduGap() {
    
    var EdugapTotal = 0;
    var index = $(".healthselectrow tr").length;

    for (var i = index; i >= 0; i--) {
        var edugap = parseInt($('#hdntxtedutable' + i).val());
        if (isNaN(edugap)) {
            edugap = 0;
        }
        EdugapTotal = EdugapTotal + edugap;
        $("#hdntxtEduGapTotal").val(EdugapTotal.toFixed(0));
        f2("txtEduGapTotal");
        $("#hdntxtEduMaturity").val(EdugapTotal.toFixed(0));
        f2("txtEduMaturity");

    }
}
function Eduamntcomma(id) {
    
    f1(id);
    var index = $(".healthselectrow tr").length;
    var rate = parseInt($("#txtEduInflationRate").val());
    if (isNaN(rate)) {
        rate = 8;
    }
    for (var i = index; i >= 0; i--) {
        var educurr = parseInt($('#hdntxteducurrent' + i).val());
        if (isNaN(educurr)) {
            educurr = 0;
        }
        var eduterm = parseInt($('#txteduterm' + i).val());
        if (isNaN(eduterm)) {
            eduterm = 0;
        }
        var estamnt = educurr * Math.pow((1 + (rate / 100)), eduterm);
        $('#hdntxteduamount' + i).val(estamnt.toFixed(0));
        f2("txteduamount" + i);
    }
    Edufund();
}
function Eduamnt() {
    
    var index = $(".healthselectrow tr").length;
    var rate = parseInt($("#txtEduInflationRate").val());
    if (isNaN(rate)) {
        rate = 8;
    }
    for (var i = index; i >= 0; i--) {
        var educurr = parseInt($('#hdntxteducurrent' + i).val());
        if (isNaN(educurr)) {
            educurr = 0;
            }
        var eduterm = parseInt($('#txteduterm' + i).val());
        if (isNaN(eduterm)) {
            eduterm = 0;
            }
        var estamnt = educurr * Math.pow((1 +(rate / 100)), eduterm);
        $('#hdntxteduamount' +i).val(estamnt.toFixed(0));
        f2("txteduamount" +i);
}
    Edufund();
    calcEduMaturityAge();
    }
function Edufundcomma(id) {
    
    f1(id);
    var index = $(".healthselectrow tr").length;
    for (var i = index; i >= 0; i--) {
        var edufundvalue = parseInt($('#hdntxtedufund' + i).val());
        if (isNaN(edufundvalue)) {
            edufundvalue = 0;
        }
        var eduamnt = parseInt($('#hdntxteduamount' + i).val());
        if (isNaN(eduamnt)) {
            eduamnt = 0;
        }
        var edugap = eduamnt - edufundvalue;
        $('#hdntxtedutable' + i).val(edugap.toFixed(0));
        f2("txtedutable" + i);
    }
    EduGap();
}
function Edufund() {
    
    var index = $(".healthselectrow tr").length;
    for (var i = index; i >= 0; i--) {
        var edufundvalue = parseInt($('#hdntxtedufund' + i).val());
        if (isNaN(edufundvalue)) {
            edufundvalue = 0;
        }
        var eduamnt = parseInt($('#hdntxteduamount' + i).val());
        if (isNaN(eduamnt)) {
            eduamnt = 0;
        }
        var edugap = eduamnt - edufundvalue;
        $('#hdntxtedutable' + i).val(edugap.toFixed(0));
        f2("hdntxtedutable" + i);
    }
    EduGap();
}
function EduLumpSum() {
   
    var Interestrate = parseInt($("#txtEduInterestRate").val());
    var AnnualEduExpense = parseInt($('#hdntxtAnnualEduExpense').val());
    if (isNaN(AnnualEduExpense)) {
        AnnualEduExpense = 0;
    }
    var EduLumpSumValue = (AnnualEduExpense * 100) / Interestrate;
    $("#hdntxtEduLumpSum").val(EduLumpSumValue.toFixed(0));
    f2("txtEduLumpSum");
}
function IncrementInflationrateEdu() {
    clearValidation();
    writeMultipleMessage("error", "", "txtEduInflationRate");
    var Inflationrate = parseInt($("#txtEduInflationRate").val());
    if (Inflationrate >= 15) {
        writeMultipleMessage("error", "Inflation Rate maximum limit is 15", "txtEduInflationRate");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate += 1;
        $("#txtEduInflationRate").val(Inflationrate);
        //RequirementTotal();

    }
    Eduamnt();
    Edufund();
}
function DecrementInflationrateEdu() {
    clearValidation();
    writeMultipleMessage("error", "", "txtEduInflationRate");

    var Inflationrate = parseInt($("#txtEduInflationRate").val());
    if (Inflationrate <= 5) {
        writeMultipleMessage("error", "Inflation Rate minimum limit is 5", "txtEduInflationRate");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate -= 1;
        $("#txtEduInflationRate").val(Inflationrate);
        //RequirementTotal();
    }
    Eduamnt();
    Edufund();
}
function ValidateInflationrateEdu() {

    clearValidation();
    var Inflationrate = $("#txtEduInflationRate").val();
    if (Inflationrate > 15 || Inflationrate < 5) {
        writeMultipleMessage("error", "Inflation rate ranges from 5 to 15", "txtEduInflationRate");
    }
    else {
        writeMultipleMessage("error", "", "txtEduInflationRate");
        // RequirementTotal();
    }
    Eduamnt();
    Edufund();
}
function IncrementInterestrateEdu() {
    clearValidation();
    writeMultipleMessage("error", "", "txtEduInterestRate");
    var Interestrate = parseInt($("#txtEduInterestRate").val());
    if (Interestrate >= 18) {
        writeMultipleMessage("error", "Interest Rate maximum limit is 18", "txtEduInterestRate");

    }
    else {
        if (isNaN(Interestrate)) {
            Inflationrate = 8;
        }
        Interestrate += 1;
        $("#txtEduInterestRate").val(Interestrate);
        EduLumpSum();

    }
    //Eduamnt();
    //Edufund();
}
function DecrementInterestrateEdu() {
    clearValidation();
    writeMultipleMessage("error", "", "txtEduInterestRate");

    var Interestrate = parseInt($("#txtEduInterestRate").val());
    if (Interestrate <= 4) {
        writeMultipleMessage("error", "Interest Rate minimum limit is 4", "txtEduInterestRate");

    }
    else {
        if (isNaN(Interestrate)) {
            Interestrate = 8;
        }
        Interestrate -= 1;
        $("#txtEduInterestRate").val(Interestrate);
        EduLumpSum();
    }
    //Eduamnt();
    //Edufund();
}
function ValidateInterestrateEdu() {

    clearValidation();
    var Inflationrate = $("#txtEduInterestRate").val();
    if (Inflationrate > 18 || Inflationrate < 4) {
        writeMultipleMessage("error", "Inflation rate ranges from 4 to 18", "txtEduInterestRate");
    }
    else {
        writeMultipleMessage("error", "", "txtEduInterestRate");
        EduLumpSum();
    }
    //Eduamnt();
    //Edufund();
}
function IncrementInflationrateSaving() {
    clearValidation();
    writeMultipleMessage("error", "", "txtSavInflationRate");
    var Inflationrate = parseInt($("#txtSavInflationRate").val());
    if (Inflationrate >= 15) {
        writeMultipleMessage("error", "Inflation Rate maximum limit is 15", "txtSavInflationRate");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate += 1;
        $("#txtSavInflationRate").val(Inflationrate);
        //RequirementTotal();

    }
    savingfund();
    savingestamnt();
}
function DecrementInflationrateSaving() {
    clearValidation();
    writeMultipleMessage("error", "", "txtSavInflationRate");

    var Inflationrate = parseInt($("#txtSavInflationRate").val());
    if (Inflationrate <= 5) {
        writeMultipleMessage("error", "Inflation Rate minimum limit is 5", "txtSavInflationRate");

    }
    else {
        if (isNaN(Inflationrate)) {
            Inflationrate = 8;
        }
        Inflationrate -= 1;
        $("#txtSavInflationRate").val(Inflationrate);
        //RequirementTotal();
    }
    savingfund();
    savingestamnt();
}
function ValidateInflationrateSaving() {
    clearValidation();
    var Inflationrate = $("#txtSavInflationRate").val();
    if (Inflationrate > 15 || Inflationrate < 5) {
        writeMultipleMessage("error", "Inflation rate ranges from 5 to 15", "txtSavInflationRate");
    }
    else {
        writeMultipleMessage("error", "", "txtSavInflationRate");
        // RequirementTotal();
    }
    savingfund();
    savingestamnt();
}

function ValidateName(id) {

    var Name = $("#" + id).val();
    var cArray = Name.split("");
    for (var i = 0; i < cArray.length; i++) {
        if (cArray[i] == " ") {
            flag = 0;
        }
        else {
            flag = 1;
            break;
        }
    }
    if (flag == 0) {
        writeMultipleMessage("error", "Enter Valid Name", id);
        return false;

    } else {
        writeMultipleMessage("error", "", id);
        return true;
    }
}
function calcMaturityAge() {
     
    var index = $(".savingselectrow tr").length;
    for (var i = index; i >= 0; i--) {
        var age = parseInt($("#txtsavingage" + i).val());
        if (isNaN(age)) {
            age = 0;
        }
        var term = parseInt($("#txtsavingterm" + i).val());
        if (isNaN(term)) {
            term = 0;
        }
        var Maturityage = age + term;
        $("#txtsavingmaturityage" + i).val(Maturityage)
    }
}
function calcEduMaturityAge() {
     
    var index = $(".healthselectrow tr").length;
    for (var i = index; i >= 0; i--) {
        var age = parseInt($("#txteduage" + i).val());
        if (isNaN(age)) {
            age = 0;
        }
        var term = parseInt($("#txteduterm" + i).val());
        if (isNaN(term)) {
            term = 0;
        }
        var Maturityage = age + term;
        $("#txtedumaturityage" + i).val(Maturityage)
    }
}

function savingestamnt() {

    var index = $(".savingselectrow tr").length;
    var rate = parseInt($('#txtSavInflationRate').val());
    if (isNaN(rate)) {
        rate = 8;
    }
    for (var i = index; i >= 0; i--) {
        var savingcurrent = parseInt($('#hdntxtsavingtable' + i).val());
        if (isNaN(savingcurrent)) {
            savingcurrent = 0;
        }
        var savingterm = parseInt($('#txtsavingterm' + i).val());
        if (isNaN(savingterm)) {
            savingterm = 0;
        }
        var saveestamnt = savingcurrent * Math.pow((1 + (rate / 100)), savingterm);
        $('#hdntxtsavingest' + i).val(saveestamnt.toFixed(0));
        f2("txtsavingest" + i)
    }
    SavingRequirementTotal();
    calcMaturityAge();
    savingfund();
}
function savingestamntcomma(id) {
    f1(id);
    var index = $(".savingselectrow tr").length;
    var rate = parseInt($('#txtSavInflationRate').val());
    if (isNaN(rate)) {
        rate = 8;
    }
    for (var i = index; i >= 0; i--) {
        var savingcurrent = parseInt($('#hdntxtsavingtable' + i).val());
        if (isNaN(savingcurrent)) {
            savingcurrent = 0;
        }
        var savingterm = parseInt($('#txtsavingterm' + i).val());
        if (isNaN(savingterm)) {
            savingterm = 0;
        }
        var saveestamnt = savingcurrent * Math.pow((1 + (rate / 100)), savingterm);
        $('#hdntxtsavingest' + i).val(saveestamnt.toFixed(0));
        f2("txtsavingest" + i)
    }
    SavingRequirementTotal();
    calcMaturityAge();
    savingfund();
}
function savingfund() {

    var index = $(".savingselectrow tr").length;
    for (var i = index; i >= 0; i--) {
        var savingfund = parseInt($('#hdntxtsavingfund' + i).val());
        if (isNaN(savingfund)) {
            savingfund = 0;
        }
        var savingest = parseInt($('#hdntxtsavingest' + i).val());
        if (isNaN(savingest)) {
            savingest = 0;
        }
        var savinggap = savingest - savingfund;
        $('#hdntxtsavinggap' + i).val(savinggap.toFixed(0))
        f2("txtsavinggap" + i)
    }
    SavingRequirementTotal();
}
function savingfundcomma(id) {
    f1(id);
    var index = $(".savingselectrow tr").length;
    for (var i = index; i >= 0; i--) {
        var savingfund = parseInt($('#hdntxtsavingfund' + i).val());
        if (isNaN(savingfund)) {
            savingfund = 0;
        }
        var savingest = parseInt($('#hdntxtsavingest' + i).val());
        if (isNaN(savingest)) {
            savingest = 0;
        }
        var savinggap = savingest - savingfund;
        $('#hdntxtsavinggap' + i).val(savinggap.toFixed(0))
        f2("txtsavinggap" + i)
    }
    SavingRequirementTotal();
}
function SavingRequirementTotal() {
    
    var totalsavings = 0;
    var totalest = 0;
    var totalfund = 0;
    var totalgap = 0;

    var index = $(".savingselectrow tr").length;

    for (var i = index; i >= 0; i--) {
        var saving = parseInt($('#hdntxtsavingtable' + i).val());
        if (isNaN(saving)) {
            saving = 0;
        }
        totalsavings = totalsavings + saving;
        $("#hdntxtSavingReqTotal").val(totalsavings.toFixed(0));
        f2("txtSavingReqTotal");

        var savingest = parseInt($('#hdntxtsavingest' + i).val());
        if (isNaN(savingest)) {
            savingest = 0;
        }
        totalest = totalest + savingest;
        $("#hdntxtSavingEstTotal").val(totalest.toFixed(0));
        f2("txtSavingEstTotal");

        var savingfund = parseInt($('#hdntxtsavingfund' + i).val());
        if (isNaN(savingfund)) {
            savingfund = 0;
        }
        totalfund = totalfund + savingfund;
        $("#hdntxtSavingCurrentTotal").val(totalfund.toFixed(0));
        f2("txtSavingCurrentTotal");

        var savinggap = parseInt($('#hdntxtsavinggap' + i).val());
        if (isNaN(savinggap)) {
            savinggap = 0;
        }
        totalgap = totalgap + savinggap;
        $("#hdntxtSavingGapTotal").val(totalgap.toFixed(0));
        f2("txtSavingGapTotal");

    }
    $("#hdntxtSavingTarget").val(totalgap.toFixed(0));
    f2("txtSavingTarget");
}
function changeImage1() {
    var image = document.getElementById('plusimg1');
    if (image.src.match("minus")) {
        image.src = '../../Content/Images/plus.png';
    } else {
        image.src = '../../Content/Images/minus.png';
    }
}
function changeImage2() {
    var image = document.getElementById('plusimg2');
    if (image.src.match("minus")) {
        image.src = '../../Content/Images/plus.png';
    } else {
        image.src = '../../Content/Images/minus.png';
    }
}
function changeImage3() {
    var image = document.getElementById('plusimg3');
    if (image.src.match("minus")) {
        image.src = '../../Content/Images/plus.png';
    } else {
        image.src = '../../Content/Images/minus.png';
    }
}
function changeImage4() {
    var image = document.getElementById('plusimg4');
    if (image.src.match("minus")) {
        image.src = '../../Content/Images/plus.png';
    } else {
        image.src = '../../Content/Images/minus.png';
    }
}
function changeImage5() {
    var image = document.getElementById('plusimg5');
    if (image.src.match("minus")) {
        image.src = '../../Content/Images/plus.png';
    } else {
        image.src = '../../Content/Images/minus.png';
    }
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

function RemoveDependent(DivID, index) {

    $('#txtIsDeleted' + index).val(true);

    writeMultipleMessage("error", "", "ddRelationship" + index + "");
    writeMultipleMessage("error", "", "txtDepName" + index + "");
    writeMultipleMessage("error", "", "DpDependents" + index + "");
    $('#' + DivID).hide();
};

function AssignpersonallifeSI() {
    var l1 = getValue(parseInt($("#txtTotalDeathBenifit").val()));
    var l2 = getValue(parseInt($("#txtTotalAccidental").val()));
    var l3 = getValue(parseInt($("#txtTotalCritical").val()));
    var l4 = getValue(parseInt($("#txtTotalHospitalization").val()));
    var l5 = getValue(parseInt($('#txtinvestmentsforUncertainEvents').val()));

    $('#txtTotalProtectionAvailable').val(l1 + l2 + l3 + l4 + l5);

    var s1 = getValue(parseInt($("#txtCapitalReq").val()));
    var s2 = getValue(parseInt($("#txtTotalProtectionAvailable").val()));

    $("#txtgapidentified").val(s1 - s2);
}

function getValue(val) {
    if (isNaN(val) == true) {
        return 0;
    }
    else {
        return val;
    }
}

function ageNeedProspectCount() {
    var dob = $('#ProspectDOB').val();
    if (dob != '') {
        var d = new Date();
        var currentYear = d.getFullYear();
        var arr = dob.split('/');
        var birthYear = arr[2];
        var calcAge = currentYear - birthYear;
        $('#txtprospectAge').val(calcAge);
    }
}

function ageCountSpouse() {
    var dob = $('#dtSpouseDOB').val();
    if (dob != '') {
        var d = new Date();
        var currentYear = d.getFullYear();
        var arr = dob.split('/');
        var birthYear = arr[2];
        var calcAge = currentYear - birthYear;
        $('#txtSpouseAge').val(calcAge);
    }
}

function ageDependentCount(count) {
    var dob = $('#DpDependents' + count).val();
    var controlid = 'DpDependents' + count;
    if (dob == "") {
        $('#txtDependentAge' + count).val('');
        return;
    }
    if (!ValidateDOB(controlid)) {
        writeMultipleMessage("error", "Please enter valid Date Of Birth. It cannot be future date.", controlid);
        return false;
    }
    else {
        writeMultipleMessage("error", "", controlid);

    }
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
        $('#txtDependentAge' + count).val(calYear);
    }


}

function ValidateDOB(controlid) {

    var dob = $('#' + controlid).val();
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
function ValidateDependantDOB(controlid) {

    var dob = $('#' + controlid).val();
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var currentDate = (day < 10 ? '0' : '') + day + '-' + (month < 10 ? '0' : '') + month + '-' + d.getFullYear();
    var arrDOB = dob.split('/');
    var birthDay = arrDOB[0];
    var birthMonth = arrDOB[1];
    var birthYear = arrDOB[2];
    var arrCurrentDate = currentDate.split('-');
    var currentDay = arrCurrentDate[0];
    var currentMonth = arrCurrentDate[1];
    var currentYear = arrCurrentDate[2];
    if (birthYear > currentYear) {
        return false;
    }
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
function FetchUploadSignPath() {

    var SignPath = $("#UploadSignature").val();
    $("#HdnUploadSignPath").val(SignPath);
}
function showVal(_i, value) {

    var val = parseInt($('#' + _i).val())
    $('#' + value).val(val);

    var a, b, c, d, e, f, g, h, i, m = 0;

    a = getValue(parseInt($("#txtFoodAmount").val()));
    if (a == "" || a == 0) {
        $("#Outputfood_S").val('0');
    }
    else if (a > 1000000) {
        writeMultipleMessage("error", "Please enter Food amount should be below 10,00,000", "txtFoodAmount");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtFoodAmount");
    }
    b = getValue(parseInt($("#txtHouseElectricityWaterRent").val()));
    if (b == "" || b == 0) {
        $("#OutputHouseElectricityWaterRent_S").val('0');
    }
    else if (b > 1000000) {
        writeMultipleMessage("error", "Please enter HouseRent/Electricity/Water/Telephone should be below 10,00,000", "txtHouseElectricityWaterRent");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtHouseElectricityWaterRent");
    }
    c = getValue(parseInt($("#txtClothes").val()));
    if (c == "" || c == 0) {
        $("#OutputClothes_S").val('0');
    }
    else if (c > 1000000) {
        writeMultipleMessage("error", "Please enter Clothes amount should be below 10,00,000", "txtClothes");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtClothes");
    }
    d = getValue(parseInt($("#txtTransport").val()));
    if (d == "" || d == 0) {
        $("#OutputTransport_S").val('0');
    }
    else if (d > 1000000) {
        writeMultipleMessage("error", "Please enter Transport amount should be below 10,00,000", "txtTransport");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtTransport");
    }
    e = getValue(parseInt($("#txtHealthCare").val()));
    if (e == "" || e == 0) {
        $("#OutputHealthCare_S").val('0');
    }
    else if (e > 1000000) {
        writeMultipleMessage("error", "Please enter Family Health Care amount should be below 10,00,000", "txtHealthCare");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtHealthCare");
    }
    f = getValue(parseInt($("#txtFamilyEducation").val()));
    if (f == "" || f == 0) {
        $("#OutputFamilyEducation_S").val('0');
    }
    else if (f > 1000000) {
        writeMultipleMessage("error", "Please enter Education Of Children amount should be below 10,00,000", "txtFamilyEducation");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtFamilyEducation");
    }
    g = getValue(parseInt($("#txtSpecialEvents").val()));
    if (g == "" || g == 0) {
        $("#OutputSpecialEvents_S").val('0');
    }
    else if (g > 1000000) {
        writeMultipleMessage("error", "Please enter Special Events and Entertainments should be below 10,00,000", "txtSpecialEvents");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtSpecialEvents");
    }
    h = getValue(parseInt($("#txtMaidAndOtherHelpers").val()));
    if (h == "" || h == 0) {
        $("#OutputMaidAndOtherHelpers_S").val('0');
    }
    else if (h > 1000000) {
        writeMultipleMessage("error", "Please enter Maid's and other Helpers Salary should be below 10,00,000", "txtMaidAndOtherHelpers");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtMaidAndOtherHelpers");
    }
    i = getValue(parseInt($("#txtOtherMontly").val()));
    if (i == "" || i == 0) {
        $("#OutputOtherMontly_S").val('0');
    }
    else if (i > 1000000) {
        writeMultipleMessage("error", "Please enter Other Monthly Expenditures amount should be below 10,00,000", "txtOtherMontly");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtOtherMontly");
    }
    var total = a + b + c + d + e + f + g + h + i;
    total = getValue(total);
    $("#txtprospectTotalMonthlyExp").val(total);
    $('#txtTotalMonthlyExp').val(total);

    m = getValue(parseInt($("#txtMonthlyInstallments").val()));
    if (m == "" || m == 0) {
        $("#OutputMonthlyInstallments_S").val('0');
    }
    else if (m > 1000000) {
        writeMultipleMessage("error", "Please enter Monthly Installments should be below 10,00,000", "txtMonthlyInstallments");
        return false;
    }
    else {
        writeMultipleMessage("error", "", "txtMonthlyInstallments");
    }
    total = total + m;
    $('#txtMonthlyExpenditure').val(total);

    var s1 = getValue(parseInt($('#txtMonthlyExpenditure').val()));
    var r1 = getValue(parseInt($('#txtRateofInterest').val()));
    var TotalSI = ((s1 * 12 * 100) / r1);
    $("#txtCapitalReq").val(TotalSI);
}

function getDeathValue(i, value) {

    //var amount=0;
    //var noOfRows = $("#FamilyIncomePreviousPolicy > tbody > tr").length;
    //for (var i = 0; i < noOfRows; i++) {
    //    var a = getValue(parseInt($("#txtTotalDeathBenifit").val()));
    //    amount =+ $("#txtDeathAmount"+i).val();
    //    $("#txtTotalDeathBenifit").val(a+amount);
    //}
    var total = 0;
    var a = getValue(parseInt($("#txtTotalDeathBenifit").val()));
    var b = getValue(value);
    var total = getValue(parseInt(a) + parseInt(b));
    $("#txtTotalDeathBenifit").val(total);
}

function getAccidentalValue(i, value) {
    var a = getValue(parseInt($("#txtTotalAccidental").val()));
    var b = getValue(value);
    var total = getValue(parseInt(a) + parseInt(b));
    $("#txtTotalAccidental").val(total);
}

function getCriticalValue(i, value) {
    var a = getValue(parseInt($("#txtTotalCritical").val()));
    var b = getValue(value);
    var total = getValue(parseInt(a) + parseInt(b));
    $("#txtTotalCritical").val(total);
}

function getHospitalizationValue(i, value) {
    var a = getValue(parseInt($("#txtTotalHospitalization").val()));
    var b = getValue(value);
    var total = getValue(parseInt(a) + parseInt(b));
    $("#txtTotalHospitalization").val(total);
}

var TotalSI = parseInt(0);
function AssignSItoTotal(e) {
    var s1 = getValue(parseInt($("#txtEstimationFood").val()));
    var s2 = getValue(parseInt($("#txtEstimationHouseRent").val()));
    var s3 = getValue(parseInt($("#txtEstimationClothes").val()));
    var s4 = getValue(parseInt($("#txtEstimationTransport").val()));
    var s5 = getValue(parseInt($("#txtEstimationFamilyHealthCare").val()));
    var s6 = getValue(parseInt($("#txtEstimationEducationOfChildren").val()));
    var s7 = getValue(parseInt($("#txtEstimationEventsandEntertainments").val()));
    var s8 = getValue(parseInt($("#txtEstimationOtherHelpersSalary").val()));
    var s9 = getValue(parseInt($("#txtEstimationMonthlyExpenditure").val()));
    var s10 = getValue(parseInt($("#txtEstimationLeases").val()));
    $("#txtprospectTotalMonthlyExp").val(s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9 + s10);
    var SI = s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9 + s10;
}

function AssignTotalFamilyIncome() {
    var s1 = getValue(parseInt($("#txtFamilyIncomeProspect").val()));
    var s2 = getValue(parseInt($("#txtFamilyIncomeSpouse").val()));
    $("#txtFamilyIncomeHouseTotal").val(s1 + s2);
}

function AddAssetsToTotal(e) {
    var a1 = getValue(parseInt($("#txtAssetsLandOrHouses").val()));
    var a2 = getValue(parseInt($("#txtAssetsMotorVehicles").val()));
    var a3 = getValue(parseInt($("#txtAssetsBankDeposits").val()));
    var a4 = getValue(parseInt($("#txtAssetsInvestments").val()));
    $("#txtAssetsTotal").val(a1 + a2 + a3 + a4);
}

function AddLiabalityToTotal(e) {
    var l1 = getValue(parseInt($("#txtLiabilitiesLoans").val()));
    var l2 = getValue(parseInt($("#txtLiabilitiesMortgages").val()));
    var l3 = getValue(parseInt($("#txtLiabilitiesLeases").val()));
    var l4 = getValue(parseInt($("#txtLiabilitiesOthers").val()));
    $("#txtLiabilitiesTotal").val(l1 + l2 + l3 + l4);
}
function NeedAnalysisAmChart()
{
    var EduCount = 0;
    for (var i = 1; i <= 5; i++) {
        if ($("#chkEducation" + i).is(':checked')) {
            EduCount++;
        }
    }
    var RtmCount = 0;
    for (var i = 1; i <= 5; i++) {
        if ($("#chkRetirement" + i).is(':checked')) {
            RtmCount++;
        }
    }
    var ProCount = 0;
    for (var i = 1; i <= 5; i++) {
        if ($("#chkProtection" + i).is(':checked')) {
            ProCount++;
        }
    }
    var HltCount = 0;
    for (var i = 1; i <= 5; i++) {
        if ($("#chkHealth" + i).is(':checked')) {
            HltCount++;
        }
    }
    var SavCount = 0;
    for (var i = 1; i <= 5; i++) {
        if ($("#chkSaving" + i).is(':checked')) {
            SavCount++;
        }
    }

    var chart = AmCharts.makeChart("chartdiv", {
        "type": "serial",
        "hidecredits": true,
        "theme": "light",
        "marginRight": 70,
        "dataProvider": [{
            "country": "Children’s Education",
            "visits": EduCount,
            "color": "#FF0F00"
        }, {
            "country": "Health",
            "visits": HltCount,
            "color": "#FF6600"
        }, {
            "country": "Saving for future",
            "visits": SavCount,
            "color": "#FF9E01"
        }, {
            "country": "Retirement planning",
            "visits": RtmCount,
            "color": "#FF9E01"
        }, {
            "country": "Protection",
            "visits": ProCount,
            "color": "#FF9E01"
        }],
        "valueAxes": [{
            "axisAlpha": 0,
            "position": "left",
            "minimum": 0,
            "maximum": 5,
            "integersOnly": true,
            "title": ""
        }],
        "startDuration": 1,
        "graphs": [{
            "balloonText": "<b>[[category]]: [[value]]</b>",
            "fillColorsField": "color",
            "fillAlphas": 0.9,
            "lineAlpha": 0.2,
            "type": "column",
            "valueField": "visits",
            "showBalloon": false
        }],
        "chartCursor": {
            "categoryBalloonEnabled": false,
            "cursorAlpha": 0,
            "zoomable": false
        },
        "categoryField": "country",
        "categoryAxis": {
            "gridPosition": "start",
            "labelRotation": 45,
            "fontSize":9,
        },
        "export": {
            "enabled": true,

        }

    });


}
var ProtectionChart;
var HealthChart;
function HealthAmChart() {
    
    var CriticalGap = parseInt($("#hdntxtCriticalGap").val());
    if (isNaN(CriticalGap)) {
        CriticalGap = 0;
    }
    var HospitalizationGap = parseInt($("#hdntxtHospitalizationGap").val())
    if (isNaN(HospitalizationGap)) {
        HospitalizationGap = 0;
    }
    var AdditionalexpenseGap = parseInt($("#hdntxtAdditionalexpenseGap").val());
    if (isNaN(AdditionalexpenseGap)) {
        AdditionalexpenseGap = 0;
    }
    //"listeners": [{
    //    "event": "rendered",
    //    "method": RenderHealthChart
    //}],

    HealthChart = AmCharts.makeChart("healthchartdiv", {
        "type": "serial",
        "theme": "light",
        "hidecredits": true,
        "marginRight": 70,
        "listeners": [{
            "event": "rendered",
            "method": RenderHealthChart
        }],
        
        "dataProvider": [{
            "country": "Reserve for Critical Illness",
            "visits": CriticalGap,
            "color": "#FF0F00"
        }, {
            "country": "Reserve for Hospitalization",
            "visits": HospitalizationGap,
            "color": "#FF6600"
        }, {
            "country": "Cash for additional expense/ Loss of income",
            "visits": AdditionalexpenseGap,
            "color": "#FF9E01"
        }],
        "valueAxes": [{
            "axisAlpha": 0,
            "position": "left",
            "minimum": 0,
            "integersOnly": true,
            "title": ""
        }],
        "startDuration": 1,
        "graphs": [{
            "balloonText": "<b>[[category]]: [[value]]</b>",
            "fillColorsField": "color",
            "fillAlphas": 0.9,
            "lineAlpha": 0.2,
            "type": "column",
            "valueField": "visits",
            "showBalloon": false
        }],
        "chartCursor": {
            "categoryBalloonEnabled": false,
            "cursorAlpha": 0,
            "zoomable": false
        },
        "categoryField": "country",
        "categoryAxis": {
            "gridPosition": "start",
            "labelRotation": 0,
            "fontSize":9,
            "autoWrap":true
        },
        "export": {
            "delay": 1,
            "enabled": true,
            "onReady": function (type, timedout) {
                if (!timedout) {
                    RenderHealthChart();
                }
                // if (!timedout) {
                //    this.capture({}, function () {
                //        this.toPNG({
                //            multiplier: 2,
                //            "fit": [223.28, 229.89]}, function (data) {
                //            $("#HdnHealthGraphByteArray").val(data);
                //        });
                //    });
                //}
            }
        },
        "allLabels": [{
            "text": "Fund Requirement",
            "x": "60%",
            "y": "1%",
            "bold": true,
            "align": "right"
        }, {
            "text": "Amount",
            "rotation": 270,
            "x": "4",
            "y": "200",
            "bold": true,
            "align": "right"
        }]

    });
}
function ProtectAmChart() {
   
    var EmergencyFund = $("#hdntxtEmergencyFund").val();
    var FutureFund = $("#hdntxtFutureFund").val();
    //"listeners": [{
    //    "event": "rendered",
    //    "method": RenderProtectionChart
    //}],
    ProtectionChart = AmCharts.makeChart("Protectionchartdiv", {
        "type": "serial",
        "theme": "light",
        "hidecredits": true,
        "marginRight": 20,
        "fontSize": 10,
       
        "listeners": [{
            "event": "rendered",
            "method": RenderProtectionChart
        }],
        "dataProvider": [{
            "country": "Emergency<br>Fund Requirement",
            "visits": EmergencyFund,
            "color": "#FF0F00",
           
           
        }, {
            "country": "Fund Value<br> for Future Income",
            "visits": FutureFund,
            "color": "#FF6600"
        }],
        "valueAxes": [{
            "axisAlpha": 0,
            "position": "left",
            "minimum": 0,
            "integersOnly": true,
            "title": ""
        }],
       
        "startDuration": 1,
        "graphs": [{
            "balloonText": "<b>[[category]]: [[value]]</b>",
            "fillColorsField": "color",
            "fillAlphas": 0.9,
            "lineAlpha": 0,
            "type": "column",
            "valueField": "visits",
            "showBalloon": false
        }],
        "chartCursor": {
            "categoryBalloonEnabled": false,
            "cursorAlpha": 0,
            "zoomable": false
        },
        "categoryField": "country",
        "categoryAxis": {
            "gridPosition": "start",
            "labelRotation": 0,
            "autoWrap": true,
            "maxWidth": 10,
            "fontSize": 9,
            
        },
        "export": {
            "delay": 1,
            "enabled": true,
            "onReady": function (type, timedout) {
                if (!timedout) {
                    RenderProtectionChart();
                }
                //if (!timedout) {
                //    this.capture({}, function () {
                //        this.toPNG({
                //            multiplier: 2,
                //            "fit": [223.28, 229.89]

                //        }, function (data) {
                //            debugger;
                //            $("#HdnProtectionGraphByteArray").val(data);
                //        });
                //    });
                //}
            }
        },
        "allLabels": [{
            "text": "Fund Requirement",
            "x": "60%",
            "horizontalCenter": "middle",
           "y":"1%",
            "bold": true,
            "align": "right"
        }, {
            "text": "Amount",
            "rotation": 270,
            "x": "5",
            "y": "204",
            "bold": true,
            "align": "right"
            }]
        

    });
}
function RenderProtectionChart(e) {
    debugger;
    $('#divBusyIndicator').show();
    if (ProtectionChart==null)
        return;
    ProtectionChart.dataChanged = true;
    var ProtectionTimer = setInterval(function () {
       
        ProtectionChart["export"].capture({delay:3}, function () {
        this.toPNG({
                multiplier: 2,
                "fit": [523.28, 769.89]
            }, function (data) {
               
                if (data.length > 10) {
                  
                $("#HdnProtectionGraphByteArray").val(data);

                }
                 //  this.download(data, "image/jpg", "Protectionharts.jpg");
                //HumanValueReport(1);
                // alert(data.length);
            });
        });
        clearInterval(ProtectionTimer);
    }, 3000);
    $('#divBusyIndicator').hide();
}
function RenderHealthChart(e) {
    debugger;
    $('#divBusyIndicator').show();
    if (HealthChart==null)
        return;
    HealthChart.dataChanged = true;
    var HealthTimer = setInterval(function () {
       
        HealthChart["export"].capture({delay:3}, function () {
            this.toPNG({
                multiplier: 2,
                "fit": [523.28, 769.89],
            }, function (data) {
               
                if (data.length > 10) {
                    $("#HdnHealthGraphByteArray").val(data);
                }
                // this.download(data, "image/png", "Healthharts.png");
                //HealthCalcPDF(1);
                // alert(data.length);
            });
        }); clearInterval(HealthTimer);
    }, 3000);
    $('#divBusyIndicator').hide();
}

$(document).ajaxComplete(function () {
    RenderProtectionChart();
    RenderHealthChart();
});
function Handlechange(id) {
    
    //var fileinput = document.getElementById("browse");
    //var textinput = document.getElementById("filename");
    //textinput.value = fileinput.value;
    UploadFileToServer(id);

}
function HandleBrowseClick() {
    var fileinput = document.getElementById("browse");
    fileinput.click();
}
function ageProspectCount() {
     
    var dob = $('#dtProspectDOB').val();
    var controlid = 'dtProspectDOB';
    writeMultipleMessage("error", "", controlid);
    if (dob == "") {
        $('#txtAge').val('');
        writeMultipleMessage("error", "Please enter valid Date Of Birth.", controlid);
        return false;
    }
    if (!ValidateDOB(controlid)) {
        $('#txtAge').val('');
        writeMultipleMessage("error", "Please enter valid Date Of Birth. It cannot be future date.", controlid);
        return false;
    }
    else {
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
            $('#txtAge').val(calYear);
            var ProspectAge = $('#txtAge').val();
            $('#txtprospectAge').val(ProspectAge);
            $('#ProspectDOB').val(dob);
            $('#txtAgeInfo').val(ProspectAge);
            $('#dtProspectDOBInfo').val(dob);

        }
    }
}
function UploadFileToServer(id) {

    var formdata = new FormData(); //FormData object
    var fileInput = document.getElementById(id);
    var iSize = fileInput.files[0].size;

    if (iSize / 1024 > 1)
     {
        if (((iSize / 1024) / 1024) > 1)
        {
            iSize = (Math.round(((iSize / 1024) / 1024) * 100) / 100)//MB
            if (iSize > 3) {
                writeMultipleMessage("error", "File Size should be less than 3 MB", "txtInflationrateFNA");
                return false;
            }
        
        }
     }
       
    //Iterating through each files selected in fileInput
    for (i = 0; i < fileInput.files.length; i++) {
        //Appending each file to FormData object
        formdata.append(fileInput.files[i].name, fileInput.files[i]);

    }
    var uploadFilepath = $("#filename").val();
    var hiddenFilepath = $("#filename").val();

    var errors = "";
    var errorflag1 = true;
    if (uploadFilepath == "" && hiddenFilepath == "") {
        errors = errors + "Please choose a file for upload.";
        errorflag1 = false;
    }
    else {
        $.ajax({
            type: "POST",
            async: false,
            url: "../../Prospect/UploadFilePath",
            contentType: false,
            processData: false,
            data: formdata,
            success: function (data) {
                if (data == "error") {
                    writeMultipleMessage("error", "Invalid File Type", "txtInflationrateFNA");
                }
                else {
                    writeMultipleMessage("error", "", "txtInflationrateFNA");
                    //$("#" + fileVariable).val(data);
                }
               
            },
            fail: function () {
                ErrorSuccessAlert("OOPS!! something went wrong", "danger");
            }
        });
        event.preventDefault();
    }
    if (errorflag1) {
        return true;
    }
    else {
        ErrorSuccessAlert(errors, "danger");
        return false;
    }

}

//function UploadFileToServer(id) {
//    var fileUpload = $("#"+id).get(0);
//    var files = fileUpload.files;
//    var formdata = new FormData(); //FormData object
//    //var fileInput = document.getElementById(id);

//    //Iterating through each files selected in fileInput
//    for (i = 0; i < files.length; i++) {
//        //Appending each file to FormData object
//        formdata.append(files[i].name, files[i]);

//    }
//    var uploadFilepath = $("#filename").val();
//    var hiddenFilepath = $("#filename").val();

//    var errors = "";
//    var errorflag1 = true;
//    if (uploadFilepath == "" && hiddenFilepath == "") {
//        errors = errors + "Please choose a file for upload.";
//        errorflag1 = false;
//    }
//    else {
//        $.ajax({
//            type: "POST",
//            async: false,
//            url: "../../Prospect/UploadFilePath",
//            contentType: false,
//            processData: false,
//            data: formdata,
//            success: function (data) {
//                if (data == "error") {
//                    writeMultipleMessage("error", "Invalid File Type", "txtInflationrateFNA");
//                }
//                else {
//                    writeMultipleMessage("error", "", "txtInflationrateFNA");
//                    //$("#" + fileVariable).val(data);
//                }

//            },
//            fail: function () {
//                ErrorSuccessAlert("OOPS!! something went wrong", "danger");
//            }
//        });
//    }
//    if (errorflag1) {
//        return true;
//    }
//    else {
//        ErrorSuccessAlert(errors, "danger");
//        return false;
//    }

//}
function SavingAmount(id)
{
    f1(id);
    var MonthlySavingAmount = parseInt($('#hdntxtMonthlySaveExpense').val());
    if (isNaN(MonthlySavingAmount)) {
        MonthlySavingAmount = 0;
    }
    var AnnualSavingAmount = MonthlySavingAmount * 12;
    $('#hdntxtAnnualSaveExpense').val(AnnualSavingAmount);
    f2("txtAnnualSaveExpense");

}
function EduAnnualExp(id)
{
    f1(id);
    var MonthlyEduExpense = parseInt($('#hdntxtMonthlyEduExpense').val());
    if (isNaN(MonthlyEduExpense)) {
        MonthlyEduExpense = 0;
    }
    var AnnualEduExpense = MonthlyEduExpense * 12;
    $('#hdntxtAnnualEduExpense').val(AnnualEduExpense);
    f2("txtAnnualEduExpense");
    EduLumpSum();
}
function EduRelation(id) {
    debugger;
    var maxlimit = 0;
    var val = 0;
    if (id.length == 16) {
        val = parseInt(id.substring(14, 16));
    }
    else {
        val = parseInt(id.substring(14, 15));
    }
    var num = parseInt($('#objNeedAnalysis_DependantCount').val());
    if (isNaN(num)) {
        num = 0;
    }
    if ($('#ddlMaritalStatus').val() == "M") {
        maxlimit = num + 2;
    }
    else {
        maxlimit = num + 1;
    }
    for (var i = 0; i <= maxlimit; i++) {
        var name = $('#txtedurelation' + val).val();
        if (name == $('#txtNameInfo').val()) {
            var age = $('#txtAgeInfo').val();
            $('#txteduage' + val).val(age);
        }
        if (name == $('#txtSpouseNameInfo').val()) {
            var age = $('#txtSpouseAgeInfo').val();
            $('#txteduage' + val).val(age);
        }
        var dependname = $('#txtedurelation' + val).val();
        if (dependname == $('#dtdependantName' + i).val()) {
            var age = $('#txtdependantAge' + i).val();
            $('#txteduage' + val).val(age);
        }
    }
    //for (var i = 0; i <= num +val; i++) {
    //    var name = $('#txtedurelation' + i).val();
    //    if (name == $('#txtNameInfo').val()) {
    //        var age = $('#txtAgeInfo').val();
    //        $('#txteduage' + i).val(age);
    //    }
    //    if (name == $('#txtSpouseNameInfo').val()) {
    //        var age = $('#txtSpouseAgeInfo').val();
    //        $('#txteduage' + i).val(age);
    //    }
    //}
    //for (var i = 0; i <= num+val; i++) {
    //    j =  val-i;
    //    var name = $('#txtedurelation' + i).val();
    //    if (name == $('#dtdependantName' + j).val()) {
    //        var age = $('#txtdependantAge'+j).val();
    //        $('#txteduage' + i).val(age);
    //    }
    //}
    calcEduMaturityAge();
}
function SaveRelationship(id)
{
    debugger;
    var maxlimit = 0;
    var val = 0;
    if (id.length == 19) {
        val = parseInt(id.substring(17, 19));
    }
    else
    {
        val = parseInt(id.substring(17, 18));
    }
    var num = parseInt($('#objNeedAnalysis_DependantCount').val());
    if (isNaN(num)) {
        num = 0;
    }
    if ($('#ddlMaritalStatus').val() == "M") {
        maxlimit = num + 2;
    }
    else {
        maxlimit = num + 1;
    }
    for (var i = 0; i <= maxlimit; i++) {
        var name = $('#txtsavingrelation' + val).val();
        if (name == $('#txtNameInfo').val()) {
            var age = $('#txtAgeInfo').val();
            $('#txtsavingage' + val).val(age);
        }
        if (name == $('#txtSpouseNameInfo').val()) {
            var age = $('#txtSpouseAgeInfo').val();
            $('#txtsavingage' + val).val(age);
        }
        var dependname = $('#txtsavingrelation' + val).val();
        if (dependname == $('#dtdependantName' + i).val()) {
            var age = $('#txtdependantAge' + i).val();
            $('#txtsavingage' + val).val(age);
        }
    }
    //for (var i = 0; i <= num + val; i++) {
    //    var name = $('#txtsavingrelation' + i).val();
    //    if (name == $('#txtNameInfo').val()) {
    //        var age = $('#txtAgeInfo').val();
    //        $('#txtsavingage' + i).val(age);
    //    }
    //    if (name == $('#txtSpouseNameInfo').val()) {
    //        var age = $('#txtSpouseAgeInfo').val();
    //        $('#txtsavingage' + i).val(age);
    //    }
    //}
    //for (var i = 0; i <= num+val; i++) {
    //    j = i - val;
    //    var name = $('#txtsavingrelation' +i).val();
    //    if (name == $('#dtdependantName' + j).val()) {
    //        var age = $('#txtdependantAge' + j).val();
    //        $('#txtsavingage' + i).val(age);
    //    }
    //}
    calcMaturityAge();
}
function clearValidation() {

    writeMultipleMessage("error", "", "txtProIntrestRate");
    writeMultipleMessage("error", "", "txtInflationrateFNA");
    writeMultipleMessage("error", "", "txtnoofyearFNA");
    writeMultipleMessage("error", "", "txtInterestrateFNA");
    writeMultipleMessage("error", "", "txtInflationrate");
    writeMultipleMessage("error", "", "txtnoofyear");
    writeMultipleMessage("error", "", "txtInterestrate");
    writeMultipleMessage("error", "", "txtEduInflationRate");
    writeMultipleMessage("error", "", "txtEduInterestRate");
    writeMultipleMessage("error", "", "txtSavInflationRate");
}
function CalculateToYear() {
    var FromYear = parseInt($("#txtFromYear").val());
        if (isNaN(FromYear)) {
            FromYear = 0;
        }
        var Time = parseInt($("#txtnoofyear").val(), 10);
        if (isNaN(Time)) {
            Time = 10;
        }
        var ToYear = parseInt(FromYear) + parseInt(Time);
        if (isNaN(ToYear)) {
            ToYear = 0;
        }
        $("#txtToYear").val(ToYear);
}
function CalculateToYearFNA() {
    
        var FromYear = parseInt($("#txtFromYearFNA").val());
        if (isNaN(FromYear)) {
            FromYear = 0;
        }
        var Time = parseInt($("#txtnoofyearFNA").val(), 10);
        if (isNaN(Time)) {
            Time = 10;
        }
        var ToYear = parseInt(FromYear) + parseInt(Time);
        if (isNaN(ToYear)) {
            ToYear = 0;
        }
        $("#txtToYearFNA").val(ToYear);
    }
$(document).ready(function () {
    $('.need-analysis-list li').click(function () {
        debugger;
        if ($(this).find('a').prop('id') == 'needidentificationtab') {
            $('#chartdiv').css({ 'position': 'relative', 'z-index': '2' });
        }
        else {
            $('#chartdiv').css({ 'position': 'absolute', 'z-index': '-2' });
        }
        if (($(this).find('a').prop('id') == 'collapseFinancialAnalysis') && $('.financial_analysis_calculator_protection').hasClass('calculator-details-active')) {
            $('#Protectionchartdiv').css({ 'position': 'relative', 'z-index': '2', 'top': '-5px' });
        }
        else if (($(this).find('a').prop('id') == 'collapseFinancialAnalysis') && $('.financial_analysis_calculator_health').hasClass('calculator-details-active')) {
            $('#healthchartdiv').css({ 'position': 'relative', 'z-index': '2', 'top': '-5px' });
        }
        else {
            $('#Protectionchartdiv').css({ 'position': 'absolute', 'z-index': '-2', 'top': '-233px' });
            $('#healthchartdiv').css({ 'position': 'absolute', 'z-index': '-2', 'top': '-233px' });

        }
    })
})