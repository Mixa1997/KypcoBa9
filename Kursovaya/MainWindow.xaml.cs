using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kursovaya.Model;
using Kursovaya.ViewModel;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SustemShopDB"].
            ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
            UpdateTovar();
            edit.Visibility = Visibility.Collapsed;
            delete.Visibility = Visibility.Collapsed;
            List.SelectedIndex = 0;
            DataContext = new ClientViewModel();
            UpdateSuppliers();
            using (SustemShopDB db = new SustemShopDB())
            {
                List<Managers> managers = db.Managers.ToList();
                foreach(Managers managers1 in managers)
                {
                    MenegerID.Items.Add(managers1.fiomeneger);
                }
                List<Tovar> tovar = db.Tovar.ToList();
                foreach (Tovar tovar1 in tovar)
                {
                    TovarID.Items.Add(tovar1.TovarName);
                }
            }
          

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (SustemShopDB db = new SustemShopDB())
            {
                Tovar tovar = new Tovar();
                tovar.TovarName = TovarName.Text;
                tovar.TovarKol = TovarKol.Text;
                tovar.TovarPrice = decimal.Parse(TovarPrice.Text);
                tovar.ViewTovar = ViewTovar.Text;
                db.Tovar.Add(tovar);
                db.SaveChanges();
                UpdateTovar();
            }
        }
        private void UpdateTovar()
        {
            using (SustemShopDB db = new SustemShopDB())
            {
                var Tovars = db.Tovar.ToList();
                TovarTable.ItemsSource = null;
                TovarTable.ItemsSource = Tovars;
            }
            TovarName.Text = "";
            TovarKol.Text = "";
            TovarPrice.Text = "";
            ViewTovar.Text = "";
        }

        private void UpdateSuppliers()
        {
            SuppliersTab.ItemsSource = null;
            using (SustemShopDB db = new SustemShopDB())
            {
                var result = from suppliers in db.Suppliers
                             join tovar in db.Tovar on suppliers.TovarID equals tovar.TovarID
                             join managers in db.Managers on suppliers.MenegerID equals managers.menagerID
                             select new SuppliersModel
                             {
                                 data = suppliers.data,
                                 MenegerID = suppliers.MenegerID,
                                 Pricetovar = suppliers.Pricetovar,
                                 TovarID = suppliers.TovarID,
                                 Koltovar = suppliers.Koltovar,
                                 ID = suppliers.ID
                             };
                SuppliersTab.ItemsSource = result.ToList();
            }
        }

        private void TovarTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            edit.Visibility = Visibility.Visible;
            delete.Visibility = Visibility.Visible;
            add.Visibility = Visibility.Collapsed;

            Tovar tovar = TovarTable.SelectedItem as Tovar;
            if (tovar != null)
            {
                TovarName.Text = tovar.TovarName;
                TovarKol.Text = tovar.TovarKol;
                TovarPrice.Text = tovar.TovarPrice.ToString();
                ViewTovar.Text = tovar.ViewTovar;
            }
            
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            Tovar tovar = TovarTable.SelectedItem as Tovar;
            if (tovar != null)
            {
                TovarName.Text = tovar.TovarName;
                TovarKol.Text = tovar.TovarKol;
                TovarPrice.Text = tovar.TovarPrice.ToString();
                ViewTovar.Text = tovar.ViewTovar;
                using (SustemShopDB db = new SustemShopDB())
                {
                    db.Entry(tovar).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                UpdateTovar();
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Tovar tovar = TovarTable.SelectedItem as Tovar;
            if (tovar != null)
            {
                using (SustemShopDB db = new SustemShopDB())
                {
                    db.Entry(tovar).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                UpdateTovar();
            }
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            String str = filter.Text;
            if (List.SelectedIndex==0)
            {
                if (str.Length != 0)
                {
                    using (SustemShopDB db = new SustemShopDB())
                    {
                        List<Tovar> list = db.Tovar.Where(p => p.TovarName.StartsWith(str)).ToList();
                        TovarTable.ItemsSource = null;
                        TovarTable.ItemsSource = list;
                    }
                }
                else
                {
                    UpdateTovar();
                }
            }
            else if(List.SelectedIndex == 1)
            {
                if (str.Length != 0)
                {
                    using (SustemShopDB db = new SustemShopDB())
                    {
                        List<Tovar> list = db.Tovar.Where(p => p.ToString().StartsWith(str)).ToList();
                        TovarTable.ItemsSource = null;
                        TovarTable.ItemsSource = list;
                    }
                }
                else
                {
                    UpdateTovar();
                }
            }
            else
            {
                if (str.Length != 0)
                {
                    using (SustemShopDB db = new SustemShopDB())
                    {
                        List<Tovar> list = db.Tovar.Where(p => p.ViewTovar.StartsWith(str)).ToList();
                        TovarTable.ItemsSource = null;
                        TovarTable.ItemsSource = list;
                    }
                }
                else
                {
                    UpdateTovar();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateTovar();
            filter.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataContext = new ClientViewModel(zero.Text);
        }

        private void id_tovar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SustemShopDB db = new SustemShopDB())
            {
                string TovarName = TovarID.SelectedItem.ToString();
                decimal TovarPrice = db.Tovar.Where(p => p.TovarName.Equals(TovarName)).FirstOrDefault().TovarPrice;
                TovarPrice.ToString();
            }
        }

        private void id_manager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SustemShopDB db = new SustemShopDB())
            {

            }
        }

        private void count_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TovarKol.Text.Length != 0)
                TovarKol.Text = (decimal.Parse(TovarKol.Text) * int.Parse(TovarKol.Text)).ToString();
            else TovarKol.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (SustemShopDB db = new SustemShopDB())
            {
               int id_mn = db.Managers.Where(p => p.fiomeneger.Equals(MenegerID.SelectedItem.ToString())).FirstOrDefault().menagerID;
               int id_tv = db.Tovar.Where(p => p.TovarName.Equals(TovarID.SelectedItem.ToString())).FirstOrDefault().TovarID;
                Suppliers suppliers = new Suppliers();
                suppliers.TovarID = id_tv;
                suppliers.MenegerID = id_mn;
                suppliers.Pricetovar = int.Parse(Pricetovar.Text);
                suppliers.Koltovar = int.Parse(Koltovar.Text);
                suppliers.data = Date.SelectedDate.Value;
                db.Suppliers.Add(suppliers);
                db.SaveChanges();
                UpdateSuppliers();
            }
        }

        private void SuppliersTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuppliersTab.SelectedItem!=null)
            {
                SuppliersModel suppliersModel = (SuppliersModel)SuppliersTab.SelectedItem;
                using (SustemShopDB db = new SustemShopDB())
                {
                    Suppliers suppliers = db.Suppliers.Where(p => p.ID == suppliersModel.ID).FirstOrDefault();
                    TovarID.Text = db.Tovar.Where(p => p.TovarID == suppliers.TovarID).FirstOrDefault().TovarName;
                    MenegerID.Text = db.Managers.Where(p => p.menagerID == suppliers.MenegerID).FirstOrDefault().fiomeneger;
                    TovarKol.Text = suppliers.Koltovar.ToString();
                    TovarPrice.Text = suppliers.Pricetovar.ToString();
                    Date.SelectedDate = suppliers.data;
                    suppliersModel = null;
                }
            }
          
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SuppliersModel suppliersModel = (SuppliersModel)SuppliersTab.SelectedItem;
            using (SustemShopDB db = new SustemShopDB())
            {
                Suppliers suppliers = db.Suppliers.Where(p => p.ID == suppliersModel.ID).FirstOrDefault();
                suppliers.Pricetovar = int.Parse(Pricetovar.Text);
                suppliers.ID = suppliersModel.ID;
                suppliers.Koltovar = int.Parse(Koltovar.Text);
                suppliers.data = Date.SelectedDate.Value;
                string name = TovarID.SelectedValue.ToString();
                string fio = FIOMeneger.SelectedText.ToString();
                suppliers.TovarID = db.Tovar.Where(p => p.TovarID.Equals(TovarID)).FirstOrDefault().TovarID;
                suppliers.MenegerID = db.Managers.Where(p => p.fiomeneger.Equals(fio)).FirstOrDefault().menagerID;
                db.Entry(suppliers).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            UpdateSuppliers();
            SuppliersTab.SelectedItem = null;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SuppliersModel suppliersModel = (SuppliersModel)SuppliersTab.SelectedItem;
            using (SustemShopDB db = new SustemShopDB())
            {
                Suppliers suppliers = db.Suppliers.Where(p => p.ID == suppliersModel.ID).FirstOrDefault();
                if (suppliers != null)
                {
                    db.Entry(suppliers).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            SuppliersTab.SelectedItem = null;
            UpdateSuppliers();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Filter filter = new Filter();
            if (filter.ShowDialog() == true)
            {
                List<SuppliersModel> suppliersModels = new List<SuppliersModel>();
                string query = filter.Querry;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var cells = row.ItemArray;
                            SuppliersModel suppliers = new SuppliersModel();
                            suppliers.ID = int.Parse(cells[0].ToString());
                            suppliers.data = DateTime.Parse(cells[1].ToString());
                            suppliers.Koltovar = int.Parse(cells[2].ToString());
                            suppliers.Pricetovar = int.Parse(cells[3].ToString());
                            using (SustemShopDB db = new SustemShopDB())
                            {
                                int id_t = int.Parse(cells[4].ToString());
                                suppliers.TovarID = db.Tovar.Where(p => p.TovarID ==1).FirstOrDefault().TovarID;
                                int id_m = int.Parse(cells[5].ToString());
                                suppliers.MenegerID = db.Managers.Where(p => p.menagerID == 1).FirstOrDefault().menagerID;
                            }
                            suppliersModels.Add(suppliers);
                        }
                    }
                    SuppliersTab.ItemsSource = null;
                    SuppliersTab.ItemsSource = suppliersModels;               
                }
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            UpdateSuppliers();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private async void Load()
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Text files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
                if (openFile.ShowDialog() == true)
                {
                    string path = openFile.FileName;
                    ISheet sheet;
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        stream.Position = 0;
                        XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                        sheet = xssWorkbook.GetSheetAt(0);
                        IRow headerRow = sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            Managers managers = new Managers();
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    if (j == 0) managers.fiomeneger = row.GetCell(j).ToString();
                                    if (j == 1) managers.viewtovarmanager = row.GetCell(j).ToString();
                                    if (j == 2) managers.phonemanager = row.GetCell(j).ToString();
                                }
                            }
                            using (SustemShopDB db = new SustemShopDB())
                            {
                                db.Managers.Add(managers);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
