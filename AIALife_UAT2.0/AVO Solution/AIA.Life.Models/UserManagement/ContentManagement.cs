using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class ContentManagement
    {
        [Required]
        public string Resoucecatagory { get; set; }
        [Required]
        public string EffectiveDate { get; set; }
        public DateTime? EffectiveDate1 { get; set; }
        [Required]
        public string ExpiryDate { get; set; }
        public int ResourceID { get; set; }
        public int ResourceChildID { get; set; }
        public bool EditMode { get; set; }
        public List<ContentManagement> lstContentManagement { get; set; }
        public List<ContentManagementDeatils> lstContentManagementDeatils { get; set; }
        public ContentManagementDeatils objContentManagementDeatils { get; set; }
    }
    public class ContentManagementDeatils
    {
        [Required]
        public int ContentType { get; set; }
        [Required]
        public int ContentLanguage { get; set; }        
        public string EffectiveDate { get; set; }        
        public string ExpiryDate { get; set; }
        public string FileDescription { get; set; }
        public string Link { get; set; }
        public string FileName { get; set; }
        public string ImgLink { get; set; }
        public string ImgName { get; set; }
        public string ContentTypeName { get; set; }
        public string ContentLanguageName { get; set; }
        public string Resoucecatagory { get; set; }
        public int ResoucecatagoryFK { get; set; }
        public int ResoucecatagoryDetailsPK { get; set; }
        public List<MasterListItem> lstContentType { get; set; }
        public List<MasterListItem> lstContentLanguage { get; set; }
    }
    public class ResouceManagent
    {
        public string UserName { get; set; }
        public string ServiceTraceID { get; set; }
        public string Message { get; set; }
        public List<ResourceCatagory> lstResourceCatagory { get; set; }        
    }
    public class ResourceCatagory
    {
        public string Resoucecatagory { get; set; }        
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
        public int? ResoucecatagoryPK { get; set; }
       public List<ResourceCatagorydetails> lstResourceCatagorydetails { get; set; }
    }
    public class ResourceCatagorydetails
    {
        public int? ContentType { get; set; }        
        public int? ContentLanguage { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
        public string FileDescription { get; set; }
        public string Link { get; set; }
        public string imgLink { get; set; }
        public int? ResoucecatagoryDetailsPK { get; set; }
    }
}
