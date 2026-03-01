using Inventario.Data;
using Inventario.Entity;
using Inventario.Entity.Inventario.Entity;
using System;
using System.Collections.Generic;

namespace Inventario.Business
{
    public class MovInventarioBusiness
    {
       
        private readonly MovInventarioData _data = new MovInventarioData();
        
        public List<MovInventario> Listar(DateTime? inicio, DateTime? fin, string tipo, string nroDoc)
        {
            
            if (inicio > fin)
            {
                throw new Exception("La fecha de inicio no puede ser mayor a la fecha fin.");
            }

            return _data.Consultar(inicio, fin, tipo, nroDoc);
        }
        public MovInventario ObtenerPorId(string cia, string cia3, string alm, string tmov, string tdoc, string ndoc, string item)
        {
            return _data.ObtenerPorId(cia, cia3, alm, tmov, tdoc, ndoc, item);
        }

        public string SugerirCorrelativo()
        {
            return _data.ObtenerSiguienteCorrelativo();
        }
        public bool Registrar(MovInventario entidad)
        {
            
            if (entidad.CANTIDAD <= 0)
            {
                
                return false;
            }

            return _data.Insertar(entidad);
        }

        public bool Editar(MovInventario entidad)
        {
            return _data.Actualizar(entidad);
        }
        public bool Borrar(string cia, string comp, string alm, string mov, string doc, string nro, string item)
        {
            return _data.Eliminar(cia, comp, alm, mov, doc, nro, item);
        }
        public List<MasterTableRegister> ListarMaestra(string codigoMaestro)
        {
            return _data.ConsultarMaestra(codigoMaestro);
        }
    }
}