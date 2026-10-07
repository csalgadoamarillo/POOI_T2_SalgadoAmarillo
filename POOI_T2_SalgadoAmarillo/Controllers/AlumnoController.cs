using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using POOI_T2_SalgadoAmarillo.Models;

namespace POOI_T2_SalgadoAmarillo.Controllers
{
    public class AlumnoController : Controller
    {
        private List<Alumno> alumnos = new List<Alumno>();
        private string json = "[]";

        private void LeerAlumnos()
        {
            string ruta = Server.MapPath("~/App_Data/alumnos.json");
            if (System.IO.File.Exists(ruta))
                json = System.IO.File.ReadAllText(ruta);

            alumnos = JsonConvert.DeserializeObject<List<Alumno>>(json);
        }

        private void GuardarAlumnos()
        {
            json = JsonConvert.SerializeObject(alumnos, Formatting.Indented);
            string ruta = Server.MapPath("~/App_Data/alumnos.json");
            Directory.CreateDirectory(Server.MapPath("~/App_Data"));
            System.IO.File.WriteAllText(ruta, json);
        }

        public ActionResult Index()
        {
            LeerAlumnos();
            return View(alumnos);
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Alumno alumno)
        {
            if (!ModelState.IsValid)
                return View(alumno);

            LeerAlumnos();
            if (alumnos.Any(a => a.dni == alumno.dni))
            {
                ModelState.AddModelError("dni", "El DNI ya está registrado.");
                return View(alumno);
            }

            Alumno nuevo = new Alumno(alumno.dni, alumno.nombres, alumno.apellidos,
                alumno.carrera, alumno.ciclo);
            alumnos.Add(nuevo);
            GuardarAlumnos();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(string dni)
        {
            LeerAlumnos();
            Alumno alumno = alumnos.FirstOrDefault(a => a.dni == dni);
            if (alumno == null)
                return HttpNotFound();

            alumnos.Remove(alumno);
            GuardarAlumnos();
            return RedirectToAction("Index");
        }

        public ActionResult Detalles(string dni)
        {
            LeerAlumnos();
            Alumno alumno = alumnos.FirstOrDefault(a => a.dni == dni);
            if (alumno == null)
                return HttpNotFound();

            return View(alumno);
        }

        public ActionResult Actualizar(string dni)
        {
            LeerAlumnos();
            Alumno alumno = alumnos.FirstOrDefault(a => a.dni == dni);
            if (alumno == null)
                return HttpNotFound();

            ViewBag.DniOriginal = dni;
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(Alumno alumno, string dniOriginal)
        {
            ViewBag.DniOriginal = dniOriginal;
            if (!ModelState.IsValid)
                return View(alumno);

            LeerAlumnos();
            int posicion = alumnos.FindIndex(a => a.dni == dniOriginal);
            if (posicion == -1)
                return HttpNotFound();

            if (alumnos.Any(a => a.dni == alumno.dni && a.dni != dniOriginal))
            {
                ModelState.AddModelError("dni", "El DNI ya está registrado.");
                return View(alumno);
            }

            alumnos[posicion] = new Alumno(alumno.dni, alumno.nombres, alumno.apellidos,
                alumno.carrera, alumno.ciclo);
            GuardarAlumnos();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Serializar()
        {
            LeerAlumnos();
            GuardarAlumnos();
            TempData["Mensaje"] = "La colección se guardó en el archivo JSON.";
            return RedirectToAction("Index");
        }
    }
}
