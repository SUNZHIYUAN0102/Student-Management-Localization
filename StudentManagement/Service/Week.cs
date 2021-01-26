using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public class Week
    {
        public static IEnumerable<String> Split(DateTime start, DateTime end)
        {
            DateTime chunkEnd;
            while ((chunkEnd = start.AddDays(6)) < end)
            {
                yield return start.ToShortDateString() + " - " + chunkEnd.ToShortDateString();
                start = chunkEnd.AddDays(1);
            }
            yield return start.ToShortDateString() + " - " + start.AddDays(6).ToShortDateString();
        }
    }
}
