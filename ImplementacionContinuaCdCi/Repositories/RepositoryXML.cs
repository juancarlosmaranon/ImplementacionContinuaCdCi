using MvcCoreLinqToXML.Helpers;
using MvcCoreLinqToXML.Models;
using System.Xml.Linq;

namespace MvcCoreLinqToXML.Repositories
{
    public class RepositoryXML
    {
        private HelperPathProvider helper;
        private XDocument documentClientes;
        private string pathClientes;

        public RepositoryXML(HelperPathProvider helper)
        {
            this.helper = helper;
            this.pathClientes = this.helper.MapPath("ClientesID.xml"
                , Folders.Documents);

            this.documentClientes = XDocument.Load(this.pathClientes);
        }

        public List<Cliente> GetClientes()
        {
            

            List<Cliente> clientes = new List<Cliente>();
            var consulta = from datos in documentClientes.Descendants("CLIENTE")
                           select datos;

            foreach (XElement tag in consulta)
            {
                Cliente cliente = new Cliente();
                //PARA ACCEDER AL VALOR DE UN TAG SE UTILIZA Element
                //PARA ACCEDER AL VALOR DE UN ATRIBUTO SE UTILIZA Attribute
                cliente.IdCliente = int.Parse(tag.Element("IDCLIENTE").Value);
                cliente.Nombre = tag.Element("NOMBRE").Value;
                cliente.Direccion = tag.Element("DIRECCION").Value;
                cliente.Email = tag.Element("EMAIL").Value;
                cliente.ImagenCliente = tag.Element("IMAGENCLIENTE").Value;
                clientes.Add(cliente);
            }
            return clientes;
        }

        public Cliente FindCliente(int idcliente)
        {
            var consulta = from datos in 
                               this.documentClientes.Descendants("CLIENTE")
                           where datos.Element("IDCLIENTE").Value ==
                           idcliente.ToString()
                           select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                XElement tag = consulta.FirstOrDefault();
                Cliente cliente = new Cliente();
                cliente.IdCliente = int.Parse(tag.Element("IDCLIENTE").Value);
                cliente.Nombre = tag.Element("NOMBRE").Value;
                cliente.Direccion = tag.Element("DIRECCION").Value;
                cliente.Email = tag.Element("EMAIL").Value;
                cliente.ImagenCliente = tag.Element("IMAGENCLIENTE").Value;
                return cliente;
            }
        }

        public XElement FindXMLCliente(string id)
        {
            var consulta = from datos in
                               this.documentClientes.Descendants("CLIENTE")
                           where datos.Element("IDCLIENTE").Value == id
                           select datos;
            return consulta.FirstOrDefault();
        }

   
        
        public void CreateCliente(int idcliente, string nombre, string email, string direccion, string imagen)
        {
            //CREAMOS EL OBJETO VACIO XElement
            XElement rootCliente = new XElement("CLIENTE");
            rootCliente.Add(new XElement("IDCLIENTE", idcliente));
            rootCliente.Add(new XElement("NOMBRE", nombre));
            rootCliente.Add(new XElement("DIRECCION", direccion));
            rootCliente.Add(new XElement("EMAIL", email));
            rootCliente.Add(new XElement("IMAGENCLIENTE", imagen));
            //INSERTAMOS EN EL DOCUMENT EN EL TAG QUE CORRESPONDA
            //EN ESTE EJEMPLO, SOBRE LA RAIZ CLIENTES
            this.documentClientes.Element("CLIENTES").Add(rootCliente);
            this.documentClientes.Save(this.pathClientes);
        }

        public void UpdateCliente (int idcliente, string nombre, string email, string direccion, string imagen)
        {
            XElement clienteXML = this.FindXMLCliente(idcliente.ToString());
            clienteXML.Element("NOMBRE").Value = nombre;
            clienteXML.Element("DIRECCION").Value = direccion;
            clienteXML.Element("EMAIL").Value = email;
            clienteXML.Element("IMAGENCLIENTE").Value = imagen;
            this.documentClientes.Save(this.pathClientes);
        }

        public void DeleteCliente(int idcliente)
        {
            //NECESITAMOS BUSCAR EL XML DEL CLIENTE A ELIMINAR
            XElement clienteXML = this.FindXMLCliente(idcliente.ToString());
            //EL PROPIO OBJETO XElment CONTIENE UN METODO PARA ELIMINARSE
            clienteXML.Remove();
            //EL OBJETO XDocument TIENE UN METODO SAVE PARA GUARDAR
            //EL DOCUMENTO EN EL SISTEMA DE FILES
            //NECESITAMOS LA RUTA AL DOCUMENTO
            this.documentClientes.Save(this.pathClientes);
        }

        public List<Joyeria> GetJoyerias()
        {
            string path = helper.MapPath("joyerias.xml"
                , Folders.Documents);
            //AQUI TENEMOS DOS METODOS PARA CARGAR DATOS EN UN
            //OBJETO XDocument
            //1) Parse(string XML)
            //2) Load(path XML)
            XDocument document = XDocument.Load(path);
            //DEBEMOS EXTRAER LOS DATOS MANUALMENTE
            List<Joyeria> joyerias = new List<Joyeria>();
            var consulta = from datos in document.Descendants("joyeria")
                           select datos;
            //RECORREMOS TODOS LOS OBJETOS XELEMENT
            foreach (XElement tag in consulta)
            {
                Joyeria joyeria = new Joyeria();
                //PARA ACCEDER AL VALOR DE UN TAG SE UTILIZA Element
                //PARA ACCEDER AL VALOR DE UN ATRIBUTO SE UTILIZA Attribute
                joyeria.Nombre = tag.Element("nombrejoyeria").Value;
                joyeria.CIF = tag.Attribute("cif").Value;
                joyeria.Telefono = tag.Element("telf").Value;
                joyeria.Direccion = tag.Element("direccion").Value;
                joyerias.Add(joyeria);
            }
            return joyerias;
        }
    }
}
