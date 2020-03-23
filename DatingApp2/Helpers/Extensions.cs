using Microsoft.AspNetCore.Http;

namespace DatingApp2.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

            //Will allow any origin because of wildcard second parameter.
            response.Headers.Add("Access-Control-Allow-Origin", "*"); 
        }
    }
}