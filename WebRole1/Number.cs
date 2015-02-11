using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class Number : TableEntity
    {
        public Number(int n)
        {
            this.PartitionKey = "sum";
            this.RowKey = n.ToString();
            this.Sum = n;
        }

        public Number() { }

        public int Sum { get; set; }
    }
}