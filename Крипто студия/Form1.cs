using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Windows.Forms;

namespace Крипто_студия
{
    public partial class Form1 : Form
    {
        //private string data;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Работаем с биржей Yobit.net - Yobit.io
        /// </summary>
        private void load_yobit()  //Загружаем список
        {
            string url = "https://yobit.net/api/3/info";
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_yobit);
            wc.DownloadStringAsync(new Uri(url));
        }
        void wc_yobit(object sender, DownloadStringCompletedEventArgs e)
        {
                var jo = JObject.Parse(e.Result);
                foreach (JToken tkn in jo["pairs"])
                {
                    listBox1.Items.Add(((JProperty)tkn).Name.ToString());
                }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData_yobit(listBox1.GetItemText(listBox1.SelectedItem));
        }
        public void GetData_yobit(string pair)
        {
            string queryStr = string.Format("https://yobit.net/api/3/ticker/{0}", pair);
            WebClient client = new WebClient();
            var json = client.DownloadString(queryStr);
            var jo = JObject.Parse(json)[pair];
            double data = (double)jo["buy"];
            label1.Text = "Пара " + pair.Replace("_", "/").ToUpper();
            label2.Text = String.Format("{0:0.00000000}", data);
            linkLabel1.Text = "https://yobit.net/ru/trade/" + pair.Replace("_", "/").ToUpper();
        }
        
        // Загружаем данные Cryptopia
        private void load_cryptopia()
        {
            string url = "https://www.cryptopia.co.nz/api/GetMarkets";
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_cryptopia);
            wc.DownloadStringAsync(new Uri(url));
        }

        void wc_cryptopia(object sender, DownloadStringCompletedEventArgs e)
        {
                var jo = JObject.Parse(e.Result);
                foreach (JToken tkn in jo["Data"])
                {
                    listBox2.Items.Add((string)tkn["Label"] + ":" + (string)tkn["TradePairId"]);
                }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string output = listBox2.GetItemText(listBox2.SelectedItem).Substring(listBox2.GetItemText(listBox2.SelectedItem).IndexOf(':') + 1);
            GetData_cryptopia(output);

        }

        public void GetData_cryptopia(string pair)
        {
            string queryStr = string.Format("https://www.cryptopia.co.nz/api/GetMarket/{0}", pair);
            WebClient client = new WebClient();
            var json = client.DownloadString(queryStr);
            var jo = JObject.Parse(json)["Data"];
            double data = (double)jo["BidPrice"];
            label3.Text = "Пара " + (listBox2.GetItemText(listBox2.SelectedItem).Substring(0, listBox2.GetItemText(listBox2.SelectedItem).IndexOf(':'))).Replace("/", "_").ToUpper();
            label4.Text = String.Format("{0:0.00000000}", data);
            string link = (listBox2.GetItemText(listBox2.SelectedItem).Substring(0, listBox2.GetItemText(listBox2.SelectedItem).IndexOf(':'))).Replace("/", "_").ToUpper();
            linkLabel2.Text = "https://www.cryptopia.co.nz/Exchange/?market=" + link;
        }

        // Загружаем данные Bleutrade
        private void load_bleutrade()
        {
            string url = "https://bleutrade.com/api/v2/public/getmarkets";
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_bleutrade);
            wc.DownloadStringAsync(new Uri(url));
        }

        void wc_bleutrade(object sender, DownloadStringCompletedEventArgs e)
        {
                var jo = JObject.Parse(e.Result);
                foreach (JToken tkn in jo["result"])
                {
                    listBox3.Items.Add((string)tkn["MarketName"]);

                }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData_bleutrade(listBox3.GetItemText(listBox3.SelectedItem));
        }
        public void GetData_bleutrade(string pair)
        {
            string queryStr = string.Format("https://bleutrade.com/api/v2/public/getticker?market={0}", pair);
            WebClient client = new WebClient();
            var json = client.DownloadString(queryStr);
            var jo = JObject.Parse(json)["result"];
            double data = (double)jo[0]["Bid"];
            label5.Text = "Пара " + pair.Replace("_", "/").ToUpper();
            label6.Text = String.Format("{0:0.00000000}", data);
            linkLabel3.Text = "https://bleutrade.com/exchange/" + pair.Replace("_", "/").ToUpper();
        }
        
        // Обрабатываем Bittrex
        private void load_bittrex()
        {
            string url = "https://bittrex.com/api/v1.1/public/getmarkets";
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_bittrex);
            wc.DownloadStringAsync(new Uri(url));
        }
        void wc_bittrex(object sender, DownloadStringCompletedEventArgs e)
        {
            var jo = JObject.Parse(e.Result);
            foreach (JToken tkn in jo["result"])
            {
                listBox4.Items.Add((string)tkn["MarketName"]);

            }
        }
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData_bittrex(listBox4.GetItemText(listBox4.SelectedItem));
        }
        public void GetData_bittrex(string pair)
        {
            string queryStr = string.Format("https://bittrex.com/api/v1.1/public/getticker?market={0}", pair);
            WebClient client = new WebClient();
            var json = client.DownloadString(queryStr);
            var jo = JObject.Parse(json)["result"];
            double data = (double)jo["Bid"];
            label7.Text = "Пара " + pair.Replace("_", "/").ToUpper();
            label8.Text = String.Format("{0:0.00000000}", data);
            linkLabel4.Text = "https://bittrex.com/Market/Index?MarketName=" + pair.ToUpper();
        }

        //Обрабатываем Poloniex 
        private void load_poloniex()
        {
            string url = "https://poloniex.com/public?command=returnTicker";
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_poloniex);
            wc.DownloadStringAsync(new Uri(url));
        }

        void wc_poloniex(object sender, DownloadStringCompletedEventArgs e)
        {
            var jo = JObject.Parse(e.Result);
            foreach (var pair in jo)
            {
                listBox5.Items.Add(pair.Key);
            }
        }
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData_poloniex(listBox5.GetItemText(listBox5.SelectedItem));
        }

        public void GetData_poloniex(string pair)
        {
            string queryStr = string.Format("https://poloniex.com/public?command=returnOrderBook&currencyPair={0}&depth=1", pair);
            WebClient client = new WebClient();
            var json = client.DownloadString(queryStr);

            var data = JsonConvert.DeserializeObject<get_ticker>(json);
            string bids = data.Bids[0][0].ToString();
            bids = bids.Replace("\"", "");
            //Console.WriteLine(data.Bids[0][0].ToString());
            //var jo = JObject.Parse(json);

            label9.Text = "Пара " + pair.Replace("_", "/").ToUpper();
            label10.Text = bids;
            linkLabel5.Text = "https://poloniex.com/exchange#" + pair.ToLower();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FindMyString(textBox1.Text, listBox1);
            FindMyString(textBox1.Text, listBox2);
            FindMyString(textBox1.Text, listBox3);
            FindMyString("BTC-"+textBox1.Text, listBox4);
            FindMyString("BTC_" + textBox1.Text, listBox5);
        }

     

        private void FindMyString(string searchString, ListBox listbox)
        {
            // Ensure we have a proper string to search for. 
            if (searchString != string.Empty)
            {
                // Find the item in the list and store the index to the item. 
                int index = listbox.FindString(searchString);
                // Determine if a valid index is returned. Select the item if it is valid. 
                if (index != -1)
                {
                    listbox.SetSelected(index, true);
                }

                else
                {

                }
                   // MessageBox.Show("На бирже Bleutrade такой пары нет");
            }
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 splash = new Form2();
            splash.Show();
            load_yobit();
            load_cryptopia();
            load_bleutrade();
            load_bittrex();
            load_poloniex();
            splash.Close();
        }

 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel2.Text);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel3.Text);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel4.Text);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel5.Text);
        }
    }
}
