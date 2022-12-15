using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebEjemplo3.Models;
namespace WebEjemplo3.Controllers
{
    public class HomeController : Controller
    {
        private dbPruebasEntities _db = new dbPruebasEntities();
        public ActionResult Index()
        {
            if (Session["idUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
           

        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
      
        public ActionResult Register(User _user)
        {
           
                var check = _db.Login.FirstOrDefault(s => s.correo == _user.correo);
            if (check == null)
            {
                var user = new Login()
                {
                    nombre = _user.nombre,
                    aPaterno = _user.aPaterno,
                    aMaterno = _user.aMaterno,
                    correo = _user.correo,
                    contraseña = GetMD5(_user.ConfirmarContraseña)
                };

                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.Login.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Email already exists";
                return View();
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult Login(string correo, string contraseña)
        {

                var f_password = GetMD5(contraseña);
                var data = _db.Login.Where(s => s.correo.Equals(correo) && s.contraseña.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().nombre + " " + data.FirstOrDefault().aPaterno;
                    Session["Email"] = data.FirstOrDefault().correo;
                    Session["idUser"] = data.FirstOrDefault().idUser;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            
            
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
      
        public ActionResult CRUD(FormCollection objetoForm)
        {
            if(objetoForm["idUsuario"] != null)
            {
                if(objetoForm["idUsuario"] != "")
                {
                    int iduser = Convert.ToInt32(objetoForm["idUsuario"].ToString());
                    if (objetoForm["btnAgregar"] != null)
                    {
                        if (objetoForm["nombre"] != null && objetoForm["aPaterno"] != null && objetoForm["aMaterno"] != null)
                        {
                            var user = new Login()
                            {
                                nombre = objetoForm["nombre"].ToString(),
                                aPaterno = objetoForm["aPaterno"].ToString(),
                                aMaterno = objetoForm["aMaterno"].ToString(),
                                correo = objetoForm["correo"].ToString(),

                            };
                            _db.Login.Add(user);
                            _db.SaveChanges();
                        }

                    }
                    else if (objetoForm["btnActualizar"] != null)
                    {
                        var user = _db.Login.SingleOrDefault(c => c.idUser == iduser);
                        if (user != null)
                        {
                            user.nombre = objetoForm["nombre"].ToString();
                            user.aPaterno = objetoForm["aPaterno"].ToString();
                            user.aMaterno = objetoForm["aMaterno"].ToString();
                            user.correo = objetoForm["correo"].ToString();
                            _db.SaveChanges();
                        }

                    }
                    else if (objetoForm["btnEliminar"] != null)
                    {

                        var user = _db.Login.SingleOrDefault(c => c.idUser == iduser);
                        if (user != null)
                        {
                            _db.Login.Remove(user);
                            _db.SaveChanges();
                        }

                    }
                }

            }
            var data = _db.Login.ToList();
            return View(data);


        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }



    }
}