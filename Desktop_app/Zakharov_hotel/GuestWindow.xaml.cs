using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Zakharov_hotel
{
    public partial class GuestWindow : Window
    {
        Database db = new Database();
        string guestName;

        public GuestWindow(string name)
        {
            InitializeComponent();
            guestName = name;
            lblHello.Text = $"Добро пожаловать, {name}!";

            dpStart.SelectedDate = DateTime.Today;
            dpEnd.SelectedDate = DateTime.Today.AddDays(1);

            LoadServices();
            LoadMyBookings();
            LoadFreeRooms();
        }

        // --- ЗАГРУЗКА ДАННЫХ ---
        void LoadServices()
        {
            string query = @"SELECT ""ServiceID"", ""ServiceName"" || ' - ' || ""Price"" || ' руб.' as displayinfo FROM ""Services""";

            DataTable dt = db.ExecuteQuery(query);
            lstServices.DisplayMemberPath = "displayinfo";
            lstServices.SelectedValuePath = "ServiceID"; 
            lstServices.ItemsSource = dt?.DefaultView;
        }

        void LoadMyBookings()
        {
            try
            { 
                string query = $@"SELECT * FROM guest_bookings_view WHERE fullname ILIKE '%{guestName.Trim()}%'";

                DataTable dt = db.ExecuteQuery(query);

                if (dt != null)
                {
                    dgMyBook.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void BtnFindFree_Click(object sender, RoutedEventArgs e) => LoadFreeRooms();

        void LoadFreeRooms()
        {
            try
            {
                dgRooms.ItemsSource = null;

                string query = @"SELECT ""RoomID"" as roomid, ""RoomName"" as roomname, ""Capacity"" as capacity, ""Price"" as price FROM ""Rooms""";

                // занятость(может не работать)

                string start = dpStart.SelectedDate.Value.ToString("yyyy-MM-dd");
                string end = dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
                query = $@"SELECT r.""RoomID"" as roomid, r.""RoomName"" as roomname, r.""Capacity"" as capacity, r.""Price"" as price 
                           FROM ""Rooms"" r 
                           WHERE NOT EXISTS (
                               SELECT 1 FROM ""Clients"" c 
                               WHERE c.""RoomID"" = r.""RoomID"" 
                               AND c.""CheckInDate"" < '{end}' 
                               AND c.""CheckOutDate"" > '{start}'
                           )";


                DataTable dt = db.ExecuteQuery(query);
                if (dt != null)
                {
                    dgRooms.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска номеров: " + ex.Message);
            }
        }

        // --- БРОНИРОВАНИЕ ---
        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem == null) { MessageBox.Show("Выберите номер!"); return; }
            if (dpStart.SelectedDate >= dpEnd.SelectedDate) { MessageBox.Show("Дата выезда должна быть позже заезда!"); return; }

            DataRowView roomRow = (DataRowView)dgRooms.SelectedItem;

            // Достаем ID и цену
            string roomId = roomRow["roomid"].ToString();
            decimal roomPrice = Convert.ToDecimal(roomRow["price"]);

            DateTime d1 = dpStart.SelectedDate.Value;
            DateTime d2 = dpEnd.SelectedDate.Value;
            int days = (d2 - d1).Days;

            string serv1 = "NULL", serv2 = "NULL", serv3 = "NULL";
            decimal servicesCost = 0;

            var selectedServices = lstServices.SelectedItems;
            if (selectedServices.Count > 3)
            {
                MessageBox.Show("Можно выбрать максимум 3 услуги!");
                return;
            }

            int count = 0;
            foreach (DataRowView item in selectedServices)
            {
                string sId = item["ServiceID"].ToString();

                // Получаем цену услуги
                DataTable dtPrice = db.ExecuteQuery($@"SELECT ""Price"" FROM ""Services"" WHERE ""ServiceID""={sId}");
                decimal sPrice = Convert.ToDecimal(dtPrice.Rows[0][0]);
                servicesCost += sPrice;

                if (count == 0) serv1 = sId;
                else if (count == 1) serv2 = sId;
                else if (count == 2) serv3 = sId;
                count++;
            }

            decimal totalCost = (roomPrice * days) + servicesCost;
            string totalCostStr = totalCost.ToString().Replace(',', '.');

            string query = $@"
                INSERT INTO ""Clients"" 
                (""FullName"", ""RoomID"", ""CheckInDate"", ""CheckOutDate"", ""Service1ID"", ""Service2ID"", ""Service3ID"", ""TotalCost"", ""EmployeeID"")
                VALUES 
                ('{guestName}', {roomId}, '{d1:yyyy-MM-dd}', '{d2:yyyy-MM-dd}', {serv1}, {serv2}, {serv3}, {totalCostStr}, NULL)";

            try
            {
                db.ExecuteUpdate(query);
                MessageBox.Show($"Успешно! Итоговая стоимость: {totalCost} руб.");
                LoadMyBookings();
                LoadFreeRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка бронирования: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (dgMyBook.SelectedItem is DataRowView row)
            {
                string id = row["clientid"].ToString();
                db.ExecuteUpdate($@"DELETE FROM ""Clients"" WHERE ""ClientID""={id}");
                LoadMyBookings();
                LoadFreeRooms();
            }
        }
    }
}