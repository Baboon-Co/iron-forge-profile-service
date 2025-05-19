using Application.Features.Profiles.Abstractions;
using Application.Features.Profiles.Create;
using Application.Features.Profiles.Get;
using BaboonCo.Utility.Grpc.Server;
using BaboonCo.Utility.Result.ResultErrors;
using BaboonCo.Utility.Result.ResultErrors.Enums;
using FluentResults;
using Grpc.Core;
using Infrastructure.Validation;
using IronForge.Contracts.ProfileService;
using CreateProfileResponse = IronForge.Contracts.ProfileService.CreateProfileResponse;
using GetProfileResponse = IronForge.Contracts.ProfileService.GetProfileResponse;

namespace Api.GrpcControllers;

public class ProfileGrpcController(
    IProfileService profileService,
    ILogger<ProfileGrpcController> logger
) : Profile.ProfileBase
{
    public override async Task<GetProfileResponse> GetProfile(
        GetProfileRequest request,
        ServerCallContext context)
    {
        var profileDto = new ProfileRequestDto(request.Nickname);
        var getProfileResult = await profileService.GetProfileAsync(profileDto, context.CancellationToken);
        if (getProfileResult.IsSuccess)
        {
            logger.LogInformation("Profile {Nickname} retrieved.", request.Nickname);

            var profile = getProfileResult.Value;
            return new GetProfileResponse
            {
                Wins = profile.Wins,
                Losses = profile.Losses
            };
        }

        throw GrpcServerHelper.CreateRpcException(getProfileResult);
    }

    public override async Task<CreateProfileResponse> CreateProfile(
        CreateProfileRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            throw GrpcServerHelper.CreateRpcException(Result.Fail(
                new RequestError("Invalid userId", RequestErrorType.BadRequest)).ToResult());
        }

        if (!ValidationPatterns.NicknameRegex.IsMatch(request.Nickname))
        {
            throw GrpcServerHelper.CreateRpcException(Result
                .Fail(new RequestError("Invalid nickname", RequestErrorType.BadRequest)).ToResult());
        }

        var profileDto = new CreateProfileDto(userId, request.Nickname);
        var createProfileResult = await profileService.CreateProfileAsync(profileDto, context.CancellationToken);
        if (createProfileResult.IsSuccess)
        {
            logger.LogInformation("Profile {Nickname} created.", request.Nickname);
            return new CreateProfileResponse();
        }

        throw GrpcServerHelper.CreateRpcException(createProfileResult);
    }
}