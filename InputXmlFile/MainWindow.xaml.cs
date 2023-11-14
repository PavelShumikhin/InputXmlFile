using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml.Serialization;

namespace InputXmlFile
{
    public partial class MainWindow : Window
    {
        //Строка подключения к БД
        private ApplicationDbContext dbContext;       
        public MainWindow()
        {
            InitializeComponent();
            dbContext = new ApplicationDbContext();
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
                    // Загрузка данных XML в список объектов вашего типа
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Card>));
                    using (FileStream file = new FileStream(xmlFilePath, FileMode.Open))
                    {
                        List<Card> cards = (List<Card>)formatter.Deserialize(file);
                        if (cards != null && cards.Count > 0)
                        {
                            MessageBox.Show("Данные успешно импортированы.");
                        }
                        dataGridView.DataContext = cards;
                    }
                    

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при импорте данных: " + ex.Message);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Добавление или обновление сущностей в базе данных
                foreach (Card client in dataGridView.ItemsSource)
                {
                    var existingClient = dbContext.Clients.Find(client.CARDCODE);

                    if (existingClient != null)
                    {
                        // Обновление существующего клиента
                        dbContext.Entry(existingClient).CurrentValues.SetValues(client);
                    }
                    else
                    {
                        // Добавление нового клиента
                        dbContext.Clients.Add(client);
                    }
                }

                dbContext.SaveChanges();

                MessageBox.Show("Изменения успешно сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
