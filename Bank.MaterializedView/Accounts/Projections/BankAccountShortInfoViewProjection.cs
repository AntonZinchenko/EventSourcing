using Bank.MaterializedView.Accounts.Views;
using Bank.DomainModel.Accounts.Events;
using System;
using Marten.Events.Projections;

namespace Bank.MaterializedView.Accounts.Projections
{
    public class BankAccountShortInfoViewProjection : ViewProjection<BankAccountShortInfoView, Guid>
    {
        public BankAccountShortInfoViewProjection()
        {
            ProjectEvent<BankAccountCreated>(e => e.AccountId, Apply);
            ProjectEvent<DepositePerformed>(e => e.AccountId, Apply);
            ProjectEvent<OwnerChanged>(e => e.AccountId, Apply);
            ProjectEvent<WithdrawalPerformed>(e => e.AccountId, Apply);
        }

        private void Apply(BankAccountShortInfoView view, BankAccountCreated @event)
        {
            view.Id = @event.AccountId;
            view.Owner = @event.Owner;
            view.Balance = 0;
        }

        private void Apply(BankAccountShortInfoView view, DepositePerformed @event)
        {
            view.Id = @event.AccountId;
            view.Balance += @event.Sum;
        }

        private void Apply(BankAccountShortInfoView view, OwnerChanged @event)
        {
            view.Id = @event.AccountId;
            view.Owner = @event.NewOwner;
        }

        private void Apply(BankAccountShortInfoView view, WithdrawalPerformed @event)
        {
            view.Id = @event.AccountId;
            view.Balance -= @event.Sum;
        }
    }
}
