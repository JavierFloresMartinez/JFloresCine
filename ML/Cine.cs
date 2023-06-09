namespace ML
{
    public class Cine
    {
        public int IdCine { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Venta { get; set; }
        public Zona Zona { get; set; }
        public List<Object> Cines { get; set; }
        public List<Object> Porcentajes { get; set; }
        public float Porcentaje { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }

    }
}