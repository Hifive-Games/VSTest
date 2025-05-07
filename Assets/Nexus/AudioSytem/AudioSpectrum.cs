using System;
using UnityEngine;
using TMPro;
public class AudioSpectrum : MonoBehaviour
{
    public static AudioSpectrum Instance { get; private set; }

    public SoundCapture capture;
    public static event Action<int> OnBandTrigger;
    public Color[] bandColors = new Color[8];

    #region Constants
    const int SAMPLES = 1024;
    const int BANDS = 8;
    #endregion
    #region Spectrums
    private float[]
    spectrum,
    samplesL,
    samplesR;
    #endregion
    #region Bands
    private float[]
    band,
    bandBuffer,
    bufferDecrease,
    maxValues;
    #endregion
    #region Thresholds
    private float[,]
    maxThresholds,
    maxAmplitude,
    minThresholds;
    #endregion

    private bool[] isBandTrigger;

    ExponentialMovingAverage[] ema;

    #region Test
    public TMP_Text _text;
    public float _maxAmplitude = 1f;

    public bool _isAmplitudeTrigger = false;

    public Transform _spawnPoint;

    public float[] floats = new float[8];

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Application.targetFrameRate = 60;

        Initialization();
    }

    void Update()
    {
        GetFFT();

        for (int i = 0; i < band.Length; i++) CheckTrigger(i);

        //Test:
        //check if the amplitude is greater than the max amplitude
        float amplitude = GetAmplitude();

        /*_text.text = "Amplitude: " + amplitude.ToString("F4") +
                    "\nMaxAmplitude: " + _maxAmplitude.ToString("F4");*/


        if (amplitude > _maxAmplitude)
        {
            _maxAmplitude = amplitude;
            _isAmplitudeTrigger = true;
        }
        else
        {
            _maxAmplitude -= _maxAmplitude * 0.005f;
            if (_maxAmplitude < .1f) _maxAmplitude = .1f;
            _isAmplitudeTrigger = false;
        }
    }

    public bool AmplitutdeTrigger(float threshold)
    {
        float amplitude = GetAmplitude();
        if (amplitude > _maxAmplitude * threshold) return true;
        else                                       return false;
    }

    private void Initialization()
    {

        spectrum = new float[SAMPLES];
        samplesL = new float[SAMPLES];
        samplesR = new float[SAMPLES];

        band = new float[BANDS];
        bandBuffer = new float[BANDS];
        bufferDecrease = new float[BANDS];
        maxValues = new float[BANDS];
        maxThresholds = new float[BANDS, 10];
        minThresholds = new float[BANDS, 10];
        maxAmplitude = new float[BANDS, 10];
        isBandTrigger = new bool[BANDS];
        ema = new ExponentialMovingAverage[BANDS];


        for (int i = 0; i < BANDS; i++)
        {
            maxValues[i] = 0;
            maxThresholds[i, 0] = 0.999f;
            minThresholds[i, 0] = 0.001f;
            isBandTrigger[i] = false;
            ema[i] = new ExponentialMovingAverage(1f);
            ema[i].SetAlpha(0.1f);
        }
    }

    private void CheckTrigger(int bandIndex)
    {

        float smoothBand = ema[bandIndex].Update(band[bandIndex]);

        float currentBand = smoothBand;

        float currentMaxThreshold = GetAverageMaxThreshold(bandIndex);
        float currentMinThreshold = GetAvaregeMinThreshold(bandIndex);

        if (currentBand > maxValues[bandIndex] * currentMaxThreshold &&
            currentBand > currentMinThreshold)
        {
            SetMaxThreshold(bandIndex, maxValues[bandIndex] / currentBand);


            ema[bandIndex].SetAlpha(GetAverageMaxThreshold(bandIndex));

            floats[bandIndex] = ema[bandIndex].GetAlpha();

            if (currentBand > maxValues[bandIndex])
            {
                maxValues[bandIndex] = currentBand;
                isBandTrigger[bandIndex] = true;
                OnBandTrigger?.Invoke(bandIndex);
            }

        }
        else
        {
            isBandTrigger[bandIndex] = false;
            maxValues[bandIndex] -= maxValues[bandIndex] * 0.005f;
        }

        if (maxValues[bandIndex] < currentMinThreshold)
            maxValues[bandIndex] = currentMinThreshold;
    }

    private int MostChangedBand()
    {
        float maxChange = 0;
        int maxChangeIndex = 0;

        for (int i = 0; i < band.Length; i++)
        {
            bandBuffer[i] = BandBuffer(i);
            float change = band[i] - bandBuffer[i];

            if (change > maxChange)
            {
                maxChange = change;
                maxChangeIndex = i;
            }
        }

        return maxChangeIndex;
    }

    private float GetAverageMaxThreshold(int bandIndex)
    {
        float averageThreshold = 0f;

        for (int i = 0; i < maxThresholds.GetLength(1); i++)
        {
            averageThreshold += maxThresholds[bandIndex, i];
        }

        averageThreshold /= maxThresholds.GetLength(1);

        return averageThreshold;
    }

    private float GetAvaregeMinThreshold(int bandIndex)
    {
        float averageThreshold = 0f;

        for (int i = 0; i < minThresholds.GetLength(1); i++)
        {
            averageThreshold += minThresholds[bandIndex, i];
        }

        averageThreshold /= minThresholds.GetLength(1);

        return averageThreshold;
    }

    private void SetMaxThreshold(int bandIndex, float threshold)
    {
        for (int i = 0; i < maxThresholds.GetLength(1) - 1; i++)
        {
            maxThresholds[bandIndex, i] = maxThresholds[bandIndex, i + 1];
        }

        maxThresholds[bandIndex, maxThresholds.GetLength(1) - 1] = threshold;
    }

    public float BandBuffer(int bandIndex)
    {
        float relativeDecrease = 0.005f * bandBuffer[bandIndex];
        float relativeIncrease = 1.2f * (bandBuffer[bandIndex] / maxValues[bandIndex]);

        if (band[bandIndex] > bandBuffer[bandIndex])
        {
            bandBuffer[bandIndex] = band[bandIndex];
            bufferDecrease[bandIndex] = Mathf.Max(0.0f, relativeDecrease);
        }
        else if (band[bandIndex] < bandBuffer[bandIndex])
        {
            bandBuffer[bandIndex] -= bufferDecrease[bandIndex];
            bufferDecrease[bandIndex] *= relativeIncrease;
        }

        return bandBuffer[bandIndex];

    }

    private float[] GetFrequencyBands()
    {
        int numBands = band.Length;

        for (int i = 0; i < numBands; i++)
        {
            int startSample = (int)Mathf.Pow(2, i) * 2 - 1;
            int endSample = (int)Mathf.Pow(2, i + 1) * 2 - 1;

            startSample = Mathf.Clamp(startSample, 0, spectrum.Length - 1);
            endSample = Mathf.Clamp(endSample, 0, spectrum.Length - 1);

            float average = 0;

            for (int j = startSample; j <= endSample; j++)
            {
                if (j >= 0 && j < spectrum.Length)
                {
                    average += spectrum[j];
                }
                else
                {
                    Debug.Log("Index out of range: " + j);
                }
            }

            average /= Mathf.Max(1, endSample - startSample + 1);

            band[i] = average;
        }

        // Apply Savitzky-Golay filter to the band array
        band = SavitzkyGolayFilter.Apply(band);

        return band;
    }

    private void GetFFT()
    {
        spectrum = capture.GetSpectrumData();

        band = GetFrequencyBands();
    }

    public void SetSpectrum(float[] samples)
    {
        samplesL = samples;
        samplesR = samples;

        band = GetFrequencyBands();
    }
    public void SetSpectrum(float[] samplesL, float[] samplesR)
    {
        this.samplesL = samplesL;
        this.samplesR = samplesR;

        band = GetFrequencyBands();
    }

    public float GetAmplitude()
    {
        float amplitude = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            amplitude += spectrum[i];
        }

        return amplitude;
    }

    public float GetMaxAmplitude()
    {
        return _maxAmplitude;
    }

    public float GetAmplitudeBuffer()
    {
        float amplitude = 0;
        for (int i = 0; i < bandBuffer.Length; i++)
        {
            amplitude += bandBuffer[i];
        }

        return amplitude;
    }
}

public class ExponentialMovingAverage
{
    private float alpha;
    private float ema;

    public ExponentialMovingAverage(float alpha)
    {
        // Ensure that alpha is between 0 and 1
        this.alpha = Mathf.Clamp(alpha, 0f, 1f);
        this.ema = 0f;
    }

    public float Update(float newValue)
    {
        // Update the EMA using the formula: EMA_t = (1 - alpha) * EMA_{t-1} + alpha * X_t
        ema = (1 - alpha) * ema + alpha * newValue;
        return ema;
    }

    public float GetCurrentEMA()
    {
        return ema;
    }

    public void SetAlpha(float alpha)
    {
        this.alpha = Mathf.Clamp(alpha, 0f, 1f);
    }

    public float GetAlpha()
    {
        return this.alpha;
    }
}

public static class SavitzkyGolayFilter
{
    private static readonly float[] coeffs = { -2, 3, 6, 7, 6, 3, -2 };

    public static float[] Apply(float[] input)
    {
        int size = input.Length;
        float[] output = new float[size];
        int half = coeffs.Length / 2;

        for (int i = 0; i < size; i++)
        {
            float result = 0;
            for (int j = -half; j <= half; j++)
            {
                int index = Mathf.Clamp(i + j, 0, size - 1);
                result += input[index] * coeffs[j + half];
            }
            output[i] = result / 21;
        }

        return output;
    }
}
