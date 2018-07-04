using System.Collections.Generic;

namespace Compilador
{
    public class TabelaSimbolos
    {
        private int linha;
        private int coluna;
        Dictionary<Token, InfIdentificador> tabelaSimbolos;

        public TabelaSimbolos()
        {
            tabelaSimbolos = new Dictionary<Token, InfIdentificador>();

            #region Palavras Reservadas

            Token palavra;
            palavra = new Token(EnumTab.KW_PROGRAM, "program", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_IF, "if", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_ELSE, "else", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_WHILE, "while", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_WRITE, "write", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_READ, "read", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_NUM, "num", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_CHAR, "char", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_NOT, "not", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_OR, "or", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_AND, "and", linha, coluna);
            tabelaSimbolos[palavra] = new InfIdentificador();
            
        }
        #endregion

        //Insere o identificador
        public void insereIdentificador(Token palavra, InfIdentificador identificador)
        {
            tabelaSimbolos.Add(palavra, identificador);
        }

        //Retorna um identificfador de um determinado token
        public InfIdentificador obtemIdentificador(Token palavra)
        {
            InfIdentificador infoIdentificador = tabelaSimbolos[palavra];
            return infoIdentificador;
        }

        //Pesquisa a existencia do lexema
        public Token retornaToken(string lexema)
        {
            foreach (Token token in tabelaSimbolos.Keys)
            {
                if (token.lexema.Equals(lexema))
                {
                    return token;
                }
            }
            return null;
        }

        public override string ToString()
        {
            
            string saida = " ";

            foreach (Token token in tabelaSimbolos.Keys)
            {

                saida += ("\t " + token.ToString()) + "\n\t\t Linha: " + linha + " Coluna: " + coluna + "\n";
            }

            return saida;
        }
    }
}
