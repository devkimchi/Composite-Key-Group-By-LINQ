using System.Linq;
using EntityModels;
using EntityModels.Models;
using System;

namespace SampleService
{
    public class CarService : IDisposable
    {
        private readonly SampleDatabaseContext _context;
        private readonly CarRepository _repository;

        public CarService()
        {
            this._context = new SampleDatabaseContext();
            this._repository = new CarRepository(this._context);
        }

        public void GetAllCars()
        {
            var results = this._repository
                              .Get()
                              .ToList();
        }

        public void GetMaxMinPricesByManufacturer()
        {
            var results = this._repository
                              .Get()
                              .GroupBy(p => p.Manufacturer, q => new { Manufacturer = q.Manufacturer, Price = q.Price })
                              .Select(p => new { Manufacturer = p.Key, MaxPrice = p.Max(q => q.Price) })
                              .ToList();
        }

        public void GetCars3()
        {
        }

        public void GetCars4()
        {
        }

        public void GetCars5()
        {
        }

        public void Dispose()
        {
            this._repository.Dispose();
            this._context.Dispose();
        }
    }
}