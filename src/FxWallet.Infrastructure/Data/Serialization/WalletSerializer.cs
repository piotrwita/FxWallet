using System.Reflection;
using FxWallet.Domain.Wallets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FxWallet.Infrastructure.Data.Serialization;

internal static class WalletSerializer
{
    private static readonly JsonSerializerSettings _settings = new()
    {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new ContractResolverWithPrivates()
    };

    public static string Serialize(Wallet wallet)
        => JsonConvert.SerializeObject(wallet, _settings);

    public static Wallet Deserialize(string json)
        => JsonConvert.DeserializeObject<Wallet>(json, _settings)
            ?? throw new InvalidOperationException($"Failed to deserialize JSON to Wallet");

    internal sealed class ContractResolverWithPrivates : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            List<JsonProperty> props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Select(p => base.CreateProperty(p, memberSerialization))
                .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(f => base.CreateProperty(f, memberSerialization)))
                .ToList();
            
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }
}
