using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores
{
    public enum EnumTab
    {
        EOF,

        //Operadores
        OP_ASS,
        OP_EQ,
        OP_GT,
        OP_GE,
        OP_LT,
        OP_LE,
        OP_NE,
        OP_AD,
        OP_MIN,
        OP_MUL,
        OP_DIV,

        //Símbolos
        SMB_OBC,
        SMB_CBC,
        SMB_OPA,
        SMB_CPA,
        SMB_COM,
        SMB_SEM,

        //Palavras reservadas
        KW,

        //Identificador
        ID,

        //Constante: Númerica
        NUM_CONST,

        //Constante de Caracter
        CHAR_CONST,
        COMENTARIO,

        STRING
    }
}
