using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.DtoInternal
{
    public class ProductionCompanyDtoInternalMapper : IDataMapper<ProductionCompanyDto, InternalProductionCompany>
    {
        public InternalProductionCompany Map(ProductionCompanyDto obj)
        {
            return new InternalProductionCompany()
            {
                Name = obj.Name,
                Origin_country = obj.OriginCountry
            };
        }

        public ProductionCompanyDto Map(InternalProductionCompany obj)
        {
            return new ProductionCompanyDto()
            {
                Name = obj.Name,
                OriginCountry = obj.Origin_country
            };
        }
    }
}
