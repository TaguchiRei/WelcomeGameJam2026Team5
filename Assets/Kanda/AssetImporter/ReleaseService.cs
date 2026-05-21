using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using sepLog = WelcomeGameJam2026Team5.Editor.Logger;

namespace WelcomeGameJam2026Team5.Editor.AssetImporter
{
    public class ReleaseService : IReleaseService
    {
        private readonly bool _enableLogger;

        public ReleaseService(bool enableLogger = true)
        {
            _enableLogger = enableLogger;
        }

        public async Task<List<Release>> GetReleasesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var packages = await GetUnityPackagesInFolderAsync(
                    AssetImportConstants.GoogleDriveRootFolderId,
                    cancellationToken);

                return packages
                    .OrderBy(asset => asset.Name)
                    .Select(asset => new Release
                    {
                        ID = asset.ID,
                        Name = asset.Name,
                        TagName = asset.Name,
                        Assets = new List<Asset> { asset }
                    })
                    .ToList();
            }
            catch (Exception e) when (!(e is ReleaseServiceException))
            {
                var error = $"Google DriveからUnityPackage一覧を取得できませんでした: {e.Message}";
                sepLog.Logger.LogError(error);
                throw new ReleaseServiceException(error, e);
            }
        }

        private async Task<List<Asset>> GetUnityPackagesInFolderAsync(string rootFolderId, CancellationToken cancellationToken)
        {
            var results = new List<Asset>();
            var visitedFolderIds = new HashSet<string>();
            await CollectUnityPackagesAsync(rootFolderId, results, visitedFolderIds, cancellationToken);
            sepLog.Logger.LogInfo($"UnityPackage検出数: {results.Count}", _enableLogger);
            return results;
        }

        private async Task CollectUnityPackagesAsync(
            string folderId,
            List<Asset> results,
            HashSet<string> visitedFolderIds,
            CancellationToken cancellationToken)
        {
            if (!visitedFolderIds.Add(folderId))
            {
                return;
            }

            var folderUrl = string.Format(AssetImportConstants.GoogleDrive.FolderUrlFormat, folderId);
            var html = await GetTextAsync(folderUrl, cancellationToken);
            var items = ParseDriveItems(html);

            foreach (var folder in items.Where(item => item.IsFolder))
            {
                await CollectUnityPackagesAsync(folder.ID, results, visitedFolderIds, cancellationToken);
            }

            foreach (var package in items.Where(item => item.IsUnityPackage))
            {
                results.Add(new Asset
                {
                    ID = package.ID,
                    Name = package.Name,
                    URL = string.Format(AssetImportConstants.GoogleDrive.DownloadUrlFormat, package.ID),
                    Size = package.Size
                });

                sepLog.Logger.LogInfo($"UnityPackage検出: {package.Name} ({package.ID})", _enableLogger);
            }
        }

        private static List<DriveItem> ParseDriveItems(string html)
        {
            var dataMatch = Regex.Match(
                html,
                @"window\['_DRIVE_ivd'\]\s*=\s*'(?<data>(?:\\.|[^'])*)'",
                RegexOptions.Singleline);

            if (!dataMatch.Success)
            {
                return new List<DriveItem>();
            }

            var decoded = DecodeJavaScriptString(dataMatch.Groups["data"].Value);
            var itemPattern = new Regex(
                @"\[""(?<id>[A-Za-z0-9_-]{10,})"",\[""(?<parent>[A-Za-z0-9_-]{10,})""\],""(?<name>(?:\\.|[^""])*)"",""(?<mime>(?:\\.|[^""])*)""",
                RegexOptions.Singleline);

            return itemPattern.Matches(decoded)
                .Cast<Match>()
                .Select(match => new DriveItem
                {
                    ID = match.Groups["id"].Value,
                    Name = DecodeJavaScriptString(match.Groups["name"].Value),
                    MimeType = DecodeJavaScriptString(match.Groups["mime"].Value)
                })
                .GroupBy(item => item.ID)
                .Select(group => group.First())
                .ToList();
        }

        private static string DecodeJavaScriptString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var decoded = Regex.Replace(value, @"\\x([0-9A-Fa-f]{2})", match =>
                ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString());

            decoded = Regex.Replace(decoded, @"\\u([0-9A-Fa-f]{4})", match =>
                ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString());

            return decoded
                .Replace(@"\/", "/")
                .Replace("\\\"", "\"")
                .Replace(@"\\", @"\");
        }

        private static async Task<string> GetTextAsync(string url, CancellationToken cancellationToken)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new ReleaseServiceException($"Driveフォルダ取得エラー: {request.error}");
                }

                return request.downloadHandler.text;
            }
        }

        private struct DriveItem
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string MimeType { get; set; }
            public long Size { get; set; }
            public bool IsFolder => MimeType == AssetImportConstants.GoogleDrive.FolderMimeType;
            public bool IsUnityPackage => Path.GetExtension(Name)
                .Equals(AssetImportConstants.FileExtensions.UnityPackageExtension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
