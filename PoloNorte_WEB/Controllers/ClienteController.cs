using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PoloNorte_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoloNorte_WEB.Controllers
{
    public class ClienteController : Controller
    {
        //variable que contiene la memoria cache del navegador
        private readonly IMemoryCache _cache;
        //constructor de la clase
        public ClienteController(IMemoryCache cache)
        {
            _cache = cache;
        }

        //metodo para llenar rapidamente la lista de clientes
        public void LlenarListaClientes(List<Models.Cliente> lista)
        { 
            Cliente a = new Cliente();
            Cliente b = new Cliente();
            Cliente c = new Cliente();
            Cliente d = new Cliente();

            //deberia ser prospecto
            a.Cedula = "0100000000"; //fisico
            a.Nombre ="AAAAAAAAAA";
            a.Telefono = "88888888";
            a.Descuento = true;
            a.ComprasRecientes = 1000001;
            a.ComprasUltimoAño = 1;
            a.ComprasAC = 2;

            //deberia ser prospecto
            b.Cedula = "2000000000"; //juridico
            b.Nombre = "BBBBBBBBBB";
            b.Telefono = "88888888";
            b.Descuento = true;
            b.ComprasRecientes = 1000002;
            b.ComprasUltimoAño = 2;
            b.ComprasAC = 3;

            //no deberia ser prospecto
            c.Cedula = "010000000";
            c.Nombre = "CCCCCCCCCC";
            c.Telefono = "88888888";
            c.Descuento = true;
            c.ComprasRecientes = 1000005;
            c.ComprasUltimoAño = 3;
            c.ComprasAC = 1; //sin suficientes compras AC

            //no deberia ser prospecto
            d.Cedula = "010000000";
            d.Nombre = "DDDDDDDDDD";
            d.Telefono = "88888888";
            d.Descuento = true;
            d.ComprasRecientes = 999999; //sin suficientes compras recientes
            d.ComprasUltimoAño = 4;
            d.ComprasAC = 3;

            lista.Add(a);
            lista.Add(b);
            lista.Add(c);
            lista.Add(d);
        }

        //metodo que obtiene la lista de clientes de la cache
        public List<Models.Cliente> ObtenerListaClientes()
        {
            List<Models.Cliente> ListaClientes;

            //si no hay memoria cache
            if (_cache.Get("ListaClientes") is null)
            {//se crea.
                ListaClientes = new List<Models.Cliente>();
                //LlenarListaClientes(ListaClientes); // <---------------- METODO QUE LLENA LA LISTA AUTOMATICAMENTE
                _cache.Set("ListaClientes", ListaClientes);
            }
            else //de otro modo...
            {//se obtiene de la cache.
                ListaClientes = (List<Models.Cliente>)_cache.Get("ListaClientes");
            }

            return ListaClientes;
        }

        //metodo que la filtra la lista de clientes en prospectos
        public List<Models.Prospecto> ObtenerListaProspectos()
        {
            List<Models.Prospecto> ListaProspectos = new List<Models.Prospecto>();

            List<Models.Cliente> ListaClientes;
            ListaClientes = ObtenerListaClientes(); //obtenemos la lista de clientes

            //para cada cliente en en la lista
            foreach(Models.Cliente cliente in ListaClientes)
            {
                //si el cliente como 2 o mas aires acondicionados Y tiene mas de 1.000.000 en compras recientes
                if(cliente.ComprasAC >= 2 && cliente.ComprasRecientes > 1000000)
                {
                    //se crea un nuevo prospecto y se copian los datos del cliente
                    Prospecto nuevo_prospecto = new Prospecto();
                    nuevo_prospecto.Cedula = cliente.Cedula;
                    nuevo_prospecto.Nombre = cliente.Nombre;

                    //para definir el tipo de cedula
                    //si la cedula empieza con el numero 0
                    if(cliente.Cedula.StartsWith("0"))
                    {//se define el tipo de cedula como fisico
                        nuevo_prospecto.TipoCedula = "Fisico";
                    }
                    else//de otro modo...
                    {//la cedula es juridica.
                        nuevo_prospecto.TipoCedula = "Juridico";
                    }

                    ListaProspectos.Add(nuevo_prospecto);
                }
            }

            return ListaProspectos;
        }

        //metodo que guarda un cliente dentro de la lista de la cache
        public bool GuardarNuevoCliente(Models.Cliente nuevo_cliente)
        {
            List<Models.Cliente> ListaClientes;
            ListaClientes = ObtenerListaClientes(); //obtenemos la lista

            //verificamos si el cliente no existe en la lista
            if (ElClienteExiste(nuevo_cliente) == false)
            {
                //si no existe, se añade y se retorna verdadero
                ListaClientes.Add(nuevo_cliente);
                return true;
            }
            else//de otro modo...
            {
                //no se añade y se retorna falso.
                return false;
            }
        }

        // GET: ClienteController
        //este metodo se activa al cargar el index, que es la lista de prospectos
        public ActionResult Index()
        {
            List<Models.Prospecto> ListaProspectos;
            ListaProspectos = ObtenerListaProspectos(); //se obtiene la lista de prospectos

            //se carga la pagina con la lista obtenida
            return View(ListaProspectos);
        }

        //este metodo se activa al cargar la pagina de Editar Clientes
        public ActionResult EditIndex(string busqueda)
        {
            List<Models.Cliente> ListCliente = new List<Models.Cliente>();

            //si a busqueda no es nula...
            if (busqueda != null)
            {
                //se busca la cedula en la lista de clientes
                Cliente clienteobtenido = BuscarCedula(busqueda);
                if(clienteobtenido != null) //si se obtuvo un cliente
                {
                    ListCliente.Add(clienteobtenido); //se añade a una lista
                    return View(ListCliente); //es retornada a la vista
                }
                else //de otro modo...
                {
                    //si no se obtuvo un cliente se retorna un mensaje de error y una lista vacia
                    ViewBag.Error = "Ese cliente no existe.";
                    return View(ListCliente);
                }
            }
            else //si la busqueda era nula era la carga de la pagina
            {
                //simplemente se retorna la vista con una lista vacia
                return View(ListCliente);
            }
        }

        // GET: ClienteController/Create
        //metodo que se ejecuta al cargar la pagina para crear un nuevo cliente
        public ActionResult Create()
        {
            //el controlador simplemente retorna una vista sin ningun dato extra.
            return View();
        }

        // POST: ClienteController/Create
        //metodo que se ejecuta al subir un formulario desde la pagina de creacion de cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Cliente nuevo_cliente)
        {
            //si el metodo de guardar nuevo cliente tuvo exito
            if (GuardarNuevoCliente(nuevo_cliente) == true)
            {
                //se redirige el usuario al indice de prospectos
                return RedirectToAction(nameof(Index));
            }
            else//de otro modo
            {
                //se retorna al usuario un error
                ViewBag.Error = "Lo sentimos, ese cliente ya existe.";
                return View();
            }
        }

        // GET: ClienteController/Edit/5
        //metodo que se activa al cargar la pagina para editar un cliente
        public ActionResult Edit(Models.Cliente cliente)
        {
            //se retorna la vista con la informacion del cliente que se a solicitado
            return View(cliente);
        }

        //este metodo busca una cedula y retorna un cliente
        public Models.Cliente BuscarCedula(string cedula)
        {
            List<Models.Cliente> ListaClientes;
            ListaClientes = ObtenerListaClientes(); //se obtiene la lista de clientes

            //para cada cliente en la lista
            foreach (Models.Cliente cliente in ListaClientes)
            {
                //si la cedula del cliente que estamos iterando coincide con la buscada
                if (cliente.Cedula == cedula)
                {
                    //se retorna ese cliente
                    return cliente;
                }
            }

            //si se llego al final de la lista y no se encontro ningun cliente
            //se retorna nulo
            return null;
        }


        // POST: ClienteController/Edit/5
        //metodo que se activa al intentar subir los datos modificados de un cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(Models.Cliente cliente)
        {
            //se envian los datos recibidos por parametro para guardarlos
            EditarElCliente(cliente);
            //se regresa el usuario a la lista de prospectos
            return RedirectToAction(nameof(Index));
        }

        //este metodo se encarga de editar los clientes
        public void EditarElCliente(Models.Cliente clienteeditado)
        {
            List<Models.Cliente> ListaClientes;
            ListaClientes = ObtenerListaClientes(); //se obtiene la lista de clientes

            //para cada cliente que se encuentre en la lista
            foreach (Models.Cliente cliente in ListaClientes)
            {
                //si la cedula coincide con la que fue recibida por parametro
                if (cliente.Cedula == clienteeditado.Cedula)
                {
                    //entonces se cambian los valores originales por los recibidos por parametro
                    cliente.Nombre = clienteeditado.Nombre;
                    cliente.Telefono = clienteeditado.Telefono;
                    cliente.Descuento = clienteeditado.Descuento;
                }
            }
        }

        //este metodo nos ayuda a determinar si el cliente existe
        public bool ElClienteExiste(Models.Cliente clientenuevo)
        {
            List<Models.Cliente> ListaClientes;
            ListaClientes = ObtenerListaClientes(); //se obtiene la lista de clientes

            //para cada cliente en la lista
            foreach (Models.Cliente cliente in ListaClientes)
            {
                //si la cedula recibida por parametro coincide con la del cliente en la lsita
                if (cliente.Cedula == clientenuevo.Cedula)
                {
                    //entonces ese cliente existe, se retorna verdadero.
                    return true;
                }
            }

            //si se llega al final de la lista sin ninguna concidencia
            //entonces el cliente no existe
            //se retorna falso
            return false;
        }

        //metodo que se ejecuta al intentar cargar la pagina "about"
        public ActionResult About()
        {
            //simplemente se retorna la vista
            return View();
        }

    }
}
