using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public static class Database
    {
        private static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=A2;Trusted_Connection=True;MultipleActiveResultSets=true";
        internal static bool CheckIfObjectExist(Content ObjectToCheck)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = $"SELECT COUNT (*) FROM Orders WHERE dealNumber = '{ObjectToCheck.dealNumber}'";
                con.Open();


                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    var result = cmd.ExecuteScalar();
                    con.Close();
                    if ((int)result == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                    
                }
            }

        }

        private static int GenerateID()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                int count = 0;
                string query = "SELECT COUNT(*) FROM Orders";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    count= (int)cmd.ExecuteScalar();
                }
                con.Close();
                return count;
            }
        }
        internal static void AddNewOrder(Content order)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string VolumeSeller, VolumeBuyer;
                VolumeSeller = order.woodVolumeSeller.ToString().Replace(',', '.');
                VolumeBuyer = order.woodVolumeBuyer.ToString().Replace(',', '.');


                string format = "yyyy-MM-dd HH:mm:ss";
                string query = $"INSERT INTO Orders (id, dealNumber, sellerName, sellerInn, buyerName, buyerInn, dealdate, woodVolumeSeller, woodVolumeBuyer) " +
                    $"VALUES ( {GenerateID()}, N'{order.dealNumber}', N'{order.sellerName}',N'{order.sellerInn}', N'{order.buyerName}', N'{order.buyerInn}', N'{order.dealDate.ToString(format)}'," +
                    $" {VolumeSeller}, {VolumeBuyer})";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                Console.WriteLine("В базу данных добавлена новая сделка, ее номер: " + order.dealNumber);
            }
        }
        internal static void CreateTable()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "CREATE TABLE Orders(" +
                         "id int primary key," +
                         "dealNumber nvarchar(max) not null," +
                         "sellerName nvarchar(max) not null," +
                         "sellerInn nvarchar(max) not null," +
                         "buyerName nvarchar(max) not null," +
                         "buyerInn nvarchar(max)," +
                         "dealdate datetime not null," +
                         "woodVolumeSeller float," +
                         "woodVolumeBuyer float)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteScalar();
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
