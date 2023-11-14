using Hubtel.Wallet.Api.Contracts.Requests.User;
using Hubtel.Wallet.Api.Contracts.Requests.Wallet;
using Hubtel.Wallet.Api.Models;
using Hubtel.Wallet.Api.Services.Common;
using Hubtel.Wallet.Api.Services.Requests.Commands.User;
using Hubtel.Wallet.Api.Services.Requests.Commands.Wallet;
using Hubtel.Wallet.Api.Utilities;
using Mapster;

namespace Hubtel.Wallet.Api.Mapper;

public class ApiMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserContract, CreateUserCommand>();

        config.NewConfig<CreateUserCommand, UserModel>();

        config.NewConfig<UserModel, UserResponse>()
            .Map(dest => dest.OwnedWallets, src => src.OwnedWallets);

        config.NewConfig<CreateWalletCommand, WalletModel>()
            .Map(dest => dest.Name, src => src.WalletName);

        config.NewConfig<WalletModel, WalletResponse>()
            .Map(dest => dest.Owner, src => src.OwnerPhoneNumber)
            .Map(dest => dest.WalletId, src => src.Id)
            .Map(dest => dest.WalletName, src => src.Name);

        config.NewConfig<CreateWalletContract, CreateWalletCommand>()
            .Map(dest => dest.AccountScheme, src => Converter.ConvertToSchemeType(src.AccountScheme))
            .Map(dest => dest.AccountType, src => Converter.ConvertToAccountType(src.AccountType));
    }
}
