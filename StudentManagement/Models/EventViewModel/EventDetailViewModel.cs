using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.EventViewModel
{
    public class EventDetailViewModel
    {
        public IEnumerable<Event> Events { get; set; }

        public EventCreateViewModel EventCreateViewModel { get; set; }
    }
}
