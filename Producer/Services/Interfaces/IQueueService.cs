namespace Producer.Services.Interfaces
{
    public interface IQueueService<TResponse>
    {
        public void Post(TResponse model);
         
    }
}