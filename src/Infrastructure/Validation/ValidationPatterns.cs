using System.Text.RegularExpressions;

namespace Infrastructure.Validation;

public static partial class ValidationPatterns
{
    public const string NicknamePattern = @"^[\p{L}\p{N}\-_\. ]+$";
    [GeneratedRegex(NicknamePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex GetNicknameRegex();
    public static readonly Regex NicknameRegex = GetNicknameRegex();
}