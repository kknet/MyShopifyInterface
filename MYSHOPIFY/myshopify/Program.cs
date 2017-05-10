using System;
using System.Linq;

namespace myshopify
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Controller controlador = new Controller();
                long ORDEN = Convert.ToInt64(args.Count() > 0 ? args[0].ToString() : "-1");
                string OPCION = args.Count() > 1 ? args[1] : "";
                int NumeroRegistrosExtraidos = 0;
                if (ORDEN >= 0)
                {
                    if (OPCION == "PENDIENTE")
                    {
                        if (controlador.ExtraerRegistros(ORDEN, "PENDIENTE", ref NumeroRegistrosExtraidos))
                        {
                            Console.WriteLine("Se extrajo la orden pendiente no. " + (ORDEN));
                        }
                        else
                        {
                            Console.WriteLine("No se pudo extraer la orden pendiente no. " + (ORDEN));
                        }
                    }
                    else
                    {
                        if (controlador.ExtraerRegistros(ORDEN, "MASIVA", ref NumeroRegistrosExtraidos))
                        {
                            ORDEN++;
                            Console.WriteLine("Se extrajeron " + NumeroRegistrosExtraidos + " registros a partir de la orden " + ORDEN);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("La orden no fue introducida como parámetro");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("[ERROR]: " + exc.Message);
            }
        }
    }
}
