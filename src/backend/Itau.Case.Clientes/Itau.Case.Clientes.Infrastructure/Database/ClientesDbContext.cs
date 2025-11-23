using MySql.Data.MySqlClient;

namespace Itau.Case.Clientes.Infrastructure.Data;

public class ClientesDbContext
{
    private readonly string _connectionString;

    public ClientesDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}
