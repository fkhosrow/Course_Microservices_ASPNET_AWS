using System;
using System.Collections.Generic;
namespace AdvertiseApi.Models
{
    public class AdvertiseModel
    {
        public string Title{ get; set; }

        public string Description { get; set; }

        public string Price{ get; set; }
        public AdvertiseStatus Status { get; set; }
    }
}
