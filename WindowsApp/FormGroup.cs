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
    public partial class FormGroup : Form
    {
        HttpClient client = new HttpClient();

        public IEnumerable<Groupe> ListeGroupe { get; private set; }
        public FormGroup()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:7070/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var groupe = new Groupe()
            {
                Id = 0,
                Nom = "",
                Formation = "",
            };

            if (!string.IsNullOrEmpty(textBoxID.Text) && int.TryParse(textBoxID.Text, out int id))
            {
                groupe.Id = id;
            }

            groupe.Nom = comboBoxgroup.Text;
            groupe.Formation = comboBoxpromo.Text;

            if (string.IsNullOrEmpty(groupe.Nom) || string.IsNullOrEmpty(groupe.Formation))
            {
                lb1Message.Text = "Veuillez remplir tous les champs";
            }
            else
            {
                if (groupe.Id == 0)
                {
                    this.SaveGroupe(groupe);
                    lb1Message.Text = "Groupe enregistré";
                }
                else
                {
                    // Supprimer la valeur existante de la combobox
                    comboBoxgroup.Items.Remove(comboBoxgroup.Text);

                    this.UpdateGroupe(groupe);
                    lb1Message.Text = "Groupe modifié";
                }

                // Ajouter la nouvelle valeur à la combobox
                comboBoxgroup.Items.Add(groupe.Nom);

                // Réinitialiser les champs de saisie
                textBoxID.Text = "0";
                comboBoxgroup.Text = "";
                comboBoxpromo.Text = "";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var selectedGroupe = dataGridView1.SelectedRows[0].DataBoundItem as Groupe;
                textBoxID.Text = selectedGroupe.Id.ToString();

                comboBoxgroup.Text = selectedGroupe.Nom;
                comboBoxpromo.Text = selectedGroupe.Formation;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.GetGroupe();
        }
        private async void GetGroupe()
        {
            lb1Message.Text = "";

            var response = await client.GetStringAsync("api/Groupe");
            var groupes = JsonConvert.DeserializeObject<List<Groupe>>(response);
            dataGridView1.DataSource = groupes;
        }
        private async Task SaveGroupe(Groupe groupe)
        {
            var jsonContent = JsonConvert.SerializeObject(groupe);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Groupe", content);
            response.EnsureSuccessStatusCode();
        }

        private async Task UpdateGroupe(Groupe groupe)
        {
            var jsonContent = JsonConvert.SerializeObject(groupe);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Groupe/{groupe.Id}", content);
            response.EnsureSuccessStatusCode();
        }
        private async void DeleteGroupe(int id)
        {
            // Envoyer une requête DELETE vers l'API pour supprimer l'élève
            HttpResponseMessage response = client.DeleteAsync($"api/Groupe/{id}").Result;

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

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupérer l'ID de l'élève à supprimer à partir du champ textBoxID
                int groupeId = int.Parse(textBoxID.Text);

                // Afficher une boîte de dialogue de confirmation
                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce groupe ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Vérifier la réponse de l'utilisateur
                if (result == DialogResult.Yes)
                {
                    // Supprimer l'élève
                    DeleteGroupe(groupeId);

                    // Effacer les champs de saisie après la suppression
                    textBoxID.Text = "";

                    comboBoxgroup.Text = "";
                    comboBoxpromo.Text = "";

                    // Mettre à jour la liste des élèves (si nécessaire)

                    // Afficher un message de confirmation
                    MessageBox.Show("Groupe supprimé avec succès.", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "groupe.csv";
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

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxID.Text = "";
            comboBoxgroup.Text = "";
            comboBoxpromo.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FormEleve form3 = new FormEleve();
            form3.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormPromotion form2 = new FormPromotion();
            form2.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            FormEleve form2 = new FormEleve();
            form2.Show();
            this.Hide();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            FormPromotion form1 = new FormPromotion();
            form1.Show();
            this.Hide();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Voulez vous vraiment vous deconnectez ?", "Confirmation de deconnexion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                FormConnexion form = new FormConnexion();
                form.Show();
                this.Hide();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SearchPromotion(textBoxrecherche.Text);
        }

        private void SearchPromotion(string searchTerm)
        {
            var groupes = dataGridView1.DataSource as List<Groupe>;

            if (groupes != null)
            {
                // Convert search term to lowercase
                string lowerCaseSearchTerm = searchTerm.ToLower();

                var filteredgroupes = groupes.Where(p =>
                    p.Nom.ToLower().Contains(lowerCaseSearchTerm) ||
                    p.Formation.ToLower().Contains(lowerCaseSearchTerm)).ToList();

                dataGridView1.DataSource = filteredgroupes;
            }
        }
    }
    
}
