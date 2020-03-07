using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.ViewModels
{
    public class HomeDetailsViewModel
    {
        public Filme Filme { get; set; }
        public Login Login { get; set; }
        public List<Filme> FilmeList { get; set; }

    }
}
