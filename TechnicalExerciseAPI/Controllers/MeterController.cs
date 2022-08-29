using Microsoft.AspNetCore.Mvc;
using MeterReadingsAPI.Models;
using MeterReadingsAPI;

namespace MeterReadingsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterController : ControllerBase
    {
        [HttpPost("/meter-reading-uploads")]
        public ActionResult<string> UploadReadings(List<IFormFile> files)
        {
            return Uploads.UploadCsvReadings(files);
        }

        [HttpGet("/meter-readings")]
        public ActionResult<List<MeterReading>> GetAllReadings()
        {
            using (MeterReadingsContext ctx = new MeterReadingsContext())
            {
                return ctx.MeterReadings.ToList();
            }
        }

        [HttpGet("/meter-reading")]
        public ActionResult<List<MeterReading>> GetUserReadings(string accountId)
        {
            using (MeterReadingsContext ctx = new MeterReadingsContext())
            {
                return ctx.MeterReadings.Where(r => r.AccountId == accountId).ToList();
            }
        }

        [HttpGet("/users")]
        public ActionResult<List<UserAccount>> GetAllUsers()
        {
            using (MeterReadingsContext ctx = new MeterReadingsContext())
            {
                return ctx.UserAccounts.ToList();
            }
        }

        [HttpGet("/user")]
        public ActionResult<UserAccount> GetUser(string firstName, string lastName)
        {
            using (MeterReadingsContext ctx = new MeterReadingsContext())
            {
                return ctx.UserAccounts.Where(a => a.FirstName == firstName && a.LastName == lastName).FirstOrDefault();
            }
        }
    }
}
