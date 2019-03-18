using System.Text;
using System;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusQueueMessageSender
{
    class Program
    {
        const string ServiceBusConnectionString = "";
        const string QueueName = "";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            const int numberOfMessages = 1;
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            // Send messages.
            await SendMessagesAsync(numberOfMessages);

            await queueClient.CloseAsync();
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    MessageBody body = new MessageBody() {
                        CallTypeId = 1,
                        KeyId1 = "foo",
                        KeyId2 = 1,
                        TransactionTypeId = 1
                    };

                    string messageBody = JsonConvert.SerializeObject(body);

                    // Create a new message to send to the queue.
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue.
                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }

    public class MessageBody {
        public int CallTypeId { get; set; }
        public string KeyId1 { get; set; }
        public int KeyId2 { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
