namespace Kursovaya.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [Table("Client")]
    public partial class Managers:INotifyPropertyChanged
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Managers()
        {
            Suppliers = new HashSet<Suppliers>();
        }

        
        private int MenegerID;
        [Key]
        public int menagerID
        {
            get { return MenegerID; }
            set
            {
                MenegerID = value;
                OnPropertyChanged("menegerID");
            }
        }

       
        private string FIOMeneger;
        [Required]
        [StringLength(50)]
        public string fiomeneger
        {
            get { return FIOMeneger; }
            set
            {
                FIOMeneger = value;
                OnPropertyChanged("fiomeneger");
            }
        }


        private string ViewTovarManager;
        [Required]
        [StringLength(50)]
        public string viewtovarmanager
        {
            get { return ViewTovarManager; }
            set
            {
                ViewTovarManager = value;
                OnPropertyChanged("viewtovarmanager");
            }
        }
       
        private string PhoneManager;
        [Required]
        [StringLength(50)]
        public string phonemanager
        {
            get { return PhoneManager; }
            set
            {
                PhoneManager = value;
                OnPropertyChanged("phonemanager");
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Suppliers> Suppliers { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
