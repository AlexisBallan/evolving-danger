using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public List<NeuralNetwork> population;
    public int populationSize;
    public float mutationRate;

    public GeneticAlgorithm(int populationSize, int numInputs, int numHidden, int numOutputs, float mutationRate)
    {
        this.populationSize = populationSize;
        this.mutationRate = mutationRate;
        population = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            population.Add(new NeuralNetwork(numInputs, numHidden, numOutputs));
        }
    }

    public void Evolve(List<float> fitnessScores)
    {
        List<NeuralNetwork> newPopulation = new List<NeuralNetwork>();

        // Sélection des parents en fonction des scores de fitness
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork parent1 = SelectParent(fitnessScores);
            NeuralNetwork parent2 = SelectParent(fitnessScores);

            NeuralNetwork offspring = Crossover(parent1, parent2);
            Mutate(offspring);

            newPopulation.Add(offspring);
        }

        population = newPopulation;
    }

    NeuralNetwork SelectParent(List<float> fitnessScores)
    {
        // Sélection basée sur la roulette ou une autre stratégie
        int index = Random.Range(0, populationSize);
        return population[index];
    }

    NeuralNetwork Crossover(NeuralNetwork parent1, NeuralNetwork parent2)
    {
        // Fusion des poids des parents pour créer un nouvel enfant
        NeuralNetwork child = new NeuralNetwork(parent1.numInputs, parent1.numHidden, parent1.numOutputs);
        for (int i = 0; i < parent1.numInputs; i++)
        {
            for (int j = 0; j < parent1.numHidden; j++)
            {
                child.weightsInputHidden[i, j] = (Random.value > 0.5f) ? parent1.weightsInputHidden[i, j] : parent2.weightsInputHidden[i, j];
            }
        }
        for (int i = 0; i < parent1.numHidden; i++)
        {
            for (int j = 0; j < parent1.numOutputs; j++)
            {
                child.weightsHiddenOutput[i, j] = (Random.value > 0.5f) ? parent1.weightsHiddenOutput[i, j] : parent2.weightsHiddenOutput[i, j];
            }
        }
        return child;
    }

    void Mutate(NeuralNetwork network)
    {
        // Mutation des poids avec un certain taux
        for (int i = 0; i < network.numInputs; i++)
        {
            for (int j = 0; j < network.numHidden; j++)
            {
                if (Random.value < mutationRate)
                {
                    network.weightsInputHidden[i, j] += Random.Range(-0.5f, 0.5f);
                }
            }
        }

        for (int i = 0; i < network.numHidden; i++)
        {
            for (int j = 0; j < network.numOutputs; j++)
            {
                if (Random.value < mutationRate)
                {
                    network.weightsHiddenOutput[i, j] += Random.Range(-0.5f, 0.5f);
                }
            }
        }
    }
}
