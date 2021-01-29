﻿using Bank.Application.Accounts.Commands;
using Bank.DomainModel.Accounts;
using Bank.DomainModel.Accounts.Events;
using Bank.MaterializedView.Accounts.Views;
using Bank.Orchestrators.Contracts;
using MassTransit;
using MediatR;
using SeedWorks;
using SeedWorks.Core.Events;
using SeedWorks.Core.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bank.Application.Accounts
{
    internal class BankAccountCommandHandler:
        IRequestHandler<CreateBankAccountCommand, Guid>,
        IRequestHandler<ChangeOwnerCommand>,
        IRequestHandler<PerformDepositeCommand>,
        IRequestHandler<PerformWithdrawalCommand>,
        IRequestHandler<RebuildAccountsViewsCommand>,
        IRequestHandler<TransferBetweenAccountsCommand>
    {
        private readonly IRepository<BankAccount> _repository;
        private readonly IBusControl _bus;

        public BankAccountCommandHandler(IRepository<BankAccount> repository, IBusControl bus)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        /// <summary>
        /// Обработчик команды открытия расчетного счета.
        /// </summary>
        public Task<Guid> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
            => BankAccount.Create(request.Owner, request.CorrelationId)
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
            => await _repository.RebuildViews(new[]
                {
                        typeof(BankAccount),
                        typeof(BankAccountDetailsView),
                        typeof(BankAccountShortInfoView),
                }, cancellationToken).ContinueWith(_ => Unit.Value, TaskContinuationOptions.OnlyOnRanToCompletion);

        public async Task<Unit> Handle(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
        {
            await _bus.Publish(new SumTransferStarted
            {
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.TargetAccountId,
                Sum = request.Sum,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
            return Unit.Value;
        }

        private async Task<Unit> TransformEntity(Guid accountId, Action<BankAccount> processEvent, CancellationToken cancellationToken)
            => (await _repository.Find(accountId, cancellationToken))
                .Do(bankAccount => processEvent(bankAccount))
                .Do(bankAccount => _repository.Update(bankAccount, cancellationToken).Wait())
                .PipeTo(_ => Unit.Value);
    }
}