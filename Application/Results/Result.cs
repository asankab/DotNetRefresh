namespace Pros.Application.Results
{
    //public class Result<T> where T : class
    //{
    //    public bool isFailuer { get; set; } = false;
    //    public T? Data { get; set; }
    //    public Exception? Error { get; set; }
    //}

    public class Result {
        public bool isFailuer { get; set; } = false;
        public object Data { get; set; }
        public Exception? Error { get; set; }
    }
}
