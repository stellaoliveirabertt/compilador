namespace Compilador
{
    public class Token
    {
        #region Variaveis

        public string lexema { get; set; }
        public EnumTab classe { get; set; }
        public int linha { get; set; }
        public int coluna { get; set; }
        
        #endregion

        #region Construtor
        public Token (EnumTab classe, string lexema, int linha, int coluna)
        {
            this.classe = classe;
            this.lexema = lexema;
            this.linha = linha;
            this.coluna = coluna;
        }
        
        #endregion

        public override string ToString()
        {
            return "< " + classe + " - " + lexema + " > \n\t\t     Linha: " + linha + " Coluna: " + coluna + "\n";
        }

    }
}
