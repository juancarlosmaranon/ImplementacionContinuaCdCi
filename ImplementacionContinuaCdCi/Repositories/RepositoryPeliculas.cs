using MvcCoreLinqToXML.Helpers;
using MvcCoreLinqToXML.Models;
using System.Xml.Linq;

namespace MvcCoreLinqToXML.Repositories
{
    public class RepositoryPeliculas
    {

        private HelperPathProvider helper;

        public RepositoryPeliculas(HelperPathProvider helper)
        {
            this.helper = helper;
        }

        public List<Pelicula> GetPeliculas()
        {
            string path = this.helper.MapPath("peliculas.xml", Folders.Documents);
            XDocument document = XDocument.Load(path);
            var consulta = from datos in document.Descendants("pelicula")
                           select datos;
            List<Pelicula> peliculasList = new List<Pelicula>();
            foreach (XElement tag in consulta)
            {
                Pelicula peli = new Pelicula();
                peli.IdPelicula = int.Parse(tag.Attribute("idpelicula").Value);
                peli.Titulo = tag.Element("titulo").Value;
                peli.TituloOriginal = tag.Element("titulooriginal").Value;
                peli.Descripcion = tag.Element("descripcion").Value;
                peli.Poster = tag.Element("poster").Value;
                peliculasList.Add(peli);
            }
            return peliculasList;
        }

        public Pelicula FindPelicula(int idpelicula)
        {
            string path = this.helper.MapPath("peliculas.xml", Folders.Documents);
            XDocument document = XDocument.Load(path);
            var consulta = from datos in document.Descendants("pelicula")
                           where datos.Attribute("idpelicula").Value == idpelicula.ToString()
                           select datos;
            XElement tag = consulta.FirstOrDefault();
            Pelicula peli = new Pelicula();
            peli.IdPelicula = int.Parse(tag.Attribute("idpelicula").Value);
            peli.Titulo = tag.Element("titulo").Value;
            peli.TituloOriginal = tag.Element("titulooriginal").Value;
            peli.Descripcion = tag.Element("descripcion").Value;
            peli.Poster = tag.Element("poster").Value;
            return peli;
        }

        public List<Escena> GetEscenasPelicula(int idpelicula)
        {
            string path = this.helper.MapPath
                ("escenaspeliculas.xml", Folders.Documents);
            XDocument document = XDocument.Load(path);
            var consulta = from datos in document.Descendants("escena")
                           where datos.Attribute("idpelicula").Value == idpelicula.ToString()
                           select datos;
            List<Escena> escenasList = new List<Escena>();
            foreach (XElement tag in consulta)
            {
                Escena escena = new Escena();
                escena.IdPelicula = int.Parse(tag.Attribute("idpelicula").Value);
                escena.Titulo = tag.Element("tituloescena").Value;
                escena.Descripcion = tag.Element("descripcion").Value;
                escena.Imagen = tag.Element("imagen").Value;
                escenasList.Add(escena);
            }
            return escenasList;
        }

        public Escena GetEscenaPelicula(int idpelicula, int posicion, ref int numeroEscenas)
        {
            string path =
                this.helper.MapPath("escenaspeliculas.xml", Folders.Documents);
            XDocument document = XDocument.Load(path);
            //VOY A RECUPERAR LA COLECCION DE ESCENAS DE
            //UNA PELICULA
            //PARA ELLO, UTILIZAMPS EL METODO ANTERIOR
            List<Escena> escenasList = this.GetEscenasPelicula(idpelicula);
            numeroEscenas = escenasList.Count;
            //VAMOS A PAGINAR DE UNO EN UNO 
            Escena escena =
                escenasList.Skip(posicion).Take(1).FirstOrDefault();
            return escena;
        }

    }
}
