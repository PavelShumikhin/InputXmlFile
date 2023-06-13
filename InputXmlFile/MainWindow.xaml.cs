using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InputXmlFile
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Строка подключения к БД
        private string connectionString = "Data Source=Shuma\\MSSQLSERVER02;Initial Catalog=Test;Integrated Security=True";
        private DataTable data;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Title = "Выберите XML файл для импорта";

            if(openFileDialog.ShowDialog() == true)
            {
                string xmlFilePath = openFileDialog.FileName;
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        // Таблицу для хранения импортированных данных
                        data = new DataTable();
                        data.Columns.Add("CARDCODE", typeof(string));
                        data.Columns.Add("STARTDATE", typeof(DateTime));
                        data.Columns.Add("FINISHDATE", typeof(DateTime));
                        data.Columns.Add("LASTNAME", typeof(string));
                        data.Columns.Add("FIRSTNAME", typeof(string));
                        data.Columns.Add("SURNAME", typeof(string));
                        data.Columns.Add("GENDERID", typeof(string));
                        data.Columns.Add("BIRTHDAY", typeof(DateTime));
                        data.Columns.Add("PHONEHOME", typeof(string));
                        data.Columns.Add("PHONEMOBIL", typeof(string));
                        data.Columns.Add("EMAIL", typeof(string));
                        data.Columns.Add("CITY", typeof(string));
                        data.Columns.Add("STREET", typeof(string));
                        data.Columns.Add("HOUSE", typeof(string));
                        data.Columns.Add("APARTMENT", typeof(string));

                        // Загружаем XML файл в DataSet
                        DataSet dataSet = new DataSet();
                        dataSet.ReadXml(xmlFilePath);

                        // Получаем таблицу с данными из DataSet
                        DataTable xmlData = dataSet.Tables[0];

                        // Копируем данные из таблицы XML в таблицу данных
                        foreach (DataRow row in xmlData.Rows)
                        {
                            DataRow newRow = data.NewRow();
                            newRow["CARDCODE"] = row["CARDCODE"];                           
                            string startDateString = row["STARTDATE"].ToString();
                            DateTime startDate;
                            // Проверяем пустое значение для "STARTDATE"
                            if (string.IsNullOrEmpty(startDateString) || !DateTime.TryParse(startDateString, out startDate))
                            {
                                newRow["STARTDATE"] = DBNull.Value; 
                            }
                            else
                            {
                                newRow["STARTDATE"] = startDate;
                            }
                            string finishDateString = row["FINISHDATE"].ToString();
                            DateTime finishDate;
                            // Проверяем пустое значение для "FINISHDATE"
                            if (string.IsNullOrEmpty(finishDateString) || !DateTime.TryParse(finishDateString, out finishDate))
                            {
                                newRow["FINISHDATE"] = DBNull.Value;
                            }
                            else
                            {
                                newRow["FINISHDATE"] = finishDate;
                            }

                            newRow["LASTNAME"] = row["LASTNAME"];
                            newRow["FIRSTNAME"] = row["FIRSTNAME"];
                            newRow["SURNAME"] = row["SURNAME"];
                            newRow["GENDERID"] = row["GENDERID"];
                            string birthdayString = row["BIRTHDAY"].ToString();
                            DateTime birthday;
                            // Проверяем пустое значение для "BIRTHDAY"
                            if (string.IsNullOrEmpty(birthdayString) || !DateTime.TryParse(birthdayString, out birthday))
                            {
                                newRow["BIRTHDAY"] = DBNull.Value;
                            }
                            else
                            {
                                newRow["BIRTHDAY"] = birthday;
                            }
                            newRow["PHONEHOME"] = row["PHONEHOME"];
                            newRow["PHONEMOBIL"] = row["PHONEMOBIL"];
                            newRow["EMAIL"] = row["EMAIL"];
                            newRow["CITY"] = row["CITY"];
                            newRow["STREET"] = row["STREET"];
                            newRow["HOUSE"] = row["HOUSE"];
                            newRow["APARTMENT"] = row["APARTMENT"];    

                            data.Rows.Add(newRow);
                        }

                        // Отображаем данные в DataGrid
                        dataGridView.ItemsSource = data.DefaultView;

                        MessageBox.Show("Данные успешно импортированы в базу данных.");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка импорта данных: " + ex.Message);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание временной таблицы
                    string tempTableName = "#TempClients";
                    string createTempTableQuery = @"
                CREATE TABLE " + tempTableName + @" (
                    CARDCODE VARCHAR(50),
                    STARTDATE DATE,
                    FINISHDATE DATE,
                    LASTNAME NVARCHAR(50),
                    FIRSTNAME NVARCHAR(50),
                    SURNAME NVARCHAR(50),
                    GENDERID NVARCHAR(1),
                    BIRTHDAY DATE,
                    PHONEHOME NVARCHAR(50),
                    PHONEMOBIL NVARCHAR(50),
                    EMAIL NVARCHAR(50),
                    CITY NVARCHAR(50),
                    STREET NVARCHAR(50),
                    HOUSE NVARCHAR(10),
                    APARTMENT NVARCHAR(10)
                )";
                    SqlCommand createTempTableCommand = new SqlCommand(createTempTableQuery, connection);
                    createTempTableCommand.ExecuteNonQuery();

                    // Загрузка данных из файла XML во временную таблицу
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = tempTableName;
                        bulkCopy.WriteToServer(data);
                    }

                    // Обновление и вставка данных в основную таблицу с использованием оператора MERGE
                    string mergeQuery = @"
                MERGE INTO Clients AS target
                USING " + tempTableName + @" AS source
                ON (target.CARDCODE = source.CARDCODE)
                WHEN MATCHED THEN
                    UPDATE SET
                        target.STARTDATE = source.STARTDATE,
                        target.FINISHDATE = source.FINISHDATE,
                        target.LASTNAME = source.LASTNAME,
                        target.FIRSTNAME = source.FIRSTNAME,
                        target.SURNAME = source.SURNAME,
                        target.GENDERID = source.GENDERID,
                        target.BIRTHDAY = source.BIRTHDAY,
                        target.PHONEHOME = source.PHONEHOME,
                        target.PHONEMOBIL = source.PHONEMOBIL,
                        target.EMAIL = source.EMAIL,
                        target.CITY = source.CITY,
                        target.STREET = source.STREET,
                        target.HOUSE = source.HOUSE,
                        target.APARTMENT = source.APARTMENT
                WHEN NOT MATCHED BY TARGET THEN
                    INSERT (CARDCODE, STARTDATE, FINISHDATE, LASTNAME, FIRSTNAME, SURNAME, GENDERID, BIRTHDAY, PHONEHOME, PHONEMOBIL, EMAIL, CITY, STREET, HOUSE, APARTMENT)
                    VALUES (source.CARDCODE, source.STARTDATE, source.FINISHDATE, source.LASTNAME, source.FIRSTNAME, source.SURNAME, source.GENDERID, source.BIRTHDAY, source.PHONEHOME, source.PHONEMOBIL, source.EMAIL, source.CITY, source.STREET, source.HOUSE, source.APARTMENT)
                WHEN NOT MATCHED BY SOURCE THEN
                    DELETE;";
                    SqlCommand mergeCommand = new SqlCommand(mergeQuery, connection);
                    mergeCommand.ExecuteNonQuery();

                    // Удаление временной таблицы
                    string dropTempTableQuery = "DROP TABLE " + tempTableName;
                    SqlCommand dropTempTableCommand = new SqlCommand(dropTempTableQuery, connection);
                    dropTempTableCommand.ExecuteNonQuery();

                    MessageBox.Show("Изменения сохранены успешно!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }

        

    }
}
