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
    
    public partial class tblMasPermissionNew
    {
        public int PermissionID { get; set; }
        public Nullable<System.Guid> AppID { get; set; }
        public string ItemType { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> MenuID { get; set; }
        public string ItemDescription { get; set; }
        public string url { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<decimal> ItemID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string ControllerDesc { get; set; }
        public string ActionDesc { get; set; }
        public Nullable<decimal> MasterMenuID { get; set; }
        public long TempID { get; set; }
    }
}
