using System;

namespace Compilador
{
    public class AnalisadorSintatico
    {
        private readonly AnalisadorLexico lexo;
        private Token token;

        public AnalisadorSintatico(AnalisadorLexico lexo)
        {
            this.lexo = lexo;
            token = lexo.proximoToken();
            Console.WriteLine("\t\t    [DEBUG]" + token.ToString());
        }

        public void fecharArquivo()
        {
            lexo.fecharArquivo();
        }

        public void erroSintatico(string mensagem)
        {
            Console.WriteLine("\n\n [ERRO SINTATICO] - linha: " + token.linha + " coluna: " + token.coluna);
            Console.WriteLine(mensagem + "\n");
            Console.ReadLine();
        }

        public void avancar()
        {
            token = lexo.proximoToken();
            Console.WriteLine("\t\t    [DEBUG] " + token.ToString());
        }

        public Boolean eat(EnumTab tab)
        {
            if (token.classe == tab)
            {
                avancar();
                return true;
            }
            return false;
        }

        #region Processo do Analisador Sintático

        // prog → “program” “id” body
        public void Prog()
        {
            if (!eat(EnumTab.KW_PROGRAM))
            {
                erroSintatico("\t\t    Esperado \"program\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
            if (!eat(EnumTab.ID))
            {
                erroSintatico("\t\t    Esperado \"id\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

            Body();

        }

        // decl-list “{“ stmt-list “}”
        public void Body()
        {
            Decl_List();

            if (!eat(EnumTab.SMB_OBC))
            {
                erroSintatico("\t\t    Esperado \"{\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

            Stmt_List();

            if (!eat(EnumTab.SMB_CBC))
            {
                erroSintatico("\t\t    Esperado \"}\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        // decl “;” decl-list | ε
        private void Decl_List()
        {
            if (eat(EnumTab.KW_NUM) || eat(EnumTab.KW_CHAR))
            {
                Decl();

                if (eat(EnumTab.SMB_SEM))
                {
                    Decl_List();
                }
                else if (token.classe == EnumTab.SMB_OBC)
                {
                    return;
                }
                else
                {
                    erroSintatico("\t\t    Esperado \"num\" ou \"char\" ou \"ε\", encontrado: " + token.lexema);
                    Environment.Exit(1);
                }
            }
        }

        //decl → type id-list
        private void Decl()
        {
            Type();
            Id_List();
        }

        //id-list → “id” id-list’
        private void Id_List()
        {
            if (eat(EnumTab.ID))
            {
                Id_List2();
            }
            else
            {
                erroSintatico("\t\t    Esperado \"ID\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //id-list’ → “,” id-list 9 | ε 10
        private void Id_List2()
        {
            if (eat(EnumTab.SMB_COM))
            {
                Id_List();
            }
            else if (token.classe == EnumTab.SMB_SEM)
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \",\" ou \"ε\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        // type → “num” | “char”
        private void Type()
        {
            return;
        }

        // stmt “;” stmt-list | ε
        private void Stmt_List()
        {
            if (token.classe == EnumTab.ID || token.classe == EnumTab.KW_IF || token.classe == EnumTab.KW_WHILE || token.classe == EnumTab.KW_READ || token.classe == EnumTab.KW_WRITE)
            {
                Stmt();
                if (!eat(EnumTab.SMB_SEM) && token.classe != EnumTab.SMB_CBC)
                {
                    erroSintatico("\t\t    Esperado \";\", encontrado: " + token.lexema);
                    Environment.Exit(1);
                }
                Stmt_List();
            }
            else if (token.classe == EnumTab.SMB_CBC)
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"id\" ou \"ε\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        // stmt → assign-stmt | if-stmt | while-stmt | read-stmt | write-stmt
        private void Stmt()
        {
            if (eat(EnumTab.ID))
            {
                Assign_stmt();
            }
            else if (eat(EnumTab.KW_IF))
            {
                If_stmt();
            }
            else if (eat(EnumTab.KW_WHILE))
            {
                While_stmt();
            }
            else if (eat(EnumTab.KW_READ))
            {
                Read_stmt();
            }
            else if (eat(EnumTab.KW_WRITE))
            {
                Write_stmt();
            }
            else
            {

                erroSintatico("\t\t    Esperado \"id\" ou \"if\" ou \"while\" ou \"read\" ou \"write\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //write-stmt → “write” writable
        private void Write_stmt()
        {
            //Write já foi lido
            Writable();
        }

        //writable → simple-expr | “literal”
        private void Writable()
        {
            if (token.classe == EnumTab.ID || token.classe == EnumTab.CON_NUM || token.classe == EnumTab.CON_CHAR || token.classe == EnumTab.SMB_OPA || token.classe == EnumTab.KW_NOT)
            {
                Simple_Expr();
            }
            else if (eat(EnumTab.LIT))
            {
                return;
            }
            else
            {

                erroSintatico("\t\t    Esperado \"id\" ou \"CON_NUM\" ou \"CONST_CHAR\" ou \"(\" ou \"not\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //read-stmt → “read” “id”
        private void Read_stmt()
        {
            if (eat(EnumTab.ID))
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"id\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //while-stmt → stmt-prefix “{“ stmt-list “}”
        private void While_stmt()
        {
            Stmt_Prefix();
            if (eat(EnumTab.SMB_OBC))
            {
                Stmt_List();
                if (eat(EnumTab.SMB_CBC))
                {
                    return;
                }
                else
                {
                    erroSintatico("\t\t    Esperado \"}\", encontrado: " + token.lexema);
                    Environment.Exit(1);
                }
            }
            else
            {
                erroSintatico("\t\t    Esperado \"{\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //stmt-prefix → “while” “(“ condition “)”
        private void Stmt_Prefix()
        {
            if (eat(EnumTab.SMB_OPA))
            {
                Condition();
                if (eat(EnumTab.SMB_CPA))
                {
                    return;
                }
                else
                {
                    erroSintatico("\t\t    Esperado \")\", encontrado: " + token.lexema);
                    Environment.Exit(1);
                }
            }
            else
            {
                erroSintatico("\t\t    Esperado \"(\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //if-stmt → “if” “(“ condition “)” “{“ stmt-list “}” if-stmt’
        private void If_stmt()
        {
            if (eat(EnumTab.SMB_OPA))
            {
                Condition();
                if (eat(EnumTab.SMB_CPA))
                {
                    if (eat(EnumTab.SMB_OBC))
                    {
                        Stmt_List();
                        if (eat(EnumTab.SMB_CBC))
                        {
                            If_stmt2();
                        }
                        else
                        {
                            erroSintatico("\t\t    Esperado \"}\", encontrado: " + token.lexema);
                            Environment.Exit(1);
                        }
                    }
                    else
                    {
                        erroSintatico("\t\t    Esperado \"{\", encontrado: " + token.lexema);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    erroSintatico("\t\t    Esperado \")\", encontrado: " + token.lexema);
                    Environment.Exit(1);
                }
            }
            else
            {
                erroSintatico("\t\t    Esperado \"(\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //condition → expression
        private void Condition()
        {
            Expression();
        }

        //expression → simple-expr expression’
        private void Expression()
        {
            Simple_Expr();
            Expression2();
        }

        //relop simple-expr | ε
        private void Expression2()
        {
            if (token.classe == EnumTab.OP_EQ || token.classe == EnumTab.OP_GT || token.classe == EnumTab.OP_GE || token.classe == EnumTab.OP_LE || token.classe == EnumTab.OP_LT || token.classe == EnumTab.OP_NE)
            {
                Relop();
                Simple_Expr();
            }
            else if (token.classe == EnumTab.SMB_CPA)
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"==\" ou \">\" ou \">=\" ou \"<\" ou \"<=\" ou \"!=\" ou \"ε\" encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //relop → “==” | “>” | “>=” | “<” | “<=” | “!=”
        private void Relop()
        {
            if (eat(EnumTab.OP_EQ) || eat(EnumTab.OP_GT) || eat(EnumTab.OP_GE) || eat(EnumTab.OP_LT) || eat(EnumTab.OP_LE) || eat(EnumTab.OP_NE))
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"==\" ou \">\" ou \">=\" ou \"<\" ou \"<=\" ou \"!=\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

        }

        //simple-expr → term simple-expr’
        private void Simple_Expr()
        {
            Term();
            Simple_Expr2();
        }

        //simple-expr’ → addop term simple-expr’ | ε
        private void Simple_Expr2()
        {
            if (token.classe == EnumTab.OP_AD || token.classe == EnumTab.OP_MIN || token.classe == EnumTab.KW_OR)
            {
                Addop();
                Term();
                Simple_Expr2();
            }
            else if (token.classe == EnumTab.SMB_SEM || token.classe == EnumTab.OP_EQ || token.classe == EnumTab.OP_GT || token.classe == EnumTab.OP_GE || token.classe == EnumTab.OP_LT || token.classe == EnumTab.OP_LE || token.classe == EnumTab.OP_NE || token.classe == EnumTab.SMB_CPA)
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"+\" ou \"-\" ou \"or\" ou \"ε\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

        }

        //addop → “+” | “-” | “or”
        private void Addop()
        {
            if (eat(EnumTab.OP_AD) || eat(EnumTab.OP_MIN) || eat(EnumTab.KW_OR))
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"+\" ou \"-\" ou \"or\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

        }

        //term → factor-a term’
        private void Term()
        {
            Factor_A();
            Term2();
        }

        //term’ → mulop factor-a term’ | ε 
        private void Term2()
        {
            if (token.classe == EnumTab.OP_MUL || token.classe == EnumTab.OP_DIV || token.classe == EnumTab.KW_AND)
            {
                Mulop();
                Factor_A();
                Term2();
            }
            else if (token.classe == EnumTab.OP_AD || token.classe == EnumTab.OP_MIN || token.classe == EnumTab.KW_OR || token.classe == EnumTab.SMB_SEM || token.classe == EnumTab.OP_EQ || token.classe == EnumTab.OP_GT || token.classe == EnumTab.OP_GE || token.classe == EnumTab.OP_LT || token.classe == EnumTab.OP_LE || token.classe == EnumTab.OP_NE || token.classe == EnumTab.SMB_CPA)
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"*\" ou \"/\" ou \"and\" ou \"ε\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //mulop → “*” 52 | “/” 53 | “and” 54
        private void Mulop()
        {
            if (eat(EnumTab.OP_MUL) || eat(EnumTab.OP_DIV) || eat(EnumTab.KW_AND))
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"*\" ou \"/\" ou \"and\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

        }

        //factor-a → factor | “not” factor
        private void Factor_A()
        {
            if (token.classe == EnumTab.ID || token.classe == EnumTab.CON_NUM || token.classe == EnumTab.CON_CHAR || token.classe == EnumTab.SMB_OPA)
            {
                Factor();
            }
            else if (eat(EnumTab.KW_NOT))
            {
                Factor();
            }
            else
            {
                erroSintatico("\t\t    Esperado \"id\" ou \"CONST_NUM\" ou \"CON_CHAR\" ou \"(\" ou \"not\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }
        }

        //factor → “id” | constant | “(“ expression “)”
        private void Factor()
        {
            if (eat(EnumTab.ID) || eat(EnumTab.CON_NUM) || eat(EnumTab.CON_CHAR))
            {
                return; // Já lendo o ID, ou o Constant
            }
            else if (eat(EnumTab.SMB_OPA))
            {
                Expression();
                if (eat(EnumTab.SMB_CPA))
                {
                    return;
                }
                else
                {
                    erroSintatico("\t\t    Esperado \")\", encontrado: " + token.lexema);
                    Environment.Exit(1);
                }
            }
            else
            {
                erroSintatico("\t\t    Esperado \"(\" ou \"ID\" ou \"CON_NUM\" ou \"CON_CHAR\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

        }

        //if-stmt’ → “else” “{“ stmt-list “}” | ε
        private void If_stmt2()
        {
            if (eat(EnumTab.KW_ELSE))
            {
                if (eat(EnumTab.SMB_OBC))
                {
                    Stmt_List();
                    if (eat(EnumTab.SMB_CBC))
                    {
                        return;
                    }
                    else
                    {
                        erroSintatico("\t\t    Esperado \"}\", encontrado: " + token.lexema);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    erroSintatico("\t\t    Esperado \"{\", encontrado: " + token.lexema);
                    Environment.Exit(1);
                }
            }
            else if (token.classe == EnumTab.SMB_SEM)
            {
                return;
            }
            else
            {
                erroSintatico("\t\t    Esperado \"else\" ou \"ε\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

        }

        //assign-stmt → “id” “=” simple_expr
        private void Assign_stmt()
        {
            if (eat(EnumTab.OP_ASS))
            {
                Simple_Expr();
            }
            else
            {
                erroSintatico("\t\t    Esperado \"=\", encontrado: " + token.lexema);
                Environment.Exit(1);
            }

        }

        #endregion
    }
}
