namespace Chat.Controller
{
    using Chat.Models;
    using Chat.Models.Repository;
    using Microsoft.AspNetCore.Mvc;
    using Nancy.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FilmeController" />
    /// </summary>
    [Route("api/Filme")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        /// <summary>
        /// Defines the _dataRepository
        /// </summary>
        private readonly IDataRepository<Filme> _dataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilmeController"/> class.
        /// </summary>
        /// <param name="dataRepository">The dataRepository<see cref="IDataRepository{Filme}"/></param>
        public FilmeController(IDataRepository<Filme> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        /// <summary>
        /// List all movies from the DB
        /// </summary>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var filme = _dataRepository.GetAll();
            return Ok(filme);
        }

        /// <summary>
        /// Addup the Like counter in DB
        /// </summary>
        /// <param name="id">DB MovieID<see cref="int"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet("likes")]
        public IActionResult Get(int id)
        {
            Filme filmToUpdate = _dataRepository.Get(id);
            Filme filmneu = filmToUpdate;
            if (filmneu != null)
            {
                filmneu.Likes++;
            }
            else
            {
                return NotFound("Keine Filme gefunden");
            }

            _dataRepository.Update(filmToUpdate, filmneu);
            return NoContent();
        }
    }
}
