using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel.DataAnnotations;
using arduino.Models;

namespace arduino
{
    public partial class Form1 : Form
    {
        string sonuc;
        long maksm = 30, minm = 0, i = 0;
        ApplicationConnectionDb db= new ApplicationConnectionDb();
        public Form1()
        {
            InitializeComponent();

            // Seri port ayarlarını yapılandırma
            serialPort1.PortName = "COM3";  // Arduino'nun bağlı olduğu COM portu
            serialPort1.BaudRate = 9600;     // Arduino'nun baud hızı
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Bağlantıyı kapat
            serialPort1.Close();

            // Zamanlayıcıyı durdur
            timer1.Stop();

            // Bağlanma butonunu etkinleştir
            button1.Enabled = true;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Grafik üzerine tıklama olayı (Gerekirse bu metot içine kod ekleyebilirsiniz)
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Etiket üzerine tıklama olayı (Gerekirse bu metot içine kod ekleyebilirsiniz)
        }

        private void lblWarning_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxYellow_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxRed_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        private void LoadDataIntoDataGridView()
        {
            // Veritabanından verileri yükle ve DataGridView'a bağla
            dataGridView1.DataSource = db.Personel.ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Bağlantıyı aç
            serialPort1.Open();

            // Zamanlayıcıyı başlat
            timer1.Start();

            // Bağlanma butonunu devre dışı bırak
            button1.Enabled = false;
            LoadDataIntoDataGridView();
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Seri port üzerinden veri gönder
            serialPort1.Write("1");

            // Seri porttan gelen veriyi oku
            sonuc = serialPort1.ReadLine();

            if (sonuc != null)
            {
                // Etiket üzerine alınan veriyi yaz
                label1.Text = sonuc + "";

                // Grafik üzerine alınan veriyi ekle
                this.chart1.Series[0].Points.AddXY(i, sonuc);

                // Check the sensor value for warning
                int sensorValue = int.Parse(sonuc);
                InsertData(DateTime.Now, sensorValue);

                // Append data to CSV file
                string csvPath = "C:\\Users\\deney\\OneDrive\\Belgeler\\gazsensoru.cvs"; // Replace "your_path" with the actual path
                string currentTimeStamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm\t");
                
                using (StreamWriter sw = new StreamWriter(csvPath, true))
                {
                    sw.WriteLine($"{currentTimeStamp};{sensorValue}");
                }

                // Update the UI based on sensor value
                if (sensorValue > 400)
                {
                    // Display warning message
                    lblWarning.Text = "ORTAMDA ÇOK FAZLA GAZ VAR     \n               DİKKATLİ OLUN!..";

                    // Show red PictureBox and hide yellow PictureBox
                    pictureBoxRed.Visible = true;
                    pictureBoxYellow.Visible = false;
                }
                else
                {
                    // Hide warning message
                    lblWarning.Text = "\nSAĞLIKLI GAZ ORTAMI ARALIĞI :)\n ";

                    // Show yellow PictureBox and hide red PictureBox
                    pictureBoxRed.Visible = false;
                    pictureBoxYellow.Visible = true;
                }
            }

            // Gelen veriyi seri portun veri alınan tamponundan temizle
            serialPort1.DiscardInBuffer();
        }
        private void InsertData(DateTime Tarih, int gazOrani)
        {
            try
            {
                using (var db = new ApplicationConnectionDb())
                {
                    // Veritabanına yeni bir Personel nesnesi ekleyerek kaydetme işlemi
                    Personel yeniKayit = new Personel
                    {
                        Tarih = Tarih,
                        gazOrani = gazOrani
                    };

                    db.Personel.Add(yeniKayit);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda bir şeyler yapabilirsiniz
                Console.WriteLine(ex.Message);
            }
        }
    }
}
