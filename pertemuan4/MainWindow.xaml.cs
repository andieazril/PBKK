using System.Windows;
using System.Windows.Controls;

namespace StudentRegistrationApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {
            string nim = txtNim.Text;
            string nama = txtNama.Text;
            string prodi = (cmbProdi.SelectedItem as ComboBoxItem)?.Content.ToString();
            string jenisKelamin = rbLaki.IsChecked == true ? "Laki-laki"
                                : rbPerempuan.IsChecked == true ? "Perempuan"
                                : null;

            // Validasi sederhana
            if (string.IsNullOrWhiteSpace(nim) || string.IsNullOrWhiteSpace(nama)
                || prodi == null || jenisKelamin == null)
            {
                MessageBox.Show("Mohon lengkapi semua data terlebih dahulu.", "Peringatan",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tambahkan ke ListBox
            string dataMahasiswa = $"{nim} - {nama} - {prodi} - {jenisKelamin}";
            lstMahasiswa.Items.Add(dataMahasiswa);

            ResetForm();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtNim.Clear();
            txtNama.Clear();
            cmbProdi.SelectedIndex = -1;
            rbLaki.IsChecked = false;
            rbPerempuan.IsChecked = false;
            lstMahasiswa.SelectedItem = null;
            txtNim.Focus();
        }

        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            if (lstMahasiswa.SelectedItem == null)
            {
                MessageBox.Show("Pilih data yang ingin dihapus terlebih dahulu.", "Peringatan",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var konfirmasi = MessageBox.Show(
                $"Yakin ingin menghapus data ini?\n\n{lstMahasiswa.SelectedItem}",
                "Konfirmasi Hapus",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (konfirmasi == MessageBoxResult.Yes)
            {
                lstMahasiswa.Items.Remove(lstMahasiswa.SelectedItem);
            }
        }
    }
}   