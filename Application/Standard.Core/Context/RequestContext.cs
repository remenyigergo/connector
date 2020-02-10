using System.Threading;

namespace Standard.Core.Context
{
    public class RequestContext
    {
        private static readonly AsyncLocal<RequestContext> _logicalInstance = new AsyncLocal<RequestContext>();

        public static RequestContext Current
        {
            get => _logicalInstance.Value;
            set => _logicalInstance.Value = value;
        }
    }
}