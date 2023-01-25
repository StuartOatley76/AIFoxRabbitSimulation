using System;

namespace Genetics {

    /// <summary>
    /// Generic class to represent a Gene
    /// </summary>
    /// <typeparam name="T">Type held by value. Must be value type</typeparam>
    public class Gene<T> {

        /// <summary>
        /// The T value held by the gene
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Percentage chance of the gene mutating when replicated
        /// </summary>
        private readonly float mutationChance;

        /// <summary>
        /// Delegate that mutates a gene of the type
        /// Takes the original T as first argument and a T representing the amount T can change as the second argument
        /// and returns a new T
        /// </summary>
        private readonly Func<T, T, T> mutate;

        /// <summary>
        /// The T to be supplied as the second argument in the mutate Func
        /// </summary>
        private readonly T mutateVariance;

        /// <summary>
        /// Constructor for a Gene
        /// </summary>
        /// <param name="value">The value held</param>
        /// <param name="mutationFunc">The function that creates a mutated T</param>
        /// <param name="mutateChance">Percentage chance of the gene mutating when replicated</param>
        /// <param name="variance">A T representing the amount the gene can mutate</param>
        public Gene(T value, Func<T, T, T> mutationFunc, float mutateChance, T variance) {
            Value = value;
            mutationChance = mutateChance;
            mutate = mutationFunc;
            mutateVariance = variance;
        }

        /// <summary>
        /// Static method to create a gene from 2 genes. Picks one of the genes to copy, then checks whether it mutates
        /// </summary>
        /// <param name="firstGene"></param>
        /// <param name="secondGene"></param>
        /// <returns></returns>
        public static Gene<T> CombineGenes(Gene<T> firstGene, Gene<T> secondGene) {

            Gene<T> newGene = (UnityEngine.Random.Range(0, 2) == 0) ? firstGene : secondGene;

            if (UnityEngine.Random.Range(0f, 100f) <= newGene.mutationChance && newGene.mutate != null) {
                newGene = new Gene<T>(newGene.mutate(newGene.Value, newGene.mutateVariance),
                    newGene.mutate, newGene.mutationChance, newGene.mutateVariance);
            }

            return newGene;
        }

    }
}