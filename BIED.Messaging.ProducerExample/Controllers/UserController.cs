using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BIED.Messaging.Abstractions;
using BIED.Messaging.ProducerExample.Messaging.Messages;
using BIED.Messaging.ProducerExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace BIED.Messaging.ProducerExample.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        IMessageProducer MessageProducer;

        public UserController(IMessageProducer messageProducer)
        {
            MessageProducer = messageProducer;
        }

        [HttpPost("/api/users")]
        public async Task<IActionResult> Create(User user)
        {
            var message = new UserCreatedMessage()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = $"{user.FirstName}{user.LastName}"
            };

            await MessageProducer.SendAsync(message, "users.created");

            Console.WriteLine("Message sent: \n" +  JsonSerializer.Serialize(message));

            return Ok();

        }

    }
}
