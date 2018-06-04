namespace TrabalhoPratico_entrega2
{
    public enum EnumTab
    {
        #region Fim de Arquivo
        EOF,
        #endregion

        #region Operadores
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
        #endregion

        #region Simbolos
        SMB_OBC,
        SMB_CBC,
        SMB_OPA,
        SMB_CPA,
        SMB_COM,
        SMB_SEM,
        #endregion

        #region Palavras Reservadas
        KW_PROGRAM,
        KW_IF,
        KW_ELSE,
        KW_WHILE,
        KW_WRITE,
        KW_READ,
        KW_NUM,
        KW_CHAR,
        KW_NOT,
        KW_OR,
        KW_AND,
        #endregion

        #region Identificadores
        ID,
        #endregion

        #region Constantes
        NUM_CONST,
        #endregion

        #region Constantes
        CHAR_CONST,
        COMENTARIO,
        #endregion

        #region String
        STRING
        #endregion
    }
}
