using FxWallet.Application.Wallets.Dtos;
using FxWallet.Domain.Wallets;

namespace FxWallet.Application.Wallets.Mapping;

internal static class WalletMapper
{
    public static WalletDto MapToDto(Wallet wallet)
    {
        ArgumentNullException.ThrowIfNull(wallet);

        var balances = wallet.Balances
            .Select(b => new WalletBalanceDto(b.Balance.Currency.Code, b.Balance.Amount))
            .ToList();

        return new WalletDto(wallet.Id.Value, wallet.Name.Value, balances);
    }

    public static IReadOnlyList<WalletDto> MapToDto(IReadOnlyList<Wallet> wallets)
    {
        ArgumentNullException.ThrowIfNull(wallets);

        return [.. wallets.Select(MapToDto)];
    }
}
