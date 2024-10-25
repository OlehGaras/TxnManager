using AutoMapper;
using TxnManager.Domain.Model;
using TxnManager.Web.Dto;

namespace TxnManager.Web.AutoMapper
{
    public class DomainToDtoProfile: Profile
    {
        public DomainToDtoProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(t => t.Payment, opt => opt.MapFrom(src => $"{src.Amount} {src.CurrencyCode}"))
                .ForMember(t => t.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
