using System;
using System.Threading.Tasks;
using DingoUnityExtensions.Utils;
using UnityEngine;
using UnityEngine.Scripting;

namespace ProjectAppStructure.Core.AppRootCore
{
    [Serializable, Preserve]
    public class DataRoot
    {
        public PathUtils.PathPrefix PathPrefix;
        public string Path;

        public async Task<string> LoadDataAsync()
        {
            var filePath = FullPath();
            if (filePath == null)
                return null;
            try
            {
                var data = await MultiplatformLoadUtils.LoadSerializedStringAsync(filePath);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        public string FullPath()
        {
            var filePath = PathUtils.MakePathWithPrefix(PathPrefix, Path, false);
            return filePath;
        }
    }
    
    [Serializable, Preserve]
    public class ImageDataRoot
    {
        public Texture2D BlankTexture;
        public PathUtils.PathPrefix PathPrefix;
        public string RootPath;

        public async Task<Texture> LoadImage(string fileName)
        {
            var imagePath = FullImagePath(fileName);
            if (imagePath == null)
                return BlankTexture;
            try
            {
                return await MultiplatformLoadUtils.LoadTexture2DAsync(imagePath);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return BlankTexture;
            }
        }

        public string FullImagePath(string fileName)
        {
            var directoryPath = PathUtils.MakePathWithPrefix(PathPrefix, RootPath, false);
            var imagePath = PathUtils.MakeImagePath(directoryPath, fileName);
            return imagePath;
        }
    }
    
    [Serializable, Preserve]
    public class VideoDataRoot
    {
        public string VideoExtension;
        public PathUtils.PathPrefix PathPrefix;
        public string RootPath;

        public string VideoPath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;
            var path = PathUtils.MakePathWithPrefix(PathPrefix, RootPath, false);
            var videoPath = $"{path}{fileName}.{VideoExtension}";
            return videoPath;
        }
    }
}