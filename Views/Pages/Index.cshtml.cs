using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CvManagementApp.Models;

namespace Views.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CvManagementApp.Models.CvManagementDbContext _context;

        public IndexModel(CvManagementApp.Models.CvManagementDbContext context)
        {
            _context = context;
        }

        public IList<Candidate> Candidate { get; set; }
        public IList<Degree> Degree { get; set; }

        public async Task OnGetAsync()
        {
            Candidate = await _context.Candidates.ToListAsync();
            Degree = await _context.Degrees.ToListAsync();
        }
    }
}
