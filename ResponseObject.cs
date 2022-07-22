using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ResponseObject
    {
        public Data data;

    }

    public class searchReportWoodDeal
    {
        public string __typename;
        public List<Content> content;
    }
    public class Data
    {
        public searchReportWoodDeal searchReportWoodDeal;
    }

    public class Content
    {
        public string __typename;
        public string buyerInn;
        public string buyerName;
        public DateTime dealDate;
        public string dealNumber;
        public string sellerInn;
        public string sellerName;
        public float woodVolumeSeller;
        public float woodVolumeBuyer;
    }
}
