using FxWallet.Application.Abstractions;
using FxWallet.Application.Wallets.Dtos;

namespace FxWallet.Application.Wallets.Queries;

public sealed record GetAllWallets : IQuery<IReadOnlyList<WalletDto>>;