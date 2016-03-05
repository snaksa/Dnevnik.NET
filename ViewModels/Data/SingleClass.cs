using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.ViewModels.Data
{
    public class SingleClass
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Letter { get; set; }
        public string NumberAndLetter
        {
            get
            {
                return Number.ToString() + Letter;
            }
        }
    }
}
