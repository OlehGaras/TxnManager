using AutoMapper;
using TxnManager.Data.EntityFramework.Entities;
using TxnManager.Domain.Model;

namespace TxnManager.Data.EntityFramework.AutoMapper
{
    public class DomainToEntityProfile: Profile
    {
        public DomainToEntityProfile()
        {
            CreateMap<Transaction, TransactionEntity>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(t => t.Status, opt => opt.MapFrom(src => (int) src.Status));

            CreateMap<TransactionEntity, Transaction>()
                .ConstructUsing(e => new Transaction(e.Id, e.Amount, e.CurrencyCode, e.TransactionDate, (TransactionStatus)e.Status));
        }
    }
}
