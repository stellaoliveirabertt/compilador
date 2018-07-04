using System;

namespace Compilador
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AnalisadorLexico lexo = new AnalisadorLexico("PrimeiroCerto.txt");
            AnalisadorSintatico sintatico = new AnalisadorSintatico(lexo);

            sintatico.Prog();
            sintatico.fecharArquivo();
            lexo.imprimeTabelaSimbolos();
            Console.WriteLine("Programa compilado !");
            Console.ReadLine();
        }
    }
}
