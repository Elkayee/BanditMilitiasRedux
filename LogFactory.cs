using System;
using Bannerlord.ButterLib.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BanditMilitias
{
    internal static class LogFactory
    {
        internal static ILogger Get<T>()
        {
            try
            {
                var serviceProvider = SubModule.Instance?.GetServiceProvider()
                    ?? SubModule.Instance?.GetTempServiceProvider();
                if (serviceProvider is null)
                    return NullLogger<T>.Instance;
                return serviceProvider.GetService<ILogger<T>>() ?? NullLogger<T>.Instance;
            }
            catch
            {
                return NullLogger<T>.Instance;
            }
        }
    }
}