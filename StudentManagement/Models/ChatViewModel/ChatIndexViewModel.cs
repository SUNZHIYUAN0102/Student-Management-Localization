using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models.ChatViewModel
{
    public class ChatIndexViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public ChatCreateViewModel chatCreateViewModel { get; set; }
    }
}
