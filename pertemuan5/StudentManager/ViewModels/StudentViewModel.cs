using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.ViewModels;

public class StudentViewModel : INotifyPropertyChanged
{
    private readonly StudentRepository _repository = new();

    public ObservableCollection<Student> Students { get; } = new();

    // Baris yang dipilih di DataGrid
    private Student? _selectedStudent;
    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged();
            if (value != null)
                EditStudent = value.Clone(); // form bekerja di salinan
        }
    }

    // Data yang sedang diedit di form
    private Student _editStudent = new();
    public Student EditStudent
    {
        get => _editStudent;
        set { _editStudent = value; OnPropertyChanged(); }
    }

    private string _searchText = "";
    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); }
    }

    public int TotalStudents => Students.Count;
    public int TotalInformatika => Students.Count(x => x.Jurusan == "Informatika");
    public int TotalSistemInformasi => Students.Count(x => x.Jurusan == "Sistem Informasi");
    public int TotalLakiLaki => Students.Count(x => x.Gender == "Laki-laki");
    public int TotalPerempuan => Students.Count(x => x.Gender == "Perempuan");

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SearchCommand { get; }

    public StudentViewModel()
    {
        SaveCommand = new RelayCommand(Save);
        DeleteCommand = new RelayCommand(Delete, () => SelectedStudent != null);
        ResetCommand = new RelayCommand(Reset);
        SearchCommand = new RelayCommand(Search);

        LoadData(_repository.GetAll());
    }

    private void LoadData(IEnumerable<Student> data)
    {
        Students.Clear();
        foreach (var s in data)
            Students.Add(s);
        RefreshStatistics();
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(EditStudent.NIM) ||
            string.IsNullOrWhiteSpace(EditStudent.Nama) ||
            string.IsNullOrWhiteSpace(EditStudent.Jurusan) ||
            string.IsNullOrWhiteSpace(EditStudent.Gender))
        {
            MessageBox.Show("NIM, Nama, Jurusan, dan Gender wajib diisi.", "Validasi");
            return;
        }

        if (EditStudent.Id == 0)
            _repository.Insert(EditStudent);
        else
            _repository.Update(EditStudent);

        LoadData(_repository.GetAll());
        Reset();
    }

    private void Delete()
    {
        if (SelectedStudent == null) return;

        var confirm = MessageBox.Show($"Hapus {SelectedStudent.Nama}?", "Konfirmasi",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes) return;

        _repository.Delete(SelectedStudent.Id);
        LoadData(_repository.GetAll());
        Reset();
    }

    private void Search()
    {
        LoadData(string.IsNullOrWhiteSpace(SearchText)
            ? _repository.GetAll()
            : _repository.Search(SearchText));
    }

    private void Reset()
    {
        SelectedStudent = null;
        EditStudent = new Student();
    }

    private void RefreshStatistics()
    {
        OnPropertyChanged(nameof(TotalStudents));
        OnPropertyChanged(nameof(TotalInformatika));
        OnPropertyChanged(nameof(TotalSistemInformasi));
        OnPropertyChanged(nameof(TotalLakiLaki));
        OnPropertyChanged(nameof(TotalPerempuan));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}