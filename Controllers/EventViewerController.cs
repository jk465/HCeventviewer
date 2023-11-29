using HCeventviewer.Repository;
using HCeventviewer.Service;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace HCeventviewer.Controllers
{
    
    [AuthService]
    public class EventViewerController : ApiController
    {
        private readonly IEventLogs eventLogs;
        public EventViewerController(IEventLogs _eventLogs)
        {
            eventLogs = _eventLogs;
            LogFileCleanUpService.CleanUpLogFiles();
        }

        [HttpGet]
        [Route("api/eventviewer/getevents")]
        public async Task<IHttpActionResult> GetEvents([FromUri] DateTime? fromDate = null, [FromUri] DateTime? toDate = null)
        {
            try
            {
                Log.Logger.Information("Process Started");

                var FilePath = await eventLogs.GetEventLogs(fromDate, toDate);

                Log.Logger.Information("Process Completed");

                return Content(HttpStatusCode.OK, new { success = true, response = FilePath });
            }
            catch (Exception e)
            {
                Log.Logger.Error(e,e.Message);
                return Content(HttpStatusCode.InternalServerError, new { success = false, err = e.Message });
            }
        }
    }
}
