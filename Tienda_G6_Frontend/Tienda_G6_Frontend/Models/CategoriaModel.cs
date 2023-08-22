using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Web;
using Tienda_G6_Frontend.Entities;
using System.IO;
using System.Web.Services.Description;
using static Tienda_G6_Frontend.Models.CategoriaModel;
using Newtonsoft.Json;
using System.Net;

namespace Tienda_G6_Frontend.Models
{
    public class CategoriaModel
    {
        public class RespuestaApi
        {
            public bool Success { get; set; }
            public string Mensaje { get; set; }
            public List<string> Errores { get; set; }
        }

        public List<CategoriaEnt> ConsultarCategorias()
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultarCategorias";
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<CategoriaEnt>>().Result;
                }

                return new List<CategoriaEnt>();
            }
        }

        public RespuestaApi ConsultarCategoria(long q)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultarCategoria?q=" + q;
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                RespuestaApi respuestaApi = null;

                if (resp.IsSuccessStatusCode)
                {
                    respuestaApi = resp.Content.ReadFromJsonAsync<RespuestaApi>().Result;
                    respuestaApi.Mensaje = $"Operación exitosa: {respuestaApi.Mensaje}";
                }
                else if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResponse = resp.Content.ReadAsStringAsync().Result;
                    respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(errorResponse);
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }
                else
                {
                    respuestaApi = new RespuestaApi
                    {
                        Success = false,
                        Mensaje = "Error en la solicitud al servidor.",
                        Errores = new List<string> { "Error interno del servidor." }
                    };
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }

                GuardarMensajeEnArchivo(respuestaApi.Mensaje);
                return respuestaApi;
            }
        }

        public RespuestaApi RegistrarCategoria(CategoriaEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/AgregarCategoria";
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();
                JsonContent body = JsonContent.Create(entidad);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.PostAsync(url, body).Result;

                RespuestaApi respuestaApi = null;

                if (resp.IsSuccessStatusCode)
                {
                    respuestaApi = resp.Content.ReadFromJsonAsync<RespuestaApi>().Result;
                    respuestaApi.Mensaje = $"Operación exitosa: {respuestaApi.Mensaje}";
                }
                else if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResponse = resp.Content.ReadAsStringAsync().Result;
                    respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(errorResponse);
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }
                else
                {
                    respuestaApi = new RespuestaApi
                    {
                        Success = false,
                        Mensaje = "Error en la solicitud al servidor.",
                        Errores = new List<string> { "Error interno del servidor." }
                    };
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }

                GuardarMensajeEnArchivo(respuestaApi.Mensaje);
                return respuestaApi;
            }
        }

        public RespuestaApi ActualizarCategoria(CategoriaEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ActualizarCategoria";
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();
                JsonContent body = JsonContent.Create(entidad);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.PutAsync(url, body).Result;

                RespuestaApi respuestaApi = null;

                if (resp.IsSuccessStatusCode)
                {
                    respuestaApi = resp.Content.ReadFromJsonAsync<RespuestaApi>().Result;
                    respuestaApi.Mensaje = $"Operación exitosa: {respuestaApi.Mensaje}";
                }
                else if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResponse = resp.Content.ReadAsStringAsync().Result;
                    respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(errorResponse);
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }
                else
                {
                    respuestaApi = new RespuestaApi
                    {
                        Success = false,
                        Mensaje = "Error en la solicitud al servidor.",
                        Errores = new List<string> { "Error interno del servidor." }
                    };
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }

                GuardarMensajeEnArchivo(respuestaApi.Mensaje);
                return respuestaApi;
            }
        }

        public RespuestaApi EliminarCategoria(long q)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/EliminarCategoria?q=" + q;
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.DeleteAsync(url).Result;

                RespuestaApi respuestaApi = null;

                if (resp.IsSuccessStatusCode)
                {
                    respuestaApi = resp.Content.ReadFromJsonAsync<RespuestaApi>().Result;
                    respuestaApi.Mensaje = $"Operación exitosa: {respuestaApi.Mensaje}";
                }
                else if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResponse = resp.Content.ReadAsStringAsync().Result;
                    respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(errorResponse);
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }
                else
                {
                    respuestaApi = new RespuestaApi
                    {
                        Success = false,
                        Mensaje = "Error en la solicitud al servidor.",
                        Errores = new List<string> { "Error interno del servidor." }
                    };
                    respuestaApi.Mensaje = $"Error en la solicitud: {respuestaApi.Mensaje}";
                }

                GuardarMensajeEnArchivo(respuestaApi.Mensaje);
                return respuestaApi;
            }
        }

        //------------------------------------------------//
        private void GuardarMensajeEnArchivo(string mensaje)
        {
            string rutaArchivo = @"C:\Users\antho\Documents\GitHub\Tienda_G6\prueba.txt";

            try
            {
                // Usar StreamWriter para escribir en el archivo
                using (StreamWriter sw = File.AppendText(rutaArchivo))
                {
                    sw.WriteLine(mensaje);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir al escribir en el archivo
                Console.WriteLine("Error al escribir en el archivo: " + ex.Message);
            }
        }
    }
}