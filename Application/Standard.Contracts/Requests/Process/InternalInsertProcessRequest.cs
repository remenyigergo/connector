using System.Collections.Generic;

namespace Standard.Contracts.Requests
{
    public class InternalInsertProcessRequest
    {
        public HashSet<string> Processes;
        public int UserId;
    }
}