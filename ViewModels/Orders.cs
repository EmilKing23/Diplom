using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomKarakuyumjyan.ViewModels
{
    public class Orders
    {
        int Id { get; set; }
        public string EmployerName { get; set; }
        public string ManagerName { get; set; }
        public string Status { get; set; }
        public string Service { get; set; }
        public string DateStart { get; set; }

        public string DateEnd { get; set; }

        

    }
}
