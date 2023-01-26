using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.eTALENT
{
    public class EPDO_MASTER_DATA
    {
        public int EPDO_CODIGO { get; set; }
        public string P_APELLIDO { get; set; }
        public string S_APELLIDO { get; set; }
        public string P_NOMBRE { get; set; }
        public string S_NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string NACIONALIDAD { get; set; }
        public string FECHA_NACIMIENTO { get; set; }
        public string EPDO_SEXO { get; set; }
        public string EPDO_ESTADO { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string FECHA_INGRESO { get; set; }
        public decimal SALARIO { get; set; }
        public decimal BONIFICACION { get; set; }
        public string CUENTA { get; set; }
        public string FORMA_PAGO { get; set; }
        public string TIPO_CIUENTA { get; set; }
        public string CONTRATO_INICIO { get; set; }
        public string CONTRATO_FIN { get; set; }
        public int EDAD { get; set; }
        public string FECHA_INGRESO_COPR { get; set; }
        public decimal BONO_NO_AFECTO { get; set; }
        public string MAIL { get; set; }
        public string MAIL_INTERNO { get; set; }
        public string CELULAR { get; set; }
        public string EPDO_MOTIVO_RETIRO { get; set; }
        public string CAUSA_RETIRO { get; set; }
        public string EPDO_PUESTO_RETIRO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string EPDO_CASADA { get; set; }
        public decimal NUMERO_HIJOS { get; set; }
        public string PROFESION { get; set; }
        public string CODIGO_EXTERNO { get; set; }
        public string NUMERO_EMERGENCIA { get; set; }
        public string UNIDAD { get; set; }
        public string JEFE_INMEDIATO { get; set; }
        public string PLAZA { get; set; }

    }
}