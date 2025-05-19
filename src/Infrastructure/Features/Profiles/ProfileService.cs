using Application.Features.Profiles.Abstractions;
using Application.Features.Profiles.Create;
using Application.Features.Profiles.Get;
using BaboonCo.Utility.Result.ResultErrors;
using BaboonCo.Utility.Result.ResultErrors.Enums;
using Domain.Entities;
using EntityFramework.Exceptions.Common;
using FluentResults;
using Infrastructure.Databases;
using Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Features.Profiles;

public class ProfileService(
    ApplicationDbContext dbContext,
    ILogger<ProfileService> logger
) : IProfileService
{
    public async Task<Result<CreateProfileResponse>> CreateProfileAsync(
        CreateProfileDto dto,
        CancellationToken ct)
    {
        var profile = new Profile
        {
            Nickname = dto.Nickname,
            UserId = dto.UserId,
            PlayerStats = new PlayerStats()
        };

        if (!ValidationPatterns.NicknameRegex.IsMatch(dto.Nickname))
        {
            logger.LogInformation("Invalid nickname: {Nickname}.", dto.Nickname);
            return Result.Fail(new RequestError("Invalid nickname", RequestErrorType.BadRequest));
        }

        try
        {
            dbContext.Profiles.Add(profile);
            await dbContext.SaveChangesAsync(ct);
        }
        catch (UniqueConstraintException e)
        {
            var nicknameViolation = e.ConstraintProperties[0] == nameof(profile.Nickname);
            if (nicknameViolation)
            {
                logger.LogInformation(e, "Profile already exists with Nickname: {Nickname}.", profile.Nickname);
                return Result.Fail(new RequestError("Profile with this Nickname already exists.",
                    RequestErrorType.AlreadyExists));
            }

            var userIdViolation = e.ConstraintProperties[0] == nameof(profile.UserId);
            if (userIdViolation)
            {
                logger.LogInformation(e, "Profile already exists with UserId: {UserId}.", profile.UserId);
                return Result.Fail(new RequestError("Profile with this UserId already exists.",
                    RequestErrorType.AlreadyExists));
            }
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Failed to create profile for Nickname and UserId: {Nickname}, {UserId}.",
                profile.Nickname, profile.UserId);
            return Result.Fail(new RequestError("Failed to create profile", RequestErrorType.Internal));
        }

        return Result.Ok(new CreateProfileResponse());
    }

    public async Task<Result<ProfileResponseDto>> GetProfileAsync(
        ProfileRequestDto requestDto,
        CancellationToken ct)
    {
        var profile = await dbContext.Profiles
            .AsNoTracking()
            .Where(p => p.Nickname == requestDto.Nickname)
            .Include(p => p.PlayerStats)
            .FirstOrDefaultAsync(ct);

        if (profile is null)
            return Result.Fail(new RequestError("Profile not found", RequestErrorType.NotFound));

        var response = new ProfileResponseDto(profile.PlayerStats.Wins, profile.PlayerStats.Losses);
        return Result.Ok(response);
    }
}