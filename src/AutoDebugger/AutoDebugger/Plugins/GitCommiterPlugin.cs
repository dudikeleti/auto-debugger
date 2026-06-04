using Microsoft.SemanticKernel;
using System.ComponentModel;
using LibGit2Sharp;
using AutoDebugger.Misc;

namespace AutoDebugger.Plugins
{
    internal class GitCommiterPlugin : DiagnosticPlugin
    {
        [KernelFunction, Description("Commit code changes to a git repository and return the commit sha")]
        public string Commit([Description("The code to commit")] string commitContent)
        {
            var commitToTrace = commitContent?.Length > 100 ? commitContent[..99] + " ..." : commitContent;
            try
            {
                using var repo = new Repository(Config.RepositoryPath);
                var fileToCommit = Path.GetFileName(Path.GetTempFileName()) + ".txt";
                File.WriteAllText(Path.Combine(repo.Info.WorkingDirectory, fileToCommit), commitContent);

                // Stage and commit
                repo.Index.Add(fileToCommit);
                repo.Index.Write();

                Signature author = new Signature("Dudi", "dudi@dudi.com", DateTime.Now);
                Signature committer = author;
                Commit commit = repo.Commit("Committed by Auto Debugger", author, committer);

                CaptureCall(nameof(Commit),
                    commit.Sha,
                    new Dictionary<string, object?>
                    {
                        {nameof(commitContent), commitToTrace}
                    });

                return commit.Sha;
            }
            catch (Exception e)
            {
                var errorReturn = $"There was error when trying to send http get request. Error: {e.Message}";
                CaptureCall(nameof(Commit),
                    errorReturn,
                    new Dictionary<string, object?>
                    {
                        {nameof(commitContent), commitToTrace}
                    });

                return errorReturn;
            }
        }
    }
}
