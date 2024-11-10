using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class NeuralNetwork
{
    public int numInputs;
    public int numHidden;
    public int numOutputs;

    private float[] inputs;
    public float[,] weightsInputHidden;
    public float[,] weightsHiddenOutput;
    private float[] hiddenLayer;
    private float[] outputs;

    public NeuralNetwork(int numInputs, int numHidden, int numOutputs, float?[,] _weightsInputHidden, float?[,] _weightsHiddenOutput)
    {
        this.numInputs = numInputs;
        this.numHidden = numHidden;
        this.numOutputs = numOutputs;

        inputs = new float[numInputs];
        weightsInputHidden = new float[numInputs, numHidden];
        weightsHiddenOutput = new float[numHidden, numOutputs];
        hiddenLayer = new float[numHidden];
        outputs = new float[numOutputs];

        if (_weightsInputHidden != null)
        {
            weightsInputHidden = new float[_weightsInputHidden.GetLength(0), _weightsInputHidden.GetLength(1)];
            for (int i = 0; i < _weightsInputHidden.GetLength(0); i++)
            {
                for (int j = 0; j < _weightsInputHidden.GetLength(1); j++)
                {
                    weightsInputHidden[i, j] = _weightsInputHidden[i, j] ?? 0f; // Utilise 0f si la valeur est null
                }
            }
        }

        if (_weightsHiddenOutput != null)
        {
            weightsHiddenOutput = new float[_weightsHiddenOutput.GetLength(0), _weightsHiddenOutput.GetLength(1)];
            for (int i = 0; i < _weightsHiddenOutput.GetLength(0); i++)
            {
                for (int j = 0; j < _weightsHiddenOutput.GetLength(1); j++)
                {
                    weightsHiddenOutput[i, j] = _weightsHiddenOutput[i, j] ?? 0f; // Utilise 0f si la valeur est null
                }
            }
        }

        if (_weightsInputHidden == null && _weightsHiddenOutput == null)
        {
            InitializeWeights();
        }        
    }

    void InitializeWeights()
    {
        // Initialise les poids avec des valeurs aléatoires
        System.Random rand = new System.Random();
        for (int i = 0; i < numInputs; i++)
        {
            for (int j = 0; j < numHidden; j++)
            {
                weightsInputHidden[i, j] = (float)(rand.NextDouble() * 2 - 1); // entre -1 et 1
            }
        }

        for (int i = 0; i < numHidden; i++)
        {
            for (int j = 0; j < numOutputs; j++)
            {
                weightsHiddenOutput[i, j] = (float)(rand.NextDouble() * 2 - 1); // entre -1 et 1
            }
        }
    }

    public float[] FeedForward(float[] inputValues)
    {
        inputs = inputValues;

        // Calcul de la couche cachée
        for (int i = 0; i < numHidden; i++)
        {
            float sum = 0f;
            for (int j = 0; j < numInputs; j++)
            {
                sum += inputs[j] * weightsInputHidden[j, i];
            }
            hiddenLayer[i] = Sigmoid(sum);
        }

        // Calcul de la sortie
        for (int i = 0; i < numOutputs; i++)
        {
            float sum = 0f;
            for (int j = 0; j < numHidden; j++)
            {
                sum += hiddenLayer[j] * weightsHiddenOutput[j, i];
            }
            outputs[i] = Sigmoid(sum);
        }

        return outputs;
    }

    float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }
}
