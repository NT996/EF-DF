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
    /// Interaction logic for ZaposleniWindow.xaml
    /// </summary>
    public partial class ZaposleniWindow : Window
    {
        ProjectDbContext _context = new ProjectDbContext();
        public static DataGrid datagrid;

        public ZaposleniWindow()
        {
            InitializeComponent();
            ucitajGrid();
        }
        private void ucitajGrid()
        {
            gridZaposleni.ItemsSource = _context.Employees.ToList();
            datagrid = gridZaposleni;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            osveziEkran();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                updateEmployee();
            }
            else
            {
                createEmployee();
            }
        }
        private void createEmployee()
        {
            Employee newEmploye = new Employee()
            {
                Name = txtIme.Text,
                Lastname = txtPrezime.Text,
                JMBG =Convert.ToInt64(txtJMBG.Text)
            
            };

            _context.Employees.Add(newEmploye);
            _context.SaveChanges();
            gridZaposleni.ItemsSource = _context.Employees.ToList();
            osveziEkran();
        }
        private void updateEmployee()
        {
            int Id = Convert.ToInt32(txtId.Text);
            Employee updateEmployee = (from c in _context.Employees
                                       where c.Id == Id
                                       select c).Single();
            updateEmployee.Name = txtIme.Text;
            updateEmployee.Lastname = txtPrezime.Text;
            updateEmployee.JMBG =Convert.ToInt64(txtJMBG.Text);
           

            _context.SaveChanges();
            datagrid.ItemsSource = _context.Employees.ToList();

            osveziEkran();

        }
        private void osveziEkran()
        {
            txtId.Clear();
            txtIme.Clear();
            txtPrezime.Clear();
            txtPretraga.Clear();
            txtJMBG.Clear();
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            deleteEmployee();
        }
        private void deleteEmployee()
        {

            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Niste odabrali radnika za brisanje");
            }
            else
            {
                int id = Convert.ToInt32(txtId.Text);
                var deleteEmployee = _context.Employees.Where(c => c.Id == id).Single();
                _context.Employees.Remove(deleteEmployee);
                _context.SaveChanges();
                datagrid.ItemsSource = _context.Employees.ToList();
            }
            osveziEkran();
        }

        private void btnPretraga_Click(object sender, RoutedEventArgs e)
        {
            pretraziZaposlene();
        }
        private void pretraziZaposlene()
        {

            int id = Convert.ToInt32(txtPretraga.Text);
            var zaposleni = _context.Employees.SingleOrDefault(p => p.Id == id);
            if (zaposleni == null)
            {
                MessageBox.Show("Ne postoji zaposleni sa ovim id-jem.");
                return;
            }
            txtId.Text = zaposleni.Id.ToString();
            txtIme.Text = zaposleni.Name;
            txtPrezime.Text = zaposleni.Lastname;
            txtJMBG.Text = zaposleni.JMBG.ToString();
        }
    
    }
}
