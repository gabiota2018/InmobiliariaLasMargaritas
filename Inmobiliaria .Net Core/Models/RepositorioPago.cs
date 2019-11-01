using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class RepositorioPago:RepositorioBase,IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuracion) : base(configuracion)
        {
           
        }

        public int Alta(Pago p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Pago (NroPago,IdAlquiler, Fecha, Importe,Borrado) " +
                    $"VALUES ('{p.NroPago}','{p.IdAlquiler}','{p.Fecha}','{p.Importe}',0)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdPago = Convert.ToInt32(id);
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
                //string sql = $"DELETE FROM Inquilino WHERE IdInquilino = {id}";
                string sql = $"UPDATE Pago SET Borrado=1 WHERE IdPago = {id}";
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

        public int Modificacion(Pago p)
        {
            throw new NotImplementedException();
        }

        public Pago ObtenerPorId(int id)
        {
           Pago p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago, NroPago,IdAlquiler,Fecha,Importe " +
                    $" FROM pago WHERE IdPago=@id AND Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            NroPago = reader.GetInt32(1),
                            IdAlquiler = reader.GetInt32(2),
                            Fecha = reader.GetString(3),
                            Importe = reader.GetDecimal(4),
                        };
                   }
                    connection.Close();
                }
            }
            return p;
        }

        
        public int ObtenerUltimoNro(int id)
        {
            Pago p = new Pago();
            p.NroPago = 0;
            int contador = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT COUNT(*) FROM Pago WHERE IdAlquiler=@id AND  Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read() != null)
                    {
                        contador = reader.GetInt32(0);
                    }
                    connection.Close();
                }
                //-----------------------------------------------------------------
                if(contador>0)
                {
                    sql = $"SELECT MAX(NroPago) FROM Pago WHERE IdAlquiler=@id AND  Borrado=0";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        var reader1 = command.ExecuteReader();
                        if (reader1.Read())
                        {
                            p.NroPago = reader1.GetInt32(0);
                        }
                        connection.Close();
                    }
                }
            }
            return p.NroPago;
        }
        public IList<Pago> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public IList<Pago> ObtenerPorIdAlquiler(int id)
        {
            IList<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPago, NroPago,IdAlquiler,Fecha,Importe " +
                    $" FROM pago WHERE IdAlquiler=@id AND Borrado=0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            IdPago = reader.GetInt32(0),
                            NroPago = reader.GetInt32(1),
                            IdAlquiler = reader.GetInt32(2),
                            Fecha = reader.GetString(3),
                            Importe = reader.GetDecimal(4),
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
