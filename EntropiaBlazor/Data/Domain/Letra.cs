using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Letra
    {
        //clave primaria para la base de datos
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        //Nombre de la letra, representa un caracter, letra, numero o simbolo que genera la fuente
        [MaxLength(1)]
        public string Name { get; set; }
        //Probabilidad de aparicion de la letra en la fuente
        [Required]
        public float Probability { get; set; }
        //Informacion calculada en base a la probabilidad como Shanon lo establece
        public float Information { get; set; }
        //Codigo que depende de la probabilidad, asignando uno mas corto para los simbolos mas frecuentes y mas largos para los menos frecuentes
        public string Codigo { get; set; } = string.Empty; //el codigo es vacio, la fuente es quien luego lo establece
        //Cantidad de veces que aparece en la cadena que genera la Fuente
        public int FrecuenciaDeAparicion { get; set; }

        //Relacion con la Fuente, necesaria para el Framework y coneccion a Base de Datos
        public string IdFuente { get; set; }
        [ForeignKey("IdFuente")]
        public virtual Fuente Fuente { get; set; }

        //Constructor vacio, necesario para el Framework
        public Letra()
        {
            Id = Guid.NewGuid().ToString();
        }

        //Constructor que recibe los datosnecesarios para crear un objeto de la clase Letra
        //recibimos el nombre y la probabilidad al crear la letra, la informacion se calcula en funcoin de la probabilidad
        public Letra(string name, double probability, int freq)
        {
            //Simbolo de la fuente
            Name = name;
            //casteamos de tipo float
            Probability = (float) probability;
            //calculamos la informacion segun la ecuacion que depende de la probabilidad
            // I = Log2(1/Pa)
            Information = (float) (Math.Log(1 / probability)/Math.Log(2));
            FrecuenciaDeAparicion = freq;
        }
    }
}