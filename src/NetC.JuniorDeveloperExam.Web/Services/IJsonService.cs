using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetC.JuniorDeveloperExam.Web.Services
{
    public interface IJsonService
    {
        T ReadJson<T>(string filePath);
        void WriteJson<T>(string filePath, T data);
    }
}