namespace FxWallet.Domain.Shared.Exceptions;

internal sealed class NegativeResultException(decimal currentAmount, decimal subtractingAmount)
    : CustomException($"Subtraction would result in negative amount. Current: {currentAmount}, subtracting: {subtractingAmount}.");
