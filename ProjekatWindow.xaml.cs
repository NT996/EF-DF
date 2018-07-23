using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFProjectDBFirst
{
    /// <summary>
    /// Interaction logic for ProjekatWindow.xaml
    /// </summary>
    public partial class ProjekatWindow : Window
    {
        ProjectDbContext _context = new ProjectDbContext();
        public static DataGrid datagrid;

        public ProjekatWindow()
        {
            InitializeComponent();
            ucitajGrid();
        }
        private void ucitajGrid()
        {
            gridProjekat.ItemsSource = _context.Projects.ToList();
            datagrid = gridProjekat;
        }

        private void btnOsvezi_Click(object sender, RoutedEventArgs e)
        {
            osveziEkran();
        }
        private void osveziEkran()
        {
            txtId.Clear();
            txtNaziv.Clear();
            txtOpis.Clear();
            txtPretraga.Clear();
            txtDatumPocetka.SelectedDate = null;
            txtDatumPocetka.DisplayDate = DateTime.Today;
            txtDeadline.SelectedDate = null;
            txtDeadline.DisplayDate = DateTime.Today;
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            deleteProject();
        }
        private void deleteProject()
        {

            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Niste odabrali projekat za brisanje");
            }
            else
            {
                int id = Convert.ToInt32(txtId.Text);
                var deleteProject = _context.Projects.Where(c => c.Id == id).Single();
                _context.Projects.Remove(deleteProject);
                _context.SaveChanges();
                datagrid.ItemsSource = _context.Projects.ToList();
            }
            osveziEkran();
        }

        private void btnPretraga_Click(object sender, RoutedEventArgs e)
        {
            pretraziProjekte();
        }
        private void pretraziProjekte()
        {

            int id = Convert.ToInt32(txtPretraga.Text);
            var projekti = _context.Projects.SingleOrDefault(p => p.Id == id);
            if (projekti == null)
            {
                MessageBox.Show("Ne postoji projekat sa ovim id-jem.");
                return;
            }
            txtId.Text = projekti.Id.ToString();
            txtNaziv.Text = projekti.Name;
            txtOpis.Text = projekti.Description;
            txtDatumPocetka.SelectedDate = projekti.DateStarted;
            if (projekti.Deadline.HasValue)
                txtDeadline.SelectedDate = projekti.Deadline.Value;
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                updateProject();
            }
            else
            {
                createProject();
            }
        }
        private void createProject()
        {
            Project newProject = new Project()
            {
                Name = txtNaziv.Text,
                Description = txtOpis.Text,
                DateStarted = txtDatumPocetka.DisplayDate,
                Deadline = txtDeadline.DisplayDate

            };

            _context.Projects.Add(newProject);
            _context.SaveChanges();
            gridProjekat.ItemsSource = _context.Projects.ToList();
            osveziEkran();
        }
        private void updateProject()
        {
            int Id = Convert.ToInt32(txtId.Text);
            Project updateProject = (from c in _context.Projects
                                       where c.Id == Id
                                       select c).Single();
            updateProject.Name = txtNaziv.Text;
            updateProject.Description = txtOpis.Text;
            updateProject.DateStarted = txtDatumPocetka.SelectedDate;
            updateProject.Deadline = txtDeadline.SelectedDate;

            _context.SaveChanges();
            datagrid.ItemsSource = _context.Projects.ToList();

            osveziEkran();

        }
    }
}
