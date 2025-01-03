using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivoFijoAPI.Util;
using Npgsql;
using TsaakAPI.Entities;

using ConnectionTools.DBTools;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace TsaakAPI.Model.DAO
{
    public class EnfermedadCronicaDao
    {
         private ISqlTools _sqlTools;
        
        

         public EnfermedadCronicaDao(string connectionString) 
        {
            this._sqlTools = new SQLTools(connectionString);
            
        }

 
      

        public async Task<ResultOperation<VMCatalog>> GetByIdAsyncnow(int id)
        {
            ResultOperation<VMCatalog> resultOperation = new ResultOperation<VMCatalog>();

            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync("schemasye.fn_get_enfermedad_cronica", new ParameterPGsql[]{
                    new ParameterPGsql("p_id_enf_cronica", NpgsqlTypes.NpgsqlDbType.Integer,id),
                });
            RespuestaBD respuestaBD = await respuestaBDTask;
            resultOperation.Success = !respuestaBD.ExisteError;
            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                 && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {

                    VMCatalog aux = new VMCatalog
                    {
                        Id = (int)respuestaBD.Data.Tables[0].Rows[0]["id_enf_cronica"],
                        Nombre = respuestaBD.Data.Tables[0].Rows[0]["nombre"].ToString(),
                        Descripcion = respuestaBD.Data.Tables[0].Rows[0]["descripcion"].ToString(),
                        Estado = respuestaBD.Data.Tables[0].Rows[0]["estado"] as bool?,

                    }; 

                    resultOperation.Result = aux; 
                }
                else
                {
                    resultOperation.Result = null;
                    resultOperation.Success = false;
                    resultOperation.AddErrorMessage($"No fue posible regresar el registro de la tabla. {respuestaBD.Detail}");
                }
                
            }
            else
            {
                //TODO Agregar error en el log             
                if (respuestaBD.ExisteError)
                    Console.WriteLine("Error {0} - {1} - {2} - {3}", respuestaBD.ExisteError, respuestaBD.Mensaje, respuestaBD.CodeSqlError, respuestaBD.Detail);
                throw new Exception(respuestaBD.Mensaje);
            }
            return resultOperation;


        }       
///////recien agregado
/// <summary>
/// 
/// 




public async Task<ResultOperation<List<VMCatalog>>> GetAllperson()
        {
            ResultOperation<List<VMCatalog>> resultOperation = new ResultOperation<List<VMCatalog>>();

            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync("schemasye.fn_getallperson_enfermedad_cronica"
                );
            RespuestaBD respuestaBD = await respuestaBDTask;
            resultOperation.Success = !respuestaBD.ExisteError;
            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                 && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {
                     List<VMCatalog> catalogos = new List<VMCatalog>();

                      foreach (DataRow row in respuestaBD.Data.Tables[0].Rows)
                    {
                    VMCatalog catalogo = new VMCatalog
                    {
                        Id = (int)row["id_enf_cronica"],
                            Nombre = row["nombre"].ToString(),
                            Descripcion = row["descripcion"].ToString(),
                            Estado = (bool?)row["estado"]

                    }; 
                    catalogos.Add(catalogo);
                    }
                    resultOperation.Result = catalogos; 

  }
                else
                {
                    resultOperation.Result = null;
                    resultOperation.Success = false;
                    resultOperation.AddErrorMessage($"No se encontraron registros en la tabla.");

                }
            }
                  else
            {
                           
                if (respuestaBD.ExisteError)
                    Console.WriteLine("Error {0} - {1} - {2} - {3}", respuestaBD.ExisteError, respuestaBD.Mensaje, respuestaBD.CodeSqlError, respuestaBD.Detail);
                throw new Exception(respuestaBD.Mensaje);
            }
            return resultOperation;

            }
/*
        public async Task<ResultOperation<int>> AddAsync([FromBody] EnfermedadCronica enfermedad)
        {
            ResultOperation<int> resultOperation = new ResultOperation<int>();

            try
            {
                // Llama la función 
                Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(
                    "schemasye.fn_add_enfermedad_cronica",
                    new ParameterPGsql[]
                    {
                new ParameterPGsql("e_id_enf_cronica", NpgsqlTypes.NpgsqlDbType.Integer, enfermedad.id_enf_cronica),
                new ParameterPGsql("e_nombre", NpgsqlTypes.NpgsqlDbType.Varchar, enfermedad.nombre),
                new ParameterPGsql("e_descripcion", NpgsqlTypes.NpgsqlDbType.Varchar, enfermedad.descripcion),
                new ParameterPGsql("e_fecha_registro", NpgsqlTypes.NpgsqlDbType.Date, enfermedad.fecha_registro),
                new ParameterPGsql("e_fecha_inicio", NpgsqlTypes.NpgsqlDbType.Date, enfermedad.fecha_inicio),
                new ParameterPGsql("e_estado", NpgsqlTypes.NpgsqlDbType.Boolean, enfermedad.estado),
                new ParameterPGsql("e_fecha_actualizacion", NpgsqlTypes.NpgsqlDbType.Date, enfermedad.fecha_actualizacion)
                    }
                );

                RespuestaBD respuestaBD = await respuestaBDTask;

                // Verifica si la operación fue exitosa
                resultOperation.Success = !respuestaBD.ExisteError;

                if (!respuestaBD.ExisteError)
                {
                   
                    resultOperation.Result = (int)respuestaBD.Data.Tables[0].Rows[0]["id_enf_cronica"];
                }
                else
                {
                    
                    resultOperation.Result = 0;
                    resultOperation.Success = false;
                    resultOperation.AddErrorMessage($"Error al insertar el registro en la base de datos: {respuestaBD.Mensaje}");
                }
            }
            catch (Exception ex)
            {
                
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Error al insertar el registro en la base: {ex.Message}");
            }

            return resultOperation;
        }



        public async Task<ResultOperation<VMCatalog>> UpdAsync(EnfermedadCronica enfermedad, int id)
        {
            ResultOperation<VMCatalog> resultOperation = new ResultOperation<VMCatalog>();
            enfermedad.id_enf_cronica = id;

            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(
                "schemasye.fn_update_enfermedad_cronica",
                new ParameterPGsql[]
                {
                new ParameterPGsql("p_id_enf_cronica", NpgsqlTypes.NpgsqlDbType.Integer, id),
                new ParameterPGsql("p_nombre", NpgsqlTypes.NpgsqlDbType.Varchar, enfermedad.nombre),
                new ParameterPGsql("p_descripcion", NpgsqlTypes.NpgsqlDbType.Varchar, enfermedad.descripcion),
                new ParameterPGsql("p_fecha_actualizacion", NpgsqlTypes.NpgsqlDbType.Date, enfermedad.fecha_actualizacion),
                new ParameterPGsql("p_estado", NpgsqlTypes.NpgsqlDbType.Boolean, true),
                }
            );
            RespuestaBD respuestaBD = await respuestaBDTask;
            resultOperation.Success = !respuestaBD.ExisteError;
            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {
                    VMCatalog aux = new VMCatalog
                    {
                        Id = (int)respuestaBD.Data.Tables[0].Rows[0]["id_enf_cronica"],
                        Nombre = respuestaBD.Data.Tables[0].Rows[0]["nombre"].ToString(),
                        Descripcion = respuestaBD.Data.Tables[0].Rows[0]["descripcion"].ToString(),
                        Estado = respuestaBD.Data.Tables[0].Rows[0]["estado"] as bool?,
                    };
                    resultOperation.Result = aux;
                }
                else
                {
                    resultOperation.Result = null;
                    resultOperation.Success = false;
                    resultOperation.AddErrorMessage("No se encontró el registro actualizado.");
                }
            }
            else
            {
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Error al actualizar el registro: {respuestaBD.Mensaje}");
            }
            return resultOperation;
        }

*/
        public async Task<ResultOperation<VMCatalog>> DeleteAsync(int id)
        {
            ResultOperation<VMCatalog> resultOperation = new ResultOperation<VMCatalog>();


            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(
                "schemasye.fn_delete_enfermedad_cronica", 
                new ParameterPGsql[]
                {
                new ParameterPGsql("p_id_enf_cronica", NpgsqlTypes.NpgsqlDbType.Integer, id)
                }
            );

            RespuestaBD respuestaBD = await respuestaBDTask;
            resultOperation.Success = !respuestaBD.ExisteError;
            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {
                    VMCatalog aux = new VMCatalog
                    {
                        Id = (int)respuestaBD.Data.Tables[0].Rows[0]["id_enf_cronica"],
                        //Nombre = respuestaBD.Data.Tables[0].Rows[0]["nombre"].ToString(),
                        //Descripcion = respuestaBD.Data.Tables[0].Rows[0]["descripcion"].ToString(),
                        //Estado = respuestaBD.Data.Tables[0].Rows[0]["estado"] as bool?,
                    };
                    resultOperation.Result = aux;
                }
                else
                {
                    resultOperation.Result = null;
                    resultOperation.Success = false;
                    resultOperation.AddErrorMessage("No se encontró el registro actualizado.");
                }
            }
            else
            {
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Error al eliminar el registro: {respuestaBD.Mensaje}");
            }
            return resultOperation;
        }


    }
}





