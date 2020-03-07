namespace Chat.Controllers
{
    using Chat.Models;
    using Chat.Models.Repository;
    using Chat.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Nancy.Json;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="HomeController" />
    /// </summary>
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>
        /// Defines the _dataRepository
        /// </summary>
        private readonly IDataRepository<Filme> _datarepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="dataRepository">The dataRepository<see cref="IDataRepository{Filme}"/></param>
        public HomeController(IDataRepository<Filme> dataRepository)
        {
            _datarepository = dataRepository;
        }

        /// <summary>
        /// Index Page
        /// </summary>
        /// <param name="filter">Filte z.b Action<see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Route("/")]
        // GET: Home
        public ActionResult Index(string filter)
        {
            ViewData["filter"] = (filter == null) ? "" : filter;
            ViewData["loginstatus"] = 0;
            ViewData["error"] = 0;

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                FilmeList = _datarepository.GetAll(),
                Login = SetLoginData()
            };
            return View(homeDetailsViewModel);
        }

        /// <summary>
        /// Listet of all movis from the DB
        /// </summary>
        /// <returns>The <see cref="ViewResult"/></returns>

        /// <summary>
        /// Login Page (html Formular)
        /// </summary>
        /// <returns>The <see cref="ViewResult"/></returns>
        /// <returns>The <see cref="ViewResult"/></returns>
        [Route("/loginform")]
        public ViewResult loginform()
        {
            return View();
        }

        [Route("/registerform")]
        public ViewResult registerform()
        {
            return View();
        }




        /// <summary>
        /// Login Processing
        /// </summary>
        /// <param name="password">User Password<see cref="string"/></param>
        /// <param name="username">User Username<see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Route("/login")]
        [HttpPost]
        public ActionResult index(string password, string username)
        {
            if (username == null)
            {
                return View();
            }
            var login = new Login();
            var filme = _datarepository.GetAll();
            Login logindata = _datarepository.GetUser(username);
            try
            {
                if ((string)logindata.password == password)
                {
                    HttpContext.Session.SetInt32("loginstatus", 1);
                    HttpContext.Session.SetString("username", (string)logindata.username);
                    ViewData["loginstatus"] = 1;
                }
            }
            catch
            {
                ViewData["loginstatus"] = 0;
                return View();
            }

            ViewData["error"] = 0;
            ViewData["filter"] = "";
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                FilmeList = filme,
                Login = SetLoginData()
            };

            return View(homeDetailsViewModel);
        }

        [Route("/register")]
        [HttpPost]
        public ActionResult loginform(string email, string username, string password)

        {
            Login newLogin = new Login();
            newLogin.email = email;
            newLogin.username = username;
            newLogin.password = password;

            _datarepository.Register(newLogin);

            ViewData["loginstatus"] = 0;

            return View();

        }

        /// <summary>
        /// Search Details Page (from the imdb Api)
        /// </summary>
        /// <param name="input">Search input<see cref="string"/></param>
        /// <returns>The <see cref="Task{ViewResult}"/></returns>
        [Route("search/details/{imdbID}")]

        public async Task<ViewResult> details(string imdbID)
        {
            string username = SetLoginData().username;

            var apinreturn = await SearchSingleItem(imdbID);

            foreach (var select in _datarepository.GetFavorits(username))
            {
                if (select.imdbID == imdbID)
                {
                    ViewData["ID"] = "1";
                }
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Filme = apinreturn,
                Login = SetLoginData(),
                FilmeList = { }
            };

            return View(homeDetailsViewModel);
        }

        [Route("/search")]

        public async Task<ViewResult> searchlist(string input, int page, string type, int amount)
        {
            //Set Param for the search and if they are default, set them to null because otherwise we cant search
            page = (page == 0) ? page = 1 : page;
            type = (type == null) ? type = "default" : type;
            amount = (amount == 0) ? amount = 20 : amount;

            //The raw string JSON gets saved here
            string json = "";

            //The full transformed JSON gets saved here
            JObject data;

            //Sets the amount how often the API gets requested
            int loop = 0;

            switch (amount)
            {
                case 20:
                    loop = 3;
                    break;
                case 50:
                    loop = 5;
                    break;
                case 100:
                    loop = 10;
                    break;
                case 200:
                    loop = 20;
                    break;
                default:
                    loop = 3;
                    break;
            }

            //Client which requests the API
            using (var httpClient = new HttpClient())
            {
                //URL to request the API
                json = await httpClient.GetStringAsync("http://www.omdbapi.com/?s=" + input + "&apikey=9f672c2d&page=" + page);
                page++;

                //Gets the first totalResult Token
                //Important because sometimes the totalResult number is not fix and changes while the request
                int[] resultIndex = new int[19];
                int counter = 0;

                //Error when there is no film found
                try
                {
                    resultIndex[counter] = (int)((JObject)JsonConvert.DeserializeObject(json))["totalResults"];
                }
                catch (Exception)
                {
                    ViewData["error"] = 1;
                    HomeDetailsViewModel error = new HomeDetailsViewModel()
                    {
                        Login = SetLoginData()
                    };
                    return View("index", error);
                }

                //Gets Type
                string typeFilter = (string)((JObject)JsonConvert.DeserializeObject(json))["Type"];
                ViewData["Type"] = type;


                //String to requests the API
                string getAPI = "";

                for (int pageLoop = page; pageLoop <= loop; pageLoop++)
                {

                    getAPI = await httpClient.GetStringAsync("http://www.omdbapi.com/?s=" + input + "&apikey=9f672c2d&page=" + pageLoop);

                    //Gets highest Result
                    ViewData["NumberofResults"] = resultIndex.Max();

                    try
                    {
                        //If result changes during the request
                        if (resultIndex[counter] != (int)((JObject)JsonConvert.DeserializeObject(getAPI))["totalResults"])
                        {
                            //Gets a new Array 
                            counter++;
                            resultIndex[counter] = (int)((JObject)JsonConvert.DeserializeObject(getAPI))["totalResults"];
                        }
                    }
                    catch (Exception)
                    {
                        loop = 0;
                        continue;
                    }

                    json += getAPI;
                }
                ViewData["Input"] = input;

                //Gets the single JSON strings together
                json = JsonReplaceJunk(json, resultIndex);

                //Converts the json string to JObject
                data = (JObject)JsonConvert.DeserializeObject(json);
                var filmeoutput = new List<Filme>();
                foreach (JObject film in data["Search"])
                {

                    if ((type == "default" && (string)film["Poster"] != "N/A") || ((string)film["Type"] == type && (string)film["Poster"] != "N/A"))
                    {
                        filmeoutput.Add(new JavaScriptSerializer().Deserialize<Filme>(film.ToString(Formatting.None)));
                    }
                }

                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    FilmeList = filmeoutput,
                    Login = SetLoginData()
                };


                return View(homeDetailsViewModel);

            }
        }



        /// <summary>
        /// The index
        /// </summary>
        /// <returns>The <see cref="ViewResult"/></returns>
        [Route("/logout")]

        public ActionResult index()
        {
            HttpContext.Session.SetInt32("loginstatus", 0);
            HttpContext.Session.SetString("username", "");
            HttpContext.Session.SetString("email", "");

            ViewData["loginstatus"] = 0;
            ViewData["error"] = 0;
            ViewData["filter"] = "";
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                FilmeList = _datarepository.GetAll(),
                Login = SetLoginData()
            };
            return View(homeDetailsViewModel);
        }



        [Route("/favorit")]

        public async Task<ActionResult> favorit(int status, string imdbID)
        {
            var username = SetLoginData().username;
            if (status == -1)
            {
                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    FilmeList = _datarepository.GetFavorits(username),
                    Login = SetLoginData()
                };
                return View(homeDetailsViewModel);

            }
            if (status == 0)
            {
                _datarepository.Delete(imdbID, username);
                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    FilmeList = _datarepository.GetFavorits(username),
                    Login = SetLoginData()
                };
                return View(homeDetailsViewModel);

            }
            else if (status == 1)
            {
                var apireturn = await SearchSingleItem(imdbID);
                apireturn.username = username;
                _datarepository.Add(apireturn);
                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    FilmeList = _datarepository.GetFavorits(username),
                    Login = SetLoginData()
                };
                return View(homeDetailsViewModel);

            }
            else
            {
                return View("index");
            }
        }


        private async Task<Filme> SearchSingleItem(string imdbID)
        {
            using (var httpClient = new HttpClient())
            {
                string json = "";
                string url = "http://www.omdbapi.com/?i=" + imdbID + "&apikey=9f672c2d&plot=full";
                json = await httpClient.GetStringAsync(url);
                var apireturn = JsonConvert.DeserializeObject<Filme>(json);
                return (apireturn);
            }
        }

        /// <summary>
        /// Refactored Code
        /// </summary>
        /// <param name="login">The login<see cref="Login"/></param>
        private Login SetLoginData()
        {
            var login = new Login();

            if (HttpContext.Session.GetInt32("loginstatus") == null)
            {
                ViewData["loginstatus"] = 0;
            }
            else
            {
                ViewData["loginstatus"] = (int)HttpContext.Session.GetInt32("loginstatus");
                login.username = HttpContext.Session.GetString("username");
                login.email = HttpContext.Session.GetString("email");
                try
                {
                    login.id = (int)HttpContext.Session.GetInt32("ID");
                }
                catch
                {
                    login.id = 0;
                }
            }

            ViewData["search"] = 1;
            return login;
        }

        /// <summary>
        /// Refactored Code
        /// </summary>
        /// <param name="login">The login<see cref="Login"/></param>

        private string JsonReplaceJunk(string json, int[] result)
        {
            string search = json.Replace("{\"Search\":", "");
            string endClip = search.Replace("]", "");
            string totalResults = endClip;

            if (result[1] != 0)
            {
                foreach (int allResults in result)
                {
                    totalResults = endClip.Replace("\"totalResults\":\"" + allResults + "\",\"Response\":\"True\"}[", "");
                }
            }
            else
            {
                totalResults = endClip.Replace("\"totalResults\":\"" + result[0] + "\",\"Response\":\"True\"}[", "");

            }

            var square = totalResults.Replace("}{", "},{");
            square += "end";
            json = square.Replace("},\"totalResults\":\"" + result.Max() + "\",\"Response\":\"True\"}end", "}],\"totalResults\":\"" + result.Max() + "\",\"Response\":\"True\"}");
            json = "{\"Search\":" + json;
            return json;
        }
    }
}