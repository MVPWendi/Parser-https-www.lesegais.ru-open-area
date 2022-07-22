
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    class Program
    {

        private static int IntervalMinutes = 10;
       
        static void Main(string[] args)
        {
            
            Database.CreateTable();
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    ResponseObject respObject = GetData(i, 5);
                    foreach (var responseobj in respObject.data.searchReportWoodDeal.content)
                    {
                        Console.WriteLine($"Проверка сделки: {responseobj.dealNumber}");
                        if (!Database.CheckIfObjectExist(responseobj))
                        {
                            Database.AddNewOrder(responseobj);
                        }
                        else
                        {
                            Console.WriteLine($"Сделка уже есть в базе данных");
                        }
                    }
                }
                Console.WriteLine($"Следующая проверка в:  { DateTime.Now.AddSeconds(IntervalMinutes*60).ToString()}");
                Thread.Sleep(IntervalMinutes * 60 * 1000);
            }
        }

        static ResponseObject GetData(int pagenumber, int count)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.lesegais.ru/open-area/graphql");
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";

                request.Headers.Add("Origin: https://www.lesegais.ru");

                request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Mobile Safari/537.36";

                request.Referer = "https://www.lesegais.ru/open-area/deal";
                string content = "{\"query\":\"query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\\n searchReportWoodDeal(filter: $filter, pageable: { number: $number, size: $size}, orders: $orders) {\\n content {\\n sellerName\\n sellerInn\\n buyerName\\n buyerInn\\n woodVolumeBuyer\\n woodVolumeSeller\\n dealDate\\n dealNumber\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\",\"variables\":{\"size\":" + count + ",\"number\":" + pagenumber + ",\"filter\":null,\"orders\":null},\"operationName\":\"SearchReportWoodDeal\"}";
                byte[] postBytes = Encoding.UTF8.GetBytes(content);
                request.ContentLength = postBytes.Length;
                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(postBytes, 0, postBytes.Length);
                }
                var str = request.GetResponse();
                using (var sr = new StreamReader(str.GetResponseStream()))
                {
                    string text = sr.ReadToEnd();
                    var respobj = JsonConvert.DeserializeObject<ResponseObject>(text);
                    return respobj;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка при выполнении POST запроса");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        

        
    }


    
}
