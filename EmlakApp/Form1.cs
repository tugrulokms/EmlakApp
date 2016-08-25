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

namespace EmlakApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source = TUGRUL; Initial Catalog = deneme; Persist Security Info=True;User ID = sa; Password=ugur1230");
        private void Form1_Load(object sender, EventArgs e)
        {
            con.Open();

            string select = "SELECT Sehir From Cities";

            SqlCommand cmd = new SqlCommand(select, con);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cmbSehir.Items.Add(reader.GetValue(0));
            }
            con.Close();

            cmbIlce.Enabled = false;
            dgv.Enabled = false;
            btnAdd.Enabled = false;
        }

        private void cmbSehir_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbIlce.Enabled = true;
            dgv.Enabled = false;
            btnAdd.Enabled = false;

            con.Open();
         
            cmbIlce.Items.Clear();
            cmbIlce.Text = "İlçe Seçiniz";

            string select = "SELECT Counties.Ilce FROM Cities INNER JOIN Counties ON Cities.ID=Counties.SehirID WHERE Counties.SehirID = "+ (cmbSehir.SelectedIndex + 1) +" ORDER BY Counties.ID ASC";

            SqlCommand cmd = new SqlCommand(select, con);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cmbIlce.Items.Add(reader.GetValue(0).ToString());
            }
 
            con.Close();

            cmbIlce.SelectedIndex = -1;
        }

        private void cmbIlce_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv.Enabled = true;
            btnAdd.Enabled = true;
            con.Open();

            string query = "SELECT EstateInfo.FullName, EstateInfo.Address,EstateInfo.ID, EstateType.EstateType FROM EstateInfo INNER JOIN Counties ON Counties.Ilce=EstateInfo.County INNER JOIN EstateType ON EstateInfo.TypeID=EstateType.TypeID WHERE Counties.SehirID = "
                + (cmbSehir.SelectedIndex + 1) + " AND Counties.ID = (select ID from Counties where SehirID=" + (cmbSehir.SelectedIndex + 1) + " and Ilce='" + cmbIlce.Text+"')";
            //Clipboard.SetText(query);

            SqlCommand cmd = new SqlCommand(query,con);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                dgv.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
            }

            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Register r1 = new Register(0);
            
            if (r1.ShowDialog() == DialogResult.OK)
            {
                con.Open();

                string query = "UPDATE EstateInfo SET County = '" + cmbIlce.Text + "' WHERE County IS NULL";

                string query2 = "SELECT FullName,Address,ID FROM EstateInfo ORDER BY ID DESC";

                SqlCommand cmd = new SqlCommand(query,con);

                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand(query2, con);

                SqlDataReader reader = cmd2.ExecuteReader();

                reader.Read();
                dgv.Rows.Add(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2));
        
                con.Close();
            }
        }

        private void dgv_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells[2].Value);

            Register r1 = new Register(id);

            if (r1.ShowDialog() == DialogResult.OK)
            {
                con.Open();

                string query = "SELECT FullName,Address,ID FROM EstateInfo WHERE ID = " + id;

                SqlCommand cmd = new SqlCommand(query,con);

                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                for (int i = 0; i < 3; i++)
                {
                    dgv.Rows[e.RowIndex].Cells[i].Value = reader.GetValue(i);
                }

                con.Close();
            }
            
        }
    }
 
        


}
