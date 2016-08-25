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
    public partial class Register : Form
    {
        SqlConnection con = new SqlConnection("Data Source = TUGRUL; Initial Catalog = deneme; Persist Security Info=True;User ID = sa; Password=ugur1230");
        int id;

        public Register(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            label4.Text = "\u33A1" + ":";

            cbType.Items.Add("Residence");
            cbType.Items.Add("Office");
            cbType.Items.Add("Land");

            cbRoom.Items.Add("1+0 (Studio)");
            cbRoom.Items.Add("1+1");
            cbRoom.Items.Add("2+1");
            cbRoom.Items.Add("2+2");
            cbRoom.Items.Add("3+1");
            cbRoom.Items.Add("3+2");            
            
            if (id != 0)
            {
                con.Open();

                string[] array = {"1+0 (Studio)", "1+1", "2+1", "2+2", "3+1", "3+2"};

                string query = "SELECT TypeID, FullName, Address, RoomNumber, mSquare, State, Floor FROM EstateInfo WHERE ID = " + id;

                SqlCommand cmd = new SqlCommand(query,con);
                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                txtName.Text = reader.GetString(1);
                txtAddress.Text = reader.GetString(2);
                txtSquare.Text = reader.GetString(4);
                txtFloor.Text = reader.GetString(6);

                cbType.SelectedIndex = ((int)reader.GetValue(0)) - 1;
                cbRoom.Text = reader.GetString(4);

                if (reader.GetString(5) == "Sale")
                    rbSale.Checked = true;
                else
                    rbRental.Checked = true;



                btnAdd.Text = "UPDATE";

                con.Close();
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            string query;

            string result;

            if (rbRental.Checked)
                result = rbRental.Text;
            else
                result = rbSale.Text;

            con.Open();

            if (id != 0)
            {             
                query = "UPDATE EstateInfo SET FullName = '" + txtName.Text + "', Address = '" + txtAddress.Text + "', RoomNumber = '" +
                    cbRoom.Text + "', mSquare = '" + txtSquare.Text + "', State = '" + result + "', Floor = '" + txtFloor.Text + "' WHERE ID = " + id;
            }
            else
            {
                query = "INSERT INTO EstateInfo (FullName, Address, RoomNumber, mSquare, State, Floor) VALUES ('" + txtName.Text + "','" + txtAddress.Text + "','" +
                    cbRoom.Text + "','" + txtSquare.Text + "','" + result + "','" + txtFloor.Text + "')";
            }

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.ExecuteNonQuery();

            con.Close();

            DialogResult = DialogResult.OK;
        }
    }
}
