using Microsoft.Data.SqlClient;
using StudentManager.Models;

namespace StudentManager.Data;

public class StudentRepository
{
    private readonly string connectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

    private const string SelectColumns = "SELECT Id, NIM, Nama, Jurusan, Gender, Email FROM Students ";

    private static List<Student> ReadStudents(SqlCommand command)
    {
        var students = new List<Student>();
        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                NIM = reader["NIM"].ToString()!,
                Nama = reader["Nama"].ToString()!,
                Jurusan = reader["Jurusan"].ToString()!,
                Gender = reader["Gender"].ToString()!,
                Email = reader["Email"].ToString()!
            });
        }
        return students;
    }

    public List<Student> GetAll()
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(SelectColumns + "ORDER BY Id DESC", connection);
        connection.Open();
        return ReadStudents(command);
    }

    public List<Student> Search(string keyword)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(
            SelectColumns +
            "WHERE NIM LIKE @Keyword OR Nama LIKE @Keyword OR Jurusan LIKE @Keyword " +
            "ORDER BY Id DESC", connection);
        command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
        connection.Open();
        return ReadStudents(command);
    }

    public void Insert(Student s)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Students (NIM, Nama, Jurusan, Gender, Email) " +
            "VALUES (@NIM, @Nama, @Jurusan, @Gender, @Email)", connection);
        AddParams(command, s);
        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Update(Student s)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(
            "UPDATE Students SET NIM=@NIM, Nama=@Nama, Jurusan=@Jurusan, " +
            "Gender=@Gender, Email=@Email WHERE Id=@Id", connection);
        command.Parameters.AddWithValue("@Id", s.Id);
        AddParams(command, s);
        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand("DELETE FROM Students WHERE Id=@Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();
        command.ExecuteNonQuery();
    }

    private static void AddParams(SqlCommand c, Student s)
    {
        c.Parameters.AddWithValue("@NIM", s.NIM);
        c.Parameters.AddWithValue("@Nama", s.Nama);
        c.Parameters.AddWithValue("@Jurusan", s.Jurusan ?? "");
        c.Parameters.AddWithValue("@Gender", s.Gender ?? "");
        c.Parameters.AddWithValue("@Email", s.Email ?? "");
    }
}