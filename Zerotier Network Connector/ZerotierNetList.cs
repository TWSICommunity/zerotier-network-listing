using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Request library
using System.Net;
using System.IO;
using System.Windows.Forms;
using Zerotier_Network_Connector;
using Newtonsoft.Json;

namespace Zerotier_Network_Connector
{
    public class ZerotierNetList
    {
        public string getSubmittedNetworks(string URL)
        {
            string json = (new WebClient()).DownloadString(URL + "listing_json.php");
            return json;
            /* using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["ztconfig"] = "true"; // ztconfig - Sets the 

                var response = client.UploadValues(URL + "listing_json.php", values);

                var responseString = Encoding.Default.GetString(response);
                string responser = responseString;

                Console.WriteLine(responser);

                return responser;
            } */
        }

        public string getVersion()
        {
            string json = (new WebClient()).DownloadString("https://ztlist.wolf.mba/version_check.php");
            return json;
        }

        public string SearchNetwork(string URL, string Search)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["search"] = Search; // ztconfig - Sets the 

                var response = client.UploadValues(URL + "listing_json.php", values);

                var responseString = Encoding.Default.GetString(response);
                string responser = responseString;

                Console.WriteLine(responser);

                return responser;
            }
        }
    }
}
