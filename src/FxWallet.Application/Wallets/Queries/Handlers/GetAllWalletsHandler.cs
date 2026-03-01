using FxWallet.Application.Abstractions;
using FxWallet.Application.Wallets.Dtos;
using FxWallet.Application.Wallets.Mapping;
using FxWallet.Domain.Wallets;

namespace FxWallet.Application.Wallets.Queries.Handlers;

internal sealed class GetAllWalletsHandler(IWalletRepository walletRepository) : IQueryHandler<GetAllWallets, IReadOnlyList<WalletDto>>
{
    public async Task<IReadOnlyList<WalletDto>> HandleAsync(GetAllWallets query, CancellationToken cancellationToken = default)
    {
        var wallets = await walletRepository.GetAllAsync(cancellationToken);
        return WalletMapper.MapToDto(wallets);
    }
}