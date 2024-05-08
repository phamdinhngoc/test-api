
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class frmMedigo : Form
    {
        //private static LibHISExtension.AccessData m1;
        public static string auth_name = "service-key";
        public static string auth_value = "d-ms-j4dmxfi0-893d723a-e16b-42ed-b378-f0e4dfac259f";
        public static string endpoint = "https://dev-api.medigoapp.com/mq/webhook";
        DataTable auth_key = new DataTable();
        public frmMedigo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eventType = "01";
            //string response = m1.post_btdbn_medigo("01", txtmabn.Text, txthoten.Text, dtpngaysinh.Value.ToString(), int.Parse(txtnamsinh.Text),int.Parse(txtphai.Text), int.Parse(txtmann.Text),int.Parse(txtmadantoc.Text),txtsonha.Text ,txtthon.Text ,txtcholam.Text,txtmatt.Text,txtmaqu.Text,txtmaphuongxa.Text,txtsocmnd.Text);
            //auth_key.Columns.Add("auth_key");
            //auth_key.Columns.Add("auth_value");
            //object[] obj = { "service-key", "d-ms-j4dmxfi0-893d723a-e16b-42ed-b378-f0e4dfac259f" };
            //auth_key.Rows.Add(obj);

            JObject btdbn = new JObject{
                { "MABN",txtmabn.Text},
                { "HOTEN",txthoten.Text},
                { "NGAYSINH",dtpngaysinh.Value.ToString("dd/MM/yyyy")},
                { "NAMSINH",txtnamsinh.Text },
                { "PHAI",txtphai.Text },
                { "MANN",txtmann.Text },
                {"MADANTOC",txtmadantoc.Text },
                {"SONHA",txtsonha.Text },
                {"THON",txtthon.Text },
                {"CHOLAM",txtcholam.Text },
                {"MATT",txtmatt.Text },
                {"MAQU",txtmaqu.Text },
                {"MAPHUONGXA",txtmaphuongxa.Text },
                {"SOCMND",txtsocmnd.Text },

            };
            //string data = JsonConvert.SerializeObject(btdbn);
            JObject objdata = new JObject
            {
                {"eventType",eventType },
                {"timestamp",DateTime.Now.ToString()},
                {"data",btdbn }
            };
            string responseFromWebHook = sendRequest(endpoint, objdata.ToString(), auth_key);
            JObject jmessage = JObject.Parse(responseFromWebHook);
            if (jmessage.GetValue("code").ToString() == "200")
            {
                MessageBox.Show("Thành công" + jmessage.GetValue("code").ToString());
            }
            else
            {
                MessageBox.Show("Thất bại" + jmessage.GetValue("code").ToString());
            }
            
        }
        public static string sendRequest(string endpoint, string postJsonString, DataTable auth_key)
        {
            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);
                var postData = postJsonString;
                var data = Encoding.UTF8.GetBytes(postData);


                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                for (int i = 0; i < auth_key.Rows.Count; i++)
                {

                    httpWReq.Headers.Add(auth_key.Rows[i]["auth_key"].ToString(), auth_key.Rows[i]["auth_value"].ToString());
                }
                //httpWReq.Headers.Add(auth_name, auth_value);

                httpWReq.ContentType = "application/json";
                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;

                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }
                //todo parse it
                return jsonresponse;
                //return new Response(mtid,jsonresponse)
            }
            catch (System.Exception e)
            {

                return e.Message;
            }
        }
        private void frmMedigo_Load(object sender, EventArgs e)
        {
            //m1 = Program.dal_t;
            auth_key.Columns.Add("auth_key");
            auth_key.Columns.Add("auth_value");
            object[] obj = { "service-key", "d-ms-j4dmxfi0-893d723a-e16b-42ed-b378-f0e4dfac259f" };
            auth_key.Rows.Add(obj);

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string eventType = "02";
            //auth_key.Columns.Add("auth_key");
            //auth_key.Columns.Add("auth_value");
            //object[] obj = { "service-key", "d-ms-j4dmxfi0-893d723a-e16b-42ed-b378-f0e4dfac259f" };
            //auth_key.Rows.Add(obj);

            JObject v_ttrvll = new JObject{
                { "ID",txtid.Text},
                { "QUYENSO",txtquyenso.Text},
                { "SOBIENLAI",txtsobienlai.Text },
                { "NGAY",dtpngay.Value.ToString("dd/MM/yyyy HH:mm:ss") },
                { "MAKP",txtmakp.Text },
                { "SOTIEN",txtsotien.Text },
                {"TAMUNG",txttamung.Text },
                {"MIEN",txtmien.Text },
                {"BHYT",txtbhyt.Text },
                {"USERID",txtuserid.Text },
                {"NGAYUD",dtpngayud.Value.ToString("dd/MM/yyyy HH:mm:ss") },
                {"MADOITUONG",txtmadoituong.Text },
                {"SOPHIEU",txtsophieu.Text },
                {"QUYENSOGTGT",txtquyensogtgt.Text },
                {"SOBIENLAIGTGT",txtsobienlaigtgt.Text }
            };

            JObject objdata = new JObject
            {
                {"eventType",eventType },
                {"timestamp",DateTime.Now.ToString()},
                {"data",v_ttrvll }
            };
            string responseFromWebHook = sendRequest(endpoint, objdata.ToString(), auth_key);
            JObject jmessage = JObject.Parse(responseFromWebHook);
            if (jmessage.GetValue("code").ToString() == "200")
            {
                MessageBox.Show("Thành công " + jmessage.GetValue("code").ToString());
            }
            else
            {
                MessageBox.Show("Thất bại " + jmessage.GetValue("code").ToString());
            }
        }
    }
}
