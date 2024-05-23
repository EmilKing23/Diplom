using DiplomKarakuyumjyan.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using static DiplomKarakuyumjyan.Pages.ClientsPage;

namespace DiplomKarakuyumjyan
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        ДипломEntities context = new ДипломEntities();
        public ReportsViewModel()
        {
            SelectedReport = new Reports 
            { 
                  Id = 0,
                  Adress = "adasd",
                  Client = new Clients
                  {
                      Adress = "awda",
                      Name = "Name",
                      Phone = "120893983"
                  }

            
            };


            Заявки searchOrder;
            Клиенты client;
            Работники employer;
            ReportsList = new List<Reports>();
            foreach (var item in context.Отчеты)
            {
                searchOrder = context.Заявки.First(_ => _.IDЗаявки.Equals(item.IDЗаявки));
                client = context.Клиенты.First(_ => _.IDКлиента.Equals(item.Заявки.IDКлиента));
                employer = context.Работники.First(_ => _.IDРаботника.Equals(item.Заявки.IDРаботника));



                ReportsList.Add(new Reports()
                {
                    Id = item.IDОтчета,
                    Description = item.ОписаниеРабот,
                    Client = new Clients()
                    {
                        Name = $"{client.Фамилия} {client.Имя} {client.Отчество}",

                        Phone = $"{client.НомерТелефона}"
                    },
                    Order = new Orders
                    {
                        DateEnd = searchOrder.ПлановаяДатаОкончанияРабот.Value.ToString(),
                        DateStart = searchOrder.ПлановаяДатаНачалаРабот.Value.ToString(),
                        Status = searchOrder.СтатусРаботы.Наименование,
                        Service = context.ВидыРабот.First(_=>_.IDВида.Equals(searchOrder.IDВида)).Наименование,
                    }

                });
            }
        }
        public Reports SelectedReport { get; set; }
        public List<Reports> ReportsList { get; set; }

        public class Reports
        {

            public int Id { get; set; }
            public string Description { get; set; }
            public Orders Order { get; set; }
            public string Employer { get; set; }
            public string Adress { get; set; }
            public Clients Client { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!(object.Equals(field, newValue)))
            {
                field = (newValue);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}