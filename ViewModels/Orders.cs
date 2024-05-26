using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomKarakuyumjyan.ViewModels
{
    public class Orders
    {
        //public Orders(ДипломEntities диплом) 
        //{
        //    OrdersCollection = new List<Orders>();

        //}

        //public List<Orders> OrdersCollection { get; set; }

        public int Id { get; set; }
        public int StatusId { get; set; }
        public string EmployerName { get; set; }
        public string ManagerName { get; set; }
        public string Status { get; set; }
        public string Service { get; set; }
        public string DateStart { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string ClientName { get; set; }
        public string DateEnd { get; set; }
        public string Phone { get; set; }



    }
}
