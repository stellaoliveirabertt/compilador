using System.Collections.Generic;

namespace TrabalhoPratico01
{
    public class TabelaSimbolos
    {
        private Dictionary<Token, InfIdentificador> tabelaSimbolos;

        //Construtor
        public TabelaSimbolos()
        {
            tabelaSimbolos = new Dictionary<Token, InfIdentificador>();

            //Palavras reservadas
            Token palavra;

            palavra = new Token(EnumTab.KW, "program", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "if", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "else", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "while", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "write", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "read", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "num", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "char", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "not", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "or", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();

            palavra = new Token(EnumTab.KW, "and", 0, 0);
            tabelaSimbolos[palavra] = new InfIdentificador();
        }

        //Insere o identificaor
        public void insereIdentificador(Token palavra, InfIdentificador ident)
        {
            tabelaSimbolos.Add(palavra, ident);
        }

        // Retorna um identificador de um determinado token
        public InfIdentificador obtemIdentificador(Token palavra)
        {
            InfIdentificador infoIdentificador = tabelaSimbolos[palavra];
            return infoIdentificador;
        }

        //Pesquisa na tabela de símbolos se há algum token com determinado lexema Esse método diferencia ID e KW
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

        //Saída
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

    }
}
