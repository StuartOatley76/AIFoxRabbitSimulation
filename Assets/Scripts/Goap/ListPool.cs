using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class to provide pools for lists
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListPool<T> {

    /// <summary>
    /// The queue that acts as the pool
    /// </summary>
    private Queue<List<T>> pool = new Queue<List<T>>();

    /// <summary>
    /// The initial size of the pool
    /// </summary>
    private const int startSize = 50;

    /// <summary>
    /// The current size of the pool
    /// </summary>
    private int currentSize = 0;

    /// <summary>
    /// Constructor. Initialises the pool
    /// </summary>
    public ListPool() {
        CreateLists(startSize);
    }

    /// <summary>
    /// Resizes the pool if needed, then provides an empty list
    /// </summary>
    /// <returns></returns>
    public List<T> GetList() {
        if (pool.Count <= 0) {
            CreateLists(currentSize);
        }
        List<T> list = pool.Dequeue();
        list.Clear();
        return list;
    }

    /// <summary>
    /// Returns the list to the pool
    /// </summary>
    /// <param name="list"></param>
    public void ReturnList(List<T> list) {
        pool.Enqueue(list);
    }

    /// <summary>
    /// Adds lists to the pool
    /// </summary>
    /// <param name="numberOfLists"></param>
    private void CreateLists(int numberOfLists) {
        for (int i = 0; i < numberOfLists; i++) {
            pool.Enqueue(new List<T>());
        }
        currentSize += numberOfLists;
    }
}
