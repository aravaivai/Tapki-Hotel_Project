using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Zakharov_hotel
{
    public partial class AdminWindow : Window
    {
        Database db = new Database();

        public AdminWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private string GetVal(DataRowView row, string name)
        {
            string n = name.ToLower();
            if (row.Row.Table.Columns.Contains(n)) return row[n].ToString();
            if (row.Row.Table.Columns.Contains(name)) return row[name].ToString();
            return "";
        }

        void LoadData()
        {
            try
            { 
                dgStaff.ItemsSource = null;

                DataTable dtStaff = db.ExecuteQuery("SELECT * FROM staff_view");

                if (dtStaff != null)
                {
                    dgStaff.ItemsSource = dtStaff.DefaultView;
                }

                DataTable dtPos = db.ExecuteQuery("SELECT \"PositionID\" as pid, \"Title\" as tit FROM \"Positions\"");
                cmbPositions.SelectedValuePath = "pid";
                cmbPositions.DisplayMemberPath = "tit";
                cmbPositions.ItemsSource = dtPos?.DefaultView;

                dgPositions.ItemsSource = db.ExecuteQuery("SELECT * FROM \"Positions\" ORDER BY \"PositionID\"")?.DefaultView;

                DataTable dtStaffForRooms = db.ExecuteQuery("SELECT \"EmployeeID\" as employeeid, \"FullName\" as fullname FROM \"Employees\"");
                cmbRoomEmployee.ItemsSource = dtStaffForRooms?.DefaultView;
                dgRoomsAdmin.ItemsSource = db.ExecuteQuery("SELECT * FROM rooms_view")?.DefaultView;

                dgServicesAdmin.ItemsSource = db.ExecuteQuery("SELECT * FROM \"Services\"")?.DefaultView;

                dgAllClients.ItemsSource = db.ExecuteQuery("SELECT * FROM \"Clients\"")?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void DgStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStaff.SelectedItem is DataRowView row)
            {
                editName.Text = GetVal(row, "fullname");
                editAge.Text = GetVal(row, "age");
                editGender.Text = GetVal(row, "gender");
                editAddress.Text = GetVal(row, "address");
                editPhone.Text = GetVal(row, "phone");
                editPassport.Text = GetVal(row, "passportdata");
                editBonus.Text = GetVal(row, "bonus");

                object posId = row.Row.Table.Columns.Contains("positionid") ? row["positionid"] : row["PositionID"];
                cmbPositions.SelectedValue = posId;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dgStaff.SelectedItem is DataRowView row && cmbPositions.SelectedValue != null)
            {
                try
                {
                    string empId = GetVal(row, "employeeid");
                    string posId = cmbPositions.SelectedValue.ToString();
                    string bonus = editBonus.Text.Replace(',', '.');

                    string query = $@"UPDATE ""Employees"" SET 
                        ""FullName"" = '{editName.Text}', 
                        ""Age"" = {editAge.Text}, 
                        ""Gender"" = '{editGender.Text}', 
                        ""Address"" = '{editAddress.Text}', 
                        ""Phone"" = '{editPhone.Text}', 
                        ""PassportData"" = '{editPassport.Text}', 
                        ""PositionID"" = {posId}, 
                        ""Bonus"" = {bonus} 
                        WHERE ""EmployeeID"" = {empId}";

                    db.ExecuteUpdate(query);
                    LoadData();
                    MessageBox.Show("Сотрудник обновлен!");
                }
                catch (Exception ex) { MessageBox.Show("Ошибка сохранения: " + ex.Message); }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPositions.SelectedValue == null) { MessageBox.Show("Выберите должность!"); return; }
            try
            {
                string posId = cmbPositions.SelectedValue.ToString();
                string query = $@"INSERT INTO ""Employees"" (""FullName"", ""Age"", ""Gender"", ""Address"", ""Phone"", ""PassportData"", ""PositionID"", ""Bonus"") 
                    VALUES ('{editName.Text}', {editAge.Text}, '{editGender.Text}', '{editAddress.Text}', '{editPhone.Text}', '{editPassport.Text}', {posId}, 0)";
                
                db.ExecuteUpdate(query);
                LoadData();
                MessageBox.Show("Сотрудник добавлен!");
            }
            catch (Exception ex) { MessageBox.Show("Ошибка добавления: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgStaff.SelectedItem is DataRowView row)
            {
                string empId = GetVal(row, "employeeid");
                db.ExecuteUpdate($@"DELETE FROM ""Employees"" WHERE ""EmployeeID"" = {empId}");
                LoadData();
            }
        }

        // Логика номеров
        private void DgRoomsAdmin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRoomsAdmin.SelectedItem is DataRowView row)
            {
                try
                {
                    
                    if (row["EmployeeID"] != DBNull.Value && row["EmployeeID"] != null)
                    {
                        txtRoomName.Text = GetVal(row, "roomname");
                        txtRoomCap.Text = GetVal(row, "capacity");
                        txtRoomPrice.Text = GetVal(row, "price");
                        cmbRoomEmployee.SelectedValue = row["EmployeeID"];
                    }
                    else
                    {       
                        cmbRoomEmployee.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    cmbRoomEmployee.SelectedIndex = -1;
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void BtnAddRoom_Click(object sender, RoutedEventArgs e)
        {
           try
           {
                string empId = cmbRoomEmployee.SelectedValue != null ? cmbRoomEmployee.SelectedValue.ToString() : "NULL";
                string price = txtRoomPrice.Text.Replace(',', '.');
                db.ExecuteUpdate($@"INSERT INTO ""Rooms"" (""RoomName"", ""Capacity"", ""Price"", ""EmployeeID"") VALUES ('{txtRoomName.Text}', {txtRoomCap.Text}, {price}, {empId})");
                LoadData();
                MessageBox.Show("Номер добавлен!");
           }
           catch (Exception ex)
           {
                MessageBox.Show("Ошибка: " + ex.Message);
           }
        }

        private void BtnEditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgRoomsAdmin.SelectedItem is DataRowView row)
            {
                try
                {
                    string id = GetVal(row, "roomid");
                    string price = txtRoomPrice.Text.Replace(',', '.');

                    string empId = cmbRoomEmployee.SelectedValue != null ? cmbRoomEmployee.SelectedValue.ToString() : "NULL";

                    string query = $@"UPDATE ""Rooms"" SET 
                ""RoomName""='{txtRoomName.Text}', 
                ""Capacity""={txtRoomCap.Text}, 
                ""Price""={price}, 
                ""EmployeeID""={empId} 
                WHERE ""RoomID""={id}";

                    db.ExecuteUpdate(query);
                    LoadData();
                    MessageBox.Show("Данные номера обновлены!");
                }
                catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
            }
        }

        private void BtnDelRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgRoomsAdmin.SelectedItem is DataRowView row)
            {
                try {
                    db.ExecuteUpdate($@"DELETE FROM ""Rooms"" WHERE ""RoomID""={GetVal(row, "RoomID")}");
                    LoadData();
                } catch { MessageBox.Show("Ошибка удаления!"); }
            }
        }

        // Логика услуг
        private void DgServicesAdmin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgServicesAdmin.SelectedItem is DataRowView row)
            {
                txtServName.Text = GetVal(row, "ServiceName");
                txtServPrice.Text = GetVal(row, "Price");
            }
        }

        private void BtnAddServ_Click(object sender, RoutedEventArgs e)
        {
            string price = txtServPrice.Text.Replace(',', '.');
            db.ExecuteUpdate($@"INSERT INTO ""Services"" (""ServiceName"", ""Price"") VALUES ('{txtServName.Text}', {price})");
            LoadData();
        }

        private void BtnEditServ_Click(object sender, RoutedEventArgs e)
        {
            if (dgServicesAdmin.SelectedItem is DataRowView row)
            {
                string id = GetVal(row, "ServiceID");
                string price = txtServPrice.Text.Replace(',', '.');
                db.ExecuteUpdate($@"UPDATE ""Services"" SET ""ServiceName""='{txtServName.Text}', ""Price""={price} WHERE ""ServiceID""={id}");
                LoadData();
            }
        }

        private void BtnDelServ_Click(object sender, RoutedEventArgs e)
        {
            if (dgServicesAdmin.SelectedItem is DataRowView row)
            {
                db.ExecuteUpdate($@"DELETE FROM ""Services"" WHERE ""ServiceID""={GetVal(row, "ServiceID")}");
                LoadData();
            }
        }

        private void BtnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgAllClients.SelectedItem is DataRowView row)
            {
                db.ExecuteUpdate($@"DELETE FROM ""Clients"" WHERE ""ClientID"" = {GetVal(row, "clientid")}");
                LoadData();
            }
        }

        // Логика должностей

        private void dgPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPositions.SelectedItem is DataRowView row)
            {
                txtPosTitle.Text = row["Title"]?.ToString();
                txtPosSalary.Text = row["Salary"]?.ToString();
                txtPosDuties.Text = row["Duties"]?.ToString();
                txtPosRequirements.Text = row["Requirements"]?.ToString();
            }
        }

        // Добавление новой должности
        private void AddPosition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = $"INSERT INTO \"Positions\" (\"Title\", \"Salary\", \"Duties\", \"Requirements\") " +
             $"VALUES ('{txtPosTitle.Text}', {txtPosSalary.Text.Replace(",", ".")}, " +
             $"'{txtPosDuties.Text}', '{txtPosRequirements.Text}')";
                db.ExecuteQuery(sql);
                MessageBox.Show("Должность успешно добавлена!");
                LoadData(); 
            }
            catch (Exception ex) { MessageBox.Show("Ошибка при добавлении: " + ex.Message); }
        }

        // Сохранение изменений в существующей должности
        private void SavePosition_Click(object sender, RoutedEventArgs e)
        {
            if (dgPositions.SelectedItem is DataRowView row)
            {
                try
                {
                    string id = row["PositionID"].ToString();
                    string sql = $"UPDATE \"Positions\" SET " +
                                 $"\"Title\" = '{txtPosTitle.Text}', " +
                                 $"\"Salary\" = {txtPosSalary.Text.Replace(",", ".")}, " +
                                 $"\"Duties\" = '{txtPosDuties.Text}', " +
                                 $"\"Requirements\" = '{txtPosRequirements.Text}' " +
                                 $"WHERE \"PositionID\" = {id}";
                    db.ExecuteQuery(sql);
                    MessageBox.Show("Данные обновлены!");
                    LoadData();
                }
                catch (Exception ex) { MessageBox.Show("Ошибка при сохранении: " + ex.Message); }
            }
        }

        // Удаление должности
        private void DeletePosition_Click(object sender, RoutedEventArgs e)
        {
            if (dgPositions.SelectedItem is DataRowView row)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту должность? Это может повлиять на сотрудников!",
                                             "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        string id = row["PositionID"].ToString();
                        db.ExecuteQuery($"DELETE FROM \"Positions\" WHERE \"PositionID\" = {id}");
                        LoadData();
                    }
                    catch (Exception ex) { MessageBox.Show("Ошибка при удалении: " + ex.Message); }
                }
            }
        }
    }
}