//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AIA.Life.Repository.AIAEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblResourceCategoryDetail
    {
        public int ResourcecatagoryDetailsPK { get; set; }
        public Nullable<int> ContentType { get; set; }
        public Nullable<int> ContentLanguage { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string FileDescription { get; set; }
        public string Link { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ResourcecatagoryPK { get; set; }
        public string FileName { get; set; }
        public string ImgName { get; set; }
    
        public virtual tblResourceCatagory tblResourceCatagory { get; set; }
        public virtual tblResourceCatagory tblResourceCatagory1 { get; set; }
    }
}
