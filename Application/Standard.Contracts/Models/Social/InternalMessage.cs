using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Models.Social
{
    public class InternalMessage
    {
        public int FromId;
        public int ToId;
        public string Message;
        public DateTime Date;
    }
}
