using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvManagementApp.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
