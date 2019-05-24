using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AIA.Life.Models.DashBoard
{
    public class GraphDetails
    {
        public GraphDetails()
        {
            this.DataProvider = new List<Dictionary<string, string>>();
            this.lstvalueAxes = new List<ValueAxes>();
            this.lstGraphs = new List<Graphs>();
            this.categoryAxis = new CategoryAxis();
            this.legend = new Legend();
            this.myGoal = new MyGoal();
            this.graphs = new Graphs();
            this.valueAxes = new ValueAxes();
        }
        public string GraphType { get; set; }
        public bool IsDrillDown { get; set; }
        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public string Theme { get; set; }
        public string Type { get; set; }
        public decimal StartDuration { get; set; }
        public decimal PlotAreaFillAlphas { get; set; }
        public int? Depth3D { get; set; }
        public int? Angle { get; set; }
        public string CategoryField { get; set; }
        public List<Dictionary<string, string>> DataProvider { get; set; }

        public List<ValueAxes> lstvalueAxes { get; set; }
        public ValueAxes valueAxes { get; set; }
        public List<Graphs> lstGraphs { get; set; }
        public Graphs graphs { get; set; }
        public CategoryAxis categoryAxis { get; set; }
        public string strDataProvider { get; set; }
        public string strvalueAxes { get; set; }
        public string strlstGraphs { get; set; }
        public string strcategoryAxis { get; set; }
        public Legend legend { get; set; }
        public string strlegend { get; set; }
        public string valueField { get; set; }
        public string titleField { get; set; }
        public string balloonText { get; set; }
        public decimal outlineAlpha { get; set; }
        public string Growth { get; set; }
        public string GraphHeight { get; set; }
        public string GraphWidth { get; set; }
        public bool Rotate { get; set; }
        public string CustomerGroupName { get; set; }
        public bool IsGrowth { get; set; }
        public string Message { get; set; }
        public MyGoal myGoal { get; set; }
        public MyCalendar appointment { get; set; }
        public List<MyCalendar> appointmentList { get; set; }
        public myReminder reminder { get; set; }
        public List<myReminder> reminderList { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
    }
    public class ValueAxes
    {

        public string unit { get; set; }
        public string position { get; set; }
        public string title { get; set; }
        public bool labelsEnabled { get; set; }
    }
    public class Graphs
    {
        public string balloonText { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string valueField { get; set; }
        public string valueAxis { get; set; }
        public decimal fillAlphas { get; set; }
        public decimal lineAlpha { get; set; }
        public string color { get; set; }
        public string colorField { get; set; }
        public string labelText { get; set; }
        public string labelFunction { get; set; }
        public string labelPosition { get; set; }
        public bool newStack { get; set; }
        public string bullet { get; set; }
        public bool useLineColorForBulletBorder { get; set; }
        public double bulletBorderAlpha { get; set; }
        public string bulletColor { get; set; }
        public double bulletSize { get; set; }
        public string legendValueText { get; set; }
        public int? fixedColumnWidth { get; set; }
        public string fillColorsField { get; set; }
        public double lineThickness { get; set; }

        public string urlField { get; set; }
        public string alphaField { get; set; }
        public string dashLengthField { get; set; }
    }
    public class CategoryAxis
    {
        public CategoryAxis()
        {
            this.autoWrap = true;
        }
        public string gridPosition { get; set; }
        public string position { get; set; }
        public decimal axisAlpha { get; set; }
        public decimal gridAlpha { get; set; }
        public bool autoWrap { get; set; }
        public int labelRotation { get; set; }
        public decimal tickLength { get; set; }

    }
    public class Legend
    {
        public bool useGraphSettings { get; set; }
        public decimal horizontalGap { get; set; }
        public decimal markerSize { get; set; }
        public string position { get; set; }
        public string color { get; set; }
        public decimal maxColumns { get; set; }
    }
    public class MyGoal
    {
        public string mydreamOptions { get; set; }
        public string laptopMake { get; set; }
        public string laptopModel { get; set; }
        public string laptopmemorySpace { get; set; }
        public string laptopPrice { get; set; }
        public DateTime? laptoptargetDate { get; set; }
        public string motorCycleMake { get; set; }
        public string motorcycleModel { get; set; }
        public string motorcycleColour { get; set; }
        public string motorcyclePrice { get; set; }
        public DateTime? motorcycletargetDate { get; set; }
        public string vehicleMake { get; set; }
        public string vehicleModel { get; set; }
        public string vehicleseatingCapacity { get; set; }
        public string vehiclePrice { get; set; }
        public string vehicleColour { get; set; }
        public DateTime? vehicletargetDate { get; set; }

        public string dailypolicyNoValue { get; set; }
        public string weeklypolicyNoValue { get; set; }
        public string monthlypolicyNoValue { get; set; }
        public string dailyavgpolicyValue { get; set; }
        public string weeklyavgpolicyValue { get; set; }
        public string monthlyavgpolicyValue { get; set; }
        public string dailyMCFPValue { get; set; }
        public string weeklyMCFPValue { get; set; }
        public string monthlyMCFPValue { get; set; }
        public string dailyyear1PValue { get; set; }
        public string weeklyyear1PValue { get; set; }
        public string monthlyyear1PValue { get; set; }
        public string dailyyear2PValue { get; set; }
        public string weeklyyear2PValue { get; set; }
        public string monthlyyear2PValue { get; set; }
        public string dailyyear3PValue { get; set; }
        public string weeklyyear3PValue { get; set; }
        public string monthlyyear3PValue { get; set; }

        public string dailysuspectsValue { get; set; }
        public string weeklysuspectsValue { get; set; }
        public string monthlysuspectsValue { get; set; }
        public string suspectConvRatioValue { get; set; }
        public string dailycnfmprospectsValue { get; set; }
        public string weeklycnfmprospectsValue { get; set; }
        public string monthlycnfmprospectsValue { get; set; }
        public string prospectConvRatioValue { get; set; }

        public string dailycustappointValue { get; set; }
        public string weeklycustappointValue { get; set; }
        public string monthlycustappointValue { get; set; }
        public string custappointConvRatioValue { get; set; }

        public string dailyneedanalysisValue { get; set; }
        public string weeklyneedanalysisValue { get; set; }
        public string monthlyneedanalysisValue { get; set; }
        public string needanalysisConvRatioValue { get; set; }

        public string dailyquotationValue { get; set; }
        public string weeklyquotationValue { get; set; }
        public string monthlyquotationValue { get; set; }
        public string quotationConvRatioValue { get; set; }

        public string dailyproposalValue { get; set; }
        public string weeklyproposalValue { get; set; }
        public string monthlyproposalValue { get; set; }
        public string proposalConvRatioValue { get; set; }

        public string firstmonthcommValue { get; set; }
        public string firstmonthincentiveValue { get; set; }
        public string firstyearcommValue { get; set; }
        public string renewalcommValue { get; set; }
        public string orValue { get; set; }
        public string oronORValue { get; set; }
        public string packageValue { get; set; }
        public string totalIncomeValue { get; set; }
        public string monthlyIncomeValue { get; set; }
        public string planincomeValue { get; set; }
        public string dreamandplanValue { get; set; }
    }
    public class MyCalendar
    {
        public DateTime? appointmentdate { get; set; }
        public DateTime? timeFrom { get; set; }
        public DateTime? timeTo { get; set; }
        public string subject { get; set; }
        public string meetingWith { get; set; }
        public string description { get; set; }
        public HttpPostedFileBase[] uploadFiles { get; set; }
    }
    public class myReminder
    {
        public DateTime? reminderDate { get; set; }
        public DateTime? reminderTime { get; set; }
        public string reminderSubject { get; set; }
    }
    public class myProfile
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string workPhone { get; set; }
        public string homePhone { get; set; }
        public string emailId { get; set; }
        public string NIC { get; set; }
        public string designation { get; set; }
        public string maritalStatus { get; set; }
        public DateTime? weddingAnniversaryDate { get; set; }
        public string familyName { get; set; }
        public string familyRelationship { get; set; }
        public DateTime? DOB { get; set; }
        public string supervisorName { get; set; }
        public string supervisormobileNo { get; set; }
        public string supervisorEmailId { get; set; }
        public string supervisorDesignation { get; set; }
    }
    public class statusEnquiry
    {
        public string statusType { get; set; }
        public string transactionId { get; set; }
        public string contactNo { get; set; }
        public string NIC { get; set; }
        public bool status { get; set; }
    }
}
