using System.Text;
using Producer.Models;
using Producer.Services.Interfaces;
using RabbitMQ.Client;

namespace Producer.Services{
    public class QueueServiceImpl : IQueueService<ProduceModel>
    {
        private readonly ILogger<QueueServiceImpl> _logger;
        public QueueServiceImpl(ILogger<QueueServiceImpl> logger)
        {
            _logger = logger;
        }

        public void Post(ProduceModel model)
        {
            _logger.LogInformation(@$"
                ======== MESSAGE CREATING ============
                id: {model.id.ToString()}
                message: {model.message}
                ======== MESSAGE CREATING ============
            ");

            ConnectionFactory? factory = new ConnectionFactory(){ 
                HostName = "localhost"
            };
            
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "MsQueue",
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    durable: true
                );

                string message = model.message;
                var body = Encoding.UTF8.GetBytes(message);    

                channel.ExchangeDeclare(
                    exchange: "tdc-honduras", 
                    type: ExchangeType.Direct,
                    durable: true
                    );
                
                channel.BasicPublish(
                    exchange: "tdc-honduras",
                    routingKey: "MsQueue",
                    body: body
                );

                Console.WriteLine(" [x] Sent {0}", message);
            }

        }
    }
}