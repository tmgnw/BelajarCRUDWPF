using BelajarCRUDWPF.Model;
using BelajarCRUDWPF.MyContext;
using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using System.Net;
using System.Globalization;
using System.Net.Mail;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        myContext conn = new myContext(); // mendeklarasikan connection new myContext
        public int cb_sup;      //menampung supplier_id
        public int cb_role;     //menampung role_id
        public string email;

        public MainWindow(String tempmail)
        {
            InitializeComponent();
            tbl_supplier.ItemsSource = conn.Suppliers.ToList();             //untuk refresh table_supplier
            tbl_item.ItemsSource = conn.Items.ToList();                     //untuk refresh table_item
            combo_supplier.ItemsSource = conn.Suppliers.ToList();           //untuk refresh combobox_suppliers
            combo_role.ItemsSource = conn.Roles.ToList();                   //untuk refresh combobox_roles
            email = tempmail;       //menampung email
            roleAccess();       //hak akses
        }

        private void roleAccess()
        {
            var roleacc = conn.Suppliers.Where(S => S.Email == email).FirstOrDefault();
            if (roleacc.Role.Id == 2)
            {
                tab2.IsSelected = true; 
                tab1.IsEnabled = false;
                tab1.IsSelected = false;
            }
            else
            {
                tab1.IsEnabled = true;      //tab 1 milik supplier
                tab2.IsEnabled = true;      //tab 2 milik item
            }
        }

        // Insert Supplier 
        private void Btn_Insert_Click(object sender, RoutedEventArgs e)
        {
            string password = System.Guid.NewGuid().ToString();
            var iRole  = conn.Roles.Where(S => S.Id == cb_role).FirstOrDefault();
            var input = new Supplier(txtb_name.Text, txtb_address.Text, txtb_email.Text, password, iRole);
            
            // pattern email
            string pattern = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9[\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";

            // validasi_supplier
            if (txtb_name.Text == "")
            {
                MessageBox.Show("Nama tidak boleh kosong...");
                txtb_name.Focus();
            }
            else if (txtb_address.Text == "")
            {
                MessageBox.Show("Address tidak boleh kosong...");
                txtb_address.Focus();
            } 
            else if (txtb_email.Text == "")
            {
                MessageBox.Show("Email tidak boleh kosong...");
                txtb_email.Focus();

            } 
            else if (!Regex.Match(txtb_email.Text, pattern).Success)
            {
                MessageBox.Show("Format email salah...");
                txtb_email.Focus();
            }
            else
            {
                //push ke database
                conn.Suppliers.Add(input);
                conn.SaveChanges();
                sendPassToEmail(txtb_email.Text, password, txtb_name.Text); // method send password to email
                txtb_name.Text = string.Empty;
                txtb_address.Text = string.Empty;
                txtb_email.Text = string.Empty;
                MessageBox.Show("Data Berhasil ditambahkan");
            }
            tbl_supplier.ItemsSource = conn.Suppliers.ToList(); // refresh table
        }

        // Data Grid Supplier
        // Using Selection
        private void tbl_supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = tbl_supplier.SelectedItem;
            if (data == null) 
            {
                tbl_supplier.ItemsSource = conn.Suppliers.ToList();
            } 
            else
            {
                string id = (tbl_supplier.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                txtb_id.Text = id;
                string name = (tbl_supplier.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                txtb_name.Text = name;
                string address = (tbl_supplier.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
                txtb_address.Text = address;
                string email = (tbl_supplier.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
                txtb_email.Text = email;
            }

        }

        // Update Supplier
        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            int inId = Convert.ToInt32(txtb_id.Text);                //mengambil id dari textbox id
            var cekId = conn.Suppliers.Where(S => S.Id == inId).FirstOrDefault();         //s -> inisialisasi objek dari tbl_supplier
            cekId.Name = txtb_name.Text;            
            cekId.Address = txtb_address.Text;
            var update = conn.SaveChanges();
            MessageBox.Show(update + " Data berhasil di update");
            tbl_supplier.ItemsSource = conn.Suppliers.ToList();     //refresh table supplier
        }

        // Delete supplier
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            int inId = Convert.ToInt32(txtb_id.Text);               //mengambil id dari textbox id
            var cekId = conn.Suppliers.Where(S => S.Id == inId).FirstOrDefault();           //s -> inisialisasi objek dari tbl_supplier
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Anda yakin ingin menghapus data?", "Konfirmasi Hapus", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                conn.Suppliers.Remove(cekId);
                var delete = conn.SaveChanges();
                txtb_id.Text = string.Empty;
                txtb_name.Text = string.Empty;
                txtb_address.Text = string.Empty;
                tbl_supplier.ItemsSource = conn.Suppliers.ToList();
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (txtb_id.Text == "")
            {
                string password = System.Guid.NewGuid().ToString();
                var iRole = conn.Roles.Where(S => S.Id == cb_role).FirstOrDefault();
                var input = new Supplier(txtb_name.Text, txtb_address.Text, txtb_email.Text, password, iRole);

                // pattern email
                string pattern = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9[\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";

                // validasi_supplier
                if (txtb_name.Text == "")
                {
                    MessageBox.Show("Nama tidak boleh kosong...");
                    txtb_name.Focus();
                }
                else if (txtb_address.Text == "")
                {
                    MessageBox.Show("Address tidak boleh kosong...");
                    txtb_address.Focus();
                }
                else if (txtb_email.Text == "")
                {
                    MessageBox.Show("Email tidak boleh kosong...");
                    txtb_email.Focus();

                }
                else if (!Regex.Match(txtb_email.Text, pattern).Success)
                {
                    MessageBox.Show("Format email salah...");
                    txtb_email.Focus();
                }
                else
                {
                    //push ke database
                    conn.Suppliers.Add(input);
                    conn.SaveChanges();
                    sendPassToEmail(txtb_email.Text, password, txtb_name.Text); // method send password to email
                    txtb_name.Text = string.Empty;
                    txtb_address.Text = string.Empty;
                    txtb_email.Text = string.Empty;
                    MessageBox.Show("Data Berhasil ditambahkan");
                }
                tbl_supplier.ItemsSource = conn.Suppliers.ToList(); // refresh table
            }
            else
            {
                int inId = Convert.ToInt32(txtb_id.Text);                //mengambil id dari textbox id
                var cekId = conn.Suppliers.Where(S => S.Id == inId).FirstOrDefault();         //s -> inisialisasi objek dari tbl_supplier
                cekId.Name = txtb_name.Text;
                cekId.Address = txtb_address.Text;
                var update = conn.SaveChanges();
                MessageBox.Show(update + " Data berhasil di update");
                tbl_supplier.ItemsSource = conn.Suppliers.ToList();     //refresh table supplier
            }
        }

        // Set up combo supplier
        private void combo_supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_sup = Convert.ToInt32(combo_supplier.SelectedValue);         //select value combo supplier input di item menu
        }

        // Insert Item
        private void btn_item_insert_Click(object sender, RoutedEventArgs e)
        {
            string item_pattern = "[^a-zA-Z0-9]";
            try
            {  
                // validasi
                if (txtb_item_name.Text == "")
                {
                    MessageBox.Show("Nama item tidak boleh kosong...");
                    txtb_item_name.Focus();
                }
                else if (Regex.IsMatch(txtb_item_name.Text, item_pattern))
                {
                    MessageBox.Show("Format nama item salah");
                    txtb_item_name.Focus();
                }
                else if (txtb_item_price.Text == "")
                {
                    MessageBox.Show("Price tidak boleh kosong...");
                    txtb_item_price.Focus();
                }
                else if (txtb_item_stock.Text == "")
                {
                    MessageBox.Show("Stock tidak boleh kosong...");
                    txtb_item_stock.Focus();
                }
                else if (combo_supplier.Text == "")
                {
                    MessageBox.Show("Supplier tidak boleh kosong...");
                    combo_supplier.Focus();
                }
                else
                {
                    var inPrice = Convert.ToInt32(txtb_item_price.Text);
                    var inStock = Convert.ToInt32(txtb_item_stock.Text);
                    var inSupp = conn.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault();
                    var inputItem = new Item(txtb_item_name.Text, inPrice, inStock, inSupp);
                    conn.Items.Add(inputItem);
                    var insert = conn.SaveChanges();
                    MessageBox.Show(insert + "Data telah ditambahkan...");
                    txtb_item_name.Text = string.Empty;
                    txtb_item_price.Text = string.Empty;
                    txtb_item_stock.Text = string.Empty;
                    combo_supplier.Text = string.Empty;
                }
                tbl_item.ItemsSource = conn.Items.ToList();     //refresh table_item
            }
            catch
            {
                //do nothing
            }
        }

        // Select datagrid table item
        private void tbl_item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = tbl_item.SelectedItem;

            if (data == null)
            {
                tbl_item.ItemsSource = conn.Items.ToList();
            }
            else
            {
                string id = (tbl_item.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                txtb_item_id.Text = id;
                string name = (tbl_item.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                txtb_item_name.Text = name;
                string price = (tbl_item.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
                txtb_item_price.Text = price;
                string stock = (tbl_item.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
                txtb_item_stock.Text = stock;
                string supplier = (tbl_item.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
                combo_supplier.Text = supplier;
            }
        }

        // Update Item
        private void btn_item_update_Click(object sender, RoutedEventArgs e)
        {
            int inputId = Convert.ToInt32(txtb_item_id.Text);
            var cekId = conn.Items.Where(S => S.Id == inputId).FirstOrDefault();
            var inPrice = Convert.ToInt32(txtb_item_price.Text);
            var inStock = Convert.ToInt32(txtb_item_stock.Text);
            var inSupp = conn.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault();
            cekId.Name = txtb_item_name.Text;
            cekId.Price = inPrice;
            cekId.Stock = inStock;
            cekId.Supplier = inSupp;
            var update = conn.SaveChanges();
            MessageBox.Show(update + " Data berhasil di update");
            txtb_item_id.Text = string.Empty;
            txtb_item_name.Text = string.Empty;
            txtb_item_price.Text = string.Empty;
            txtb_item_stock.Text = string.Empty;
            combo_supplier.Text = string.Empty;
            tbl_item.ItemsSource = conn.Items.ToList();
        }

        // Delete Item
        private void btn_item_delete_Click(object sender, RoutedEventArgs e)
        {
            int inId = Convert.ToInt32(txtb_item_id.Text); // menangkap id dari textbox id
            var cekId = conn.Items.Where(S => S.Id == inId).FirstOrDefault(); // s -> objek dari tbl_supplier
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Anda yakin ingin menghapus data?", "Konfirmasi Hapus", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                conn.Items.Remove(cekId);
                var delete = conn.SaveChanges();
                txtb_item_id.Text = string.Empty;
                txtb_item_name.Text = string.Empty;
                txtb_item_price.Text = string.Empty;
                txtb_item_stock.Text = string.Empty;
                combo_supplier.Text = string.Empty;
                tbl_item.ItemsSource = conn.Items.ToList();
            }
        }

        private void btn_saveItem_Click(object sender, RoutedEventArgs e)
        {
            if (txtb_item_id.Text == "")
            {
                string item_pattern = "[^a-zA-Z0-9]";
                try
                {
                    // validasi
                    if (txtb_item_name.Text == "")
                    {
                        MessageBox.Show("Nama item tidak boleh kosong...");
                        txtb_item_name.Focus();
                    }
                    else if (Regex.IsMatch(txtb_item_name.Text, item_pattern))
                    {
                        MessageBox.Show("Format nama item salah");
                        txtb_item_name.Focus();
                    }
                    else if (txtb_item_price.Text == "")
                    {
                        MessageBox.Show("Price tidak boleh kosong...");
                        txtb_item_price.Focus();
                    }
                    else if (txtb_item_stock.Text == "")
                    {
                        MessageBox.Show("Stock tidak boleh kosong...");
                        txtb_item_stock.Focus();
                    }
                    else if (combo_supplier.Text == "")
                    {
                        MessageBox.Show("Supplier tidak boleh kosong...");
                        combo_supplier.Focus();
                    }
                    else
                    {
                        var inPrice = Convert.ToInt32(txtb_item_price.Text);
                        var inStock = Convert.ToInt32(txtb_item_stock.Text);
                        var inSupp = conn.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault();
                        var inputItem = new Item(txtb_item_name.Text, inPrice, inStock, inSupp);
                        conn.Items.Add(inputItem);
                        var insert = conn.SaveChanges();
                        MessageBox.Show(insert + "Data telah ditambahkan...");
                        txtb_item_name.Text = string.Empty;
                        txtb_item_price.Text = string.Empty;
                        txtb_item_stock.Text = string.Empty;
                        combo_supplier.Text = string.Empty;
                    }
                    tbl_item.ItemsSource = conn.Items.ToList();     //refresh table_item
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                int inputId = Convert.ToInt32(txtb_item_id.Text);
                var cekId = conn.Items.Where(S => S.Id == inputId).FirstOrDefault();
                var inPrice = Convert.ToInt32(txtb_item_price.Text);
                var inStock = Convert.ToInt32(txtb_item_stock.Text);
                var inSupp = conn.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault();
                cekId.Name = txtb_item_name.Text;
                cekId.Price = inPrice;
                cekId.Stock = inStock;
                cekId.Supplier = inSupp;
                var update = conn.SaveChanges();
                MessageBox.Show(update + " Data berhasil di update");
                txtb_item_id.Text = string.Empty;
                txtb_item_name.Text = string.Empty;
                txtb_item_price.Text = string.Empty;
                txtb_item_stock.Text = string.Empty;
                combo_supplier.Text = string.Empty;
                tbl_item.ItemsSource = conn.Items.ToList();
            }
        }

        // Function send password to email
        private void sendPassToEmail(string email, string password, string name)
        {
            string from = "thomasgunawan13@gmail.com";
            string to = email;
            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
            MailMessage mm = new MailMessage(from, to);
            mm.Subject = "Password anda untuk Login " + currentdate + " ";
            string teks = "halo " + name + " ini password anda " + password + " Terimakasih.";
            mm.Body = teks;
            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("thomasgunawan13@gmail.com", "Tgunawan1797");
            smtp.EnableSsl = true;
           
            try
            {
                smtp.Send(mm);
            }
            catch(Exception e)
            {
                MessageBox.Show("Failed to send email..." + e.ToString());
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }
        private void txt_id_PreviewTextInput(object sender, TextCompositionEventArgs e) { }
        private void txt_name_PreviewTextInput(object sender, TextCompositionEventArgs e) { }
        private void txt_address_PreviewTextInput(object sender, TextCompositionEventArgs e) { }
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e) { }
        private void txtb_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void combo_role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_role = Convert.ToInt32(combo_role.SelectedValue); // handle load role input in supplier menu
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            txtb_id.Text = string.Empty;
            txtb_name.Text = string.Empty;
            txtb_address.Text = string.Empty;
            txtb_email.Text = string.Empty;
            combo_role.Text = string.Empty;
        }

        private void btn_refreshitem_Click(object sender, RoutedEventArgs e)
        {
            txtb_item_id.Text = string.Empty;
            txtb_item_name.Text = string.Empty;
            txtb_item_price.Text = string.Empty;
            txtb_item_stock.Text = string.Empty;
            combo_supplier.Text = string.Empty;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            List<Supplier> filSup = new List<Supplier>();
            int parseValue;
            if (txtb_search.Text == "")
            {
                tbl_supplier.ItemsSource = conn.Suppliers.ToList();
            }
            else
            { 
                foreach(Supplier s in conn.Suppliers.ToList())
                {
                    if (s.Name.ToLower().Contains(txtb_search.Text.ToLower()))
                    {
                        filSup.Add(s);
                    }
                    else if (int.TryParse(txtb_search.Text, out parseValue))
                    {
                        if (s.Id.Equals(Convert.ToInt32(txtb_search.Text.ToLower()))) 
                        {
                            filSup.Add(s);
                        }
                    }
                    else if (s.Address.ToLower().Contains(txtb_search.Text.ToLower()))
                    {
                        filSup.Add(s);
                    }
                    else if (s.Email.ToLower().Contains(txtb_search.Text.ToLower()))
                    {
                        filSup.Add(s);
                    }
                }
                tbl_supplier.ItemsSource = filSup.ToList();
            }
        }
        

        private void btn_searchitem_Click(object sender, RoutedEventArgs e)
        {
            List<Item> filItm = new List<Item>();
            int parseValue;
            if (txtb_searchItem.Text == "")
            {
                tbl_item.ItemsSource = conn.Items.ToList();
            }
            else
            {
                foreach (Item itm in conn.Items.ToList())
                {
                    if (itm.Name.ToLower().Contains(txtb_searchItem.Text.ToLower()))
                    {
                        filItm.Add(itm);
                    }
                    else if (int.TryParse(txtb_searchItem.Text, out parseValue))
                    {
                        if (itm.Id.Equals(Convert.ToInt32(txtb_searchItem.Text.ToLower())))
                        {
                            filItm.Add(itm);
                        }
                        else if (itm.Price.Equals(Convert.ToInt32(txtb_searchItem.Text.ToLower())))
                        {
                            filItm.Add(itm);
                        }
                        else if (itm.Stock.Equals(Convert.ToInt32(txtb_searchItem.Text.ToLower())))
                        {
                            filItm.Add(itm);
                        }
                    }
                }
                tbl_item.ItemsSource = filItm.ToList();
            }
        }
    }
}