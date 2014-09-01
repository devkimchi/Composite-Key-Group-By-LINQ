using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public IList<Car> GetAllCars()
        {
            var results = this._repository
                              .Get();
            return results.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturer1()
        {
            var results = (from c in this._context.Cars
                           group c by c.Manufacturer into g
                           select new CarViewModel()
                                  {
                                      Manufacturer = g.Key,
                                      MaxPrice = g.Max(q => q.Price),
                                      MinPrice = g.Min(q => q.Price)
                                  });
            return results.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturer2()
        {
            var results = this._repository
                              .Get()
                              .GroupBy(p => p.Manufacturer, q => new {Manufacturer = q.Manufacturer, Price = q.Price})
                              .Select(p => new CarViewModel()
                                           {
                                               Manufacturer = p.Key,
                                               MaxPrice = p.Max(q => q.Price),
                                               MinPrice = p.Min(q => q.Price)
                                           });
            return results.ToList();
        }

        public IList<CarViewModel> GetCars3()
        {
            throw new NotImplementedException();
        }

        public IList<CarViewModel> GetCars4()
        {
            throw new NotImplementedException();
        }

        public IList<CarViewModel> GetCars5()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this._repository.Dispose();
            this._context.Dispose();
        }
    }
}