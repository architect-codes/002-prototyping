using Microsoft.Extensions.DependencyInjection;

namespace prototyping.tests
{
    /// <summary>
    /// base class for all unit tests
    /// </summary>
    public class BaseUnitTest
    {
        internal IServiceProvider GetServices()
        {
            var services = new ServiceCollection();

            services.AddMediatR(
                conf =>
                {
                    conf.RegisterServicesFromAssemblyContaining(typeof(BaseUnitTest));
                });

            return services.BuildServiceProvider();
        }
    }
}