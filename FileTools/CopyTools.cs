using static FileTools.CopyStrategies;

namespace FileTools
{
    public static class CopyTools
    {
        public abstract record Result;
        public record Success : Result;
        public record Aborted(string message) : Result;
        public record Cancelled : Result;
        public record Failure(string message) : Result;

      
        public static async Task<Result> CopyAsync(CopyPlan plan,
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
                if(token.IsCancellationRequested)
                    return new Cancelled();

                try
                {
                    if (!dstDir.Exists)
                        Directory.CreateDirectory(dstDir.FullName);
                    else if (conflictStrategy == ConflictStrategy.Abort)
                        return new Aborted($"Directory already exists: {dstDir.FullName}");
                }
                catch (Exception ex)
                {
                    if (fileLockStrategy == FileLockStrategy.Abort)
                        return new Failure($"Error creating directory {dstDir.FullName}: {ex.Message}");
                }
            }

            foreach (var (srcFile, dstFile) in plan.Files)
            {
                token.ThrowIfCancellationRequested();

                copyProgress.CurrentFile = srcFile.FullName;
                progress?.Report(copyProgress);

                if (dstFile.Exists)
                {
                    switch (conflictStrategy)
                    {
                        case ConflictStrategy.Skip:
                            continue;

                        case ConflictStrategy.Abort:
                            return new Aborted($"File already exists: {dstFile.FullName}");

                        case ConflictStrategy.Overwrite:
                            try
                            {
                                dstFile.Delete();
                            }
                            catch (Exception ex)
                            {
                                if (fileLockStrategy == FileLockStrategy.Abort)
                                    return new Failure($"Error deleting file {dstFile.FullName}: {ex.Message}");
                                else
                                    continue;
                            }
                            break;
                    }
                }


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
                        return new Failure($"Error copying file: {srcFile.FullName} -> {dstFile.FullName}: {ex.Message}");
                }
            }

            return new Success();
        }

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
