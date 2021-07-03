using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class INSERTTABLE : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\C#\Курсач СУБД\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\Shop.mdf;Integrated Security=True;Connect Timeout=30";
        public INSERTTABLE()
        {
            InitializeComponent();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string NameProc = "Insert_Продукты";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(NameProc, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;                       
                    SqlParameter nameParam1 = new SqlParameter
                    {
                        ParameterName = "@ID_Категория",
                        Value = textBox1.Text.Trim()
                    };
                    command.Parameters.Add(nameParam1);
                    SqlParameter nameParam2 = new SqlParameter
                    {
                        ParameterName = "@Наименование",
                        Value = textBox2.Text.Trim()
                    };
                    command.Parameters.Add(nameParam2);
                    SqlParameter nameParam3 = new SqlParameter
                    {
                        ParameterName = "@Количество",
                        Value = textBox3.Text.Trim()
                    };
                    command.Parameters.Add(nameParam3);
                    SqlParameter nameParam4 = new SqlParameter
                    {
                        ParameterName = "@Цена",
                        Value = textBox4.Text.Trim()
                    };
                    command.Parameters.Add(nameParam4);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Товар добавлен");
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }
    }
}
