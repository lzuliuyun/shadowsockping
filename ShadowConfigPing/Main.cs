using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WindowsFormsApplication1.com;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using WindowsFormsApplication1.Model;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        string filePath = "";
        public Main()
        {
            InitializeComponent();           
        }

        private void Start_Click(object sender, EventArgs e)
        {
            try
            {
                filePath = path.Text;

                string jsonStr = Read(filePath);
                textBox1.Text = jsonStr;
                initDataGrid(jsonStr);

                data.Columns["Ping_time"].SortMode = DataGridViewColumnSortMode.Automatic;

                System.Timers.Timer t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
                t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //到达时间的时候执行事件  
                t.AutoReset = false;   //设置是执行一次（false）还是一直执行(true)  
                t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件； 
                //startPingAll();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            startPingAll();
        }

        public void initDataGrid(string jsonStr)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonStr);
            string configs = jo["configs"].ToString();
            JArray ja = (JArray)JsonConvert.DeserializeObject(configs);
            //string ja1a = ja[1]["remarks"].ToString();

            IList<ShadowSockIP> ipConfigs = ((JArray)ja).Select(x => new ShadowSockIP
            {
                Remarks = x["remarks"].ToString().TrimEnd(),
                Server = (string)x["server"],
                Server_port = (long)x["server_port"],
                Password = (string)x["password"],
                Method = (string)x["method"],
                Ping_time = 0
            }).ToList();

            data.DataSource = ToDataTable(ipConfigs);
        }

        public  DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        private void startPingAll()
        {
            int timeout = 7000;

            try
            {
                int length = data.Rows.Count;
                int start = 1;
                while (start != length)
                {
                    for (int j = 0; j < length - 1; j++)
                    {
                        string server = data.Rows[j].Cells["Server"].Value.ToString();
                        string ping_time = data.Rows[j].Cells["Ping_time"].Value.ToString();
                        if (server == timeout.ToString()) continue;

                        if (ping_time == "0")
                        {
                            start++;
                            Console.WriteLine(start);
                            long time = StartPing(server, timeout);
                            data.Rows[j].Cells["Ping_time"].Value = time.ToString();
                        }
                    }
                }

                data.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        /// <summary>
        /// 根据超时时间，ping
        /// </summary>
        /// <param name="ipStr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private long StartPing(string ipStr,int timeout)
        {
            long responseTime = timeout;
            try
            {
                //构造Ping实例  
                Ping pingSender = new Ping();
                //Ping 选项设置  
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                //测试数据  
                string data = "hello world i am pinging you abcdefg";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                //设置超时时间  
                
                //调用同步 send 方法发送数据,将返回结果保存至PingReply实例  
                PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    responseTime = reply.RoundtripTime;
                    //lst_PingResult.Items.Add("答复的主机地址：" + reply.Address.ToString());
                    //lst_PingResult.Items.Add("往返时间：" + reply.RoundtripTime);
                    //lst_PingResult.Items.Add("生存时间（TTL）：" + reply.Options.Ttl);
                    //lst_PingResult.Items.Add("是否控制数据包的分段：" + reply.Options.DontFragment);
                    //lst_PingResult.Items.Add("缓冲区大小：" + reply.Buffer.Length);
                }
                else
                {
                    responseTime = timeout;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return responseTime;
        }

        /// <summary>
        /// 根据路径读取json文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Read(string path)
        {
            string jsonStr = "";
            byte[] byData = new byte[100];
            char[] charData = new char[1000];

            try
            {
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    jsonStr =  jsonStr + line.ToString();
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }

            return jsonStr;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XMLOperate xml = new XMLOperate();
            xml.XmlName = "./config.xml";
            string value = xml.getXmlValue("load","path");
            if(value != "")
            {
                path.Text = value;
                Start_Click(null, null);
            }
            else
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "JSON文件(*.json)|*.json";
                if (openFile.ShowDialog()  == DialogResult.OK)
                {
                    path.Text = openFile.FileName;
                    xml.setXmlValue("load", "path",path.Text);

                    Start_Click(null, null);
                }
            }          
        }

        /// <summary>
        /// 自动排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void data_DataSourceChanged(object sender, EventArgs e)
        {
            // data.Rows[i].Cells["Ping_time"].Value = time.ToString();            
            data.Sort(data.Columns["Ping_time"], ListSortDirection.Ascending);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                data.Rows[i].Selected = false;
            }
        }
    }
}

