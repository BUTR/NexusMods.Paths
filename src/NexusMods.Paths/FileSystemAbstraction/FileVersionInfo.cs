using System;
using JetBrains.Annotations;

namespace NexusMods.Paths;

/// <summary>
/// Represents version information for a file on disk.
/// </summary>
/// <param name="ProductVersion">Gets the version of the product this file is distributed with.</param>
/// <param name="FileVersion">Gets the file version number.</param>
[PublicAPI]
public record struct FileVersionInfo(Version ProductVersion, Version FileVersion)
{
    private static readonly Version Zero = new(0, 0, 0, 0);

    /// <summary>
    /// Returns the first non-zero version.
    /// </summary>
    public Version GetBestVersion()
    {
        if (ProductVersion.Equals(Zero)) return FileVersion;
        if (FileVersion.Equals(Zero)) return ProductVersion;
        return ProductVersion;
    }
}
