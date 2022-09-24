using System.Security.Principal;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;

namespace Screen.Core.Model
{
    public class LocationEntity
    {

        public string t_id { get; set; }

        public string location_no { get; set; }

        public string location_status { get; set; }

        public string location_flag { get; set; }

        public string location_row { get; set; }
        public string location_column { get; set; }
        public string location_layer { get; set; }
        public string location_group { get; set; }


    }

    public class LaneWayEntity
    {

        [JsonProperty(Order = 1)]
        public string sc { get; set; }
        [JsonProperty(Order = 2)]
        public string status { get; set; }
        [JsonProperty(Order = 3)]
        public int count { get; set; }


    }

    public class TaskEntity
    {

        public string type { get; set; }
        public string zone_code { get; set; }
        public string title { get; set; }
        public int count { get; set; }


    }
    public class TaskItemEntity
    {

        public string title { get; set; }
        public string zone_code { get; set; }
        public int count { get; set; }


    }

    public class EquipTaskEntity
    {

        public string sc { get; set; }
        public int in_task_count { get; set; }
        public int out_task_count { get; set; }
    }


    public class StorgedEntity
    {

        [JsonProperty(Order = 1)]
        public string item_code { get; set; }
        [JsonProperty(Order = 2)]
        public string lot_no { get; set; }
        [JsonProperty(Order = 3)]
        public string qc { get; set; }
        [JsonProperty(Order = 4)]
        public string count { get; set; }
        public string zone_code { get; set; }


    }
}
