﻿using System.IO;
using System.Linq;

namespace AIStudio.Api.Controllers.Download.Mock
{
    public static class DummyFileHelper
    {
        public const string TempFilesExtension = ".temp";
        public const string SampleFile1KbName = "Sample1Kb.test";
        public const string SampleFile16KbName = "Sample16Kb.test";
        public static readonly string TempDirectory = Path.GetTempPath();
        public static int Port => 5000;
        public static int FileSize1Kb => 1024;
        public static int FileSize16Kb => 16*1024;
        public static readonly byte[] File1Kb = DummyData.GenerateOrderedBytes(FileSize1Kb);
        public static readonly byte[] File16Kb = DummyData.GenerateOrderedBytes(FileSize16Kb);

        public static string GetFileUrl(int size)
        {
            return $"http://localhost:{Port}/dummyfile/file/size/{size}";
        }

        public static string GetFileWithNameUrl(string filename, int size)
        {
            return $"http://localhost:{Port}/dummyfile/file/{filename}?size={size}";
        }

        public static string GetFileWithNameOnRedirectUrl(string filename, int size)
        {
            return $"http://localhost:{Port}/dummyfile/file/{filename}/redirect?size={size}";
        }

        public static string GetFileWithoutHeaderUrl(string filename, int size)
        {
            return $"http://localhost:{Port}/dummyfile/noheader/file/{filename}?size={size}";
        }

        public static string GetFileWithContentDispositionUrl(string filename, int size)
        {
            return $"http://localhost:{Port}/dummyfile/file/{filename}/size/{size}";
        }

        public static string GetFileWithNoAcceptRangeUrl(string filename, int size)
        {
            return $"http://localhost:{Port}/dummyfile/file/{filename}/size/{size}/norange";
        }

        public static string GetFileWithFailureAfterOffset(int size, int failureOffset)
        {
            return $"http://localhost:{Port}/dummyfile/file/size/{size}/failure/{failureOffset}";
        }

        public static string GetFileWithTimeoutAfterOffset(int size, int timeoutOffset)
        {
            return $"http://localhost:{Port}/dummyfile/file/size/{size}/timeout/{timeoutOffset}";
        }

        public static bool AreEqual(this byte[] expected, Stream actual)
        {
            using (actual)
            {
                actual.Seek(0, SeekOrigin.Begin);
                return actual?.Length == expected.Length &&
                       expected.All(expectedByte => actual.ReadByte() == expectedByte);
            }
        }
    }
}
