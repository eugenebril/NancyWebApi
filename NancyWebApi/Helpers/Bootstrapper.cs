using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.TinyIoc;
using NancyWebApi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NyTimesApi.Options;
using NyTimesApi.Services;

namespace NancyWebApi.Helpers
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        readonly IServiceProvider _serviceProvider;

        public Bootstrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<NytSettings>((NytSettings)_serviceProvider.GetService(typeof(NytSettings)));
            container.Register<INyTimesService, NyTimesService>();
            container.Register<IArticleService, ArticleService>();
          //  container.Register<ILoggerFactory>((ILoggerFactory)_serviceProvider.GetService<ILoggerFactory>());
        }
    }
}
