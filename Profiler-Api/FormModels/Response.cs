namespace Profiler_Api.FormModels;

public class Response<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = "";
    public int Code { get; set; } = 0;
}
