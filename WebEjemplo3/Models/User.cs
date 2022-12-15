using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebEjemplo3.Models
{
    public class User
    {
        
        public int idUser { get; set; }
      
        public string nombre { get; set; }
       
        public string aPaterno { get; set; }
       
        public string aMaterno { get; set; }
       
        public string correo { get; set; }

      
        public string contraseña { get; set; }

        public string ConfirmarContraseña { get; set; }
       
    }
}