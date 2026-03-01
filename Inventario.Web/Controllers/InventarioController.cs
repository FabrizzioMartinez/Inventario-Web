using Inventario.Business;
using Inventario.Entity;
using Inventario.Entity.Inventario.Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Inventario.Web.Controllers
{
    public class InventarioController : Controller
    {
        private readonly MovInventarioBusiness _bus = new MovInventarioBusiness();

        #region LISTADO PRINCIPAL
        public ActionResult Index(DateTime? inicio, DateTime? fin, string tipo, string nroDoc)
        {
            try
            {
                CargarCombosEstandar();

                var fechaInicio = inicio ?? DateTime.Now.AddMonths(-1);
                var fechaFin = fin ?? DateTime.Now;

                var lista = _bus.Listar(fechaInicio, fechaFin, tipo, nroDoc);
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<MovInventario>());
            }
        }
        #endregion

        #region OBTENER CORRELATIVO
        [HttpGet]
        public JsonResult GetCorrelativo()
        {
            string nuevoId = _bus.SugerirCorrelativo();
            return Json(new { correlativo = nuevoId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region NUEVO
        [HttpGet]
        public ActionResult Nuevo()
        {
            try
            {
                var modelo = new MovInventario
                {
                    COD_CIA = _bus.SugerirCorrelativo()
                };

                TempData["CORRELATIVO_NUEVO"] = modelo.COD_CIA;

                CargarCombosEstandar(modelo);

                ViewBag.EsEdicion = false;

                return PartialView("_Formulario", modelo);
            }
            catch (Exception ex)
            {
                return Content("ERROR NUEVO: " + ex.ToString());
            }
        }
        #endregion

        #region EDITAR
        [HttpGet]
        public ActionResult Editar(string cia, string cia3, string alm, string tmov, string tdoc, string ndoc, string item)
        {
            try
            {
                if (string.IsNullOrEmpty(cia) || string.IsNullOrEmpty(cia3) || string.IsNullOrEmpty(alm) ||
                    string.IsNullOrEmpty(tmov) || string.IsNullOrEmpty(tdoc) || string.IsNullOrEmpty(ndoc) ||
                    string.IsNullOrEmpty(item))
                {
                    return Content("ERROR: Parámetros incompletos para editar.");
                }

                var modelo = _bus.ObtenerPorId(cia, cia3, alm, tmov, tdoc, ndoc, item);

                if (modelo == null)
                {
                    return Content("ERROR: No se encontró el registro en base de datos.");
                }

                CargarCombosEstandar(modelo);
                modelo.EsEdicion = true;

                return PartialView("_Formulario", modelo);
            }
            catch (Exception ex)
            {
                return Content("ERROR EN EDITAR: " + ex.Message);
            }
        }
        #endregion

        #region GUARDAR (CREAR / EDITAR)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Guardar(MovInventario model, bool esEdicion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Modelo inválido. Verifique los campos." });
                }

                bool resultado;

                if (esEdicion)
                {
                    resultado = _bus.Editar(model);
                    if (!resultado)
                        return Json(new { success = false, message = "No se encontró el registro para actualizar. Verifique la llave." });
                }
                else
                {
                    resultado = _bus.Registrar(model);
                    if (!resultado)
                        return Json(new { success = false, message = "No se pudo registrar el movimiento." });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region ELIMINAR
        [HttpPost]
        public JsonResult Delete(string cia, string cia3, string alm, string tmov, string tdoc, string ndoc, string item)
        {
            try
            {
                bool eliminado = _bus.Borrar(cia, cia3, alm, tmov, tdoc, ndoc, item);

                if (!eliminado)
                    return Json(new { success = false, message = "No se pudo eliminar el registro." });

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region MÉTODO PRIVADO PARA CARGAR COMBOS
        private void CargarCombosEstandar(MovInventario modelo = null)
        {
            var ciaList = _bus.ListarMaestra("CIAVT") ?? new List<MasterTableRegister>();
            var almList = _bus.ListarMaestra("ALMAC") ?? new List<MasterTableRegister>();
            var movList = _bus.ListarMaestra("TPMOV") ?? new List<MasterTableRegister>();
            var docList = _bus.ListarMaestra("TPDOC") ?? new List<MasterTableRegister>();
            var itemList = _bus.ListarMaestra("ITEMS") ?? new List<MasterTableRegister>();
            var provList = _bus.ListarMaestra("PROVE") ?? new List<MasterTableRegister>();

            ViewBag.Ciavt_List = new SelectList(ciaList, "CodigoDocumento", "DescripcionDocumento", modelo?.COMPANIA_VENTA_3);
            ViewBag.Almacenes_List = new SelectList(almList, "CodigoDocumento", "DescripcionDocumento", modelo?.ALMACEN_VENTA);
            ViewBag.TiposMov_List = new SelectList(movList, "CodigoDocumento", "DescripcionDocumento", modelo?.TIPO_MOVIMIENTO);
            ViewBag.TiposDoc_List = new SelectList(docList, "CodigoDocumento", "DescripcionDocumento", modelo?.TIPO_DOCUMENTO);
            ViewBag.Items_List = new SelectList(itemList, "CodigoDocumento", "DescripcionDocumento", modelo?.COD_ITEM_2);
            ViewBag.Proveedores_List = new SelectList(provList, "CodigoDocumento", "DescripcionDocumento", modelo?.PROVEEDOR);
        }
        #endregion
    }
}