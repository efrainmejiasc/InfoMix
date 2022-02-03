using System.ComponentModel;

namespace Informix.Domain
{
    public enum EnumTipoIngreso
    {
        [Description("Solicitud 1, sin paciente (Laboratorios Ambiental)")]
        Ambiental = 1,

        [Description("Solicitud 2, con paciente (Laboratorios de Salud)")]
        ConPaciente = 2,

        [Description("Solicitud 3, sin paciente")]
        SinPaciente = 3,

        [Description("Solicitud 4, SOCA")]
        Soca = 4,

    }
    public enum EnumMsj
    {
        #region Mensajes Informix
        [Description("Formulario ya registrado en informix")]
        Registrado,

        [Description("Falta definir procedencia, para rut")]
        FaltaProcedencia,
        #endregion
    }
    public enum EnumNivel
    {

        [Description("Elimina solicitud, paciente, prestacion")]
        SolicitudCompleta = 0,

        [Description("Elimina paciente y prestacion")]
        PacienteYPrestac = 1,
    }
}