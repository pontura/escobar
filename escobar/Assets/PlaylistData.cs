using System;

[Serializable]
public class PlaylistData
{
    public VideoData[] playlist;
    public string playlistID;

    [Serializable]
    public class VideoData
    {
        public string playlistID;
        public string title;
        public string description;
        public string image;
        public Sources[] sources;
    }
    [Serializable]
    public class Sources
    {
        public int width;
        public string file;
    }
}
