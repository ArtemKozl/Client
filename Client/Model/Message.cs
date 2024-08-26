using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Message
    {
        public int SerialNumber { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public DateTime SendTime { get; set; }
    }
}
