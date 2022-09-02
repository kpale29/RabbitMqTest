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
                HostName = "https://b-fc22b968-88b0-498b-ab54-fd12be568fea.mq.us-east-1.amazonaws.com",
                UserName = "administrator",
                Password = "P1assw3rd#852",
                VirtualHost = "/"  
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "MsQueue",
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                string message = model.message;
                var body = Encoding.UTF8.GetBytes(message);    
                
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine(" [x] Sent {0}", message);
            }

        }
    }
}