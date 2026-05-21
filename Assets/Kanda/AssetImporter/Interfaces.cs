using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WelcomeGameJam2026Team5.Editor.AssetImporter
{
    public struct ProgressInfo
    {
        public string Status { get; set; }
        public float Progress { get; set; }
        public string Detail { get; set; }
    }

    public interface IProgressReporter
    {
        event Action<ProgressInfo> OnProgressChanged;
        void ReportProgress(ProgressInfo progress);
    }

    public interface IAssetDownloader
    {
        Task<string> DownloadAssetAsync(Asset asset, string outputDirectory, CancellationToken cancellationToken);
    }

    public interface IFileExtractor
    {
        Task<string> ExtractZipFileAsync(string zipPath, string extractPath, CancellationToken cancellationToken);
    }

    public interface IReleaseService
    {
        Task<List<Release>> GetReleasesAsync(CancellationToken cancellationToken);
    }

    public interface IAssetImportService
    {
        Task<List<Release>> GetReleasesAsync(CancellationToken cancellationToken);
        Task<string> DownloadAndExtractAssetAsync(List<Asset> assets, CancellationToken cancellationToken);
        void ImportUnityPackages(string extractPath, bool showImportDialog = false);
        event Action<ProgressInfo> OnProgressChanged;
    }

    public interface IAssetImportController
    {
        bool IsImporting { get; }
        List<string> ReleaseNames { get; }
        int SelectedReleaseIndex { get; set; }

        Task InitializeAsync();
        Task ImportSelectedAssetAsync();

        event Action<ProgressInfo> OnProgressChanged;
        event Action<string> OnStatusChanged;
    }
}
