using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for phishfinder
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class phishfinder : System.Web.Services.WebService
{

    public phishfinder()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public void ForeignLoginsThisWeek()
    {
        List<EASLogin> logins = new List<EASLogin>();
        var cs = ConfigurationManager.ConnectionStrings["LogHitsDB"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spForeignLoginsThisWeek", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                EASLogin entry = new EASLogin
                {
                    UserName = rdr["UserName"].ToString(),
                    Alpha2Code = rdr["Alpha2Code"].ToString(),
                    IP = rdr["IP"].ToString()
                };
                logins.Add(entry);
            }
            con.Close();
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(logins));
    }
    public class EASLogin
    {
        public string UserName;
        public string Alpha2Code;
        public string IP;
    }
}
