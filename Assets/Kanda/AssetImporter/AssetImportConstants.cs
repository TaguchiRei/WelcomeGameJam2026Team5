using System.IO;

namespace WelcomeGameJam2026Team5.Editor.AssetImporter
{
    public static class AssetImportConstants
    {
        public const string WindowTitle = "Asset Import Tool";
        public const string MenuPath = "WelcomeGameJam2026Team5/Import";
        public const string WorkingDirectory = "Temp/AssetImporter";
        public const string GoogleDriveRootFolderId = "1A91a8gelcB24_DB-PbmfW6Nwa1r2f8qN";
        
        public const int ProgressUpdateIntervalMs = 100;
        public const int DefaultTimeoutMs = 120000;
        
        public static class Messages
        {
            public const string LoadingResources = "リソース読み込み中";
            public const string ImportWaiting = "インポート実行待機中";
            public const string ImportInProgress = "アセットインポート中";
            public const string ImportCompleted = "アセットのインポートが完了しました";
            public const string ImportFailed = "アセットのインポートに失敗しました";
            public const string ResourcesLoaded = "リソース読み込みが完了しました";
            public const string DoNotPlayScene = "インポートが終わるまでUnityのシーンを再生しないでください！";
            public const string Initializing = "初期化中...";
            public const string ProgrammerSection = "-----------------これより下はプログラマー用-----------------";
        }
        
        public static class ProgressMessages
        {
            public const string DownloadStarting = "ダウンロード開始";
            public const string Downloading = "ダウンロード中";
            public const string FileSaving = "ファイル保存中";
            public const string ExtractionStarting = "解凍開始";
            public const string Extracting = "解凍中";
            public const string Importing = "インポート中";
            public const string Completed = "完了";
            public const string Error = "エラー";
            
            public const string DownloadStartDetail = "アセットのダウンロードを開始しています...";
            public const string FileSaveDetail = "ダウンロードしたファイルを保存しています...";
            public const string ExtractionStartDetail = "ZIPファイルの解凍を開始しています...";
            public const string ImportDetail = "UnityPackageをインポートしています...";
            public const string CompletedDetail = "アセットのダウンロードと解凍が完了しました";
        }
        
        public static class FileExtensions
        {
            public const string UnityPackage = "*.unitypackage";
            public const string UnityPackageExtension = ".unitypackage";
            public const string Zip = ".zip";
        }

        public static class GoogleDrive
        {
            public const string FolderMimeType = "application/vnd.google-apps.folder";
            public const string FolderUrlFormat = "https://drive.google.com/drive/folders/{0}";
            public const string DownloadUrlFormat = "https://drive.google.com/uc?export=download&id={0}";
        }

        public static string GetWorkingDirectory()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), WorkingDirectory);
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
