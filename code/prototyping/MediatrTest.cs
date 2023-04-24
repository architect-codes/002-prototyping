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
            var med = GetServices().GetRequiredService<IMediator>();

            await med.Publish(new NotifyOfSomething() { EventName = "some notification" } );
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

    public class BusinessEvent: INotification
    {
        public Guid Id { get; set; }
    }

    public class NotifyOfSomething : BusinessEvent
    {
        public required string EventName { get; set; }
    }

    public class NofificationHandler1 : INotificationHandler<NotifyOfSomething>
    {
        readonly IDummyService _dummyService;

        public NofificationHandler1(IDummyService dummyService)
        {
            _dummyService = dummyService;
        }

        public async Task Handle(NotifyOfSomething notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Handler 1");
            await _dummyService.DoSomething();
        }
    }

    public class NofificationHandler2 : INotificationHandler<NotifyOfSomething>
    {
        public async Task Handle(NotifyOfSomething notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Handler 2");
            await Task.CompletedTask;
        }
    }
    #endregion
}