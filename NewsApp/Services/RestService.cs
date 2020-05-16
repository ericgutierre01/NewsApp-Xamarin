using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NewsApp.Models;
using Newtonsoft.Json;

namespace NewsApp.Services
{
    public class RestService
    {
        private HttpClient client;
        //private const string URL = "http://192.168.8.128:8081"; //Domirec api
        private const string URL = "http://newsapp.somee.com"; //


        public RestService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(URL)
            };

           // if (Util.Constants.CurrentUser != null) //si estoy logeado
           // {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "1");
           // }

        }

        public Task<List<Noticia>> GetNoticias(string noti, string start) 
        {
            return Task.Run(() =>
            {
                try
                {
                    var response = client.GetAsync("/Noticias/" + noti+"?start="+start);
                    // ValidateToken(response.Result);
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        throw new Exception(response.Result.Content.ReadAsStringAsync().Result);
                    }

                    string obj = response.Result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Noticia>>(obj);
                }

                catch (Exception e)
                {
                    Console.Write(e.Message);
                    throw new Exception(e.Message);
                }
            });
        }

        public Task<List<ImagenHot>> GetHotImagenes(string noti)
        {
            return Task.Run(() =>
            {
                try
                {
                    var response = client.GetAsync("/Noticias/GetHotImagenes" + noti);
                    // ValidateToken(response.Result);
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        throw new Exception(response.Result.Content.ReadAsStringAsync().Result);
                    }

                    string obj = response.Result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<ImagenHot>>(obj);
                }

                catch (Exception e)
                {
                    Console.Write(e.Message);
                    throw new Exception(e.Message);
                }
            });
        }

        public Task<List<HomeMenuItem>> GetMenu()
        {
            return Task.Run(() =>
            {
                try
                {
                    var response = client.GetAsync("/Menu");
                    // ValidateToken(response.Result);
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        throw new Exception(response.Result.Content.ReadAsStringAsync().Result);
                    }

                    string obj = response.Result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<HomeMenuItem>>(obj);
                }

                catch (Exception e)
                {
                    Console.Write(e.Message);
                    throw new Exception(e.Message);
                }
            });
        }
    }
}
