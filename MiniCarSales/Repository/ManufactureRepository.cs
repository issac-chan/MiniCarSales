using System;
using System.Collections.Generic;
using MiniCarSales.Models;
using System.Linq;

namespace MiniCarSales.Repository
{
    public class ManufactureRepository : BaseRepository, IManufactureRepository
    {
        public ManufactureRepository(FileConnection fileConnection)
        {
            Connection = fileConnection;
        }

        public Make GetMake(int makeId)
        {
            var lstMake = ListOfMake();

            return lstMake.FirstOrDefault(x => x.MakeId == makeId);
        }

        public Model GetModel(int modelId)
        {
            var lstModel = ListOfModel(null);

            return lstModel.FirstOrDefault(x => x.ModelId == modelId);
        }

        public Year GetYear(int yearId)
        {
            var lstYear = ListOfYear();

            return lstYear.FirstOrDefault(x => x.YearId == yearId);
        }

        public List<Make> ListOfMake()
        {
            return FileRepository<List<Make>>.ReadDataFromFile(TableType.make, Connection.FilePath);
        }

        public List<Model> ListOfModel(int? makeId)
        {
            var lstModel = FileRepository<List<Model>>.ReadDataFromFile(TableType.model, Connection.FilePath);

            if (!makeId.HasValue) return lstModel;

            return lstModel.Where(x => x.MakeId == makeId.Value).ToList();
        }

        public List<Year> ListOfYear()
        {
            return FileRepository<List<Year>>.ReadDataFromFile(TableType.year, Connection.FilePath);
        }
    }
}