namespace Standard.Contracts
{
    public class Result<T>
    {
        public T Data;
        public int ResultCode;
        public string ResultMessage;
    }
}