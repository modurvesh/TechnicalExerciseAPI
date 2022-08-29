using MeterReadingsAPI.Models;

namespace MeterReadingsAPI.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckReadingFormat_ValidData_Success()
        {
            // Arrange
            CsvReading reading = new CsvReading();
            reading.AccountId = "2344";
            reading.MeterReadingDateTime = DateTime.Now.ToString();
            reading.MeterReadValue = "23566";

            // Act
            bool result = Uploads.CheckReadingFormat(reading);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckReadingFormat_InvalidAccountId_Fail()
        {
            // Arrange
            CsvReading reading = new CsvReading();
            reading.AccountId = "";
            reading.MeterReadingDateTime = DateTime.Now.ToString();
            reading.MeterReadValue = "23566";

            // Act
            bool result = Uploads.CheckReadingFormat(reading);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckReadingFormat_InvalidDate_Fail()
        {
            // Arrange
            CsvReading reading = new CsvReading();
            reading.AccountId = "";
            reading.MeterReadingDateTime = "Invalid";
            reading.MeterReadValue = "23566";

            // Act
            bool result = Uploads.CheckReadingFormat(reading);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckReadingFormat_InvalidNumberMeterReading_Fail()
        {
            // Arrange
            CsvReading reading = new CsvReading();
            reading.AccountId = "";
            reading.MeterReadingDateTime = DateTime.Now.ToString();
            reading.MeterReadValue = "invalid";

            // Act
            bool result = Uploads.CheckReadingFormat(reading);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckReadingFormat_InvalidFormatMeterReading_Fail()
        {
            // Arrange
            CsvReading reading = new CsvReading();
            reading.AccountId = "";
            reading.MeterReadingDateTime = DateTime.Now.ToString();
            reading.MeterReadValue = "1002";

            // Act
            bool result = Uploads.CheckReadingFormat(reading);

            //Assert
            Assert.IsFalse(result);
        }
    }
}