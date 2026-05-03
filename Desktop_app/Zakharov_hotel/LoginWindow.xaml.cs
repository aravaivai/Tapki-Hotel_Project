using System.Data;
using System.Windows;

namespace Zakharov_hotel
{
    public partial class LoginWindow : Window
    {
        Database db = new Database();

        public LoginWindow() => InitializeComponent();

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string query = $"SELECT * FROM \"Users\" WHERE \"Login\"='{txtLogin.Text}' AND \"Password\"='{txtPass.Password}'";

            DataTable dt = db.ExecuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                string role = dt.Rows[0]["Role"].ToString();
                string name = dt.Rows[0]["FullName"].ToString();

                if (role == "Admin" || role == "Администратор")
                {
                    new AdminWindow().Show();
                }
                else
                {
                    new GuestWindow(name).Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Неверные данные!");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = $"INSERT INTO \"Users\" (\"Login\", \"Password\", \"FullName\", \"Role\") VALUES ('{txtRegLogin.Text}', '{txtRegPass.Password}', '{txtRegName.Text}', 'Гость')";

                db.ExecuteUpdate(query);
                MessageBox.Show("Успешно! Теперь войдите.");
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("duplicate key") || ex.Message.Contains("unique constraint"))
                    MessageBox.Show("Этот логин уже занят!");
                else
                    MessageBox.Show("Ошибка регистрации: " + ex.Message);
            }
        }
    }
}