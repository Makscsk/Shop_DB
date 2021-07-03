using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\C#\Курсач СУБД\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\Shop.mdf;Integrated Security=True;Connect Timeout=30";
        int userId;
        string userStr;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {            
            label4.Visible = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            MainForm.ActiveForm.Hide();
            Regist reg = new Regist();
            reg.Show();
        }
        public static bool VerifyPassword(string hashedPassword, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, salt, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string pass = textBox2.Text.Trim();
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string authRequest = "SELECT * FROM Пользователи WHERE Логин = '" + login + "'";
                SqlCommand command = new SqlCommand(authRequest, connect);
                SqlDataReader reader = command.ExecuteReader();

                if (textBox1.Text != "" || textBox2.Text != "")
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        string hashedPassword = reader.GetValue(2).ToString();
                        if (VerifyPassword(hashedPassword, pass) == true)
                        {
                            userId = Convert.ToInt32(reader.GetValue(0).ToString());
                            userStr = reader.GetValue(0).ToString();
                            Magazin f3 = new Magazin(userId, userStr, textBox1, textBox2);
                            MainForm.ActiveForm.Hide();

                            f3.Show();
                        }
                    }
                    else
                    {
                        label4.Text = "Неверный логин или пароль!";
                        label4.Visible = true;
                    }
                }
                else
                {
                    label4.Text = "Вы не ввели значения";
                    label4.Visible = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
                button1.BackgroundImage = WindowsFormsApplication1.Properties.Resources.Фон_кнопок_Подтв;
            else
                button1.BackgroundImage = WindowsFormsApplication1.Properties.Resources.Фон_кнопок_Выход;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
                button1.BackgroundImage = WindowsFormsApplication1.Properties.Resources.Фон_кнопок_Подтв;
            else
                button1.BackgroundImage = WindowsFormsApplication1.Properties.Resources.Фон_кнопок_Выход;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Свяжитесь с нашим оператором по телефону  +375 (33) 345 73 62 и сообщите ему свой \"Лоигн\"", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
