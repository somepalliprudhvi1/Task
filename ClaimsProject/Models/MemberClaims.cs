using System;
using System.Collections.Generic;

namespace ClaimsProject.Models
{
    public class Member
    {
        public string MemberID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<MemberClaim> Claims { get; set; }
    }

    public class MemberClaim
    {
        public string MemberID { get; set; }
        public DateTime ClaimDate { get; set; }
        public Decimal ClaimAmount { get; set; }
    }
}
