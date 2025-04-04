﻿using RapidPay.Domain.Interfaces.Infrastructure.Repositories;

namespace RapidPay.Domain.Interfaces.Infrastructure
{
    public interface IUnitOfWork
    {
        ICardRepository CardRepository { get; set; }
        IAuthorizationLogRepository AuthorizationLogRepository { get; set; }
        IFeeRepository FeeRepository { get; set; }
        ITransactionRepository TransactionRepository { get; set; }
        IManualCardUpdateRepository ManualCardUpdateRepository { get; set; }
        IUserRepository UserRepository { get; set; }
        Task SaveChangesAsync();
    }
}
