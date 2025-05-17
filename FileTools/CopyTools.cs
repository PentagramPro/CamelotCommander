using static FileTools.CopyStrategies;

namespace FileTools
{
    public static class CopyTools
    {
        public static async Task<CopyPlan> PrepareAsync(IEnumerable<string> sourcePaths, string destinationRoot, CancellationToken token = default)
        {
            var plan = new CopyPlan();

            var destinationRootDir = new DirectoryInfo(destinationRoot);
            if (!destinationRootDir.Exists)
            {
                plan.Warnings.Add($"Target directory doesn't exist: {destinationRoot}");
                return plan;
            }

            foreach (var sourcePath in sourcePaths)
            {
                token.ThrowIfCancellationRequested();

                var sourceAttr = File.GetAttributes(sourcePath);
                if ((sourceAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var srcDir = new DirectoryInfo(sourcePath);
                    await TraverseDirectoryRecursiveAsync(srcDir, destinationRootDir, plan, token);
                }
                else
                {
                    var srcFile = new FileInfo(sourcePath);
                    var destFile = new FileInfo(Path.Combine(destinationRoot, srcFile.Name));

                    plan.Files.Add((srcFile, destFile));
                    plan.TotalSizeBytes += srcFile.Length;

                    if (destFile.Exists)
                        plan.AlreadyExists.Add(destFile);
                }
            }

            return plan;
        }

        public static async Task CopyAsync(CopyPlan plan,
            ConflictStrategy conflictStrategy = ConflictStrategy.Abort,
            FileLockStrategy fileLockStrategy = FileLockStrategy.Skip,
            IProgress<CopyProgress>? progress = null,
            CancellationToken token = default)
        {
            var copyProgress = new CopyProgress
            {
                TotalFiles = plan.Files.Count,
                TotalBytes = plan.TotalSizeBytes,
                FilesCopied = 0,
                BytesCopied = 0
            };

            foreach (var (srcDir, dstDir) in plan.Directories)
            {
                token.ThrowIfCancellationRequested();

                try
                {
                    if (!dstDir.Exists)
                    {
                        Directory.CreateDirectory(dstDir.FullName);
                    }
                    else if (conflictStrategy == ConflictStrategy.Abort)
                    {
                        throw new IOException($"Папка уже существует: {dstDir.FullName}");
                    }
                }
                catch (Exception ex)
                {
                    if (fileLockStrategy == FileLockStrategy.Abort)
                        throw new IOException($"Ошибка создания папки {dstDir.FullName}: {ex.Message}", ex);
                    else
                        continue;
                }
            }

            foreach (var (srcFile, dstFile) in plan.Files)
            {
                token.ThrowIfCancellationRequested();

                copyProgress.CurrentFile = srcFile.FullName;
                progress?.Report(copyProgress);

                bool skip = false;

                if (dstFile.Exists)
                {
                    switch (conflictStrategy)
                    {
                        case ConflictStrategy.Skip:
                            skip = true;
                            break;

                        case ConflictStrategy.Abort:
                            throw new IOException($"Файл уже существует: {dstFile.FullName}");

                        case ConflictStrategy.Overwrite:
                            try
                            {
                                dstFile.Delete();
                            }
                            catch (Exception ex)
                            {
                                if (fileLockStrategy == FileLockStrategy.Abort)
                                    throw new IOException($"Ошибка удаления файла: {dstFile.FullName}", ex);
                                else
                                    skip = true;
                            }
                            break;
                    }
                }

                if (skip)
                    continue;

                try
                {
                    dstFile.Directory?.Create();

                    using var sourceStream = new FileStream(srcFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var destStream = new FileStream(dstFile.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.None);

                    byte[] buffer = new byte[81920];
                    int bytesRead;
                    while ((bytesRead = await sourceStream.ReadAsync(buffer.AsMemory(0, buffer.Length), token)) > 0)
                    {
                        await destStream.WriteAsync(buffer.AsMemory(0, bytesRead), token);
                        copyProgress.BytesCopied += bytesRead;
                        progress?.Report(copyProgress);
                    }

                    copyProgress.FilesCopied++;
                    progress?.Report(copyProgress);
                }
                catch (IOException ex)
                {
                    if (fileLockStrategy == FileLockStrategy.Abort)
                        throw new IOException($"Ошибка копирования файла: {srcFile.FullName} -> {dstFile.FullName}", ex);
                    // иначе пропускаем
                }
            }
        }

        private static async Task TraverseDirectoryRecursiveAsync(
            DirectoryInfo sourceDir,
            DirectoryInfo destinationRoot,
            CopyPlan plan,
            CancellationToken token)
        {
            Queue<(DirectoryInfo source, DirectoryInfo destination)> queue = new();
            queue.Enqueue((sourceDir, new DirectoryInfo(Path.Combine(destinationRoot.FullName, sourceDir.Name))));

            while (queue.Count > 0)
            {
                token.ThrowIfCancellationRequested();
                var (currentSource, currentDest) = queue.Dequeue();


                plan.Directories.Add((currentSource, currentDest));
                if (currentDest.Exists)
                    plan.AlreadyExists.Add(currentDest);

                FileSystemInfo[] entries;
                try
                {
                    entries = currentSource.GetFileSystemInfos();
                }
                catch (Exception ex)
                {
                    plan.Warnings.Add($"Access error {currentSource.FullName}: {ex.Message}");
                    continue;
                }

                foreach (var entry in entries)
                {
                    token.ThrowIfCancellationRequested();

                    if (entry is DirectoryInfo subdir)
                    {
                        var subDest = new DirectoryInfo(Path.Combine(currentDest.FullName, subdir.Name));
                        queue.Enqueue((subdir, subDest));
                    }
                    else if (entry is FileInfo file)
                    {
                        var destFile = new FileInfo(Path.Combine(currentDest.FullName, file.Name));
                        plan.Files.Add((file, destFile));
                        plan.TotalSizeBytes += file.Length;

                        if (destFile.Exists)
                            plan.AlreadyExists.Add(destFile);
                    }
                }

                await Task.Yield();
            }
        }
    }
}
