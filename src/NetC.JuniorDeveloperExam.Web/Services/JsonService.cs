using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NetC.JuniorDeveloperExam.Web.Services
{
    public class JsonService : IJsonService
    {
        public T ReadJson<T>(string filePath)
        {
            // Implement JSON reading logic here
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void WriteJson<T>(string filePath, T data)
        {
            // Implement JSON writing logic here
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, json);
        }
    }
}