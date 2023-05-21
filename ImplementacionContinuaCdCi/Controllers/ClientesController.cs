using Microsoft.AspNetCore.Mvc;
using MvcCoreLinqToXML.Models;
using MvcCoreLinqToXML.Repositories;

namespace MvcCoreLinqToXML.Controllers
{

    public class ClientesController : Controller
    {
        private RepositoryXML repo;

        public ClientesController(RepositoryXML repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Cliente> clientes = this.repo.GetClientes();
            return View(clientes);
        }

        public IActionResult Details(int idcliente)
        {
            Cliente cliente = this.repo.FindCliente(idcliente);
            return View(cliente);
        }

        public IActionResult Delete(int idcliente)
        {
            this.repo.DeleteCliente(idcliente);
            return RedirectToAction("Index");
        }

        public IActionResult Update(int idcliente)
        {
            Cliente cliente = this.repo.FindCliente(idcliente);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult Update(Cliente cliente)
        {
            this.repo.UpdateCliente(cliente.IdCliente
                , cliente.Nombre, cliente.Direccion, cliente.Email
                , cliente.ImagenCliente);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cliente cliente)
        {
            this.repo.CreateCliente(cliente.IdCliente
                , cliente.Nombre, cliente.Direccion
                , cliente.Email, cliente.ImagenCliente);
            return RedirectToAction("Index");
        }
    }
}
