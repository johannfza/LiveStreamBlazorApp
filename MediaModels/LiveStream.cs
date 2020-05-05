using System;
using System.ComponentModel.DataAnnotations;

namespace MediaModels
{
    public class LiveStream
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string StreamKey { get; set; }

        public string DatePublished { get; set; }
        public int Views { get; set; }

        public LiveStream()
        {
        }

        public LiveStream(int id, string title, string description, string url, string streamKey)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            DatePublished = DateTime.UtcNow.ToLocalTime().ToString();
            Views = 0;
            StreamKey = streamKey;

        }

        public override string ToString()
        {
            return Title + ": " + Description;
        }
    }

}
