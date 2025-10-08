using System.ComponentModel;

namespace Infrastructure.Agent.Contracts
{
    public class ResultContract<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ResultContract<T> Ok(T data)
        => new() { Success = true, Data = data };

    public static ResultContract<T> Fail(string message)
        => new() { Success = false, Message = message };
}
}