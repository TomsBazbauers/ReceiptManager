using AutoMapper;
using ReceiptManager.Core.Models;
using ReceiptManager.Models;

namespace ReceiptManager.RequestHandlers
{
    public class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptRequest, Receipt>()
                .ForMember(x => x.Id, options => options.Ignore())
                .ForMember(x => x.CreatedOn, options => options.Ignore())
                .ForMember(x => x.Items, opt => opt.MapFrom(y => y.Items));

                cfg.CreateMap<Receipt, ReceiptRequest>()
                .ForMember(x => x.Items, opt => opt.MapFrom(y => y.Items));
            });

            config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }
    }
}
