using System;
using System.IO;
using System.Collections.Generic;
namespace DeskMetrics
{
	public class Cache
	{
		internal string ApplicationId
		{
			get;set;
		}
		
		private System.Object ObjectLock = new System.Object();
		
		
		internal bool Delete()
        {
            lock (ObjectLock)
            {
                string FileName = Path.GetTempPath() + ApplicationId + ".dsmk";
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                    return true;
                }
				return false;
            }
        }
		
		private string GetCacheFileName()
		{
			return Path.GetTempPath() + ApplicationId + ".dsmk";
		}
		private string GetFileContents(string FileName)
		{
			string FileContents = "";
			FileStream FileS = new FileStream(@FileName, FileMode.Open, FileAccess.Read);
            StreamReader Stream = new StreamReader(FileS);
            try
            {
                FileContents =  Util.DecodeFrom64(Stream.ReadToEnd());
            }
            finally
            {
                Stream.Close();
                FileS.Close();
            }
			return FileContents;
		}
		
        internal string GetCacheData()
        {
            lock (ObjectLock)
            {
                string FileName = GetCacheFileName();
                if (File.Exists(FileName))
                	return GetFileContents(FileName);
                return "";
            }
        }
		private Int64 GetCacheSize()
        {
			return GetCacheData().Length;
        }

		private FileStream GetOrCreateCacheFile(string FileName)
		{
			FileStream FileS = new FileStream(@FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			File.SetAttributes(FileName, FileAttributes.Hidden);
			return FileS;
		}
		
        internal void Save(List<string> JSON) 
        {
            lock (ObjectLock)
            {
                string FileName = GetCacheFileName();
				FileStream FileS = GetOrCreateCacheFile(FileName);
				StreamWriter StreamFile = new StreamWriter(FileS);
                if (FileS.Length == 0)
                    StreamFile.Write(Util.EncodeTo64(JsonBuilder.GetJsonFromList(JSON)));
                else
                    StreamFile.Write(","+Util.EncodeTo64(JsonBuilder.GetJsonFromList(JSON)),FileS.Length);
				StreamFile.Close();
				FileS.Close();
            }
        }
	}
}

