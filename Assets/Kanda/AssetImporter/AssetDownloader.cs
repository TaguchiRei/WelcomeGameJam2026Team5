using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using sepLog = WelcomeGameJam2026Team5.Editor.Logger;

namespace WelcomeGameJam2026Team5.Editor.AssetImporter
{
    public class AssetDownloader : IAssetDownloader, IProgressReporter
    {
        private readonly bool _enableLogger;

        public event Action<ProgressInfo> OnProgressChanged;

        public AssetDownloader(bool enableLogger = true)
        {
            _enableLogger = enableLogger;
        }

        public async Task<string> DownloadAssetAsync(Asset asset, string outputDirectory, CancellationToken cancellationToken)
        {
            var safeFileName = SanitizeFileName(asset.Name);
            var outputPath = Path.Combine(outputDirectory, safeFileName);
            var downloadUrl = string.IsNullOrEmpty(asset.URL)
                ? string.Format(AssetImportConstants.GoogleDrive.DownloadUrlFormat, asset.ID)
                : asset.URL;

            ReportProgress(new ProgressInfo
            {
                Status = AssetImportConstants.ProgressMessages.DownloadStarting,
                Progress = 0f,
                Detail = $"{asset.Name} のダウンロードを開始しています..."
            });

            try
            {
                await DownloadFileAsync(downloadUrl, outputPath, cancellationToken);
                sepLog.Logger.LogInfo($"UnityPackageダウンロード完了: ID={asset.ID}, Path={outputPath}", _enableLogger);
                return outputPath;
            }
            catch (Exception e) when (!(e is AssetDownloadException))
            {
                var error = $"ダウンロード中に予期しないエラーが発生: {e.Message}";
                sepLog.Logger.LogError(error);
                throw new AssetDownloadException(error, e);
            }
        }

        private async Task DownloadFileAsync(string url, string outputPath, CancellationToken cancellationToken)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                request.downloadHandler = new DownloadHandlerFile(outputPath) { removeFileOnAbort = true };
                await SendRequestAsync(request, cancellationToken);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new AssetDownloadException($"ダウンロードエラー: {request.error}");
                }

                var contentType = request.GetResponseHeader("Content-Type") ?? string.Empty;
                if (contentType.IndexOf("text/html", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    File.Delete(outputPath);
                    var confirmedUrl = await GetConfirmedDownloadUrlAsync(url, cancellationToken);
                    await DownloadConfirmedFileAsync(confirmedUrl, outputPath, cancellationToken);
                }
            }
        }

        private async Task<string> GetConfirmedDownloadUrlAsync(string url, CancellationToken cancellationToken)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                await SendRequestAsync(request, cancellationToken);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new AssetDownloadException($"Drive確認ページ取得エラー: {request.error}");
                }

                var html = request.downloadHandler.text;
                var confirmMatch = Regex.Match(html, @"confirm=([0-9A-Za-z_-]+)");
                if (!confirmMatch.Success)
                {
                    throw new AssetDownloadException("Google Driveのダウンロード確認トークンを取得できませんでした");
                }

                if (!Regex.IsMatch(url, @"[?&]id=([^&]+)"))
                {
                    throw new AssetDownloadException("Google DriveファイルIDを取得できませんでした");
                }

                return $"{url}&confirm={confirmMatch.Groups[1].Value}";
            }
        }

        private async Task DownloadConfirmedFileAsync(string url, string outputPath, CancellationToken cancellationToken)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                request.downloadHandler = new DownloadHandlerFile(outputPath) { removeFileOnAbort = true };
                await SendRequestAsync(request, cancellationToken);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new AssetDownloadException($"確認後ダウンロードエラー: {request.error}");
                }
            }
        }

        private async Task SendRequestAsync(UnityWebRequest request, CancellationToken cancellationToken)
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                var progress = request.downloadProgress < 0 ? 0f : request.downloadProgress;
                ReportProgress(new ProgressInfo
                {
                    Status = AssetImportConstants.ProgressMessages.Downloading,
                    Progress = progress,
                    Detail = $"ダウンロード進捗: {progress * 100:F1}%"
                });

                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return $"Asset_{DateTime.Now:yyyyMMdd_HHmmss}{AssetImportConstants.FileExtensions.UnityPackageExtension}";
            }

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidChar, '_');
            }

            if (!Path.GetExtension(fileName).Equals(
                    AssetImportConstants.FileExtensions.UnityPackageExtension,
                    StringComparison.OrdinalIgnoreCase))
            {
                fileName += AssetImportConstants.FileExtensions.UnityPackageExtension;
            }

            return fileName;
        }

        public void ReportProgress(ProgressInfo progress)
        {
            OnProgressChanged?.Invoke(progress);
        }
    }
}
