using Microsoft.AspNetCore.Mvc;

using System.Data;

namespace devopsproyect.Controllers
{
    public class GenerateController : Controller
    {
         public object GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 100)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}