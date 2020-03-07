namespace Chat.Models.DataManager
{
    using AutoMapper;
    using Chat.Models.Repository;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="FilmeManager" />
    /// </summary>
    public class FilmeManager : IDataRepository<Filme>
    {
        /// <summary>
        /// Defines the _employeeContext
        /// </summary>
        internal readonly FilmeContext _employeeContext;

        /// <summary>
        /// Defines the _mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilmeManager"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="FilmeContext"/></param>
        /// <param name="mapper">The mapper<see cref="IMapper"/></param>
        public FilmeManager(FilmeContext context, IMapper mapper)
        {
            _employeeContext = context;
            _mapper = mapper;
        }

        /// <summary>
        /// List all movies from the DB.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{Filme}"/></returns>
        public List<Filme> GetAll()
        {
            var filmeoutput = new List<Filme>();

            var filme = _employeeContext.Filme.ToList();
            foreach (var film in filme)
            {
                filmeoutput.Add(film);
            }
            return filmeoutput;
        }
      public List<Filme> GetFavorits(string username)
        {
            var filmeoutput= new List<Filme>();

            var allFilms = _employeeContext.Filme.ToList();
            var filme = allFilms.Where(e => e.username.Equals(username)).ToList();
            foreach (var film in filme)
            {
                filmeoutput.Add(film);
            }
            return filmeoutput;
        }
        

        /// <summary>
        /// Add a new movies to the DB.
        /// </summary>
        /// <param name="entity">Filme Model<see cref="Filme"/></param>
        public void Add(Filme entity)
        {
            _employeeContext.Add(entity);
            _employeeContext.SaveChanges();
        }

        /// <summary>
        /// Update a movies in the DB.
        /// </summary>
        /// <param name="film">Filme Model (old)<see cref="Filme"/></param>
        /// <param name="entity">Filme Model (new)<see cref="Filme"/></param>
        public void Update(Filme film, Filme entity)
        {
            film.MovieID = entity.MovieID;
            film.Title = entity.Title;
            film.Genre = entity.Genre;
            film.Poster = entity.Poster;
            film.Language = entity.Language;
            film.Plot = entity.Plot;
            film.Awards = entity.Awards;
            film.Likes = entity.Likes;

            _employeeContext.SaveChanges();
        }

        /// <summary>
        /// Delete a movies in the DB.
        /// </summary>
        /// <param name="film">The film<see cref="Filme"/></param>
        public void Delete(string imdbID, string username)
        {
            var allFilms = _employeeContext.Filme.Where(e => e.username.Equals(username)).ToList();
            _employeeContext.Filme.RemoveRange(_employeeContext.Filme.Where(id => id.imdbID == imdbID));
            _employeeContext.SaveChanges();
        }

        /// <summary>
        /// Get a movies from the DB.
        /// </summary>
        /// <param name="id">internal movieID (DB)<see cref="long"/></param>
        /// <returns>The <see cref="Filme"/></returns>
        public Filme Get(long id)
        {
            if (_employeeContext != null && _employeeContext.Filme.Any())
            {
                return _employeeContext.Filme.FirstOrDefault(e => e.MovieID == id);
            }
            return null;
        }

        /// <summary>
        /// The GetUser
        /// </summary>ad
        /// <param name="username">The username<see cref="string"/></param>
        /// <returns>The <see cref="Login"/></returns>
        public Login GetUser(string username)
        {
            if (_employeeContext != null && _employeeContext.Login.Any())
            {
                return _employeeContext.Login.FirstOrDefault(b => b.username == username);
            }
            return null;
        }

        public void Register(Login entity)
        {
            _employeeContext.Login.Add(entity);
            _employeeContext.SaveChanges();
        }
        public void Remove(Filme entity)
        {
            _employeeContext.Filme.Remove(entity);
            _employeeContext.SaveChanges();
        }

    }
}
