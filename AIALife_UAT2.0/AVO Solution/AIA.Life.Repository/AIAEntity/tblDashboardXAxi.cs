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
    
    public partial class tblDashboardXAxi
    {
        public long XaxisId { get; set; }
        public string XAxisTitle { get; set; }
        public string XAxisValue { get; set; }
        public string ColorCodeTitle { get; set; }
        public string ColorCode { get; set; }
        public long SubReportID { get; set; }
    
        public virtual tblDashboardSubReport tblDashboardSubReport { get; set; }
    }
}
