using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using it = iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.OleDb;
using iTextSharp.text;

namespace WindowsFormsApplication1
{
    public partial class Corzina : Form
    {
        public int UserId;
        public int Plus_Product;
        public int IDDProduct;
        public int IDCategory;
        public string NameProduct;
        public int CountProduct;
        public int SumProduct;
        DataTable resultGrid = new DataTable();

        public Corzina(int UserId)
        {
                this.UserId = UserId;
                InitializeComponent();
                dataGridView1.CellClick += new DataGridViewCellEventHandler(cellClickEvent);
                loadData();
                loadData1();
        }

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\C#\Курсач СУБД\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\Shop.mdf;Integrated Security=True;Connect Timeout=30";
        int selectedGameId;

        private void cellClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Plus_Product = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString());

                IDDProduct = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString());
                IDCategory = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString());
                NameProduct = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
                CountProduct = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString());
                SumProduct = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[9].Value.ToString());

                this.selectedGameId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                MessageBox.Show("Выбран ID: " + selectedGameId.ToString() + "");
            }
            catch
            {
                MessageBox.Show("Выберите строку товара корзины", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadData(string searchString = "", string typeOfFilter = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID_Корзина, Наименование, Цена, Сумма, Корзина.Количество FROM Корзина, Продукты WHERE Корзина.ID_Продукт=Продукты.ID_Продукт AND ID_Пользователь = " + UserId + "";
            cmd.ExecuteNonQuery();
            DataTable dta = new DataTable();
            SqlDataAdapter dataadp = new SqlDataAdapter(cmd);
            dataadp.Fill(dta);
            dataGridView1.DataSource = dta;
            connection.Close();
        }

        private void loadData1(string searchString = "", string typeOfFilter = "")
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Корзина, Продукты WHERE Корзина.ID_Продукт=Продукты.ID_Продукт AND ID_Пользователь = " + UserId + "";
            cmd.ExecuteNonQuery();
            DataTable dta = new DataTable();
            SqlDataAdapter dataadp = new SqlDataAdapter(cmd);
            dataadp.Fill(dta);
            dataGridView2.DataSource = dta;
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name_Proc = "Delete_Корзина";
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                SqlCommand command = new SqlCommand(name_Proc, connect);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@ID_Корзина",
                        Value = this.selectedGameId
                    };
                    command.Parameters.Add(nameParam);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Продукт удален из корзины");
                    
                }
                catch
                {
                    MessageBox.Show("Ошибка удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            string NameProc = "Update_Продукт";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command1 = new SqlCommand(NameProc, connection);
                command1.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameParam00 = new SqlParameter
                {
                    ParameterName = "@ID_Продукт",
                    Value = IDDProduct
                };
                command1.Parameters.Add(nameParam00);
                SqlParameter nameParam01 = new SqlParameter
                {
                    ParameterName = "@ID_Категория",
                    Value = IDCategory
                };
                command1.Parameters.Add(nameParam01);
                SqlParameter nameParam02 = new SqlParameter
                {
                    ParameterName = "@Наименование",
                    Value = NameProduct
                };
                command1.Parameters.Add(nameParam02);
                SqlParameter nameParam03 = new SqlParameter
                {
                    ParameterName = "@Количество",
                    Value = (CountProduct + Plus_Product)
                };
                command1.Parameters.Add(nameParam03);
                SqlParameter nameParam04 = new SqlParameter
                {
                    ParameterName = "@Цена",
                    Value = SumProduct
                };
                command1.Parameters.Add(nameParam04);
                command1.ExecuteNonQuery();
            }
            loadData();
            loadData1();
            data_Loader();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resultGrid.Columns.Clear();
            resultGrid.Columns.AddRange(new DataColumn[] { new DataColumn("Наименование"), new DataColumn("Цена"), new DataColumn("Сумма"), new DataColumn("Количество") });
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "SELECT Наименование, Цена, Сумма, Корзина.Количество FROM Корзина, Продукты WHERE Корзина.ID_Продукт=Продукты.ID_Продукт AND ID_Пользователь = " + UserId + "";
            connection.Open();
            SqlDataReader reader = new SqlCommand(zapros, connection).ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    resultGrid.Rows.Add(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));
            connection.Close();
            if (resultGrid.Rows.Count == 0) return;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Файл формата PDF |*pdf";
            saveDialog.DefaultExt = ".pdf";
            saveDialog.Title = "Дайте название вашему файлу и выберите место сохранения";
            DialogResult result = saveDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            try
            {
                it.Document doc = new it.Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(saveDialog.FileName, FileMode.Create));
                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, false);
                it.Font font = new it.Font(baseFont, 14f, it.Font.NORMAL);
                doc.Open();
                PdfPTable table = new PdfPTable(new float[] { 100f, 100f, 100f, 100f });

                PdfPCell mainCell = new PdfPCell(new it.Phrase("Продукты: ", font)) { Border = 0, PaddingBottom = 5f, HorizontalAlignment = 0 };
                mainCell.Colspan = 4;
                table.AddCell(mainCell);

                PdfPCell headerCell = new PdfPCell(new it.Phrase(resultGrid.Columns[0].Caption, font)) { VerticalAlignment = 1, HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = it.BaseColor.LIGHT_GRAY };
                table.AddCell(headerCell);
                table.AddCell(new PdfPCell(new it.Phrase(resultGrid.Columns[1].Caption, font)) { VerticalAlignment = 1, HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = it.BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new it.Phrase(resultGrid.Columns[2].Caption, font)) { VerticalAlignment = 1, HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = it.BaseColor.LIGHT_GRAY });
                PdfPCell headerCell2 = new PdfPCell(new it.Phrase(resultGrid.Columns[3].Caption, font)) { VerticalAlignment = 1, HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = it.BaseColor.LIGHT_GRAY };
                table.AddCell(headerCell2);


                foreach (DataRow row in resultGrid.Rows)
                {
                    table.AddCell(new PdfPCell(new it.Phrase(row.ItemArray[0].ToString(), font)) { PaddingBottom = 5f, VerticalAlignment = 1, HorizontalAlignment = 1 });
                    table.AddCell(new PdfPCell(new it.Phrase(row.ItemArray[1].ToString(), font)) { PaddingBottom = 5f, VerticalAlignment = 1, HorizontalAlignment = 1 });
                    table.AddCell(new PdfPCell(new it.Phrase(row.ItemArray[1].ToString(), font)) { PaddingBottom = 5f, VerticalAlignment = 1, HorizontalAlignment = 1 });
                    table.AddCell(new PdfPCell(new it.Phrase(row.ItemArray[3].ToString(), font)) { PaddingBottom = 5f, VerticalAlignment = 1, HorizontalAlignment = 1 });
                }

                PdfPCell mainCell2 = new PdfPCell(new it.Phrase("Общая сумма: " + label2.Text, font)) { Border = 0, PaddingBottom = 5f, HorizontalAlignment = 0 };
                mainCell2.Colspan = 4;
                table.AddCell(mainCell2);

                doc.Add(table);
                doc.Close();
                writer.Close();

                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                string NameProc = "Insert_История";

                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {

                    try
                    {
                        connection1.Open();
                        SqlCommand command = new SqlCommand(NameProc, connection1);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter nameParam1 = new SqlParameter
                        {
                            ParameterName = "@ID_Пользователь",
                            Value = UserId
                        };
                        command.Parameters.Add(nameParam1);
                        SqlParameter nameParam2 = new SqlParameter
                        {
                            ParameterName = "@Дата_оформления",
                            Value = date.ToShortDateString()
                        };
                        command.Parameters.Add(nameParam2);
                        SqlParameter nameParam3 = new SqlParameter
                        {
                            ParameterName = "@Сумма_к_уплате",
                            Value = Convert.ToInt32(label2.Text)
                        };
                        command.Parameters.Add(nameParam3);
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка оформления заказа");
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Для начала закройте PDF файл!", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Corzina_Load(object sender, EventArgs e)
        {
            data_Loader();
        }

        public void data_Loader()
        {
            int sum = 0;
            for (int i = 0; i < dataGridView1.RowCount; ++i)
            {
                sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
            }
            label2.Text = sum.ToString();
        }
    }
}
