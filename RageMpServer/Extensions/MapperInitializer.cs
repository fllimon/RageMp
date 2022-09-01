using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageMpServer.Extensions
{
    class MapperInitializer
    {
        private static IMapper _mapper = null;

        public static IMapper GetInstance()
        {
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Models.Player, Entity.Player>()
                    .ForPath(x => x.Position.X, option => option.MapFrom(x => x.Position.PositionX))
                    .ForPath(x => x.Position.Y, option => option.MapFrom(x => x.Position.PositionY))
                    .ForPath(x => x.Position.Z, option => option.MapFrom(x => x.Position.PositionZ))
                    .ForPath(x => x.Rotation.X, option => option.MapFrom(x => x.Position.RotationX))
                    .ForPath(x => x.Rotation.Y, option => option.MapFrom(x => x.Position.RotationY))
                    .ForPath(x => x.Rotation.Z, option => option.MapFrom(x => x.Position.RotationZ));

                    cfg.CreateMap<Entity.Player, Models.Player>()
                    .ForPath(x => x.Position.PositionX, option => option.MapFrom(x => x.Position.X))
                    .ForPath(x => x.Position.PositionX, option => option.MapFrom(x => x.Position.Y))
                    .ForPath(x => x.Position.PositionX, option => option.MapFrom(x => x.Position.Z))
                    .ForPath(x => x.Position.RotationX, option => option.MapFrom(x => x.Rotation.X))
                    .ForPath(x => x.Position.RotationY, option => option.MapFrom(x => x.Rotation.Y))
                    .ForPath(x => x.Position.RotationZ, option => option.MapFrom(x => x.Rotation.Z));
                });

                _mapper = new Mapper(config);
            }

            return _mapper;
        }
    }
}
