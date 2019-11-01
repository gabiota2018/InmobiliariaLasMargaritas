using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class RepositorioAlquiler:RepositorioBase,IRepositorioAlquiler
    {
        public RepositorioAlquiler(IConfiguration configuracion) : base(configuracion)
        {
           
        }

        public int Alta(Alquiler p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Alquiler (Precio,Fecha_inicio, Fecha_fin, IdInquilino, IdInmueble,Borrado) " +
                    $"VALUES ('{p.Precio}','{p.Fecha_inicio}','{p.Fecha_fin}','{p.IdInquilino}','{p.IdInmueble}',0)";
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
            DateTime hoy = DateTime.Now;
            String fecha = Convert.ToString(hoy);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Alquiler SET Fecha_fin=fecha WHERE IdAlquiler = {id}";
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
       public int Modificacion(Alquiler p)
        {
            throw new NotImplementedException();
        }

        public Alquiler ObtenerPorId(int id)
        {
            Alquiler alquiler = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdAlquiler, a.Precio, Fecha_inicio, Fecha_fin, p.IdInquilino,p.Nombre,p.Apellido,i.IdInmueble,i.Direccion,i.Tipo,i.Uso,i.Precio " +
                    $" FROM Alquiler a INNER JOIN Inquilino p ON a.IdInquilino=p.IdInquilino INNER JOIN Inmueble i ON a.IdInmueble = i.IdInmueble" +
                   $" WHERE a.IdAlquiler=@id AND a.Borrado=0";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        alquiler = new Alquiler
                        {
                            IdAlquiler = reader.GetInt32(0),
                            Precio = reader.GetDecimal(1),
                            Fecha_inicio = reader.GetString(2),
                            Fecha_fin = reader.GetString(3),
                            IdInquilino = reader.GetInt32(4),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),
                            },
                            IdInmueble = reader.GetInt32(7),
                            inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(7),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Uso = reader.GetString(10),
                                Precio = reader.GetDecimal(11),
                            }

                        };
                    }
                    connection.Close();
                }
            }
            return alquiler;
        }

        public IList<Alquiler> ObtenerPorIdInmueble(int id)
        {
            IList<Alquiler> res = new List<Alquiler>();
            Alquiler a = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
              string sql = $"SELECT IdAlquiler, a.Precio, Fecha_inicio, Fecha_fin, p.IdInquilino,p.Nombre,p.Apellido,i.IdInmueble,i.Direccion,i.Tipo,i.Uso,i.Precio " +
                    $" FROM Alquiler a INNER JOIN Inquilino p ON a.IdInquilino=p.IdInquilino INNER JOIN Inmueble i ON a.IdInmueble = i.IdInmueble WHERE i.IdInmueble=@id AND a.Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        a = new Alquiler
                        {
                            IdAlquiler = reader.GetInt32(0),
                            Precio = reader.GetDecimal(1),
                            Fecha_inicio = reader.GetString(2),
                            Fecha_fin = reader.GetString(3),
                            IdInquilino = reader.GetInt32(4),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),
                            },
                            IdInmueble = reader.GetInt32(7),
                            inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(7),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Uso = reader.GetString(10),
                                Precio = reader.GetDecimal(11),
                            }
                        };
                        res.Add(a);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Alquiler> ObtenerPorIdInquilino(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Alquiler> ObtenerTodos()
        {
            IList<Alquiler> res = new List<Alquiler>();
            Alquiler a = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdAlquiler, a.Precio, Fecha_inicio, Fecha_fin, p.IdInquilino,p.Nombre,p.Apellido,i.IdInmueble,i.Direccion,i.Tipo,i.Uso,i.Precio " +
                     $" FROM Alquiler a INNER JOIN Inquilino p ON a.IdInquilino=p.IdInquilino INNER JOIN Inmueble i ON a.IdInmueble = i.IdInmueble" +
                    $" WHERE a.Borrado=0";

               using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        a = new Alquiler
                        {
                            IdAlquiler = reader.GetInt32(0),
                            Precio = reader.GetDecimal(1),
                            Fecha_inicio = reader.GetString(2),
                            Fecha_fin = reader.GetString(3),
                            IdInquilino = reader.GetInt32(4),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre= reader.GetString(5),
                                Apellido= reader.GetString(6),
                            },
                            IdInmueble = reader.GetInt32(7),
                            inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(7),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Uso = reader.GetString(10),
                                Precio = reader.GetDecimal(11),
                            }
                        };
                        res.Add(a);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        public IList<Alquiler> ObtenerVigentes()
        {
            IList<Alquiler> res = new List<Alquiler>();
            IList<Alquiler> rta = new List<Alquiler>();
            Alquiler a = null;
            DateTime hoy = DateTime.Now;
            DateTime fecha = new DateTime();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdAlquiler, a.Precio, Fecha_inicio, Fecha_fin, p.IdInquilino,p.Nombre,p.Apellido,i.IdInmueble,i.Direccion,i.Tipo,i.Uso,i.Precio " +
                     $" FROM Alquiler a INNER JOIN Inquilino p ON a.IdInquilino=p.IdInquilino INNER JOIN Inmueble i ON a.IdInmueble = i.IdInmueble" +
                    $" WHERE  a.Borrado=0";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        a = new Alquiler
                        {
                            IdAlquiler = reader.GetInt32(0),
                            Precio = reader.GetDecimal(1),
                            Fecha_inicio = reader.GetString(2),
                            Fecha_fin = reader.GetString(3),
                            IdInquilino = reader.GetInt32(4),
                            inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(4),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),
                            },
                            IdInmueble = reader.GetInt32(7),
                            inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(7),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Uso = reader.GetString(10),
                                Precio = reader.GetDecimal(11),
                            }
                        };
                        res.Add(a);
                    }
                    connection.Close();
                    //--------------------------------------------------------------
                    foreach (Alquiler aux in res) 
                    {
                        fecha = Convert.ToDateTime(aux.Fecha_fin);
                        if (fecha > hoy)
                            rta.Add(aux);
                    }
                        
                }
            }
            return rta;
        }

       
    }
}
