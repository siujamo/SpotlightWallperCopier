# Spotlight Wallpaper Copier

A command-line tool developed with .NET 9 that copies the current Windows Spotlight wallpaper from its system location to the user's Downloads folder.

## Usage

Run the programme from the command line using the following syntax:

```shell
SpotlightWallpaperCopier [options]
```

### Options

- `-n, --dst-file-name <name>`  
  Specifies the destination filename (without extension). Default is "UntitledWallpaper".

- `-h, --help`  
  Displays this help information.

- `-v, --version`  
  Shows the programme version.

### Examples

1. Copy with default filename:
   ```shell
   SpotlightWallpaperCopier
   ```

2. Copy with custom filename:
   ```shell
   SpotlightWallpaperCopier --dst-file-name MyWallpaper
   ```

3. Show help:
   ```shell
   SpotlightWallpaperCopier --help
   ```

4. Show version:
   ```shell
   SpotlightWallpaperCopier --version
   ```

## Requirements

- Windows operating system
- .NET 9 runtime

## Notes

- The programme copies the wallpaper from: `C:\Users\<username>\AppData\Roaming\Microsoft\Windows\Themes\TranscodedWallpaper`
- The file is saved to the user's Downloads folder with a `.jpg` extension
- If a file with the same name exists, a number will be appended (e.g., "MyWallpaper (1).jpg")