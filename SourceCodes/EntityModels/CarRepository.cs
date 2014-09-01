using EntityModels.Models;
using System;
using System.Linq;

namespace EntityModels
{
    public class CarRepository : IDisposable
    {
        private readonly SampleDatabaseContext _context;

        public CarRepository(SampleDatabaseContext context)
        {
            this._context = context;
        }

        public IQueryable<Car> Get()
        {
            return this._context.Cars;
        }

        public void Dispose()
        {
        }
    }
}