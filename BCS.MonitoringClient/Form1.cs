using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Net;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;

namespace BCS.MonitoringClient
{
    public partial class Form1 : Form
    {
        private HubConnection _hubConnection;
        private Cookie _cookie;
        private bool _isAuthenticated;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoginCredentials();
            ListenToServer();
        }

        private void LoginCredentials(){
            _isAuthenticated = AuthenticateUser("rubelitoi", "default", out _cookie);

            if (_isAuthenticated)
            {
                MessageBox.Show("Authenticated");
            }
            else
            {
                MessageBox.Show("Not Authenticated");
            }
        }

        private void ListenToServer(){
            string serverPath = @"http://localhost:1421/";
            if (_hubConnection == null)
            {
                if (_isAuthenticated)
                {
                    _hubConnection = new HubConnection(serverPath);
                    _hubConnection.CookieContainer = new CookieContainer();
                    _hubConnection.CookieContainer.Add(_cookie);
                    var hubProxy = _hubConnection.CreateHubProxy("monitorHub");
                    _hubConnection.Start().Wait();
                    hubProxy.On<string, string>("activityLog", (operation, message) =>
                        this.Invoke((Action)(() =>
                            listView1.Items.Add(operation).SubItems.Add(message)
                            )));
                }
            }
        }

        private bool AuthenticateUser(string user, string password, out Cookie authCookie)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:1421/Home/Login/") as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json;";
            httpWebRequest.CookieContainer = new CookieContainer();

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())){
                string json = new JavaScriptSerializer().Serialize(new
                {
                    UserName = user,
                    Password = password
                });

                streamWriter.Write(json);
            }

            using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
            {
                authCookie = response.Cookies[FormsAuthentication.FormsCookieName];
            }

            if (authCookie != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
