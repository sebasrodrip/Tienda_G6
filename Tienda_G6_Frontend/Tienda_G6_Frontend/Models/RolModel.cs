﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Web;
using Tienda_G6_Frontend.Entities;

namespace Tienda_G6_Frontend.Models
{
    public class RolModel
    {
        public class RespuestaApi
        {
            public bool Success { get; set; }
            public string Mensaje { get; set; }
            public List<string> Errores { get; set; }
        }

        public List<RolEnt> ConsultarRoles()
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultarRoles";
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<RolEnt>>().Result;
                }

                return new List<RolEnt>();
            }
        }

        public RespuestaApi ConsultarRol(long q)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultarRol?q=" + q;
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

                return respuestaApi;
            }
        }

        public RespuestaApi RegistrarRol(RolEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/AgregarRol";
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

                return respuestaApi;
            }
        }

        public RespuestaApi ActualizarRol(RolEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ActualizarRol";
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

                return respuestaApi;
            }
        }

        public RespuestaApi EliminarRol(long q)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/EliminarRol?q=" + q;
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

                return respuestaApi;
            }
        }
    }

}