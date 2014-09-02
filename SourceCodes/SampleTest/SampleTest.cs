using FluentAssertions;
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

            results1.Count.Should().Be(results2.Count);
        }

        [Test]
        public void GetMaxMinPricesByManufacturer()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturer1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturer2();

            results1.Count.Should().Be(results2.Count);
        }

        [Test]
        public void GetMaxMinPricesByManufacturerName()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturerName1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturerName2();

            results1.Count.Should().Be(results2.Count);
        }

        [Test]
        public void GetMaxMinPricesByManufacturerNameWithYear()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturerNameWithYear1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturerNameWithYear2();

            results1.Count.Should().Be(results2.Count);
        }

        [Test]
        public void GetMaxMinPricesByManufacturerNameWithYearMoreThanOne()
        {
            var results1 = this._service.GetMaxMinMinPricesByManufacturerNameWithYearMoreThanOne1();
            var results2 = this._service.GetMaxMinMinPricesByManufacturerNameWithYearMoreThanOne2();

            results1.Count.Should().Be(results2.Count);
        }

        #endregion Tests
    }
}