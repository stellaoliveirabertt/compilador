using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoPratico01
{
    public class Token
    {
        private string lexema;
        private EnumTab classe;
        public int nLinha;
        public int nColuna;       

        //Construtor
        public Token(EnumTab classe, String lexema, int linha, int Coluna)
        {
            this.classe = classe;
            this.lexema = lexema;
            this.nLinha = linha;
            this.nColuna = Coluna;
        }

        public String getLexema()
        {

            return lexema;
        }

        public void setLexema(String lexema)
        {

            this.lexema = lexema;
        }
        public EnumTab getClasse()
        {

            return classe;
        }

        public void setClasse(EnumTab classe)
        {

            this.classe = classe;
        }

        public int getLinha()
        {
            return nLinha;
        }

        public void setLinha(int linha)
        {
            this.nLinha = linha;
        }

        public int getColuna()
        {
            return nColuna;
        }

        public void setColuna(int coluna)
        {
            this.nColuna = coluna;
        }

        public override string ToString()
        {
            return "<" + classe + " , \"" + lexema + " \">\nLinha: " + getLinha() + " \tColuna: " + getColuna();
        }
    }
}
