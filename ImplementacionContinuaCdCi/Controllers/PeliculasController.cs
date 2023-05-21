using Microsoft.AspNetCore.Mvc;
using MvcCoreLinqToXML.Models;
using MvcCoreLinqToXML.Repositories;

namespace MvcCoreLinqToXML.Controllers
{
    public class PeliculasController : Controller
    {
        private RepositoryPeliculas repo;

        public PeliculasController(RepositoryPeliculas repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Pelicula> pelis = this.repo.GetPeliculas();
            return View(pelis);
        }

        public IActionResult EscenasPelicula(int idpelicula)
        {
            Pelicula peli = this.repo.FindPelicula(idpelicula);
            ViewData["PELICULA"] = peli;
            List<Escena> escenas = this.repo.GetEscenasPelicula(idpelicula);
            return View(escenas);
        }

        public IActionResult DetallePeliculaEscena(int idpelicula, int? posicion)
        {
            //LINQ FUNCIONA EN BASE 0
            //SQL SERVER FUNCIONA EN BASE 1
            if(posicion == null)
            {
                posicion = 0;
            }
            int numeroEscenas = 0;
            Escena escena = this.repo.GetEscenaPelicula
                (idpelicula, posicion.Value, ref numeroEscenas);
            ViewData["DATO"] = "Escena " + (posicion + 1)
                + " de " + numeroEscenas;
            int siguiente = posicion.Value + 1;
            if(siguiente >= numeroEscenas)
            {
                siguiente = 0;
            }
            int anterior = posicion.Value - 1;
            if(anterior < 0)
            {
                anterior = numeroEscenas - 1;
            }
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            Pelicula peli = this.repo.FindPelicula(idpelicula);
            ViewData["PELICULA"] = peli;
            return View(escena);
        }
    }
}
