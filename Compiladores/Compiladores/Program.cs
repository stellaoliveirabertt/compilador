using System;

namespace Compiladores
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AnalisadorLex lexer = new AnalisadorLex("teste.txt");
            AnalisadorSint sintatico = new AnalisadorSint(lexer);
            

            #region Versão 01
            //Token token;
            //TabelaSimbolos tabelaSimbolos = new TabelaSimbolos();

            //do
            //{
            //    token = lexer.proximoToken();

            //    if (token != null)
            //    {
            //        Console.WriteLine("-------------------------\n" +
            //                           "Token: " + token.ToString());
            //    }
            //} while (lookahead != END_OF_FILE);
            //lexer.fechaArquivo();
            #endregion

            Console.WriteLine("\n\n\t\tArquivo Finalizado");
            Console.ReadLine();
        }
    }
}
