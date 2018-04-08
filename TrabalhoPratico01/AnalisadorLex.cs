using System;
using System.IO;
using System.Text;

namespace TrabalhoPratico01
{
    public class AnalisadorLex
    {
        #region Variaveis
        private static readonly int END_OF_FILE = -1; //Fim de arquivo 
        private static int lookahead = 0; //Armazena o último caracter lido
        public static int nLinhas = 1; //Contador de Linhas
        public static int nColunas = 1; // " de colunas
        private FileStream arquivo; //Referencia para o arquivo
        private TabelaSimbolos tabelaSimbolos; //Tabela de Simbolos      

        #endregion

        // Construtor abre o arquivo de: xxxxx e verifica
        public AnalisadorLex(String arquivoEntrada)
        {
            tabelaSimbolos = new TabelaSimbolos();
            try
            {
                #region Leitura de Arquivo
                arquivo = new FileStream("E:\\StellaOliveiraMoreira\\TrabalhoPratico01\\Arquivos_Teste\\Teste01.txt", FileMode.Open);
                #endregion
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

        //Fecha o arquivo aberto
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

        //Sinaliza erro do token
        public void sinalizaErro(String mensagem)
        {
            Console.WriteLine("\n\tErro Lexico: " + mensagem + "\n");
            fechaArquivo();
            Console.WriteLine("\n\n\t\tArquivo Finalizado");
            Console.ReadLine();
        }

        //Retorna uma posição do buffer de leitura
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

        //Obtem o próximo token

        public Token proximoToken()
        {
            var lexema = new StringBuilder();
            int estado = 1;
            char caracter;

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
                #region Analise de Token
                //Inicia a movimentação do automato            
                switch (estado)
                {
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
                    case 17:
                        estado = 30;
                        break;
                    case 18:
                        estado = 31;
                        break;
                    case 21:
                        if (caracter == '\n' || lookahead == END_OF_FILE)
                        {
                            return null;
                        }
                        break;
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
                    case 30:
                        if (caracter == '\n' || lookahead == END_OF_FILE)
                        {
                            return null;
                        }
                        break;
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
                    case 32:
                        if(caracter == '/')
                        {
                            break;
                        }
                        else if (caracter == '\n' || lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: Comentário deve ser fechada com */ antes do fim de arquivo");
                        }
                        break;
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
                    case 34:
                        if(caracter == '/')
                        {
                            return null;
                        }
                        else if (caracter == '\0' || lookahead == END_OF_FILE)
                        {
                            sinalizaErro("\n-------------------------\nErro: Comentário deve ser fechada com */ antes do fim de arquivo");
                        }
                        break;
                }
            }
            return null;
        }
        #endregion

        public static void Main(string[] args)
        {
            //Parametro de arquivo
            AnalisadorLex lexer = new AnalisadorLex("Teste01.txt");
            Token token;
            TabelaSimbolos tabelaSimbolos = new TabelaSimbolos();

            //Enquanto não houver erros e nem for fim de arquivo:
            do
            {
                token = lexer.proximoToken();

                //Imprime o Token
                if (token != null)
                {
                    Console.WriteLine("-------------------------\n" +
                                       "Token: " + token.ToString());
                }
            } while (lookahead != END_OF_FILE);
            lexer.fechaArquivo();
            Console.WriteLine("\n\n\t\tArquivo Finalizado");
            Console.ReadLine();
        }
    }
}
