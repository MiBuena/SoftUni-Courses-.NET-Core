using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMapperProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AppProfile>();
                cfg.CreateMap<Source, Destination>();
            });

            var firstSource = new Source()
            {
                Id = 1,
                Name = "Name",
                Description = "Description"
            };

            var secondSource = new Source()
            {
                Id = 2,
                Name = "Name2",
                Description = "Description2"
            };
           
            IMapper mapper = new Mapper(config);
            var dest = mapper.Map<Source, Destination>(firstSource);




            List<Source> list = new List<Source>();
            list.Add(firstSource);
            list.Add(secondSource);

            IQueryable<Source> a = list.AsQueryable();
            var collectionDestination = a.ProjectTo<Destination>(config);
        }
    }
}
