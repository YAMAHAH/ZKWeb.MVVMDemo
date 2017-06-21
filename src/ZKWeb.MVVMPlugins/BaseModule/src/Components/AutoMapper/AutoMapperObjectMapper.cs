using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZKWebStandard.Ioc;

namespace InfrastructurePlugins.BaseModule.Components.AutoMapper
{
    [ExportMany,SingletonReuse]
    public class AutoMapperObjectMapper : IObjectMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
