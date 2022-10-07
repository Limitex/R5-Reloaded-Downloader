# [Deprecated] R5-Reloaded-Downloader

A safer and more stable of R5 Reloaded Downloader that replaces R5 Reloaded Installer.

Only C# HttpClient is used for downloading and 7z.dll is used for decompressing the archive.

Torrents are not used.

# Files included within this app

## 7z.dll

This application requires "7z.dll" to decompress the 7z archive, so it is generated in the Temp directory at runtime.
Since it is in use during execution, it cannot be deleted, but it can be removed after the application is terminated.

https://sevenzip.osdn.jp/


# ApplicationType

Either can be installed.

## CLI

![CUI](https://user-images.githubusercontent.com/76650151/158708512-8f582fcc-0e77-457a-b919-4e520c76b3ee.png)

## GUI

![GUI](https://user-images.githubusercontent.com/76650151/158708524-94ebd218-fd18-44e2-aac9-d7275b1eda52.png)

# How to use

## CLI

Start the downloader and enter y.
A directory called R5-Reloaded is created in the same directory, and R5-Reloaded is created in that directory.

## GUI

Run and select the location where you want to install R5-Reloaded.
Click Install to continue.
R5-Reloaded will be created in the selected directory.
