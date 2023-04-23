using MediatR;
using Shouldly;

using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace prototyping.tests
{

    [TestClass]
    public class MediatrDemoTest: BaseUnitTest
    {
        [TestMethod]
        public async Task SendTest()
        {
            var srvc = GetServices();

            var med = srvc.GetRequiredService<IMediator>();

            var response = await med.Send(new Ping01());
            response.ShouldBe("Pong-01");
        }

        [TestMethod]
        public async Task NotifyTest()
        {
            var srvc = GetServices();

            var med = srvc.GetRequiredService<IMediator>();

            await med.Publish(new Ping_Notify() { EventName = "some event" } );
        }
    }

    #region handler
    public class Ping01 : IRequest<string> { }

    public class PingHandler01 : IRequestHandler<Ping01, string>
    {
        public async Task<string> Handle(Ping01 request, CancellationToken cancellationToken)
        {
            return await Task.FromResult("Pong-01");
        }
    }
    #endregion

    #region notification
    public class Ping_Notify : INotification 
    {
        public required string EventName { get; set; }
    }

    public class NofificationHandler1 : INotificationHandler<Ping_Notify>
    {
        public async Task Handle(Ping_Notify notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 1");
            await Task.CompletedTask;
        }
    }

    public class NofificationHandler2 : INotificationHandler<Ping_Notify>
    {
        public async Task Handle(Ping_Notify notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 2");
            await Task.CompletedTask;
        }
    }
    #endregion
}