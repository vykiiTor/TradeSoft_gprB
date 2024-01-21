using System;

//to be removed
public class ObjectEventArgs<T> : EventArgs
{
    public T data { get; }

    public ObjectEventArgs(T data)
    {
        data = data;
    }
}

public abstract class ObjectReceptor<T>
{
    static Semaphore syncObject = new Semaphore(1, 1);
    private List<T> objectList = new List<T>();
    public event EventHandler<ObjectEventArgs<T>> dataReceived;
    public Semaphore GetSyncObject()
    {
        return syncObject;
    }

    public List<T> GetObjectList()
    {
        return objectList;
    }

    public EventHandler<ObjectEventArgs<T>> GetDataReceived()
    {
        return dataReceived;
    }
    public ObjectReceptor()
    {

    }

    public virtual void DataReception(Object sender, ObjectEventArgs<T> e)
    {
        Thread receptor = new Thread(() =>
        {
            syncObject.WaitOne();
            objectList.Add(e.data);
            Console.WriteLine($"Received Object: {e.data.ToString}");
            syncObject.Release();
            OnDataReceived(e.data);
        });
        receptor.Start();
    }

    protected virtual void OnDataReceived(T data)
    {
        dataReceived?.Invoke(this, new ObjectEventArgs<T>(data));
    }

}

public class TicksReceptor : ObjectReceptor<TicksData>
{
    public override void DataReception(Object sender, ObjectEventArgs<TicksData> e)
    {
        Thread market = new Thread(() =>
        {
            this.GetSyncObject().WaitOne();
            //Console.WriteLine($"Received Time: {e.Tick.Time} and received Price : {e.Tick.Price}");
            GetObjectList().Add(e.data);
            //Console.WriteLine(" ticks price : " + getObjectList().Last().Price);
            this.GetSyncObject().Release();
        });
        market.Start();
    }
}
