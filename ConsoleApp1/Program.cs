using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ConsoleApp1;

class Program
{
    // Cadena de conexión a la base de datos
    public static string connectionString = "Data Source=LAB1504-30\\SQLEXPRESS;Initial Catalog=tecsup2023;User ID=userTecsup;Password=123456";

    static void Main()
    {
        // Ejemplo de cómo usar los métodos para listar trabajadores.
        List<Trabajador> trabajadoresListaObjetos = ListarTrabajadoresListaObjetos();

        foreach (var trabajador in trabajadoresListaObjetos)
        {
            Console.WriteLine($"ID: {trabajador.IdTrabajador}, Nombre: {trabajador.Nombres} {trabajador.Apellidos}, Sueldo: {trabajador.Sueldo}, Fecha de Nacimiento: {trabajador.FechaNacimiento}");
        }

        Console.WriteLine("\n--------------------------\n");

        // Llamado al método ListarTrabajadoresDataTable
        DataTable dataTable = ListarTrabajadoresDataTable();

        // Imprimir los datos desde el DataTable
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"ID: {row["IdTrabajador"]}, Nombre: {row["Nombres"]} {row["Apellidos"]}, Sueldo: {row["Sueldo"]}, Fecha de Nacimiento: {row["FechaNacimiento"]}");
        }
    }

    // De forma desconectada
    private static DataTable ListarTrabajadoresDataTable()
    {
        // Crear un DataTable para almacenar los resultados
        DataTable dataTable = new DataTable();

        // Crear una conexión a la base de datos
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Abrir la conexión
            connection.Open();

            // Consulta SQL para seleccionar datos
            string query = "SELECT * FROM Trabajadores";

            // Crear un adaptador de datos
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

            // Llenar el DataTable con los datos de la consulta
            adapter.Fill(dataTable);

            // Cerrar la conexión
            connection.Close();
        }
        return dataTable;
    }

    // De forma conectada
    private static List<Trabajador> ListarTrabajadoresListaObjetos()
    {
        List<Trabajador> trabajadores = new List<Trabajador>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Abrir la conexión
            connection.Open();

            // Consulta SQL para seleccionar datos
            string query = "SELECT IdTrabajador, Nombres, Apellidos, Sueldo, FechaNacimiento FROM Trabajadores";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Verificar si hay filas
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Leer los datos de cada fila
                            trabajadores.Add(new Trabajador
                            {
                                IdTrabajador = (int)reader["IdTrabajador"],
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Sueldo = (decimal)reader["Sueldo"],
                                FechaNacimiento = (DateTime)reader["FechaNacimiento"]
                            });
                        }
                    }
                }
            }

            // Cerrar la conexión
            connection.Close();
        }

        return trabajadores;
    }
}
