namespace GeneradorNumerosAleatorios.BLazorMAUI.Services;

internal class PruebasEstadisticasService : IPruebasEstadisticasService
{
    public bool Promedios(List<double> numeros, double zc)
    {
        zc = zc > 0 ? double.Parse($"0.{zc}") : zc;
        double promedio = numeros.Sum(x => x) / numeros.Count;
        double estadistico = (promedio - 0.5) * Math.Sqrt(numeros.Count)
            / Math.Sqrt(1 / 12);
        if (Math.Abs(estadistico) < zc)
        {
            return true;
        }
        return false;
    }

    public bool Frecuencia(List<double> numeros, int cantidadDeIntervalos, double chiCuadrado)
    {
        var limite = 1f / cantidadDeIntervalos;
        var limiteRecorrido = limite;
        var frecuenciaEsperada = numeros.Count / cantidadDeIntervalos;
        List<int> frecuenciasObservadas = new List<int>(new int[cantidadDeIntervalos]); // Inicializar la lista con ceros

        int i = 0;
        while (limiteRecorrido < 1)
        {
            foreach (var numero in numeros.ToArray()) // Usamos ToArray para evitar una excepción de modificación concurrente
            {
                if (numero <= limiteRecorrido)
                {
                    numeros.Remove(numero);
                    frecuenciasObservadas[i]++;
                }
            }
            limiteRecorrido += limite;
            i++;
        }

        // Realizar el cálculo del estadístico de chi-cuadrado
        double chiCuadradoCalculado = 0.0;
        foreach (var frecuenciaObservada in frecuenciasObservadas)
        {
            double diferencia = frecuenciaObservada - frecuenciaEsperada;
            chiCuadradoCalculado += (diferencia * diferencia) / frecuenciaEsperada;
        }

        // Comparar el chi-cuadrado calculado con el valor crítico para determinar si pasa la prueba
        return chiCuadradoCalculado <= chiCuadrado;
    }


    public bool Serie(List<double> muestra, int x, double estadistico)
    {
        if (muestra.Count % 2 != 0) throw new Exception("La muestra no tiene longitud par.");

        // Emparejamiento
        List<Tuple<double, double>> pares = new List<Tuple<double, double>>();
        for (int i = 0; i < muestra.Count; i += 2)
        {
            pares.Add(Tuple.Create(muestra[i], muestra[i + 1]));
        }

        double intervalo = 1.0 / x;
        short[,] cuadrado = new short[x, x];
        double fe = (double)pares.Count / (x * x);

        foreach (Tuple<double, double> p in pares)
        {
            int fila = 0, columna = 0;
            short numeroCelda = 0;
            // No rompe el ciclo al encontrar la solucion, seria lento
            // Posible de mejorar
            for (double i = 0; i < 1; i += intervalo)
            {
                if (i <= p.Item1 && p.Item1 < i + intervalo)
                    fila = numeroCelda;
                if (i <= p.Item2 && p.Item2 < i + intervalo)
                    columna = numeroCelda;
                numeroCelda++;
            }
            cuadrado[fila, columna]++;
        }

        double sumatoria = 0;
        foreach (double celda in cuadrado)
        {
            double resta = celda - fe;
            sumatoria += (resta * resta);
        }

        double chiCuadrado = (x * x / (double)pares.Count) * sumatoria;

        return chiCuadrado < estadistico;
    }

    public bool Ks(List<double> muestra, double estadistico)
    {
        muestra.Sort();
        // Calculo la distibucion y la diferencia de una sola vez:
        double[] diferencias = new double[muestra.Count];
        for (int i = 0; i < diferencias.Length; i++)
        {
            double distribucion = (double)(i + 1) / muestra.Count;
            diferencias[i] = distribucion - muestra[i];
        }
        double Dn = diferencias.Max();
        return Dn < estadistico;
    }

    public bool CorridaArribaYCorridaMedia(List<double> muestras, double estadistico)
    {
        List<string> muestrasBinarias = muestras.Select(muestra => muestra <= 0.5 ? "0" : "1").ToList();
        string cadenaBinaria = string.Join("", muestrasBinarias);

        List<int> intervalos = new List<int>();
        int contadorUnos = 0;
        int contadorCeros = 0;
        for (int i = 0; i < cadenaBinaria.Length; i++)
        {
            if (cadenaBinaria[i] == '1')
            {
                contadorUnos++;
                if (i == cadenaBinaria.Length - 1 || cadenaBinaria[i] != cadenaBinaria[i + 1])
                {
                    intervalos.Add(contadorUnos);
                    contadorUnos = 0;
                }
            }
            else
            {
                contadorCeros++;
                if (i == cadenaBinaria.Length - 1 || cadenaBinaria[i] != cadenaBinaria[i + 1])
                {
                    intervalos.Add(contadorCeros);
                    contadorCeros = 0;
                }
            }
        }

        //Cálculo de Fo
        List<int> fo = new List<int>();
        int total = 0;
        for (int i = 1; i <= intervalos.Max(); i++)
        {
            total = intervalos.Count(x => x == i);
            fo.Add(total);
        }

        //Cálculo de Fe
        int n = muestras.Count;
        List<float> fe = new List<float>();
        for (int i = 1; i <= intervalos.Max(); i++)
        {
            float feSubI = (n - i + 3) / (float)Math.Pow(2, i + 1);
            fe.Add(feSubI);
        }

        //Cálculo de Chi
        float chiCuadrado = 0;
        for (int i = 0; i < fo.Count(); i++)
        {
            chiCuadrado += (fo[i] - fe[i]) * (fo[i] - fe[i]) / fe[i];
        }

        return chiCuadrado < estadistico;
    }

}

