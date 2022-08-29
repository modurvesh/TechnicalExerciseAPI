using CsvHelper;
using MeterReadingsAPI.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MeterReadingsAPI
{
    public class Uploads
    {
        public static string UploadCsvReadings(List<IFormFile> files) {
            // initialise reading counters
            int failedReadings = 0;
            int successfulReadings = 0;

            foreach (var file in files)
            {
                // check the file is not empty & the file type is csv
                if (file.Length > 0 && (file.ContentType == "application/vnd.ms-excel" || file.ContentType == "application/csv" || file.ContentType == "text/csv"))
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            // get the csv records
                            var meterReadings = csv.GetRecords<CsvReading>();

                            // for loop to validate each record and upon success - import into the database
                            foreach (CsvReading reading in meterReadings)
                            {
                                // initialise local variables
                                DateTime dateValue;
                                int numberValue;

                                // validation checks - check valid formats for accountId, date and value
                                if (!CheckReadingFormat(reading))
                                {
                                    // upon failure, update the failed readings counter and break
                                    failedReadings++;
                                    continue;
                                }

                                // store the csv record in the db table format
                                MeterReading meterReading = new MeterReading()
                                {
                                    AccountId = reading.AccountId,
                                    DateChecked = DateTime.Parse(reading.MeterReadingDateTime),
                                    Value = int.Parse(reading.MeterReadValue)
                                };

                                using (MeterReadingsContext ctx = new MeterReadingsContext())
                                {
                                    // check for if the accountId is associated with an account
                                    bool accountExists = ctx.UserAccounts.Where(a => a.AccountId == meterReading.AccountId).Count() > 0;
                                    // check for if the reading is a duplicate entry
                                    bool readingExists = ctx.MeterReadings.Where(r => r.AccountId == meterReading.AccountId && r.Value == meterReading.Value).Count() > 0;

                                    if (!accountExists || readingExists)
                                    {
                                        // upon failure, update failed readings counter
                                        failedReadings++;
                                        continue;
                                    }

                                    // if validation checks pass, add the reading to the db table
                                    ctx.MeterReadings.Add(meterReading);
                                    ctx.SaveChanges();
                                    successfulReadings++;
                                }
                            }
                        }
                    }
                }
                else
                    return "Incorrect file format. Please try again.";
            }

            return "Successful readings: " + successfulReadings + ", Failed readings: " + failedReadings;
        }

        public static bool CheckReadingFormat(CsvReading reading)
        {
            // initialise local variables
            DateTime dateValue;
            int numberValue;

            // validation checks - check valid formats for accountId, date and value
            return !string.IsNullOrEmpty(reading.AccountId) &&
                DateTime.TryParse(reading.MeterReadingDateTime, out dateValue) &&
                int.TryParse(reading.MeterReadValue, out numberValue) &&
                Regex.IsMatch(reading.MeterReadValue, @"^[1-9]\d{4}$");
        }
    }
}
