using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Dominio.Models.Enums
{
    public enum TipoEvento
    {
        Criacao,
        Alteracao,
        Delecao,
        Download,
        Upload,
        BookmarkEncontrado = 1,
        ComandoFalhou = 2,
        BookmarkRepetido = 3,
        BookmarkNaoEncontrado = 4
    }
}
