using MiniCarSales.Models;
using System.Collections.Generic;

namespace MiniCarSales.Repository
{
    public interface IVehicleRepository
    {
        int AddVehicle(Vehicle vehicle);

        bool UpdateVehicle(Vehicle vehicle);

        bool DeleteVehicle(int vehicleId);

        List<Vehicle> SearchVehicles(int? makeId, int? modelId, int? yearId, int? vehicleId);        
    }
}
