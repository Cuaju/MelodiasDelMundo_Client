using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasDelMundo_Client
{
    public class SesionEmpleado
    {
        private static SesionEmpleado _instancia;
        public static SesionEmpleado Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new SesionEmpleado();
                }
                return _instancia;
            }
        }

        public int IdEmpleado { get; set; }

        private SesionEmpleado() { }
    }

}
