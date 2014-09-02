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

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        public void GetAllCars()
        {
            var results1 = this._service.GetAllCars1();
            var results2 = this._service.GetAllCars2();
        }

        [Test]
        public void GetMaxMinPricesByManufacturer()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturer1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturer2();
        }

        [Test]
        public void GetMaxMinPricesByManufacturerName()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturerName1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturerName2();
        }

        [Test]
        public void GetMaxMinPricesByManufacturerNameWithYear()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturerNameWithYear1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturerNameWithYear2();
        }

        #endregion Tests
    }
}