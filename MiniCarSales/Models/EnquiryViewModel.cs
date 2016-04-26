using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniCarSales.Models
{
    public class EnquiryViewModel
    {
        public int EnquiryId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Comment { get; set; }

        public int VehicleId { get; set; }

        public string VehicleName { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime? WhenRead { get; set; }
    }
}