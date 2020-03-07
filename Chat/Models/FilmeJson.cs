using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class FilmeJson
    {
        public int MovieID { get; set; }

        /// <summary>
        /// Title of the Movie (string)
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Title")]

        public string Title { get; set; }

        /// <summary>
        /// List of the Genres (string)
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Genre")]
        public string Genre { get; set; }

        /// <summary>
        /// Link to Poster (string)
        /// </summary>

        [Newtonsoft.Json.JsonProperty("Poster")]
        public string Poster { get; set; }

        /// <summary>
        /// Available Languages (string)
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Language")]
        public string Language { get; set; }

        /// <summary>
        /// short summery of the Plot (string)
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Plot")]
        public string Plot { get; set; }

        /// <summary>
        /// Awards List (string)
        /// </summary>
        [Newtonsoft.Json.JsonProperty("Awards")]
        public string Awards { get; set; }

        /// <summary>
        /// Like counter (int)
        /// </summary>
        public int Likes { get; set; }

    }
}
