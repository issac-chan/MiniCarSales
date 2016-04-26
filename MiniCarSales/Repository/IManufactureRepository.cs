using MiniCarSales.Models;
using System.Collections.Generic;

namespace MiniCarSales.Repository
{
    public interface IManufactureRepository
    {
        List<Make> ListOfMake();

        List<Model> ListOfModel(int? makeId);

        List<Year> ListOfYear();

        Make GetMake(int makeId);

        Model GetModel(int modelId);

        Year GetYear(int yearId);
    }
}
