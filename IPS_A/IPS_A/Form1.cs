using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace IPS_A
{
    public partial class Form1 : Form
    {
        public System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Xml>));
        public List<Xml> list = new List<Xml>();


        public Form1()
        {
            InitializeComponent();


        }

        public static string Enc(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(bytes);
            
        }

        public static string Dec(string text)
        {
            var s = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(s);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //list.Add(new Xml { Name = "p", Value = textBox1.Text });
            //save("test");

            XDocument xDocument = new XDocument(new XDeclaration("1.0","utf-8",null), new XElement("root", new XAttribute("value", Enc(textBox1.Text))));
            xDocument.Save(Application.ProductName);
        }


        public void save(string file)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.OpenOrCreate))
                xmlSerializer.Serialize(fs, list);
        }


        public void load(string file)
        {
            list.Clear();
            using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.OpenOrCreate))
                foreach (var item in (List<Xml>)xmlSerializer.Deserialize(fs))
                {
                    list.Add(item);
                }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //load("test");
            //textBox1.Text = list[0].Value;

            XDocument xDocument = XDocument.Load(Application.ProductName);

            var val = xDocument.Descendants("root").First().Attribute("value").Value;

            //label2.Text = Dec(val);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Settings.Save(textBox1.Text);
            this.Close();
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {
            
            
            this.Hide();
            //MessageBox.Show(Interop.GetKeyboardLayout(Interop.GetWindowThreadProcessId(Interop.GetForegroundWindow(), IntPtr.Zero)).ToString());

            //System.Threading.Thread.Sleep(2000);
            //SendKeys.SendWait("alekseevns@alrosa.ru");
            //SendKeys.SendWait("{Tab}");
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            //Task.Factory.StartNew(Work);
        }
        private void Work()
        {
            for (int i = 0; i < 1000; i++)
            {
                Invoke(new Action(() => textBox2.AppendText(i.ToString())));
            }
        }

        //не закрывать форму а скрывать
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }



    [Serializable]
    public class Xml
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
