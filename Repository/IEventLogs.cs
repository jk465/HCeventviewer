using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCeventviewer.Repository
{
    public interface IEventLogs
    {
        Task<string> GetEventLogs(DateTime? fromDate, DateTime? toDate);
    }
}
