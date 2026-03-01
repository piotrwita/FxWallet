using FxWallet.Application.Abstractions;
using FxWallet.Application.Wallets.Dtos;
using FxWallet.Application.Wallets.Mapping;
using FxWallet.Domain.Wallets;

namespace FxWallet.Application.Wallets.Queries.Handlers;

internal sealed class GetWalletHandler(IWalletRepository walletRepository) : IQueryHandler<GetWallet, WalletDto?>
{
    public async Task<WalletDto?> HandleAsync(GetWallet query, CancellationToken cancellationToken = default)
    {
        var wallet = await walletRepository.GetByIdAsync(query.WalletId, cancellationToken);
        if (wallet is null)
        {
            return null;
        }

        return WalletMapper.MapToDto(wallet);
    }
}