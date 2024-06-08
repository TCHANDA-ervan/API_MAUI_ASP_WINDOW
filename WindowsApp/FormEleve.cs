using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsApp.Models;

namespace WindowsApp
{
    public partial class FormEleve : Form
    {
        HttpClient client = new HttpClient();

        public IEnumerable<Eleve> ListeEleve { get; private set; }
        public FormEleve()
        {
            client.BaseAddress = new Uri("https://localhost:7070/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();
        }

        private void FormEleve_Load(object sender, EventArgs e)
        {
       
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupérer l'ID de l'élève à supprimer à partir du champ textBoxID
                int eleveId = int.Parse(textBoxID.Text);

                // Afficher une boîte de dialogue de confirmation
                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet élève ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Vérifier la réponse de l'utilisateur
                if (result == DialogResult.Yes)
                {
                    // Supprimer l'élève
                    DeleteEleve(eleveId);

                    // Effacer les champs de saisie après la suppression
                    textBoxID.Text = "0";
                    textBoxgroupe.Text = "0";
                    textBoxpromotion.Text = "0";
                    textBoxNom.Text = "";
                    textBoxprenom.Text = "";
                    textBoxphone.Text = "";
                    textBoxemail.Text = "";
                    textBoxINE.Text = "";
                    textBoxpassword.Text = "";
                    textBoxrole.Text = "";
                    textBoxdescription.Text = "";

                    // Mettre à jour la liste des élèves (si nécessaire)

                    // Afficher un message de confirmation
                    MessageBox.Show("Élève supprimé avec succès.", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                var selectedEleve = dataGridView1.SelectedRows[0].DataBoundItem as Eleve;
                textBoxID.Text = selectedEleve.Id.ToString();
              textBoxgroupe.Text = selectedEleve.IdGroupe.ToString();
               textBoxpromotion.Text = selectedEleve.IdPromotion.ToString();
                textBoxNom.Text = selectedEleve.Nom;
                textBoxprenom.Text = selectedEleve.Prenom;
                textBoxphone.Text = selectedEleve.Telephone;
               textBoxemail.Text = selectedEleve.Email;
                textBoxpassword.Text = selectedEleve.Password;
                textBoxrole.Text = selectedEleve.Role;
                textBoxINE.Text = selectedEleve.INE;
                textBoxdescription.Text = selectedEleve.Description;
               
                

            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
           
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7070/");
                    HttpResponseMessage response = client.GetAsync("api/eleves").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var elevesJson = response.Content.ReadAsStringAsync().Result;
                        var eleves = JsonConvert.DeserializeObject<IEnumerable<Models.Eleve>>(elevesJson);
                        dataGridView1.DataSource = eleves;
                    }
                   
                }
            
              this.GetEleve();
        }

        private async void GetEleve()
        {
            lb1Message.Text = "";

            var response = await client.GetStringAsync("api/Eleve");
            var eleves = JsonConvert.DeserializeObject<List<Eleve>>(response);
            dataGridView1.DataSource = eleves;
        }
        private async void SaveEleve(Eleve eleve)
        {
            var jsonContent = JsonConvert.SerializeObject(eleve);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("api/Eleve/register", content);

                // Ensure the request was successful
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Élève enregistré avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Read the response content and extract the error message
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                    if (responseData != null && responseData.ContainsKey("Message"))
                    {
                        MessageBox.Show(responseData["Message"], "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Une erreur s'est produite lors de l'enregistrement de l'élève  , Email/INE existe deja ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erreur de requête: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur inattendue: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void UpdateEleve(Eleve eleve)
        {
            var jsonContent = JsonConvert.SerializeObject(eleve);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Eleve/{eleve.Id}", content);
            response.EnsureSuccessStatusCode();
        }
        private async void DeleteEleve(int id)
        {
            // Envoyer une requête DELETE vers l'API pour supprimer l'élève
            HttpResponseMessage response = client.DeleteAsync($"api/Eleve/{id}").Result;

            // Vérifier si la suppression s'est effectuée avec succès
            if (response.IsSuccessStatusCode)
            {
                // Élève supprimé avec succès
                // MessageBox.Show("Élève supprimé avec succès.", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Erreur lors de la suppression de l'élève
                // MessageBox.Show("Erreur lors de la suppression de l'élève.", "Erreur de suppression", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var eleve = new Eleve()
            {
                Id = 0,
                IdGroupe = 0,
                IdPromotion = 0,
                Nom = "",
                Prenom = "",
                Email = "",
                Telephone = "",
                Password = "",
                INE = "",
                Description = "",
                Role = ""
            };

            if (!string.IsNullOrEmpty(textBoxID.Text) && int.TryParse(textBoxID.Text, out int id))
            {
                eleve.Id = id;
            }

            if (!string.IsNullOrEmpty(textBoxgroupe.Text) && int.TryParse(textBoxgroupe.Text, out int idGroupe))
            {
                eleve.IdGroupe = idGroupe;
            }

            if (!string.IsNullOrEmpty(textBoxpromotion.Text) && int.TryParse(textBoxpromotion.Text, out int idPromotion))
            {
                eleve.IdPromotion = idPromotion;
            }

            eleve.Nom = textBoxNom.Text;
            eleve.Prenom = textBoxprenom.Text;
            eleve.Email = textBoxemail.Text;
            eleve.Telephone = textBoxphone.Text;
            eleve.Role = textBoxrole.Text;
            eleve.Description = textBoxdescription.Text;
            eleve.INE = textBoxINE.Text;
            eleve.Password = textBoxpassword.Text;

            if (string.IsNullOrEmpty(eleve.Nom) || string.IsNullOrEmpty(eleve.Prenom) ||
                string.IsNullOrEmpty(eleve.Email) || string.IsNullOrEmpty(eleve.INE) || string.IsNullOrEmpty(eleve.Password))
            {
                lb1Message.Text = "Veuillez remplir tous les champs";
            }
            else
            {
                if (eleve.Id == 0)
                {
                    this.SaveEleve(eleve);
                    lb1Message.Text = "Élève enregistré";
                }
                else
                {
                    this.UpdateEleve(eleve);
                    lb1Message.Text = "Élève modifié";
                }

                // Réinitialiser les champs de saisie
                textBoxID.Text = "0";
                textBoxgroupe.Text = "0";
                textBoxpromotion.Text = "0";
                textBoxNom.Text = "";
                textBoxprenom.Text = "";
                textBoxphone.Text = "";
                textBoxemail.Text = "";
                textBoxINE.Text = "";
                textBoxpassword.Text = "";
                textBoxrole.Text = "";
                textBoxdescription.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxID.Text = "";
            textBoxgroupe.Text = "";
            textBoxpromotion.Text = "";
            textBoxNom.Text = "";
            textBoxprenom.Text = "";
            textBoxphone.Text = "";
            textBoxemail.Text = "";
            textBoxINE.Text = "";
            textBoxpassword.Text = "";
            textBoxrole.Text = "";
            textBoxdescription.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "eleve.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();

                // En-têtes des colonnes
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    sb.Append(dataGridView1.Columns[i].HeaderText);
                    if (i < dataGridView1.Columns.Count - 1)
                    {
                        sb.Append(";");
                    }
                }
                sb.AppendLine();

                // Données des cellules
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        sb.Append(row.Cells[j].Value);
                        if (j < row.Cells.Count - 1)
                        {
                            sb.Append(";");
                        }
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(sfd.FileName, sb.ToString());
                MessageBox.Show("Données exportées avec succès.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormPromotion form1 = new FormPromotion();
            form1.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SearchEleve(textBoxrecherche.Text);
        }
        private void SearchEleve(string searchTerm)
        {
            var eleves = dataGridView1.DataSource as List<Eleve>;

            if (eleves != null)
            {
                string lowerCaseSearchTerm = searchTerm.ToLower();

                var filteredEleves = eleves.Where(e =>
                    e.Nom.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.Prenom.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.Email.ToLower().Contains(lowerCaseSearchTerm) ||
                     e.Description.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.Role.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.Telephone.ToLower().Contains(lowerCaseSearchTerm) ||
                    e.INE.ToLower().Contains(lowerCaseSearchTerm)).ToList();

                dataGridView1.DataSource = filteredEleves;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            FormGroup form2 = new FormGroup();
            form2.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Voulez vous vraiment vous deconnectez ?", "Confirmation de deconnexion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                FormConnexion form = new FormConnexion();
                form.Show();
                this.Hide();
            }
        }
    }
}
