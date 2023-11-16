namespace NexusMods.Paths.Extensions;

/// <summary>
/// Path related extensions tied to strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts an existing path represented as a string to a <see cref="RelativePath"/>.
    /// </summary>
    public static RelativePath ToRelativePath(this string s) => new(s);
}
