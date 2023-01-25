
using UnityEngine;

namespace Genetics {
    public static class Mutations {

        /// <summary>
        /// Provides a mutation for a positive float
        /// </summary>
        /// <param name="original"></param>
        /// <param name="variance"></param>
        /// <returns></returns>
        public static float MutatePositiveFloat(float original, float variance) {
            int safety = 0;
            do {
                safety++;
                float toReturn = Random.Range(original - variance, original + variance);
                if (toReturn >= 0) {
                    return toReturn;
                }
            } while (safety < 10000);

            return 0f;
        }

    }
}