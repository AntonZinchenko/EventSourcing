using BankAccount.Application.Commands;
using BankAccount.MaterializedView.Views;
using MediatR;
using SeedWorks;
using SeedWorks.Core.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccount.Application
{
    internal class commandHandler :
        IRequestHandler<CreateBankAccountCommand, Guid>,
        IRequestHandler<ChangeOwnerCommand>,
        IRequestHandler<PerformDepositeCommand>,
        IRequestHandler<PerformWithdrawalCommand>,
        IRequestHandler<RebuildAccountsViewsCommand>
    {
        private readonly IRepository<DomainModel.BankAccount> _repository;

        public commandHandler(IRepository<DomainModel.BankAccount> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Обработчик команды открытия расчетного счета.
        /// </summary>
        public Task<Guid> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
            => DomainModel.BankAccount.Create(request.Owner, request.CorrelationId)
                .Do(bankAccount => _repository.Add(bankAccount, cancellationToken).Wait())
                .PipeTo(bankAccount => Task.FromResult(bankAccount.Id));

        /// <summary>
        /// Обработчик команды начисления депозитных процентов.
        /// </summary>
        public async Task<Unit> Handle(PerformDepositeCommand request, CancellationToken cancellationToken)
            => await TransformEntity(
                request.AccountId,
                ag => ag.PerformDeposite(request.Sum, request.CorrelationId),
                cancellationToken);

        /// <summary>
        /// Обработчик команды переоформления счета на другого пользователя.
        /// </summary>
        public async Task<Unit> Handle(ChangeOwnerCommand request, CancellationToken cancellationToken)
            => await TransformEntity(
                request.AccountId,
                ag => ag.ChangeOwner(request.NewOwner, request.CorrelationId),
                cancellationToken);

        /// <summary>
        /// Обработчик команды списания с расчетного счета.
        /// </summary>
        public async Task<Unit> Handle(PerformWithdrawalCommand request, CancellationToken cancellationToken)
            => await TransformEntity(
                request.AccountId,
                ag => ag.PerformWithdrawal(request.Sum, request.CorrelationId),
                cancellationToken);

        /// <summary>
        /// Обработчик команды пересборки материализованных представлений.
        /// </summary>
        public async Task<Unit> Handle(RebuildAccountsViewsCommand request, CancellationToken cancellationToken)
            => await _repository.RebuildViews(
                new[]
                {
                        typeof(DomainModel.BankAccount),
                        typeof(BankAccountDetailsView),
                        typeof(BankAccountShortInfoView),
                }, cancellationToken).ContinueWith(_ => Unit.Value, TaskContinuationOptions.OnlyOnRanToCompletion);

        private async Task<Unit> TransformEntity(Guid accountId, Action<DomainModel.BankAccount> processEvent, CancellationToken cancellationToken)
            => (await _repository.Find(accountId, cancellationToken))
                .Do(bankAccount => processEvent(bankAccount))
                .Do(bankAccount => _repository.Update(bankAccount, cancellationToken).Wait())
                .PipeTo(_ => Unit.Value);
    }
}