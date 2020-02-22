using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class NetworkDtoInternalMapper : IDataMapper<NetworkDto, InternalNetwork>
    {
        public InternalNetwork Map(NetworkDto obj)
        {
            return new InternalNetwork() {
                Name = obj.Name,
                Origin_country = obj.OriginCountry
            };

        }

        public NetworkDto Map(InternalNetwork obj)
        {
            return new NetworkDto() {
                Name = obj.Name,
                OriginCountry = obj.Origin_country
            };
        }
    }
}
