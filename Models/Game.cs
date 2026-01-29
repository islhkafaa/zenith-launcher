using System;

namespace Zenith_Launcher.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string InstallPath { get; set; } = string.Empty;
        public string CoverImagePath { get; set; } = string.Empty;
        public DateTime? LastPlayed { get; set; }
        public TimeSpan PlayTime { get; set; }
        public string StoreId { get; set; } = string.Empty;
        public string LaunchParameters { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
    }
}
