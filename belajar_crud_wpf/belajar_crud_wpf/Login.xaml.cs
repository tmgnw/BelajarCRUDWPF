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
using System.Windows.Shapes;
using BelajarCRUDWPF.Model;
using BelajarCRUDWPF.MyContext;

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        myContext conn = new myContext();

        public Login()
        {
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cekemail = conn.Suppliers.Where(S => S.Email == txt_login_email.Text).FirstOrDefault();

                if (cekemail == null)
                {
                    MessageBox.Show("Email tidak terdaftar...");
                    txt_login_email.Focus();
                }
                else if (cekemail.Password != txt_login_password.Password.ToString())
                {
                    MessageBox.Show("Password Salah...");
                    txt_login_password.Focus();
                }
                else
                {
                    var mw = new MainWindow(txt_login_email.Text);
                    mw.Show();
                    this.Close();
                }
            }
            catch
            {
                //do nothing
            }
        }

        private void Btn_Forgot_Password_Click(object sender, RoutedEventArgs e)
        {
            var forgot = new ForgotPassword();
            forgot.Show();
            this.Close();
        }
    }
}