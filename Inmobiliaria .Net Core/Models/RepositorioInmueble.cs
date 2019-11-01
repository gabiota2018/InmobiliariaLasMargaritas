using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Inmueble p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inmueble (Direccion, Ambientes, Tipo, Uso, Precio, Disponible,IdPropietario,Borrado) " +
                    $"VALUES ('{p.Direccion}', '{p.Ambientes}','{p.Tipo}','{p.Uso}','{p.Precio}','{p.Disponible}','{p.IdPropietario}',0)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdInmueble = Convert.ToInt32(id);
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
                string sql = $"UPDATE Inmueble SET Borrado=1 WHERE IdInmueble = {id}";
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
        public int Disponer(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmueble SET Disponible=1 WHERE IdInmueble = {id}";
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
        public int NoDisponer(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmueble SET Disponible=0 WHERE IdInmueble = {id}";
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
        public IList<Inmueble> BuscarPorPropietario(int idPropietario)
        {
            List<Inmueble> res = new List<Inmueble>();
            Inmueble inmueble = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, Ambientes,Tipo, Uso, Precio, Disponible, i.IdPropietario,p.Dni,p.Nombre,p.Apellido" +
                    $" FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario" +
                    $" WHERE i.IdPropietario=@idPropietario AND i.Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idPropietario", SqlDbType.Int).Value = idPropietario;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                       inmueble = new Inmueble
                        {
                           IdInmueble = reader.GetInt32(0),
                           Direccion = reader.GetString(1),
                           Ambientes = reader.GetInt32(2),
                           Tipo = reader.GetString(3),
                           Uso = reader.GetString(4),
                           Precio = reader.GetDecimal(5),
                           Disponible = reader.GetInt32(6),
                           IdPropietario = reader.GetInt32(7),
                           propietario = new Propietario
                           {
                               IdPropietario = reader.GetInt32(7),
                               Dni = reader.GetInt32(8),
                               Nombre = reader.GetString(9),
                               Apellido = reader.GetString(10),
                           }
                       };
                        res.Add(inmueble);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inmueble p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmueble SET Direccion=@direccion, Ambientes=@ambientes, Tipo=@tipo, Uso=@uso, Precio=@precio, Disponible=@disponible, IdPropietario=@IdPropietario " +
                    $"WHERE IdInmueble = {p.IdInmueble} AND Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@direccion", SqlDbType.VarChar).Value = p.Direccion;
                    command.Parameters.Add("@ambientes", SqlDbType.Int).Value = p.Ambientes;
                    command.Parameters.Add("@tipo", SqlDbType.VarChar).Value = p.Tipo;
                    command.Parameters.Add("@uso", SqlDbType.VarChar).Value = p.Uso;
                    command.Parameters.Add("@precio", SqlDbType.Decimal).Value =p.Precio;
                    command.Parameters.Add("@disponible", SqlDbType.Int).Value = p.Disponible;
                    command.Parameters.Add("@IdPropietario", SqlDbType.Int).Value = p.IdPropietario;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble inmueble = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, Ambientes,Tipo, Uso, Precio, Disponible, IdPropietario" +
                   $" FROM Inmueble " +
                   $"WHERE IdInmueble = @id AND Borrado=0";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Uso = reader.GetString(4),
                            Precio = reader.GetDecimal(5),
                            Disponible = reader.GetInt32(6),
                            IdPropietario = reader.GetInt32(7),
                           
                        };
                    }
                    connection.Close();
                }
            }
            return inmueble;
        }
        public decimal DevolverPrecio(int id)
        {
            decimal precio = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT  Precio FROM Inmueble WHERE IdInmueble = @id AND Borrado=0";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        precio = reader.GetDecimal(0);
                    }
                    connection.Close();
                }
            }
            return precio;
        }
        public IList<Inmueble> ObtenerTodos()
        {
            List<Inmueble> res = new List<Inmueble>();
            Inmueble inmueble = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, Ambientes,Tipo, Uso, Precio, Disponible, p.IdPropietario,p.Dni,p.Nombre,p.Apellido" +
                    $" FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario" +
                    $" WHERE i.Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Uso = reader.GetString(4),
                            Precio = reader.GetDecimal(5),
                            Disponible = reader.GetInt32(6),
                            IdPropietario = reader.GetInt32(7),
                            propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Dni = reader.GetInt32(8),
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10),
                            }
                        };
                        res.Add(inmueble);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        public IList<Inmueble> ObtenerDisponibles()
        {
            List<Inmueble> res = new List<Inmueble>();
            Inmueble inmueble = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, Ambientes,Tipo, Uso, Precio, Disponible, p.IdPropietario,p.Dni,p.Nombre,p.Apellido" +
                    $" FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario" +
                    $" WHERE i.Disponible=1 AND i.Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Tipo = reader.GetString(3),
                            Uso = reader.GetString(4),
                            Precio = reader.GetDecimal(5),
                            Disponible = reader.GetInt32(6),
                            IdPropietario = reader.GetInt32(7),
                            propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Dni = reader.GetInt32(8),
                                Nombre = reader.GetString(9),
                                Apellido = reader.GetString(10),
                            }
                        };
                        res.Add(inmueble);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
