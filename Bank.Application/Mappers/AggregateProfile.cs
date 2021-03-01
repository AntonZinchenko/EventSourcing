using AutoMapper;
using BankAccount.Contracts.Views;

namespace BankAccount.Application.Mappers
{
    public class AggregateProfile : Profile
    {
        public AggregateProfile()
        {
            CreateMap<DomainModel.BankAccount, BankAccountShortInfoView>();
        }
    }
}
