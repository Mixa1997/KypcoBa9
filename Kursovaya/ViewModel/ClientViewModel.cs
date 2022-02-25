using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Kursovaya.Model;

namespace Kursovaya.ViewModel
{
    class ClientViewModel:INotifyPropertyChanged
    {
        private Managers selectedClient;
        public ObservableCollection<Managers> Clients { get; set; }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Managers client= new Managers();
                      Clients.Add(client);
                      SelectedClient = client;
                }));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      Managers p1;
                      using (SustemShopDB db = new SustemShopDB())
                      {
                          p1 = db.Managers.Where(p=>p.menagerID==SelectedClient.menagerID).FirstOrDefault();
                      }
                      using (SustemShopDB db = new SustemShopDB())
                      {
                          if (p1 != null)
                          {
                              p1.fiomeneger = SelectedClient.fiomeneger;
                              db.Entry(p1).State = System.Data.Entity.EntityState.Modified;
                              db.SaveChanges();
                          }
                          else
                          {
                              db.Managers.Add(SelectedClient);
                              db.SaveChanges();
                          }
                      }
                  }));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      using (SustemShopDB db = new SustemShopDB())
                      {
                          Managers p1 = db.Managers.Where(p => p.menagerID == SelectedClient.menagerID).FirstOrDefault();
                          if (p1 != null)
                          {
                              db.Entry(p1).State = System.Data.Entity.EntityState.Deleted;
                              db.SaveChanges();
                              Clients.Remove(SelectedClient);
                          }
                      }

                  }));
            }
        }
        public Managers SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        public void FilterByFio(String fio)
        {
            using (SustemShopDB db = new SustemShopDB())
            {
                List<Managers> managers = db.Managers.Where(p=>p.fiomeneger.StartsWith(fio)).ToList();
                foreach (Managers c in managers) Clients.Add(c);
            }
        }

        public ClientViewModel()
        {
            Clients = new ObservableCollection<Managers>();
            using (SustemShopDB db = new SustemShopDB())
            {
                List<Managers> clients = db.Managers.ToList();
                foreach (Managers c in clients) Clients.Add(c);
            }
        }

        public ClientViewModel(string fio)
        {
            Clients = new ObservableCollection<Managers>();
            using (SustemShopDB db = new SustemShopDB())
            {
                List<Managers> clients = db.Managers.Where(p => p.fiomeneger.StartsWith(fio)).ToList();
                 foreach (Managers c in clients) Clients.Add(c);
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                 PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    //
    }
}
