using System;

namespace Inventario.Entity
{
    public class MovInventario
    {
        public string COD_CIA { get; set; }
        public string COMPANIA_VENTA_3 { get; set; }
        public string ALMACEN_VENTA { get; set; }
        public string TIPO_MOVIMIENTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NRO_DOCUMENTO { get; set; }
        public string COD_ITEM_2 { get; set; }
        public string PROVEEDOR { get; set; }
        public string ALMACEN_DESTINO { get; set; }
        public int CANTIDAD { get; set; }
        public DateTime FECHA_TRANSACCION { get; set; }
        public string NombreCompania { get; set; }
        public string NombreAlmacen { get; set; }
        public string NombreMovimiento { get; set; }
        public string NombreTipoDoc { get; set; }
        public string NombreItem { get; set; }
        public string NombreProveedor { get; set; }

        public bool EsEdicion { get; set; }
    }
    namespace Inventario.Entity
    {
        public class MasterTableRegister
        {
            public string CodigoMaestro { get; set; }
            public string CodigoDocumento { get; set; }
            public string DescripcionDocumento { get; set; }
        }
    }
}