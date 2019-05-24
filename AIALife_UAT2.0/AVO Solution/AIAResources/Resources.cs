using AIAResources.Abstract;
using AIAResources.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIAResources
{
        public class Resources
    {
        private static IResourceProvider resourceProvider = new DbResourceProvider(); //  new XmlResourceProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Resources.xml"));

        /// <summary>Menu</summary>
        public static string GetMenu(string Menu, string CultureInfo)
        {

            return resourceProvider.GetResource(Menu, CultureInfo) as String;

        }

        /// <summary>Menu</summary>
        public static string GetLabelName(string Label)
        {

            return resourceProvider.GetResource(Label, CultureInfo.CurrentUICulture.Name) as String;

        }
        /// <summary>Email</summary>
        public static string NewBusiness
        {
            get
            {
                return resourceProvider.GetResource("New Business", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        /// <summary>Email</summary>
        public static string Email
        {
            get
            {
                return resourceProvider.GetResource("Email", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

      

        /// <summary>First name</summary>
        public static string FirstName
        {
            get
            {
                return resourceProvider.GetResource("First Name", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        /// <summary>Must be less than 50 characters</summary>
        public static string FirstNameLong
        {
            get
            {
                return resourceProvider.GetResource("FirstNameLong", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        /// <summary>First name is required</summary>
        public static string FirstNameRequired
        {
            get
            {
                return resourceProvider.GetResource("FirstNameRequired", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        /// <summary>Last name</summary>
        public static string LastName
        {
            get
            {
                return resourceProvider.GetResource("Last Name", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        /// <summary>Must be less than 50 characters</summary>
        public static string LastNameLong
        {
            get
            {
                return resourceProvider.GetResource("LastNameLong", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        /// <summary>Last name is required</summary>
        public static string LastNameRequired
        {
            get
            {
                return resourceProvider.GetResource("LastNameRequired", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

     
        public static string ClientCode
        {
            get
            {
                return resourceProvider.GetResource("Client Code", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        public static string BranchName
        {
            get
            {
                return resourceProvider.GetResource("Branch Name", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string FGChannel
        {
            get
            {
                return resourceProvider.GetResource("FG Channel", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

        public static string Search
        {
            get
            {
                return resourceProvider.GetResource("Search", CultureInfo.CurrentUICulture.Name) as String;
            }
        }

      
        public static string ClientType
        {
            get
            {
                return resourceProvider.GetResource("Client Type", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Individual
        {
            get
            {
                return resourceProvider.GetResource("Individual", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Organization
        {
            get
            {
                return resourceProvider.GetResource("Organization", CultureInfo.CurrentUICulture.Name) as String;
            }
        }


        public static string CorporateName
        {
            get
            {
                return resourceProvider.GetResource("Corporate Name", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Gender
        {
            get
            {
                return resourceProvider.GetResource("Gender", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string DateOfBirth
        {
            get
            {
                return resourceProvider.GetResource("DateOfBirth", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string MaritalStatus
        {
            get
            {
                return resourceProvider.GetResource("Marital Status", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string ContactPerson
        {
            get
            {
                return resourceProvider.GetResource("Contact Person", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Address1
        {
            get
            {
                return resourceProvider.GetResource("Address1", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Address2
        {
            get
            {
                return resourceProvider.GetResource("Address2", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Address3
        {
            get
            {
                return resourceProvider.GetResource("Address3", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Pincode
        {
            get
            {
                return resourceProvider.GetResource("Pincode", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string City
        {
            get
            {
                return resourceProvider.GetResource("City", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string District
        {
            get
            {
                return resourceProvider.GetResource("District", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string State
        {
            get
            {
                return resourceProvider.GetResource("State", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string Country
        {
            get
            {
                return resourceProvider.GetResource("Country", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string TelNoOffice
        {
            get
            {
                return resourceProvider.GetResource("Tel No Office", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string MobileNumber
        {
            get
            {
                return resourceProvider.GetResource("Mobile Number", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string EmailId
        {
            get
            {
                return resourceProvider.GetResource("Email Id", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string PAN
        {
            get
            {
                return resourceProvider.GetResource("PAN", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
        public static string PanHolderName
        {
            get
            {
                return resourceProvider.GetResource("Pan Holder Name", CultureInfo.CurrentUICulture.Name) as String;
            }
        }
    
      



    }
}
