using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AIA.Life.Models.Integration
{


    public class BasicCover
    {
        public int basicSumAssured { get; set; }
    }

    public class RiderLevel
    {
        public string riderCode { get; set; }
        public string minSumAssured { get; set; }
        public string maxSumAssured { get; set; }
        public int sumAssured { get; set; }
    }

    public class MainInsured
    {
        public string age { get; set; }
        public string occupationCode { get; set; }
        public string monthlyIncome { get; set; }
        public BasicCover basicCover { get; set; }
        public List<RiderLevel> riderLevel { get; set; }
    }

    public class BasicCover2
    {
        public string basicSumAssured { get; set; }
    }

    public class RiderLevel2
    {
        public string riderCode { get; set; }
        public string minSumAssured { get; set; }
        public string maxSumAssured { get; set; }
        public string sumAssured { get; set; }
    }

    public class Spouse
    {
        public string age { get; set; }
        public string occupationCode { get; set; }
        public string monthlyIncome { get; set; }
        public BasicCover2 basicCover { get; set; }
        public List<RiderLevel2> riderLevel { get; set; }
    }

    public class BasicCover3
    {
        public string basicSumAssured { get; set; }
    }

    public class RiderLevel3
    {
        public string riderCode { get; set; }
        public string minSumAssured { get; set; }
        public string maxSumAssured { get; set; }
        public string sumAssured { get; set; }
    }

    public class Child
    {
        public string age { get; set; }
        public string occupationCode { get; set; }
        public string monthlyIncome { get; set; }
        public BasicCover3 basicCover { get; set; }
        public List<RiderLevel3> riderLevel { get; set; }
    }

    public class RootObject
    {
        public string planCode { get; set; }
        public string policyTerm { get; set; }
        public string proposalDate { get; set; }
        public string postalCode { get; set; }
        public string modeOfPayment { get; set; }
        public string frequency { get; set; }
        public string premiumpaymentTerm { get; set; }
        
        public MainInsured mainInsured { get; set; }
        public Spouse spouse { get; set; }
        public List<Child> children { get; set; }
    }




}

namespace AIA.Life.Models.Integration.PremiumResponse
{


    public class BasicCover
    {
        public string purePremium { get; set; }
        public string occupationalLoading { get; set; }
    }

    public class RiderLevel
    {
        public string riderCode { get; set; }
        public string purePremium { get; set; }
        public string occupationalLoading { get; set; }
    }

    public class MainInsured
    {
        public MainInsured()
        {
            basicCover = new BasicCover();
            riderLevel = new List<RiderLevel>();
        }
        public BasicCover basicCover { get; set; }
        public List<RiderLevel> riderLevel { get; set; }
        public string halfYearlyPremium { get; set; }
        public string monthlyPremium { get; set; }
        public string quaterlyPremium { get; set; }
        public string annualPremium { get; set; }

    }
  
    public class BasicCover2
    {
        public string purePremium { get; set; }
        public string occupationalLoading { get; set; }
    }

    public class RiderLevel2
    {
        public string riderCode { get; set; }
        public string purePremium { get; set; }
        public string occupationalLoading { get; set; }
    }

    public class Spouse
    {
        public Spouse()
        {
            basicCover = new BasicCover2();
            riderLevel = new List<RiderLevel2>();
        }
        public BasicCover2 basicCover { get; set; }
        public List<RiderLevel2> riderLevel { get; set; }
        public string halfYearlyPremium { get; set; }
        public string monthlyPremium { get; set; }
        public string quaterlyPremium { get; set; }
        public string annualPremium { get; set; }
    }

    public class BasicCover3
    {
        public string purePremium { get; set; }
        public string occupationalLoading { get; set; }
    }

    public class RiderLevel3
    {
        public string riderCode { get; set; }
        public string purePremium { get; set; }
        public string occupationalLoading { get; set; }
    }

    public class Child
    {
        public Child()
        {
            basicCover = new BasicCover3();
            riderLevel = new List<RiderLevel3>();
        }
        public BasicCover3 basicCover { get; set; }
        public List<RiderLevel3> riderLevel { get; set; }
        public string halfYearlyPremium { get; set; }
        public string monthlyPremium { get; set; }
        public string quaterlyPremium { get; set; }
        public string annualPremium { get; set; }
    }

    public class Data
    {
        public Data()
        {
            mainInsured = new MainInsured();
            spouse = new Spouse();
            children = new List<Child>();
        }
        public string planCode { get; set; }
        //public int policyAdminFee { get; set; }
        //public int cess { get; set; }
        //public double vat { get; set; }
        public string policyAdminFee { get; set; }
        public string cess { get; set; }
        public string vat { get; set; }
        public string annualPremium { get; set; }
        public string halfYearlyPremium { get; set; }      
        public string quaterlyPremium { get; set; }
        public string monthlyPremium { get; set; }
        public string totalPremium { get; set; }
        public MainInsured mainInsured { get; set; }
        public Spouse spouse { get; set; }
        public List<Child> children { get; set; }
    }

    public class RootObject
    {
        public RootObject()
        {
            data = new Data();
        }
        public string message { get; set; }
        public string status { get; set; }
        public Data data { get; set; }
    }
}


