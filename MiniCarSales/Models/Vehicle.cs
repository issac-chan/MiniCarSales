using System;

namespace MiniCarSales.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        
        public int MakeId { get; set; }

        public int ModelId { get; set; }

        public int YearId { get; set; }

        public double Price { get; set; }

        public string photo { get; set; }

        public string Phone { get; set; }

        public string ContactEmail { get; set; }

        public string ContactName { get; set; }        

        public string ABN { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime WhenCreated { get; set; }

        public int? ChangedBy { get; set; }

        public DateTime? WhenChanged { get; set; }
    }
}