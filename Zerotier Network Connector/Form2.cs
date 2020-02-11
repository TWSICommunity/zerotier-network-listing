using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zerotier_Network_Connector
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                if (textBox1.Text != "" || textBox2.Text != "")
                {
                    var values = new NameValueCollection();
                    values["networkid"] = textBox1.Text;
                    values["is_private"] = checkBox1.Checked.ToString();
                    values["networkname"] = textBox2.Text;
                    //values["api"] = APIKEY;
                    //values["file_name"] = filename;
                    // values["content"] = "Hello!";
                    //values["action"] = "r";

                    var response = client.UploadValues("https://ztlist.wolf.mba/submit.php", values);

                    var responseString = Encoding.Default.GetString(response);
                    string responser = responseString;

                    Console.WriteLine(responser);
                    this.Close();
                }
                else
                {
                    Console.WriteLine("All fields must be filled in before submitting...");
                }
            }
        }
    }
}
