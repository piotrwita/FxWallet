namespace FxWallet.Application.Wallets.Dtos;

public sealed record WalletDto(Guid Id, string Name, IReadOnlyList<WalletBalanceDto> Balances);

public sealed record WalletBalanceDto(string CurrencyCode, decimal Amount);