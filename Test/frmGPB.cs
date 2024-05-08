using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Test
{
    public partial class frmGPB : Form
    {
        public frmGPB()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           
        }

      
        public string login(string username, string password)
        {
            username = "daiphuocgpb";
            password = "123";
            string Url = "https://gpb.ecoclinic.vn:8091";

            string link_api = "/api/Account/login";
            string endpoint = Url + link_api;
            string jsoncontent = "";
            // Tạo StringContent
            jsoncontent = "{";
            jsoncontent += "\"UserName\": \"" + username + "\", ";
            jsoncontent += "\"Password\": \"" + password + "\"";

            jsoncontent += "}";

            var httpClient = new HttpClient();
           
           
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(endpoint);



            var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = httpContent;

            var response = httpClient.SendAsync(httpRequestMessage).Result;

            if (response.IsSuccessStatusCode)
            {
                var contents = response.Content.ReadAsStringAsync();

                var json = contents.Result;
                JObject jmessage = JObject.Parse(json);
                return jmessage.ToString();

            }
            else
            {
                return "";
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            string response = GuiMauGPB(lbluser.Text, lblagentcode.Text,lbltoken.Text, txtmabn.Text, txthotenbn.Text, dtpngaysinh.Value.ToString("dd/MM/yyyy"),int.Parse(txtnamsinh.Text),int.Parse(txtgioitinh.Text),txtpara.Text,txtaddress.Text,txtphone.Text,txtmadv.Text,txtmaubenh.Text,txtmachandoan.Text,txtnoidungchandoan.Text,dtpchidinh.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                txtmabschidinh.Text, txttenbschidinh.Text, txtmabsthuchien.Text,txttenbsthuchien.Text,txtmayeucau.Text, txtcode.Text);
            lblresponse.Text = response;

        }
        public string GuiMauGPB(string username, string agentcode,string token, string PatientNo, string FullName, string sBirthday, int Age, int Sex, string Para, string Address, string Telephone, string HospitalCode, string SpecimenCode, string DiagnosticsID, string DiagnosticsContent, string AssignedDate,
        string DoctorAppointCode, string DoctorAppointName, string DoctorActID, string DoctorActName, string CodeRequest, string ScanCode)
        {
            //string Url = "https://gpb.ecoclinic.vn:8091";

            //string link_api = "/api/PathoSample/GuiMauGPB";
            //string endpoint = Url + link_api;
            //string jsoncontent = "";
            //// Tạo StringContent
            //jsoncontent = "{";
            //jsoncontent += "\"PatientNo\": \"" + PatientNo + "\", ";
            //jsoncontent += "\"FullName\": \"" + FullName + "\", ";
            //jsoncontent += "\"sBirthday\": \"" + sBirthday + "\", ";
            //jsoncontent += "\"Age\":" + Age + ", ";
            //jsoncontent += "\"Sex\":" + Sex + ", ";
            //jsoncontent += "\"Para\": \"" + Para + "\", ";
            //jsoncontent += "\"Address\": \"" + Address + "\", ";
            //jsoncontent += "\"Telephone\": \"" + Telephone + "\", ";
            //jsoncontent += "\"HospitalCode\": \"" + HospitalCode + "\", ";
            //jsoncontent += "\"SpecimenCode\": \"" + SpecimenCode + "\", ";
            //jsoncontent += "\"DiagnosticsID\": \"" + DiagnosticsID + "\", ";
            //jsoncontent += "\"DiagnosticsContent\": \"" + DiagnosticsContent + "\", ";
            //jsoncontent += "\"AssignedDate\": \"" + AssignedDate + "\", ";
            //jsoncontent += "\"DoctorAppointCode\": \"" + DoctorAppointCode + "\", ";
            //jsoncontent += "\"DoctorAppointName\": \"" + DoctorAppointName + "\", ";
            //jsoncontent += "\"DoctorActID\": \"" + DoctorActID + "\", ";
            //jsoncontent += "\"DoctorActName\": \"" + DoctorActName + "\", ";
            //jsoncontent += "\"CodeRequest\": \"" + CodeRequest + "\", ";
            //jsoncontent += "\"ScanCode\": \"" + ScanCode + "\" ";
            //jsoncontent += "}";
            //var httpClient = new HttpClient();

            ////httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            //httpClient.DefaultRequestHeaders.Add("username", username);
            //httpClient.DefaultRequestHeaders.Add("agentcode", agentcode);

            //var httpRequestMessage = new HttpRequestMessage();
            //httpRequestMessage.Method = HttpMethod.Post;
            //httpRequestMessage.RequestUri = new Uri(endpoint);



            //var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            //httpRequestMessage.Content = httpContent;

            //var response = httpClient.SendAsync(httpRequestMessage).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    var contents = response.Content.ReadAsStringAsync();

            //    var json = contents.Result;
            //    JObject jmessage = JObject.Parse(json);
            //    return jmessage.ToString();

            //}
            //else
            //{
            //    return "";
            //}

            //////////////////////////Fail////////////////////////////////////
            ///
            string _response = login(lbluser.Text, lblpass.Text);
            JObject jmess = JObject.Parse(_response);
            //lbltoken.Text = (string)jmess["token"];

            string Url = string.Empty;

            if (Url == "") Url = "https://gpb.ecoclinic.vn:8091";
            token = (string)jmess["token"];
            string link_api = "/api/PathoSample/GuiMauGPB";
            string endpoint = Url + link_api;
            string jsoncontent = "";
            // Tạo StringContent
            jsoncontent = "{";
            jsoncontent += "\"PatientNo\": \"" + PatientNo + "\", ";
            jsoncontent += "\"FullName\": \"" + FullName + "\", ";
            jsoncontent += "\"sBirthday\": \"" + sBirthday + "\", ";
            jsoncontent += "\"Age\":" + Age + ", ";
            jsoncontent += "\"Sex\":" + Sex + ", ";
            jsoncontent += "\"Para\": \"" + Para + "\", ";
            jsoncontent += "\"Address\": \"" + Address + "\", ";
            jsoncontent += "\"Telephone\": \"" + Telephone + "\", ";
            jsoncontent += "\"HospitalCode\": \"" + HospitalCode + "\", ";
            jsoncontent += "\"SpecimenCode\": \"" + SpecimenCode + "\", ";
            jsoncontent += "\"DiagnosticsID\": \"" + DiagnosticsID + "\", ";
            jsoncontent += "\"DiagnosticsContent\": \"" + DiagnosticsContent + "\", ";
            jsoncontent += "\"AssignedDate\": \"" + AssignedDate + "\", ";
            jsoncontent += "\"DoctorAppointCode\": \"" + DoctorAppointCode + "\", ";
            jsoncontent += "\"DoctorAppointName\": \"" + DoctorAppointName + "\", ";
            jsoncontent += "\"DoctorActID\": \"" + DoctorActID + "\", ";
            jsoncontent += "\"DoctorActName\": \"" + DoctorActName + "\", ";
            jsoncontent += "\"CodeRequest\": \"" + CodeRequest + "\", ";
            jsoncontent += "\"ScanCode\": \"" + ScanCode + "\" ";
            jsoncontent += "}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            httpClient.DefaultRequestHeaders.Add("username", username);
            httpClient.DefaultRequestHeaders.Add("agentcode", agentcode);

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(endpoint);

            //string _path = "..\\xmlGPB\\";
            //if (!System.IO.Directory.Exists(_path)) System.IO.Directory.CreateDirectory(_path);
            //System.IO.File.WriteAllText(_path + "GuiMau_GPB.xml", jsoncontent.ToString());

            var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = httpContent;

            var response = httpClient.SendAsync(httpRequestMessage).Result;

            if (response.IsSuccessStatusCode)
            {
                var contents = response.Content.ReadAsStringAsync();

                var json = contents.Result;
                JObject jmessage = JObject.Parse(json);
                return jmessage.ToString();

            }
            else
            {
                var contents = response.Content.ReadAsStringAsync();

                var json = contents.Result;
                JObject jmessage = JObject.Parse(json);
                return jmessage.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            string response = LayKetQuaGPB(lbluser.Text, lblagentcode.Text, txtcode_request.Text,lbltoken.Text);
            txtketqua.Text = response;
        }
        public string LayKetQuaGPB(string username, string agentcode, string code_request,string token)
        {
            string Url = "https://gpb.ecoclinic.vn:8091";

            string link_api = "/api/PathoSample/LayKetQua?CodeRequest=" + code_request + "";
            string endpoint = Url + link_api;
           // string jsoncontent = "";
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            httpClient.DefaultRequestHeaders.Add("username", username);
            httpClient.DefaultRequestHeaders.Add("agentcode", agentcode);

          

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri(endpoint);

            var response = httpClient.SendAsync(httpRequestMessage).Result;

            if (response.IsSuccessStatusCode)
            {
                var contents = response.Content.ReadAsStringAsync();

                var json = contents.Result;
                JObject jmessage = JObject.Parse(json);
                return jmessage.ToString();

            }
            else
            {
                return "";
            }
        }

        private void frmGPB_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            string response = login(lbluser.Text, lblpass.Text);
            JObject jmess = JObject.Parse(response);
            lbltoken.Text = (string)jmess["token"];
        }
    }
}
