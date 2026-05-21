using System;
using System.Collections.Generic;

namespace WelcomeGameJam2026Team5.Editor.AssetImporter
{
    [Serializable]
    public struct Release
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string TagName { get; set; }
        public string PublishedAt { get; set; }
        public List<Asset> Assets { get; set; }
    }

    [Serializable]
    public struct Asset
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public long Size { get; set; }
    }
}

// データサンプル
// {
//     [
//         id: 219025835,
//         name: 'Release main',
//         tag_name: 'main',
//         published_at: '2025-05-16T07:52:08Z',
//         assets: 
//             [
//                 {
//                     id: 255181855,
//                     name: 'Unity.zip',
//                     download_url: 'https://github.com/RyuichiroYoshida/SepDriveActions/releases/download/main/Unity.zip',
//                     size: 63003695
//                 },
//             ]
//     ],
// }
