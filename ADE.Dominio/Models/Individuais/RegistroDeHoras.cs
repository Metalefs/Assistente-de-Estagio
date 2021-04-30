using ADE.Dominio.Models.Individuais;
using System;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models
{
    public partial class RegistroDeHoras : ModeloBase
    {
        public override int Identificador { get; set; }
        [Required(ErrorMessage = "O Campo Data é obrigatório")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "O Campo Hora inicio é obrigatório")]
        public DateTime HoraInicio { get; set; }
        [Required(ErrorMessage = "O Campo Hora fim é obrigatório")]
        public DateTime HoraFim { get; set; }
        [Required(ErrorMessage = "O Campo Atividade é obrigatório")]
        public string Atividade { get; set; }
        public float CargaHoraria { get; set; }
        public string IdUsuario { get; set; }
        public virtual UsuarioADE Usuario { get; set; }

        public bool Validar()
        {
            if(HoraFim.Hour == 0)
            {
                HoraFim = HoraFim.AddDays(1);
            }
            bool HoraInicio_Diferente_HoraFim = HoraInicio != HoraFim;
            bool HorasValidas = HoraInicio < HoraFim;
            //bool DataValida = Data <= DateTime.Now;
            double CargaHoraria = (HoraFim - HoraInicio).TotalMinutes;
            bool CargaHorariaMaiorQueUmMinuto = CargaHoraria >= 1 || this.CargaHoraria >=1;
            this.CargaHoraria = (float)CargaHoraria;
            if (!HoraInicio_Diferente_HoraFim || !CargaHorariaMaiorQueUmMinuto)
                throw new Exception("A Carga Horária deve ser maior do que 1 minuto");
            if(!HorasValidas)
                throw new Exception("A hora de inicio da atividade deve ser inferior a hora final");
            //if (!DataValida)
            //    throw new Exception("A data da atividade não pode ultrapassar a data atual");

            return true;
        }

        public void Clonar(RegistroDeHoras registro)
        {
            Data = registro.Data;
            HoraInicio = registro.HoraInicio;
            HoraFim = registro.HoraFim;
            Atividade = registro.Atividade;
            CargaHoraria = registro.CargaHoraria;
            IdUsuario = registro.IdUsuario;
        }

        public override string ToString()
        {
            return $"{Atividade}";
        }

    }
}
