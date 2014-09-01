using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModels;
using EntityModels.Models;
using NUnit.Framework;
using SampleService;

namespace SampleTest
{
    [TestFixture]
    public class SampleTest
    {
        #region SetUp / TearDown
        private CarService _service;

        [SetUp]
        public void Init()
        {
            this._service = new CarService();
        }

        [TearDown]
        public void Dispose()
        {
            this._service.Dispose();
        }

        #endregion

        #region Tests

        [Test]
        public void GetAllCars()
        {
            var results = this._service.GetAllCars();
        }

        [Test]
        public void GetMaxMinPricesByManufacturer()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturer1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturer2();
        }

        #endregion
    }
}
