using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Standard.Contracts.Requests
{
    public class InternalInsertProcessRequest
    {
        public int UserId;
        public HashSet<string> Processes;
    }
}
