Dictionary<long, List<string>> files = new Dictionary<long, List<string>>();
Console.WriteLine("Args: {0}", string.Join("','", ScriptArgs));

if(ScriptArgs[0] == "-scan")
{
 if(Directory.Exists(ScriptArgs[1]))
 {
  Console.WriteLine("Scanning {0}", ScriptArgs[1]);
  ScanFolders(new System.IO.DirectoryInfo(ScriptArgs[1]));
  Console.WriteLine("\n\n******** Scanning {0} COMPLETE ********", ScriptArgs[1]);
  foreach(var v in files)
  {
   if(v.Value.Count > 1)
   {
    Console.WriteLine("Following {0} Files have the same size \n -'{1}'", v.Value.Count, string.Join("',\n -'", v.Value));
   }
  }
 }
}

void ScanFolders(System.IO.DirectoryInfo dirInfo)
{
 try
 {
  IEnumerable<System.IO.FileInfo> files = dirInfo.EnumerateFiles("*.mp3");
  Parallel.ForEach<FileInfo>(files, SaveName);
  IEnumerable<DirectoryInfo> directories = dirInfo.EnumerateDirectories();
  if (directories.Count<DirectoryInfo>() > 0)
  {
   Parallel.ForEach<DirectoryInfo>(directories, ScanFolder);
  }
 }
 catch(Exception ex)
 {
  Console.WriteLine("Borked: {0}", ex.Message);
 }
}

void ScanFolder(System.IO.DirectoryInfo currentFolder, ParallelLoopState arg2, long arg3)
{
 ScanFolders(currentFolder);
}

void SaveName(FileInfo currentFileInfo, ParallelLoopState arg2, long arg3)
{
 if(files.ContainsKey(currentFileInfo.Length))
 {
  files[currentFileInfo.Length].Add(currentFileInfo.FullName);
 }
 else
 {
  files[currentFileInfo.Length] = new List<string>();
  files[currentFileInfo.Length].Add(currentFileInfo.FullName);
 }
}