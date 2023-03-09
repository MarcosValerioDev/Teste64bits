using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatternFactory
{
    public abstract class AbstractConstruct
    {
        public abstract GameObject Fabricar(string car);
    }

    public class Fabrica: AbstractConstruct
    {
        public Fabrica(){}

        public List<GameObject> objs = new List<GameObject>();
        public override GameObject Fabricar(string car)
        {
            GameObject obj = null;
            GameObject o = null;
            switch (car)
            {
                case "Car1":
                    obj = Resources.Load<GameObject>(car);
                    o = GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
                    objs.Add(o);
                    obj = null;
                    return (o);
                   
                case "Car2":
                    obj = Resources.Load<GameObject>(car);
                    o = GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
                    objs.Add(o);
                    obj = null;
                    return (o);
                  
                case "Car3":
                    obj = Resources.Load<GameObject>(car);
                    o = GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
                    objs.Add(o);
                    obj = null;
                    return (o);
                   
                default:
                    obj = Resources.Load<GameObject>(car);
                    o = GameObject.Instantiate(obj, obj.transform.position, Quaternion.identity);
                    objs.Add(o);
                    obj = null;
                    return (o);
                   
            }
        }
    }

}
