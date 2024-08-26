using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class MessageInput
    {
        public int id { get; set; }
        public int serialnumber { get; set; }
        public string message { get; set; } = string.Empty;
        public DateTime sendTime { get; set; }
    }
}
