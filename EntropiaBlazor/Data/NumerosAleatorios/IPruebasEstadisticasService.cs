namespace GeneradorNumerosAleatorios.BLazorMAUI.Services;

internal interface IPruebasEstadisticasService
{
    bool Promedios(List<double> numeros, double zc);
    bool Frecuencia(List<double> numeros, int cantidadDeIntervalos, double chiCuadrado);
    bool Serie(List<double> muestra, int x, double estadistico);
    bool Ks(List<double> muestra, double estadistico);
    bool CorridaArribaYCorridaMedia(List<double> muestras, double estadistico);
}
