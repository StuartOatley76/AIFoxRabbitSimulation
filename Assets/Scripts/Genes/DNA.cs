using UnityEngine;

namespace Genetics {

    /// <summary>
    /// Enum to represent sex
    /// </summary>
    public enum Sex {
        male = 0,
        female = 1
    }

    /// <summary>
    /// Class to represent the DNA of a creature holding all their genes
    /// </summary>
    public class DNA {
        /// <summary>
        /// Chance of a gene mutating (percentage)
        /// </summary>
        private const float geneMutationChance = 1f;

        /// <summary>
        /// How much either way the gene can change whilst mutating
        /// </summary>
        private const float variance = 0.3f;

        /// <summary>
        /// minimum value for random creation of a speed gene
        /// </summary>
        private const float minSpeed = 5f;

        /// <summary>
        /// maximum value for random creation of a speed gene
        /// </summary>
        private const float maxSpeed = 8f;

        /// <summary>
        /// multiplier for varience for speed
        /// </summary>
        private float speedVarianceMultiplier = 1f;

        /// <summary>
        /// minimum value for random creation of a hearing or smell gene
        /// </summary>
        private const float minSense = 1f;

        /// <summary>
        /// maximum value for random creation of a hearing or smell gene
        /// </summary>
        private const float maxSense = 2f;

        /// <summary>
        /// multiplier for varience for hearing or smell
        /// </summary>
        private float senseVarianceMultiplier = 100f;
        /// <summary>
        /// Speed gene
        /// </summary>
        private readonly Gene<float> speed;

        /// <summary>
        /// Speed value
        /// </summary>
        public float Speed { get { return speed.Value; } }

        /// <summary>
        /// Sex Gene
        /// </summary>
        private readonly Gene<Sex> sex;

        /// <summary>
        /// Sex value
        /// </summary>
        public Sex Sex { get { return sex.Value; } }

        /// <summary>
        /// Hearing gene
        /// </summary>
        private readonly Gene<float> hearing;

        /// <summary>
        /// How far the character can hear
        /// </summary>
        public float HearingRange { get { return hearing.Value; } }

        /// <summary>
        /// smell gene
        /// </summary>
        private readonly Gene<float> smell;

        /// <summary>
        /// How far the character can smell
        /// </summary>
        public float SmellRange { get { return smell.Value; } }

        /// <summary>
        /// Gene that dictates when character is hungry
        /// </summary>
        private readonly Gene<float> hungerLevel;

        /// <summary>
        /// Level at which character is classed as hungry
        /// </summary>
        public float HungerLevel { get { return hungerLevel.Value; } }

        /// <summary>
        /// minimum value for random creation of a level gene
        /// </summary>
        private const float minLevel = 30f;

        /// <summary>
        /// maximum value for random creation of a level gene
        /// </summary>
        private const float maxLevel = 70f;

        /// <summary>
        /// multiplier for varience for level gene
        /// </summary>
        private const float levelVarianceMultiplier = 100;

        /// <summary>
        /// Gene that dictates when character is thirsty
        /// </summary>
        private Gene<float> thirstLevel;

        /// <summary>
        /// Level at which character is thirsty
        /// </summary>
        public float ThirstLevel { get { return thirstLevel.Value; } }

        /// <summary>
        /// gene that dictates when a character wants to mate
        /// </summary>
        private readonly Gene<float> hornyLevel;

        /// <summary>
        /// Level at which character wants to mate
        /// </summary>
        public float HornyLevel { get { return hornyLevel.Value; } }

        /// <summary>
        /// Gene that dictates when a character wants to rest
        /// </summary>
        private readonly Gene<float> tiredLevel;

        /// <summary>
        /// stamina level at which a character wants to rest
        /// </summary>
        public float TiredLevel { get { return tiredLevel.Value; } }

        /// <summary>
        /// Constructor that creates random genes 
        /// </summary>
        public DNA() {
            speed = new Gene<float>(
                Random.Range(minSpeed, maxSpeed),
                Mutations.MutatePositiveFloat,
                geneMutationChance,
                variance * speedVarianceMultiplier
                );
            sex = new Gene<Sex>(
                (Sex)Random.Range(0, 2),
                null,
                0,
                0
                );
            hearing = new Gene<float>(
                Random.Range(minSense, maxSense),
                Mutations.MutatePositiveFloat,
                geneMutationChance,
                variance * senseVarianceMultiplier
                );
            smell = new Gene<float>(
                Random.Range(minSense, maxSense),
                Mutations.MutatePositiveFloat,
                geneMutationChance,
                variance * senseVarianceMultiplier
                );
            hungerLevel = new Gene<float>(
                Random.Range(minLevel, maxLevel),
                Mutations.MutatePositiveFloat,
                geneMutationChance,
                variance * levelVarianceMultiplier
                );
            thirstLevel = new Gene<float>(
                Random.Range(minLevel, maxLevel),
                Mutations.MutatePositiveFloat,
                geneMutationChance,
                variance * levelVarianceMultiplier
            );
            hornyLevel = new Gene<float>(
                Random.Range(minLevel, maxLevel),
                Mutations.MutatePositiveFloat,
                geneMutationChance,
                variance * levelVarianceMultiplier
            );
            tiredLevel = new Gene<float>(
                Random.Range(minLevel, maxLevel),
                Mutations.MutatePositiveFloat,
                geneMutationChance,
                variance * levelVarianceMultiplier
            );

        }

        /// <summary>
        /// Constructor that creates genes from existing DNA
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        public DNA(DNA parent1, DNA parent2) {
            speed = Gene<float>.CombineGenes(parent1.speed, parent2.speed);
            sex = Gene<Sex>.CombineGenes(parent1.sex, parent2.sex);
            hearing = Gene<float>.CombineGenes(parent1.hearing, parent2.hearing);
            smell = Gene<float>.CombineGenes(parent1.smell, parent2.smell);
            hungerLevel = Gene<float>.CombineGenes(parent1.hungerLevel, parent2.hungerLevel);
            thirstLevel = Gene<float>.CombineGenes(parent1.thirstLevel, parent2.thirstLevel);
            hornyLevel = Gene<float>.CombineGenes(parent1.hornyLevel, parent2.hornyLevel);
            tiredLevel = Gene<float>.CombineGenes(parent1.tiredLevel, parent2.tiredLevel);
        }

    }
}
