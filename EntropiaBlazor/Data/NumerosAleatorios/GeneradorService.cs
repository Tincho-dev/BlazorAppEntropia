using System.Reflection.Metadata.Ecma335;

namespace NumerosAleatoriosIO.Services;
public class GeneradorService : IGeneradorService
{

    #region Metodos de Generacion
    public List<double> ParteCentralDelCuadrado(int digitosDeseados, int semilla, int totalDeNumerosaGenerar)
    {
        List<double> u = new List<double>();
        for (int i = 1; i <= totalDeNumerosaGenerar; i++)
        {
            int x = semilla * semilla;
            if (semilla.ToString().Length > digitosDeseados) continue;
            semilla = ((x.ToString().Length - digitosDeseados) % 2 == 0)
                        ? NumerosCentrales(digitosDeseados, x)
                        : NumerosCentrales(digitosDeseados, x * 10);
            u.Add(double.Parse("0," + semilla));
        }
        return u;
    }

    public List<double> Lehmer(int semilla, int constanteMultiplicativa, int totalDeNumerosaGenerar)
    {
        List<double> u = new List<double>();
        var kDigitos = constanteMultiplicativa.ToString().Length;
        for (int i = 1; i <= totalDeNumerosaGenerar; i++)
        {
            //if(semilla<2) continue;

            var n = semilla * constanteMultiplicativa;
            var new_t = n.ToString().Substring(0, kDigitos);
            var corte = 
                n.ToString().Substring(kDigitos).Length > 0 
                ? n.ToString().Substring(kDigitos) 
                : 0.ToString();
            semilla = Math.Abs(Int32.Parse(corte) - Int32.Parse(new_t));
            if (semilla > 0) u.Add(double.Parse("0," + semilla));
        }
        return u;
    }

    public List<double> CongruencialMixto(int semilla, int constanteAditiva, int constanteMultiplicativa,
        int modulo, int digitos, int totalDeNumerosaGenerar)
    {
        List<double> u = new List<double>();

        for (int i = 1; i <= totalDeNumerosaGenerar; i++)
        {
            semilla = (constanteMultiplicativa * semilla + constanteAditiva) % modulo;
            u.Add(
                Math.Round(
                    value: (double)semilla / modulo,
                    digits: digitos,
                    mode: MidpointRounding.ToZero));//trunca
        }
        return u;
    }

    public List<double> CongruencialMultiplicativo(int semilla, int constanteMultiplicativa, int modulo, int totalDeNumerosaGenerar, int digitos)
    {
        List<double> u = new List<double>();
        for (int i = 1; i <= totalDeNumerosaGenerar; i++)
        {
            semilla = (semilla * constanteMultiplicativa) % modulo;
            u.Add(
                Math.Round(
                    value: (double)semilla / modulo,
                    digits: digitos,
                    mode: MidpointRounding.ToZero));
        }
        return u;
    }

    public List<double> CongruencialAditivo(List<int> listaSemilla, int modulo, int totalDeNumerosaGenerar, int digitos)
    {
        List<double> u = new List<double>();
        int techo = listaSemilla.Count - 1;
        int semilla;
        int piso = 0;
        for (int i = 1; i <= totalDeNumerosaGenerar; i++)
        {
            semilla = (listaSemilla[piso] + listaSemilla[techo]) % modulo;
            techo++;
            piso++;
            listaSemilla.Add(semilla);
            u.Add(Math.Round(
                value: (double)semilla / modulo,
                digits: digitos,
                mode: MidpointRounding.ToZero));
        }
        return u;
    }
    #endregion

    #region Funciones complementarias

    private int NumerosCentrales(int numeroDeDigitosDeseados, int cuadradoDeLaSemilla)
    {
        string xstr = cuadradoDeLaSemilla.ToString();
        int digitosDescartados = Math.Abs((xstr.Length - numeroDeDigitosDeseados) / 2);
        xstr = xstr.Remove(0, digitosDescartados);
        xstr = xstr.Remove(xstr.Length - digitosDescartados, digitosDescartados);
        int numerosCentrales = Int32.Parse(xstr);
        return numerosCentrales;
    }
    #endregion
}

