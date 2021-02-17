using BankAccount.MaterializedView.Views;
using BankAccount.DomainModel.Events;
using Marten.Events.Projections;
using System;

namespace BankAccount.MaterializedView.Projections
{
    public class BankAccountDetailsViewProjection : ViewProjection<BankAccountDetailsView, Guid>
    {
        public BankAccountDetailsViewProjection()
        {
            ProjectEvent<BankAccountCreated>(e => e.AccountId, Apply);
            ProjectEvent<DepositePerformed>(e => e.AccountId, Apply);
            ProjectEvent<OwnerChanged>(e => e.AccountId, Apply);
            ProjectEvent<WithdrawalPerformed>(e => e.AccountId, Apply);
        }

        private void Apply(BankAccountDetailsView view, BankAccountCreated @event)
        {
            view.Id = @event.AccountId;
            view.Owner = @event.Owner;
        }

        private void Apply(BankAccountDetailsView view, DepositePerformed @event)
        {
            view.Id = @event.AccountId;
            view.CashFlow.Add(new CashFlowItem(@event.Created, @event.Sum));
        }

        private void Apply(BankAccountDetailsView view, OwnerChanged @event)
        {
            view.Id = @event.AccountId;
            view.Owner = @event.NewOwner;
        }

        private void Apply(BankAccountDetailsView view, WithdrawalPerformed @event)
        {
            view.Id = @event.AccountId;
            view.CashFlow.Add(new CashFlowItem(@event.Created, (@event.Sum * -1)));
        }
    }
}
