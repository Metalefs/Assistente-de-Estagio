﻿using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Documento
    {
        public int IdDocumento { get; set; }
        public int CursoIdCurso { get; set; }
        public string TituloDocumento { get; set; }
        public string DescricaoDocumento { get; set; }
        public string RequisitoDocumento { get; set; }
        public string PreenchimentoDocumento { get; set; }
        public int PosicaoDocumento { get; set; }
        public string AutorDocumento { get; set; }
        public string CaminhoDocumento { get; set; }
        public string TiposRequisitos { get; set; }
    }
}
