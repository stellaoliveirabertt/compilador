using System;
using System.IO;
using System.Text;

namespace Compilador
{
    public class AnalisadorLexico
    {
        #region Variaveis

        private static readonly int END_OF_FILE = -1;
        private static int lookahead = 0;
        private static int linha = 1;
        private static int coluna = 0;
        private FileStream arquivo;
        private TabelaSimbolos tabelaSimbolos;

        #endregion

        #region Manipulação de Arquivo
        //Abre arquivo
        public AnalisadorLexico(string arquivoEntrada)
        {
            try
            {
                arquivo = new FileStream("C:\\Users\\stella.moreira\\Downloads\\PrimeiroCerto.txt", FileMode.Open);
            }
            catch (IOException exception)
            {
                Console.WriteLine("\t Erro na abertura de Arquivo " + exception);
                Environment.Exit(1);
                Console.WriteLine("\t Pressione enter para sair do programa. ");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine("\t Erro do programa ou falha na tabela de símbolos " + exception);
                Environment.Exit(2);
                Console.WriteLine("\t Pressione enter para sair do programa. ");
                Console.ReadLine();
            }

            tabelaSimbolos = new TabelaSimbolos();
        }

        //Fecha Arquivo
        public void fecharArquivo()
        {
            try
            {
                arquivo.Close();
            }
            catch (IOException errorFile)
            {
                Console.WriteLine("\t Erro ao fechar o arquivo: " + errorFile);
                Environment.Exit(3);
                Console.WriteLine("\t Pressione enter para sair do programa. ");
                Console.ReadLine();
            }
        }
        #endregion


        public void imprimeTabelaSimbolos()
        {
            Console.Write("\n\t\t    Tabela de Símbolos: \n\n");
            Console.WriteLine(tabelaSimbolos.ToString() + "\n");
        }

        public void sinalizaErro(string mensagem)
        {
            Console.WriteLine("\n\t\t    [Erro Lexico]: " + mensagem + "\n");

        }

        public void retornaPonteiro()
        {
            try
            {
                if (lookahead != END_OF_FILE)
                {
                    arquivo.Position--;
                    coluna -= 1;
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine("\n\t\t    Falha ao retornar a leitura: " + exception);
                Environment.Exit(4);
                Console.WriteLine("\t Pressione enter para sair do programa. ");
                Console.ReadLine();
            }
        }

        public Token proximoToken()
        {
            var lexema = new StringBuilder();
            int estado = 0;
            char caracter;

            while (true)
            {
                caracter = '\u0000';

                try
                {
                    lookahead = arquivo.ReadByte();
                    if (lookahead != END_OF_FILE)
                    {
                        caracter = (char)lookahead;
                    }
                    coluna += 1;
                }
                catch (IOException exception)
                {
                    Console.WriteLine("\t Erro na leitra de Arquivo " + exception);
                    Environment.Exit(3);
                    Console.WriteLine("\t Pressione enter para sair do programa. ");
                    Console.ReadLine();
                }

                #region Analise de estado e modificações do automato
                switch (estado)
                {
                    case 0:
                        if (lookahead == END_OF_FILE)
                            return new Token(EnumTab.EOF, "EOF", linha, coluna);
                        else if (caracter == ' ')
                        {

                        }
                        else if (caracter == '\r')
                        {
                            coluna = 0;
                        }
                        else if (caracter == '\n')
                        {
                            linha += 1;
                            coluna = 0;
                        }
                        else if (caracter == '\t')
                        {
                            coluna += 2;
                        }
                        else if (caracter == '=')
                        {
                            estado = 1;
                        }
                        else if (caracter == '!')
                        {
                            estado = 2;
                        }
                        else if (caracter == '>')
                        {
                            estado = 3;
                        }
                        else if (caracter == '<')
                        {
                            estado = 4;
                        }
                        else if (caracter == '+')
                        {
                            estado = 5;
                            return new Token(EnumTab.OP_AD, "+", linha, coluna);
                        }
                        else if (caracter == '-')
                        {
                            estado = 6;
                            return new Token(EnumTab.OP_MIN, "-", linha, coluna);
                        }
                        else if (caracter == '*')
                        {
                            estado = 7;
                            return new Token(EnumTab.OP_MUL, "*", linha, coluna);
                        }
                        else if (caracter == '/')
                        {
                            estado = 25;
                        }
                        else if (Char.IsDigit(caracter))
                        {
                            estado = 9;
                            lexema.Append(caracter);
                        }
                        else if (Char.IsLetter(caracter))
                        {
                            estado = 10;
                            lexema.Append(caracter);
                        }
                        else if (caracter == '{')
                        {
                            estado = 11;
                            return new Token(EnumTab.SMB_OBC, "{", linha, coluna);
                        }
                        else if (caracter == '}')
                        {
                            estado = 12;
                            return new Token(EnumTab.SMB_CBC, "}", linha, coluna);
                        }
                        else if (caracter == '(')
                        {
                            estado = 13;
                            return new Token(EnumTab.SMB_OPA, "(", linha, coluna);
                        }
                        else if (caracter == ')')
                        {
                            estado = 14;
                            return new Token(EnumTab.SMB_CPA, ")", linha, coluna);
                        }
                        else if (caracter == ',')
                        {
                            estado = 15;
                            return new Token(EnumTab.SMB_COM, ",", linha, coluna);
                        }
                        else if (caracter == ';')
                        {
                            estado = 16;
                            return new Token(EnumTab.SMB_SEM, ";", linha, coluna);
                        }
                        else if (caracter == (char)39)
                        {
                            estado = 22;
                        }
                        else if (caracter == '"')
                        {
                            estado = 23;
                        }
                        else
                        {
                            sinalizaErro("\n\t\t    Caracter inválido: " + caracter +
                                                    " linha: " + linha + " coluna: " + coluna);
                            return null;
                        }
                        break;

                    case 1:
                        if (caracter == '=')
                        {
                            estado = 17;
                            return new Token(EnumTab.OP_EQ, "==", linha, coluna);
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.OP_ASS, "=", linha, coluna);
                        }
                    case 2:
                        if (coluna == '=')
                        {
                            estado = 18;
                            return new Token(EnumTab.OP_NE, "!=", linha, coluna);
                        }
                        else
                        {
                            sinalizaErro("\n\t\t    Caracter inválido [! ou =] indisponíveis: " +
                                                    "\n\t\t\t    Linha: " + linha + " coluna: " + coluna);
                            return null;
                        }
                    case 3:
                        if (caracter == '=')
                        {
                            estado = 19;
                            return new Token(EnumTab.OP_GE, ">=", linha, coluna);
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.OP_GT, ">", linha, coluna);
                        }
                    case 4:
                        if (coluna == '=')
                        {
                            estado = 20;
                            return new Token(EnumTab.OP_LE, "<=", linha, coluna);
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.OP_LT, "<", linha, coluna);
                        }
                    case 9:
                        if (Char.IsDigit(caracter))
                        {
                            estado = 9;
                            lexema.Append(caracter);
                        }
                        else if (caracter == '.')
                        {
                            lexema.Append(caracter);
                            estado = 21;
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.CON_NUM, lexema.ToString(), linha, coluna);
                        }
                        break;
                    case 10:
                        if (Char.IsLetterOrDigit(caracter) || caracter == '_')
                        {
                            estado = 10;
                            lexema.Append(caracter);
                        }
                        else
                        {
                            retornaPonteiro();
                            tabelaSimbolos.ToString();
                            Token token = tabelaSimbolos.retornaToken(lexema.ToString());

                            if (token == null)
                            {
                                Token novoToken = new Token(EnumTab.ID, lexema.ToString(), linha, coluna);
                                tabelaSimbolos.insereIdentificador(novoToken, new InfIdentificador());
                                return novoToken;
                            }
                            return token;
                        }
                        break;
                    case 21:
                        if (Char.IsDigit(caracter))
                        {
                            lexema.Append(caracter);
                            estado = 24;
                        }
                        else
                        {
                            sinalizaErro("\n\t\t    Padrão para número inválido: " + caracter +
                                                    " linha: " + linha + " coluna: " + coluna);
                            return null;
                        }
                        break;
                    case 22:
                        if (caracter == (char)39)
                        {
                            return new Token(EnumTab.CON_CHAR, lexema.ToString(), linha, coluna);
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n\t\t    Conjunto de char deve ser fechado com ' antes do fim de arquivo");
                            return null;
                        }
                        else
                        {
                            lexema.Append(caracter);
                        }
                        break;
                    case 23:
                        if (caracter == '"')
                        {
                            return new Token(EnumTab.LIT, lexema.ToString(), linha, coluna);
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n\t\t    String deve ser fechada com \" antes do fim de arquivo");
                            return null;
                        }
                        else
                        {
                            lexema.Append(caracter);
                        }
                        break;
                    case 24:
                        if (Char.IsDigit(caracter))
                        {
                            lexema.Append(caracter);
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.CON_NUM, lexema.ToString(), linha, coluna);
                        }
                        break;
                    case 25:
                        if (caracter == '*')
                        {
                            estado = 26;
                        }
                        else if (caracter == '/')
                        {
                            estado = 27;
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.OP_DIV, "/", linha, coluna);
                        }
                        break;
                    case 26:
                        if (caracter == '*')
                        {
                            estado = 28;
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n\t\t    Comentário deve ser fechado antes do fim do arquivo com */");
                            estado = 0;
                            coluna = 0;
                            linha += 1;
                        }
                        else
                        {
                            estado = 26;
                        }
                        break;
                    case 27:
                        if (caracter == '\n')
                        {
                            estado = 0;
                            coluna = 0;
                            linha += 1;
                        }
                        else
                        {
                            estado = 27;
                        }
                        break;
                    case 28:
                        if (caracter == '/')
                        {
                            estado = 0;
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n\t\t    Comentário deve ser fechado antes do fim do arquivo com */ ");
                            estado = 0;
                            coluna = 0;
                            linha += 1;
                        }
                        else
                        {
                            estado = 26;
                        }
                        break;
                }
            }
            #endregion
        }
    }
}
