using System.Collections.Generic;

namespace TrabalhoPratico_entrega2
{
    public class TabelaSimbolos
    {
        private Dictionary<Token, InfIdentificador> tabelaSimbolos;

        public TabelaSimbolos(int linha, int coluna)
        {
            tabelaSimbolos = new Dictionary<Token, InfIdentificador>();

            #region Palavras reservadas
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
            #endregion
        }

        #region Insere o identificador
        public void insereIdentificador(Token palavra, InfIdentificador ident)
        {
            tabelaSimbolos.Add(palavra, ident);
        }
        #endregion

        #region Retorna o Identificador
        public InfIdentificador obtemIdentificador(Token palavra)
        {
            InfIdentificador infoIdentificador = tabelaSimbolos[palavra];
            return infoIdentificador;
        }
        #endregion

        #region Verifica Identificadores e Palavra reservada
        public Token retornaToken(string lexema, int linha, int coluna)
        {
            foreach (Token token in tabelaSimbolos.Keys)
            {
                if (token.getLexema().Equals(lexema))
                {
                    token.nLinha = linha;
                    token.nColuna = coluna;
                    return token;
                }
            }
            return null;
        }
        #endregion

        #region Encontra Token - Retorna
        public override string ToString()
        {
            string mensagemSaida = " ";

            foreach (Token token in tabelaSimbolos.Keys)
            {
                mensagemSaida += (("\t " + token.ToString()) + "\n");
            }

            return mensagemSaida;
        }
        #endregion

    }
}
