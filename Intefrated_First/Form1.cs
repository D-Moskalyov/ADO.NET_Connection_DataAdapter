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

namespace Intefrated_First
{
    public partial class Form1 : Form
    {
        string connectionSrting = "server=localhost;integrated security=true;database=northwind";
        SqlConnection connection;
        SqlCommand commamd;
        SqlDataReader reader;
        DataTable sqlTable;
        SqlDataAdapter adapter;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                connection = new SqlConnection(connectionSrting);
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        label1.BackColor = Color.Green;
                        //string cmd = "select * from Employees";
                        //SqlCommand commamd = new SqlCommand(cmd, connection);
                        //SqlDataReader reader = commamd.ExecuteReader();
                    }
                    else
                    {
                        MessageBox.Show("Connection failed");
                    }
                }
                catch (SqlException ee)
                {
                    MessageBox.Show("Connection failed");
                    MessageBox.Show(String.Format("Server {0}", ee.Server));
                    MessageBox.Show(ee.Message);
                }
            }
            //finally
            //{
            //    connection.Close();
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
                connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
                label1.BackColor = Color.Red;
                listBox1.Items.Clear();
                listBox2.Items.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                listBox1.Items.Clear();
                string cmd = "select * from Employees";
                commamd = new SqlCommand(cmd, connection);
                reader = commamd.ExecuteReader();
                while (reader.Read())
                {
                    listBox1.Items.Add((string)reader[2] + " " + (string)reader[1]);
                }
                reader.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                string name = listBox1.SelectedItem.ToString();
                int num = name.IndexOf(" ");
                char[] lastName = name.ToCharArray(num + 1, name.Length - num - 1);
                string last = new string(lastName);
                string cmd = "select * from Employees where LastName = '" + last + "'";
                //MessageBox.Show(cmd);
                SqlCommand commamd = new SqlCommand(cmd, connection);
                SqlDataReader reader = commamd.ExecuteReader();
                reader.Read();
                Form2 form2 = new Form2();
                form2.Show();
                Label lb;
                TextBox tB;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    lb = new Label();
                    lb.Location = new System.Drawing.Point(5, i * 20 + i * 2 + 5);
                    lb.AutoSize = true;
                    lb.Text = reader.GetName(i) + ":";

                    tB = new TextBox();
                    tB.Location = new System.Drawing.Point(90, i * 20 + i * 2 + 5);
                    tB.Size = new System.Drawing.Size(222, 20);
                    tB.ReadOnly = true;
                    tB.Text = reader[i].ToString();

                    form2.Controls.Add(lb);
                    form2.Controls.Add(tB);
                    //Console.WriteLine("{0}: {1}", reader.GetName(i), reader[i]);
                }
                reader.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                listBox2.Items.Clear();
                string cmd = "select * from Customers";
                commamd = new SqlCommand(cmd, connection);
                reader = commamd.ExecuteReader();
                while (reader.Read())
                {
                    listBox2.Items.Add((string)reader[1]);
                }
                reader.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                ListBox.SelectedObjectCollection emp = listBox1.SelectedItems;
                ListBox.SelectedObjectCollection cust = listBox2.SelectedItems;
                if (emp.Count != 0 && cust.Count != 0)
                {
                    string cmd = "select * from Customers";
                    //            select OrderID, o.CustomerID, o.EmployeeID, OrderDate
                    //from Orders as o
                    //join
                    //Employees as e
                    //on 
                    //o.EmployeeID = e.EmployeeID
                    //join
                    //Customers as c
                    //on o.CustomerID = c.CustomerID
                    //where (c.ContactName = '')
                    //and (e.LastName = '')
                    commamd = new SqlCommand(cmd, connection);
                    sqlTable = new DataTable();
                    adapter = new SqlDataAdapter();
                    adapter.SelectCommand = commamd;
                    adapter.Fill(sqlTable);
                    dataGridView1.DataSource = sqlTable;
                }
            }
        }
    }
}
