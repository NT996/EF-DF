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
    /// Interaction logic for ZaduzenjeWindow.xaml
    /// </summary>
    public partial class ZaduzenjeWindow : Window
    {
        ProjectDbContext _context = new ProjectDbContext();
        public static DataGrid datagrid;

        public ZaduzenjeWindow()
        {
            InitializeComponent();
            ucitajGrid();
            txtZaposleni.ItemsSource = _context.Employees.ToList();
            txtZaposleni.DisplayMemberPath = "Name";
            txtZaposleni.SelectedValuePath = "Id";

        }
        private void ucitajGrid()
        {
            gridProjekat.ItemsSource = _context.Projects.ToList();
            datagrid = gridProjekat;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetujEkran();
        }
        private void resetujEkran()
        {
            txtPretraga.Clear();
            txtPretragaRadnik.Clear();
            txtZaposleni.SelectedIndex= -1;
            gridZaposleni.ItemsSource = null;


        }

        private void btnPretraga_Click(object sender, RoutedEventArgs e)
        {
            pretrazi();
        }
        private void pretrazi()
        {
            int id = Convert.ToInt32(txtPretraga.Text);
            gridZaposleni.ItemsSource = _context.Assignments.Include("Employee").Where(i=> i.ProjectId == id).Select(o => new
            { Id= o.EmployeeId,Ime = o.Employee.Name, Prezime = o.Employee.Lastname }).ToList();
        }

        private void txtZaposleni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            dodajZaposlenog();
        }
        private void dodajZaposlenog()
        {
            Assignment newAssignment = new Assignment()
            {
                ProjectId = Convert.ToInt32(txtPretraga.Text),
                EmployeeId = Convert.ToInt32(txtZaposleni.SelectedValue),
                DateJoined = DateTime.Now

            };

            _context.Assignments.Add(newAssignment);
            _context.SaveChanges();
            int id = Convert.ToInt32(txtPretraga.Text);
            gridZaposleni.ItemsSource = _context.Assignments.Include("Employee").Where(i => i.ProjectId == id).Select(o => new
            { Id = o.EmployeeId, Ime = o.Employee.Name, Prezime = o.Employee.Lastname }).ToList();

        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            deleteAssignment();
        }
        private void deleteAssignment()
        {

            if (string.IsNullOrWhiteSpace(txtPretragaRadnik.Text))
            {
                MessageBox.Show("Niste odabrali radnika za brisanje");
            }
            else
            {
                int id = Convert.ToInt32(txtPretragaRadnik.Text);
                int pid = Convert.ToInt32(txtPretraga.Text);
                var deleteAsignment = _context.Assignments.Where(c => c.EmployeeId == id).Where(c => c.ProjectId == pid).Single();
                _context.Assignments.Remove(deleteAsignment);
                _context.SaveChanges();
                
                gridZaposleni.ItemsSource = _context.Assignments.Include("Employee").Where(i => i.ProjectId == pid).ToList();
            }
            
        }

        private void btnPretragaRadnik_Click(object sender, RoutedEventArgs e)
        {
            pretragaRadnika();
        }
        private void pretragaRadnika()
        {
            int pid = Convert.ToInt32(txtPretraga.Text);
            int id = Convert.ToInt32(txtPretragaRadnik.Text);
            var zaposleni = _context.Assignments.Where(p => p.ProjectId ==pid).FirstOrDefault(p => p.EmployeeId == id);
            if (zaposleni == null)
            {
                MessageBox.Show("Ne postoji zaposleni sa ovim id-jem.");
                return;
            }
            txtZaposleni.SelectedValue = zaposleni.EmployeeId;


        }
    }
}
