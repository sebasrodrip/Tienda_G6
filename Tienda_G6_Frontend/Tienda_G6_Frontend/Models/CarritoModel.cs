using Newtonsoft.Json;
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
    public class CarritoModel
    {

        public List<CarritoEnt> ConsultarCarrito(long q)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultarCarrito?q=" + q;
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<CarritoEnt>>().Result;
                }

                return new List<CarritoEnt>();
            }
        }

        public List<CarritoEnt> ConsultarArticulosCliente(long q)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ConsultarArticulosCliente?q=" + q;
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<CarritoEnt>>().Result;
                }

                return new List<CarritoEnt>();
            }
        }

        public int AgregarArticuloCarrito(CarritoEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/AgregarArticuloCarrito";
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();
                JsonContent body = JsonContent.Create(entidad); //Serializar

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.PostAsync(url, body).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<int>().Result;
                }

                return 0;
            }
        }

        public int RemoverArticuloCarrito(long q)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/RemoverArticuloCarrito?q=" + q;
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.DeleteAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<int>().Result;
                }

                return 0;
            }
        }

        public int PagarArticuloCarrito(CarritoEnt entidad)
        {
            using (var client = new HttpClient())
            {
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/PagarArticuloCarrito";
                string token = HttpContext.Current.Session["TokenUsuario"].ToString();
                JsonContent body = JsonContent.Create(entidad); //Serializar

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage resp = client.PostAsync(url, body).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<int>().Result;
                }

                return 0;
            }
        }


    }
}