using GameSystem;

namespace GameInterfaces
{
    public interface IPoolingPeople
    {
        void EnabledObject();
        void DisableObject();
    }

    public interface IFactoryObject
    {
        void CreateObject(string objectName);
    }

    public interface IPoolingManager
    {
        void AddObject(PoolingPeople poolingPeople);
        void RemoveObject(PoolingPeople poolingPeople);
    }

    public interface IMoneyController 
    {
        void Sum();
        void Sub();
    }

    public interface IBoxMoney
    {
        public void BoxMoneyOn();
        public void BoxMoneyOff();
    }

    public interface IScore
    {
        void SumScore();
    }
}
