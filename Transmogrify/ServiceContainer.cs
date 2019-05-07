using Autofac;
using Autofac.Core;
using Transmogrify.Services;

namespace Transmogrify
{
    static class ServiceContainer
    {
        public static void Build()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<ProjectService>()
                .SingleInstance();

            builder
                .RegisterType<LibraryService>()
                .SingleInstance();

            Container = builder.Build();
        }

        private static ILifetimeScope Container { get; set; }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Resolve<T>(Parameter[] parameters)
        {
            return Container.Resolve<T>(parameters);
        }
    }
}
