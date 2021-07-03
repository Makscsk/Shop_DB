using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using it=iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data.OleDb;
using iTextSharp.text;

namespace WindowsFormsApplication1
{
    public partial class Magazin : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\C#\Курсач СУБД\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\Shop.mdf;Integrated Security=True;Connect Timeout=30";
        int selectedGameId;
        public int UserId;
        public string UserStr;
        public int addCena;
        public int addProduct;
        public int addCount;
        TextBox login, password;
        DataSet dataSet = new DataSet();
        DataTable resultGrid = new DataTable();
        public Magazin(int UserId, string UserStr,TextBox textBox1, TextBox textBox2)
        {
            InitializeComponent();
            dataGridView1.CellClick += new DataGridViewCellEventHandler(cellClickEvent);
            this.UserId = UserId;
            this.UserStr = UserStr;
            textBox1_TextChanged_1(null, null);
            login = textBox1;
            password = textBox2;

        }
        public Magazin(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;
        }

        private void cellClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                addCena = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                addProduct = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                addCount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                this.selectedGameId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                MessageBox.Show("Выбран ID: " + selectedGameId.ToString() + "");
            }
            catch
            {
                MessageBox.Show("Выберите строку товара", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Magazin_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (login.Text != "admin" && password.Text != "admin")
                button4.Visible = button5.Visible = button6.Visible = button7.Visible = false;
            else
                button4.Visible = button5.Visible = button6.Visible = button7.Visible = true;
            label9.Text ="Ваш ID #" + UserStr;
           
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Corzina fff = new Corzina(UserId);
            fff.ShowDialog();
            loadData(textBox1.Text);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string sql;
            string filtersQuery = "";
            if (comboBox1.SelectedItem.ToString() != "")
            {
                if (comboBox1.SelectedItem.ToString() == "По возрастанию")
                {
                    filtersQuery = "ORDER BY Продукты.Наименование ASC";
                }
                if (comboBox1.SelectedItem.ToString() == "По убыванию")
                {
                    filtersQuery = "ORDER BY Продукты.Наименование DESC";
                }
            }
            sql = "SELECT ID_Продукт, Категории.Наименование AS Категория, Продукты.Наименование, Количество, Цена FROM Продукты, Категории WHERE Категории.ID_Категория=Продукты.ID_Категория " + filtersQuery;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "ID_Продукт");
            connection.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "ID_Продукт";
        }

        public void display_data()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID_Продукт, Категории.Наименование AS Категория, Продукты.Наименование, Количество, Цена FROM Продукты, Категории WHERE Категории.ID_Категория=Продукты.ID_Категория";
            cmd.ExecuteNonQuery();
            DataTable dta = new DataTable();
            SqlDataAdapter dataadp = new SqlDataAdapter(cmd);
            dataadp.Fill(dta);
            dataGridView1.DataSource = dta;
            connection.Close();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {   
            loadData(textBox1.Text);
        }
        private void loadData(string searchString = "")
        {
            string sql;
            string searchQuery = "";
            if (searchString != "")
            {
                searchQuery = "AND (Продукты.Наименование Like N'%" + searchString + "%')";
            }
            sql = "SELECT ID_Продукт, Категории.Наименование AS Категория, Продукты.Наименование, Количество, Цена FROM Продукты, Категории WHERE Категории.ID_Категория=Продукты.ID_Категория " + searchQuery;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "ID_Продукт"); 
            connection.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "ID_Продукт";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT ID_Продукт, Категории.Наименование AS Категория, Продукты.Наименование, Количество, Цена FROM Продукты, Категории WHERE Категории.ID_Категория=Продукты.ID_Категория AND Цена BETWEEN " + lowerBound.Value.ToString() + " AND " + upperBound.Value.ToString() + "";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "ID_Продукт");
            connection.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "ID_Продукт";
        }

        private void label10_Click(object sender, EventArgs e)
        {
           
        }
     
        private void Magazin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int countProduct = Convert.ToInt32(textBox2.Text.Trim());
                int ccc = countProduct;
                if (ccc > addCount || ccc <= 0)
                {
                    MessageBox.Show("Указанное количество товара превышает, его количество на складе.");
                }
                else
                {
                    if (addProduct != 0)
                    {
                        if (addCount != 0)
                        {
                            string name_Proc = "Insert_Корзина";
                            using (SqlConnection connect = new SqlConnection(connectionString))
                            {
                                connect.Open();
                                SqlCommand command = new SqlCommand(name_Proc, connect);
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                try
                                {
                                    SqlParameter nameParam = new SqlParameter
                                    {
                                        ParameterName = "@ID_Пользователь",
                                        Value = this.UserId
                                    };
                                    command.Parameters.Add(nameParam);
                                    SqlParameter nameParam2 = new SqlParameter
                                    {
                                        ParameterName = "@ID_Продукт",
                                        Value = addProduct
                                    };
                                    command.Parameters.Add(nameParam2);
                                    SqlParameter nameParam3 = new SqlParameter
                                    {
                                        ParameterName = "@Сумма",
                                        Value = (addCena * countProduct)
                                    };
                                    command.Parameters.Add(nameParam3);
                                    SqlParameter nameParam4 = new SqlParameter
                                    {
                                        ParameterName = "@Количество",
                                        Value = countProduct
                                    };
                                    command.Parameters.Add(nameParam4);
                                    command.ExecuteScalar();
                                    MessageBox.Show("Продукт добавлен в вашу корзину!");
                                    loadData(textBox1.Text);
                                }
                                catch
                                {
                                    MessageBox.Show("Произошла ошибка добавления в корзину!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Произошла ошибка добавления в корзину! Товар, который вам нужен кончился.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка добавления в корзину!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка добавления в корзину!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Profl profl = new Profl();
            profl.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить товар под ID " + this.selectedGameId.ToString(), "Удаляемый товар", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                string NameProc = "Delete_Продукты";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(NameProc, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter nameParam = new SqlParameter
                        {
                            ParameterName = "@ID_Продукт",
                            Value = this.selectedGameId
                        };
                        command.Parameters.Add(nameParam);
                        command.ExecuteNonQuery();
                        loadData(textBox1.Text);
                        MessageBox.Show("Товар удален");
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            INSERTTABLE aut = new INSERTTABLE();
            aut.ShowDialog();
            loadData(textBox1.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            resultGrid.Columns.Clear();
            resultGrid.Columns.AddRange(new DataColumn[] { new DataColumn("Категория"), new DataColumn("Наименование"), new DataColumn("Количество"), new DataColumn("Цена") });
            SqlConnection connection = new SqlConnection(connectionString);
            string zapros = "SELECT Категории.Наименование AS Категория, Продукты.Наименование, Количество, Цена FROM Продукты, Категории WHERE Категории.ID_Категория=Продукты.ID_Категория";
            connection.Open();
            SqlDataReader reader = new SqlCommand(zapros, connection).ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    resultGrid.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3));
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
                    table.AddCell(new PdfPCell(new it.Phrase(row.ItemArray[2].ToString(), font)) { PaddingBottom = 5f, VerticalAlignment = 1, HorizontalAlignment = 1 });
                    table.AddCell(new PdfPCell(new it.Phrase(row.ItemArray[3].ToString(), font)) { PaddingBottom = 5f, VerticalAlignment = 1, HorizontalAlignment = 1 });
                }

                doc.Add(table);
                doc.Close();
                writer.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("Для начала закройте PDF файл!", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MainForm auth = new MainForm();
            this.Hide();
            auth.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            HistoryList historyList = new HistoryList(UserId);
            historyList.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\"Магазин спортивного инвентаря\" - это компьютерная программа, используемая в повседневной работе.\nПриложение можно запустить прямо с рабочего стола компьютера или ноутбука.\n\nДля того, чтобы осуществлять покупки в нашем приложении необходимо выбрать товары из \"Списка товаров\", далее добавить их в корзину при этом указав их количество снизу, зайти в корзину и нажать на кнопку \"Оформить заказ\", позже идти с распечатанным чеком в наш магазин.\n\nВ нашем приложении вы можете осуществлять сортировку, поиск по наименовании и по цене предложенный \"Список товаров\".", "Информационная справка приложения", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UPDATETABLE auth = new UPDATETABLE();
            auth.ShowDialog();
            loadData(textBox1.Text);
        }

    }
}
