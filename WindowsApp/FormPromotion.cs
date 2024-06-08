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
    public partial class FormPromotion : Form
    {
        HttpClient client = new HttpClient();

        public IEnumerable<Promotion> ListePromotion { get; private set; }
        public FormPromotion()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:7070/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormPromotion_Load(object sender, EventArgs e)
        {

        }
        private async void GetPromotion()
        {
            lb1Message.Text = "";

            var response = await client.GetStringAsync("api/Promotion");
            var promotions = JsonConvert.DeserializeObject<List<Promotion>>(response);
            dataGridView1.DataSource = promotions;
        }
        private async Task SavePromotion(Promotion promotion)
        {
            var jsonContent = JsonConvert.SerializeObject(promotion);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Promotion", content);
            response.EnsureSuccessStatusCode();
        }

        private async Task UpdatePromotion(Promotion promotion)
        {
            var jsonContent = JsonConvert.SerializeObject(promotion);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/promotion/{promotion.Id}", content);
            response.EnsureSuccessStatusCode();
        }

        private async void DeletePromotion(int id)
        {
            // Envoyer une requête DELETE vers l'API pour supprimer l'élève
            HttpResponseMessage response = client.DeleteAsync($"api/Promotion/{id}").Result;

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

        private void button4_Click(object sender, EventArgs e)
        {
            this.GetPromotion();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var selectedGroupe = dataGridView1.SelectedRows[0].DataBoundItem as Promotion;
                textBoxID.Text = selectedGroupe.Id.ToString();

                comboBoxgroup.Text = selectedGroupe.Nom;
                textBoxan.Text = selectedGroupe.Annee;
                textBoxni.Text = selectedGroupe.Niveau;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FormEleve form2 = new FormEleve();
            form2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var promotion = new Promotion()
            {
                Id = 0,
                Nom = "",
                Annee = "2024",
                Niveau = "1",
            };

            if (!string.IsNullOrEmpty(textBoxID.Text) && int.TryParse(textBoxID.Text, out int id))
            {
                promotion.Id = id;
            }
           
            
                promotion.Annee = textBoxan.Text;
            
                   promotion.Niveau = textBoxni.Text;
            
            promotion.Nom = comboBoxgroup.Text;


            if (string.IsNullOrEmpty(promotion.Nom))
            {
                lb1Message.Text = "Veuillez remplir tous les champs";
            }
            else
            {
                if (promotion.Id == 0)
                {
                    this.SavePromotion(promotion);
                    lb1Message.Text = "Promotion enregistré";
                }
                else
                {
                    // Supprimer la valeur existante de la combobox
                    comboBoxgroup.Items.Remove(comboBoxgroup.Text);

                    this.UpdatePromotion(promotion);
                    lb1Message.Text = "Promotion modifié";
                }

                // Ajouter la nouvelle valeur à la combobox
                comboBoxgroup.Items.Add(promotion.Nom);

                // Réinitialiser les champs de saisie
                textBoxID.Text = "0";
                comboBoxgroup.Text = "";
                textBoxni.Text = "1";
                textBoxan.Text = "2024";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupérer l'ID de l'élève à supprimer à partir du champ textBoxID
                int promotionId = int.Parse(textBoxID.Text);

                // Afficher une boîte de dialogue de confirmation
                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette promotion ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Vérifier la réponse de l'utilisateur
                if (result == DialogResult.Yes)
                {
                    // Supprimer l'élève
                    DeletePromotion(promotionId);

                    // Effacer les champs de saisie après la suppression
                    textBoxID.Text = "";

                    comboBoxgroup.Text = "";
                    textBoxni.Text = "";
                    textBoxan.Text = "";

                    // Mettre à jour la liste des élèves (si nécessaire)

                    // Afficher un message de confirmation
                    MessageBox.Show("promotion supprimé avec succès.", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBoxgroup.Text = "";
            textBoxni.Text = "1";
            textBoxan.Text = "2024";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "promotion.csv";
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

        private void button8_Click(object sender, EventArgs e)
        {
            
            SearchPromotion(textBoxrecherche.Text);
        }

        private void SearchPromotion(string searchTerm)
        {
            var promotions = dataGridView1.DataSource as List<Promotion>;

            if (promotions != null)
            {
                // Convert search term to lowercase
                string lowerCaseSearchTerm = searchTerm.ToLower();

                var filteredPromotions = promotions.Where(p =>
                    p.Nom.ToLower().Contains(lowerCaseSearchTerm) ||
                    p.Annee.ToLower().Contains(lowerCaseSearchTerm) ||
                    p.Niveau.ToLower().Contains(lowerCaseSearchTerm)).ToList();

                dataGridView1.DataSource = filteredPromotions;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FormGroup form10 = new FormGroup();
            form10.Show();
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

        private void textBoxrecherche_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

