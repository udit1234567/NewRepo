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
    
    public partial class tblAppVersion
    {
        public int ID { get; set; }
        public string AppName { get; set; }
        public string CloudPath { get; set; }
        public string VersioNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string APKType { get; set; }
        public Nullable<int> isMandatory { get; set; }
        public string CustomMessage { get; set; }
    }
}
