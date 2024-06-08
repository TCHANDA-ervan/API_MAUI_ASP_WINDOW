using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsApp
{
    public partial class FormConnexion : Form
    {
        public FormConnexion()
        {
            InitializeComponent();
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=HP_PAVILLON\SQLEXPRESS;Initial Catalog=PointeuseDB;Integrated Security=True;TrustServerCertificate=True");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Voulez vous vraiment vous Quittez ?", "Confirmation de deconnexion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Show();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string email = textBoxlogin.Text;
            string password = textBoxpassword.Text;

            using (SqlConnection connexion = GetConnection())
            {
                try
                {
                    connexion.Open();
                    string query = "SELECT * FROM adminitrateurs WHERE Email = @Login AND Password = @Password";
                    SqlDataAdapter sda = new SqlDataAdapter(query, connexion);
                    sda.SelectCommand.Parameters.AddWithValue("@Login", email);
                    sda.SelectCommand.Parameters.AddWithValue("@Password", password);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        FormEleve form2 = new FormEleve();
                        form2.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Login ou mot de passe incorrect", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBoxlogin.Clear();
                        textBoxpassword.Clear();
                        textBoxlogin.Focus();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Erreur SQL : {ex.Message}", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormConnexion_Load(object sender, EventArgs e)
        {
            // Implémentation pour l'événement de chargement du formulaire (si nécessaire)
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBoxpassword.UseSystemPasswordChar = false;
            else
                textBoxpassword.UseSystemPasswordChar = true;
        }


        private void textBoxpassword_TextChanged(object sender, EventArgs e)
        {
            textBoxpassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        
    }
}
