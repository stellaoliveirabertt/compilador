# Trabalho Prático de Compiladores

Um compilador é um programa que a partir de um código fonte escrito em uma linguagem compilada, é criado um programa semanticamente equivalente, porém escrito em outra linguagem. De forma geral, um compilador traduz um programa de uma linguagemm que uma pessoa seja capaz de entender.

## 1º Parte 
### Analise Léxica:

É um processo de analise de entrada de linhas de caracteres (código fonte) que produz uma sequência de sómbolos (símbolos léxicos), que podem ser manipulados mais facilmente por um parser (leitor de saída).
A Análise léxica verifica o alfabeto a partir de uma tabela de símbolos, se existe ou não algum caracter que faz parte dessa tabela.

1. A entrada é lida, uma de cada vez, mudando o estado em que os caracteres se encontram. Quando o analisador encontra um caracter que ele não identifica como correto, ele chama o "estado morto" logo, volta a última analise aceita e tem o tipo de comprimento do léxico válido.

2. Os caracteres são repassados do léxico para produzir um valor, o tipo do léxico combinado com seu valor é o adequado, o que pode ser dado um parser.

Para mais explicações [Análise Léxica](https://pt.wikipedia.org/wiki/An%C3%A1lise_l%C3%A9xica).


## 2º Parte
### Analisador Sintático (Neste trabalho foi implementado o Top-Down):

É o processo de se determinar se uma cadeia de simbolos léxicos pode ser gerada por uma gramática. Ela transforma um texto na entrada em uma estrutura de dados, em geral uma árvore, o que é conveniente para o processamento posterior e captura a hierarquia implicita desta entrada. Através da análise léxica é obtido um grupo de tokens, para que o analisador sintático use um conjunto de regras para constuir uma árvore sintática da estrutura.

A tarefa do analisador léxico é deerminar se uma entrada de dados pode ser derivada de um símbolo inicial com as regras de uma gramática formal. Essa tarefa pode ser feita de duas formas:

1. Descendente (top-down), onde o analisador pode iniciar com um simbolo inciial e tentar transfomá-lo na entrada de dados. De maneira intuitiva o analisador inicia dos maiores elementos e os quebra em elementos menores. 
2. Ascendente (bottom-up), onde o analisador pode iniciar com uma entrada de dados e tentar reescrevê-la até o simbolo inicial. Intuitivamente, o analisador tenta localizar os elementos mais básicos, e então elementos maiores que contêm os elementos mais básicos e assim por diante.

Para mais explicações [Análise Sintática](https://pt.wikibooks.org/wiki/Constru%C3%A7%C3%A3o_de_compiladores/An%C3%A1lise_sint%C3%A1tica).
