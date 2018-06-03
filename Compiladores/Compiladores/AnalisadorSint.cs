using System;
using System.Collections.Generic;

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
            Console.WriteLine("[ERRO SINTATICO]: " + token.LinhaPercorrida());
            Console.WriteLine(mensagem + "\n");
        }

        public void proximoToken()
        {
            Console.WriteLine("[DEBUG]: " + token.ToString());
            token = lexer.proximoToken();
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
        //prog -> “program” “id” body |Fol| $
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
                erroSintatico("Esperado \"program\", encontrado: " + token.getLexema());
                //FOLLOW PROG()
                sincroniza.Add(EnumTab.EOF);
                sincronizaToken("[MODO PÂNICO] Esperado \"EOF\", encontrado: " + token.getLexema());
            }
        }

        //body -> “num”, “char”, “{“ |Fol| $
        public void body()
        {
            if (token.getClasse() == EnumTab.KW_NUM || token.getClasse() == EnumTab.KW_CHAR || token.getClasse() == EnumTab.SMB_OBC)
            {
                decl_list();
                if (!eat(EnumTab.SMB_OBC))
                {
                    erroSintatico("Esperado: \"{\", encontrado: " + token.getLexema());
                }
                stmtList();
                if (!eat(EnumTab.SMB_CBC))
                {
                    erroSintatico("Esperado: \"}\", encontrado: " + token.getLexema());
                }
            }
            else
            {
                erroSintatico("Esperado \"NUM ou CHAR\", encontrado: " + token.getLexema());
                //FOLLOW BODY()
                sincroniza.Add(EnumTab.EOF);
                sincronizaToken("[MODO PÂNICO] Esperado \"EOF\", encontrado: " + token.getLexema());
            }
        }

        //dcl-list -> “num”, “char”, “ε” |Fol| “{“
        public void decl_list()
        {
            if (token.getClasse() == EnumTab.KW_NUM || token.getClasse() == EnumTab.KW_CHAR)
            {
                dcl();
                if (!eat(EnumTab.SMB_SEM))
                {
                    //retorna para o método para inclusão de novas palavras reservadas
                    decl_list();
                }
                else
                {
                    erroSintatico("Esperado \";\" encontrado: " + token.getLexema());
                }
            }
            else if (token.getClasse() == EnumTab.SMB_OBC)
            {
                proximoToken();
            }
            else
            {
                erroSintatico("Esperado \"NUM ou CHAR\" encontrado: " + token.getLexema());
                // FOLLOW DECL_LIST()
                sincroniza.Add(EnumTab.SMB_OBC);
                sincronizaToken("[MODO PÂNICO] Esperado \"{\", encontrado: " + token.getLexema());
            }
        }

        //decl -> “num”, “char” |Fol| “;”
        public void dcl()
        {
            if (token.getClasse() == EnumTab.KW_NUM || token.getClasse() == EnumTab.KW_CHAR)
            {
                type();
                idList();
            }
            else
            {
                erroSintatico("Esperado NUM ou CHAR, encontrado: " + token.getLexema());
                //FOLLOW DCL()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado \";\", encontrado " + token.getLexema());
            }
        }

        //type -> “num”, “char” |Fol| “id”
        public void type()
        {
            if (!eat(EnumTab.KW_NUM) && (!eat(EnumTab.KW_CHAR)))
            {
                erroSintatico("Esperado \"NUM ou CHAR\" encontrado: " + token.getLexema());
                //FOLLOW TYPE()
                sincroniza.Add(EnumTab.ID);
                sincronizaToken("[MODO PÂNICO] Esperado \"ID\", encontrado: " + token.getLexema());
            }
        }

        #region IDLIST
        //id-list -> “id”  |Fol| “;”
        public void idList()
        {
            if (eat(EnumTab.ID))
            {
                idListLinha();
            }
            else
            {
                erroSintatico("Esperado \"ID\", encontrado: " + token.getLexema());
                //FOLLOW IDLIST()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado \";\", encontrado: " + token.getLexema());
            }
        }

        //id-list’ -> “,”, “ε” |Fol| “;”
        public void idListLinha()
        {
            if (eat(EnumTab.SMB_COM))
            {
                idList();
            }
            else if (token.getClasse() == EnumTab.SMB_SEM)
            {
                proximoToken();
            }
            else
            {
                erroSintatico("Esperado \", ou , ou ;\", encontrado: " + token.getLexema());
                //FOLLOW IDLISTLINHA()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado: \";\", encontrado: " + token.getLexema());
            }
        }
        #endregion 

        //stmt-list -> “id”, “if”, “while”, “read”, “write”, “ε” |Fol| “}”
        public void stmtList()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.KW_IF ||
                token.getClasse() == EnumTab.KW_WHILE || token.getClasse() == EnumTab.KW_READ ||
                token.getClasse() == EnumTab.KW_WRITE)
            {
                stmt();
                if (!eat(EnumTab.SMB_SEM))
                {
                    erroSintatico("Esperado \";\", encontrado: " + token.getLexema());
                }
                stmtList();
            }
            else if (token.getClasse() == EnumTab.SMB_CBC)
            {
                proximoToken();
            }
            else
            {
                erroSintatico("Esperado \"ID ou IF ou WHILE ou READ ou WRITE\", encontrado: " + token.getLexema());
                //FOLLOW STMTLIST()
                sincroniza.Add(EnumTab.SMB_CBC);
                sincronizaToken("[MODO PÂNICO] Esperado \"}\", encontrado: " + token.getLexema());
            }
        }

        //stmt -> “id”, “if”, “while”, “read”, “write” |Fol| “;”
        public void stmt()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.KW_IF || token.getClasse() == EnumTab.KW_WHILE ||
                token.getClasse() == EnumTab.KW_READ || token.getClasse() == EnumTab.KW_WRITE)
            {
                if (token.getClasse() == EnumTab.ID)
                {
                    assignStmt();
                }
                if (token.getClasse() == EnumTab.KW_IF)
                {
                    ifStmt();
                }
                if (token.getClasse() == EnumTab.KW_WHILE)
                {
                    whileStmt();
                }
                if (token.getClasse() == EnumTab.KW_READ)
                {
                    readStmt();
                }
                if (token.getClasse() == EnumTab.KW_WRITE)
                {
                    writeStmt();
                }
            }
            else
            {
                erroSintatico("Esperado \"ID ou IF ou WHILE ou READ ou WRITE\", encontrado: " + token.getLexema());
                //FOLLOW STMT()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado \"}\", encontrado: " + token.getLexema());
            }

        }

        //assign-stmt -> “id” |Fol| “;”
        public void assignStmt()
        {
            if (eat(EnumTab.ID))
            {
                if (!eat(EnumTab.OP_ASS))
                {
                    erroSintatico("Esperado \"!=\", encontrado: " + token.getLexema());
                }
                simpleExpr();
            }
            else
            {
                erroSintatico("Esperado: \"ID\", encontrado: " + token.getLexema());
                //FOLLOW ASSIGNSTMT()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado: \";\", encontrado: " + token.getLexema());
            }
        }

        #region ifSTMT()
        //if-stmt -> “if” |Fol| “;”
        public void ifStmt()
        {
            if (eat(EnumTab.KW_IF))
            {
                if (!eat(EnumTab.SMB_OPA))
                {
                    erroSintatico("Esperado \"(\", encontrado: " + token.getLexema());
                }
                condition();
                if (!eat(EnumTab.SMB_CPA))
                {
                    erroSintatico("Esperado \")\", encontrado: " + token.getLexema());
                }
                if (!eat(EnumTab.SMB_OBC))
                {
                    erroSintatico("Esperado \"{\", encontrado: " + token.getLexema());
                }
                stmtList();
                if (!eat(EnumTab.SMB_CBC))
                {
                    erroSintatico("Esperado \"}\", encontrado: " + token.getLexema());
                }
                ifStmtLinha();
            }
            else
            {
                erroSintatico("Esperado \"IF\", encontrado: " + token.getLexema());
                //FOLLOW IFSTMT()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado \";\", encontrado: " + token.getLexema());
            }
        }

        //if-stmt’ -> “else”, “ε” |Fol| “;”
        public void ifStmtLinha()
        {
            if (token.getClasse() == EnumTab.KW_ELSE || token.getClasse() == EnumTab.SMB_COM
                || token.getClasse() == EnumTab.SMB_SEM)
            {
                if (eat(EnumTab.KW_ELSE))
                {
                    if (!eat(EnumTab.SMB_OBC))
                    {
                        erroSintatico("Esperado \"{\", encontrado: " + token.getLexema());
                    }
                    stmtList();
                    if (!eat(EnumTab.SMB_CBC))
                    {
                        erroSintatico("Esperado \"}\", encontrado: " + token.getLexema());
                    }
                }
                else if (token.getClasse() == EnumTab.SMB_SEM)
                {
                    proximoToken();
                }
            }
            else
            {
                erroSintatico("Esperado \"ELSE ou , ou ;\", encontrado: " + token.getLexema());
                //FOLLOW IFSTMTLINHA()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado: \";\", encontrado: " + token.getLexema());
            }
        }
        #endregion

        //condition -> “id”, “num_const”, “char_const”, “(“, “not” |Fol| “)”
        public void condition()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.NUM_CONST || token.getClasse() == EnumTab.CHAR_CONST
                 || token.getClasse() == EnumTab.SMB_OPA || token.getClasse() == EnumTab.KW_NOT)
            {
                expression();
            }
            else
            {
                erroSintatico("Esperado \"ID ou NUM_CONST ou CHAR_CONST ou ( ou NOT\", encontrado: " + token.getLexema());
                //FOLLOW condition()
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado: \")\", encontrado: " + token.getLexema());
            }
        }

        //while-stmt -> “while” |Fol| “;”
        public void whileStmt()
        {
            if (token.getClasse() == EnumTab.KW_WHILE)
            {
                stmtPrefix();
                if (!eat(EnumTab.SMB_OBC))
                {
                    erroSintatico("Esperado \"{\", encontrado: " + token.getLexema());
                }
                stmtList();
                if (!eat(EnumTab.SMB_CBC))
                {
                    erroSintatico("Esperado \"}\", encontrado: " + token.getLexema());
                }
            }
            else
            {
                erroSintatico("Esperado \"WHILE\", encontrado: " + token.getLexema());
                //FOLLOW WHILESTMT()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado \";\", encontrado: " + token.getLexema());
            }
        }

        //stmt-prefix -> “while” |Fol| “{”
        public void stmtPrefix()
        {
            if (eat(EnumTab.KW_WHILE))
            {
                if (!eat(EnumTab.SMB_OPA))
                {
                    erroSintatico("Esperado \"{\", encontrado: " + token.getLexema());
                }
                condition();
                if (!eat(EnumTab.SMB_CPA))
                {
                    erroSintatico("Esperado \"}\", encontrado: " + token.getLexema());
                }
            }
            else
            {
                erroSintatico("Esperado \"WHILE\", encontrado: " + token.getLexema());
                //FOLLOW WHILE()
                sincroniza.Add(EnumTab.SMB_OBC);
                sincronizaToken("[MODO PÂNICO] Esperado \"{\", encontrado: " + token.getLexema());
            }
        }

        //read-stmt -> “read” |Fol| “;”
        public void readStmt()
        {
            if (eat(EnumTab.KW_READ))
            {
                if (!eat(EnumTab.ID))
                {
                    erroSintatico("Esperado: \"ID\", encontrado: " + token.getLexema());
                }
            }
            else
            {
                erroSintatico("Esperado: \"READ\", encontrado: " + token.getLexema());
                //FOLLOW READ()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado \";\", encontrado: " + token.getLexema());
            }
        }

        //write-stmt -> “write” |Fol| “;”
        public void writeStmt()
        {
            if (eat(EnumTab.KW_WRITE))
            {
                writable();
            }
            else
            {
                erroSintatico("Esperado WRITE\", encontrado: " + token.getLexema());
                //FOLLOW WRITE()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado \";\", encontrado: " + token.getLexema());
            }
        }

        //writable -> “id”, “num_const”, “char_const”, “(“, “not”, “literal” |Fol| “;” 
        //OBS _ "LITERAL" NÃO PERTENCE A LINGUAGEM
        public void writable()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.NUM_CONST ||
                token.getClasse() == EnumTab.CHAR_CONST || token.getClasse() == EnumTab.SMB_OPA ||
                token.getClasse() == EnumTab.KW_NOT)
            {
                simpleExpr();
            }
            else
            {
                erroSintatico("Esperado \"ID ou NUM_CONST ou CHAR_CONST ou ) ou NOT\", encontrado: " + token.getLexema());
                //FOLLOW WRITABLE()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincronizaToken("[MODO PÂNICO] Esperado: \";\", encontrado: " + token.getLexema());
            }
        }

        #region Expression()
        //expression -> “id”, “num_const”, “char_const”, “(“, “not” |Fol| “)”
        public void expression()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.NUM_CONST ||
                token.getClasse() == EnumTab.CHAR_CONST || token.getClasse() == EnumTab.SMB_OPA ||
                token.getClasse() == EnumTab.KW_NOT)
            {
                simpleExpr();
                expressionLinha();
            }
            else
            {
                erroSintatico("Esperado \"ID ou NUM_CONST ou CHAR_CONST ou NOT\", encontrado: " + token.getLexema());
                //FOLLOW EXPRESSION()
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado: \")\", encontrado: " + token.getLexema());
            }
        }

        //expression’ -> “==”, “>”, “>=”, “<”, “<=”, “!=”, “ε” |Fol| “)”
        public void expressionLinha()
        {
            if (token.getClasse() == EnumTab.OP_EQ || token.getClasse() == EnumTab.OP_GT ||
                token.getClasse() == EnumTab.OP_GE || token.getClasse() == EnumTab.OP_LT ||
                token.getClasse() == EnumTab.OP_LE || token.getClasse() == EnumTab.OP_NE)
            {
                relop();
                simpleExpr();
            }
            else
            {
                erroSintatico("Esperado \"== ou > ou >= ou < ou <= ou !=\", encontrado: " + token.getLexema());
                //FOLLOW expresionLinha()
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado: \")\", encontrado: " + token.getLexema());
            }
        }

        #endregion

        #region SimpleExpr()
        //simple-expr -> “id”, “num_const”, “char_const”, “(“, “not” |Fol| “;”, “==”, “>”, “>=”, “<”, “<=”, “!=”, “)”
        public void simpleExpr()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.NUM_CONST || token.getClasse() == EnumTab.CHAR_CONST ||
                token.getClasse() == EnumTab.SMB_OPA || token.getClasse() == EnumTab.KW_NOT)
            {
                term();
                simpleExprLinha();
            }
            else erroSintatico("Esperado \"ID ou NUM_CONST ou CHAR_CONST ou ( ou NOT\", encontrado: " + token.getLexema());
            //FOLLOW SIMPLEEXPR()
            sincroniza.Add(EnumTab.SMB_SEM);
            sincroniza.Add(EnumTab.OP_EQ);
            sincroniza.Add(EnumTab.OP_GT);
            sincroniza.Add(EnumTab.OP_GE);
            sincroniza.Add(EnumTab.OP_LT);
            sincroniza.Add(EnumTab.OP_LE);
            sincroniza.Add(EnumTab.OP_NE);
            sincroniza.Add(EnumTab.SMB_CPA);
            sincronizaToken("[MODO PÂNICO] Esperado \"; ou == ou > ou >= ou < ou <= ou != ou )\", encontrado: " + token.getLexema());
        }


        //simple-expr’ -> “+”, “-”, “or”, “ε” |Fol| “;”, “==”, “>”, “>=”, “<”, “<=”,“!=”, “)”
        public void simpleExprLinha()
        {
            if (token.getClasse() == EnumTab.OP_AD || token.getClasse() == EnumTab.OP_MIN ||
                token.getClasse() == EnumTab.KW_OR)
            {
                addop();
                term();
                simpleExprLinha();
            }
            else if (token.getClasse() == EnumTab.SMB_SEM || token.getClasse() == EnumTab.OP_EQ ||
                token.getClasse() == EnumTab.OP_LT || token.getClasse() == EnumTab.OP_LE ||
                token.getClasse() == EnumTab.OP_GT || token.getClasse() == EnumTab.OP_GE ||
                token.getClasse() == EnumTab.OP_NE || token.getClasse() == EnumTab.SMB_CPA)
            {
                proximoToken();
            }
            else
            {
                erroSintatico("Esperado \"+ ou - ou OR\", encontrado: " + token.getLexema());
                //FOLLOW simpleExprLinha()
                sincroniza.Add(EnumTab.SMB_SEM);
                sincroniza.Add(EnumTab.OP_EQ);
                sincroniza.Add(EnumTab.OP_GT);
                sincroniza.Add(EnumTab.OP_GE);
                sincroniza.Add(EnumTab.OP_LT);
                sincroniza.Add(EnumTab.OP_LE);
                sincroniza.Add(EnumTab.OP_NE);
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado: \"; ou == ou > ou >= ou < ou <= ou != ou )\", encontrado: " + token.getLexema());
            }
        }
        #endregion

        #region Term()
        //term -> “id”, “num_const”, “char_const”, “(“, “not” |Fol| “+”, “-”, “or”, “;”, “==”, “>”, “>=”, “<”, “<=”, “!=”, “)”
        public void term()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.NUM_CONST ||
                token.getClasse() == EnumTab.CHAR_CONST || token.getClasse() == EnumTab.SMB_OPA
                || token.getClasse() == EnumTab.KW_NOT)
            {
                factoA();
                termLinha();
            }
            else
            {
                erroSintatico("Esperado \"ID ou NUM_CONST ou CHAR_CONST ou ) ou NOT\", encontrado: " + token.getLexema());
                //FOLLOW term()
                sincroniza.Add(EnumTab.OP_AD);
                sincroniza.Add(EnumTab.OP_MIN);
                sincroniza.Add(EnumTab.KW_OR);
                sincroniza.Add(EnumTab.SMB_SEM);
                sincroniza.Add(EnumTab.OP_EQ);
                sincroniza.Add(EnumTab.OP_GT);
                sincroniza.Add(EnumTab.OP_GE);
                sincroniza.Add(EnumTab.OP_LT);
                sincroniza.Add(EnumTab.OP_LE);
                sincroniza.Add(EnumTab.OP_NE);
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado: \"+ ou - ou OR ou ; ou == ou > ou >= ou < ou <= ou != ou )\", encontrado: " + token.getLexema());
            }
        }

        //term’ -> “*”, “/”, “and”, “ε” |Fol| “+”, “-”, “or”, “;”, “==”, “>”, “>=”, “<”, “<=”, “!=”, “)”
        public void termLinha()
        {
            if (token.getClasse() == EnumTab.OP_MUL || token.getClasse() == EnumTab.OP_DIV || token.getClasse() == EnumTab.KW_AND)
            {
                mulop();
                factoA();
                termLinha();
            }
            else if (token.getClasse() == EnumTab.OP_AD || token.getClasse() == EnumTab.OP_MIN ||
                token.getClasse() == EnumTab.KW_OR || token.getClasse() == EnumTab.SMB_SEM || token.getClasse() == EnumTab.OP_EQ ||
                token.getClasse() == EnumTab.OP_LT || token.getClasse() == EnumTab.OP_LE ||
                token.getClasse() == EnumTab.OP_GT || token.getClasse() == EnumTab.OP_GE ||
                token.getClasse() == EnumTab.OP_NE || token.getClasse() == EnumTab.SMB_CPA)
            {
                proximoToken();
            }
            else
            {
                erroSintatico("Esperado \"* ou / ou AND\", encontrado " + token.getLexema());
                //FOLLOW TERMLINHA()
                sincroniza.Add(EnumTab.OP_AD);
                sincroniza.Add(EnumTab.OP_MIN);
                sincroniza.Add(EnumTab.KW_OR);
                sincroniza.Add(EnumTab.SMB_SEM);
                sincroniza.Add(EnumTab.OP_EQ);
                sincroniza.Add(EnumTab.OP_GT);
                sincroniza.Add(EnumTab.OP_GE);
                sincroniza.Add(EnumTab.OP_LT);
                sincroniza.Add(EnumTab.OP_LE);
                sincroniza.Add(EnumTab.OP_NE);
                sincroniza.Add(EnumTab.SMB_CPA);

                sincronizaToken("[MODO PÂNICO] Esperado \"+ ou - ou OR ou ; ou == ou > ou >= ou < ou <= ou != ou )\", encontrado " + token.getLexema());
            }
        }
        #endregion

        //factor-a -> “id”, “num_const”, “char_const”, “(“, “not” |Fol| “*”, “/”, “and”, “+”, “-”, “or”, “;”, “==”, “>”, “>=”, “<”, “<=”, “!=”, “)”
        public void factoA()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.NUM_CONST || token.getClasse() == EnumTab.CHAR_CONST ||
                token.getClasse() == EnumTab.SMB_OPA || token.getClasse() == EnumTab.KW_NOT)
            {
                factor();
            }
            else if (token.getClasse() == EnumTab.KW_NOT)
            {
                if (!eat(EnumTab.KW_NOT))
                {
                    erroSintatico("Esperado \"NOT\", encontrado: " + token.getLexema());
                }
                factor();
            }
            else
            {
                erroSintatico("Esperado  \"ID ou NUM_CONST ou CHAR_CONST ou ( ou NOT\", encontrado: " + token.getLexema());
                //FOLLOW FACTORA()
                sincroniza.Add(EnumTab.OP_MUL);
                sincroniza.Add(EnumTab.OP_DIV);
                sincroniza.Add(EnumTab.KW_AND);
                sincroniza.Add(EnumTab.OP_AD);
                sincroniza.Add(EnumTab.OP_MIN);
                sincroniza.Add(EnumTab.KW_OR);
                sincroniza.Add(EnumTab.SMB_SEM);
                sincroniza.Add(EnumTab.OP_EQ);
                sincroniza.Add(EnumTab.OP_GT);
                sincroniza.Add(EnumTab.OP_GE);
                sincroniza.Add(EnumTab.OP_LT);
                sincroniza.Add(EnumTab.OP_LE);
                sincroniza.Add(EnumTab.OP_NE);
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado: \"* ou / ou AND ou + ou - ou OR ou ; ou == ou > ou >= ou < ou <= ou != ou )\", encontrado: " + token.getLexema());
            }
        }

        //factor -> “id”, “num_const”, “char_const”, “(“ |Fol| “*”, “/”, “and”, “+”, “-”, “or”, “;”, “==”, “>”, “>=”, “<”, “<=”, “!=”, “)”
        public void factor()
        {
            if (token.getClasse() == EnumTab.ID || token.getClasse() == EnumTab.NUM_CONST || token.getClasse() == EnumTab.CHAR_CONST ||
                token.getClasse() == EnumTab.SMB_CPA)
            {
                if (token.getClasse() == EnumTab.NUM_CONST || token.getClasse() == EnumTab.CHAR_CONST)
                {
                    constant();
                }
                else if (!eat(EnumTab.ID))
                {
                    erroSintatico("Esperado \"ID\", encontrado: " + token.getLexema());
                }
                else if (token.getClasse() == EnumTab.SMB_OPA)
                {
                    if (!eat(EnumTab.SMB_OPA))
                    {
                        erroSintatico("Esperado \"(\", encontrado: " + token.getLexema());
                    }
                    expression();
                    if (!eat(EnumTab.SMB_CPA))
                    {
                        erroSintatico("Esperado \")\" encontrado: " + token.getLexema());
                    }
                }
            }
            else
            {
                //FOLLOW FACTOR()
                sincroniza.Add(EnumTab.OP_MUL);
                sincroniza.Add(EnumTab.OP_DIV);
                sincroniza.Add(EnumTab.KW_AND);
                sincroniza.Add(EnumTab.OP_AD);
                sincroniza.Add(EnumTab.OP_MIN);
                sincroniza.Add(EnumTab.KW_OR);
                sincroniza.Add(EnumTab.SMB_SEM);
                sincroniza.Add(EnumTab.OP_EQ);
                sincroniza.Add(EnumTab.OP_GT);
                sincroniza.Add(EnumTab.OP_GE);
                sincroniza.Add(EnumTab.OP_LT);
                sincroniza.Add(EnumTab.OP_LE);
                sincroniza.Add(EnumTab.OP_NE);
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado: \"* ou / ou AND ou + ou - ou OR ou ; ou == ou > ou >= ou < ou <= ou != ou )\" encontrado: " + token.getLexema());
            }
        }

        //relop -> “==”, “>”, “>=”, “<”, “<=”, “!=” |Fol| “id”, “num_const”, “char_const”, “(“, “not”
        public void relop()
        {
            if (!eat(EnumTab.OP_EQ))
            {
                erroSintatico("Esperado \"==\" encontrado: " + token.getLexema());
            }
            if (!eat(EnumTab.OP_LT))
            {
                erroSintatico("Esperado \"<=\" encontrado: " + token.getLexema());
            }
            if (!eat(EnumTab.OP_LE))
            {
                erroSintatico("Esperado \"<=\" encontrado: " + token.getLexema());
            }

            if (!eat(EnumTab.OP_GT))
            {
                erroSintatico("Esperado \">=\" encontrado: " + token.getLexema());
            }

            if (!eat(EnumTab.OP_GE))
            {
                erroSintatico("Esperado \">=\" encontrado: " + token.getLexema());
            }

            if (!eat(EnumTab.OP_NE))
            {
                erroSintatico("Esperado \"!=\" encontrado: " + token.getLexema());
            }
            else
            {
                sincroniza.Add(EnumTab.ID);
                sincroniza.Add(EnumTab.NUM_CONST);
                sincroniza.Add(EnumTab.CHAR_CONST);
                sincroniza.Add(EnumTab.SMB_OPA);
                sincroniza.Add(EnumTab.KW_NOT);
                sincronizaToken("[MODO PÂNICO] Esperado \"ID ou NUM_CONST ou CHAR_CONST ou ( ou NOT\", encontrado: " + token.getLexema());
            }

        }

        //addop -> “+”, “-”, “or” |Fol| “id”, “num_const”, “char_const”, “(“, “not”
        public void addop()
        {
            if (!eat(EnumTab.OP_AD))
            {
                erroSintatico("Esperado \"+\" encontrado " + token.getLexema());
            }
            if (!eat(EnumTab.OP_MIN))
            {
                erroSintatico("Esperado \"-\" encontrado " + token.getLexema());
            }
            if (!eat(EnumTab.KW_OR))
            {
                erroSintatico("Esperado \"OR\" encontrado " + token.getLexema());
            }
            else
            {
                //FOLLOW ADDOP()
                sincroniza.Add(EnumTab.ID);
                sincroniza.Add(EnumTab.NUM_CONST);
                sincroniza.Add(EnumTab.CHAR_CONST);
                sincroniza.Add(EnumTab.SMB_OPA);
                sincroniza.Add(EnumTab.KW_NOT);
                sincronizaToken("[MODO PÂNICO] Esperado \"ID ou NUM_CONST ou CHAR_CONST ou ( ou NOT\", encontrado " + token.getLexema());
            }
        }

        //mulop “*”, “/”, “and” |Fol| “id”, “num_const”, “char_const”, “(“, “not”
        public void mulop()
        {
            if (!eat(EnumTab.OP_MUL))
            {
                erroSintatico("Esperado \"*\" encontrado: " + token.getLexema());
            }
            if (!eat(EnumTab.OP_DIV))
            {
                erroSintatico("Esperado \"-\" encontrado: " + token.getLexema());

            }
            if (!eat(EnumTab.KW_AND))
            {
                erroSintatico("Esperado \"OR\" encontrado: " + token.getLexema());
            }
            else
            {
                erroSintatico("Esperado \"* ou - ou OR\", encontrado: " + token.getLexema());
                //FOLLOW MULOP()
                sincroniza.Add(EnumTab.ID);
                sincroniza.Add(EnumTab.NUM_CONST);
                sincroniza.Add(EnumTab.CHAR_CONST);
                sincroniza.Add(EnumTab.SMB_OPA);
                sincroniza.Add(EnumTab.KW_NOT);
                sincronizaToken("[MODO PÂNICO] Esperado \"ID ou NUM_CONST ou CHAR_CONST ou ( ou NOT\", encontrado: " + token.getLexema());
            }
        }

        //constant -> “num_const”, “char_const” |Fol| “*”, “/”, “and”, “+”, “-”, “or”, “;”, “==”, “>”, “>=”, “<”, “<=”, “! =”, “)”
        public void constant()
        {
            if (!eat(EnumTab.NUM_CONST))
            {
                erroSintatico("Esperado \"NUM_CONST\" encontrado: " + token.getLexema());
            }

            if (!eat(EnumTab.CHAR_CONST))
            {
                erroSintatico("Esperado \"CHAR_CONST\" encontrado: " + token.getLexema());
            }
            else
            {
                erroSintatico("Esperado \"NUM_CONST ou CHAR_CONST\", encontrado: " + token.getLexema());
                //FOLLOW CONSTANT()
                sincroniza.Add(EnumTab.OP_MUL);
                sincroniza.Add(EnumTab.OP_DIV);
                sincroniza.Add(EnumTab.KW_AND);
                sincroniza.Add(EnumTab.OP_AD);
                sincroniza.Add(EnumTab.OP_MIN);
                sincroniza.Add(EnumTab.KW_OR);
                sincroniza.Add(EnumTab.SMB_SEM);
                sincroniza.Add(EnumTab.OP_EQ);
                sincroniza.Add(EnumTab.OP_GT);
                sincroniza.Add(EnumTab.OP_GE);
                sincroniza.Add(EnumTab.OP_LT);
                sincroniza.Add(EnumTab.OP_LE);
                sincroniza.Add(EnumTab.OP_NE);
                sincroniza.Add(EnumTab.SMB_CPA);
                sincronizaToken("[MODO PÂNICO] Esperado \"* ou / ou AND ou + ou - ou OR ou ; ou == ou > ou >= ou < ou <= ou != ou ),\" encontrado: " + token.getLexema());

            }
        }
        #endregion
    }
}
