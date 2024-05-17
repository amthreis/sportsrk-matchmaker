using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRkMatchmaker.Framework
{
    public struct StartRequest
    {
        public string Message { get; set; }

        public StartRequest(string message)
        {
            Message = message;
        }
    }
}
