using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Models
{
    public class ScanCodeExistsModel
    {
        public string ScanCode { get; set; }
        public bool Exists { get; set; }
    }
}
