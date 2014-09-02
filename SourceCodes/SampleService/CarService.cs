using EntityModels;
using EntityModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IList<Car> GetAllCars1()
        {
            var results1 = (from c in this._context.Cars select c);
            return results1.ToList();
        }

        public IList<Car> GetAllCars2()
        {
            var results2 = this._repository.Get();
            return results2.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturer1()
        {
            var results1 = (from c in this._context.Cars
                            group c by c.Manufacturer into g
                            select new CarViewModel()
                                   {
                                       Manufacturer = g.Key,
                                       MaxPrice = g.Max(q => q.Price),
                                       MinPrice = g.Min(q => q.Price)
                                   });
            return results1.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturer2()
        {
            var results2 = this._repository
                               .Get()
                               .GroupBy(c => c.Manufacturer, r => new { Manufacturer = r.Manufacturer, Price = r.Price })
                               .Select(c => new CarViewModel()
                                            {
                                                Manufacturer = c.Key,
                                                MaxPrice = c.Max(q => q.Price),
                                                MinPrice = c.Min(q => q.Price)
                                            });
            return results2.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturerName1()
        {
            var results1 = (from c in this._context.Cars
                            group c by new { Manufacturer = c.Manufacturer, Name = c.Name } into g
                            select new CarViewModel()
                                   {
                                       Manufacturer = g.Key.Manufacturer,
                                       Name = g.Key.Name,
                                       MaxPrice = g.Max(q => q.Price),
                                       MinPrice = g.Min(q => q.Price)
                                   });
            return results1.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturerName2()
        {
            var results2 = this._repository
                               .Get()
                               .GroupBy(c => new { Manufacturer = c.Manufacturer, Name = c.Name },
                                        (g, r) => new CarViewModel()
                                                  {
                                                      Manufacturer = g.Manufacturer,
                                                      Name = g.Name,
                                                      MaxPrice = r.Max(q => q.Price),
                                                      MinPrice = r.Min(q => q.Price)
                                                  });
            return results2.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturerNameWithYear1()
        {
            var results1 = (from c in this._context.Cars
                            where c.Year != null
                            group c by new { Manufacturer = c.Manufacturer, Name = c.Name } into g
                            select new CarViewModel()
                                   {
                                       Manufacturer = g.Key.Manufacturer,
                                       Name = g.Key.Name,
                                       MaxPrice = g.Max(q => q.Price),
                                       MinPrice = g.Min(q => q.Price)
                                   });
            return results1.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturerNameWithYear2()
        {
            var results2 = this._repository
                               .Get()
                               .Where(c => c.Year != null)
                               .GroupBy(c => new { Manufacturer = c.Manufacturer, Name = c.Name },
                                        (g, r) => new CarViewModel()
                                                  {
                                                      Manufacturer = g.Manufacturer,
                                                      Name = g.Name,
                                                      MaxPrice = r.Max(q => q.Price),
                                                      MinPrice = r.Min(q => q.Price)
                                                  });
            return results2.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturerNameWithYearMoreThanOne1()
        {
            var results1 = (from c in this._context.Cars
                            where c.Year != null && c.Price != null
                            group c by new { Manufacturer = c.Manufacturer, Name = c.Name, Year = c.Year } into g
                            where g.Count() > 1
                            select new CarViewModel()
                                   {
                                       Manufacturer = g.Key.Manufacturer,
                                       Name = g.Key.Name,
                                       Year = g.Key.Year,
                                       MaxPrice = g.Max(q => q.Price),
                                       MinPrice = g.Min(q => q.Price)
                                   });
            return results1.ToList();
        }

        public IList<CarViewModel> GetMaxMinMinPricesByManufacturerNameWithYearMoreThanOne2()
        {
            var results2 = this._repository
                               .Get()
                               .Where(c => c.Year != null && c.Price != null)
                               .GroupBy(c => new { Manufacturer = c.Manufacturer, Name = c.Name, Year = c.Year },
                                        (g, r) => new
                                                  {
                                                      Manufacturer = g.Manufacturer,
                                                      Name = g.Name,
                                                      Year = g.Year,
                                                      Count = r.Count(),
                                                      MaxPrice = r.Max(q => q.Price),
                                                      MinPrice = r.Min(q => q.Price)
                                                  })
                               .Where(p => p.Count > 1)
                               .Select(p => new CarViewModel()
                                            {
                                                Manufacturer = p.Manufacturer,
                                                Name = p.Name,
                                                Year = p.Year,
                                                MaxPrice = p.MaxPrice,
                                                MinPrice = p.MinPrice
                                            });
            return results2.ToList();
        }

        public void Dispose()
        {
            this._repository.Dispose();
            this._context.Dispose();
        }
    }
}