using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;



namespace Chat.Models
{
    /// <summary>
    /// Defines the <see cref="Filme" />
    /// </summary>
  
    public class Filme
    {
        /// <summary>
        /// Gets or sets the MovieID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int MovieID { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// Title of the Movie (string)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Genre
        /// List of the Genres (string)
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the Poster
        /// Link to Poster (string)
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the Language
        /// Available Languages (string)
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the Plot
        /// short summery of the Plot (string)
        /// </summary>
        public string Plot { get; set; }

        /// <summary>
        /// Gets or sets the Awards
        /// Awards List (string)
        /// </summary>
        public string Awards { get; set; }

        /// <summary>
        /// Gets or sets the Likes
        /// Like counter (int)
        /// </summary>
        public int Likes { get; set; }
        public string Metascore { get; set; }
        public string Rated { get; set; }
        public string Year { get; set; }
        public string Runtime { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Type { get; set; }
        public string imdbID { get; set; }
        public string username { get; set; }
    }
}