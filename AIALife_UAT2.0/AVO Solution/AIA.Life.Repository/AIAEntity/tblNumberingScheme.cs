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
    
    public partial class tblNumberingScheme
    {
        public decimal NumberingSchemeId { get; set; }
        public string fixedcode { get; set; }
        public int nextnumber { get; set; }
        public int highestnumber { get; set; }
        public int step { get; set; }
        public System.Guid rowguid { get; set; }
        public string NumberingType { get; set; }
    }
}
