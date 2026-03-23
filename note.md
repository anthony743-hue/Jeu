# Classe de Connection 
```csharp
using Npgsql;

public class FactoryConnection
{
    private static string _connectionString;

    public FactoryConnection(string server, string database, string user, string password, int port = 5432)
    {
        _connectionString = $"Host={server};Port={port};Database={database};Username={user};Password={password}";
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
```

# Exemple de DAO
```csharp
public class UserDAO
{
    private FactoryConnection _factory;

    public UserDAO(FactoryConnection factory)
    {
        _factory = factory;
    }

    public void InsertUser(string name, string email)
    {
        using (var connection = _factory.GetConnection())
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand("INSERT INTO users (name, email) VALUES (@name, @email)", connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        using (var connection = _factory.GetConnection())
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand("SELECT id, name, email FROM users", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (int)reader["id"],
                            Name = reader["name"].ToString(),
                            Email = reader["email"].ToString()
                        });
                    }
                }
            }
        }
        return users;
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```