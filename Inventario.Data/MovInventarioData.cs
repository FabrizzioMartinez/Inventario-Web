using Inventario.Entity;
using Inventario.Entity.Inventario.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Inventario.Data
{
    public class MovInventarioData
    {
        private readonly string _conexion = ConfigurationManager.ConnectionStrings["CnxInventario"].ConnectionString;

        #region CORRELATIVO
        public string ObtenerSiguienteCorrelativo()
        {
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_MovInventario_ObtenerSiguienteCorrelativo", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cn.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }
        #endregion

        #region CONSULTAS
        public List<MovInventario> Consultar(DateTime? inicio, DateTime? fin, string tipo, string nroDoc)
        {
            var lista = new List<MovInventario>();
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_MovInventario_Consultar", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FechaInicio", (object)inicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaFin", (object)fin ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoMovimiento", (object)tipo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NroDocumento", (object)nroDoc ?? DBNull.Value);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new MovInventario
                        {
                            COD_CIA = dr["COD_CIA"].ToString(),
                            COMPANIA_VENTA_3 = dr["COMPANIA_VENTA_3"].ToString(),
                            ALMACEN_VENTA = dr["ALMACEN_VENTA"].ToString(),
                            TIPO_MOVIMIENTO = dr["TIPO_MOVIMIENTO"].ToString(),
                            TIPO_DOCUMENTO = dr["TIPO_DOCUMENTO"].ToString(),
                            NRO_DOCUMENTO = dr["NRO_DOCUMENTO"].ToString(),
                            COD_ITEM_2 = dr["COD_ITEM_2"].ToString(),
                            PROVEEDOR = dr["PROVEEDOR"].ToString(),
                            CANTIDAD = Convert.ToInt32(dr["CANTIDAD"]),
                            FECHA_TRANSACCION = Convert.ToDateTime(dr["FECHA_TRANSACCION"]),
                            NombreCompania = dr["NombreCompania"].ToString(),
                            NombreAlmacen = dr["NombreAlmacen"].ToString(),
                            NombreMovimiento = dr["NombreMovimiento"].ToString(),
                            NombreItem = dr["NombreItem"].ToString(),
                            NombreProveedor = dr["NombreProveedor"].ToString(),
                            NombreTipoDoc = dr["NombreTipoDoc"].ToString(),
                        });
                    }
                }
            }
            return lista;
        }

        public MovInventario ObtenerPorId(string cia, string cia3, string alm, string tmov, string tdoc, string ndoc, string item)
        {
            MovInventario ent = null;
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_MovInventario_ObtenerPorId", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@COD_CIA", cia);
                cmd.Parameters.AddWithValue("@COMPANIA_VENTA_3", cia3);
                cmd.Parameters.AddWithValue("@ALMACEN_VENTA", alm);
                cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", tmov);
                cmd.Parameters.AddWithValue("@TIPO_DOCUMENTO", tdoc);
                cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", ndoc);
                cmd.Parameters.AddWithValue("@COD_ITEM_2", item);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ent = new MovInventario
                        {
                            COD_CIA = dr["COD_CIA"].ToString(),
                            COMPANIA_VENTA_3 = dr["COMPANIA_VENTA_3"].ToString(),
                            ALMACEN_VENTA = dr["ALMACEN_VENTA"].ToString(),
                            TIPO_MOVIMIENTO = dr["TIPO_MOVIMIENTO"].ToString(),
                            TIPO_DOCUMENTO = dr["TIPO_DOCUMENTO"].ToString(),
                            NRO_DOCUMENTO = dr["NRO_DOCUMENTO"].ToString(),
                            COD_ITEM_2 = dr["COD_ITEM_2"].ToString(),
                            PROVEEDOR = dr["PROVEEDOR"].ToString(),
                            CANTIDAD = Convert.ToInt32(dr["CANTIDAD"]),
                            FECHA_TRANSACCION = Convert.ToDateTime(dr["FECHA_TRANSACCION"]),
                            ALMACEN_DESTINO = dr["ALMACEN_DESTINO"].ToString()
                        };
                    }
                }
            }
            return ent;
        }
        #endregion

        #region CRUD
        public bool Insertar(MovInventario ent)
        {
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_MovInventario_Insertar", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@COD_CIA", ent.COD_CIA);
                cmd.Parameters.AddWithValue("@COMPANIA_VENTA_3", ent.COMPANIA_VENTA_3);
                cmd.Parameters.AddWithValue("@ALMACEN_VENTA", ent.ALMACEN_VENTA);
                cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", ent.TIPO_MOVIMIENTO);
                cmd.Parameters.AddWithValue("@TIPO_DOCUMENTO", ent.TIPO_DOCUMENTO);
                cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", ent.NRO_DOCUMENTO);
                cmd.Parameters.AddWithValue("@COD_ITEM_2", ent.COD_ITEM_2);
                cmd.Parameters.AddWithValue("@PROVEEDOR", (object)ent.PROVEEDOR ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ALMACEN_DESTINO", (object)ent.ALMACEN_DESTINO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CANTIDAD", (object)ent.CANTIDAD ?? DBNull.Value);

                var fechaParaSql = (ent.FECHA_TRANSACCION == DateTime.MinValue || ent.FECHA_TRANSACCION.Year < 1753)
                                    ? DateTime.Now
                                    : ent.FECHA_TRANSACCION;
                cmd.Parameters.AddWithValue("@FECHA_TRANSACCION", fechaParaSql);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Actualizar(MovInventario ent)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_MovInventario_Actualizar", cn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@COD_CIA", ent.COD_CIA);
                    cmd.Parameters.AddWithValue("@COMPANIA_VENTA_3", ent.COMPANIA_VENTA_3);
                    cmd.Parameters.AddWithValue("@ALMACEN_VENTA", ent.ALMACEN_VENTA);
                    cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", ent.TIPO_MOVIMIENTO);
                    cmd.Parameters.AddWithValue("@TIPO_DOCUMENTO", ent.TIPO_DOCUMENTO);
                    cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", ent.NRO_DOCUMENTO);
                    cmd.Parameters.AddWithValue("@COD_ITEM_2", ent.COD_ITEM_2);

                    cmd.Parameters.AddWithValue("@PROVEEDOR", string.IsNullOrEmpty(ent.PROVEEDOR) ? (object)DBNull.Value : ent.PROVEEDOR);
                    cmd.Parameters.AddWithValue("@ALMACEN_DESTINO", string.IsNullOrEmpty(ent.ALMACEN_DESTINO) ? (object)DBNull.Value : ent.ALMACEN_DESTINO);
                    cmd.Parameters.AddWithValue("@CANTIDAD", ent.CANTIDAD);

                    cmd.Parameters.AddWithValue("@FECHA_TRANSACCION", ent.FECHA_TRANSACCION == DateTime.MinValue ? (object)DBNull.Value : ent.FECHA_TRANSACCION);

                    cn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Eliminar(string cia, string cia3, string alm, string tmov, string tdoc, string ndoc, string item)
        {
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_MovInventario_Eliminar", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@COD_CIA", cia);
                cmd.Parameters.AddWithValue("@COMPANIA_VENTA_3", cia3);
                cmd.Parameters.AddWithValue("@ALMACEN_VENTA", alm);
                cmd.Parameters.AddWithValue("@TIPO_MOVIMIENTO", tmov);
                cmd.Parameters.AddWithValue("@TIPO_DOCUMENTO", tdoc);
                cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", ndoc);
                cmd.Parameters.AddWithValue("@COD_ITEM_2", item);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region MAESTRAS
        public List<MasterTableRegister> ConsultarMaestra(string codigoMaestro)
        {
            var lista = new List<MasterTableRegister>();
            using (SqlConnection cn = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_MasterTable_ConsultarPorCodigo", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CodigoMaestro", codigoMaestro);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new MasterTableRegister
                        {
                            CodigoMaestro = dr["CodigoMaestro"].ToString(),
                            CodigoDocumento = dr["CodigoDocumento"].ToString(),
                            DescripcionDocumento = dr["DescripcionDocumento"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        #endregion
    }
}