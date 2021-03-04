using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ClaimsProject.Models;
using System.IO;
using System.Globalization;
using CsvHelper;
using System;

namespace ClaimsProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ClaimController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Sample API: https://localhost:44301/api/claim/12-5-2020");
        }

        [HttpGet("{claimdate}")]
        public IActionResult GetMemberClaims(DateTime claimdate)
        {
            var resp = new List<Member>();

            //read files to models
            var claims = new List<MemberClaim>();
            var members = new List<Member>();

            var memberClaims = new List<MemberClaim>();

            var folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "TestFiles");

            //read claims data
            using (var reader = new StreamReader(Path.Combine(folderPath, "Claim.csv")))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    claims = csv.GetRecords<MemberClaim>().ToList();
                }
            }

            if (claims != null && claims.Count > 0)
            {
                //search claims
                var filterClaims = claims.Where(x => x.ClaimDate == claimdate).ToList();

                if (filterClaims != null)
                {
                    //read members data
                    using (var reader = new StreamReader(Path.Combine(folderPath, "Member.csv")))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            members = csv.GetRecords<Member>().ToList();
                        }
                    }

                    //attach claims to memebers

                    filterClaims.ForEach(claim =>
                    {
                        var member = members.Where(x => x.MemberID == claim.MemberID).FirstOrDefault();
                        if (member != null)
                        {
                            member.Claims = new List<MemberClaim>();

                            member.Claims.Add(claim);
                            resp.Add(member);
                        }
                    });
                }
            }

            if (resp != null && resp.Count > 0)
                return Ok(resp);
            else
                return Ok("No claims found");
        }
    }
}
