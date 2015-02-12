using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        public static int[] numArray;
        public static CloudTable table;

        [WebMethod]
        public string WorkerRoleCalculateSum(int n1, int n2, int n3)
        {
            //List<int> filedata = new List<int>();

            //filedata.Add(n1);
            //filedata.Add(n2);
            //filedata.Add(n3);
            
            //numArray = filedata.Skip(0)
            //    .Take(3)
            //    .ToArray();
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference("numberStorage");
            table.CreateIfNotExists();

            string message = n1.ToString() + ", " + n2.ToString() + ", " + n3.ToString();

            //foreach (int n in numArray)
            //{
            //    TableOperation insertOperation = TableOperation.Insert(new Number(n));
            //    table.Execute(insertOperation);
            //    result = result + n;
            //}

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("numberQueue");
            queue.CreateIfNotExists();

            queue.AddMessage(new CloudQueueMessage(message));

            return message;
        }

        [WebMethod]
        public int ReadSumFromTableStorage()
        {
            int sum = 0;

            TableQuery<Number> rangeQuery = new TableQuery<Number>().ToList();
            foreach (Number n in table.ExecuteQuery(rangeQuery))
            {
                sum = n.Num;
            }

            return sum;
        }
    }
}
