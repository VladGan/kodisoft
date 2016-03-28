using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WeatherApp
{
    public partial class Form1 : Form
    {

        string Temperature;
        string Condition;
        string Humidity;
        string Windspeed;
        string Town;
        string LastValidTown;
        string Country;
        string Code;
        string Local;
        string LastUpdate;
        string[] next_day=new string[10];
        string[] next_cond = new string[10];
        string[] next_condt = new string[10];
        string[] next_high = new string[10];
        string[] next_low = new string[10];

        public Form1()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            InitializeComponent();
            Town = "Kiev"; LastValidTown = "Kiev";
            getWeather();
            label1.Text = Temperature;
            label2.Text = Town;
            label6.Text = Condition;
            label7.Text = Humidity + "%";
            label8.Text = Windspeed + "km/h";
            label10.Text = string.Format("\u00B0")+ "C";
            label12.Text = next_day[1];
            label15.Text = next_day[2];
            label17.Text = next_day[3];
            label19.Text = next_day[4];
            label20.Text = Country;
            label13.Text = next_condt[1];
            label14.Text = next_condt[2];
            label16.Text = next_condt[3];
            label18.Text = next_condt[4];

            label21.Text = next_high[1] + string.Format("\u00B0") + "C";
            label24.Text = next_high[2] + string.Format("\u00B0") + "C";
            label26.Text = next_high[3] + string.Format("\u00B0") + "C";
            label28.Text = next_high[4] + string.Format("\u00B0") + "C";
            label22.Text = next_low[1] + string.Format("\u00B0") + "C";
            label23.Text = next_low[2] + string.Format("\u00B0") + "C";
            label25.Text = next_low[3] + string.Format("\u00B0") + "C";
            label27.Text = next_low[4] + string.Format("\u00B0") + "C";
            setIcons();
            setIcon();
        }

        private void setIcons()
        {
            pictureBox1.Image = Image.FromFile(getString(next_cond[1]));
            pictureBox3.Image = Image.FromFile(getString(next_cond[2]));
            pictureBox4.Image = Image.FromFile(getString(next_cond[3]));
            pictureBox5.Image = Image.FromFile(getString(next_cond[4]));
        }

        private void ChangeCode(ref string code)
        {
            switch (code)
            {
                case "395": code = "0";  break; case "320": code = "42"; break;
                case "392": code = "0";  break; case "317": code = "42"; break;
                case "389": code = "2";  break; case "314": code = "10"; break;
                case "386": code = "2";  break; case "311": code = "10"; break;
                case "377": code = "42"; break; case "308": code = "12"; break;
                case "374": code = "42"; break; case "305": code = "12"; break;
                case "371": code = "42"; break; case "302": code = "10"; break;
                case "368": code = "11"; break; case "299": code = "10"; break;
                case "365": code = "11"; break; case "296": code = "10"; break;
                case "362": code = "11"; break; case "293": code = "10"; break;
                case "359": code = "12"; break; case "284": code = "25"; break;
                case "356": code = "12"; break; case "281": code = "25"; break;
                case "353": code = "12"; break; case "266": code = "25"; break;
                case "350": code = "42"; break; case "263": code = "11"; break;
                case "338": code = "42"; break; case "260": code = "25"; break;
                case "335": code = "42"; break; case "248": code = "20"; break;
                case "332": code = "42"; break; case "230": code = "40"; break;
                case "329": code = "42"; break; case "227": code = "40"; break;
                case "326": code = "42"; break; case "200": code = "17"; break;
                case "323": code = "42"; break; case "185": code = "25"; break;
                case "182": code = "6";  break; case "119": code = "26"; break;
                case "179": code = "42"; break; case "116": code = "28"; break;
                case "176": code = "11"; break; case "113": code = "36"; break;
                case "143": code = "20"; break; case "122": code = "32"; break; 
                               
            }
        }

        private string getString(string code)
        {
            ChangeCode(ref code);
            return "../Pics/" + code + ".png";

        }
        private void setIcon()
        {
            ChangeCode(ref Code);
            string st = "../Pics/"+ Code +".png";
            pictureBox2.Image = Image.FromFile(st);
        }

        private void getWeather()
        {
            if (CheckForInternetConnection())
            {
                label29.Text = "";
                var url = 
                       "http://api.worldweatheronline.com/premium/v1/weather.ashx?key=98a9e453f70b4d47b7c150932162703&q="
                     + Town 
                     + "&format=xml&num_of_days=5&tp=24";
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                XmlNodeList nodes = xmlDoc.SelectNodes("data");

                if (xmlDoc.InnerText == "Unable to find any matching weather location to the query submitted!")
                {
                    Town = LastValidTown;
                    textBox1_Error();
                    return;
                }

                Temperature = nodes[0].SelectNodes("current_condition")[0].SelectNodes("temp_F")[0].InnerXml;
                Condition =   nodes[0].SelectNodes("current_condition")[0].SelectNodes("weatherDesc")[0].InnerText;
                Code =        nodes[0].SelectNodes("current_condition")[0].SelectNodes("weatherCode")[0].InnerXml;
                Humidity =    nodes[0].SelectNodes("current_condition")[0].SelectNodes("humidity")[0].InnerXml;
                Windspeed =   nodes[0].SelectNodes("current_condition")[0].SelectNodes("windspeedKmph")[0].InnerXml;
                Town =        nodes[0].SelectNodes("request")[0].SelectNodes("query")[0].InnerXml;
                Local =       nodes[0].SelectNodes("current_condition")[0].SelectNodes("temp_F")[0].InnerXml;
                LastUpdate =  nodes[0].SelectNodes("current_condition")[0].SelectNodes("observation_time")[0].InnerXml;

                label11.Text = "Last Updated on : " + LastUpdate + " GMT";
                double x = (Double.Parse(Temperature) - 32) * 5.0 / 9.0;
                x = (int)x;
                label9.Text = x.ToString() + " " + string.Format("\u00B0") + "C";


                XmlNodeList forecast = nodes[0].SelectNodes("weather");
                for (int i = 1; i < 5; i++)
                {
                    string[] split = forecast[i].SelectNodes("date")[0].InnerXml.Split(new Char[] {'-'});
                    DateTime dateValue = new DateTime(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]));
                    next_day[i] = dateValue.ToString("dddd");
                    next_condt[i] = forecast[i].SelectNodes("hourly")[0].SelectNodes("weatherDesc")[0].InnerText;
                    next_cond[i] = forecast[i].SelectNodes("hourly")[0].SelectNodes("weatherCode")[0].InnerXml;
                    next_high[i] = forecast[i].SelectNodes("maxtempC")[0].InnerXml;
                    next_low[i] = forecast[i].SelectNodes("mintempC")[0].InnerXml;
                }

                string xmlString = xmlDoc.OuterXml;
                return;
            }
            else
            {
                label29.Text = "Offline";
                StreamReader file = new StreamReader("data.txt");
                Temperature = file.ReadLine();
                Condition = file.ReadLine();
                Code = file.ReadLine();
                Humidity = file.ReadLine();
                Windspeed = file.ReadLine();
                Town = file.ReadLine();
                Country = file.ReadLine();
                Local = file.ReadLine();
                LastUpdate = file.ReadLine();

                for (int i = 0; i < 4; i++)
                {
                    next_day[i] = file.ReadLine();
                    next_cond[i] = file.ReadLine();
                    next_condt[i] = file.ReadLine();
                    next_high[i] = file.ReadLine();
                    next_low[i] = file.ReadLine();
                }

                label11.Text = file.ReadLine();
                label19.Text = file.ReadLine();
                file.Close();

                return;
            }

        }

        private void getWeather1()
        {
            if (CheckForInternetConnection())
            {
                label29.Text = "";
                FileStream fs = new FileStream("data.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                String st = String.Format(@"http://forecast.weather.gov/MapClick.php?lat=38.4247341&lon=-86.9624086&FcstType=XML");
                //st = st + woeid;
                XmlDocument wData = new XmlDocument();
                var url = "http://forecast.weather.gov/MapClick.php?lat=42&lon=-75&FcstType=dwml";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
                var xml = client.GetStringAsync(url);
                //wData.Load(st);

                XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
                manager.AddNamespace("yweather", @"http://xml.weather.yahoo.com/ns/rss/1.0");

                XmlNode channel = wData.SelectSingleNode("rss").SelectSingleNode("channel");
                // XmlNodeList nodes = wData.SelectNodes("rss/channel/item/yweather:forecast", manager);

                Temperature = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
                Condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;
                Code = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["code"].Value;
                Humidity = channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["humidity"].Value;
                Windspeed = channel.SelectSingleNode("yweather:wind", manager).Attributes["speed"].Value;
                Town = channel.SelectSingleNode("yweather:location", manager).Attributes["city"].Value;
                //Region = channel.SelectSingleNode("yweather:location", manager).Attributes["region"].Value;
                Country = channel.SelectSingleNode("yweather:location", manager).Attributes["country"].Value;
                Local = channel.SelectSingleNode("item").SelectSingleNode("title", manager).InnerXml;
                LastUpdate = channel.SelectSingleNode("item").SelectSingleNode("pubDate", manager).InnerXml;
                //foreach( channel.SelectSingleNode("item").Select("yweather:forecast", manager))
                XmlNodeList forecast = channel.SelectSingleNode("item").SelectNodes("yweather:forecast", manager);

                for (int i = 0; i < forecast.Count; i++)
                {
                    next_day[i] = forecast[i].Attributes["day"].Value;
                    sw.WriteLine(next_day[i]);
                    next_cond[i] = forecast[i].Attributes["code"].Value;
                    sw.WriteLine(next_cond[i]);
                    next_condt[i] = forecast[i].Attributes["text"].Value;
                    sw.WriteLine(next_condt[i]);
                    next_high[i] = forecast[i].Attributes["high"].Value;
                    sw.WriteLine(next_high[i]);
                    next_low[i] = forecast[i].Attributes["low"].Value;
                    sw.WriteLine(next_low[i]);              
                }

                label11.Text = "Last Updated on : " + LastUpdate;
                sw.WriteLine(label11.Text);
                double x = (Double.Parse(Temperature) - 32) * 5.0 / 9.0;
                x = (int)x;
                label9.Text = x.ToString() + " " + string.Format("\u00B0") + "C";
                sw.WriteLine(label9.Text);
                //  IfCond = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast").Attributes["text"].Value;
                // IfHigh = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast").Attributes["high"].Value;
                // Iflow = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast").Attributes["low"].Value;
                sw.Flush();
                fs.Close();
            }
            else
            {
                label29.Text = "Offline";
                StreamReader file = new StreamReader("data.txt");
                Temperature = file.ReadLine();
                Condition = file.ReadLine();
                Code = file.ReadLine();
                Humidity = file.ReadLine();
                Windspeed = file.ReadLine();
                Town = file.ReadLine();
                Country = file.ReadLine();
                Local = file.ReadLine();
                LastUpdate = file.ReadLine();

                for (int i = 0; i < 5; i++)
                {
                    next_day[i] = file.ReadLine();
                    next_cond[i] = file.ReadLine();
                    next_condt[i] = file.ReadLine();
                    next_high[i] = file.ReadLine();
                    next_low[i] = file.ReadLine();
                }

                label11.Text = file.ReadLine();
                label19.Text = file.ReadLine();
                file.Close();
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var Client = new WebClient()) 
                using (var stream = Client.OpenRead("http://www.google.com"))
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                textBox1.Text = "Enter city name";
                textBox1.ForeColor = Color.LightGray;
            }
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            if (textBox1.Text == "Enter city name" || textBox1.Text == "City name must be valid") textBox1.Text = string.Empty;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox6_Click(this, new EventArgs());
            }
        }
        public void textBox1_Error()
        {
            textBox1.Text = "City name must be valid";
            textBox1.ForeColor = Color.LightGray;
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                textBox1.Text = "Enter city name";
                textBox1.ForeColor = Color.LightGray;
            }
            label1.Focus();
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Town = textBox1.Text;
            getWeather();
            label1.Text = Temperature;
            label6.Text = Condition;
            label7.Text = Humidity + "%";
            label8.Text = Windspeed + "km/h";
            label10.Text = string.Format("\u00B0") + "C";
            label12.Text = next_day[0];
            label15.Text = next_day[1];
            label17.Text = next_day[2];
            label19.Text = next_day[3];
            label20.Text = Country;
            label13.Text = next_condt[0];
            label14.Text = next_condt[1];
            label16.Text = next_condt[2];
            label18.Text = next_condt[3];
            setIcons();
            setIcon();
        }
       
        //private void pictureBox6_MouseEnter(object sender, EventArgs e)
        //{
        //    pictureBox6.BackColor = Color.FromArgb(1,3,4);
        //}

        //private void pictureBox6_MouseLeave(object sender, EventArgs e)
        //{
        //    pictureBox6.BackColor = Color.FromArgb(65, 33, 34); ;
        //}
    }
    


}
