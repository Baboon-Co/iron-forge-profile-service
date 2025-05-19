using Application.Features.Profiles.Create;
using Application.Features.Profiles.Get;
using FluentResults;

namespace Application.Features.Profiles.Abstractions;

public interface IProfileService
{
    public Task<Result<ProfileResponseDto>> GetProfileAsync(ProfileRequestDto requestDto,
        CancellationToken ct = default);

    public Task<Result<CreateProfileResponse>> CreateProfileAsync(CreateProfileDto dto,
        CancellationToken ct = default);
}