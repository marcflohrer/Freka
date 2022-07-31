namespace OutputProject.Messaging.Response.Generated;

public interface IResponseSender
{
    public Task SendMessageAsync(Models.Response response);
}
