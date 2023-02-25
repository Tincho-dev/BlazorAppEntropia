using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Fuente
    {
        [Key]
        //Clave primaria para la base de datos
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IdFuente { get; set; }

        //Coleccion de letras, es decir los simbolos que puede generar
        [Required]
        public List<Letra> Letras { get; set; } = new List<Letra>();

        //Cadena usada para generar la fuente, de alli se obtienen las letras que genera la fuente y sus probabilidades
        [Display(Name = "Cadena usada para generar la fuente: ")]
        public string CadenaFuente { get; set; } = "";

        //Constructor vacio para el Framework
        public Fuente()
        {
            IdFuente = Guid.NewGuid().ToString();
        }

        //Constructor que en base a ala cadena que genera calcula todos los datos
        public Fuente(string cadena)
        {
            //Validamos que no sea nula la cadena, si lo fuera le establecemos un string vacio
            if (cadena == null) CadenaFuente = "";
            CadenaFuente = cadena;
            //Obetnemos todas las letras (simbolos) que puede generar usando la Funcion StringToListLetra()
            Letras = StringToListLetra();
            //Establecemos el codigo en base a la probabilidad a cada letra
            EstablecerCodigoHuffmanACadaLetra();
        }
        //Entropia de la fuente que es menor a la Entropia Maxima
        public float EntropiaDeLaFuente()
        {
            //usamos la formula de que la entropia es
            //La suma ponderada de la informacion con la probabilidad
            float suma = 0;
            foreach (Letra letra in Letras)
            {
                //informacion individual de cada simbolo ponderada por su probabilidad de ocurrecnia
                suma += letra.Probability * (float)(Math.Log(1 / letra.Probability) / Math.Log(2));
            }
            //ddevolvemos la suma
            return suma;
        }

        //Entropia maxima si todas las letras fuerna igual de probables
        public float EntropiaMaxima()
        {
            //Entropia maxima = Log2 Nç
            //en esta version del lenguaje no hay una funcion que calcule el logaritmo en base 2
            //por lo que usamos las propiedades del log
            return (float) (Math.Log10(Letras.Count)/Math.Log10(2));
        }

        public float InformacionDeCadena()
        {
            //Inicializamos la suma en 0
            //en esta variable sumaremos la informacion de cada letra
            float suma = 0;
            //Recorremos letra por letra
            foreach (Letra letra in Letras)
            {
                //Sumamos la informacion individual de cada simbolo
                //Castemaos al tipo float ya que la division por defecto da un tipo double
                suma += (float) (Math.Log(1 / letra.Probability)/Math.Log(2));
            }
            //Devolvemos la Suma 
            return suma;
        }

        public List<Letra> StringToListLetra()
        {
            //crea una lista para guardar las letras de la fuente, es la que devolveremos
            List<Letra> l = new List<Letra>();
            if (CadenaFuente != null)
            {

                //crea una cadena de caracteres que representa cada letra
                char[] caracteres = CadenaFuente.ToCharArray();
                //obtenemos el total de caracteres
                int total = caracteres.Length;

                //guardamos los daots en una variable sin elementos repetidos
                char[] letrasDistintas = caracteres.Distinct().ToArray();

                // creamos una clase letra por cada letra en la cadena y la guardamos en la lista
                //Recorremos 
                foreach (char item in letrasDistintas)
                {
                    //Obtenemos la cantidad e veces que aparece este item en la cadena orifinal
                    int freq = caracteres.Count(f => (f == item));
                    //Calculamos la probabilidad de aparicion de dicho item (simbolo)
                    double probabilidad = (double)freq / total;
                    //Creamos la nueva letra y
                    //le pasamos los datos del simbolo, su probabilidad
                    //y la cantidad ed veces que aparece en la cadena original
                    Letra letraAgregar = new Letra(item.ToString(), probabilidad, freq);
                    //Le creamos un id pseudo aleatorio para la base de datos
                    letraAgregar.Id = letraAgregar.GetHashCode().ToString();
                    //Agregamos la nueva letra a la lista que devolveremos
                    l.Add(letraAgregar);
                    //Repetimos hasta que se acaben las letras
                }

                //devolvemos la lista de letras con sus datos cargados
            }
                return l;
        }


        //Cadena codificada, generada al cambiar cada letra por su codigo
        //La cadena generada es la que se codifica para ser transmitida
        public string CodificarCadena()
        {
            //Creamos un string vacio
            string codigoFinal = "";

            //Recorremos cada simbolo(o char) de la cadena 
            foreach (var simbolo in CadenaFuente)
            {
                //Usando Linq realizamos consultas a colecciones, similar a SQL

                var codigo = (from l in Letras //de cada elemento de la coleccion Letras
                              //Donde el nombre sea igual al simbolo de la iteracion
                              where (l.Name == simbolo.ToString())
                              //Seleccione el codigo
                              select l.Codigo).Single();//.Single() dedvuelve un solo elemento en vez de una coleccion
                //Le agregamos el Codigo a la cadena que devolveremos
                codigoFinal += codigo.ToString();
            }
            //Devolvemos la cadena con todos los codigos en orden
            return codigoFinal;
        }

        public void OrdenarLetrasSegunProbabilidad()
        {
            //Ordena las letras en orden descendiente segun la probabilidad
            Letras = Letras.OrderByDescending(l => l.Probability).ToList();
        }

        public void EstablecerCodigoACadaLetra()
        {
            //Ordena las letras en orden descendiente segun la probabilidad
            OrdenarLetrasSegunProbabilidad();
            //convertimos la lista a un arreglo para tener acceso al numero de la posicion de cada elemento
            var LetrasArray = Letras.ToArray();

            for (int i = 0; i < LetrasArray.Length; i++)
            {
                //convertimos a un texto el numero i en base 2; Ej el numero 6 seria "110"
                LetrasArray[i].Codigo = Convert.ToString(i, 2);
            }
            //devolvemos el valor a la lista con los nuevos valores de los codigos y los convertimos en una lsita
            Letras = LetrasArray.ToList();
        }
        public void EstablecerCodigoHuffmanACadaLetra()
        {
            HuffmanTree huffmanTree = new HuffmanTree();
            huffmanTree.Build(CadenaFuente);
            Letras = huffmanTree.Encode(Letras);
        }

        public string DecodificarHuffman()
        {
            //Usamos el codigo de Huffman para crear el arbol
            HuffmanTree huffmanTree = new HuffmanTree();
            huffmanTree.Build(CadenaFuente);
            //Hacemos un cambio de tipo para decodificar con el tipo de dato que necesita Huffman
            var CadenaCodificada = CodificarCadena();
            var res = new BitArray(CadenaCodificada.Select(c => c == '1').ToArray());
            BitArray bits = new BitArray(res);

            return huffmanTree.Decode(res);
        }
        public string DecodificarHuffman(string cadena)
        {
            //Usamos el codigo de Huffman para crear el arbol
            HuffmanTree huffmanTree = new HuffmanTree();
            huffmanTree.Build(CadenaFuente);
            //Hacemos un cambio de tipo para decodificar con el tipo de dato que necesita Huffman
            var res = new BitArray(cadena.Select(c => c == '1').ToArray());
            BitArray bits = new BitArray(res);

            return huffmanTree.Decode(res);
        }
    }
}