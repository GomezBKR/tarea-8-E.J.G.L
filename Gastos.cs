namespace RegistroGastos
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Descripción: {Descripcion} | Monto: Q{Monto:F2} | Categoría: {Categoria}";
        }
    }
}
