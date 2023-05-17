namespace NumerosAleatoriosIO.Services;

internal interface IGeneradorService
{
    List<double> ParteCentralDelCuadrado(int digitosDeseados, int semilla, int totalDeNumerosaGenerar);
    List<double> Lehmer(int semilla, int constanteMultiplicativa, int totalDeNumerosaGenerar);
    List<double> CongruencialMixto(int semilla, int constanteAditiva, int constanteMultiplicativa,
       int modulo, int digitos, int totalDeNumerosaGenerar);
    List<double> CongruencialMultiplicativo(int semilla, int constanteMultiplicativa, int modulo, int totalDeNumerosaGenerar, int digitos);
    List<double> CongruencialAditivo(List<int> listaSemilla, int modulo, int totalDeNumerosaGenerar, int digitos);
}
