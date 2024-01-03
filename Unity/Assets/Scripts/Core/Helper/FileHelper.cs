using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ET
{
	public static class FileHelper
	{
		public static List<string> GetAllFiles(string dir, string searchPattern = "*.*")
		{
			List<string> list = new List<string>();
			GetAllFiles(list, dir, searchPattern);
			return list;
		}
		
		public static void GetAllFiles(List<string> files, string dir, string searchPattern = "*.*")
		{
			if (!Directory.Exists(dir))
				return;
			string[] fls = Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories);
			files.AddRange(fls);
		}
		
		public static void CleanDirectory(string dir)
		{
			if (!Directory.Exists(dir))
			{
				return;
			}
			foreach (string subdir in Directory.GetDirectories(dir))
			{
				Directory.Delete(subdir, true);		
			}

			foreach (string subFile in Directory.GetFiles(dir))
			{
				File.Delete(subFile);
			}
		}

		public static void CopyDirectory(string srcDir, string tgtDir)
		{
			DirectoryInfo source = new DirectoryInfo(srcDir);
			DirectoryInfo target = new DirectoryInfo(tgtDir);
	
			if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
			{
				throw new Exception("父目录不能拷贝到子目录！");
			}
	
			if (!source.Exists)
			{
				return;
			}
	
			if (!target.Exists)
			{
				target.Create();
			}
	
			FileInfo[] files = source.GetFiles();
	
			for (int i = 0; i < files.Length; i++)
			{
				File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
			}
	
			DirectoryInfo[] dirs = source.GetDirectories();
	
			for (int j = 0; j < dirs.Length; j++)
			{
				CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
			}
		}
		
		public static void ReplaceExtensionName(string srcDir, string extensionName, string newExtensionName)
		{
			if (Directory.Exists(srcDir))
			{
				string[] fls = Directory.GetFiles(srcDir);

				foreach (string fl in fls)
				{
					if (fl.EndsWith(extensionName))
					{
						File.Move(fl, fl.Substring(0, fl.IndexOf(extensionName)) + newExtensionName);
						File.Delete(fl);
					}
				}

				string[] subDirs = Directory.GetDirectories(srcDir);

				foreach (string subDir in subDirs)
				{
					ReplaceExtensionName(subDir, extensionName, newExtensionName);
				}
			}
		}
		
		public static void WriteAllBytes(string path, byte[] content)
		{
			var directoryName = Path.GetDirectoryName(path);
			if (directoryName != null)
			{
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllBytes(path, content);
			}
		}
		
		public static void WriteAllText(string path, string content)
		{
			var directoryName = Path.GetDirectoryName(path);
			if (directoryName != null)
			{
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllText(path, content, Encoding.UTF8);
			}
		}
		
		public static string[] ReadAllLines(string path)
		{
			if (!File.Exists(path))
				return Array.Empty<string>();
			return File.ReadAllLines(path);
		}

		public static void CreateDir(string directoryName)
		{
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
		}
		
		public static void CreateFile(string path)
		{
			if (File.Exists(path))
				return;
			var directoryName = Path.GetDirectoryName(path);
			if (directoryName != null)
			{
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
			}
		}

		public static FileStream CreateStream(string path, FileMode fileMode)
		{
			var directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				if (directoryName != null)
				{
					Directory.CreateDirectory(directoryName);
				}
			}

			return File.Open(path, fileMode);
		}
	}
}
