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
    
    public partial class tblProductQuestionMapping
    {
        public int MapId { get; set; }
        public int QID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> SequenceNo { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual tblProduct tblProduct { get; set; }
    }
}
