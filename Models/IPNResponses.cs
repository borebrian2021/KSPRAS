﻿namespace KSPRAS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class IPNResponses
    {
        [Key]
        public string Key { get; set; }
        public string OrderTrackingId { get; set; }
        public string OrderNotificationType { get; set; }
        public string OrderMerchantReference { get; set; }
       
    }
}
