using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores
{
    public class AnalisadorSint
    {
        #region Variaveis
        private readonly AnalisadorLex lexer;
        private Token token;
        private List<EnumTab> sincroniza;
        #endregion

        public AnalisadorSint(AnalisadorLex lexer)
        {
            this.lexer = lexer;
            token = lexer.proximoToken();
            sincroniza = new List<EnumTab>();
        }

        public void fecharArquivo()
        {
            lexer.fechaArquivo();
        }

        public void erroSintatico(String mensagem)
        {
            Console.WriteLine("[Erro Sintático] na linha: " + token.getLinha() + " e coluna: " + token.getColuna() + ": ");
            Console.WriteLine(mensagem + "\n");
        }

        public void proximoToken()
        {
            token = lexer.proximoToken();
            Console.WriteLine("[DEBUG] " + token.ToString());
        }

        #region Implementação do Skip and Eat
        public void skip(String mensagem)
        {
            erroSintatico(mensagem);
            proximoToken();
        }

        public Boolean eat(EnumTab tab)
        {
            if (token.getClasse() == tab)
            {
                proximoToken();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public void sincronizaToken(String mensagem)
        {
            Boolean casaToken = false;

            while (!casaToken && token.getClasse() != EnumTab.EOF)
            {
                if (sincroniza.Contains(token.getClasse()))
                {
                    casaToken = true;
                }
                else
                {
                    skip(mensagem);
                }
            }
            sincroniza.Clear();
        }

        #region Tabela Preditiva
        //prog - > “program” “id” body
        public void prog()
        {
            if (token.getClasse() == EnumTab.KW_PROGRAM)
            {
                program();
            }
        }

        public void program()
        {
            if (eat(EnumTab.KW_PROGRAM))
            {
                if (!eat(EnumTab.ID))
                {
                    erroSintatico("Esperado \"ID\", encontrado: " + token.getLexema());
                }
                body();
            }
            else
            {
                erroSintatico("Esperado \"public\", encontrado " + token.getLexema());
                sincroniza.Add(EnumTab.EOF);
                sincronizaToken("[MODO PÂNICO] Esperado \"EOF\", encontrado: " + token.getLexema());
            }
        }

        public void body()
        {
           if(token.getClasse() == EnumTab.KW_NUM || token.getClasse() == EnumTab.KW_CHAR)
            {
                decl_list();
                if (!eat(EnumTab.SMB_OBC))
                {
                    erroSintatico("Esperado \"{\", encontrado: " + token.getLexema());
                }
            }
        }

        public void decl_list()
        {

        }

        public void dcl()
        {

        }

        public void type()
        {

        }

        public void idList()
        {

        }

        public void stmList()
        {

        }

        public void stmt()
        {

        }

        public void assignStmt()
        {

        }

        public void ifStmt()
        {

        }

        public void condition()
        {

        }

        public void whileStmt()
        {

        }

        public void stmtPrefix()
        {

        }

        public void readStmt()
        {

        }

        public void writeStmt()
        {

        }

        public void stmtPrefix()
        {

        }

        public void readStmt()
        {

        }

        public void writable()
        {

        }

        public void expression()
        {

        }

        public void simpleExpr()
        {

        }

        public void term()
        {

        }

        public void factoA()
        {

        }

        public void factor()
        {

        }

        public void relop()
        {

        }

        public void addop()
        {

        }

        public void mulop()
        {

        }

        public void constant()
        {

        }
        #endregion
    }
}
