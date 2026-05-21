using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using sepLog = WelcomeGameJam2026Team5.Editor.Logger;

namespace WelcomeGameJam2026Team5.Editor.AssetImporter
{
    public class AssetImportService : IAssetImportService
    {
        private readonly IReleaseService _releaseService;
        private readonly IAssetDownloader _assetDownloader;
        private readonly IFileExtractor _fileExtractor;
        private readonly bool _enableLogger;

        public event Action<ProgressInfo> OnProgressChanged;

        public AssetImportService(
            IReleaseService releaseService = null,
            IAssetDownloader assetDownloader = null,
            IFileExtractor fileExtractor = null,
            bool enableLogger = true)
        {
            _enableLogger = enableLogger;
            _releaseService = releaseService ?? new ReleaseService(enableLogger);
            _assetDownloader = assetDownloader ?? new AssetDownloader(enableLogger);
            _fileExtractor = fileExtractor ?? new FileExtractor(enableLogger);

            // 進捗イベントの購読
            if (_assetDownloader is IProgressReporter downloadReporter)
                downloadReporter.OnProgressChanged += ReportProgress;
            if (_fileExtractor is IProgressReporter extractReporter)
                extractReporter.OnProgressChanged += ReportProgress;
        }

        public async Task<List<Release>> GetReleasesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _releaseService.GetReleasesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                sepLog.Logger.LogError($"リリース取得エラー: {e.Message}");
                throw;
            }
        }

        public async Task<string> DownloadAndExtractAssetAsync(List<Asset> assets, CancellationToken cancellationToken)
        {
            try
            {
                var downloadDirectory = GetDownloadDirectory();
                sepLog.Logger.LogInfo($"UnityPackage保存先パス: {downloadDirectory}", _enableLogger);

                foreach (var asset in assets.Where(asset => IsUnityPackage(asset.Name)).OrderBy(asset => asset.Name))
                {
                    await _assetDownloader.DownloadAssetAsync(asset, downloadDirectory, cancellationToken);
                }

                ReportProgress(new ProgressInfo
                {
                    Status = AssetImportConstants.ProgressMessages.Completed,
                    Progress = 1f,
                    Detail = "UnityPackageのダウンロードが完了しました"
                });

                return downloadDirectory;
            }
            catch (Exception e)
            {
                ReportProgress(new ProgressInfo
                {
                    Status = AssetImportConstants.ProgressMessages.Error,
                    Progress = 0f,
                    Detail = $"エラー: {e.Message}"
                });
                throw;
            }
        }

        public void ImportUnityPackages(string extractPath, bool showImportDialog = false)
        {
            try
            {
                ReportProgress(new ProgressInfo
                {
                    Status = AssetImportConstants.ProgressMessages.Importing,
                    Progress = 1f,
                    Detail = AssetImportConstants.ProgressMessages.ImportDetail
                });

                var files = Directory.GetFiles(extractPath, AssetImportConstants.FileExtensions.UnityPackage, SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    sepLog.Logger.LogInfo($"UnityPackageをインポート: {file}", _enableLogger);
                    AssetDatabase.ImportPackage(file, showImportDialog);
                }

                if (!AssetImportSettings.FileChecking)
                {
                    Directory.Delete(extractPath, true);
                    sepLog.Logger.LogInfo($"解凍フォルダを削除: {extractPath}", _enableLogger);
                }
            }
            catch (Exception e)
            {
                var error = $"UnityPackageインポート中にエラーが発生: {e.Message}";
                sepLog.Logger.LogError(error);
                throw new AssetImportException(error, e);
            }
        }

        private static bool IsUnityPackage(string fileName)
        {
            return Path.GetExtension(fileName)
                .Equals(AssetImportConstants.FileExtensions.UnityPackageExtension, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetDownloadDirectory()
        {
            var downloadDirectory = Path.Combine(
                AssetImportConstants.GetWorkingDirectory(),
                $"DriveUnityPackages_{DateTime.Now:yyyyMMdd_HHmmss}");

            var counter = 1;
            var originalPath = downloadDirectory;
            while (Directory.Exists(downloadDirectory))
            {
                downloadDirectory = $"{originalPath}_{counter}";
                counter++;
            }

            Directory.CreateDirectory(downloadDirectory);
            return downloadDirectory;
        }

        private void CleanupTemporaryFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    sepLog.Logger.LogInfo($"一時ファイル削除: {filePath}", _enableLogger);
                }
            }
            catch (Exception e)
            {
                sepLog.Logger.LogError($"一時ファイル削除エラー: {e.Message}");
            }
        }

        private void ReportProgress(ProgressInfo progress)
        {
            OnProgressChanged?.Invoke(progress);
        }
    }
}
