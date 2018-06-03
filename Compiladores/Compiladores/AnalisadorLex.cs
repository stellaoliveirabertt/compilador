using System;
using System.IO;
using System.Text;

namespace Compiladores
{
    public class AnalisadorLex
    {
        #region Variaveis
        private static readonly int END_OF_FILE = -1;
        private static int lookahead = 0;
        public static int nLinhas = 1;
        public static int nColunas = 1;
        private FileStream arquivo;
        private TabelaSimbolos tabelaSimbolos;

        #endregion

        #region Leitura de Arquivo
        public AnalisadorLex(String arquivoEntrada)
        {
            tabelaSimbolos = new TabelaSimbolos();
            try
            {
                arquivo = new FileStream("C:\\Users\\stell\\Documents\\Arquivos\\teste.txt", FileMode.Open);
            }
            catch (IOException excep)
            {
                Console.WriteLine("Erro de abertura do arquivo " + arquivoEntrada + "\nException: " + excep);
                Environment.Exit(1);
                Console.ReadLine();
            }
            catch (Exception excep)
            {
                Console.WriteLine("Erro do programa ou falha da tabela de símbolos \nException: " + excep);
                Environment.Exit(2);
            }
        }
        #endregion

        #region Fechar Arquivo
        public void fechaArquivo()
        {
            try
            {
                arquivo.Close();
            }
            catch (IOException errorFile)
            {
                Console.WriteLine("Erro ao fechar o arquivo \nException: " + errorFile);
                Environment.Exit(3);
                Console.ReadLine();
            }
        }
        #endregion

        #region Sinaliza Erro Léxico
        public void sinalizaErro(String mensagem)
        {
            Console.WriteLine("\n\tErro Lexico: " + mensagem + "\n");
            fechaArquivo();
            Console.WriteLine("\n\n\t\tArquivo Finalizado");
            Console.ReadLine();
        }
        #endregion

        #region Retorna uma posição
        public void retornaPonteiro()
        {
            try
            {
                if (lookahead != END_OF_FILE)
                {
                    //arquivo.Seek(arquivo.Position - 1, SeekOrigin.Current);
                    arquivo.Position--;

                }
            }
            catch (IOException excep)
            {
                Console.WriteLine("Falha ao retornar a Leitura \nException: " + excep);
                Environment.Exit(3);
                Console.ReadLine();
            }
        }
        #endregion

        #region Percorre os Tokens
        public Token proximoToken()
        {
            #region Variaveis do Método
            var lexema = new StringBuilder();
            int estado = 1;
            char caracter;
            #endregion

            #region Percorrendo o Arquivo
            for (var i = 0; i <= arquivo.Length; i++)
            {
                caracter = '\u0000'; //Caracter Null

                //Avança
                try
                {
                    lookahead = arquivo.ReadByte();
                    if (lookahead != END_OF_FILE)
                    {
                        caracter = (char)lookahead;
                    }
                }
                catch (IOException excep)
                {
                    Console.WriteLine("Erro na leitura do arquivo");
                    Environment.Exit(3);
                    Console.ReadLine();
                }
                #endregion

                switch (estado)
                {
                    #region Case 1
                    case 1:
                        if (lookahead == END_OF_FILE)
                            return new Token(EnumTab.EOF, "EOF", nLinhas, nColunas);
                        else if (caracter == ' ' || caracter == '\t' || caracter == '\n' || caracter == '\r')
                        {
                            switch (caracter)
                            {
                                case ' ':
                                    nColunas += 1;
                                    break;
                                case '\t':
                                    nColunas += 3;
                                    break;
                                case '\n':
                                    nLinhas += 1;
                                    nColunas = 1;
                                    break;
                                case '\r':
                                    nColunas += 1;
                                    break;
                            }
                        }
                        //Verificação de letra unicode 
                        else if (Char.IsLetter(caracter))
                        {
                            //Acrescenta uma cópia da cadeia de caracteres especificada a essa instância.
                            lexema.Append(caracter);
                            estado = 14; // Redireciona no case de teste == ( _ )
                        }
                        //Verificação de numero unicode 
                        else if (Char.IsDigit(caracter))
                        {
                            //Acrescenta uma cópia da cadeia de caracteres especificada a essa instância.
                            lexema.Append(caracter);
                            estado = 12; // Redireciona no case de teste == ( . )
                        }
                        else if (caracter == '<')
                        {
                            estado = 6;
                        }
                        else if (caracter == '>')
                        {
                            estado = 9;
                        }
                        else if (caracter == '=')
                        {
                            estado = 2;
                        }
                        else if (caracter == '!')
                        {
                            estado = 4;
                        }
                        else if (caracter == '/')
                        {
                            estado = 16;
                        }
                        else if (caracter == '*')
                        {
                            estado = 18;
                            return new Token(EnumTab.OP_MUL, "*", nLinhas, nColunas);
                        }
                        else if (caracter == '+')
                        {
                            estado = 19;
                            return new Token(EnumTab.OP_AD, "+", nLinhas, nColunas);
                        }
                        else if (caracter == '-')
                        {
                            estado = 20;
                            return new Token(EnumTab.OP_MIN, "-", nLinhas, nColunas);
                        }
                        else if (caracter == ';')
                        {
                            estado = 21;
                            return new Token(EnumTab.SMB_SEM, ";", nLinhas, nColunas);
                        }
                        else if (caracter == ',')
                        {
                            estado = 30;
                            return new Token(EnumTab.SMB_COM, ",", nLinhas, nColunas);
                        }
                        else if (caracter == '(')
                        {
                            estado = 22;
                            return new Token(EnumTab.SMB_OPA, "(", nLinhas, nColunas);
                        }
                        else if (caracter == ')')
                        {
                            estado = 23;
                            return new Token(EnumTab.SMB_CPA, ")", nLinhas, nColunas);
                        }
                        else if (caracter == '{')
                        {
                            estado = 24;
                            return new Token(EnumTab.SMB_CPA, "{", nLinhas, nColunas);
                        }
                        else if (caracter == '}')
                        {
                            estado = 25;
                            return new Token(EnumTab.SMB_CBC, "}", nLinhas, nColunas);
                        }
                        else if (caracter == '"')
                        {
                            estado = 26;
                        }
                        else
                        {
                            sinalizaErro("\n-------------------------\nErro: Caracter ínvalido " + caracter + " cuja linha é: " + nLinhas + " e a coluna é: " + nColunas);
                        }
                        break;
                    #endregion

                    #region Case 2
                    case 2:
                        if (caracter == '=')
                        {
                            estado = 3;
                            return new Token(EnumTab.OP_EQ, "==", nLinhas, nColunas);
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.OP_ASS, "=", nLinhas, nColunas);
                        }
                    #endregion

                    #region Case 4
                    case 4:
                        if (caracter == '=')
                        {
                            estado = 5;
                            return new Token(EnumTab.OP_NE, "!=", nLinhas, nColunas);
                        }
                        else
                        {
                            retornaPonteiro();
                            sinalizaErro("\n-------------------------\nErro: Token incompleto para o caracter: " + caracter + " cuja linha é: " + nLinhas + " e a coluna é: " + nColunas);
                        }
                        break;
                    #endregion

                    #region Case 6
                    case 6:
                        if (caracter == '=')
                        {
                            estado = 7;
                            return new Token(EnumTab.OP_LE, "<=", nLinhas, nColunas);
                        }
                        else
                        {
                            estado = 8;
                            retornaPonteiro();
                            return new Token(EnumTab.OP_LT, "<", nLinhas, nColunas);
                        }
                    #endregion

                    #region Case 9
                    case 9:
                        if (caracter == '=')
                        {
                            estado = 10;
                            return new Token(EnumTab.OP_GE, ">=", nLinhas, nColunas);
                        }
                        else
                        {
                            estado = 11;
                            retornaPonteiro();
                            return new Token(EnumTab.OP_GT, ">", nLinhas, nColunas);
                        }
                    #endregion

                    #region Case 12
                    case 12:
                        if (Char.IsDigit(caracter))
                        {
                            lexema.Append(caracter);
                        }
                        else if (caracter == '.')
                        {
                            lexema.Append(caracter);
                            estado = 26;
                        }
                        else
                        {
                            estado = 13;
                            retornaPonteiro();
                            return new Token(EnumTab.NUM_CONST, lexema.ToString(), nLinhas, nColunas);
                        }
                        break;
                    #endregion

                    #region Case 14
                    case 14:
                        if (Char.IsLetterOrDigit(caracter) || caracter == '_')
                        {
                            lexema.Append(caracter);
                        }
                        else
                        {
                            estado = 15;
                            retornaPonteiro();
                            Token token = tabelaSimbolos.retornaToken(lexema.ToString(), nLinhas, nColunas);

                            if (token == null)
                            {
                                return new Token(EnumTab.ID, lexema.ToString(), nLinhas, nColunas);
                            }
                            return token;
                        }
                        break;
                    #endregion

                    #region Case 16
                    case 16:
                        if (caracter == '/')
                        {
                            estado = 17;
                        }
                        else if (caracter == '*')
                        {
                            estado = 18;
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: Comentário deve ser fechada com */ antes do fim de arquivo");
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.OP_DIV, "/", nLinhas, nColunas);
                        }
                        break;
                    #endregion

                    #region Case 17
                    case 17:
                        estado = 30;
                        break;
                    #endregion

                    #region Case 18
                    case 18:
                        estado = 31;
                        break;
                    #endregion

                    #region Case 21
                    case 21:
                        if (caracter == '\n' || lookahead == END_OF_FILE)
                        {
                            return null;
                        }
                        break;
                    #endregion

                    #region Case 22
                    case 22:
                        if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: String deve ser fechada com ) antes do fim de arquivo");
                        }
                        else
                        {
                            lexema.Append(caracter);
                        }
                        break;
                    #endregion

                    #region Case 24
                    case 24:
                        if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: String deve ser fechada com } antes do fim de arquivo");
                        }
                        else
                        {
                            lexema.Append(caracter);
                        }
                        break;
                    #endregion

                    #region Case 26
                    case 26:
                        if (caracter == '\0' || lookahead == END_OF_FILE || caracter == '\n')
                        {
                            sinalizaErro("\n-------------------------\nErro: String deve ser fechada com \" antes do fim de arquivo");
                        }
                        else if (caracter == '"')
                        {
                            return new Token(EnumTab.STRING, lexema.ToString(), nLinhas, nColunas);
                        }
                        else
                        {
                            lexema.Append(caracter);
                        }
                        break;
                    #endregion

                    #region Case 27
                    case 27:
                        if (caracter == '"')
                        {
                            estado = 27;
                            return null;
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: String deve ser fechada antes do fim do arquivo");
                        }
                        else
                        {
                            lexema.Append(caracter);
                        }
                        break;
                    #endregion

                    #region Case 28
                    case 28:
                        if (Char.IsDigit(caracter))
                        {
                            lexema.Append(caracter);
                            estado = 27;
                        }
                        else
                        {
                            sinalizaErro("\n-------------------------\nErro: Padrão DOUBLE inválido na linha: " + nLinhas + " e na coluna: " + nColunas);
                        }
                        break;
                    #endregion

                    #region Case 29
                    case 29:
                        if (Char.IsDigit(caracter))
                        {
                            lexema.Append(caracter);
                        }
                        else
                        {
                            retornaPonteiro();
                            return new Token(EnumTab.NUM_CONST, lexema.ToString(), nLinhas, nColunas);
                        }
                        break;
                    #endregion

                    #region Case 30
                    case 30:
                        if (caracter == '\n' || lookahead == END_OF_FILE)
                        {
                            return null;
                        }
                        break;
                    #endregion

                    #region Case 31
                    case 31:
                        if (caracter == '\0' || lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: String deve ser fechada com */ antes do fim de arquivo");
                        }
                        if (caracter == '*')
                        {
                            estado = 34;
                        }
                        else
                        {
                            estado = 33;
                        }
                        break;
                    #endregion

                    #region Case 32
                    case 32:
                        if (caracter == '/')
                        {
                            break;
                        }
                        else if (caracter == '\n' || lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: Comentário deve ser fechada com */ antes do fim de arquivo");
                        }
                        break;
                    #endregion

                    #region Case 33
                    case 33:
                        if (caracter == '*')
                        {
                            estado = 34;
                        }
                        else if (caracter == '\0' || lookahead == END_OF_FILE || caracter == '/')
                        {
                            sinalizaErro("\n-------------------------\nErro: Comentário deve ser fechada com */ antes do fim de arquivo");
                        }
                        else if (caracter == '\n' || caracter == '\r')
                        {
                            estado = 33;
                        }
                        break;
                    #endregion

                    #region Case 34
                    case 34:
                        if (caracter == '/')
                        {
                            return null;
                        }
                        else if (caracter == '\0' || lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: Comentário deve ser fechada com */ antes do fim de arquivo");
                        }
                        break;
                        #endregion
                }
            }
            return null;
        }
        #endregion
    }
}
