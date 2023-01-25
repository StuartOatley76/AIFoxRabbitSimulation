using System.Collections.Generic;

public interface poolable {
    public void Reset();

    public void ReturnToPool();

}
public class Pool<T> where T : poolable, new()
{
    private Queue<T> pool = new Queue<T>();

    private int poolSize = 10;

    public Pool() {
        ResizePool(poolSize);
    }

    private void ResizePool(int increase) {
        for(int i = 0; i < increase; i++) {
            pool.Enqueue(new T());
        }
    }

    public T GetInstance() {
        if(pool.Count == 0) {
            ResizePool(poolSize);
            poolSize += poolSize;
        }
        return pool.Dequeue();
    }

    public void Return(T instance) {
        instance.Reset();
        pool.Enqueue(instance);
    }
}
