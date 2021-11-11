using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace TP3
{
    public partial class Form1 : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\elkhammy\source\repos\TP3\BasseTP3.mdb");
        OleDbDataAdapter Da;
        DataTable Dt = new DataTable();
        OleDbCommand cmd = new OleDbCommand();
        DataSet ds = new DataSet();
        OleDbDataReader rd;
        public Form1()
        {
            InitializeComponent();
            DatagridView();
        }
        void DatagridView()
        {
            Dt.Clear();
            Da = new OleDbDataAdapter("Select * from Stagiaire", conn);
            Da.Fill(Dt);
            dataGridView1.DataSource = Dt;
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (txtNom.Text != "" && txtCin.Text != "" && txtDatanaissance.Text != "" && txtSexe.Text !="")
            {
                Da = new OleDbDataAdapter("Select Nom From Stagiaire where Nom = '" + txtNom.Text + "'", conn);
                Da.Fill(ds);
                int i = ds.Tables[0].Rows.Count;
                if (i > 0)
                {
                    MessageBox.Show("Nom existant déjà");
                    ds.Clear();
                }
                else
                {

                    cmd = new OleDbCommand("Insert Into Stagiaire (Nom,CIN,Datedenaissance,Sexe) values ('" + txtNom.Text + "','" + txtCin.Text + "','"+txtDatanaissance.Text+"','"+txtSexe.Text+"')", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    DatagridView();
                    MessageBox.Show("Stagiaire ajouté", "Add", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            else
            {
                MessageBox.Show("S'il vous plait entrer (Nom,CIN,Data de naissance,Sexe", "Vide", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnRechercher_Click(object sender, EventArgs e)
        {
            if (textCin.Text != "")
            {
                Da = new OleDbDataAdapter("select * from Stagiaire where CIN = '" + textCin.Text + "'", conn);
                /*méthode1
                Da.Fill(ds, "Stagiaire");
                if (ds.Tables["Stagiaire"].Rows.Count == 0)
                {
                   
                    MessageBox.Show("le stagiaire CIN:"+this.textCin.Text+" , n'existe pas !!");
                    dataGridView1.DataSource = null;
                   
                }
                else
                {
                    
                    this.btModefier.Visible = true;
                    this.btnSupprimer.Visible = true;
                    dataGridView1.DataSource = ds.Tables["Stagiaire"];
                    
                }*/

                //methode 2
                conn.Open();
                Dt = new DataTable();
                Da.Fill(Dt);
                dataGridView1.DataSource = Dt;
                dataGridView1.Refresh();
                Da.Fill(ds);
                int i = ds.Tables[0].Rows.Count;
                if (i > 0)
                {
                    this.btModefier.Visible = true;
                    this.btnSupprimer.Visible = true;
                    ds.Clear();
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("S'il vous plait entrer CIN", "Vide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            try {
                OleDbCommand cmd = new OleDbCommand("DELETE FROM Stagiaire WHERE CIN = '" + textCin.Text + "'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DatagridView();
                conn.Close();
            }
            catch
            {
                MessageBox.Show("non supprimer");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.groupBox1.Visible = false;
            this.btnSupprimer.Visible = false;
            this.btModefier.Visible = false;
        }
        int cool;
        private void txtModefier_Click(object sender, EventArgs e)
        {
            int tr = 0;
            
            

                for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
                {
                    if (this.dataGridView1.Rows[i].Cells[1].Value.ToString() == textCin.Text)
                    {
                        cool = i;
                        tr = 1;
                }
            }
                if (tr == 0)
                    MessageBox.Show("cette persson n'existe pas dans la liste");
                else
                    this.groupBox1.Visible = true;
            
        }

        private void txtAppliqué_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows[cool].Cells[1].Value = txtC.Text;
            this.dataGridView1.Rows[cool].Cells[2].Value = txtN.Text;
            OleDbCommand cmd = new OleDbCommand("DELETE FROM Stagiaire WHERE CIN = '" + textCin.Text + "'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            MessageBox.Show("Data Modifier", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
            this.groupBox1.Visible = false;
            this.btModefier.Visible = false;
            this.btnSupprimer.Visible = false;
            this.txtCin.Text = string.Empty;
            
        }
    }
}
