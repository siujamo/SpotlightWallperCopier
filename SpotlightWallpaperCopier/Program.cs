using System.Runtime.InteropServices;

namespace SpotlightWallpaperCopier;

internal static class Program
{
    /// <summary>
    /// Represents the GUID for the user's Downloads folder as defined by the Windows Shell.
    /// This identifier is used to retrieve the path to the Downloads folder via the Windows API.
    /// </summary>
    private static readonly Guid DownloadsFolderGuid = new Guid(
        "374DE290-123F-4565-9164-39C4925E467B");

    /// <summary>
    /// Retrieves the file system path of a known folder identified by its GUID, such as the
    /// Downloads folder.
    /// This method utilises the Windows Shell API to obtain the current user's folder path, even
    /// if it has been customised.
    /// </summary>
    /// <param name="rfid">The GUID of the known folder to retrieve, e.g.,
    /// <see cref="DownloadsFolderGuid"/> for Downloads.</param>
    /// <param name="dwFlags">Flags that specify the retrieval options. Typically set to 0 for
    /// default behaviour.</param>
    /// <param name="hToken">Handle to an access token for a specific user. Use
    /// <see cref="IntPtr.Zero"/> for the current user.</param>
    /// <param name="pszPath">When this method returns, contains the folder's path as a string.
    /// This parameter is passed uninitialised.</param>
    /// <returns>
    /// Returns 0 if the path is successfully retrieved; otherwise, returns a non-zero error code
    /// indicating failure.
    /// </returns>
    /// <remarks>
    /// This method is a P/Invoke call to the Windows shell32.dll library. It requires the
    /// application to run on a Windows platform.
    /// </remarks>
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern int SHGetKnownFolderPath(
        [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
        uint dwFlags,
        IntPtr hToken,
        out string pszPath);

    /// <summary>
    /// Generates a unique file path by appending a sequence number if the file already exists.
    /// </summary>
    /// <param name="directory">The directory where the file will be saved.</param>
    /// <param name="baseName">The base name of the file without extension.</param>
    /// <param name="extension">The file extension, e.g., ".jpg".</param>
    /// <returns>A unique file path.</returns>
    private static string GetUniqueFilePath(string directory, string baseName, string extension)
    {
        var filePath = Path.Combine(directory, baseName + extension);
        if (!File.Exists(filePath))
        {
            return filePath;
        }

        var seq = 1;
        while (true)
        {
            var newFilePath = Path.Combine(directory, $"{baseName} ({seq}){extension}");
            if (!File.Exists(newFilePath))
            {
                return newFilePath;
            }
            seq++;
        }
    }

    static void Main(string[] args)
    {
        // default destination file name
        var destFileName = "UntitledWallpaper";

        // parse command-line arguments
        for (var i = 0; i < args.Length; i++)
        {
            if ((args[i] != "--dst-file-name" && args[i] != "-n") || i + 1 >= args.Length) continue;
            destFileName = args[i + 1];
            i++; // skip the next argument
        }

        var userName = Environment.UserName;

        // source file path
        var srcPath = $@"C:\Users\{userName}\AppData\Roaming\Microsoft\Windows\Themes\TranscodedWallpaper";

        // retrieve Downloads folder path
        var result = SHGetKnownFolderPath(DownloadsFolderGuid, 0, IntPtr.Zero, out var downloadsPath);
        if (result != 0)
        {
            Console.WriteLine($"Failed to retrieve the Downloads folder path, error code: {result}");
            return;
        }

        // check if the source file exists
        if (!File.Exists(srcPath))
        {
            Console.WriteLine($"Error: The source file {srcPath} does not exist.");
            return;
        }

        // generate unique destination file path with .jpg extension
        var destPath = GetUniqueFilePath(downloadsPath, destFileName, ".jpg");

        // copy the file
        File.Copy(srcPath, destPath, true);
        Console.WriteLine($"File successfully copied: {srcPath} -> {destPath}");
    }
}