using System;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VishanSabha.Models
{
    public class Activities
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ActivityDate { get; set; }

        // This is the raw JSON string saved in DB
        public string ImageJson { get; set; }

        // This property serializes/deserializes JSON string to List<string>
        public List<string> ImagePaths
        {
            get => string.IsNullOrEmpty(ImageJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(ImageJson);
            set => ImageJson = JsonConvert.SerializeObject(value);
        }

        public string VideoUrl { get; set; }
        public bool Status { get; set; }

        public string VideoFile { get; set; }
        public string LinkType { get; set; }
    }
}