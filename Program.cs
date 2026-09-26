using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RegistroGastos
{
    internal class Program
    {
        static List<Gasto> gastos = new List<Gasto>();
        static string archivo = "gastos.csv";

        public static void Main(string[] args)
        {
            int siguienteId = 1;

            CargarGastos(ref siguienteId);

            int opcion;

            do
            {
                opcion = LeerOpcion();

                switch (opcion)
                {
                    case 1:
                        AgregarGasto(ref siguienteId);
                        break;

                    case 2:
                        ListarGastos();
                        break;

                    case 3:
                        BuscarPorCategoria();
                        break;

                    case 4:
                        GuardarGastos();
                        Console.WriteLine("Gastos guardados correctamente.");
                        Console.WriteLine("Programa finalizado.");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                if (opcion != 4)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcion != 4);
        }

        private static int LeerOpcion()
        {
            Console.Clear();

            
            Console.WriteLine("       REGISTRO DE GASTOS");
         
            Console.WriteLine("1. Agregar gasto");
            Console.WriteLine("2. Listar gastos");
            Console.WriteLine("3. Buscar por categoría");
            Console.WriteLine("4. Guardar y salir");
            Console.WriteLine("=================================");
            Console.Write("Seleccione una opción: ");

            string entrada = Console.ReadLine();

            int opcion;

            if (int.TryParse(entrada, out opcion))
            {
                return opcion;
            }

            return 0;
        }

        private static void AgregarGasto(ref int siguienteId)
        {
            Console.Clear();

            
            Console.WriteLine("          AGREGAR GASTO");
         

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("La descripción no puede estar vacía.");
                Console.Write("Descripción: ");
                descripcion = Console.ReadLine();
            }

            Console.Write("Monto: ");
            string textoMonto = Console.ReadLine();

            decimal monto;

            while (!decimal.TryParse(textoMonto, out monto) || monto <= 0)
            {
                Console.WriteLine("Ingrese un monto válido mayor que 0.");
                Console.Write("Monto: ");
                textoMonto = Console.ReadLine();
            }

            Console.Write("Categoría: ");
            string categoria = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("La categoría no puede estar vacía.");
                Console.Write("Categoría: ");
                categoria = Console.ReadLine();
            }

            Gasto nuevoGasto = new Gasto();

            nuevoGasto.Id = siguienteId;
            nuevoGasto.Descripcion = descripcion.Trim();
            nuevoGasto.Monto = monto;
            nuevoGasto.Categoria = categoria.Trim();

            gastos.Add(nuevoGasto);

            siguienteId++;

            Console.WriteLine();
            Console.WriteLine("Gasto agregado correctamente.");
            Console.WriteLine(nuevoGasto);
        }

        private static void ListarGastos()
        {
            Console.Clear();

            
            Console.WriteLine("          LISTA DE GASTOS");
           

            if (gastos.Count == 0)
            {
                Console.WriteLine("No hay gastos registrados.");
                return;
            }

            decimal total = 0;

            foreach (Gasto gasto in gastos)
            {
                Console.WriteLine(gasto);
                total += gasto.Monto;
            }

            Console.WriteLine("---------------------------------");
            Console.WriteLine($"TOTAL GASTADO: Q{total:F2}");
        }

        private static void BuscarPorCategoria()
        {
            Console.Clear();

            
            Console.WriteLine("       BUSCAR POR CATEGORÍA");
           

            Console.Write("Ingrese la categoría a buscar: ");
            string categoriaBuscada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoriaBuscada))
            {
                Console.WriteLine("La categoría no puede estar vacía.");
                return;
            }

            bool encontrado = false;

            foreach (Gasto gasto in gastos)
            {
                if (gasto.Categoria.IndexOf(
                    categoriaBuscada.Trim(),
                    StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine(gasto);
                    encontrado = true;
                }
            }

            if (!encontrado)
            {
                Console.WriteLine("No se encontraron gastos con esa categoría.");
            }
        }

        private static void GuardarGastos()
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(archivo))
                {
                    foreach (Gasto gasto in gastos)
                    {
                        escritor.WriteLine(
                            $"{gasto.Id};{gasto.Descripcion};{gasto.Monto.ToString(CultureInfo.InvariantCulture)};{gasto.Categoria}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar los gastos: " + ex.Message);
            }
        }

        private static void CargarGastos(ref int siguienteId)
        {
            if (!File.Exists(archivo))
            {
                return;
            }

            try
            {
                string[] lineas = File.ReadAllLines(archivo);

                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea))
                    {
                        continue;
                    }

                    string[] datos = linea.Split(';');

                    if (datos.Length == 4)
                    {
                        int id;
                        decimal monto;

                        if (int.TryParse(datos[0], out id) &&
                            decimal.TryParse(
                                datos[2],
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out monto))
                        {
                            Gasto gasto = new Gasto();

                            gasto.Id = id;
                            gasto.Descripcion = datos[1];
                            gasto.Monto = monto;
                            gasto.Categoria = datos[3];

                            gastos.Add(gasto);

                            if (id >= siguienteId)
                            {
                                siguienteId = id + 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar los gastos: " + ex.Message);
            }
        }
    }
}