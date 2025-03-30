﻿using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces.Infrastructure.Repositories.Common;

namespace RapidPay.Domain.Interfaces.Infrastructure.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>;
}
