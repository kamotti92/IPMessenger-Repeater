using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace IPMessenger_Repeater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                HttpListener listen = new HttpListener();
                listen.Prefixes.Clear();
                listen.Prefixes.Add("http://+:80/Temporary_Listen_Addresses/");
                listen.Start();
                while (true)
                {
                    HttpListenerContext context = listen.GetContext();
                    if (context.Request.HasEntityBody)
                    {
                        //中身があったら表示
                        Stream input = context.Request.InputStream;
                        using (StreamReader sr = new StreamReader(input))
                        {
                            //日付だけ書いておく
                            Console.WriteLine(DateTime.Now.ToString());
                            Console.WriteLine(sr.ReadToEnd());
                            //メッセージボックス出す
                            //別スレッドにする（止まってフリーズするので）
                            Task.Run(() => {
                                MessageBox.Show("メッセージ受信しました");
                            });
                        }
                    }
                    HttpListenerResponse res = context.Response;
                    res.StatusCode = 200;
                    byte[] content = Encoding.UTF8.GetBytes("OK");
                    res.OutputStream.Write(content, 0, content.Length);
                    res.Close();
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }
    }
}
