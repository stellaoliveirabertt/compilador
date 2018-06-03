using System.Collections.Generic;

namespace Compiladores
{
    public class TabelaSimbolos
    {
        private Dictionary<Token, InfIdentificador> tabelaSimbolos;

        public TabelaSimbolos()
        {
            tabelaSimbolos = new Dictionary<Token, InfIdentificador>();

            #region Palavras reservadas
            Token palavra;

            palavra = new Token(EnumTab.KW_PROGRAM, "program", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_IF, "if", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_ELSE, "else", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_WHILE, "while", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_WRITE, "write", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_READ, "read", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_NUM, "num", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_CHAR, "char", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_NOT, "not", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_OR, "or", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW_AND, "and", 0, 0);
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
            int posicao = 1;

            foreach (Token token in tabelaSimbolos.Keys)
            {
                mensagemSaida += (("Posição: " + posicao + ": \t " + token.ToString()) + "\n");
            }

            return mensagemSaida;
        }
        #endregion

    }
}
