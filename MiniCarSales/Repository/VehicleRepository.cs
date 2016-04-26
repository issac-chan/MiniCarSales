using System.Collections.Generic;
using MiniCarSales.Models;
using System.Linq;

namespace MiniCarSales.Repository
{
    public class VehicleRepository : BaseRepository, IVehicleRepository
    {
        public VehicleRepository(FileConnection fileConnection)
        {
            Connection = fileConnection;
        }

        public int AddVehicle(Vehicle Vehicle)
        {
            var lstVehicles = FileRepository<List<Vehicle>>.ReadDataFromFile(TableType.vehicle, Connection.FilePath);

            if(lstVehicles == null || lstVehicles.Count==0)
            {
                lstVehicles = new List<Vehicle>();
                Vehicle.VehicleId = 1;
            }
            else
            {
                Vehicle.VehicleId = lstVehicles.OrderByDescending(x => x.VehicleId).First().VehicleId + 1;
            }

            lstVehicles.Add(Vehicle);

            var result = FileRepository<List<Vehicle>>.WriteDataToFile(TableType.vehicle, Connection.FilePath, lstVehicles);

            return Vehicle.VehicleId;
        }

        public bool DeleteVehicle(int vehicleId)
        {
            var lstVehicles = FileRepository<List<Vehicle>>.ReadDataFromFile(TableType.vehicle, Connection.FilePath);

            var index = lstVehicles.FindIndex(x => x.VehicleId == vehicleId);

            if (index <0)
                return false;

            lstVehicles.RemoveAt(index);

            return FileRepository<List<Vehicle>>.WriteDataToFile(TableType.vehicle, Connection.FilePath, lstVehicles);
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            var lstVehicles = FileRepository<List<Vehicle>>.ReadDataFromFile(TableType.vehicle, Connection.FilePath);

            if (lstVehicles == null || lstVehicles.Count() == 0)
                return false;

            var index = lstVehicles.FindIndex(x => x.VehicleId == vehicle.VehicleId);

            if (index < 0) return false;

            lstVehicles[index] = vehicle;

            return FileRepository<List<Vehicle>>.WriteDataToFile(TableType.vehicle, Connection.FilePath, lstVehicles);
        }

        public List<Vehicle> SearchVehicles(int? makeId, int? modelId, int? yearId, int? vehicleId)
        {
            var lstVehicles = FileRepository<List<Vehicle>>.ReadDataFromFile(TableType.vehicle, Connection.FilePath);

            if (lstVehicles == null) return null;

            return (from vehicle in lstVehicles
                                 where (!makeId.HasValue || makeId.Value == vehicle.VehicleId)
                                 && (!modelId.HasValue || modelId.Value == vehicle.ModelId)
                                 && (!yearId.HasValue || yearId.Value == vehicle.YearId)
                                 && (!vehicleId.HasValue || vehicleId.Value == vehicle.VehicleId)                                 
                                 select vehicle).ToList();
        }        
    }
}