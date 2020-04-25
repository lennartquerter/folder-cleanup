using System.Collections.Generic;

namespace Lenimal.FolderCleanup.Models
{
    public class CleanupFolder
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public bool RemoveSource { get; set; }

        public Dictionary<string, List<string>> Format { get; set; }
    }
}