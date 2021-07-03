using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class HistoryList : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\C#\Курсач СУБД\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\Shop.mdf;Integrated Security=True;Connect Timeout=30";
        int UserId;

        public HistoryList(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;
            display_data();
        }

        public void display_data()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            if (UserId != 9)
                cmd.CommandText = "SELECT Логин, Дата_оформления, Сумма_к_уплате FROM Пользователи, История_заказов WHERE История_заказов.ID_Пользователь = " + UserId + " AND Пользователи.ID_Пользователь = " + UserId + "";
            else
                cmd.CommandText = "SELECT Логин, Дата_оформления, Сумма_к_уплате FROM View_History";
            cmd.ExecuteNonQuery();
            DataTable dta = new DataTable();
            SqlDataAdapter dataadp = new SqlDataAdapter(cmd);
            dataadp.Fill(dta);
            dataGridView1.DataSource = dta;
            connection.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadData(textBox1.Text);
        }

        private void loadData(string searchString = "")
        {
            string sql;
            string searchQuery = ""; 
            if (searchString != "")
            {
                searchQuery = " AND (Дата_оформления Like N'%" + searchString + "%')";
            }
            if (UserId != 9)
                sql = "SELECT Логин, Дата_оформления, Сумма_к_уплате FROM Пользователи, История_заказов WHERE История_заказов.ID_Пользователь = " + UserId + " AND Пользователи.ID_Пользователь = " + UserId + "" + searchQuery;
            else
                sql = "SELECT Логин, Дата_оформления, Сумма_к_уплате FROM View_History WHERE Дата_оформления Like N'%" + searchString + "%'";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "Логин");
            connection.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Логин";
        }

        private void HistoryList_Load(object sender, EventArgs e)
        {
            int sum = 0;
            for (int i = 0; i < dataGridView1.RowCount; ++i)
            {
                sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
            }
            label2.Text = sum.ToString();
        }
    }
}
