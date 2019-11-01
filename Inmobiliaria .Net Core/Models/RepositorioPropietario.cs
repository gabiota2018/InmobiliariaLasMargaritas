using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
	{
		public RepositorioPropietario(IConfiguration configuration) : base(configuration)
		{
			
		}

        public int Alta(Propietario p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Propietario ( Dni,Nombre, Apellido, Telefono, Mail, Password,Borrado) " +
                    $"VALUES ('{p.Dni}','{p.Nombre}', '{p.Apellido}','{p.Telefono}','{p.Mail}','{p.Password}','{p.Borrado}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdPropietario = Convert.ToInt32(id);
                    connection.Close();
                }
            }
            return res;
        }
        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // string sql = $"DELETE FROM Propietario WHERE IdPropietario = {id}";
                string sql = $"UPDATE Propietario SET Borrado=1 WHERE IdPropietario = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Propietario SET Dni='{p.Dni}',Nombre='{p.Nombre}', Apellido='{p.Apellido}',  Telefono='{p.Telefono}', Mail='{p.Mail}', Password='{p.Password}', Borrado='{p.Borrado}' " +
                    $"WHERE IdPropietario = {p.IdPropietario}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Dni,Nombre, Apellido,  Telefono,Mail, Password" +
                    $" FROM Propietario WHERE Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Telefono = reader.GetInt32(4),
                            Mail = reader.GetString(5),
                            Password = reader.GetString(6)
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Propietario ObtenerPorId(int id)
        {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario,Dni, Nombre, Apellido,  Telefono, Mail, Password FROM Propietario" +
                    $" WHERE IdPropietario=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Telefono = reader.GetInt32(4),
                            Mail = reader.GetString(5),
                            Password = reader.GetString(6)
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Propietario ObtenerPorEmail(string email)
        {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario,Dni, Nombre, Apellido,  Telefono, Mail, Password FROM Propietario" +
                   $" WHERE Mail=@email";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Telefono = reader.GetInt32(4),
                            Mail = reader.GetString(5),
                            Password = reader.GetString(6)
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            List<Propietario> res = new List<Propietario>();
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario,Dni, Nombre, Apellido,  Telefono, Mail, Password FROM Propietario" +
                      $" WHERE Nombre LIKE %@nombre% OR Apellido LIKE %@nombre";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Propietario
                        {//otra forma de recuperar...
                            IdPropietario = (int)reader[nameof(p.IdPropietario)],
                            Dni = (int)reader[nameof(p.Dni)],
                            Nombre = reader[nameof(p.Nombre)].ToString(),
                            Apellido = reader[nameof(p.Apellido)].ToString(),
                            Telefono = (int)reader[nameof(p.Telefono)],
                            Mail = reader[nameof(p.Mail)].ToString(),
                            Password = reader[nameof(p.Password)].ToString(),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
