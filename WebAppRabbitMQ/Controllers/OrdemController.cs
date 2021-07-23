using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebAppRabbitMQ.Domain;

namespace WebAppRabbitMQ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdemController : ControllerBase
    {
        private ILogger<Ordem> _logger;

        public OrdemController(ILogger<Ordem> logger)
        {
            _logger = logger;
        }

        public IActionResult InsertOrdem(Ordem ordem)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "OrdemQueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = JsonSerializer.Serialize(ordem);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "OrdemQueue",
                                         basicProperties: null,
                                         body: body);
                }

                return Accepted(ordem);
            }catch (Exception ex)
            {
                _logger.LogError("Erro ao Inserir Ordem", ex);

                return new StatusCodeResult(500);
            }
        }
    }
}
