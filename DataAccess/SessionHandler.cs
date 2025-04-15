using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;
using static PKT.Models.UserInfo;

namespace PKT.DataAccess
{
    public class SessionHandler
    {
        public static void ValidateSession(HttpContext context)
        {
            if (context.Session.GetString("userId") == null)
            {

                context.Response.Redirect("/pkt/SignIn/Login");
 

            }
        }
 
        public static void ClearSessions(HttpContext context)
        {
            context.Session.Clear();
            ValidateSession(context);
        }

        public static void ValidateSession(HttpContext context, ref UserDetails _UserDetails)
        {
            var userDetailsJson = context.Session.GetString("UserDetails");

            if (string.IsNullOrEmpty(userDetailsJson))
            {
                context.Session.Clear();
                context.Response.Redirect("/pkt/SignIn/Login");
            }
            else
            {
                try
                {
                    _UserDetails = JsonConvert.DeserializeObject<UserDetails>(userDetailsJson);
                }
                catch (Exception ex)
                {
                    // Handle deserialization error (e.g., log the error)
                }
            }
        }
    }
}
