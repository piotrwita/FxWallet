using FxWallet.Application.Abstractions;
using FxWallet.Application.Wallets.Dtos;

namespace FxWallet.Application.Wallets.Queries;

public sealed record GetWallet(Guid WalletId) : IQuery<WalletDto?>;