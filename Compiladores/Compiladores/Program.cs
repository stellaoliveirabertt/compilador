using System;

namespace Compiladores
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AnalisadorLex lexico = new AnalisadorLex("teste.txt");
            AnalisadorSint sintatico = new AnalisadorSint(lexico);

            //Inicia o processo
            sintatico.prog();
            sintatico.fecharArquivo();

           // lexico.imprimeTabelaSimbolos();
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
