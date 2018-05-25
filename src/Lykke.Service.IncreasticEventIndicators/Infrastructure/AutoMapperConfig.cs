using System.Collections.Generic;
using Autofac;
using AutoMapper;
using Lykke.Service.IncreasticEventIndicators.AutoMapperProfiles;

namespace Lykke.Service.IncreasticEventIndicators.Infrastructure
{
    internal static class AutoMapperConfig
    {
        public static void RegisterAutoMapper(this ContainerBuilder builder)
        {
            builder
                .RegisterType<DefaultProfile>()
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                    .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}
