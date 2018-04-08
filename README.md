# Trabalho Prático de Compiladores

Esse repositorio, se trata do desenvolvimento de um compilador na linguagem "C#"

Um compilador é um programa que a partir de um código fonte escrito em uma linguagem compilada, é criado um programa semanticamente equivalente, porém escrito em outra linguagem. De forma geral, um compilador traduz um programa de uma linguagemm que uma pessoa seja capaz de entender.

## 1º Parte 
### Analise Léxica:

É um processo de analise de entrada de linhas de caracteres (código fonte) que produz uma sequência de sómbolos (símbolos léxicos), que podem ser manipulados mais facilmente por um parser (leitor de saída).
A Análise léxica verifica o alfabeto a partir de uma tabela de símbolos, se existe ou não algum caracter que faz parte dessa tabela.

1. A entrada é lida, uma de cada vez, mudando o estado em que os caracteres se encontram. Quando o analisador encontra um caracter que ele não identifica como correto, ele chama o "estado morto" logo, volta a última analise aceita e tem o tipo de comprimento do léxico válido.

2. Os caracteres são repassados do léxico para produzir um valor, o tipo do léxico combinado com seu valor é o adequado, o que pode ser dado um parser.

Para mais explicações [Análise Léxica](https://pt.wikipedia.org/wiki/An%C3%A1lise_l%C3%A9xica).
