using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Threading;

namespace Zerotier_Network_Connector
{
    public partial class Form1 : Form
    {
        public static ZerotierNetList ztlist = new ZerotierNetList();
        public string version = "Build 05:22 AM 02.02.2020";
        public string checkNetworks;

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Checking for updates...";
            string check = ztlist.getVersion();

            if (check != version)
            {
                toolStripStatusLabel1.Text = "New update found!";
                string message = "You have an outdated version of the client. Want to close? You'll need to download the latest client at https://ztlist.wolf.mba/download-tool/";
                string caption = "You nay need to update...";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Close();
                }
            }
            toolStripStatusLabel1.Text = "You are using the latest version.";
            MessageBox.Show("Welcome to Zerotier Network Connector!\n\nBefore continuing you msut have zerotier-one installed. To do that visit https://www.zerotier.com/download/ and download zerotier's windows installation or other operating system. Since this tool uses windows, you will need to install the windows version. After downloading and installing it run zerotier's client.\n\nNow the tool works and you can connect to other networks. We don't have a feature that ignore this message for now till the next update comes.", "Welcome to Zerotier's Network Connector Tool", MessageBoxButtons.OK);
            toolStripStatusLabel1.Text = "Querying...";
            listBox1.Items.Clear();
            string json = ztlist.getSubmittedNetworks("https://ztlist.wolf.mba/");
            checkNetworks = json;
            var strsplited = json.Split("|".ToCharArray());

            foreach (var VARIABLE in strsplited)
            {
                listBox1.Items.Add(VARIABLE);
            }
            toolStripStatusLabel1.Text = "Query Done!";
            listBox1.SelectedIndex = 0;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = ztlist.getSubmittedNetworks("https://ztlist.wolf.mba/");
            var strsplited = json.Split("|".ToCharArray());
            if (checkNetworks != json)
            {
                checkNetworks = json;
                toolStripStatusLabel1.Text = "Clearing...";
                listBox1.Items.Clear();
                foreach (var VARIABLE in strsplited)
                {
                    listBox1.Items.Add(VARIABLE);
                }
                toolStripStatusLabel1.Text = "New Query Done!";
            }
        }

        public void ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                string message = result;
                string caption = "Result of executing a command.";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result1;

                result1 = MessageBox.Show(message, caption, buttons);
            }
            catch (Exception objException)
            {
                string obj = objException.ToString();
                MessageBox.Show(obj, "Error in executing zerotier-cli...", MessageBoxButtons.OK);
            }
        }

        public void ExecuteCommandAsync(string command)
        {
            try
            {
                //Asynchronously start the Thread to process the Execute command request.
                Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
                //Make the thread as background thread.
                objThread.IsBackground = true;
                //Set the Priority of the thread.
                objThread.Priority = ThreadPriority.AboveNormal;
                //Start the thread.
                objThread.Start(command);
            }
            catch (ThreadStartException objException)
            {
                Console.WriteLine(objException);
            }
            catch (ThreadAbortException objException)
            {
                Console.WriteLine(objException);
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException);
            }
        }

        private void connectToASelectedNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsAdministrator())
            {
                if (listBox1.SelectedItem.ToString() == "MISSING_NETWORKS")
                {
                    MessageBox.Show("There are no networks submitted at the moment. Try submitting one.", "Missing networks deteched...", MessageBoxButtons.OK);
                }
                else
                {
                    

                    var strsplited = listBox1.SelectedItem.ToString().Split(":".ToCharArray());
                    string message = "Are you sure you want to join " + strsplited[0] + "'s Network?";
                    string caption = "Requires administrative privileges for zerotier-cli.";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show(message, caption, buttons);
                    if (result == DialogResult.Yes)
                    {
                        ExecuteCommandSync(@"zerotier-cli join " + strsplited[0]);
                    }
                }
            }
            else
            {
                string message = "You need administrative privileges to use zerotier-cli command.";
                string caption = "Requires administrative privileges for zerotier-cli.";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var strsplited = listBox1.SelectedItem.ToString().Split(":".ToCharArray());
            string networkid;
            string isprivate;
            string networkname;
            networkid = strsplited[0];
            networkname = strsplited[2];
            if(strsplited[1] == "1") {
                isprivate = "PRIVATE";
            } else {
                isprivate = "PUBLIC";
            }
            toolStripStatusLabel1.Text = networkid + " " + isprivate + " " + networkname;
        }

        private void aboutZeroTierNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form3 = new Form3();
            form3.ShowDialog();
        }

        private void submitNetworkIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Currently submitting...";
            int index = listBox1.SelectedIndex;
            var form2 = new Form2();
            form2.ShowDialog();
            
            string json = ztlist.getSubmittedNetworks("https://ztlist.wolf.mba/");
            var strsplited = json.Split("|".ToCharArray());
            if (checkNetworks != json)
            {
                checkNetworks = json;
                listBox1.Items.Clear();
                foreach (var VARIABLE in strsplited)
                {
                    listBox1.Items.Add(VARIABLE);
                }
                listBox1.SelectedIndex = index;
            }
            toolStripStatusLabel1.Text = "Submit form closed!";
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Checking for updates...";
            string check = ztlist.getVersion();

            if (check != version)
            {
                toolStripStatusLabel1.Text = "New update found!";
                string message = "You have an outdated version of the client. Want to close? You'll need to download the latest client at https://ztlist.wolf.mba/download-tool/";
                string caption = "You nay need to update...";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Close();
                }
            }
            toolStripStatusLabel1.Text = "You are using the latest version.";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            listBox1.SelectedIndex = 0;
            if (e.KeyCode.ToString() == "Return")
            {
                
                string json = ztlist.SearchNetwork("https://ztlist.wolf.mba/", textBox1.Text);
                var strsplited = json.Split("|".ToCharArray());
                if (checkNetworks != json)
                {
                    toolStripStatusLabel1.Text = "Clearing...";
                    checkNetworks = json;
                    listBox1.Items.Clear();
                    foreach (var VARIABLE in strsplited)
                    {
                        listBox1.Items.Add(VARIABLE);
                    }
                    toolStripStatusLabel1.Text = "Query Done!";
                    listBox1.SelectedIndex = 0;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            
            string json = ztlist.SearchNetwork("https://ztlist.wolf.mba/", textBox1.Text);
            if (checkNetworks != json)
            {
                checkNetworks = json;
                listBox1.Items.Clear();
                var strsplited = json.Split("|".ToCharArray());

                foreach (var VARIABLE in strsplited)
                {
                    listBox1.Items.Add(VARIABLE);
                }
                listBox1.SelectedIndex = index;
            }
        }
    }
}
