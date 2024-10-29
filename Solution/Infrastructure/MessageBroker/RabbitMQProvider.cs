using System.Net.Sockets;
using System.Text;
using Common.DTOs;
using Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Infrastructure.MessageBroker;

public class RabbitMQProvider
{
    private IModel _channel;
    private IConnection _connection;
    private IConnectionFactory _factory;
    private IBasicProperties _properties;
    private readonly string _appName = AppDomain.CurrentDomain.FriendlyName;
    private readonly string _machineName = Environment.MachineName;
    
    private IModel CreateChannel()
    {
        try
        {
            CreateConnection();
            
            if (_channel != null)
            {
                if (_channel.IsOpen) return _channel;
            }
            
            _channel = _connection!.CreateModel();
            
            var channelNumber = _channel.ChannelNumber;
            
            // _channel.ModelShutdown += HandleShutdownEvent;
            // _channel.CallbackException += HandleCallbackException;
            // _channel.BasicReturn += HandleBasicReturn;

            return _channel;
        }
        catch (BrokerUnreachableException)
        {
            // ignore
        }
        catch (OperationInterruptedException)
        {
            // ignore
        }
        catch (IOException)
        {
            // ignore
        }
        catch (Exception)
        {
            // ignore
        }
        
        return _channel!;
    }
    
    private void CreateConnection()
    {
        try
        {
            if (_factory != null) return;

            if (_connection != null)
            {
                if (_connection.IsOpen) return;
            }

            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                ClientProvidedName = $"App: {_appName}, Machine: {_machineName}",
                RequestedConnectionTimeout = TimeSpan.FromMilliseconds(1000),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromMilliseconds(1000),
            };

            _connection = _factory.CreateConnection();
            
            // _connection.ConnectionShutdown += HandleShutdownEvent;
            // _connection.CallbackException += HandleCallbackException;
            // _connection.ConnectionBlocked += HandleBlockedEvent;
        }
        catch (BrokerUnreachableException)
        {
            // ignore
        }
        catch (OperationInterruptedException)
        {
            // ignore
        }
        catch (SocketException)
        {
            // ignore
        }
        catch (Exception)
        {
            // ignore
        }
    }

    public void Publish(EventDto eventDto)
    {
        try
        {
            var messageId =  Guid.NewGuid().ToString();
            
            _channel = CreateChannel();
            
            _channel.ConfirmSelect();

            _channel.ExchangeDeclare(
                exchange: eventDto.Exchange,
                type: eventDto.ExchangeType,
                durable: true,
                autoDelete: false,
                arguments: null
            );
            
            _properties = _channel.CreateBasicProperties();
            _properties.Persistent = true; 
            _properties.ContentType = "application/json"; 
            _properties.Priority = 0; 
            _properties.MessageId = messageId;
            _properties.DeliveryMode = 2;

            _channel.BasicPublish(
                exchange: eventDto.Exchange,
                routingKey: eventDto.RoutingKey,
                basicProperties: _properties,
                body: Encoding.UTF8.GetBytes(eventDto.Message)
            );

            var confirmReceived = _channel.WaitForConfirms(TimeSpan.FromMilliseconds(250));

            if (!confirmReceived)
                throw new Exception("RabbitMQ channel confirmation failed");
        }
        catch (Exception exception)
        {
            // ignore
        }
    }
    
    public void Subscribe(EventDto eventDto, IEventHandler eventHandler)
    {
        try
        {
            _channel = CreateChannel();

            _channel.ExchangeDeclare(
                exchange: eventDto.Exchange,
                type: eventDto.ExchangeType,
                durable: true,
                autoDelete: false,
                arguments: null
            );
            
            _channel.QueueDeclare(
                queue: eventDto.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            
            _channel.QueueBind(queue: eventDto.QueueName, exchange: eventDto.Exchange, routingKey: eventDto.RoutingKey);
            
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var messageId = ea.BasicProperties.MessageId;
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    eventHandler.Handle(message);
                    
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception exception)
                {
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true); 
                }
            };
            
            _channel.BasicConsume(queue: eventDto.QueueName, autoAck: false, consumer: consumer);
        }
        catch (Exception)
        {
            // ignore
        }
    }

    // private void HandleShutdownEvent(object sender, ShutdownEventArgs e)
    // {
    //     switch (sender)
    //     {
    //         case IConnection:
    //             break;
    //         case IModel:
    //             break;
    //     }
    // }
    //
    // private void HandleCallbackException(object sender, CallbackExceptionEventArgs e)
    // {
    //     switch (sender)
    //     {
    //         case IConnection:
    //             break;
    //         case IModel:
    //             break;
    //     }
    // }
    //
    // private void HandleBlockedEvent(object sender, ConnectionBlockedEventArgs e)
    // {
    // }
    //
    // private void HandleBasicReturn(object sender, BasicReturnEventArgs e)
    // {
    // }
}