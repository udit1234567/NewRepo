using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class AuthorizeUser
    {
        public AuthorizeUser()
        {
            Error = new Error();
        }
        public string UserName { get; set; }
        public int ContactId { get; set; }
        public string QuoteNo { get; set; }
        public string ProposalNo { get; set; }
        public decimal PolicyId { get; set; }
        public Error Error { get; set; }
    }
}
