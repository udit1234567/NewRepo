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
    
    public partial class tblUserOTP
    {
        public int id { get; set; }
        public System.Guid UserID { get; set; }
        public string LoginName { get; set; }
        public string OTPGenerated { get; set; }
        public Nullable<System.DateTime> GeneartedDateTime { get; set; }
    }
}