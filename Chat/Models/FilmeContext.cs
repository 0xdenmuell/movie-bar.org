using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Chat.Models
{
    public class FilmeContext : DbContext
    {

        public FilmeContext(DbContextOptions options)
     : base(options)
        {
        }

        public DbSet<Filme> Filme { get; set; }
        public DbSet<Login> Login { get; set; }


    }
       }
