using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class frmDuocquocgia : Form
    {
        private static string Url = "http://beta.donthuocquocgia.vn/";
        //private static string link_api = $"api/v1/thong-tin-don-thuoc/{ma_don_thuoc}";
        private static string auth_key1 = "app-name";
        private static string auth_value1 = "MQSTest";
        private static string auth_key2 = "app-key";
        private static string auth_value2 = "YFQ3narftNY64D70u4iEHJNLLbaoGqmdAXysSQNUyU6OQX6rlknpQbsnm3lP";
        DataTable dt = new DataTable();
        public frmDuocquocgia()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 25366IVLSHNF-c
        /// 25366VMA336R-c
        /// 25366537RNFS-c
        /// 25366O15EFDA-c
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            
            dgvThongtinthuoc.Columns.Clear();
            addColumns();
            string response = get_thongtindonthuoc(txtMadonthuoc.Text);//m1.get_thongtindonthuoc("25366VMA336R-c");
            JObject jmessage = JObject.Parse(response);
            

            dt = (DataTable)JsonConvert.DeserializeObject(jmessage.GetValue("thong_tin_don_thuoc").ToString(), (typeof(DataTable)));
            if (dt.Columns.Count == 8)
            {
                dgvThongtinthuoc.DataSource = dt;
            }
            else if(dt.Columns.Count == 7)
            {
                dt.Columns.Add("so_luong_ban");
                

                //DataGridViewTextBoxColumn col8 = new DataGridViewTextBoxColumn();
                //col8.DataPropertyName = "so_luong_ban";
                //col8.HeaderText = "so_luong_ban";
                //col8.Name = "so_luong_ban";
                //dgvThongtinthuoc.Columns.Add(col8);
                dgvThongtinthuoc.DataSource = dt;
            }
            //dgvThongtinthuoc.DataSource = dt;
            richTextBox1.Text = jmessage.ToString();
        }
        public string get_thongtindonthuoc(string ma_don_thuoc)
        {
            string link_api = $"api/v1/thong-tin-don-thuoc/{ma_don_thuoc}";
            string endpoint = Url + "" + link_api;
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add(auth_key1, auth_value1);
            httpClient.DefaultRequestHeaders.Add(auth_key2, auth_value2);

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri(endpoint);

            HttpResponseMessage response = httpClient.SendAsync(httpRequestMessage).Result;
            if (response.StatusCode.ToString() == "OK")
            {
                var contents = response.Content.ReadAsStringAsync();
                return contents.Result;

            }
            else
            {
                return "";
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

            string response = Post_CapNhatDonThuocBan(dgvThongtinthuoc, txtMadonthuoc.Text, txtmadinhdanhcs.Text, txttencs.Text, txtsdt.Text, txtdiachi.Text, txtmahoadon.Text);
            richTextBox1.Text = response.ToString();

        }
        public string Post_CapNhatDonThuocBan(DataGridView dt, string ma_don_thuoc, string ma_dinh_danh_cs_cung_ung_thuoc, string ten_cs_cung_ung_thuoc, string sdt_cs_cung_ung_thuoc, string dc_cs_cung_ung_thuoc, string ma_hoa_don)
        {
            string Url = "http://beta.donthuocquocgia.vn/";

            string link_api = "api/v1/cap-nhat-don-thuoc";
            string endpoint = Url + "" + link_api;
            string jsoncontent = "";
            // Tạo StringContent
            jsoncontent = "{";
            jsoncontent += "\"ma_don_thuoc\": \"" + ma_don_thuoc + "\", ";
            jsoncontent += "\"thong_tin_thuoc\": ";

            jsoncontent += "[ ";
            foreach(DataGridViewRow row in dgvThongtinthuoc.Rows)
            {
                jsoncontent += "{ ";
                jsoncontent += "\"ma_thuoc_da_ke_don\":\"" + row.Cells["id"].Value.ToString() + "\",";
                jsoncontent += "\"ma_thuoc\": \"" + row.Cells["ma_thuoc"].Value.ToString() + "\", ";
                jsoncontent += "\"biet_duoc\": \"" + row.Cells["biet_duoc"].Value.ToString() + "\", ";
                jsoncontent += "\"ten_thuoc\": \"" + row.Cells["ten_thuoc"].Value.ToString() + "\", ";
                jsoncontent += "\"don_vi_tinh\": \"" + row.Cells["don_vi_tinh"].Value.ToString() + "\", ";
                jsoncontent += "\"so_luong\": \"" + row.Cells["so_luong"].Value.ToString() + "\", ";
                jsoncontent += "\"cach_dung\": \"" + row.Cells["cach_dung"].Value.ToString() + "\", ";
               
                if(row.Cells["so_luong_ban"].Value != null)
                {

                    jsoncontent += "\"so_luong_ban\": \"" + row.Cells["so_luong_ban"].Value.ToString() + "\" ";
                    
                }
                else
                {
                    jsoncontent += "\"so_luong_ban\": \"" + 0 + "\" ";
                }
               

                jsoncontent += "},";
            }

            jsoncontent = jsoncontent.Remove(jsoncontent.Length - 1);
            jsoncontent += "],";

            jsoncontent += "\"ma_dinh_danh_co_so_cung_ung_thuoc\": \"" + ma_dinh_danh_cs_cung_ung_thuoc + "\", ";
            jsoncontent += "\"ten_co_so_cung_ung_thuoc\": \"" + ten_cs_cung_ung_thuoc + "\" , ";
            jsoncontent += "\"so_dien_thoai_co_so_cung_ung_thuoc\": \"" + sdt_cs_cung_ung_thuoc + "\", ";
            jsoncontent += "\"dia_chi_co_so_cung_ung_thuoc\": \"" + dc_cs_cung_ung_thuoc + "\", ";
            jsoncontent += "\"ma_hoa_don\": \"" + ma_hoa_don + "\" ";
            jsoncontent += "}";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(auth_key1, auth_value1);
            httpClient.DefaultRequestHeaders.Add(auth_key2, auth_value2);
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
                MessageBox.Show("Cập nhật thành công ");
                return jmessage.ToString();

            }
            else
            {
                MessageBox.Show("Cập nhật thất bại ");
                return "";
            }

        }

        private void frmDuocquocgia_Load(object sender, EventArgs e)
        {
            txtMadonthuoc.Text = "25366VMA336R-c";
            
        }
        private void addColumns()
        {
            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.DataPropertyName = "id";
            col1.HeaderText = "id";
            col1.Name = "id";
            dgvThongtinthuoc.Columns.Add(col1);

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.DataPropertyName = "biet_duoc";
            col2.HeaderText = "biet_duoc";
            col2.Name = "biet_duoc";
            dgvThongtinthuoc.Columns.Add(col2);

            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            col3.DataPropertyName = "ten_thuoc";
            col3.HeaderText = "ten_thuoc";
            col3.Name = "ten_thuoc";
            dgvThongtinthuoc.Columns.Add(col3);

            DataGridViewTextBoxColumn col4 = new DataGridViewTextBoxColumn();
            col4.DataPropertyName = "don_vi_tinh";
            col4.HeaderText = "don_vi_tinh";
            col4.Name = "don_vi_tinh";
            dgvThongtinthuoc.Columns.Add(col4);

            DataGridViewTextBoxColumn col5 = new DataGridViewTextBoxColumn();
            col5.DataPropertyName = "cach_dung";
            col5.HeaderText = "cach_dung";
            col5.Name = "cach_dung";
            dgvThongtinthuoc.Columns.Add(col5);

            DataGridViewTextBoxColumn col6 = new DataGridViewTextBoxColumn();
            col6.DataPropertyName = "ma_thuoc";
            col6.HeaderText = "ma_thuoc";
            col6.Name = "ma_thuoc";
            dgvThongtinthuoc.Columns.Add(col6);

            DataGridViewTextBoxColumn col7 = new DataGridViewTextBoxColumn();
            col7.DataPropertyName = "so_luong";
            col7.HeaderText = "so_luong";
            col7.Name = "so_luong";
            dgvThongtinthuoc.Columns.Add(col7);

            DataGridViewTextBoxColumn col8 = new DataGridViewTextBoxColumn();
            col8.DataPropertyName = "so_luong_ban";
            col8.HeaderText = "so_luong_ban";
            col8.Name = "so_luong_ban";
            dgvThongtinthuoc.Columns.Add(col8);
        }
    }
}
